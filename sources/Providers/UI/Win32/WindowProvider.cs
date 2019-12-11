// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.User32;
using static TerraFX.UI.Providers.Win32.HelperUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.UI.Providers.Win32
{
    /// <summary>Provides access to a Win32 based window subsystem.</summary>
    [Export(typeof(IWindowProvider))]
    [Shared]
    public sealed unsafe class WindowProvider : IDisposable, IWindowProvider
    {
        /// <summary>A <see cref="HINSTANCE" /> to the entry point module.</summary>
        public static readonly HINSTANCE EntryPointModule = GetModuleHandleW(lpModuleName: null);

        private static readonly NativeDelegate<WNDPROC> s_forwardWndProc = new NativeDelegate<WNDPROC>(ForwardWindowMessage);

        private readonly ThreadLocal<Dictionary<HWND, Window>> _windows;

        private ResettableLazy<ushort> _classAtom;
        private ResettableLazy<GCHandle> _nativeHandle;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="WindowProvider" /> class.</summary>
        [ImportingConstructor]
        public WindowProvider()
        {
            _classAtom = new ResettableLazy<ushort>(CreateClassAtom);
            _nativeHandle = new ResettableLazy<GCHandle>(CreateNativeHandle);

            _windows = new ThreadLocal<Dictionary<HWND, Window>>(trackAllValues: true);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="WindowProvider" /> class.</summary>
        ~WindowProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <c>ATOM</c> of the <see cref="WNDCLASSEXW" /> registered for the instance.</summary>
        public ushort ClassAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _classAtom.Value;
            }
        }

        /// <inheritdoc />
        public IDispatchProvider DispatchProvider => Win32.DispatchProvider.Instance;

        /// <summary>Gets the <see cref="GCHandle" /> containing the native handle for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public GCHandle NativeHandle
        {
            get
            {
                _state.AssertNotDisposedOrDisposing();
                return _nativeHandle.Value;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IEnumerable<IWindow> WindowsForCurrentThread
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windows.Value?.Values ?? Enumerable.Empty<Window>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IWindow CreateWindow()
        {
            _state.ThrowIfDisposedOrDisposing();

            var windows = _windows.Value;

            if (windows is null)
            {
                windows = new Dictionary<HWND, Window>(capacity: 4);
                _windows.Value = windows;
            }

            var window = new Window(this);
            _ = windows.TryAdd(window.Handle, window);

            return window;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private static IntPtr ForwardWindowMessage(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam)
        {
            return Impl(hWnd, msg, wParam, lParam);

            static IntPtr Impl(HWND hWnd, uint msg, UIntPtr wParam, IntPtr lParam)
            {
                IntPtr result, userData;

                if (msg == WM_CREATE)
                {
                    // We allow the WM_CREATE message to be forwarded to the Window instance
                    // for hWnd. This allows some delayed initialization to occur since most
                    // of the fields in Window are lazy.

                    var createStruct = (CREATESTRUCTW*)lParam;
                    userData = (IntPtr)createStruct->lpCreateParams;
                    _ = SetWindowLongPtrW(hWnd, GWLP_USERDATA, userData);
                }
                else
                {
                    userData = GetWindowLongPtrW(hWnd, GWLP_USERDATA);
                }

                WindowProvider windowProvider = null!;
                Dictionary<HWND, Window>? windows = null;
                var forwardMessage = false;
                Window? window = null;

                if (userData != IntPtr.Zero)
                {
                    windowProvider = (WindowProvider)GCHandle.FromIntPtr(userData).Target!;
                    windows = windowProvider._windows.Value;
                    forwardMessage = (windows?.TryGetValue(hWnd, out window)).GetValueOrDefault();
                }

                if (forwardMessage)
                {
                    Assert(windows != null, Resources.ArgumentNullExceptionMessage, nameof(windows));
                    Assert(window != null, Resources.ArgumentNullExceptionMessage, nameof(window));

                    result = window.ProcessWindowMessage(msg, wParam, lParam);

                    if (msg == WM_DESTROY)
                    {
                        // We forward the WM_DESTROY message to the corresponding Window instance
                        // so that it can still be properly disposed of in the scenario that the
                        // hWnd was destroyed externally.

                        _ = RemoveWindow(windows, hWnd);
                    }
                }
                else
                {
                    result = DefWindowProcW(hWnd, msg, wParam, lParam);
                }

                return result;
            }
        }

        private static Window RemoveWindow(Dictionary<HWND, Window> windows, HWND hWnd)
        {
            _ = windows.Remove(hWnd, out var window);
            Assert(window != null, Resources.ArgumentNullExceptionMessage, nameof(window));

            if (windows.Count == 0)
            {
                PostQuitMessage(nExitCode: 0);
            }

            return window;
        }

        private static HICON GetDesktopCursor()
        {
            var desktopWindowHandle = GetDesktopWindow();

            var desktopClassName = stackalloc ushort[256]; // 256 is the maximum length of WNDCLASSEX.lpszClassName
            ThrowExternalExceptionIfZero(nameof(GetClassNameW), GetClassNameW(desktopWindowHandle, desktopClassName, 256));

            WNDCLASSEXW desktopWindowClass;

            ThrowExternalExceptionIfFalse(nameof(GetClassInfoExW), GetClassInfoExW(
                HINSTANCE.NULL,
                lpszClass: desktopClassName,
                lpwcx: &desktopWindowClass
            ));

            return desktopWindowClass.hCursor;
        }

        private ushort CreateClassAtom()
        {
            _state.AssertNotDisposedOrDisposing();

            ushort classAtom;
            {
                // lpszClassName should be less than 256 characters (this includes the null terminator)
                // Currently, we are well below this limit and should be hitting 74 characters + the null terminator

                var className = $"{GetType().FullName}.{EntryPointModule:X16}.{GetHashCode():X8}";
                Assert(className.Length < byte.MaxValue, Resources.ArgumentOutOfRangeExceptionMessage, nameof(className), className);

                fixed (char* lpszClassName = className)
                {
                    var wndClassEx = new WNDCLASSEXW {
                        cbSize = SizeOf<WNDCLASSEXW>(),
                        style = CS_VREDRAW | CS_HREDRAW | CS_DBLCLKS,
                        lpfnWndProc = s_forwardWndProc,
                        cbClsExtra = 0,
                        cbWndExtra = 0,
                        hInstance = EntryPointModule,
                        hIcon = HICON.NULL,
                        hCursor = GetDesktopCursor(),
                        hbrBackground = (IntPtr)(COLOR_WINDOW + 1),
                        lpszMenuName = null,
                        lpszClassName = (ushort*)lpszClassName,
                        hIconSm = HICON.NULL
                    };

                    classAtom = RegisterClassExW(&wndClassEx);
                }
                ThrowExternalExceptionIfZero(nameof(RegisterClassExW), classAtom);
            }
            return classAtom;
        }

        private GCHandle CreateNativeHandle() => GCHandle.Alloc(this, GCHandleType.Normal);

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeWindows(isDisposing);
                DisposeClassAtom();
                DisposeNativeHandle();
            }

            _state.EndDispose();
        }

        private void DisposeClassAtom()
        {
            _state.AssertDisposing();

            if (_classAtom.IsCreated)
            {
                ThrowExternalExceptionIfFalse(nameof(UnregisterClassW), UnregisterClassW((ushort*)_classAtom.Value, EntryPointModule));
            }
        }

        private void DisposeNativeHandle()
        {
            _state.AssertDisposing();

            if (_nativeHandle.IsCreated)
            {
                _nativeHandle.Value.Free();
            }
        }

        private void DisposeWindows(bool isDisposing)
        {
            _state.AssertDisposing();

            if (isDisposing)
            {
                var threadWindows = _windows.Values;

                for (var i = 0; i < threadWindows.Count; i++)
                {
                    var windows = threadWindows[i];

                    if (windows != null)
                    {
                        var hWnds = windows.Keys;

                        foreach (var hWnd in hWnds)
                        {
                            var dispatchProvider = Win32.DispatchProvider.Instance;
                            var window = RemoveWindow(windows, hWnd);
                            window.Dispose();
                        }

                        Assert(windows.Count == 0, Resources.ArgumentOutOfRangeExceptionMessage, nameof(windows.Count), windows.Count);
                    }
                }

                _windows.Dispose();
            }
        }
    }
}
