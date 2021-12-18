// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Interop.Xlib;
using TerraFX.Numerics;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.Xlib.Xlib;
using static TerraFX.Threading.VolatileState;
using static TerraFX.UI.XlibAtomId;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.XlibUtilities;
using XWindow = TerraFX.Interop.Xlib.Window;

namespace TerraFX.UI;

/// <summary>Defines a window.</summary>
public sealed unsafe class XlibWindow : Window
{
    private readonly FlowDirection _flowDirection;
    private readonly ReadingDirection _readingDirection;

    private ValueLazy<XWindow> _handle;
    private ValueLazy<PropertySet> _properties;

    private string _title;
    private BoundingRectangle _bounds;
    private BoundingRectangle _clientBounds;
    private BoundingRectangle _frameExtents;
    private WindowState _windowState;
    private bool _isActive;
    private bool _isEnabled;
    private bool _isVisible;

    private VolatileState _state;

    internal XlibWindow(XlibWindowService windowService)
        : base(windowService, Thread.CurrentThread)
    {
        _flowDirection = FlowDirection.TopToBottom;
        _readingDirection = ReadingDirection.LeftToRight;

        _handle = new ValueLazy<XWindow>(CreateWindowHandle);
        _properties = new ValueLazy<PropertySet>(CreateProperties);

        _title = typeof(XlibWindow).FullName!;
        _bounds = BoundingRectangle.Zero;
        _clientBounds = BoundingRectangle.Zero;
        _frameExtents = BoundingRectangle.Zero;
        _isEnabled = true;

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="XlibWindow" /> class.</summary>
    ~XlibWindow()
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
    public override BoundingRectangle Bounds
        => _bounds;

    /// <inheritdoc />
    public override BoundingRectangle ClientBounds
        => _clientBounds;

    /// <inheritdoc />
    public override FlowDirection FlowDirection
        => _flowDirection;

    /// <summary>Gets the underlying <c>Window</c> for the window.</summary>
    /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
    public XWindow Handle
        => _handle.Value;

    /// <inheritdoc />
    public override bool IsActive
        => _isActive;

    /// <inheritdoc />
    public override bool IsEnabled
        => _isEnabled;

    /// <inheritdoc />
    public override bool IsVisible
        => _isVisible;

    /// <inheritdoc />
    public override IPropertySet Properties
        => _properties.Value;

    /// <inheritdoc />
    public override ReadingDirection ReadingDirection
        => _readingDirection;

    /// <inheritdoc />
    public override string Title
        => _title;

    /// <inheritdoc cref="Window.WindowService" />
    public new XlibWindowService WindowService
        => (XlibWindowService)base.WindowService;

    /// <inheritdoc />
    public override WindowState WindowState
        => _windowState;

    /// <inheritdoc />
    protected override IntPtr SurfaceContextHandle
        => (nint)XlibDispatchService.Instance.Display;

    /// <inheritdoc />
    protected override IntPtr SurfaceHandle
        => Handle;

    /// <inheritdoc />
    protected override GraphicsSurfaceKind SurfaceKind
        => GraphicsSurfaceKind.Xlib;

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
    public override void Activate()
    {
        if (!TryActivate())
        {
            ThrowExternalException(nameof(XRaiseWindow), 0);
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
    /// <exception cref="ExternalException">The call to <see cref="XSendEvent(Display*, XWindow, int, nint, XEvent*)" /> failed.</exception>
    /// <remarks>
    ///   <para>This method can be called from any thread.</para>
    ///   <para>This method does nothing if the underlying <c>Window</c> has not been created.</para>
    /// </remarks>
    public override void Close()
    {
        if (_handle.IsValueCreated)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (dispatchService.GetAtomIsSupported(_NET_CLOSE_WINDOW))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_CLOSE_WINDOW),
                    CurrentTime,
                    SourceApplication
                );
            }
            else
            {
                SendClientMessage(
                    display,
                    window,
                    NoEventMask,
                    window,
                    dispatchService.GetAtom(WM_PROTOCOLS),
                    dispatchService.GetAtom(WM_DELETE_WINDOW)
                );
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>true</c> but the instance has already been disposed.</exception>
    public override void Disable()
    {
        if (IsEnabled)
        {
            var wmHints = new XWMHints {
                flags = InputHint,
                input = False
            };
            _ = XSetWMHints(XlibDispatchService.Instance.Display, Handle, &wmHints);
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="XGetWMHints(Display*, XWindow)" /> failed.</exception>
    /// <exception cref="ObjectDisposedException"><see cref="IsEnabled" /> was <c>false</c> but the instance has already been disposed.</exception>
    public override void Enable()
    {
        if (!IsEnabled)
        {
            var wmHints = new XWMHints {
                flags = InputHint,
                input = True
            };
            _ = XSetWMHints(XlibDispatchService.Instance.Display, Handle, &wmHints);
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Hidden" /> but the instance has already been disposed.</exception>
    public override void Hide()
    {
        if (WindowState != WindowState.Hidden)
        {
            var dispatchService = XlibDispatchService.Instance;

            ThrowForLastErrorIfZero(XWithdrawWindow(
                dispatchService.Display,
                Handle,
                XScreenNumberOfScreen(dispatchService.DefaultScreen)
            ));
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(Display*, XWindow, XWindowAttributes*)" /> failed.</exception>
    /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Maximized" /> but the instance has already been disposed.</exception>
    public override void Maximize()
    {
        var windowState = _windowState;

        if (windowState != WindowState.Maximized)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (windowState == WindowState.Minimized)
            {
                _ = XUnmapWindow(display, window);
                windowState = WindowState.Hidden;
            }

            if (windowState == WindowState.Hidden)
            {
                ShowWindow(display, window);
            }

            if (dispatchService.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_HORZ) && dispatchService.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_VERT))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_WM_STATE),
                    _NET_WM_STATE_ADD,
                    dispatchService.GetAtom(_NET_WM_STATE_MAXIMIZED_HORZ),
                    dispatchService.GetAtom(_NET_WM_STATE_MAXIMIZED_VERT),
                    SourceApplication
                );
            }
            else
            {
                ThrowNotImplementedException();
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="XIconifyWindow(Display*, XWindow, int)" /> failed.</exception>
    /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Minimized" /> but the instance has already been disposed.</exception>
    public override void Minimize()
    {
        var windowState = _windowState;

        if (windowState != WindowState.Minimized)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (windowState == WindowState.Hidden)
            {
                ShowWindow(display, window, IconicState);
            }

            ThrowForLastErrorIfZero(XIconifyWindow(
                display,
                window,
                XScreenNumberOfScreen(dispatchService.DefaultScreen)
            ));
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
    public override void Relocate(Vector2 location)
    {
        if (_bounds.Location != location)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (dispatchService.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_MOVERESIZE_WINDOW),
                    NorthWestGravity | (0b0011 << 8) | (SourceApplication << 12),
                    (nint)location.X,
                    (nint)location.Y,
                    None,
                    None
                );
            }
            else
            {
                ThrowNotImplementedException();
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
    public override void RelocateClient(Vector2 location)
    {
        if (_clientBounds.Location != location)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (dispatchService.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_MOVERESIZE_WINDOW),
                    StaticGravity | (0b0011 << 8) | (SourceApplication << 12),
                    (nint)location.X,
                    (nint)location.Y,
                    None,
                    None
                );
            }
            else
            {
                ThrowNotImplementedException();
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
    public override void Resize(Vector2 size)
    {
        if (_bounds.Size != size)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (dispatchService.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_MOVERESIZE_WINDOW),
                    NorthWestGravity | (0b1100 << 8) | (SourceApplication << 12),
                    None,
                    None,
                    (nint)(size.X - _frameExtents.Size.X),
                    (nint)(size.Y - _frameExtents.Size.Y)
                );
            }
            else
            {
                ThrowNotImplementedException();
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
    public override void ResizeClient(Vector2 size)
    {
        if (_clientBounds.Size != size)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (dispatchService.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_MOVERESIZE_WINDOW),
                    StaticGravity | (0b1100 << 8) | (SourceApplication << 12),
                    None,
                    None,
                    (nint)size.X,
                    (nint)size.Y
                );
            }
            else
            {
                ThrowNotImplementedException();
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException"><see cref="WindowState" /> was not <see cref="WindowState.Normal" /> but the instance has already been disposed.</exception>
    public override void Restore()
    {
        var windowState = _windowState;

        if (windowState != WindowState.Normal)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (windowState == WindowState.Minimized)
            {
                _ = XUnmapWindow(display, window);
                windowState = WindowState.Hidden;
            }

            if (windowState == WindowState.Hidden)
            {
                var wmHints = new XWMHints {
                    flags = StateHint,
                    initial_state = NormalState,
                };
                _ = XSetWMHints(display, window, &wmHints);

                _ = XMapWindow(display, window);
            }

            if (dispatchService.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_HORZ) && dispatchService.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_VERT))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_WM_STATE),
                    _NET_WM_STATE_REMOVE,
                    dispatchService.GetAtom(_NET_WM_STATE_MAXIMIZED_HORZ),
                    dispatchService.GetAtom(_NET_WM_STATE_MAXIMIZED_VERT),
                    SourceApplication
                );
            }
            else
            {
                ThrowNotImplementedException();
            }
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
    public override void SetTitle(string title)
    {
        if (_title != title)
        {
            var dispatchService = XlibDispatchService.Instance;
            SetWindowTitle(dispatchService, dispatchService.Display, Handle, title);
        }
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException"><see cref="IsVisible" /> was <c>false</c> but the instance has already been disposed.</exception>
    public override void Show()
    {
        if (_windowState == WindowState.Hidden)
        {
            _ = XMapWindow(XlibDispatchService.Instance.Display, Handle);
        }
    }

    /// <inheritdoc />
    /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException"><see cref="IsActive" /> was <c>false</c> but the instance has already been disposed.</exception>
    public override bool TryActivate()
    {
        if (!_isActive)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = Handle;

            if (dispatchService.GetAtomIsSupported(_NET_ACTIVE_WINDOW))
            {
                SendClientMessage(
                    display,
                    dispatchService.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    dispatchService.GetAtom(_NET_ACTIVE_WINDOW),
                    SourceApplication,
                    CurrentTime,
                    None
                );
            }
            else
            {
                ThrowNotImplementedException();
            }
        }
        return true;
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

    internal void ProcessWindowEvent(XEvent* xevent)
    {
        ThrowIfNotThread(ParentThread);

        switch (xevent->type)
        {
            case Expose:
            {
                HandleXExpose(&xevent->xexpose);
                break;
            }

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

            case UnmapNotify:
            {
                HandleXUnmap(&xevent->xunmap);
                break;
            }

            case MapNotify:
            {
                HandleXMap(&xevent->xmap);
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

            case PropertyNotify:
            {
                HandleXProperty(&xevent->xproperty);
                break;
            }

            case ClientMessage:
            {
                HandleXClientMessage(&xevent->xclient);
                break;
            }
        }
    }

    private XWindow CreateWindowHandle()
    {
        var dispatchService = XlibDispatchService.Instance;
        var display = dispatchService.Display;

        var defaultRootWindow = dispatchService.DefaultRootWindow;
        var defaultScreen = dispatchService.DefaultScreen;

        var defaultScreenWidth = XWidthOfScreen(defaultScreen);
        var defaultScreenHeight = XHeightOfScreen(defaultScreen);

        XWindow window;

        ThrowForLastErrorIfZero(window = XCreateWindow(
            display,
            defaultRootWindow,
            (int)(defaultScreenWidth * 0.125f),
            (int)(defaultScreenHeight * 0.125f),
            (uint)(defaultScreenWidth * 0.75f),
            (uint)(defaultScreenHeight * 0.75f),
            0,
            (int)CopyFromParent,
            InputOutput,
            (Visual*)CopyFromParent,
            0,
            null
        ));

        _ = XSelectInput(
            display,
            window,
            ExposureMask | VisibilityChangeMask | StructureNotifyMask | PropertyChangeMask
        );

        const int WmProtocolCount = 1;

        var wmProtocols = stackalloc Atom[WmProtocolCount] {
            dispatchService.GetAtom(WM_DELETE_WINDOW)
        };

        ThrowForLastErrorIfZero(XSetWMProtocols(
            display,
            window,
            wmProtocols,
            WmProtocolCount
        ));

        var gcHandle = GCHandle.Alloc(this, GCHandleType.Normal);
        var gcHandlePtr = (nuint)(nint)GCHandle.ToIntPtr(gcHandle);

        SendClientMessage(
            display,
            window,
            NoEventMask,
            window,
            dispatchService.GetAtom(_TERRAFX_CREATE_WINDOW),
            unchecked((nint)(uint)gcHandlePtr),
            (nint)(gcHandlePtr >> 32)
        );

        SetWindowTitle(dispatchService, display, window, _title);
        return window;
    }

    private PropertySet CreateProperties()
        => new PropertySet();

    private void DisposeWindowHandle()
    {
        AssertThread(ParentThread);
        AssertDisposing(_state);

        if (_handle.IsValueCreated)
        {
            var dispatchService = XlibDispatchService.Instance;
            var display = dispatchService.Display;
            var window = _handle.Value;

            var gcHandle = GCHandle.Alloc(this, GCHandleType.Normal);
            var gcHandlePtr = (nuint)(nint)GCHandle.ToIntPtr(gcHandle);

            SendClientMessage(
                display,
                window,
                NoEventMask,
                window,
                dispatchService.GetAtom(_TERRAFX_DISPOSE_WINDOW),
                unchecked((nint)(uint)gcHandlePtr),
                (nint)(gcHandlePtr >> 32)
            );

            _ = XDestroyWindow(display, window);
        }
    }

    private void HandleXClientMessage(XClientMessageEvent* xclientMessage)
    {
        var dispatchService = XlibDispatchService.Instance;

        if ((xclientMessage->format != 32) || (xclientMessage->message_type != dispatchService.GetAtom(WM_PROTOCOLS)))
        {
            return;
        }

        var eventAtom = (nuint)xclientMessage->data.l[0];

        if (eventAtom == dispatchService.GetAtom(WM_DELETE_WINDOW))
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

    private static void HandleXCirculate(XCirculateEvent* xcirculate) { }

    private void HandleXConfigure(XConfigureEvent* xconfigure)
    {
        if (xconfigure->send_event == False)
        {
            // The coordinates for non synthetic events are supposed
            // to be relative to the parent window but gives incorrect
            // coordinates when translated. We'll explicitly query
            // the location of our own origin for this case instead.

            XWindow child;

            ThrowForLastErrorIfZero(XTranslateCoordinates(
                xconfigure->display,
                xconfigure->window,
                XlibDispatchService.Instance.DefaultRootWindow,
                0,
                0,
                &xconfigure->x,
                &xconfigure->y,
                &child
            ));
        }

        var currentClientLocation = Vector2.Create(xconfigure->x, xconfigure->y);
        var currentClientSize = Vector2.Create(xconfigure->width, xconfigure->height);

        var previousClientLocation = _clientBounds.Location;
        var previousClientSize = _clientBounds.Size;

        _clientBounds = BoundingRectangle.CreateFromSize(currentClientLocation, currentClientSize);

        OnClientLocationChanged(previousClientLocation, currentClientLocation);
        OnClientSizeChanged(previousClientSize, currentClientSize);

        var previousLocation = _bounds.Location;
        var previousSize = _bounds.Size;

        var currentLocation = currentClientLocation + _frameExtents.Location;
        var currentSize = currentClientSize + _frameExtents.Size;

        _bounds = BoundingRectangle.CreateFromSize(currentLocation, currentSize);

        OnLocationChanged(previousLocation, currentLocation);
        OnSizeChanged(previousSize, currentSize);
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

    private static void HandleXExpose(XExposeEvent* xexpose) { }

    private void HandleXMap(XMapEvent* xmap)
        => UpdateWindowState(XlibDispatchService.Instance, xmap->display, xmap->@event);

    private void HandleXProperty(XPropertyEvent* xproperty)
    {
        var dispatchService = XlibDispatchService.Instance;
        var atom = xproperty->atom;

        if (atom == dispatchService.GetAtom(_NET_FRAME_EXTENTS))
        {
            HandleXPropertyNetFrameExtents(xproperty, dispatchService);
        }
        else if (atom == dispatchService.GetAtom(_NET_WM_NAME))
        {
            HandleXPropertyNetWmName(xproperty, dispatchService);
        }
        else if (atom == dispatchService.GetAtom(_NET_WM_STATE))
        {
            HandleXPropertyNetWmState(xproperty, dispatchService);
        }
        else if (atom == XA_WM_HINTS)
        {
            HandleXPropertyWmHints(xproperty, dispatchService);
        }
        else if (atom == XA_WM_NAME)
        {
            HandleXPropertyWmName(xproperty, dispatchService);
        }
        else if (atom == dispatchService.GetAtom(WM_STATE))
        {
            HandleXPropertyWmState(xproperty, dispatchService);
        }
    }

    private void HandleXPropertyNetFrameExtents(XPropertyEvent* xproperty, XlibDispatchService dispatchService)
        => UpdateFrameExtents(dispatchService, xproperty->display, xproperty->window);

    private void HandleXPropertyNetWmState(XPropertyEvent* xproperty, XlibDispatchService dispatchService)
        => UpdateWindowState(XlibDispatchService.Instance, xproperty->display, xproperty->window);

    private void HandleXPropertyWmHints(XPropertyEvent* xproperty, XlibDispatchService dispatchService)
    {
        XWMHints* wmHints = null;

        try
        {
            wmHints = XGetWMHints(xproperty->display, xproperty->window);

            _isEnabled = (wmHints != null)
                      && ((wmHints->flags * InputHint) != 0)
                      && (wmHints->input != False);
        }
        finally
        {
            if (wmHints != null)
            {
                _ = XFree(wmHints);
            }
        }
    }

    private void HandleXPropertyNetWmName(XPropertyEvent* xproperty, XlibDispatchService dispatchService)
        => UpdateWindowTitle(dispatchService, xproperty->display, xproperty->window);

    private void HandleXPropertyWmName(XPropertyEvent* xproperty, XlibDispatchService dispatchService)
        => UpdateWindowTitle(dispatchService, xproperty->display, xproperty->window);

    private void HandleXPropertyWmState(XPropertyEvent* xproperty, XlibDispatchService dispatchService)
        => UpdateWindowState(dispatchService, xproperty->display, xproperty->window);

    private void HandleXUnmap(XUnmapEvent* xunmap)
        => UpdateWindowState(XlibDispatchService.Instance, xunmap->display, xunmap->@event);

    private void HandleXVisibility(XVisibilityEvent* xvisibility) => _isVisible = xvisibility->state != VisibilityFullyObscured;

    private void OnClientLocationChanged(Vector2 previousClientLocation, Vector2 currentClientLocation)
    {
        if ((ClientLocationChanged is not null) && (previousClientLocation != currentClientLocation))
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
        if ((LocationChanged is not null) && (previousLocation != currentLocation))
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

    private void UpdateFrameExtents(XlibDispatchService dispatchService, Display* display, XWindow window)
    {
        if (dispatchService.GetAtomIsSupported(_NET_FRAME_EXTENTS))
        {
            Atom actualType;
            int actualFormat;
            nuint itemCount;
            nuint bytesRemaining;
            nint* cardinals;

            _ = XGetWindowProperty(
                display,
                window,
                dispatchService.GetAtom(_NET_FRAME_EXTENTS),
                0,
                4,
                False,
                XA_CARDINAL,
                &actualType,
                &actualFormat,
                &itemCount,
                &bytesRemaining,
                (byte**)&cardinals
            );

            if ((actualType == XA_CARDINAL) && (actualFormat == 32) && (itemCount == 4) && (bytesRemaining == 0))
            {
                // The cardinals given are left, right, top and bottom; respectively
                // Using these, we construct a rectangle that specifies the adjustments
                // made to a given client rectangle to compute the non-client size and
                // location.

                _frameExtents = BoundingRectangle.CreateFromSize(
                    -Vector2.Create(cardinals[0], cardinals[2]),
                    Vector2.Create(cardinals[1], cardinals[3]) + Vector2.Create(cardinals[0], cardinals[2])
                );
            }
            else
            {
                _frameExtents = default;
            }
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    private void UpdateWindowState(XlibDispatchService dispatchService, Display* display, XWindow window)
    {
        if (dispatchService.GetAtomIsSupported(_NET_WM_STATE))
        {
            Atom actualType;
            int actualFormat;
            nuint itemCount;
            nuint bytesRemaining;
            nuint* netWmState;

            _ = XGetWindowProperty(
                display,
                window,
                dispatchService.GetAtom(_NET_WM_STATE),
                0,
                nint.MaxValue,
                False,
                XA_ATOM,
                &actualType,
                &actualFormat,
                &itemCount,
                &bytesRemaining,
                (byte**)&netWmState
            );

            if ((actualType == XA_ATOM) && (actualFormat == 32) && (bytesRemaining == 0))
            {
                var foundNetWmStateFocused = false;
                var foundNetWmStateHidden = false;
                var foundNetWmStateMaximizedHorz = false;
                var foundNetWmStateMaximizedVert = false;

                for (nuint i = 0; i < itemCount; i++)
                {
                    if (netWmState[i] == dispatchService.GetAtom(_NET_WM_STATE_FOCUSED))
                    {
                        foundNetWmStateFocused = true;
                    }
                    else if (netWmState[i] == dispatchService.GetAtom(_NET_WM_STATE_HIDDEN))
                    {
                        foundNetWmStateHidden = true;
                    }
                    else if (netWmState[i] == dispatchService.GetAtom(_NET_WM_STATE_MAXIMIZED_HORZ))
                    {
                        foundNetWmStateMaximizedHorz = true;
                    }
                    else if (netWmState[i] == dispatchService.GetAtom(_NET_WM_STATE_MAXIMIZED_VERT))
                    {
                        foundNetWmStateMaximizedVert = true;
                    }
                }

                _isActive = foundNetWmStateFocused;

                if (foundNetWmStateHidden)
                {
                    _windowState = WindowState.Minimized;
                }
                else if (foundNetWmStateMaximizedHorz && foundNetWmStateMaximizedVert)
                {
                    _windowState = WindowState.Maximized;
                }
                else
                {
                    _windowState = WindowState.Normal;
                }
            }
            else
            {
                _isActive = false;
                _windowState = WindowState.Hidden;
            }
        }
        else
        {
            ThrowNotImplementedException();
        }
    }

    private void UpdateWindowTitle(XlibDispatchService dispatchService, Display* display, XWindow window)
    {
        Atom actualType;
        int actualFormat;
        nuint bytesRemaining;
        nuint itemCount;
        sbyte* wmName;

        if (dispatchService.GetAtomIsSupported(_NET_WM_NAME))
        {

            _ = XGetWindowProperty(
                display,
                window,
                dispatchService.GetAtom(_NET_WM_NAME),
                0,
                nint.MaxValue,
                False,
                dispatchService.GetAtom(UTF8_STRING),
                &actualType,
                &actualFormat,
                &itemCount,
                &bytesRemaining,
                (byte**)&wmName
            );
        }
        else
        {
            XTextProperty textProperty;
            _ = XGetWMName(display, window, &textProperty);

            actualType = textProperty.encoding;
            actualFormat = textProperty.format;
            bytesRemaining = 0;
            itemCount = textProperty.nitems;
            wmName = (sbyte*)textProperty.value;
        }

        if ((actualType == dispatchService.GetAtom(UTF8_STRING)) && (actualFormat == 8) && (bytesRemaining == 0))
        {
            _title = GetUtf8Span(wmName, checked((int)itemCount)).GetString() ?? string.Empty;
        }
        else
        {
            _title = string.Empty;
        }
    }
}
