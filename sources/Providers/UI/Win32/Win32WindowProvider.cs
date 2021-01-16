// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Interop.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.UI.Providers.Win32.HelperUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.UI.Providers.Win32
{
    /// <summary>Provides access to a Win32 based window subsystem.</summary>
    [Export(typeof(WindowProvider))]
    [Shared]
    public sealed unsafe class Win32WindowProvider : WindowProvider
    {
        private const string VulkanRequiredExtensionNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.RequiredExtensionNames";

        /// <summary>A <c>HINSTANCE</c> to the entry point module.</summary>
        public static readonly IntPtr EntryPointModule = GetModuleHandleW(lpModuleName: null);

        private readonly ThreadLocal<Dictionary<IntPtr, Win32Window>> _windows;

        private ValueLazy<ushort> _classAtom;
        private ValueLazy<GCHandle> _nativeHandle;

        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="Win32WindowProvider" /> class.</summary>
        [ImportingConstructor]
        public Win32WindowProvider()
        {
            var vulkanRequiredExtensionNamesDataName = AppContext.GetData(VulkanRequiredExtensionNamesDataName) as string;
            vulkanRequiredExtensionNamesDataName += ";VK_KHR_surface;VK_KHR_win32_surface";
            AppDomain.CurrentDomain.SetData(VulkanRequiredExtensionNamesDataName, vulkanRequiredExtensionNamesDataName);

            _classAtom = new ValueLazy<ushort>(CreateClassAtom);
            _nativeHandle = new ValueLazy<GCHandle>(CreateNativeHandle);

            _windows = new ThreadLocal<Dictionary<IntPtr, Win32Window>>(trackAllValues: true);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="Win32WindowProvider" /> class.</summary>
        ~Win32WindowProvider() => Dispose(isDisposing: false);

        /// <summary>Gets the <c>ATOM</c> of the <see cref="WNDCLASSEXW" /> registered for the instance.</summary>
        public ushort ClassAtom
        {
            get
            {
                ThrowIfDisposedOrDisposing(_state, nameof(Win32WindowProvider));
                return _classAtom.Value;
            }
        }

        /// <inheritdoc />
        public override DispatchProvider DispatchProvider => Win32DispatchProvider.Instance;

        /// <summary>Gets the <see cref="GCHandle" /> containing the native handle for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public GCHandle NativeHandle
        {
            get
            {
                AssertNotDisposedOrDisposing(_state);
                return _nativeHandle.Value;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override IEnumerable<Window> WindowsForCurrentThread
        {
            get
            {
                ThrowIfDisposedOrDisposing(_state, nameof(Win32WindowProvider));
                return _windows.Value?.Values ?? Enumerable.Empty<Win32Window>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override Window CreateWindow()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(Win32WindowProvider));
            return new Win32Window(this);
        }

        [UnmanagedCallersOnly]
        private static nint ForwardWindowMessage(IntPtr hWnd, uint msg, nuint wParam, nint lParam)
        {
            nint userData;
            GCHandle gcHandle;
            Win32WindowProvider windowProvider;
            Dictionary<IntPtr, Win32Window>? windows;
            Win32Window? window;
            bool forwardMessage;

            if (msg == WM_CREATE)
            {
                // We allow the WM_CREATE message to be forwarded to the Window instance
                // for hWnd. This allows some delayed initialization to occur since most
                // of the fields in Window are lazy.

                var createStruct = (CREATESTRUCTW*)lParam;
                userData = (nint)createStruct->lpCreateParams;

                // Unlike the WindowProvider GCHandle, the Window GCHandle is short lived and
                // we want to free it after we add the relevant entries to the window map.

                gcHandle = GCHandle.FromIntPtr(userData);
                {
                    window = (Win32Window)gcHandle.Target!;
                    windowProvider = window.WindowProvider;
                    windows = windowProvider._windows.Value!;
                }
                gcHandle.Free();

                if (windows is null)
                {
                    windows = new Dictionary<IntPtr, Win32Window>(capacity: 4);
                    windowProvider._windows.Value = windows;
                }
                windows.Add(hWnd, window);

                // We then want to ensure the window provider is registered as a property for fast
                // subsequent lookups. This proocess also allows everything to be lazily initialized.

                gcHandle = window.WindowProvider.NativeHandle;
                userData = GCHandle.ToIntPtr(gcHandle);

                _ = SetWindowLongPtrW(hWnd, GWLP_USERDATA, userData);

                forwardMessage = false;
            }
            else
            {
                userData = GetWindowLongPtrW(hWnd, GWLP_USERDATA);

                if (userData != 0)
                {
                    gcHandle = GCHandle.FromIntPtr(userData);

                    windowProvider = (Win32WindowProvider)gcHandle.Target!;
                    windows = windowProvider._windows.Value!;

                    forwardMessage = windows.TryGetValue(hWnd, out window);
                }
                else
                {
                    windows = null;
                    window = null;
                    forwardMessage = false;
                }
            }

            if (forwardMessage)
            {
                AssertNotNull(windows);
                AssertNotNull(window);

                var result = window.ProcessWindowMessage(msg, wParam, lParam);

                if (msg == WM_DESTROY)
                {
                    // We forward the WM_DESTROY message to the corresponding Window instance
                    // so that it can still be properly disposed of in the scenario that the
                    // hWnd was destroyed externally.

                    _ = RemoveWindow(windows, hWnd);
                }

                return result;
            }

            return DefWindowProcW(hWnd, msg, wParam, lParam);
        }

        private static Win32Window RemoveWindow(Dictionary<IntPtr, Win32Window> windows, IntPtr hWnd)
        {
            _ = windows.Remove(hWnd, out var window);
            AssertNotNull(window);

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
            ThrowForLastErrorIfZero(GetClassNameW(desktopWindowHandle, desktopClassName, 256), nameof(GetClassNameW));

            WNDCLASSEXW desktopWindowClass;

            ThrowExternalExceptionIfFalse(GetClassInfoExW(
                HINSTANCE.NULL,
                lpszClass: desktopClassName,
                lpwcx: &desktopWindowClass
            ), nameof(GetClassInfoExW));

            return desktopWindowClass.hCursor;
        }

        private ushort CreateClassAtom()
        {
            AssertNotDisposedOrDisposing(_state);

            ushort classAtom;
            {
                // lpszClassName should be less than 256 characters (this includes the null terminator)
                // Currently, we are well below this limit and should be hitting 74 characters + the null terminator

                var className = $"{GetType().FullName}.{EntryPointModule:X16}.{GetHashCode():X8}";
                Assert(AssertionsEnabled && (className.Length < byte.MaxValue));

                fixed (char* lpszClassName = className)
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
                        hbrBackground = (IntPtr)(COLOR_WINDOW + 1),
                        lpszMenuName = null,
                        lpszClassName = (ushort*)lpszClassName,
                        hIconSm = HICON.NULL
                    };

                    classAtom = RegisterClassExW(&wndClassEx);
                }
                ThrowForLastErrorIfZero(classAtom, nameof(RegisterClassExW));
            }
            return classAtom;
        }

        private GCHandle CreateNativeHandle() => GCHandle.Alloc(this, GCHandleType.Normal);

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
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
            AssertDisposing(_state);

            if (_classAtom.IsValueCreated)
            {
                ThrowExternalExceptionIfFalse(UnregisterClassW((ushort*)_classAtom.Value, EntryPointModule), nameof(UnregisterClassW));
            }
        }

        private void DisposeNativeHandle()
        {
            AssertDisposing(_state);

            if (_nativeHandle.IsValueCreated)
            {
                _nativeHandle.Value.Free();
            }
        }

        private void DisposeWindows(bool isDisposing)
        {
            AssertDisposing(_state);

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
                            var dispatchProvider = Win32.Win32DispatchProvider.Instance;
                            var window = RemoveWindow(windows, hWnd);
                            window.Dispose();
                        }

                        Assert(AssertionsEnabled && (windows.Count == 0));
                    }
                }

                _windows.Dispose();
            }
        }
    }
}
