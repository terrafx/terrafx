// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using TerraFX.Collections;
using TerraFX.Interop;
using TerraFX.Provider.libX11.Threading;
using TerraFX.Threading;
using TerraFX.UI;
using GC = System.GC;
using XWindow = TerraFX.Interop.Window;
using static TerraFX.Interop.libX11;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.libX11.UI
{
    /// <summary>Defines a window.</summary>
    unsafe public sealed class Window : IDisposable, IWindow
    {
        #region Fields
        internal readonly Display* _display;

        internal readonly Dispatcher _dispatcher;

        internal readonly PropertySet _properties;

        internal readonly WindowManager _windowManager;

        internal XWindow _handle;

        internal Rectangle _bounds;

        internal FlowDirection _flowDirection;

        internal ReadingDirection _readingDirection;

        internal bool _isActive;

        internal bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        internal Window(WindowManager windowManager, DispatchManager dispatchManager)
        {
            _display = dispatchManager.Display;
            _dispatcher = (Dispatcher)(dispatchManager.DispatcherForCurrentThread);
            _properties = new PropertySet();
            _windowManager = windowManager;

            _handle = CreateWindowHandle(_display);

            XWindowAttributes windowAttributes;
            var status = XGetWindowAttributes(_display, _handle, &windowAttributes);

            if (status == 0)
            {
                ThrowExternalException(nameof(XGetWindowAttributes), status);
            }

            _bounds = new Rectangle(windowAttributes.x, windowAttributes.y, windowAttributes.width, windowAttributes.height);
            _isVisible = (windowAttributes.map_state == IsViewable);
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

        /// <summary>Gets the dispatcher for the instance.</summary>
        public IDispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
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
                return (IntPtr)((void*)(_handle));
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

        /// <summary>Gets the <see cref="IWindowManager" /> for the instance.</summary>
        public IWindowManager WindowManager
        {
            get
            {
                return _windowManager;
            }
        }
        #endregion

        #region Static Methods
        internal static XWindow CreateWindowHandle(Display* display)
        {
            var defaultScreen = XDefaultScreenOfDisplay(display);
            var rootWindow = XRootWindowOfScreen(defaultScreen);

            var screenWidth = XWidthOfScreen(defaultScreen);
            var screenHeight = XHeightOfScreen(defaultScreen);

            var window = XCreateWindow(
                display,
                parent: rootWindow,
                x: (int)(screenWidth * 0.75f),
                y: (int)(screenHeight * 0.75f),
                width: (uint)(screenWidth * 0.125f),
                height: (uint)(screenHeight * 0.125f),
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
        #endregion

        #region Methods
        internal void Dispose(bool isDisposing)
        {
            if (_handle != None)
            {
                XDestroyWindow(_display, _handle);
                _windowManager.DestroyWindow(_handle);
                _handle = None;
            }
        }

        internal void HandleXVisibility(ref /* readonly */ XVisibilityEvent xvisibility)
        {
            Debug.Assert(xvisibility.window == _handle);
            _isVisible = (xvisibility.state != VisibilityFullyObscured);
        }

        internal void HandleXConfigure(ref /* readonly */ XConfigureEvent xconfigure)
        {
            Debug.Assert(xconfigure.window == _handle);
            _bounds = new Rectangle(xconfigure.x, xconfigure.y, xconfigure.width, xconfigure.height);
        }

        internal void HandleXCirculate(ref /* readonly */ XCirculateEvent xcirculate)
        {
            Debug.Assert(xcirculate.window == _handle);
            _isActive = (xcirculate.place == PlaceOnTop);
        }

        internal void ProcessXEvent(ref /* readonly */ XEvent xevent)
        {
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
        public void Activate()
        {
            if (_isActive == false)
            {
                XRaiseWindow(_display, _handle);
            }
        }

        /// <summary>Closes the instance.</summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>Hides the instance.</summary>
        public void Hide()
        {
            if (_isVisible)
            {
                XUnmapWindow(_display, _handle);
            }
        }

        /// <summary>Shows the instance.</summary>
        public void Show()
        {
            if (_isVisible == false)
            {
                XMapWindow(_display, _handle);
            }
        }
        #endregion
    }
}
