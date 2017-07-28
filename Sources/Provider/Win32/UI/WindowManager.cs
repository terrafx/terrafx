// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Interop.Desktop;
using TerraFX.Provider.Win32.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static System.Threading.Interlocked;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    [Export(typeof(IWindowManager))]
    [Export(typeof(WindowManager))]
    [Shared]
    unsafe public sealed class WindowManager : IDisposable, IWindowManager
    {
        #region State Constants
        /// <summary>Indicates the window manager is not disposing or disposed..</summary>
        internal const int NotDisposingOrDisposed = 0;

        /// <summary>Indicates the window manager is being disposed.</summary>
        internal const int Disposing = 1;

        /// <summary>Indicates the window manager has been disposed.</summary>
        internal const int Disposed = 2;
        #endregion

        #region Static Fields
        /// <summary>The <see cref="Window" /> instances that have been created.</summary>
        internal static readonly ConcurrentDictionary<IntPtr, Window> CreatedWindows = new ConcurrentDictionary<IntPtr, Window>();

        /// <summary>The HINSTANCE for the entry-point module.</summary>
        internal static readonly void* EntryModuleHandle = GetModuleHandle(); 

        /// <summary>The <see cref="NativeDelegate{TDelegate}" /> for the <see cref="WNDPROC" /> method.</summary>
        internal static readonly NativeDelegate<WNDPROC> WindowProcedure = new NativeDelegate<WNDPROC>(ProcessWindowMessage);
        #endregion

        #region Fields
        /// <summary>The <see cref="DispatchManager" /> for the instance.</summary>
        internal readonly Lazy<DispatchManager> _dispatchManager;

        /// <summary>The <see cref="NativeStringUni" /> of the registered class for the instance.</summary>
        internal readonly NativeStringUni _className;

        /// <summary>The <see cref="NativeStringUni" /> of the default window name for the instance.</summary>
        internal readonly NativeStringUni _defaultWindowTitle;

        /// <summary>The ATOM of the registered class for the instance.</summary>
        internal ushort _classAtom;

        /// <summary>The state for the instance.</summary>
        /// <remarks>
        ///     <para>This field is <c>volatile</c> to ensure state changes update all threads simultaneously.</para>
        ///     <para><c>volatile</c> does add a read/write barrier at every access, but the state transitions are believed to be infrequent enough for this to not be a problem.</para>
        /// </remarks>
        internal volatile int _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WindowManager" /> class.</summary>
        [ImportingConstructor]
        internal WindowManager(
            [Import] Lazy<DispatchManager> dispatchManager
        )
        {
            _dispatchManager = dispatchManager;
            _className = new NativeStringUni($"TerraFX.Interop.Provider.Win32.UI.WindowManager.{(IntPtr)(EntryModuleHandle)}");
            _defaultWindowTitle = new NativeStringUni("TerraFX Win32 Window");
            _classAtom = CreateClassAtom((char*)((IntPtr)(_className)));
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="WindowManager" /> class.</summary>
        ~WindowManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Properties
        /// <summary>Gets the <see cref="NativeStringUni" /> of the registered class for the instance.</summary>
        public NativeStringUni ClassName
        {
            get
            {
                return _className;
            }
        }

        /// <summary>Gets the <see cref="NativeStringUni" /> of the default window name for the instance.</summary>
        public NativeStringUni DefaultWindowTitle
        {
            get
            {
                return _defaultWindowTitle;
            }
        }

        /// <summary>Gets the <see cref="DispatchManager" /> for the instance.</summary>
        public DispatchManager DispatchManager
        {
            get
            {
                return _dispatchManager.Value;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>Disposes of all <see cref="Window" /> instances that have been created.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(void*)" /> failed.</exception>
        internal static void DisposeCreatedWindows(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var createdWindowHandle in CreatedWindows.Keys)
                {
                    if (CreatedWindows.TryRemove(createdWindowHandle, out var createdWindow))
                    {
                        createdWindow.Dispose();
                    }
                }
            }
            else
            {
                CreatedWindows.Clear();
            }

            Debug.Assert(CreatedWindows.IsEmpty);
        }

        /// <summary>Creates a ATOM for a native window class.</summary>
        /// <param name="className">The name of the native window class to create.</param>
        /// <exception cref="ExternalException">The call to <see cref="GetClassName(void*, char*, int)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="GetClassInfoEx(void*, char*, WNDCLASSEX*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="RegisterClassEx(WNDCLASSEX*)" /> failed.</exception>
        /// <returns>A ATOM for the native window class that was created.</returns>
        internal static ushort CreateClassAtom(char* className)
        {
            var wndClassEx = new WNDCLASSEX() {
                cbSize = SizeOf<WNDCLASSEX>(),
                style = (CS_VREDRAW | CS_HREDRAW | CS_DBLCLKS),
                lpfnWndProc = WindowProcedure,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = EntryModuleHandle,
                hIcon = null,
                hCursor = GetDesktopCursor(),
                hbrBackground = (void*)(COLOR_WINDOW + 1),
                lpszMenuName = null,
                lpszClassName = className,
                hIconSm = null
            };

            var classAtom = RegisterClassEx(&wndClassEx);

            if (classAtom == 0)
            {
                ThrowExternalExceptionForLastError(nameof(RegisterClassEx));
            }

            return classAtom;
        }

        /// <summary>Gets the HICON for the desktop window.</summary>
        /// <returns>The HICON for the desktop window.</returns>
        /// <exception cref="ExternalException">The call to <see cref="GetClassName(void*, char*, int)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="GetClassInfoEx(void*, char*, WNDCLASSEX*)" /> failed.</exception>
        internal static void* GetDesktopCursor()
        {
            var desktopWindowHandle = GetDesktopWindow();

            var desktopClassName = stackalloc char[256];
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

        /// <summary>Forwards native window messages to the appropriate <see cref="Window" /> instance for processing.</summary>
        /// <param name="hWnd">The HWND of the <see cref="Window" /> the message should be forwarded to.</param>
        /// <param name="Msg">The message to be processed.</param>
        /// <param name="wParam">The first parameter of the message to be processed.</param>
        /// <param name="lParam">The second parameter of the message to be processed.</param>
        /// <returns>A value that varies based on the exact message that was processed.</returns>
        internal static nint ProcessWindowMessage(void* hWnd, uint Msg, nuint wParam, nint lParam)
        {
            if (CreatedWindows.TryGetValue((IntPtr)(hWnd), out var window))
            {
                return window.ProcessWindowMessage(Msg, wParam, lParam);
            }

            return DefWindowProc(hWnd, Msg, wParam, lParam);
        }

        /// <summary>Throws a <see cref="ObjectDisposedException" /> if the instance has already been disposed.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        internal static void ThrowIfDisposed(int state)
        {
            if (state >= Disposing) // (_state == Disposing) || (_state == Disposed)
            {
                ThrowObjectDisposedException(nameof(WindowManager));
            }
        }
        #endregion

        #region Methods
        /// <summary>Removes a <see cref="Window" /> instance from the list of created windows.</summary>
        /// <param name="window">The <see cref="Window" /> to remove.</param>
        internal void RemoveWindow(Window window)
        {
            CreatedWindows.TryRemove(window.Handle, out var destroyedWindow);
            Debug.Assert(destroyedWindow == window);
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(void*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="UnregisterClass(char*, void*)" /> failed.</exception>
        internal void Dispose(bool isDisposing)
        {
            var previousState = Exchange(ref _state, Disposing);

            if (previousState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeCreatedWindows(isDisposing);

                _defaultWindowTitle.Dispose();
                _className.Dispose();

                DisposeClassAtom();
            }

            Debug.Assert(CreatedWindows.IsEmpty);
            Debug.Assert(_className == IntPtr.Zero);
            Debug.Assert(_defaultWindowTitle == IntPtr.Zero);
            Debug.Assert(_classAtom == 0);

            _state = Disposed;
        }

        /// <summary>Disposes of the ATOM that was created for the native window class.</summary>
        /// <exception cref="ExternalException">The call to <see cref="UnregisterClass(char*, void*)" /> failed.</exception>
        internal void DisposeClassAtom()
        {
            Debug.Assert(_state == Disposing);

            if (_classAtom != 0)
            {
                var result = UnregisterClass((char*)((IntPtr)(_classAtom)), EntryModuleHandle);

                if (result == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(UnregisterClass));
                }

                _classAtom = 0;
            }
        }
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region TerraFX.UI.IWindowManager Methods
        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IWindow CreateWindow()
        {
            ThrowIfDisposed(_state);
            var window = new Window(this, _dispatchManager.Value, EntryModuleHandle);

            var succeeded = CreatedWindows.TryAdd(window.Handle, window);
            Debug.Assert(succeeded);

            return window;
        }
        #endregion
    }
}
