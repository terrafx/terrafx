// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Xlib;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Xlib.UI
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class Window : IDisposable, IWindow
    {
        private readonly Lazy<UIntPtr> _handle;
        private readonly Thread _parentThread;
        private readonly PropertySet _properties;
        private readonly WindowProvider _windowProvider;
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;

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
            _handle = new Lazy<UIntPtr>(CreateWindowHandle, isThreadSafe: true);

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
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="XRaiseWindow(UIntPtr, UIntPtr)" /> failed.</exception>
        public void Activate()
        {
            var succeeded = TryActivate();

            if (succeeded == false)
            {
                ThrowExternalExceptionForLastError(nameof(XRaiseWindow));
            }
        }

        /// <summary>Closes the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="XSendEvent(UIntPtr, UIntPtr, int, IntPtr, XEvent*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        /// <remarks>
        ///   <para>This method can be called from any thread.</para>
        ///   <para>This method does nothing if the underlying <c>HWND</c> has not been created.</para>
        /// </remarks>
        public void Close()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_handle.IsValueCreated)
            {
                var dispatchProvider = _windowProvider.DispatchProvider;

                var clientEvent = new XClientMessageEvent {
                    type = ClientMessage,
                    serial = UIntPtr.Zero,
                    send_event = True,
                    display = dispatchProvider.Display,
                    window = Handle,
                    message_type = dispatchProvider.WmProtocolsAtom,
                    format = 32
                };
                clientEvent.data.l[0] = (IntPtr)(void*)dispatchProvider.WmDeleteWindowAtom;

                var status = XSendEvent(
                    clientEvent.display,
                    clientEvent.window,
                    propagate: False,
                    (IntPtr)NoEventMask,
                    (XEvent*)&clientEvent
                );

                if (status == 0)
                {
                    ThrowExternalException(nameof(XSendEvent), status);
                }
            }
        }

        /// <summary>Creates a new <see cref="IGraphicsSurface" /> for the instance.</summary>
        /// <param name="bufferCount">The number of buffers created for the instance.</param>
        /// <returns>A new <see cref="IGraphicsSurface" /> for the instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferCount" /> is less than or equal to zero.</exception>
        public IGraphicsSurface CreateGraphicsSurface(int bufferCount) => new GraphicsSurface(this, bufferCount);

        /// <summary>Disables the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Disable()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isEnabled)
            {
                var display = _windowProvider.DispatchProvider.Display;
                var wmHints = new XWMHints {
                    flags = (IntPtr)InputHint,
                    input = False
                };

                _ = XSetWMHints(display, _handle.Value, &wmHints);
                _isEnabled = false;
            }
        }

        /// <summary>Enables the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="XGetWMHints(UIntPtr, UIntPtr)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Enable()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isEnabled == false)
            {
                var display = _windowProvider.DispatchProvider.Display;
                var wmHints = new XWMHints {
                    flags = (IntPtr)InputHint,
                    input = True
                };

                _ = XSetWMHints(display, _handle.Value, &wmHints);
                _isEnabled = true;
            }
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Hide()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible)
            {
                var display = _windowProvider.DispatchProvider.Display;
                _ = XUnmapWindow(display, _handle.Value);
            }
        }

        /// <summary>Maximizes the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(UIntPtr, UIntPtr, XWindowAttributes*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Maximize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Maximized)
            {
                var display = _windowProvider.DispatchProvider.Display;

                XWindowAttributes windowAttributes;
                var status = XGetWindowAttributes(display, _handle.Value, &windowAttributes);

                if (status != Success)
                {
                    ThrowExternalExceptionForLastError(nameof(XGetWindowAttributes));
                }

                _restoredBounds = new Rectangle(windowAttributes.x, windowAttributes.y, windowAttributes.width, windowAttributes.height);

                var screenWidth = XWidthOfScreen(windowAttributes.screen);
                var screenHeight = XHeightOfScreen(windowAttributes.screen);

                _ = XMoveResizeWindow(display, _handle.Value, 0, 0, (uint)screenWidth, (uint)screenHeight);
                _windowState = WindowState.Maximized;
            }
        }

        /// <summary>Minimizes the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(UIntPtr, UIntPtr, XWindowAttributes*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="XIconifyWindow(UIntPtr, UIntPtr, int)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Minimize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Minimized)
            {
                var display = _windowProvider.DispatchProvider.Display;

                XWindowAttributes windowAttributes;
                var status = XGetWindowAttributes(display, _handle.Value, &windowAttributes);

                if (status != Success)
                {
                    ThrowExternalExceptionForLastError(nameof(XGetWindowAttributes));
                }

                var screenNumber = XScreenNumberOfScreen(windowAttributes.screen);

                status = XIconifyWindow(display, _handle.Value, screenNumber);

                if (status == 0)
                {
                    ThrowExternalExceptionForLastError(nameof(XIconifyWindow));
                }

                _windowState = WindowState.Minimized;
            }
        }

        /// <summary>Restores the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Restore()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Restored)
            {
                if (_windowState == WindowState.Maximized)
                {
                    var display = _windowProvider.DispatchProvider.Display;
                    _ = XMoveResizeWindow(display, _handle.Value, (int)_restoredBounds.X, (int)_restoredBounds.Y, (uint)_restoredBounds.Width, (uint)_restoredBounds.Height);
                }

                Show();
                _windowState = WindowState.Restored;
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Show()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible == false)
            {
                var display = _windowProvider.DispatchProvider.Display;
                _ = XMapWindow(display, _handle.Value);

                _ = TryActivate();
            }
        }

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public bool TryActivate()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isActive == false)
            {
                var display = _windowProvider.DispatchProvider.Display;
                _ = XRaiseWindow(display, _handle.Value);
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

            var dispatchProvider = _windowProvider.DispatchProvider;
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
                CopyFromParent,
                InputOutput,
                (Visual*)CopyFromParent,
                UIntPtr.Zero,
                null
            );

            if (window == (UIntPtr)None)
            {
                ThrowExternalExceptionForLastError(nameof(XCreateSimpleWindow));
            }

            _ = XSelectInput(
                display,
                window,
                (IntPtr)(VisibilityChangeMask | StructureNotifyMask)
            );

            var clientEvent = new XClientMessageEvent {
                type = ClientMessage,
                serial = UIntPtr.Zero,
                send_event = True,
                display = dispatchProvider.Display,
                window = window,
                message_type = dispatchProvider.WmProtocolsAtom,
                format = 32
            };

            clientEvent.data.l[0] = (IntPtr)(void*)dispatchProvider.WindowProviderCreateWindowAtom;

            var windowProviderNativeHandle = GCHandle.ToIntPtr(_windowProvider.NativeHandle);

            if (Environment.Is64BitProcess)
            {
                var bits = windowProviderNativeHandle.ToInt64();
                clientEvent.data.l[1] = unchecked((IntPtr)(uint)bits);
                clientEvent.data.l[2] = (IntPtr)(uint)(bits >> 32);
            }
            else
            {
                var bits = windowProviderNativeHandle.ToInt32();
                clientEvent.data.l[1] = (IntPtr)bits;
            }

            var status = XSendEvent(
                clientEvent.display,
                clientEvent.window,
                propagate: False,
                (IntPtr)NoEventMask,
                (XEvent*)&clientEvent
            );

            if (status == 0)
            {
                ThrowExternalException(nameof(XSendEvent), status);
            }

            return window;
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
                // TODO: This fails due to ObjectDisposedException if the application terminates
                // due to the application disposing.
                //
                // var display = _windowProvider.DispatchProvider.Display;
                // _ = XDestroyWindow(display, _handle.Value);
            }
        }

        private void HandleXClientMessage(XClientMessageEvent* xclientMessage, bool isWmProtocolsEvent)
        {
            var dispatchProvider = _windowProvider.DispatchProvider;

            if (isWmProtocolsEvent)
            {
                if (xclientMessage->data.l[0] == (IntPtr)(void*)dispatchProvider.WmDeleteWindowAtom)
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

        private void HandleXConfigure(XConfigureEvent* xconfigure) => _bounds = new Rectangle(xconfigure->x, xconfigure->y, xconfigure->width, xconfigure->height);

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
    }
}
