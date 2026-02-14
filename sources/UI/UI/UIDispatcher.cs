// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.Windows.PM;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI;

/// <summary>Provides a means of dispatching events for a thread.</summary>
public sealed unsafe class UIDispatcher : IDisposable, INameable
{
    private readonly UIService _service;
    private readonly Thread _parentThread;

    private readonly Dictionary<HWND, UIWindow> _windows;
    private readonly ValueReaderWriterLock _windowsLock;

    private string _name;
    private VolatileState _state;

    internal UIDispatcher(UIService service, Thread parentThread)
    {
        AssertNotNull(service);
        ThrowIfNull(parentThread);

        _service = service;
        _parentThread = parentThread;

        _windows = [];
        _windowsLock = new ValueReaderWriterLock();

        _name = GetType().Name;
        _ = _state.Transition(VolatileState.Initialized);
    }

    /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
    public event EventHandler? ExitRequested;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
        }
    }

    /// <summary>Gets the thread that was used to create the dispatcher.</summary>
    public Thread ParentThread => _parentThread;

    /// <summary>Gets the service for which the object was created.</summary>
    public UIService Service => _service;

    /// <summary>Gets the windows created for the dispatcher.</summary>
    public IEnumerable<UIWindow> Windows
    {
        get
        {
            ThrowIfDisposedOrDisposing(_state, _name);
            return _windows.Values;
        }
    }

    /// <summary>Create a new window which utilizes the dispatcher.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="ObjectDisposedException">The dispatcher has been disposed.</exception>
    public UIWindow CreateWindow()
    {
        ThrowIfDisposedOrDisposing(_state, _name);
        ThrowIfNotThread(ParentThread);
        return CreateWindowUnsafe();
    }

    /// <summary>Dispatches all events currently pending in the queue.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="ObjectDisposedException">The dispatcher has been disposed.</exception>
    /// <remarks>
    ///   <para>This method does not wait for a new event to be raised if the queue is empty.</para>
    ///   <para>This method does not performing any translation or pre-processing on the dispatched events.</para>
    ///   <para>This method will continue dispatching pending events even after the <see cref="ExitRequested" /> event is raised.</para>
    /// </remarks>
    public void DispatchPending()
    {
        ThrowIfDisposedOrDisposing(_state, _name);
        ThrowIfNotThread(ParentThread);
        DispatchPendingUnsafe();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <summary>Requests that the dispatcher exit by posting the appropriate event to the dispatch queue.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="ObjectDisposedException">The dispatcher has been disposed.</exception>
    public void RequestExit()
    {
        ThrowIfDisposedOrDisposing(_state, _name);
        ThrowIfNotThread(ParentThread);
        RequestExitUnsafe();
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    /// <summary>Tries to get the window associated with a window handle.</summary>
    /// <param name="hWnd">The window handle for which to get the associated window.</param>
    /// <param name="window">On return, contains the window associated with <paramref name="hWnd" /> or <c>null</c> if no such window exists.</param>
    /// <returns><c>true</c> if a window associated with <paramref name="hWnd" /> was successfully retrieved; otherwise, <c>false</c>.</returns>
    public bool TryGetWindow(HWND hWnd, [NotNullWhen(true)] out UIWindow? window)
    {
        using var readerLock = new DisposableReaderLock(_windowsLock, isExternallySynchronized: false);
        return _windows.TryGetValue(hWnd, out window);
    }

    internal void AddWindow(HWND hWnd, UIWindow window)
    {
        using var writerLock = new DisposableWriterLock(_windowsLock, isExternallySynchronized: false);
        _windows.Add(hWnd, window);
    }

    internal bool RemoveWindow(HWND hWnd)
    {
        using var writerLock = new DisposableWriterLock(_windowsLock, isExternallySynchronized: false);
        return RemoveWindowNoLock(hWnd);

        bool RemoveWindowNoLock(HWND hWnd)
        {
            var result = _windows.Remove(hWnd);

            if (_windows.Count == 0)
            {
                RaiseExitRequested();
            }
            return result;
        }
    }

    private UIWindow CreateWindowUnsafe() => new UIWindow(this);

    private void DispatchPendingUnsafe()
    {
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

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            DisposeWindows(_windows);
        }

        static void DisposeWindows(Dictionary<HWND, UIWindow> windows)
        {
            foreach (var window in windows)
            {
                window.Value.Dispose();
            }
            windows.Clear();
        }
    }

    private void RaiseExitRequested()
    {
        AssertThread(ParentThread);
        ExitRequested?.Invoke(this, EventArgs.Empty);
    }

    private static void RequestExitUnsafe() => PostQuitMessage(0);
}
