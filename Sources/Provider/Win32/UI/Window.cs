// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Interop;
using TerraFX.Provider.Win32.Threading;
using TerraFX.Threading;
using TerraFX.UI;
using static System.Threading.Interlocked;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Defines a window.</summary>
    unsafe public sealed class Window : IDisposable, IWindow
    {
        #region State Constants
        /// <summary>Indicates the window manager is not disposing or disposed..</summary>
        internal const int NotDisposingOrDisposed = 0;

        /// <summary>Indicates the window manager is being disposed.</summary>
        internal const int Disposing = 1;

        /// <summary>Indicates the window manager has been disposed.</summary>
        internal const int Disposed = 2;
        #endregion

        #region Fields
        /// <summary>The <see cref="Dispatcher" /> for the <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly Dispatcher _dispatcher;

        /// <summary>The <see cref="PropertySet" /> for the instance.</summary>
        internal readonly PropertySet _properties;

        /// <summary>The <see cref="WindowManager" /> for the instance.</summary>
        internal readonly WindowManager _windowManager;

        /// <summary>The native window handle for the instance.</summary>
        internal HWND _handle;

        /// <summary>A <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        internal Rectangle _bounds;

        /// <summary>The <see cref="FlowDirection" /> for the instance.</summary>
        internal FlowDirection _flowDirection;

        /// <summary>The <see cref="ReadingDirection" /> for the instance.</summary>
        internal ReadingDirection _readingDirection;

        /// <summary>The state for the instance.</summary>
        /// <remarks>
        ///     <para>This field is <c>volatile</c> to ensure state changes update all threads simultaneously.</para>
        ///     <para><c>volatile</c> does add a read/write barrier at every access, but the state transitions are believed to be infrequent enough for this to not be a problem.</para>
        /// </remarks>
        internal volatile int _state;

        /// <summary>A value that indicates whether the instance is the active window.</summary>
        internal bool _isActive;

        /// <summary>A value that indicates whether the instance is visible.</summary>
        internal bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        /// <param name="windowManager">The <see cref="WindowManager" /> for the instance.</param>
        /// <param name="dispatchManager">The <see cref="DispatchManager" /> for the instance.</param>
        /// <param name="entryModuleHandle">The <see cref="HINSTANCE" /> for the entry module.</param>
        internal Window(WindowManager windowManager, DispatchManager dispatchManager, HINSTANCE entryModuleHandle)
        {
            Debug.Assert(windowManager != null);
            Debug.Assert(dispatchManager != null);
            Debug.Assert(entryModuleHandle != (HINSTANCE)(NULL));

            _dispatcher = (Dispatcher)(dispatchManager.DispatcherForCurrentThread);
            _properties = new PropertySet();
            _windowManager = windowManager;
            _handle = CreateWindowHandle((LPCWSTR)(windowManager.ClassName), (LPCWSTR)(windowManager.DefaultWindowTitle), entryModuleHandle);
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="Window" /> class.</summary>
        ~Window()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region TerraFX.UI.IWindow Properties
        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
        }

        /// <summary>Gets the <see cref="Dispatcher" /> for the <see cref="Thread" /> that was used to create the instance.</summary>
        public IDispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }

        /// <summary>Gets <see cref="FlowDirection" /> for the instance.</summary>
        public FlowDirection FlowDirection
        {
            get
            {
                return _flowDirection;
            }
        }

        /// <summary>Gets the handle for the instance.</summary>
        public IntPtr Handle
        {
            get
            {
                return (IntPtr)((void*)(_handle));
            }
        }

        /// <summary>Gets a value that indicates whether the instance is the active window.</summary>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }

        /// <summary>Gets a value that indicates whether the instance is visible.</summary>
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
        }

        /// <summary>Gets the <see cref="IPropertySet" /> for the instance.</summary>
        public IPropertySet Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>Gets the <see cref="ReadingDirection" /> for the instance.</summary>
        public ReadingDirection ReadingDirection
        {
            get
            {
                return _readingDirection;
            }
        }

        /// <summary>Gets the <see cref="IWindowManager" /> for the instance.</summary>
        public IWindowManager WindowManager
        {
            get
            {
                return _windowManager;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>Creates a <see cref="HWND" /> for a native window.</summary>
        /// <param name="className">The name of the native window class to use when creating the native window.</param>
        /// <param name="windowTitle">The title that will be given to the created native window.</param>
        /// <param name="instanceHandle">A handle to the instance that the created native window will be associated with.</param>
        /// <returns>A <see cref="HWND" /> for the created native window.</returns>
        /// <exception cref="ExternalException">The call to <see cref="CreateWindowEx(DWORD, LPCWSTR, LPCWSTR, DWORD, int, int, int, int, HWND, HMENU, HINSTANCE, LPVOID)" /> failed.</exception>
        internal static HWND CreateWindowHandle(LPCWSTR className, LPCWSTR windowTitle, HINSTANCE instanceHandle)
        {
            var hWnd = CreateWindowEx(
                WS_EX_OVERLAPPEDWINDOW,
                className,
                windowTitle,
                WS_OVERLAPPEDWINDOW,
                X: CW_USEDEFAULT,
                Y: CW_USEDEFAULT,
                nWidth: CW_USEDEFAULT,
                nHeight: CW_USEDEFAULT,
                hWndParent: (HWND)(NULL),
                hMenu: (HMENU)(NULL),
                hInstance: instanceHandle,
                lpParam: (LPVOID)(NULL)
            );

            if (hWnd == (HWND)(NULL))
            {
                ThrowExternalExceptionForLastError(nameof(CreateWindowEx));
            }

            return hWnd;
        }

        /// <summary>Gets <see cref="Thread.CurrentThread" /> and validates that <see cref="Thread.GetApartmentState" /> is <see cref="ApartmentState.STA" />.</summary>
        /// <returns><see cref="Thread.CurrentThread"/></returns>
        /// <exception cref="InvalidOperationException">The <see cref="ApartmentState" /> for <see cref="Thread.CurrentThread" /> is not <see cref="ApartmentState.STA"/>.</exception>
        internal static Thread GetParentThread()
        {
            var currentThread = Thread.CurrentThread;

            if (currentThread.GetApartmentState() != ApartmentState.STA)
            {
                ThrowInvalidOperationException(nameof(Thread.GetApartmentState), currentThread.GetApartmentState());
            }

            return currentThread;
        }

        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of a native window.</summary>
        /// <param name="handle">A handle to the native window to get the bounds for.</param>
        /// <returns>A <see cref="Rectangle" /> that represents the bounds of <paramref name="handle" />.</returns>
        /// <exception cref="ExternalException">The call to <see cref="GetWindowRect(HWND, LPRECT)" /> failed.</exception>
        internal static Rectangle GetWindowBounds(HWND handle)
        {
            RECT rect;

            var succeeded = GetWindowRect(handle, &rect);

            if (succeeded == FALSE)
            {
                ThrowExternalExceptionForLastError(nameof(GetWindowRect));
            }

            return new Rectangle(rect.left, rect.top, (rect.right - rect.left), (rect.bottom - rect.top));
        }

        /// <summary>Geta a value that indicates whether a native window is active.</summary>
        /// <param name="handle">A handle to the native window to check.</param>
        /// <returns>A value that indicates whether <paramref name="handle" /> is active.</returns>
        internal static bool IsWindowActive(HWND handle)
        {
            var activeWindow = GetActiveWindow();
            return (activeWindow == handle);
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
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(HWND)" /> failed.</exception>
        internal void Dispose(bool isDisposing)
        {
            var previousState = Exchange(ref _state, Disposing);

            if (previousState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeWindowHandle();
            }

            Debug.Assert(_handle == (HWND)(NULL));

            _state = Disposed;
        }

        /// <summary>Disposes of the <see cref="HWND" /> for the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(HWND)" /> failed.</exception>
        internal void DisposeWindowHandle()
        {
            Debug.Assert(_state == Disposing);

            if (_handle != (HWND)(NULL))
            {
                _windowManager.RemoveWindow(this);

                var succeeded = DestroyWindow(_handle);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(DestroyWindow));
                }

                _handle = (HWND)(NULL);
            }
        }

        /// <summary>Handles the <see cref="WM_ACTIVATE" /> message.</summary>
        /// <param name="wParam">A value that indicates whether the instance is being activated or deactivated and the minimized state of the instance.</param>
        /// <returns>0</returns>
        internal LRESULT HandleWmActivate(WPARAM wParam)
        {
            _isActive = (LOWORD(wParam) != WA_INACTIVE);
            return 0;
        }

        /// <summary>Handles the <see cref="WM_CLOSE" /> message.</summary>
        /// <returns>0</returns>
        internal LRESULT HandleWmClose()
        {
            Dispose();
            return 0;
        }

        /// <summary>Handles the <see cref="WM_MOVE" /> message.</summary>
        /// <param name="lParam">A value that represents the x and y coordinates of the upper-left corner of the client area of the window.</param>
        /// <returns>0</returns>
        internal LRESULT HandleWmMove(LPARAM lParam)
        {
            _bounds.Location = new Point2D(x: LOWORD(lParam), y: HIWORD(lParam));
            return 0;
        }

        /// <summary>Handles the <see cref="WM_SHOWWINDOW" /> message.</summary>
        /// <param name="wParam">A value that indicates whether the window is being shown or hidden.</param>
        /// <returns>0</returns>
        internal LRESULT HandleWmShowWindow(WPARAM wParam)
        {
            _isVisible = (LOWORD(wParam) != FALSE);
            return 0;
        }

        /// <summary>Handles the <see cref="WM_SIZE" /> message.</summary>
        /// <param name="lParam">A value that represents the width and height of the client area of the window.</param>
        /// <returns>0</returns>
        internal LRESULT HandleWmSize(LPARAM lParam)
        {
            _bounds.Size = new Size2D(width: LOWORD(lParam), height: HIWORD(lParam));
            return 0;
        }

        /// <summary>Processes a window message sent to the instance.</summary>
        /// <param name="Msg">The message to be processed.</param>
        /// <param name="wParam">The first parameter of the message.</param>
        /// <param name="lParam">The second parameter of the message.</param>
        /// <returns>A value that varies based on the exact message that was processed.</returns>
        internal LRESULT ProcessWindowMessage(UINT Msg, WPARAM wParam, LPARAM lParam)
        {
            Debug.Assert(Thread.CurrentThread == _dispatcher.ParentThread);

            switch (Msg)
            {
                case WM_MOVE:
                {
                    return HandleWmMove(lParam);
                }

                case WM_SIZE:
                {
                    return HandleWmSize(lParam);
                }

                case WM_ACTIVATE:
                {
                    return HandleWmActivate(wParam);
                }

                case WM_CLOSE:
                {
                    return HandleWmClose();
                }

                case WM_SHOWWINDOW:
                {
                    return HandleWmShowWindow(wParam);
                }

                default:
                {
                    return DefWindowProc(_handle, Msg, wParam, lParam);
                }
            }
        }

        /// <summary>Throws a <see cref="InvalidOperationException" /> if <see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</exception>
        internal void ThrowIfNotParentThread()
        {
            if (Thread.CurrentThread != _dispatcher.ParentThread)
            {
                ThrowInvalidOperationException(nameof(Thread.CurrentThread), Thread.CurrentThread);
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

        #region TerraFX.UI.IWindow Methods
        /// <summary>Activates the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</exception>
        /// <exception cref="ExternalException">The call to <see cref="SetForegroundWindow(HWND)" /> failed.</exception>
        public void Activate()
        {
            ThrowIfDisposed(_state);
            ThrowIfNotParentThread();

            if (IsVisible)
            {
                var succeeded = SetForegroundWindow(_handle);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(SetForegroundWindow));
                }
            }
        }

        /// <summary>Closes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <remarks>This method can be called from any thread.</remarks>
        public void Close()
        {
            ThrowIfDisposed(_state);
            SendMessage(_handle, WM_CLOSE, wParam: 0, lParam: 0);
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</exception>
        /// <exception cref="ExternalException">The call to <see cref="ShowWindow(HWND, int)" /> failed.</exception>
        public void Hide()
        {
            ThrowIfDisposed(_state);
            ThrowIfNotParentThread();

            if (_isVisible)
            {
                var succeeded = ShowWindow(_handle, SW_HIDE);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</exception>
        /// <exception cref="ExternalException">The call to <see cref="ShowWindow(HWND, int)" /> failed.</exception>
        public void Show()
        {
            ThrowIfDisposed(_state);
            ThrowIfNotParentThread();

            if (_isVisible == false)
            {
                var succeeded = ShowWindow(_handle, SW_SHOW);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }
        #endregion
    }
}
