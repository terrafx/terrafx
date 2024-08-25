// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Advanced;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.Windows.COLOR;
using static TerraFX.Interop.Windows.CS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.Win32Utilities;

namespace TerraFX.UI;

/// <summary>Provides access to a UI subsystem.</summary>
public sealed unsafe class UIService : DisposableObject
{
    /// <summary>Gets the raw tick frequency of the underlying hardware timer used by <see cref="CurrentTimestamp" />.</summary>
    public static double TickFrequency { get; } = GetTickFrequency();

    internal static HINSTANCE EntryPointModule { get; } = GetModuleHandleW(lpModuleName: null);

    private static UIService? s_instance;

    private readonly ushort _classAtom;
    private readonly Dictionary<Thread, UIDispatcher> _dispatchers;
    private readonly ValueReaderWriterLock _dispatchersLock;

    private UIService() : base(name: null)
    {
        _classAtom = CreateClassAtom();

        _dispatchers = [];
        _dispatchersLock = new ValueReaderWriterLock();

        static ushort CreateClassAtom()
        {
            ushort classAtom;

            fixed (char* className = $"{nameof(UIService)}.X{EntryPointModule:X16}")
            {
                var wndClassEx = new WNDCLASSEXW {
                    cbSize = SizeOf<WNDCLASSEXW>(),
                    style = CS_VREDRAW | CS_HREDRAW | CS_DBLCLKS,
                    lpfnWndProc = &ForwardWindowMessage,
                    cbClsExtra = 0,
                    cbWndExtra = 0,
                    hInstance = EntryPointModule,
                    hIcon = HICON.NULL,
                    hCursor = GetDesktopCursor(),
                    hbrBackground = (HBRUSH)(COLOR_WINDOW + 1),
                    lpszMenuName = null,
                    lpszClassName = className,
                    hIconSm = HICON.NULL
                };

                ThrowForLastErrorIfZero(classAtom = RegisterClassExW(&wndClassEx));
            }

            return classAtom;
        }

        static HCURSOR GetDesktopCursor()
        {
            var desktopWindowHandle = GetDesktopWindow();

            var desktopClassName = stackalloc char[256]; // 256 is the maximum length of a WNDCLASSEX class name
            ThrowForLastErrorIfZero(GetClassNameW(desktopWindowHandle, desktopClassName, 256));

            WNDCLASSEXW desktopWindowClass;

            ThrowExternalExceptionIfFalse(GetClassInfoExW(
                HINSTANCE.NULL,
                lpszClass: desktopClassName,
                lpwcx: &desktopWindowClass
            ), nameof(GetClassInfoExW));

            return desktopWindowClass.hCursor;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="UIService" /> class.</summary>
    ~UIService() => Dispose(isDisposing: false);

    /// <summary>Gets the UI service.</summary>
    public static UIService Instance
    {
        get
        {
            var instance = s_instance;

            if (instance is null)
            {
                // This might unnecessarily create a service in the case of a race
                // between two threads, but that is an extremely exceptional case
                // given the typical usage pattern and is acceptable to keep this
                // simple.

                var service = new UIService();
                instance = Interlocked.CompareExchange(ref s_instance, service, null) ?? service;
            }

            AssertNotNull(instance);
            return instance;
        }
    }

    /// <summary>Gets the current timestamp for the service.</summary>
    public Timestamp CurrentTimestamp
    {
        get
        {
            LARGE_INTEGER performanceCount;
            ThrowExternalExceptionIfFalse(QueryPerformanceCounter(&performanceCount), nameof(QueryPerformanceCounter));

            var ticks = (long)(performanceCount.QuadPart * TickFrequency);
            return new Timestamp(ticks);
        }
    }

    /// <summary>Gets the dispatcher associated with <see cref="Thread.CurrentThread" />.</summary>
    /// <returns>The dispatcher associated with <see cref="Thread.CurrentThread" />.</returns>
    /// <remarks>This will create a new dispatcher if one does not already exist.</remarks>
    public UIDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

    internal ushort ClassAtom
    {
        get
        {
            ThrowIfDisposed();
            return _classAtom;
        }
    }

    [UnmanagedCallersOnly]
    private static LRESULT ForwardWindowMessage(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam)
    {
        if (!Instance.DispatcherForCurrentThread.TryGetWindow(hWnd, out var window) && (msg == WM_CREATE))
        {
            // We allow the WM_CREATE message to be forwarded to the Window instance
            // for hWnd. This allows some most of the state to be initialized here.

            var createStruct = (CREATESTRUCTW*)lParam;
            var userData = (nint)createStruct->lpCreateParams;

            var gcHandle = GCHandle.FromIntPtr(userData);
            {
                window = gcHandle.Target as UIWindow;
                AssertNotNull(window);
                window.Dispatcher.AddWindow(hWnd, window);
            }
            gcHandle.Free();
        }

        return (window is not null) ? window.ProcessWindowMessage(msg, wParam, lParam) : DefWindowProcW(hWnd, msg, wParam, lParam);
    }

    private static double GetTickFrequency()
    {
        LARGE_INTEGER frequency;
        ThrowExternalExceptionIfFalse(QueryPerformanceFrequency(&frequency), nameof(QueryPerformanceFrequency));

        const double TicksPerSecond = Timestamp.TicksPerSecond;
        return TicksPerSecond / frequency.QuadPart;
    }

    /// <summary>Gets the dispatcher associated with a thread, creating one if it does not exist.</summary>
    /// <param name="thread">The thread for which the dispatcher should be retrieved.</param>
    /// <returns>The dispatcher associated with <paramref name="thread" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
    public UIDispatcher GetDispatcher(Thread thread)
    {
        if (!TryGetDispatcher(thread, out var dispatcher))
        {
            using var writerLock = new DisposableWriterLock(_dispatchersLock, isExternallySynchronized: false);
            dispatcher = CreateDispatcherNoLock(this, thread);
        }
        return dispatcher;

        static UIDispatcher CreateDispatcherNoLock(UIService service, Thread thread)
        {
            var dispatcher = new UIDispatcher(service, thread);
            service._dispatchers.Add(thread, dispatcher);
            return dispatcher;
        }
    }

    /// <summary>Gets the dispatcher associated with a thread or <c>null</c> if one does not exist.</summary>
    /// <param name="thread">The thread for which the dispatcher should be retrieved.</param>
    /// <param name="dispatcher">The dispatcher associated with <paramref name="thread" />.</param>
    /// <returns><c>true</c> if a dispatcher was found for <paramref name="thread" />; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
    public bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out UIDispatcher dispatcher)
    {
        ThrowIfNull(thread);
        using var readerLock = new DisposableReaderLock(_dispatchersLock, isExternallySynchronized: false);
        return _dispatchers.TryGetValue(thread, out dispatcher);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            DisposeDispatchers(_dispatchers);
        }
        DisposeClassAtom(_classAtom);

        static void DisposeClassAtom(ushort classAtom)
        {
            if (classAtom != 0)
            {
                ThrowExternalExceptionIfFalse(UnregisterClassW((char*)classAtom, EntryPointModule), nameof(UnregisterClassW));
            }
        }

        static void DisposeDispatchers(Dictionary<Thread, UIDispatcher> dispatchers)
        {
            foreach (var dispatcher in dispatchers)
            {
                dispatcher.Value.Dispose();
            }
            dispatchers.Clear();
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }
}
