// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.Windows.COLOR;
using static TerraFX.Interop.Windows.CS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.Win32Utilities;

namespace TerraFX.UI;

/// <summary>Provides access to a Win32 based window subsystem.</summary>
public sealed unsafe class Win32UIService : UIService
{
    private const string VulkanRequiredExtensionNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.RequiredExtensionNames";

    /// <summary>A <c>HINSTANCE</c> to the entry point module.</summary>
    public static readonly HINSTANCE EntryPointModule = GetModuleHandleW(lpModuleName: null);

    /// <summary>Gets the raw tick frequency of the underlying hardware timer used by <see cref="CurrentTimestamp" />.</summary>
    public static readonly double TickFrequency = GetTickFrequency();

    private static Win32UIService? s_instance;

    /// <summary>Gets the Win32 based UI service.</summary>
    public static Win32UIService Instance
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

                var service = new Win32UIService();
                instance = Interlocked.CompareExchange(ref s_instance, service, null) ?? service;
            }

            AssertNotNull(instance);
            return instance;
        }
    }

    private readonly ushort _classAtom;
    private readonly Dictionary<Thread, Win32UIDispatcher> _dispatchers;
    private readonly ValueReaderWriterLock _dispatchersLock;

    private VolatileState _state;

    private Win32UIService()
    {
        var vulkanRequiredExtensionNamesDataName = AppContext.GetData(VulkanRequiredExtensionNamesDataName) as string;
        vulkanRequiredExtensionNamesDataName += ";VK_KHR_surface;VK_KHR_win32_surface";
        AppDomain.CurrentDomain.SetData(VulkanRequiredExtensionNamesDataName, vulkanRequiredExtensionNamesDataName);

        _classAtom = CreateClassAtom();
        _dispatchers = new Dictionary<Thread, Win32UIDispatcher>();
        _dispatchersLock = new ValueReaderWriterLock();

        _ = _state.Transition(to: Initialized);

        static ushort CreateClassAtom()
        {
            ushort classAtom;

            fixed (char* lpszClassName = $"{nameof(Win32UIService)}.X{EntryPointModule:X16}")
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
                    lpszClassName = (ushort*)lpszClassName,
                    hIconSm = HICON.NULL
                };

                ThrowForLastErrorIfZero(classAtom = RegisterClassExW(&wndClassEx));
            }

            return classAtom;
        }

        static HCURSOR GetDesktopCursor()
        {
            var desktopWindowHandle = GetDesktopWindow();

            var desktopClassName = stackalloc ushort[256]; // 256 is the maximum length of WNDCLASSEX.lpszClassName
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

    /// <summary>Finalizes an instance of the <see cref="Win32UIService" /> class.</summary>
    ~Win32UIService() => Dispose(isDisposing: false);

    /// <summary>Gets the <c>ATOM</c> of the <see cref="WNDCLASSEXW" /> registered for the service.</summary>
    public ushort ClassAtom
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _classAtom;
        }
    }

    /// <inheritdoc />
    public override Timestamp CurrentTimestamp
    {
        get
        {
            LARGE_INTEGER performanceCount;
            ThrowExternalExceptionIfFalse(QueryPerformanceCounter(&performanceCount), nameof(QueryPerformanceCounter));

            var ticks = (long)(performanceCount.QuadPart * TickFrequency);
            return new Timestamp(ticks);
        }
    }

    /// <inheritdoc />
    public override Win32UIDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

    [UnmanagedCallersOnly]
    private static LRESULT ForwardWindowMessage(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam)
    {
        LRESULT result;

        if (!Instance.DispatcherForCurrentThread.TryGetWindow(hWnd, out var window) && (msg == WM_CREATE))
        {
            // We allow the WM_CREATE message to be forwarded to the Window instance
            // for hWnd. This allows some most of the state to be initialized here.

            var createStruct = (CREATESTRUCTW*)lParam;
            var userData = (nint)createStruct->lpCreateParams;

            var gcHandle = GCHandle.FromIntPtr(userData);
            {
                window = gcHandle.Target as Win32Window;
                AssertNotNull(window);
                window.Dispatcher.AddWindow(hWnd, window);
            }
            gcHandle.Free();
        }

        if (window is not null)
        {
            result = window.ProcessWindowMessage(msg, wParam, lParam);
        }
        else
        {
            result = DefWindowProcW(hWnd, msg, wParam, lParam);
        }

        return result;
    }

    private static double GetTickFrequency()
    {
        LARGE_INTEGER frequency;
        ThrowExternalExceptionIfFalse(QueryPerformanceFrequency(&frequency), nameof(QueryPerformanceFrequency));

        const double TicksPerSecond = Timestamp.TicksPerSecond;
        return TicksPerSecond / frequency.QuadPart;
    }

    /// <inheritdoc />
    public override Win32UIDispatcher GetDispatcher(Thread thread)
    {
        if (!TryGetDispatcher(thread, out var dispatcher))
        {
            dispatcher = CreateDispatcher(this, thread);
        }
        return dispatcher;

        static Win32UIDispatcher CreateDispatcher(Win32UIService service, Thread thread)
        {
            var dispatcher = new Win32UIDispatcher(service, thread);

            using var writerLock = new DisposableWriterLock(service._dispatchersLock, isExternallySynchronized: false);
            service._dispatchers.Add(thread, dispatcher);

            return dispatcher;
        }
    }

    /// <inheritdoc />
    public override bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out UIDispatcher dispatcher)
    {
        var result = TryGetDispatcher(thread, out var win32Dispatcher);
        dispatcher = win32Dispatcher;
        return result;
    }

    /// <inheritdoc cref="TryGetDispatcher(Thread, out UIDispatcher)" />
    public bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out Win32UIDispatcher dispatcher)
    {
        ThrowIfNull(thread);
        using var readerLock = new DisposableReaderLock(_dispatchersLock, isExternallySynchronized: false);
        return _dispatchers.TryGetValue(thread, out dispatcher);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                DisposeDispatchers(_dispatchers);
            }
            DisposeClassAtom(_classAtom);
        }

        _state.EndDispose();

        static void DisposeClassAtom(ushort classAtom)
        {
            if (classAtom != 0)
            {
                ThrowExternalExceptionIfFalse(UnregisterClassW((ushort*)classAtom, EntryPointModule), nameof(UnregisterClassW));
            }
        }

        static void DisposeDispatchers(Dictionary<Thread, Win32UIDispatcher> dispatchers)
        {
            foreach (var dispatcher in dispatchers)
            {
                dispatcher.Value.Dispose();
            }
            dispatchers.Clear();
        }
    }
}
