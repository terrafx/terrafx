// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.X11;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.X11.UI
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class Window : IDisposable, IWindow
    {
        private const int CirculateNotify = 26;
        private const int ConfigureNotify = 22;
        private const int CopyFromParent = 0;
        private const int InputOutput = 1;
        private const int None = 0;
        private const int PlaceOnTop = 0;
        private const int StructureNotifyMask = 1 << 17;
        private const int VisibilityChangeMask = 1 << 16;
        private const int VisibilityFullyObscured = 2;
        private const int VisibilityNotify = 15;

        private readonly Lazy<UIntPtr> _handle;
        private readonly Thread _parentThread;
        private readonly PropertySet _properties;
        private readonly string _title;
        private readonly WindowProvider _windowProvider;
        private readonly FlowDirection _flowDirection;
        private readonly ReadingDirection _readingDirection;

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
        public IntPtr Handle => _state.IsNotDisposedOrDisposing ? (IntPtr)(void*)_handle.Value : IntPtr.Zero;

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
        public void Activate()
        {
            var succeeded = TryActivate();

            if (succeeded == false)
            {
                ThrowExternalExceptionForLastError(nameof(XRaiseWindow));
            }
        }

        /// <summary>Closes the instance.</summary>
        public void Close() => Dispose();

        /// <summary>Creates a new <see cref="IGraphicsSurface" /> for the instance.</summary>
        /// <param name="bufferCount">The number of buffers created for the instance.</param>
        /// <returns>A new <see cref="IGraphicsSurface" /> for the instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferCount" /> is less than or equal to zero.</exception>
        public IGraphicsSurface CreateGraphicsSurface(int bufferCount) => new GraphicsSurface(this, _bounds.Size, bufferCount);

        /// <summary>Disables the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Disable() => _isEnabled = false;

        /// <summary>Enables the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Enable() => _isEnabled = true;

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Hide()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible)
            {
                var display = (XDisplay*)_windowProvider.DispatchProvider.Display;
                _ = XUnmapWindow(display, _handle.Value);
            }
        }

        /// <summary>Maximizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Maximize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Maximized)
            {
                var display = (XDisplay*)_windowProvider.DispatchProvider.Display;
                XWindowAttributes windowAttributes;
                _ = XGetWindowAttributes(display, _handle.Value, &windowAttributes);
                _restoredBounds = new Rectangle(windowAttributes.x, windowAttributes.y, windowAttributes.width, windowAttributes.height);

                var screenWidth = XWidthOfScreen(windowAttributes.screen);
                var screenHeight = XHeightOfScreen(windowAttributes.screen);

                _ = XMoveResizeWindow(display, _handle.Value, 0, 0, (uint)screenWidth, (uint)screenHeight);
                _windowState = WindowState.Maximized;
            }
        }

        /// <summary>Minimizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Minimize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Minimized)
            {
                var display = (XDisplay*)_windowProvider.DispatchProvider.Display;
                XWindowAttributes windowAttributes;
                _ = XGetWindowAttributes(display, _handle.Value, &windowAttributes);

                var screenNumber = XScreenNumberOfScreen(windowAttributes.screen);

                _ = XIconifyWindow(display, _handle.Value, screenNumber);
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
                    var display = (XDisplay*)_windowProvider.DispatchProvider.Display;
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
                var display = (XDisplay*)_windowProvider.DispatchProvider.Display;
                _ = XMapWindow(display, _handle.Value);
            }

            _ = TryActivate();
        }

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        public bool TryActivate()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isActive == false)
            {
                var display = (XDisplay*)_windowProvider.DispatchProvider.Display;
                _ = XRaiseWindow(display, _handle.Value);
            }

            return true;
        }

        internal void ProcessWindowEvent(in XEvent xevent)
        {
            ThrowIfNotThread(_parentThread);

            switch (xevent.type)
            {
                case VisibilityNotify:
                {
                    HandleXVisibility(in xevent.xvisibility);
                    break;
                }

                case ConfigureNotify:
                {
                    HandleXConfigure(in xevent.xconfigure);
                    break;
                }

                case CirculateNotify:
                {
                    HandleXCirculate(in xevent.xcirculate);
                    break;
                }
            }
        }

        private UIntPtr CreateWindowHandle()
        {
            var display = (XDisplay*)_windowProvider.DispatchProvider.Display;

            var defaultScreen = XDefaultScreenOfDisplay(display);
            var rootWindow = XRootWindowOfScreen(defaultScreen);

            var screenWidth = XWidthOfScreen(defaultScreen);
            var screenHeight = XHeightOfScreen(defaultScreen);

            var window = XCreateWindow(
                display,
                rootWindow,
                float.IsNaN(Bounds.X) ? (int)(screenWidth * 0.75f) : (int)Bounds.X,
                float.IsNaN(Bounds.Y) ? (int)(screenHeight * 0.75f) : (int)Bounds.Y,
                float.IsNaN(Bounds.Width) ? (uint)(screenWidth * 0.125f) : (uint)Bounds.Width,
                float.IsNaN(Bounds.Height) ? (uint)(screenHeight * 0.125f) : (uint)Bounds.Height,
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

            return window;
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeWindowHandle();
            }

            _state.EndDispose();
        }

        private void DisposeWindowHandle()
        {
            _state.AssertDisposing();

            if (_handle.IsValueCreated)
            {
                var display = (XDisplay*)_windowProvider.DispatchProvider.Display;
                _ = XDestroyWindow(display, _handle.Value);
            }
        }

        private void HandleXCirculate(in XCirculateEvent xcirculate) => _isActive = xcirculate.place == PlaceOnTop;

        private void HandleXConfigure(in XConfigureEvent xconfigure) => _bounds = new Rectangle(xconfigure.x, xconfigure.y, xconfigure.width, xconfigure.height);

        private void HandleXVisibility(in XVisibilityEvent xvisibility) => _isVisible = xvisibility.state != VisibilityFullyObscured;
    }
}
