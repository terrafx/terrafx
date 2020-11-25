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
using static TerraFX.Interop.Xlib;
using static TerraFX.UI.Providers.Xlib.HelperUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.UI.Providers.Xlib
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class XlibWindow : Window
    {
        private readonly PropertySet _properties;
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;
        private readonly string _title;

        private ValueLazy<nuint> _handle;
        private Rectangle _bounds;
        private Rectangle _restoredBounds;
        private WindowState _windowState;
        private State _state;
        private bool _isActive;
        private bool _isEnabled;
        private bool _isVisible;

        internal XlibWindow(XlibWindowProvider windowProvider)
            : base(windowProvider, Thread.CurrentThread)
        {
            _handle = new ValueLazy<nuint>(CreateWindowHandle);

            _properties = new PropertySet();
            _title = typeof(XlibWindow).FullName!;
            _bounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);
            _flowDirection = FlowDirection.TopToBottom;
            _readingDirection = ReadingDirection.LeftToRight;
            _isEnabled = true;

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="XlibWindow" /> class.</summary>
        ~XlibWindow()
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
        public nuint Handle => _handle.Value;

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
        public override IntPtr SurfaceContextHandle => XlibDispatchProvider.Instance.Display;

        /// <inheritdoc />
        public override IntPtr SurfaceHandle => (nint)Handle;

        /// <inheritdoc />
        public override GraphicsSurfaceKind SurfaceKind => GraphicsSurfaceKind.Xlib;

        /// <inheritdoc />
        public override string Title => _title;

        /// <inheritdoc />
        public override WindowState WindowState => _windowState;

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Activate() => ThrowExternalExceptionIfFalse(TryActivate(), nameof(XRaiseWindow));

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="XSendEvent(IntPtr, nuint, int, nint, XEvent*)" /> failed.</exception>
        /// <remarks>
        ///   <para>This method can be called from any thread.</para>
        ///   <para>This method does nothing if the underlying <c>HWND</c> has not been created.</para>
        /// </remarks>
        public override void Close()
        {
            if (_handle.IsCreated)
            {
                var dispatchProvider = XlibDispatchProvider.Instance;
                SendClientMessage(
                    dispatchProvider.Display,
                    window: Handle,
                    messageType: dispatchProvider.WmProtocolsAtom,
                    message: dispatchProvider.WmDeleteWindowAtom
                );
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Disable()
        {
            if (_isEnabled)
            {
                var wmHints = new XWMHints {
                    flags = InputHint,
                    input = False
                };

                _ = XSetWMHints(XlibDispatchProvider.Instance.Display, Handle, &wmHints);
                _isEnabled = false;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="XGetWMHints(IntPtr, nuint)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Enable()
        {
            if (_isEnabled == false)
            {
                var wmHints = new XWMHints {
                    flags = InputHint,
                    input = True
                };

                _ = XSetWMHints(XlibDispatchProvider.Instance.Display, Handle, &wmHints);
                _isEnabled = true;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>true</c> but the instance has already been disposed.</exception>
        public override void Hide()
        {
            if (_isVisible)
            {
                _ = XUnmapWindow(XlibDispatchProvider.Instance.Display, Handle);
            }
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(IntPtr, nuint, XWindowAttributes*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Maximized" /> but the instance has already been disposed.</exception>
        public override void Maximize()
        {
            if (_windowState != WindowState.Maximized)
            {
                var display = XlibDispatchProvider.Instance.Display;
                var handle = Handle;

                XWindowAttributes windowAttributes;
                ThrowExternalExceptionIfFailed(XGetWindowAttributes(display, handle, &windowAttributes), nameof(XGetWindowAttributes));

                _restoredBounds = new Rectangle(windowAttributes.x, windowAttributes.y, windowAttributes.width, windowAttributes.height);

                var screenWidth = XWidthOfScreen(windowAttributes.screen);
                var screenHeight = XHeightOfScreen(windowAttributes.screen);

                _ = XMoveResizeWindow(display, handle, 0, 0, (uint)screenWidth, (uint)screenHeight);
                _windowState = WindowState.Maximized;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(IntPtr, nuint, XWindowAttributes*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="XIconifyWindow(IntPtr, nuint, int)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Minimized" /> but the instance has already been disposed.</exception>
        public override void Minimize()
        {
            if (_windowState != WindowState.Minimized)
            {
                var display = XlibDispatchProvider.Instance.Display;
                var handle = Handle;

                XWindowAttributes windowAttributes;
                ThrowExternalExceptionIfFailed(XGetWindowAttributes(display, handle, &windowAttributes), nameof(XGetWindowAttributes));

                var screenNumber = XScreenNumberOfScreen(windowAttributes.screen);

                ThrowExternalExceptionIfZero(XIconifyWindow(display, handle, screenNumber), nameof(XIconifyWindow));

                _windowState = WindowState.Minimized;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Restored" /> but the instance has already been disposed.</exception>
        public override void Restore()
        {
            if (_windowState != WindowState.Restored)
            {
                if (_windowState == WindowState.Maximized)
                {
                    _ = XMoveResizeWindow(XlibDispatchProvider.Instance.Display, Handle, (int)_restoredBounds.X, (int)_restoredBounds.Y, (uint)_restoredBounds.Width, (uint)_restoredBounds.Height);
                }

                Show();
                _windowState = WindowState.Restored;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override void Show()
        {
            if (_isVisible == false)
            {
                _ = XMapWindow(XlibDispatchProvider.Instance.Display, Handle);
                _ = TryActivate();
            }
        }

        /// <inheritdoc />
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public override bool TryActivate()
        {
            if (_isActive == false)
            {
                _ = XRaiseWindow(XlibDispatchProvider.Instance.Display, Handle);
            }
            return true;
        }

        internal void ProcessWindowEvent(XEvent* xevent, bool isWmProtocolsEvent)
        {
            ThrowIfNotThread(ParentThread);

            switch (xevent->type)
            {
                case VisibilityNotify:
                {
                    HandleXVisibility(&xevent->xvisibility);
                    break;
                }

                case DestroyNotify:
                {
                    HandleXDestroyWindow(&xevent->xdestroywindow);
                    break;
                }

                case ConfigureNotify:
                {
                    HandleXConfigure(&xevent->xconfigure);
                    break;
                }

                case CirculateNotify:
                {
                    HandleXCirculate(&xevent->xcirculate);
                    break;
                }

                case ClientMessage:
                {
                    HandleXClientMessage(&xevent->xclient, isWmProtocolsEvent);
                    break;
                }
            }
        }

        private nuint CreateWindowHandle()
        {
            _state.AssertNotDisposedOrDisposing();

            var dispatchProvider = XlibDispatchProvider.Instance;
            var display = dispatchProvider.Display;

            var defaultScreen = XDefaultScreenOfDisplay(display);
            var rootWindow = XRootWindowOfScreen(defaultScreen);

            var screenWidth = XWidthOfScreen(defaultScreen);
            var screenHeight = XHeightOfScreen(defaultScreen);

            var window = XCreateWindow(
                display,
                rootWindow,
                float.IsNaN(Bounds.X) ? (int)(screenWidth * 0.125f) : (int)Bounds.X,
                float.IsNaN(Bounds.Y) ? (int)(screenHeight * 0.125f) : (int)Bounds.Y,
                float.IsNaN(Bounds.Width) ? (uint)(screenWidth * 0.75f) : (uint)Bounds.Width,
                float.IsNaN(Bounds.Height) ? (uint)(screenHeight * 0.75f) : (uint)Bounds.Height,
                0,
                (int)CopyFromParent,
                InputOutput,
                (Visual*)CopyFromParent,
                0,
                null
            );
            ThrowExternalExceptionIfZero(window, nameof(XCreateSimpleWindow));

            _ = XSelectInput(
                display,
                window,
                VisibilityChangeMask | StructureNotifyMask
            );

            var wmDeleteWindowAtom = dispatchProvider.WmDeleteWindowAtom;
            ThrowExternalExceptionIfZero(XSetWMProtocols(display, window, &wmDeleteWindowAtom, 1), nameof(XSetWMProtocols));

            SendClientMessage(
                display,
                window,
                messageType: dispatchProvider.WmProtocolsAtom,
                message: dispatchProvider.WindowProviderCreateWindowAtom,
                data: GCHandle.ToIntPtr(((XlibWindowProvider)WindowProvider).NativeHandle)
            );

            return window;
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
                _ = XDestroyWindow(XlibDispatchProvider.Instance.Display, _handle.Value);
            }
        }

        private void HandleXClientMessage(XClientMessageEvent* xclientMessage, bool isWmProtocolsEvent)
        {
            if (isWmProtocolsEvent)
            {
                if (xclientMessage->data.l[0] == (nint)XlibDispatchProvider.Instance.WmDeleteWindowAtom)
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
                }
            }
        }

        private void HandleXCirculate(XCirculateEvent* xcirculate) => _isActive = xcirculate->place == PlaceOnTop;

        private void HandleXConfigure(XConfigureEvent* xconfigure)
        {
            var previousLocation = _bounds.Location;
            var currentLocation = new Vector2(xconfigure->x, xconfigure->y);

            if (currentLocation != previousLocation)
            {
                _bounds = _bounds.WithLocation(currentLocation);
                OnLocationChanged(previousLocation, currentLocation);
            }

            var previousSize = _bounds.Size;
            var currentSize = new Vector2(xconfigure->width, xconfigure->height);

            if (currentSize != previousSize)
            {
                _bounds = _bounds.WithSize(currentSize);
                OnSizeChanged(previousSize, currentSize);
            }

            _bounds = new Rectangle(xconfigure->x, xconfigure->y, xconfigure->width, xconfigure->height);
        }

        private void HandleXDestroyWindow(XDestroyWindowEvent* xdestroyWindow)
        {
            // We handle this here to ensure we transition to the appropriate state in the case
            // an end-user called XDestroyWindow themselves. The assumption here is that this was
            // done "properly" if we are Disposing, in which case we don't need to do anything.
            // Otherwise, this was triggered externally and we should just switch the state to
            // be disposed.

            if (_state != Disposing)
            {
                _ = _state.Transition(to: Disposed);
            }
        }

        private void HandleXVisibility(XVisibilityEvent* xvisibility) => _isVisible = xvisibility->state != VisibilityFullyObscured;

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
