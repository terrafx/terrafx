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
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;

        private ValueLazy<HWND> _handle;
        private ValueLazy<PropertySet> _properties;

        private string _title;
        private Rectangle _bounds;
        private Rectangle _clientBounds;
        private uint _extendedStyle;
        private uint _style;
        private WindowState _windowState;
        private bool _isActive;

        private State _state;

        internal Win32Window(Win32WindowProvider windowProvider)
            : base(windowProvider, Thread.CurrentThread)
        {
            _flowDirection = FlowDirection.TopToBottom;
            _readingDirection = ReadingDirection.LeftToRight;

            _handle = new ValueLazy<HWND>(CreateWindowHandle);
            _properties = new ValueLazy<PropertySet>(CreateProperties);

            _title = typeof(Win32Window).FullName!;
            _bounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);
            _clientBounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);
            _extendedStyle = WS_EX_OVERLAPPEDWINDOW;
            _style = WS_OVERLAPPEDWINDOW;

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="Win32Window" /> class.</summary>
        ~Win32Window()
            => Dispose(isDisposing: false);

        /// <inheritdoc />
        public override event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientLocationChanged;

        /// <inheritdoc />
        public override event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientSizeChanged;

        /// <inheritdoc />
        public override event EventHandler<PropertyChangedEventArgs<Vector2>>? LocationChanged;

        /// <inheritdoc />
        public override event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

        /// <inheritdoc />
        public override Rectangle Bounds
            => _bounds;

        /// <inheritdoc />
        public override Rectangle ClientBounds
            => _clientBounds;

        /// <inheritdoc />
        public override FlowDirection FlowDirection
            => _flowDirection;

        /// <summary>Gets the underlying <c>HWND</c> for the window.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr Handle
            => _handle.Value;

        /// <inheritdoc />
        public override bool IsActive
            => _isActive;

        /// <inheritdoc />
        public override bool IsEnabled
            => (_style & WS_DISABLED) == 0;

        /// <inheritdoc />
        public override bool IsVisible
            => (_style & WS_VISIBLE) != 0;

        /// <inheritdoc />
        public override IPropertySet Properties
            => _properties.Value;

        /// <inheritdoc />
        public override ReadingDirection ReadingDirection
            => _readingDirection;

        /// <inheritdoc />
        public override string Title
            => _title;

        /// <inheritdoc cref="Window.WindowProvider" />
        public new Win32WindowProvider WindowProvider
            => (Win32WindowProvider)base.WindowProvider;

        /// <inheritdoc />
        public override WindowState WindowState
            => _windowState;

        /// <inheritdoc />
        protected override IntPtr SurfaceContextHandle
            => EntryPointModule;

        /// <inheritdoc />
        protected override IntPtr SurfaceHandle
            => Handle;

        /// <inheritdoc />
        protected override GraphicsSurfaceKind SurfaceKind
            => GraphicsSurfaceKind.Win32;

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Activate()
            => ThrowExternalExceptionIf(TryActivate() == false, nameof(SetForegroundWindow));

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
                _ = SendMessageW(Handle, WM_CLOSE, wParam: 0, lParam: 0);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Disable()
        {
            if (IsEnabled)
            {
                _ = EnableWindow(Handle, FALSE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Enable()
        {
            if (!IsEnabled)
            {
                _ = EnableWindow(Handle, TRUE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Hide()
        {
            if (WindowState != WindowState.Hidden)
            {
                _ = ShowWindow(Handle, SW_HIDE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Maximized" /> but the instance has already been disposed.</exception>
        public override void Maximize()
        {
            if (WindowState != WindowState.Maximized)
            {
                _ = ShowWindow(Handle, SW_MAXIMIZE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Minimized" /> but the instance has already been disposed.</exception>
        public override void Minimize()
        {
            if (WindowState != WindowState.Minimized)
            {
                _ = ShowWindow(Handle, SW_MINIMIZE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void Relocate(Vector2 location)
        {
            if (_bounds.Location != location)
            {
                ThrowExternalExceptionIfFalse(MoveWindow(
                    Handle,
                    (int)location.X,
                    (int)location.Y,
                    (int)_bounds.Width,
                    (int)_bounds.Height,
                    bRepaint: TRUE
                ), nameof(MoveWindow));
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void RelocateClient(Vector2 location)
        {
            if (_clientBounds.Location != location)
            {
                var clientSize = ClientSize;

                var rect = new RECT {
                    left = (int)location.X,
                    top = (int)location.Y,
                    right = (int)(location.X + clientSize.X),
                    bottom = (int)(location.Y + clientSize.Y),
                };

                ThrowExternalExceptionIfFalse(AdjustWindowRectEx(&rect, _style, bMenu: FALSE, _extendedStyle), nameof(AdjustWindowRectEx));
                ThrowExternalExceptionIfFalse(MoveWindow(Handle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, bRepaint: TRUE), nameof(MoveWindow));
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void Resize(Vector2 size)
        {
            if (_bounds.Size != size)
            {
                ThrowExternalExceptionIfFalse(MoveWindow(
                    Handle,
                    (int)_bounds.X,
                    (int)_bounds.Y,
                    (int)size.X,
                    (int)size.Y,
                    bRepaint: TRUE
                ), nameof(MoveWindow));
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void ResizeClient(Vector2 size)
        {
            if (_clientBounds.Size != size)
            {
                var clientLocation = ClientLocation;

                var rect = new RECT {
                    left = (int)clientLocation.X,
                    top = (int)clientLocation.Y,
                    right = (int)(clientLocation.X + size.X),
                    bottom = (int)(clientLocation.Y + size.Y),
                };

                ThrowExternalExceptionIfFalse(AdjustWindowRectEx(&rect, _style, bMenu: FALSE, _extendedStyle), nameof(AdjustWindowRectEx));
                ThrowExternalExceptionIfFalse(MoveWindow(Handle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, bRepaint: TRUE), nameof(MoveWindow));
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Normal" /> but the instance has already been disposed.</exception>
        public override void Restore()
        {
            if (WindowState != WindowState.Normal)
            {
                _ = ShowWindow(Handle, SW_RESTORE);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override void SetTitle(string title)
        {
            if (_title != title)
            {
                title ??= string.Empty;

                fixed (char* pTitle = title)
                {
                    ThrowExternalExceptionIfFalse(SetWindowTextW(Handle, (ushort*)pTitle), nameof(SetWindowTextW));
                }
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Show()
        {
            if (WindowState == WindowState.Hidden)
            {
                _ = ShowWindow(Handle, SW_SHOW);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override bool TryActivate()
            => _isActive || (SetForegroundWindow(Handle) != FALSE);

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

                case WM_WINDOWPOSCHANGED:
                {
                    return HandleWmWindowPosChanged(wParam, lParam);
                }

                case WM_STYLECHANGED:
                {
                    return HandleWmStyleChanged(wParam, lParam);
                }

                default:
                {
                    return DefWindowProcW(_handle.Value, msg, wParam, lParam);
                }
            }
        }

        private PropertySet CreateProperties()
            => new PropertySet();

        private HWND CreateWindowHandle()
        {
            _state.AssertNotDisposedOrDisposing();

            HWND hWnd;

            fixed (char* lpWindowName = _title)
            {
                hWnd = CreateWindowExW(
                    _extendedStyle,
                    (ushort*)WindowProvider.ClassAtom,
                    (ushort*)lpWindowName,
                    _style,
                    X: CW_USEDEFAULT,
                    Y: CW_USEDEFAULT,
                    nWidth: CW_USEDEFAULT,
                    nHeight: CW_USEDEFAULT,
                    hWndParent: default,
                    hMenu: default,
                    hInstance: EntryPointModule,
                    lpParam: GCHandle.ToIntPtr(GCHandle.Alloc(this, GCHandleType.Normal)).ToPointer()
                );
            }
            ThrowExternalExceptionIf(hWnd == null, nameof(CreateWindowExW));

            // Set the initial bounds so that resizing and relocating before showing work as expected
            // For GetClientRect, it always returns the position as (0, 0) annd so we need to remap
            // the points to screen coordinates to ensure we are tracking the right location.

            RECT clientRect;

            ThrowExternalExceptionIfFalse(GetClientRect(hWnd, &clientRect), nameof(GetClientRect));
            ThrowExternalExceptionIfFalse(MapWindowPoints(hWnd, HWND_DESKTOP, (POINT*)&clientRect, 2), nameof(MapWindowPoints));

            _clientBounds = new Rectangle(
                clientRect.left,
                clientRect.top,
                clientRect.right - clientRect.left,
                clientRect.bottom - clientRect.top
            );

            RECT rect;

            ThrowExternalExceptionIfFalse(GetWindowRect(hWnd, &rect), nameof(GetWindowRect));

            _bounds = new Rectangle(
                rect.left,
                rect.top,
                rect.right - clientRect.left,
                rect.bottom - clientRect.top
            );

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
            if (wParam != FALSE)
            {
                _style &= ~(uint)WS_DISABLED;
            }
            else
            {
                _style |= WS_DISABLED;
            }
            return 0;
        }

        private nint HandleWmMove(nint lParam)
        {
            var previousClientLocation = _clientBounds.Location;
            var currentClientLocation = new Vector2(x: LOWORD((uint)lParam), y: HIWORD((uint)lParam));

            _clientBounds = _clientBounds.WithLocation(currentClientLocation);
            OnClientLocationChanged(previousClientLocation, currentClientLocation);

            return 0;
        }

        private nint HandleWmSetText(nuint wParam, nint lParam)
        {
            var result = DefWindowProcW(_handle.Value, WM_SETTEXT, wParam, lParam);

            if (result == TRUE)
            {
                // We only need to update the title if the text was set
                _title = Marshal.PtrToStringUni(lParam)!;
            }

            return result;
        }

        private nint HandleWmShowWindow(nuint wParam)
        {
            if (LOWORD((uint)wParam) != FALSE)
            {
                _style |= WS_VISIBLE;
            }
            else
            {
                _style &= ~(uint)WS_VISIBLE;
            }
            return 0;
        }

        private nint HandleWmSize(nuint wParam, nint lParam)
        {
            _windowState = (WindowState)(uint)wParam;
            Assert(Enum.IsDefined(typeof(WindowState), _windowState), Resources.ArgumentOutOfRangeExceptionMessage, nameof(wParam), wParam);

            var previousClientSize = _clientBounds.Size;
            var currentClientSize = new Vector2(x: LOWORD((uint)lParam), y: HIWORD((uint)lParam));

            _clientBounds = _clientBounds.WithSize(currentClientSize);
            OnClientSizeChanged(previousClientSize, currentClientSize);

            return 0;
        }

        private nint HandleWmStyleChanged(nuint wParam, nint lParam)
        {
            var styleStruct = (STYLESTRUCT*)lParam;

            if ((nint)wParam == GWL_EXSTYLE)
            {
                _extendedStyle = styleStruct->styleNew;
            }
            else if ((nint)wParam == GWL_STYLE)
            {
                _style = styleStruct->styleNew;
            }

            return 0;
        }

        private nint HandleWmWindowPosChanged(nuint wParam, nint lParam)
        {
            var result = DefWindowProc(_handle.Value, WM_WINDOWPOSCHANGED, wParam, lParam);

            var windowPos = (WINDOWPOS*)lParam;

            var previousLocation = _bounds.Location;
            var previousSize = _bounds.Size;

            var currentLocation = new Vector2(windowPos->x, windowPos->y);
            var currentSize = new Vector2(windowPos->cx, windowPos->cy);

            _bounds = new Rectangle(currentLocation, currentSize);

            OnLocationChanged(previousLocation, currentLocation);
            OnSizeChanged(previousSize, currentSize);

            return result;
        }

        private void OnClientLocationChanged(Vector2 previousClientLocation, Vector2 currentClientLocation)
        {
            if ((ClientLocationChanged != null) && (previousClientLocation != currentClientLocation))
            {
                var eventArgs = new PropertyChangedEventArgs<Vector2>(previousClientLocation, currentClientLocation);
                ClientLocationChanged(this, eventArgs);
            }
        }

        private void OnClientSizeChanged(Vector2 previousClientSize, Vector2 currentClientSize)
        {
            if ((ClientSizeChanged is not null) && (previousClientSize != currentClientSize))
            {
                var eventArgs = new PropertyChangedEventArgs<Vector2>(previousClientSize, currentClientSize);
                ClientSizeChanged(this, eventArgs);
            }
        }

        private void OnLocationChanged(Vector2 previousLocation, Vector2 currentLocation)
        {
            if ((LocationChanged != null) && (previousLocation != currentLocation))
            {
                var eventArgs = new PropertyChangedEventArgs<Vector2>(previousLocation, currentLocation);
                LocationChanged(this, eventArgs);
            }
        }

        private void OnSizeChanged(Vector2 previousSize, Vector2 currentSize)
        {
            if ((SizeChanged is not null) && (previousSize != currentSize))
            {
                var eventArgs = new PropertyChangedEventArgs<Vector2>(previousSize, currentSize);
                SizeChanged(this, eventArgs);
            }
        }
    }
}
