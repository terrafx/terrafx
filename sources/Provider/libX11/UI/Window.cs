// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.libX11;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.libX11.UI
{
    /// <summary>Defines a window.</summary>
    public sealed unsafe class Window : IDisposable, IWindow
    {
        #region Fields
        /// <summary>The native window handle for the instance.</summary>
        internal readonly Lazy<nuint> _handle;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly Thread _parentThread;

        /// <summary>The <see cref="PropertySet" /> for the instance.</summary>
        internal readonly PropertySet _properties;

        /// <summary>The title for the instance.</summary>
        internal string _title;

        /// <summary>The <see cref="WindowManager" /> for the instance.</summary>
        internal readonly WindowManager _windowManager;

        /// <summary>A <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        internal Rectangle _bounds;

        /// <summary>The <see cref="FlowDirection" /> for the instance.</summary>
        internal FlowDirection _flowDirection;

        /// <summary>The <see cref="ReadingDirection" /> for the instance.</summary>
        internal ReadingDirection _readingDirection;

        /// <summary>The <see cref="WindowState" /> for the instance.</summary>
        internal WindowState _windowState;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal readonly State _state;

        /// <summary>A value that indicates whether the instance is the active window.</summary>
        internal bool _isActive;

        /// <summary>A value that indicates whether the instance is visible.</summary>
        internal bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        /// <param name="windowManager">The <see cref="WindowManager" /> for the instance.</param>
        internal Window(WindowManager windowManager)
        {
            _handle = new Lazy<nuint>(CreateWindowHandle, isThreadSafe: true);

            _parentThread = Thread.CurrentThread;
            _properties = new PropertySet();
            _title = typeof(Window).FullName;
            _bounds = new Rectangle(float.NaN, float.NaN, float.NaN, float.NaN);

            _windowManager = windowManager;
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
                return (IntPtr)(_handle.Value);
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

        /// <summary>Gets the set of properties for the instance.</summary>
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

        /// <summary>Gets the <see cref="IWindowManager" /> for the instance.</summary>
        public IWindowManager WindowManager
        {
            get
            {
                return _windowManager;
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

        #region Methods
        /// <summary>Creates a <c>Window</c> for the instance.</summary>
        /// <returns>A <c>Window</c> for the created native window.</returns>
        /// <exception cref="ExternalException">The call to <see cref="XCreateWindow(IntPtr, nuint, int, int, uint, uint, uint, int, uint, Visual*, nuint, XSetWindowAttributes*)" /> failed.</exception>
        internal nuint CreateWindowHandle()
        {
            var display = _windowManager.DispatchManager.Display;

            var defaultScreen = XDefaultScreenOfDisplay(display);
            var rootWindow = XRootWindowOfScreen(defaultScreen);

            var screenWidth = XWidthOfScreen(defaultScreen);
            var screenHeight = XHeightOfScreen(defaultScreen);

            var window = XCreateWindow(
                display,
                parent: rootWindow,
                x: float.IsNaN(Bounds.X) ? (int)(screenWidth * 0.75f) : (int)(Bounds.X),
                y: float.IsNaN(Bounds.Y) ? (int)(screenHeight * 0.75f) : (int)(Bounds.Y),
                width: float.IsNaN(Bounds.Width) ? (uint)(screenWidth * 0.125f) : (uint)(Bounds.Width),
                height: float.IsNaN(Bounds.Height) ? (uint)(screenHeight * 0.125f) : (uint)(Bounds.Height),
                border_width: 0,
                depth: CopyFromParent,
                @class: InputOutput,
                visual: (Visual*)(CopyFromParent),
                valuemask: 0,
                attributes: null
            );

            if (window == None)
            {
                ThrowExternalExceptionForLastError(nameof(XCreateSimpleWindow));
            }

            XSelectInput(
                display,
                window,
                event_mask: (VisibilityChangeMask | StructureNotifyMask)
            );

            return window;
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeWindowHandle();
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the <c>Window</c> for the instance.</summary>
        internal void DisposeWindowHandle()
        {
            if (_handle.IsValueCreated)
            {
                var display = _windowManager.DispatchManager.Display;
                XDestroyWindow(display, _handle.Value);
            }
        }

        /// <summary>Handles the <c>XCirculate</c> event.</summary>
        /// <param name="xcirculate">The <c>XCirculate</c> event.</param>
        internal void HandleXCirculate(ref /* readonly */ XCirculateEvent xcirculate)
        {
            _isActive = (xcirculate.place == PlaceOnTop);
        }

        /// <summary>Handles the <c>XConfigure</c> event.</summary>
        /// <param name="xconfigure">The <c>XConfigure</c> event.</param>
        internal void HandleXConfigure(ref /* readonly */ XConfigureEvent xconfigure)
        {
            _bounds = new Rectangle(xconfigure.x, xconfigure.y, xconfigure.width, xconfigure.height);
        }

        /// <summary>Handles the <c>XVisiblity</c> event.</summary>
        /// <param name="xvisibility">The <c>XVisibility</c> event.</param>
        internal void HandleXVisibility(ref /* readonly */ XVisibilityEvent xvisibility)
        {
            _isVisible = (xvisibility.state != VisibilityFullyObscured);
        }

        /// <summary>Processes a window event sent to the instance.</summary>
        /// <param name="xevent">The event to be processed.</param>
        internal void ProcessWindowEvent(ref /* readonly */ XEvent xevent)
        {
            ThrowIfNotThread(_parentThread);

            switch (xevent.type)
            {
                case VisibilityNotify:
                {
                    HandleXVisibility(ref xevent.xvisibility);
                    break;
                }

                case ConfigureNotify:
                {
                    HandleXConfigure(ref xevent.xconfigure);
                    break;
                }

                case CirculateNotify:
                {
                    HandleXCirculate(ref xevent.xcirculate);
                    break;
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
        public void Activate()
        {
            var succeeded = TryActivate();

            if (succeeded == false)
            {
                ThrowExternalExceptionForLastError(nameof(XRaiseWindow));
            }
        }

        /// <summary>Closes the instance.</summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>Hides the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Hide()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible)
            {
                var display = _windowManager.DispatchManager.Display;
                XUnmapWindow(display, _handle.Value);
            }
        }

        /// <summary>Maximizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Maximize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Maximized)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>Minimizes the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Minimize()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Minimized)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>Restores the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Restore()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_windowState != WindowState.Restored)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>Shows the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public void Show()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isVisible == false)
            {
                var display = _windowManager.DispatchManager.Display;
                XMapWindow(display, _handle.Value);
            }
        }

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        public bool TryActivate()
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_isActive == false)
            {
                var display = _windowManager.DispatchManager.Display;
                XRaiseWindow(display, _handle.Value);
            }

            return true;
        }
        #endregion
    }
}
