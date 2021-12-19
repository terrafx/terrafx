// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.Windows.PM;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.UI;

/// <inheritdoc />
public sealed unsafe class Win32UIDispatcher : UIDispatcher
{
    private readonly Dictionary<HWND, Win32Window> _windows;
    private readonly ValueReaderWriterLock _windowsLock;

    private VolatileState _state;

    internal Win32UIDispatcher(Win32UIService service, Thread parentThread)
        : base(service, parentThread)
    {
        _windows = new Dictionary<HWND, Win32Window>();
        _windowsLock = new ValueReaderWriterLock();

        _ = _state.Transition(to: Initialized);
    }

    /// <inheritdoc cref="UIServiceObject.Service" />
    public new Win32UIService Service => base.Service.As<Win32UIService>();

    /// <inheritdoc />
    public override IEnumerable<Win32Window> Windows
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _windows.Values;
        }
    }

    /// <inheritdoc />
    public override Win32Window CreateWindow()
    {
        ThrowIfNotThread(ParentThread);
        ThrowIfDisposedOrDisposing(_state, nameof(Win32UIDispatcher));
        return new Win32Window(this);
    }

    /// <inheritdoc />
    public override void DispatchPending()
    {
        ThrowIfNotThread(ParentThread);
        ThrowIfDisposedOrDisposing(_state, nameof(Win32UIDispatcher));

        MSG msg;

        while (PeekMessageW(&msg, HWND.NULL, wMsgFilterMin: WM_NULL, wMsgFilterMax: WM_NULL, wRemoveMsg: PM_REMOVE) != FALSE)
        {
            if (msg.message != WM_QUIT)
            {
                _ = DispatchMessageW(&msg);
            }
            else
            {
                RaiseExitRequested();
            }
        }
    }

    /// <inheritdoc />
    public override void RequestExit()
    {
        ThrowIfNotThread(ParentThread);
        ThrowIfDisposedOrDisposing(_state, nameof(Win32UIDispatcher));

        PostQuitMessage(0);
    }

    /// <summary>Tries to get the window associated with a window handle.</summary>
    /// <param name="hWnd">The window handle for which to get the associated window.</param>
    /// <param name="window">On return, contains the window associated with <paramref name="hWnd" /> or <c>null</c> if no such window exists.</param>
    /// <returns><c>true</c> if a window associated with <paramref name="hWnd" /> was succesfully retrieved; otherwise, <c>false</c>.</returns>
    public bool TryGetWindow(HWND hWnd, [NotNullWhen(true)] out Win32Window? window)
    {
        using var readerLock = new DisposableReaderLock(_windowsLock, isExternallySynchronized: false);
        return _windows.TryGetValue(hWnd, out window);
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
        }

        _state.EndDispose();

        static void DisposeWindows(Dictionary<HWND, Win32Window> windows)
        {
            foreach (var window in windows)
            {
                window.Value.Dispose();
            }
            windows.Clear();
        }
    }

    internal void AddWindow(HWND hWnd, Win32Window window)
    {
        using var writerLock = new DisposableWriterLock(_windowsLock, isExternallySynchronized: false);
        _windows.Add(hWnd, window);
    }

    internal bool RemoveWindow(HWND hWnd)
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
