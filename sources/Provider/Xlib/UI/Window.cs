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
using static TerraFX.Interop.Xlib;
using static TerraFX.Provider.Xlib.HelperUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Xlib.UI
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class Window : IDisposable, IWindow
    {
        private readonly Thread _parentThread;
        private readonly PropertySet _properties;
        private readonly WindowProvider _windowProvider;
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;

        private ResettableLazy<UIntPtr> _handle;
        private string _title;
        private Rectangle _bounds;
        private Rectangle _restoredBounds;
        private WindowState _windowState;
        private State _state;
        private bool _isActive;
        private bool _isEnabled;
        private bool _isVisible;

        internal Window(WindowProvider windowProvider)
        {
            Assert(windowProvider != null, Resources.ArgumentNullExceptionMessage, nameof(windowProvider));

            _handle = new ResettableLazy<UIntPtr>(CreateWindowHandle);

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

        /// <summary>Occurs when the <see cref="IWindow.Location" /> property changes.</summary>
        public event EventHandler<PropertyChangedEventArgs<Vector2>>? LocationChanged;

        /// <summary>Occurs when the <see cref="IWindow.Size" /> property changes.</summary>
        public event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        public Rectangle Bounds => _bounds;

        /// <summary>Gets <see cref="FlowDirection" /> for the instance.</summary>
        public FlowDirection FlowDirection => _flowDirection;

        /// <summary>Gets the handle for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr Handle
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

        /// <summary>Gets the set of properties for the instance.</summary>
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
        /// <exception cref="ExternalException">The call to <see cref="XRaiseWindow(UIntPtr, UIntPtr)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public void Activate()
        {
            var succeeded = TryActivate();

            if (succeeded == false)
            {
                ThrowExternalExceptionForLastError(nameof(XRaiseWindow));
            }
        }

        /// <summary>Closes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="XSendEvent(UIntPtr, UIntPtr, int, IntPtr, XEvent*)" /> failed.</exception>
        /// <remarks>
        ///   <para>This method can be called from any thread.</para>
        ///   <para>This method does nothing if the underlying <c>HWND</c> has not been created.</para>
        /// </remarks>
        public void Close()
        {
            if (_handle.IsCreated)
            {
                var dispatchProvider = DispatchProvider.Instance;
                SendClientMessage(
                    dispatchProvider.Display,
                    window: Handle,
                    messageType: dispatchProvider.WmProtocolsAtom,
                    message: dispatchProvider.WmDeleteWindowAtom
                );
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
                var wmHints = new XWMHints {
                    flags = (IntPtr)InputHint,
                    input = False
                };

                _ = XSetWMHints(DispatchProvider.Instance.Display, Handle, &wmHints);
                _isEnabled = false;
            }
        }

        /// <summary>Enables the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="XGetWMHints(UIntPtr, UIntPtr)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>false</c> but the instance has already been disposed.</exception>
        public void Enable()
        {
            if (_isEnabled == false)
            {
                var wmHints = new XWMHints {
                    flags = (IntPtr)InputHint,
                    input = True
                };

                _ = XSetWMHints(DispatchProvider.Instance.Display, Handle, &wmHints);
                _isEnabled = true;
            }
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>true</c> but the instance has already been disposed.</exception>
        public void Hide()
        {
            if (_isVisible)
            {
                _ = XUnmapWindow(DispatchProvider.Instance.Display, Handle);
            }
        }

        /// <summary>Maximizes the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(UIntPtr, UIntPtr, XWindowAttributes*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Maximized" /> but the instance has already been disposed.</exception>
        public void Maximize()
        {
            if (_windowState != WindowState.Maximized)
            {
                var display = DispatchProvider.Instance.Display;
                var handle = Handle;

                XWindowAttributes windowAttributes;
                ThrowExternalExceptionIfFailed(nameof(XGetWindowAttributes), XGetWindowAttributes(display, handle, &windowAttributes));

                _restoredBounds = new Rectangle(windowAttributes.x, windowAttributes.y, windowAttributes.width, windowAttributes.height);

                var screenWidth = XWidthOfScreen(windowAttributes.screen);
                var screenHeight = XHeightOfScreen(windowAttributes.screen);

                _ = XMoveResizeWindow(display, handle, 0, 0, (uint)screenWidth, (uint)screenHeight);
                _windowState = WindowState.Maximized;
            }
        }

        /// <summary>Minimizes the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(UIntPtr, UIntPtr, XWindowAttributes*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="XIconifyWindow(UIntPtr, UIntPtr, int)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Minimized" /> but the instance has already been disposed.</exception>
        public void Minimize()
        {
            if (_windowState != WindowState.Minimized)
            {
                var display = DispatchProvider.Instance.Display;
                var handle = Handle;

                XWindowAttributes windowAttributes;
                ThrowExternalExceptionIfFailed(nameof(XGetWindowAttributes), XGetWindowAttributes(display, handle, &windowAttributes));

                var screenNumber = XScreenNumberOfScreen(windowAttributes.screen);

                ThrowExternalExceptionIfZero(nameof(XIconifyWindow), XIconifyWindow(display, handle, screenNumber));

                _windowState = WindowState.Minimized;
            }
        }

        /// <summary>Restores the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Restored" /> but the instance has already been disposed.</exception>
        public void Restore()
        {
            if (_windowState != WindowState.Restored)
            {
                if (_windowState == WindowState.Maximized)
                {
                    _ = XMoveResizeWindow(DispatchProvider.Instance.Display, Handle, (int)_restoredBounds.X, (int)_restoredBounds.Y, (uint)_restoredBounds.Width, (uint)_restoredBounds.Height);
                }

                Show();
                _windowState = WindowState.Restored;
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>false</c> but the instance has already been disposed.</exception>
        public void Show()
        {
            if (_isVisible == false)
            {
                _ = XMapWindow(DispatchProvider.Instance.Display, Handle);
                _ = TryActivate();
            }
        }

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
        public bool TryActivate()
        {
            if (_isActive == false)
            {
                _ = XRaiseWindow(DispatchProvider.Instance.Display, Handle);
            }

            return true;
        }

        internal void ProcessWindowEvent(XEvent* xevent, bool isWmProtocolsEvent)
        {
            ThrowIfNotThread(_parentThread);

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

        private UIntPtr CreateWindowHandle()
        {
            _state.AssertNotDisposedOrDisposing();

            var dispatchProvider = DispatchProvider.Instance;
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
                border_width: 0,
                depth :CopyFromParent,
                c_class: InputOutput,
                (Visual*)CopyFromParent,
                valuemask: UIntPtr.Zero,
                attributes: null
            );
            ThrowExternalExceptionIfZero(nameof(XCreateSimpleWindow), window);

            _ = XSelectInput(
                display,
                window,
                (IntPtr)(VisibilityChangeMask | StructureNotifyMask)
            );

            var wmDeleteWindowAtom = dispatchProvider.WmDeleteWindowAtom;
            ThrowExternalExceptionIfZero(nameof(XSetWMProtocols), XSetWMProtocols(display, window, &wmDeleteWindowAtom, count: 1));

            SendClientMessage(
                display,
                window,
                messageType: dispatchProvider.WmProtocolsAtom,
                message: dispatchProvider.WindowProviderCreateWindowAtom,
                data: GCHandle.ToIntPtr(_windowProvider.NativeHandle)
            );

            return window;
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
                _ = XDestroyWindow(DispatchProvider.Instance.Display, _handle.Value);
            }
        }

        private void HandleXClientMessage(XClientMessageEvent* xclientMessage, bool isWmProtocolsEvent)
        {
            if (isWmProtocolsEvent)
            {
                if (xclientMessage->data.l[0] == (IntPtr)(void*)DispatchProvider.Instance.WmDeleteWindowAtom)
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
