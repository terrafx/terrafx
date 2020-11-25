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
        public override IntPtr SurfaceContextHandle => EntryPointModule;

        /// <inheritdoc />
        public override IntPtr SurfaceHandle => _handle.Value;

        /// <inheritdoc />
        public override GraphicsSurfaceKind SurfaceKind => GraphicsSurfaceKind.Win32;

        /// <inheritdoc />
        public override string Title => _title;

        /// <inheritdoc />
        public override WindowState WindowState => _windowState;

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Activate() => ThrowExternalExceptionIfFalse(TryActivate(), nameof(SetForegroundWindow));

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
                _ = SendMessageW(SurfaceHandle, WM_CLOSE, wParam: 0, lParam: 0);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Disable()
        {
            if (_isEnabled)
            {
                _ = EnableWindow(SurfaceHandle, FALSE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Enable()
        {
            if (_isEnabled == false)
            {
                _ = EnableWindow(SurfaceHandle, TRUE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Hide()
        {
            if (_isVisible)
            {
                _ = ShowWindow(SurfaceHandle, SW_HIDE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Maximized" /> but the instance has already been disposed.</exception>
        public override void Maximize()
        {
            if (_windowState != WindowState.Maximized)
            {
                _ = ShowWindow(SurfaceHandle, SW_MAXIMIZE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Minimized" /> but the instance has already been disposed.</exception>
        public override void Minimize()
        {
            if (_windowState != WindowState.Minimized)
            {
                _ = ShowWindow(SurfaceHandle, SW_MINIMIZE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void Relocate(Vector2 location)
        {
            if (_bounds.Location != location)
            {
                var rect = new RECT {
                    left = (int)location.X,
                    top = (int)location.Y,
                    right = (int)_bounds.Width,
                    bottom = (int)_bounds.Height,
                };

                ThrowExternalExceptionIfFalse(AdjustWindowRectEx(&rect, WS_OVERLAPPEDWINDOW, bMenu: FALSE, WS_EX_OVERLAPPEDWINDOW), nameof(AdjustWindowRectEx));
                ThrowExternalExceptionIfFalse(MoveWindow(SurfaceHandle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, bRepaint: TRUE), nameof(MoveWindow));
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void Resize(Vector2 size)
        {
            if (_bounds.Size != size)
            {
                var rect = new RECT {
                    left = (int)_bounds.X,
                    top = (int)_bounds.Y,
                    right = (int)(_bounds.X + size.X),
                    bottom = (int)(_bounds.Y + size.Y),
                };

                ThrowExternalExceptionIfFalse(AdjustWindowRectEx(&rect, WS_OVERLAPPEDWINDOW, bMenu: FALSE, WS_EX_OVERLAPPEDWINDOW), nameof(AdjustWindowRectEx));
                ThrowExternalExceptionIfFalse(MoveWindow(SurfaceHandle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, bRepaint: TRUE), nameof(MoveWindow));
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Restored" /> but the instance has already been disposed.</exception>
        public override void Restore()
        {
            if (_windowState != WindowState.Restored)
            {
                _ = ShowWindow(SurfaceHandle, SW_RESTORE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void SetTitle(string title)
        {
            if (_title != title)
            {
                fixed (char* pTitle = title)
                {
                    ThrowExternalExceptionIfFalse(SetWindowTextW(SurfaceHandle, (ushort*)pTitle), nameof(SetWindowTextW));
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Show()
        {
            if (_isVisible == false)
            {
                _ = ShowWindow(SurfaceHandle, SW_SHOW);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override bool TryActivate() => _isActive || (SetForegroundWindow(SurfaceHandle) != FALSE);

        internal nint ProcessWindowMessage(uint msg, nuint wParam, nint lParam)
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
                    X: CW_USEDEFAULT,
                    Y: CW_USEDEFAULT,
                    nWidth: CW_USEDEFAULT,
                    nHeight: CW_USEDEFAULT,
                    hWndParent: default,
                    hMenu: default,
                    hInstance: EntryPointModule,
                    lpParam: GCHandle.ToIntPtr(((Win32WindowProvider)WindowProvider).NativeHandle).ToPointer()
                );
            }
            ThrowExternalExceptionIfZero(hWnd, nameof(CreateWindowExW));

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
                ThrowExternalExceptionIfFalse(DestroyWindow(_handle.Value), nameof(DestroyWindow));
            }
        }

        private nint HandleWmActivate(nuint wParam)
        {
            _isActive = LOWORD((uint)wParam) != WA_INACTIVE;
            return 0;
        }

        private nint HandleWmClose()
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
            return 0;
        }

        private nint HandleWmDestroy()
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
            return 0;
        }

        private nint HandleWmEnable(nuint wParam)
        {
            _isEnabled = wParam != FALSE;
            return 0;
        }

        private nint HandleWmMove(nint lParam)
        {
            var previousLocation = _bounds.Location;
            var currentLocation = new Vector2(x: LOWORD((uint)lParam), y: HIWORD((uint)lParam));

            _bounds = _bounds.WithLocation(currentLocation);
            OnLocationChanged(previousLocation, currentLocation);

            return 0;
        }

        private nint HandleWmSetText(nuint wParam, nint lParam)
        {
            var result = DefWindowProcW(SurfaceHandle, WM_SETTEXT, wParam, lParam);

            if (result == TRUE)
            {
                // We only need to update the title if the text was set
                _title = Marshal.PtrToStringUni(lParam)!;
            }

            return result;
        }

        private nint HandleWmShowWindow(nuint wParam)
        {
            _isVisible = LOWORD((uint)wParam) != FALSE;
            return 0;
        }

        private nint HandleWmSize(nuint wParam, nint lParam)
        {
            _windowState = (WindowState)(uint)wParam;
            Assert(Enum.IsDefined(typeof(WindowState), _windowState), Resources.ArgumentOutOfRangeExceptionMessage, nameof(wParam), wParam);

            var previousSize = _bounds.Size;
            var currentSize = new Vector2(x: LOWORD((uint)lParam), y: HIWORD((uint)lParam));

            _bounds = _bounds.WithSize(currentSize);
            OnSizeChanged(previousSize, currentSize);

            return 0;
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
            if (SizeChanged is not null)
            {
                var eventArgs = new PropertyChangedEventArgs<Vector2>(previousSize, currentSize);
                SizeChanged(this, eventArgs);
            }
        }
    }
}
