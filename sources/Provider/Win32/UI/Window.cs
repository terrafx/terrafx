// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
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
        private readonly Lazy<IntPtr> _handle;
        private readonly Thread _parentThread;
        private readonly PropertySet _properties;
        private readonly WindowProvider _windowProvider;
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;

        private string _title;
        private Rectangle _bounds;
        private WindowState _windowState;
        private State _state;
        private bool _isActive;
        private bool _isEnabled;
        private bool _isVisible;

        internal Window(WindowProvider windowProvider)
        {
            _handle = new Lazy<IntPtr>(CreateWindowHandle, isThreadSafe: true);

            _parentThread = Thread.CurrentThread;
            _properties = new PropertySet();
            _title = typeof(Window).FullName!;
            _bounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);
            _flowDirection = FlowDirection.TopToBottom;
            _readingDirection = ReadingDirection.LeftToRight;

            _windowProvider = windowProvider;
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="Window" /> class.</summary>
        ~Window()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        public Rectangle Bounds => _bounds;

        /// <summary>Gets <see cref="FlowDirection" /> for the instance.</summary>
        public FlowDirection FlowDirection => _flowDirection;

        /// <summary>Gets the handle for the instance.</summary>
        public IntPtr Handle => _state.IsNotDisposedOrDisposing ? _handle.Value : IntPtr.Zero;

        /// <summary>Gets a value that indicates whether the instance is the active window.</summary>
        public bool IsActive => _isActive;

        /// <summary>Gets a value that indicates whether the instance is enabled.</summary>
        public bool IsEnabled => _isEnabled;

        /// <summary>Gets a value that indicates whether the instance is visible.</summary>
        public bool IsVisible => _isVisible;

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread => _parentThread;

        /// <summary>Gets the <see cref="IPropertySet" /> for the instance.</summary>
        public IPropertySet Properties => _properties;

        /// <summary>Gets the <see cref="ReadingDirection" /> for the instance.</summary>
        public ReadingDirection ReadingDirection => _readingDirection;

        /// <summary>Gets the title for the instance.</summary>
        public string Title => _title;

        /// <summary>Gets the <see cref="IWindowProvider" /> for the instance.</summary>
        public IWindowProvider WindowProvider => _windowProvider;

        /// <summary>Gets the <see cref="WindowState" /> for the instance.</summary>
        public WindowState WindowState => _windowState;

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

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
        /// <para>This method can be called from any thread.</para>
        /// <para>This method does nothing if the underlying <c>HWND</c> has not been created.</para>
        /// </remarks>
        public void Close()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_handle.IsValueCreated)
            {
                _ = SendMessage(_handle.Value, WM_CLOSE, wParam: UIntPtr.Zero, lParam: IntPtr.Zero);
            }
        }

        /// <summary>Creates a new <see cref="IGraphicsSurface" /> for the instance.</summary>
        /// <param name="bufferCount">The number of buffers created for the instance.</param>
        /// <returns>A new <see cref="IGraphicsSurface" /> for the instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferCount" /> is less than or equal to zero.</exception>
        public IGraphicsSurface CreateGraphicsSurface(int bufferCount) => new GraphicsSurface(this, _bounds.Size, bufferCount);

        /// <summary>Disables the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Disable()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isEnabled)
            {
                _ = EnableWindow(_handle.Value, FALSE);
            }
        }

        /// <summary>Enables the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Enable()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isEnabled == false)
            {
                _ = EnableWindow(_handle.Value, TRUE);
            }
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Hide()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible)
            {
                _ = ShowWindow(_handle.Value, SW_HIDE);
            }
        }

        /// <summary>Maximizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Maximize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Maximized)
            {
                _ = ShowWindow(_handle.Value, SW_MAXIMIZE);
            }
        }

        /// <summary>Minimizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Minimize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Minimized)
            {
                _ = ShowWindow(_handle.Value, SW_MINIMIZE);
            }
        }

        /// <summary>Restores the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Restore()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Restored)
            {
                _ = ShowWindow(_handle.Value, SW_RESTORE);
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Show()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible == false)
            {
                _ = ShowWindow(_handle.Value, SW_SHOW);
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

        internal IntPtr ProcessWindowMessage(uint msg, UIntPtr wParam, IntPtr lParam)
        {
            ThrowIfNotThread(_parentThread);

            switch (msg)
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
                    return DefWindowProc(_handle.Value, msg, wParam, lParam);
                }
            }
        }

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

        private static bool IsWindowActive(IntPtr handle)
        {
            var activeWindow = GetActiveWindow();
            return activeWindow == handle;
        }

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

        private IntPtr HandleWmActivate(UIntPtr wParam)
        {
            _isActive = LOWORD(wParam) != WA_INACTIVE;
            return IntPtr.Zero;
        }

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

        private IntPtr HandleWmDestroy()
        {
            // We handle this here to ensure we transition to the appropriate state in the case
            // an end-user called DestroyWindow themselves. The assumption here is that this was
            // done "properly" if we are Disposing, in which case we don't need to do anything.
            // Otherwise, this was triggered externally and we should just switch the state to
            // be disposed.

            if (_state != Disposing)
            {
                _ = _state.Transition(to: Disposed);
            }
            return IntPtr.Zero;
        }

        private IntPtr HandleWmEnable(UIntPtr wParam)
        {
            _isEnabled = wParam != (UIntPtr)FALSE;
            return IntPtr.Zero;
        }

        private IntPtr HandleWmMove(IntPtr lParam)
        {
            var location = new Vector2(x: LOWORD(lParam), y: HIWORD(lParam));
            _bounds = _bounds.WithLocation(location);
            return IntPtr.Zero;
        }

        private IntPtr HandleWmSetText(UIntPtr wParam, IntPtr lParam)
        {
            var result = DefWindowProc(_handle.Value, WM_SETTEXT, wParam, lParam);

            if (result == (IntPtr)TRUE)
            {
                // We only need to update the title if the text was set
                _title = Marshal.PtrToStringUni(lParam)!;
            }

            return result;
        }

        private IntPtr HandleWmShowWindow(UIntPtr wParam)
        {
            _isVisible = LOWORD(wParam) != FALSE;
            return IntPtr.Zero;
        }

        private IntPtr HandleWmSize(UIntPtr wParam, IntPtr lParam)
        {
            _windowState = (WindowState)(uint)wParam;
            Assert(Enum.IsDefined(typeof(WindowState), _windowState), Resources.ArgumentOutOfRangeExceptionMessage, nameof(wParam), wParam);

            var size = new Vector2(x: LOWORD(lParam), y: HIWORD(lParam));
            _bounds = _bounds.WithSize(size);
            return IntPtr.Zero;
        }
    }
}
