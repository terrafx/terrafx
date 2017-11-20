// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Interop;
using TerraFX.Provider.Win32.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;
using static TerraFX.Provider.Win32.UI.WindowManager;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class Window : IDisposable, IWindow
    {
        #region Fields
        /// <summary>The native window handle for the instance.</summary>
        internal readonly Lazy<IntPtr> _handle;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly Thread _parentThread;

        /// <summary>The <see cref="PropertySet" /> for the instance.</summary>
        internal readonly PropertySet _properties;

        /// <summary>The title for the instance.</summary>
        internal string _title;

        /// <summary>The <see cref="WindowManager" /> for the instance.</summary>
        internal readonly WindowManager _windowManager;

        /// <summary>A <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        internal Rectangle _bounds;

        /// <summary>The <see cref="FlowDirection" /> for the instance.</summary>
        internal FlowDirection _flowDirection;

        /// <summary>The <see cref="ReadingDirection" /> for the instance.</summary>
        internal ReadingDirection _readingDirection;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal readonly State _state;

        /// <summary>A value that indicates whether the instance is the active window.</summary>
        internal bool _isActive;

        /// <summary>A value that indicates whether the instance is visible.</summary>
        internal bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        /// <param name="windowManager">The <see cref="WindowManager" /> for the instance.</param>
        internal Window(WindowManager windowManager)
        {
            _handle = new Lazy<IntPtr>(CreateWindowHandle, isThreadSafe: true);

            _parentThread = Thread.CurrentThread;
            _properties = new PropertySet();
            _title = typeof(Window).FullName;

            _windowManager = windowManager;
            _state.Transition(to: Initialized);
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
                return _handle.Value;
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

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread
        {
            get
            {
                return _parentThread;
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

        /// <summary>Gets the title for the instance.</summary>
        public string Title
        {
            get
            {
                return _title;
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
        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of a native window.</summary>
        /// <param name="handle">A handle to the native window to get the bounds for.</param>
        /// <returns>A <see cref="Rectangle" /> that represents the bounds of <paramref name="handle" />.</returns>
        /// <exception cref="ExternalException">The call to <see cref="GetWindowRect(IntPtr, RECT*)" /> failed.</exception>
        internal static Rectangle GetWindowBounds(IntPtr handle)
        {
            RECT rect;
            {
                var succeeded = GetWindowRect(handle, &rect);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(GetWindowRect));
                }
            }
            return new Rectangle(rect.left, rect.top, (rect.right - rect.left), (rect.bottom - rect.top));
        }

        /// <summary>Geta a value that indicates whether a native window is active.</summary>
        /// <param name="handle">A handle to the native window to check.</param>
        /// <returns>A value that indicates whether <paramref name="handle" /> is active.</returns>
        internal static bool IsWindowActive(IntPtr handle)
        {
            var activeWindow = GetActiveWindow();
            return (activeWindow == handle);
        }
        #endregion

        #region Methods
        /// <summary>Creates a <c>HWND</c> for the instance.</summary>
        /// <returns>A <c>HWND</c> for the created native window.</returns>
        /// <exception cref="ExternalException">The call to <see cref="CreateWindowEx(uint, char*, char*, uint, int, int, int, int, IntPtr, IntPtr, IntPtr, void*)" /> failed.</exception>
        internal IntPtr CreateWindowHandle()
        {
            IntPtr hWnd;

            fixed (char* lpWindowName = _title)
            {
                hWnd = CreateWindowEx(
                    WS_EX_OVERLAPPEDWINDOW,
                    (char*)(_windowManager.ClassAtom),
                    lpWindowName,
                    WS_OVERLAPPEDWINDOW,
                    X: CW_USEDEFAULT,
                    Y: CW_USEDEFAULT,
                    nWidth: CW_USEDEFAULT,
                    nHeight: CW_USEDEFAULT,
                    hWndParent: default,
                    hMenu: default,
                    hInstance: EntryPointModule,
                    lpParam: null
                );
            }

            if (hWnd == IntPtr.Zero)
            {
                ThrowExternalExceptionForLastError(nameof(CreateWindowEx));
            }

            return hWnd;
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(IntPtr)" /> failed.</exception>
        internal void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeWindowHandle();
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the <c>HWND</c> for the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(IntPtr)" /> failed.</exception>
        internal void DisposeWindowHandle()
        {
            if (_handle.IsValueCreated)
            {
                var result = DestroyWindow(_handle.Value);

                if (result == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(DestroyWindow));
                }
            }
        }

        /// <summary>Handles the <see cref="WM_ACTIVATE" /> message.</summary>
        /// <param name="wParam">A value that indicates whether the instance is being activated or deactivated and the minimized state of the instance.</param>
        /// <returns>0</returns>
        internal nint HandleWmActivate(nuint wParam)
        {
            _isActive = (LOWORD(wParam) != WA_INACTIVE);
            return 0;
        }

        /// <summary>Handles the <see cref="WM_CLOSE" /> message.</summary>
        /// <returns>0</returns>
        internal nint HandleWmClose()
        {
            Dispose();
            return 0;
        }

        /// <summary>Handles the <see cref="WM_DESTROY" /> message.</summary>
        /// <returns>0</returns>
        internal nint HandleWmDestroy()
        {
            Dispose();
            return 0;
        }

        /// <summary>Handles the <see cref="WM_MOVE" /> message.</summary>
        /// <param name="lParam">A value that represents the x and y coordinates of the upper-left corner of the client area of the window.</param>
        /// <returns>0</returns>
        internal nint HandleWmMove(nint lParam)
        {
            _bounds.Location = new Point2D(x: LOWORD(lParam), y: HIWORD(lParam));
            return 0;
        }

        /// <summary>Handles the <see cref="WM_SETTEXT" /> message.</summary>
        /// <param name="wParam">This parameter is not used.</param>
        /// <param name="lParam">A value that represents a pointer to the new window text.</param>
        /// <returns>A value dependent on the default processing for <see cref="WM_SETTEXT" />.</returns>
        internal nint HandleWmSetText(nuint wParam, nint lParam)
        {
            _title = Marshal.PtrToStringUni(lParam);
            return DefWindowProc(_handle.Value, WM_SETTEXT, wParam, lParam);
        }

        /// <summary>Handles the <see cref="WM_SHOWWINDOW" /> message.</summary>
        /// <param name="wParam">A value that indicates whether the window is being shown or hidden.</param>
        /// <returns>0</returns>
        internal nint HandleWmShowWindow(nuint wParam)
        {
            _isVisible = (LOWORD(wParam) != FALSE);
            return 0;
        }

        /// <summary>Handles the <see cref="WM_SIZE" /> message.</summary>
        /// <param name="lParam">A value that represents the width and height of the client area of the window.</param>
        /// <returns>0</returns>
        internal nint HandleWmSize(nint lParam)
        {
            _bounds.Size = new Size2D(width: LOWORD(lParam), height: HIWORD(lParam));
            return 0;
        }

        /// <summary>Processes a window message sent to the instance.</summary>
        /// <param name="Msg">The message to be processed.</param>
        /// <param name="wParam">The first parameter of the message.</param>
        /// <param name="lParam">The second parameter of the message.</param>
        /// <returns>A value that varies based on the exact message that was processed.</returns>
        internal nint ProcessWindowMessage(uint Msg, nuint wParam, nint lParam)
        {
            ThrowIfNotThread(_parentThread);

            switch (Msg)
            {
                case WM_DESTROY:
                {
                    return HandleWmDestroy();
                }

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

                case WM_SETTEXT:
                {
                    return HandleWmSetText(wParam, lParam);
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
                    return DefWindowProc(_handle.Value, Msg, wParam, lParam);
                }
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
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</exception>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="SetForegroundWindow(IntPtr)" /> failed.</exception>
        public void Activate()
        {
            ThrowIfNotThread(_parentThread);
            _state.ThrowIfDisposed();

            if (IsVisible)
            {
                var succeeded = SetForegroundWindow(_handle.Value);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(SetForegroundWindow));
                }
            }
        }

        /// <summary>Closes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <remarks>
        ///     <para>This method can be called from any thread.</para>
        ///     <para>This method does nothing if the underlying <c>HWND</c> has not been created.</para>
        /// </remarks>
        public void Close()
        {
            _state.ThrowIfDisposed();

            if (_handle.IsValueCreated)
            {
                SendMessage(_handle.Value, WM_CLOSE, wParam: 0, lParam: 0);
            }
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</exception>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="ShowWindow(IntPtr, int)" /> failed.</exception>
        public void Hide()
        {
            ThrowIfNotThread(_parentThread);
            _state.ThrowIfDisposed();

            if (_isVisible)
            {
                var succeeded = ShowWindow(_handle.Value, SW_HIDE);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="Dispatcher.ParentThread" />.</exception>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="ShowWindow(IntPtr, int)" /> failed.</exception>
        public void Show()
        {
            ThrowIfNotThread(_parentThread);
            _state.ThrowIfDisposed();

            if (_isVisible == false)
            {
                var succeeded = ShowWindow(_handle.Value, SW_SHOW);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }
        #endregion
    }
}
