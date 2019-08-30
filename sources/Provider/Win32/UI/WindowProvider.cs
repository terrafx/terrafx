// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Provides access to a Win32 based window subsystem.</summary>
    [Export(typeof(IWindowProvider))]
    [Export(typeof(WindowProvider))]
    [Shared]
    public sealed unsafe class WindowProvider : IDisposable, IWindowProvider
    {
        /// <summary>A <c>HMODULE</c> to the entry point module.</summary>
        public static readonly IntPtr EntryPointModule = GetModuleHandle();

        /// <summary>The <see cref="NativeDelegate{TDelegate}" /> for the <see cref="WNDPROC" /> method.</summary>
        private static readonly NativeDelegate<WNDPROC> ForwardWndProc = new NativeDelegate<WNDPROC>(ForwardWindowMessage);

        /// <summary>The <c>ATOM</c> of the <see cref="WNDCLASSEX" /> registered for the instance.</summary>
        private readonly Lazy<ushort> _classAtom;

        /// <summary>The <see cref="DispatchProvider" /> for the instance.</summary>
        private readonly Lazy<DispatchProvider> _dispatchProvider;

        /// <summary>The <see cref="GCHandle" /> containing the native handle for the instance.</summary>
        private readonly Lazy<GCHandle> _nativeHandle;

        /// <summary>A map of <c>HWND</c> to <see cref="Window" /> objects created for the instance.</summary>
        private readonly ConcurrentDictionary<IntPtr, Window> _windows;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="WindowProvider" /> class.</summary>
        [ImportingConstructor]
        public WindowProvider(
            [Import] Lazy<DispatchProvider> dispatchProvider
        )
        {
            _classAtom = new Lazy<ushort>(CreateClassAtom, isThreadSafe: true);
            _dispatchProvider = dispatchProvider;
            _nativeHandle = new Lazy<GCHandle>(() => GCHandle.Alloc(this, GCHandleType.Normal), isThreadSafe: true);

            _windows = new ConcurrentDictionary<IntPtr, Window>();
            _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="WindowProvider" /> class.</summary>
        ~WindowProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <c>ATOM</c> of the <see cref="WNDCLASSEX" /> registered for the instance.</summary>
        public ushort ClassAtom
        {
            get
            {
                return _state.IsNotDisposedOrDisposing ? _classAtom.Value : (ushort)0;
            }
        }

        /// <summary>Gets the <see cref="DispatchProvider" /> for the instance.</summary>
        public DispatchProvider DispatchProvider
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _dispatchProvider.Value;
            }
        }

        /// <summary>Gets the <see cref="GCHandle" /> containing the native handle for the instance.</summary>
        public GCHandle NativeHandle
        {
            get
            {
                _state.AssertNotDisposedOrDisposing();
                return _nativeHandle.Value;
            }
        }

        /// <summary>Gets the <see cref="IWindow" /> objects created by the instance.</summary>
        public IEnumerable<IWindow> Windows
        {
            get
            {
                return _state.IsNotDisposedOrDisposing ? (IEnumerable<IWindow>)_windows : Array.Empty<IWindow>();
            }
        }

        /// <summary>Forwards native window messages to the appropriate <see cref="Window" /> instance for processing.</summary>
        /// <param name="hWnd">The <c>HWND</c> of the <see cref="Window" /> the message should be forwarded to.</param>
        /// <param name="Msg">The message to be processed.</param>
        /// <param name="wParam">The first parameter of the message to be processed.</param>
        /// <param name="lParam">The second parameter of the message to be processed.</param>
        /// <returns>A value that varies based on the exact message that was processed.</returns>
        private static IntPtr ForwardWindowMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam)
        {
            IntPtr result, userData;

            if (Msg == WM_CREATE)
            {
                // We allow the WM_CREATE message to be forwarded to the Window instance
                // for hWnd. This allows some delayed initialization to occur since most
                // of the fields in Window are lazy.

                ref var pCreateStruct = ref AsRef<CREATESTRUCT>(lParam);
                userData = (IntPtr)pCreateStruct.lpCreateParams;
                SetWindowLongPtr(hWnd, GWLP_USERDATA, userData);
            }
            else
            {
                userData = GetWindowLongPtr(hWnd, GWLP_USERDATA);
            }

            // We are assuming that userData will definitely be set here and that it will be set
            // to a GCHandle for a WindowProvider instance. It is certainly possible, although unsupported,
            // for a user to get our registered class information and to create a new window from that
            // without passing in a GCHandle as the lParam to CreateWindowEx. We will just fail
            // by allowing the runtime to throw an exception in that scenario.

            var windowProvider = (WindowProvider)GCHandle.FromIntPtr(userData).Target!;

            if (windowProvider._windows.TryGetValue(hWnd, out var window))
            {
                if (Msg == WM_DESTROY)
                {
                    // We forward the WM_DESTROY message to the corresponding Window instance
                    // so that it can still be properly disposed of in the scenario that the
                    // hWnd was destroyed externally.

                    windowProvider._windows.TryRemove(hWnd, out window);
                }

                result = window!.ProcessWindowMessage(Msg, wParam, lParam);
            }
            else
            {
                result = DefWindowProc(hWnd, Msg, wParam, lParam);
            }

            return result;
        }

        /// <summary>Gets the <c>HCURSOR</c> for the desktop window.</summary>
        /// <returns>The <c>HCURSOR</c> for the desktop window.</returns>
        /// <exception cref="ExternalException">The call to <see cref="GetClassName(IntPtr, char*, int)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="GetClassInfoEx(IntPtr, char*, WNDCLASSEX*)" /> failed.</exception>
        private static IntPtr GetDesktopCursor()
        {
            var desktopWindowHandle = GetDesktopWindow();

            var desktopClassName = stackalloc char[256]; // 256 is the maximum length of WNDCLASSEX.lpszClassName
            var desktopClassNameLength = GetClassName(desktopWindowHandle, desktopClassName, 256);

            if (desktopClassNameLength == 0)
            {
                ThrowExternalExceptionForLastError(nameof(GetClassName));
            }

            WNDCLASSEX desktopWindowClass;
            var succeeded = GetClassInfoEx(
                lpszClass: desktopClassName,
                lpwcx: &desktopWindowClass
            );

            if (succeeded == FALSE)
            {
                ThrowExternalExceptionForLastError(nameof(GetClassInfoEx));
            }

            return desktopWindowClass.hCursor;
        }

        /// <summary>Creates an <c>ATOM</c> by registering a <see cref="WNDCLASSEX" /> for the entry point module.</summary>
        /// <exception cref="ExternalException">The call to <see cref="GetClassName(IntPtr, char*, int)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="GetClassInfoEx(IntPtr, char*, WNDCLASSEX*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="RegisterClassEx(WNDCLASSEX*)" /> failed.</exception>
        /// <returns>The <c>ATOM</c> created by registering a <see cref="WNDCLASSEX" /> for the entry point module.</returns>
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
                    var wndClassEx = new WNDCLASSEX {
                        cbSize = SizeOf<WNDCLASSEX>(),
                        style = CS_VREDRAW | CS_HREDRAW | CS_DBLCLKS,
                        lpfnWndProc = ForwardWndProc,
                        /* cbClsExtra = 0, */
                        /* cbWndExtra = 0, */
                        hInstance = EntryPointModule,
                        /* hIcon = IntPtr.Zero, */
                        hCursor = GetDesktopCursor(),
                        hbrBackground = (IntPtr)(COLOR_WINDOW + 1),
                        /* lpszMenuName = null, */
                        lpszClassName = lpszClassName,
                        /* hIconSm = IntPtr.Zero */
                    };

                    classAtom = RegisterClassEx(&wndClassEx);
                }

                if (classAtom == 0)
                {
                    ThrowExternalExceptionForLastError(nameof(RegisterClassEx));
                }
            }
            return classAtom;
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(IntPtr)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="UnregisterClass(char*, IntPtr)" /> failed.</exception>
        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeWindows(isDisposing);
                DisposeClassAtom();
                DisposeNativeHandle();
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the <c>ATOM</c> of the <see cref="WNDCLASSEX" /> registered for the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="UnregisterClass(char*, IntPtr)" /> failed.</exception>
        private void DisposeClassAtom()
        {
            _state.AssertDisposing();

            if (_classAtom.IsValueCreated)
            {
                var result = UnregisterClass((char*)_classAtom.Value, EntryPointModule);

                if (result == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(UnregisterClass));
                }
            }
        }

        /// <summary>Disposes of the <see cref="GCHandle" /> containing the native handle of the instance.</summary>
        private void DisposeNativeHandle()
        {
            _state.AssertDisposing();

            if (_nativeHandle.IsValueCreated)
            {
                _nativeHandle.Value.Free();
            }
        }

        /// <summary>Disposes of all <see cref="Window" /> objects that were created by the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(IntPtr)" /> failed.</exception>
        private void DisposeWindows(bool isDisposing)
        {
            _state.AssertDisposing();

            if (isDisposing)
            {
                foreach (var windowHandle in _windows.Keys)
                {
                    if (_windows.TryRemove(windowHandle, out var createdWindow))
                    {
                        createdWindow.Dispose();
                    }
                }
            }
            else
            {
                _windows.Clear();
            }

            Assert(_windows.IsEmpty, Resources.ArgumentOutOfRangeExceptionMessage, nameof(_windows.IsEmpty), _windows.IsEmpty);
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IWindow CreateWindow()
        {
            _state.ThrowIfDisposedOrDisposing();

            var window = new Window(this);
            _windows.TryAdd(window.Handle, window);

            return window;
        }
    }
}
