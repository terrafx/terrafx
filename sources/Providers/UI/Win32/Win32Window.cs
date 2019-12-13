// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.UI.Providers.Win32.HelperUtilities;
using static TerraFX.UI.Providers.Win32.Win32WindowProvider;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.UI.Providers.Win32
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class Win32Window : Window
    {
        private readonly PropertySet _properties;
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;

        private ValueLazy<HWND> _handle;
        private string _title;
        private Rectangle _bounds;
        private WindowState _windowState;
        private State _state;
        private bool _isActive;
        private bool _isEnabled;
        private bool _isVisible;

        internal Win32Window(Win32WindowProvider windowProvider)
            : base(windowProvider, Thread.CurrentThread)
        {
            _handle = new ValueLazy<HWND>(CreateWindowHandle);

            _properties = new PropertySet();
            _title = typeof(Win32Window).FullName!;
            _bounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);
            _flowDirection = FlowDirection.TopToBottom;
            _readingDirection = ReadingDirection.LeftToRight;
            _isEnabled = true;

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="Win32Window" /> class.</summary>
        ~Win32Window()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
        public override event EventHandler<PropertyChangedEventArgs<Vector2>>? LocationChanged;

        /// <inheritdoc />
        public override event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

        /// <inheritdoc />
        public override Rectangle Bounds => _bounds;

        /// <inheritdoc />
        public override FlowDirection FlowDirection => _flowDirection;

        /// <summary>Gets the handle for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public HWND Handle
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _handle.Value;
            }
        }

        /// <inheritdoc />
        public override bool IsActive => _isActive;

        /// <inheritdoc />
        public override bool IsEnabled => _isEnabled;

        /// <inheritdoc />
        public override bool IsVisible => _isVisible;

        /// <inheritdoc />
        public override IPropertySet Properties => _properties;

        /// <inheritdoc />
        public override ReadingDirection ReadingDirection => _readingDirection;

        /// <inheritdoc />
        public override string Title => _title;

        /// <inheritdoc />
        public override WindowState WindowState => _windowState;

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Activate()
        {
            var succeeded = TryActivate();

            if (succeeded == false)
            {
                ThrowExternalExceptionForLastError(nameof(SetForegroundWindow));
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <remarks>
        ///   <para>This method can be called from any thread.</para>
        ///   <para>This method does nothing if the underlying <c>HWND</c> has not been created.</para>
        /// </remarks>
        public override void Close()
        {
            if (_handle.IsCreated)
            {
                _ = SendMessageW(Handle, WM_CLOSE, wParam: UIntPtr.Zero, lParam: IntPtr.Zero);
            }
        }

        /// <inheritdoc />
        public override IGraphicsSurface CreateGraphicsSurface(int bufferCount)
        {
            if (bufferCount <= 0)
            {
                ThrowArgumentOutOfRangeException(nameof(bufferCount), bufferCount);
            }

            return new Win32GraphicsSurface(this, bufferCount);
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Disable()
        {
            if (_isEnabled)
            {
                _ = EnableWindow(Handle, FALSE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Enable()
        {
            if (_isEnabled == false)
            {
                _ = EnableWindow(Handle, TRUE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Hide()
        {
            if (_isVisible)
            {
                _ = ShowWindow(Handle, SW_HIDE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Maximized" /> but the instance has already been disposed.</exception>
        public override void Maximize()
        {
            if (_windowState != WindowState.Maximized)
            {
                _ = ShowWindow(Handle, SW_MAXIMIZE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Minimized" /> but the instance has already been disposed.</exception>
        public override void Minimize()
        {
            if (_windowState != WindowState.Minimized)
            {
                _ = ShowWindow(Handle, SW_MINIMIZE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Restored" /> but the instance has already been disposed.</exception>
        public override void Restore()
        {
            if (_windowState != WindowState.Restored)
            {
                _ = ShowWindow(Handle, SW_RESTORE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Show()
        {
            if (_isVisible == false)
            {
                _ = ShowWindow(Handle, SW_SHOW);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override bool TryActivate()
        {
            _state.ThrowIfDisposedOrDisposing();

            return _isActive || (SetForegroundWindow(Handle) != FALSE);
        }

        internal IntPtr ProcessWindowMessage(uint msg, UIntPtr wParam, IntPtr lParam)
        {
            ThrowIfNotThread(ParentThread);

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
                    return DefWindowProcW(_handle.Value, msg, wParam, lParam);
                }
            }
        }

        private HWND CreateWindowHandle()
        {
            _state.AssertNotDisposedOrDisposing();

            HWND hWnd;

            fixed (char* lpWindowName = _title)
            {
                hWnd = CreateWindowExW(
                    WS_EX_OVERLAPPEDWINDOW,
                    (ushort*)((Win32WindowProvider)WindowProvider).ClassAtom,
                    (ushort*)lpWindowName,
                    WS_OVERLAPPEDWINDOW,
                    X: float.IsNaN(Bounds.X) ? CW_USEDEFAULT : (int)Bounds.X,
                    Y: float.IsNaN(Bounds.Y) ? CW_USEDEFAULT : (int)Bounds.Y,
                    nWidth: float.IsNaN(Bounds.Width) ? CW_USEDEFAULT : (int)Bounds.Width,
                    nHeight: float.IsNaN(Bounds.Height) ? CW_USEDEFAULT : (int)Bounds.Height,
                    hWndParent: default,
                    hMenu: default,
                    hInstance: EntryPointModule,
                    lpParam: GCHandle.ToIntPtr(((Win32WindowProvider)WindowProvider).NativeHandle).ToPointer()
                );
            }
            ThrowExternalExceptionIfZero(nameof(CreateWindowExW), hWnd);

            return hWnd;
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                // We are only allowed to dispose of the window handle from the parent
                // thread. So, if we are on the wrong thread, we will close the window
                // and call DisposeWindowHandle from the appropriate thread.

                if (Thread.CurrentThread != ParentThread)
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
            Assert(Thread.CurrentThread == ParentThread, Resources.InvalidOperationExceptionMessage, nameof(Thread.CurrentThread), Thread.CurrentThread);
            _state.AssertDisposing();

            if (_handle.IsCreated)
            {
                ThrowExternalExceptionIfFalse(nameof(DestroyWindow), DestroyWindow(_handle.Value));
            }
        }

        private IntPtr HandleWmActivate(UIntPtr wParam)
        {
            _isActive = LOWORD((uint)wParam) != WA_INACTIVE;
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
            var previousLocation = _bounds.Location;
            var currentLocation = new Vector2(x: LOWORD((uint)lParam), y: HIWORD((uint)lParam));

            _bounds = _bounds.WithLocation(currentLocation);
            OnLocationChanged(previousLocation, currentLocation);

            return IntPtr.Zero;
        }

        private IntPtr HandleWmSetText(UIntPtr wParam, IntPtr lParam)
        {
            var result = DefWindowProcW(Handle, WM_SETTEXT, wParam, lParam);

            if (result == (IntPtr)TRUE)
            {
                // We only need to update the title if the text was set
                _title = Marshal.PtrToStringUni(lParam)!;
            }

            return result;
        }

        private IntPtr HandleWmShowWindow(UIntPtr wParam)
        {
            _isVisible = LOWORD((uint)wParam) != FALSE;
            return IntPtr.Zero;
        }

        private IntPtr HandleWmSize(UIntPtr wParam, IntPtr lParam)
        {
            _windowState = (WindowState)(uint)wParam;
            Assert(Enum.IsDefined(typeof(WindowState), _windowState), Resources.ArgumentOutOfRangeExceptionMessage, nameof(wParam), wParam);

            var previousSize = _bounds.Size;
            var currentSize = new Vector2(x: LOWORD((uint)lParam), y: HIWORD((uint)lParam));

            _bounds = _bounds.WithSize(currentSize);
            OnSizeChanged(previousSize, currentSize);

            return IntPtr.Zero;
        }

        private void OnLocationChanged(Vector2 previousLocation, Vector2 currentLocation)
        {
            if (LocationChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs<Vector2>(previousLocation, currentLocation);
                LocationChanged(this, eventArgs);
            }
        }

        private void OnSizeChanged(Vector2 previousSize, Vector2 currentSize)
        {
            if (SizeChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs<Vector2>(previousSize, currentSize);
                SizeChanged(this, eventArgs);
            }
        }
    }
}
