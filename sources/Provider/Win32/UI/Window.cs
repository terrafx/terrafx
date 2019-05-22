// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics.Geometry2D;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Provider.Win32.UI.WindowProvider;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class Window : IDisposable, IWindow
    {
        #region Fields
        /// <summary>The native window handle for the instance.</summary>
        private readonly Lazy<IntPtr> _handle;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        private readonly Thread _parentThread;

        /// <summary>The <see cref="PropertySet" /> for the instance.</summary>
        private readonly PropertySet _properties;

        /// <summary>The title for the instance.</summary>
        private string _title;

        /// <summary>The <see cref="WindowProvider" /> for the instance.</summary>
        private readonly WindowProvider _windowProvider;

        /// <summary>A <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        private Rectangle _bounds;

        /// <summary>The <see cref="FlowDirection" /> for the instance.</summary>
        private readonly FlowDirection _flowDirection;

        /// <summary>The <see cref="ReadingDirection" /> for the instance.</summary>
        private readonly ReadingDirection _readingDirection;

        /// <summary>The <see cref="WindowState" /> for the instance.</summary>
        private WindowState _windowState;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        private State _state;

        /// <summary>A value that indicates whether the instance is the active window.</summary>
        private bool _isActive;

        /// <summary>A value that indicates whether the instance is enabled.</summary>
        private bool _isEnabled;

        /// <summary>A value that indicates whether the instance is visible.</summary>
        private bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        /// <param name="windowProvider">The <see cref="WindowProvider" /> for the instance.</param>
        internal Window(WindowProvider windowProvider)
        {
            _handle = new Lazy<IntPtr>((Func<IntPtr>)this.CreateWindowHandle, isThreadSafe: true);

            _parentThread = Thread.CurrentThread;
            _properties = new PropertySet();
            _title = typeof(Window).FullName;
            _bounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);
            _flowDirection = FlowDirection.TopToBottom;
            _readingDirection = ReadingDirection.LeftToRight;

            _windowProvider = windowProvider;
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
                return _state.IsNotDisposedOrDisposing ? _handle.Value : IntPtr.Zero;
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

        /// <summary>Gets a value that indicates whether the instance is enabled.</summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
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

        /// <summary>Gets the <see cref="IWindowProvider" /> for the instance.</summary>
        public IWindowProvider WindowProvider
        {
            get
            {
                return _windowProvider;
            }
        }

        /// <summary>Gets the <see cref="WindowState" /> for the instance.</summary>
        public WindowState WindowState
        {
            get
            {
                return _windowState;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of a native window.</summary>
        /// <param name="handle">A handle to the native window to get the bounds for.</param>
        /// <returns>A <see cref="Rectangle" /> that represents the bounds of <paramref name="handle" />.</returns>
        /// <exception cref="ExternalException">The call to <see cref="GetWindowRect(IntPtr, RECT*)" /> failed.</exception>
        private static Rectangle GetWindowBounds(IntPtr handle)
        {
            RECT rect;
            var succeeded = GetWindowRect(handle, &rect);

            if (succeeded == FALSE)
            {
                ThrowExternalExceptionForLastError(nameof(GetWindowRect));
            }

            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

        /// <summary>Geta a value that indicates whether a native window is active.</summary>
        /// <param name="handle">A handle to the native window to check.</param>
        /// <returns>A value that indicates whether <paramref name="handle" /> is active.</returns>
        private static bool IsWindowActive(IntPtr handle)
        {
            var activeWindow = GetActiveWindow();
            return activeWindow == handle;
        }
        #endregion

        #region Methods
        /// <summary>Creates a <c>HWND</c> for the instance.</summary>
        /// <returns>A <c>HWND</c> for the created native window.</returns>
        /// <exception cref="ExternalException">The call to <see cref="CreateWindowEx(uint, char*, char*, uint, int, int, int, int, IntPtr, IntPtr, IntPtr, void*)" /> failed.</exception>
        private IntPtr CreateWindowHandle()
        {
            _state.AssertNotDisposedOrDisposing();

            IntPtr hWnd;

            var windowStyle = WS_OVERLAPPEDWINDOW;

            if (_windowState == WindowState.Minimized)
            {
                windowStyle |= WS_MAXIMIZE;
            }
            else if (_windowState == WindowState.Maximized)
            {
                windowStyle |= WS_MINIMIZE;
            }
            else
            {
                Assert(_windowState == WindowState.Restored, Resources.ArgumentOutOfRangeExceptionMessage, nameof(WindowState), _windowState);
            }

            if (_isEnabled == false)
            {
                windowStyle |= WS_DISABLED;
            }

            if (_isVisible)
            {
                windowStyle |= WS_VISIBLE;
            }

            var windowStyleEx = WS_EX_OVERLAPPEDWINDOW;

            if (_flowDirection == FlowDirection.RightToLeft)
            {
                windowStyleEx |= WS_EX_LAYOUTRTL;
            }

            if (_readingDirection == ReadingDirection.RightToLeft)
            {
                windowStyleEx |= WS_EX_RIGHT | WS_EX_RTLREADING | WS_EX_LEFTSCROLLBAR;
            }

            fixed (char* lpWindowName = _title)
            {
                hWnd = CreateWindowEx(
                    windowStyleEx,
                    (char*)_windowProvider.ClassAtom,
                    lpWindowName,
                    windowStyle,
                    X: float.IsNaN(Bounds.X) ? CW_USEDEFAULT : (int)Bounds.X,
                    Y: float.IsNaN(Bounds.Y) ? CW_USEDEFAULT : (int)Bounds.Y,
                    nWidth: float.IsNaN(Bounds.Width) ? CW_USEDEFAULT : (int)Bounds.Width,
                    nHeight: float.IsNaN(Bounds.Height) ? CW_USEDEFAULT : (int)Bounds.Height,
                    hWndParent: default,
                    hMenu: default,
                    hInstance: EntryPointModule,
                    lpParam: GCHandle.ToIntPtr(_windowProvider.NativeHandle).ToPointer()
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
        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                // We are only allowed to dispose of the window handle from the parent
                // thread. So, if we are on the wrong thread, we will close the window
                // and call DisposeWindowHandle from the appropriate thread.

                if (Thread.CurrentThread != _parentThread)
                {
                    Close();
                }
                else
                {
                    DisposeWindowHandle();
                }
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the <c>HWND</c> for the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="DestroyWindow(IntPtr)" /> failed.</exception>
        private void DisposeWindowHandle()
        {
            Assert(Thread.CurrentThread == _parentThread, Resources.InvalidOperationExceptionMessage, nameof(Thread.CurrentThread), Thread.CurrentThread);
            _state.AssertDisposing();

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
        private IntPtr HandleWmActivate(UIntPtr wParam)
        {
            _isActive = LOWORD(wParam) != WA_INACTIVE;
            return IntPtr.Zero;
        }

        /// <summary>Handles the <see cref="WM_CLOSE" /> message.</summary>
        /// <returns>0</returns>
        private IntPtr HandleWmClose()
        {
            // If we are already disposing, then Dispose is happening on some other thread
            // and Close was called in order for us to continue disposal on the parent thread.
            // Otherwise, this is a normal close call and we should ensure we step through the
            // various states properly.

            if (_state == Disposing)
            {
                DisposeWindowHandle();
            }
            else
            {
                Dispose();
            }
            return IntPtr.Zero;
        }

        /// <summary>Handles the <see cref="WM_DESTROY" /> message.</summary>
        /// <returns>0</returns>
        private IntPtr HandleWmDestroy()
        {
            // We handle this here to ensure we transition to the appropriate state in the case
            // an end-user called DestroyWindow themselves. The assumption here is that this was
            // done "properly" if we are Disposing, in which case we don't need to do anything.
            // Otherwise, this was triggered externally and we should just switch the state to
            // be disposed.

            if (_state != Disposing)
            {
                _state.Transition(to: Disposed);
            }
            return IntPtr.Zero;
        }

        /// <summary>Handles the <see cref="WM_ENABLE" /> message.</summary>
        /// <param name="wParam">A value that indicates whether the instance is being enabled or disabled.</param>
        /// <returns>0</returns>
        private IntPtr HandleWmEnable(UIntPtr wParam)
        {
            _isEnabled = wParam != (UIntPtr)FALSE;
            return IntPtr.Zero;
        }

        /// <summary>Handles the <see cref="WM_MOVE" /> message.</summary>
        /// <param name="lParam">A value that represents the x and y coordinates of the upper-left corner of the client area of the window.</param>
        /// <returns>0</returns>
        private IntPtr HandleWmMove(IntPtr lParam)
        {
            var location = new Vector2(x: LOWORD(lParam), y: HIWORD(lParam));
            _bounds = _bounds.WithLocation(location);
            return IntPtr.Zero;
        }

        /// <summary>Handles the <see cref="WM_SETTEXT" /> message.</summary>
        /// <param name="wParam">This parameter is not used.</param>
        /// <param name="lParam">A value that represents a pointer to the new window text.</param>
        /// <returns>A value dependent on the default processing for <see cref="WM_SETTEXT" />.</returns>
        private IntPtr HandleWmSetText(UIntPtr wParam, IntPtr lParam)
        {
            var result = DefWindowProc(_handle.Value, WM_SETTEXT, wParam, lParam);

            if (result == (IntPtr)TRUE)
            {
                // We only need to update the title if the text was set
                _title = Marshal.PtrToStringUni(lParam);
            }

            return result;
        }

        /// <summary>Handles the <see cref="WM_SHOWWINDOW" /> message.</summary>
        /// <param name="wParam">A value that indicates whether the window is being shown or hidden.</param>
        /// <returns>0</returns>
        private IntPtr HandleWmShowWindow(UIntPtr wParam)
        {
            _isVisible = LOWORD(wParam) != FALSE;
            return IntPtr.Zero;
        }

        /// <summary>Handles the <see cref="WM_SIZE" /> message.</summary>
        /// <param name="wParam">A value that represents the state of the window.</param>
        /// <param name="lParam">A value that represents the width and height of the client area of the window.</param>
        /// <returns>0</returns>
        private IntPtr HandleWmSize(UIntPtr wParam, IntPtr lParam)
        {
            _windowState = (WindowState)(uint)wParam;
            Assert(Enum.IsDefined(typeof(WindowState), _windowState), Resources.ArgumentOutOfRangeExceptionMessage, nameof(wParam), wParam);

            var size = new Vector2(x: LOWORD(lParam), y: HIWORD(lParam));
            _bounds = _bounds.WithSize(size);
            return IntPtr.Zero;
        }

        /// <summary>Processes a window message sent to the instance.</summary>
        /// <param name="Msg">The message to be processed.</param>
        /// <param name="wParam">The first parameter of the message.</param>
        /// <param name="lParam">The second parameter of the message.</param>
        /// <returns>A value that varies based on the exact message that was processed.</returns>
        internal IntPtr ProcessWindowMessage(uint Msg, UIntPtr wParam, IntPtr lParam)
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
                    return HandleWmSize(wParam, lParam);
                }

                case WM_ACTIVATE:
                {
                    return HandleWmActivate(wParam);
                }

                case WM_ENABLE:
                {
                    return HandleWmEnable(wParam);
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
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="SetForegroundWindow(IntPtr)" /> failed.</exception>
        public void Activate()
        {
            var succeeded = TryActivate();

            if (succeeded == false)
            {
                ThrowExternalExceptionForLastError(nameof(SetForegroundWindow));
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
            _state.ThrowIfDisposedOrDisposing();

            if (_handle.IsValueCreated)
            {
                SendMessage(_handle.Value, WM_CLOSE, wParam: UIntPtr.Zero, lParam: IntPtr.Zero);
            }
        }

        /// <summary>Disables the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Disable()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isEnabled)
            {
                EnableWindow(_handle.Value, FALSE);
            }
        }

        /// <summary>Enables the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Enable()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isEnabled == false)
            {
                EnableWindow(_handle.Value, TRUE);
            }
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Hide()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible)
            {
                ShowWindow(_handle.Value, SW_HIDE);
            }
        }

        /// <summary>Maximizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Maximize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Maximized)
            {
                ShowWindow(_handle.Value, SW_MAXIMIZE);
            }
        }

        /// <summary>Minimizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Minimize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Minimized)
            {
                ShowWindow(_handle.Value, SW_MINIMIZE);
            }
        }

        /// <summary>Restores the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Restore()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Restored)
            {
                ShowWindow(_handle.Value, SW_RESTORE);
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Show()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible == false)
            {
                ShowWindow(_handle.Value, SW_SHOW);
            }
        }

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public bool TryActivate()
        {
            _state.ThrowIfDisposedOrDisposing();

            return _isActive || (SetForegroundWindow(_handle.Value) != FALSE);
        }
        #endregion
    }
}
