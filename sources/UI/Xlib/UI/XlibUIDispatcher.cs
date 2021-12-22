// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop.Xlib;
using TerraFX.Threading;
using static TerraFX.Interop.Xlib.Xlib;
using static TerraFX.Threading.VolatileState;
using static TerraFX.UI.XlibAtomId;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.XlibUtilities;
using XWindow = TerraFX.Interop.Xlib.Window;

namespace TerraFX.UI;

/// <summary>Provides a means of dispatching events for a thread.</summary>
public sealed unsafe partial class XlibUIDispatcher : UIDispatcher
{
    private readonly PredicateData* _predicateData;
    private readonly Dictionary<XWindow, XlibWindow> _windows;
    private readonly ValueReaderWriterLock _windowsLock;

    private VolatileState _state;

    internal XlibUIDispatcher(XlibUIService service, Thread parentThread)
        : base(service, parentThread)
    {
        _predicateData = CreatePredicateData(this, service); 
        _windows = new Dictionary<XWindow, XlibWindow>();
        _windowsLock = new ValueReaderWriterLock();

        _ = _state.Transition(to: Initialized);

        static PredicateData* CreatePredicateData(XlibUIDispatcher dispatcher, XlibUIService service)
        {
            var predicateData = Allocate<PredicateData>();

            predicateData->GCHandle = GCHandle.Alloc(dispatcher, GCHandleType.Normal);
            predicateData->IsClientMessage = false;
            predicateData->TerraFXCreateWindowAtom = service.GetAtom(_TERRAFX_CREATE_WINDOW);
            predicateData->TerraFXQuitAtom = service.GetAtom(_TERRAFX_QUIT);

            return predicateData;
        }
    }

    /// <inheritdoc cref="UIServiceObject.Service" />
    public new XlibUIService Service => base.Service.As<XlibUIService>();

    /// <inheritdoc />
    public override IEnumerable<XlibWindow> Windows
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _windows.Values;
        }
    }

    [UnmanagedCallersOnly]
    private static int XEventIsForDispatcher(Display* display, XEvent* xevent, sbyte* arg)
    {
        var predicateData = (PredicateData*)arg;

        var dispatcher = predicateData->GCHandle.Target.As<XlibUIDispatcher>();
        AssertNotNull(dispatcher);

        var isClientMessage = (xevent->type == ClientMessage) && (xevent->xclient.format == 32);

        if (!dispatcher.TryGetWindow(xevent->xany.window, out var window) && isClientMessage)
        {
            var userData = Environment.Is64BitProcess
                         ? (xevent->xclient.data.l[1] << 32) | unchecked((nint)(uint)xevent->xclient.data.l[0])
                         : xevent->xclient.data.l[0];

            if (xevent->xclient.message_type == predicateData->TerraFXCreateWindowAtom)
            {
                var gcHandle = GCHandle.FromIntPtr(userData);
                {
                    window = gcHandle.Target as XlibWindow;
                    AssertNotNull(window);
                    window.Dispatcher.AddWindow(xevent->xany.window, window);
                }
                gcHandle.Free();
            }
            else if ((xevent->xclient.message_type == predicateData->TerraFXQuitAtom) && (userData == GCHandle.ToIntPtr(predicateData->GCHandle)))
            {
                dispatcher.RaiseExitRequested();
            }
        }

        predicateData->IsClientMessage = isClientMessage;
        return (window is not null) ? True : False;
    }

    /// <inheritdoc />
    public override XlibWindow CreateWindow()
    {
        ThrowIfNotThread(ParentThread);
        ThrowIfDisposedOrDisposing(_state, nameof(XlibUIDispatcher));
        return new XlibWindow(this);
    }

    /// <inheritdoc />
    public override void DispatchPending()
    {
        ThrowIfNotThread(ParentThread);
        ThrowIfDisposedOrDisposing(_state, nameof(XlibUIDispatcher));

        var display = Service.Display;
        XLockDisplay(display);

        try
        {
            DispatchPendingInternal(this, display);
        }
        finally
        {
            XUnlockDisplay(display);
        }

        static void DispatchPendingInternal(XlibUIDispatcher dispatcher, Display* display)
        {
            XEvent xevent;

            while (XCheckIfEvent(display, &xevent, &XEventIsForDispatcher, (sbyte*)dispatcher._predicateData) != 0)
            {
                _ = dispatcher.TryGetWindow(xevent.xany.window, out var window);
                AssertNotNull(window);
                window.ProcessWindowEvent(&xevent);
            }
        }
    }

    /// <inheritdoc />
    public override void RequestExit()
    {
        ThrowIfNotThread(ParentThread);
        ThrowIfDisposedOrDisposing(_state, nameof(XlibUIDispatcher));

        var service = Service;
        var gcHandle = (nuint)(nint)GCHandle.ToIntPtr(_predicateData->GCHandle);

        SendClientMessage(
            service.Display,
            XWindow.NULL,
            NoEventMask,
            XWindow.NULL,
            service.GetAtom(_TERRAFX_QUIT),
            unchecked((nint)(uint)gcHandle),
            (nint)(gcHandle >> 32)
        );
    }

    /// <summary>Tries to get the window associated with a window handle.</summary>
    /// <param name="xwindow">The window handle for which to get the associated window.</param>
    /// <param name="window">On return, contains the window associated with <paramref name="xwindow" /> or <c>null</c> if no such window exists.</param>
    /// <returns><c>true</c> if a window associated with <paramref name="xwindow" /> was succesfully retrieved; otherwise, <c>false</c>.</returns>
    public bool TryGetWindow(XWindow xwindow, [NotNullWhen(true)] out XlibWindow? window)
    {
        using var readerLock = new DisposableReaderLock(_windowsLock, isExternallySynchronized: false);
        return _windows.TryGetValue(xwindow, out window);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                DisposeWindows(_windows);
            }
            DisposePredicateData(_predicateData);
        }

        _state.EndDispose();

        static void DisposePredicateData(PredicateData* predicateData)
        {
            predicateData->GCHandle.Free();
            Free(predicateData);
        }

        static void DisposeWindows(Dictionary<XWindow, XlibWindow> windows)
        {
            foreach (var window in windows)
            {
                window.Value.Dispose();
            }
            windows.Clear();
        }
    }

    internal void AddWindow(XWindow hWnd, XlibWindow window)
    {
        using var writerLock = new DisposableWriterLock(_windowsLock, isExternallySynchronized: false);
        _windows.Add(hWnd, window);
    }

    internal bool RemoveWindow(XWindow hWnd)
    {
        using var writerLock = new DisposableWriterLock(_windowsLock, isExternallySynchronized: false);
        var result = _windows.Remove(hWnd);

        if (_windows.Count == 0)
        {
            RaiseExitRequested();
        }
        return result;
    }
}
