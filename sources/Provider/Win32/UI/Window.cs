// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;
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
        private readonly Thread _parentThread;
        private readonly PropertySet _properties;
        private readonly WindowProvider _windowProvider;
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;

        private ResettableLazy<IntPtr> _handle;
        private string _title;
        private Rectangle _bounds;
        private WindowState _windowState;
        private State _state;
        private bool _isActive;
        private bool _isEnabled;
        private bool _isVisible;

        internal Window(WindowProvider windowProvider)
        {
            Assert(windowProvider != null, Resources.ArgumentNullExceptionMessage, nameof(windowProvider));

            _handle = new ResettableLazy<IntPtr>(CreateWindowHandle);

            _parentThread = Thread.CurrentThread;
            _properties = new PropertySet();
            _title = typeof(Window).FullName!;
            _bounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);
            _flowDirection = FlowDirection.TopToBottom;
            _readingDirection = ReadingDirection.LeftToRight;
            _isEnabled = true;

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
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr Handle
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _handle.Value;
            }
        }

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
        /// <exception cref="ExternalException">The call to <see cref="SetForegroundWindow(IntPtr)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
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
        ///   <para>This method can be called from any thread.</para>
        ///   <para>This method does nothing if the underlying <c>HWND</c> has not been created.</para>
        /// </remarks>
        public void Close()
        {
            if (_handle.IsCreated)
            {
                _ = SendMessage(Handle, WM_CLOSE, wParam: UIntPtr.Zero, lParam: IntPtr.Zero);
            }
        }

        /// <summary>Creates a new <see cref="IGraphicsSurface" /> for the instance.</summary>
        /// <param name="bufferCount">The number of buffers created for the instance.</param>
        /// <returns>A new <see cref="IGraphicsSurface" /> for the instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferCount" /> is less than or equal to zero.</exception>
        public IGraphicsSurface CreateGraphicsSurface(int bufferCount)
        {
            if (bufferCount <= 0)
            {
                ThrowArgumentOutOfRangeException(nameof(bufferCount), bufferCount);
            }

            return new GraphicsSurface(this, bufferCount);
        }

        /// <summary>Disables the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>true</c> but the instance has already been disposed.</exception>
        public void Disable()
        {
            if (_isEnabled)
            {
                _ = EnableWindow(Handle, FALSE);
            }
        }

        /// <summary>Enables the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>false</c> but the instance has already been disposed.</exception>
        public void Enable()
        {
            if (_isEnabled == false)
            {
                _ = EnableWindow(Handle, TRUE);
            }
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>true</c> but the instance has already been disposed.</exception>
        public void Hide()
        {
            if (_isVisible)
            {
                _ = ShowWindow(Handle, SW_HIDE);
            }
        }

        /// <summary>Maximizes the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Maximized" /> but the instance has already been disposed.</exception>
        public void Maximize()
        {
            if (_windowState != WindowState.Maximized)
            {
                _ = ShowWindow(Handle, SW_MAXIMIZE);
            }
        }

        /// <summary>Minimizes the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Minimized" /> but the instance has already been disposed.</exception>
        public void Minimize()
        {
            if (_windowState != WindowState.Minimized)
            {
                _ = ShowWindow(Handle, SW_MINIMIZE);
            }
        }

        /// <summary>Restores the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Restored" /> but the instance has already been disposed.</exception>
        public void Restore()
        {
            if (_windowState != WindowState.Restored)
            {
                _ = ShowWindow(Handle, SW_RESTORE);
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>false</c> but the instance has already been disposed.</exception>
        public void Show()
        {
            if (_isVisible == false)
            {
                _ = ShowWindow(Handle, SW_SHOW);
            }
        }

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public bool TryActivate()
        {
            _state.ThrowIfDisposedOrDisposing();

            return _isActive || (SetForegroundWindow(Handle) != FALSE);
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

        private IntPtr CreateWindowHandle()
        {
            _state.AssertNotDisposedOrDisposing();

            IntPtr hWnd;

            fixed (char* lpWindowName = _title)
            {
                hWnd = CreateWindowEx(
                    WS_EX_OVERLAPPEDWINDOW,
                    (char*)_windowProvider.ClassAtom,
                    lpWindowName,
                    WS_OVERLAPPEDWINDOW,
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

            if (priorState < Disposing)
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

            if (_handle.IsCreated)
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
            var result = DefWindowProc(Handle, WM_SETTEXT, wParam, lParam);

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
