// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
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
    private readonly XWindow _handle;
    private readonly ReadingDirection _readingDirection;

    private BoundingRectangle _bounds;
    private BoundingRectangle _clientBounds;
    private BoundingRectangle _frameExtents;
    private bool _isActive;
    private bool _isEnabled;
    private bool _isVisible;
    private string _title = null!;
    private WindowState _windowState;

    private VolatileState _state;

    internal XlibWindow(XlibUIDispatcher dispatcher)
        : base(dispatcher)
    {
        _flowDirection = FlowDirection.TopToBottom;
        _handle = CreateHandle(this, Service.Display);
        _readingDirection = ReadingDirection.LeftToRight;

        _ = _state.Transition(to: Initialized);

        static XWindow CreateHandle(XlibWindow window, Display* display)
        {
            XLockDisplay(display);

            try
            {
                return CreateHandleInternal(window, display);
            }
            finally
            {
                XUnlockDisplay(display);
            }
        }

        static XWindow CreateHandleInternal(XlibWindow window, Display* display)
        {
            XWindow handle;

            var service = window.Service;

            var defaultRootWindow = service.DefaultRootWindow;
            var defaultScreen = service.DefaultScreen;

            var defaultScreenWidth = XWidthOfScreen(defaultScreen);
            var defaultScreenHeight = XHeightOfScreen(defaultScreen);

            ThrowForLastErrorIfZero(handle = XCreateWindow(
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
                handle,
                ExposureMask | VisibilityChangeMask | StructureNotifyMask | PropertyChangeMask
            );

            const int WmProtocolCount = 1;

            var wmProtocols = stackalloc Atom[WmProtocolCount] {
                service.GetAtom(WM_DELETE_WINDOW)
            };

            ThrowForLastErrorIfZero(XSetWMProtocols(
                display,
                handle,
                wmProtocols,
                WmProtocolCount
            ));

            var gcHandle = (nuint)(nint)GCHandle.ToIntPtr(GCHandle.Alloc(window, GCHandleType.Normal));

            SendClientMessage(
                display,
                handle,
                NoEventMask,
                handle,
                service.GetAtom(_TERRAFX_CREATE_WINDOW),
                unchecked((nint)(uint)gcHandle),
                (nint)(gcHandle >> 32)
            );

            SetWindowTitle(service, display, handle, nameof(XlibWindow));
            return handle;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="XlibWindow" /> class.</summary>
    ~XlibWindow() => Dispose(isDisposing: false);

    /// <inheritdoc />
    public override event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientLocationChanged;

    /// <inheritdoc />
    public override event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientSizeChanged;

    /// <inheritdoc />
    public override event EventHandler<PropertyChangedEventArgs<Vector2>>? LocationChanged;

    /// <inheritdoc />
    public override event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

    /// <inheritdoc />
    public override BoundingRectangle Bounds => _bounds;

    /// <inheritdoc />
    public override BoundingRectangle ClientBounds => _clientBounds;

    /// <inheritdoc cref="UIDispatcherObject.Dispatcher" />
    public new XlibUIDispatcher Dispatcher => base.Dispatcher.As<XlibUIDispatcher>();

    /// <inheritdoc />
    public override FlowDirection FlowDirection => _flowDirection;

    /// <summary>Gets the underlying handle for the window.</summary>
    public XWindow Handle
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _handle;
        }
    }

    /// <inheritdoc />
    public override bool IsActive => _isActive;

    /// <inheritdoc />
    public override bool IsEnabled => _isEnabled;

    /// <inheritdoc />
    public override bool IsVisible => _isVisible;

    /// <inheritdoc />
    public override ReadingDirection ReadingDirection => _readingDirection;

    /// <inheritdoc />
    public override string Title => _title;

    /// <inheritdoc cref="UIDispatcherObject.Service" />
    public new XlibUIService Service => base.Service.As<XlibUIService>();

    /// <inheritdoc />
    public override WindowState WindowState => _windowState;

    /// <inheritdoc />
    protected override IntPtr SurfaceContextHandle => (nint)Service.Display;

    /// <inheritdoc />
    protected override IntPtr SurfaceHandle => Handle;

    /// <inheritdoc />
    protected override GraphicsSurfaceKind SurfaceKind => GraphicsSurfaceKind.Xlib;

    /// <inheritdoc />
    public override void Activate()
    {
        if (!TryActivate())
        {
            ThrowExternalException(nameof(XRaiseWindow), 0);
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="XSendEvent(Display*, XWindow, int, nint, XEvent*)" /> failed.</exception>
    public override void Close()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        var service = Service;
        var display = service.Display;
        var window = _handle;

        if (service.GetAtomIsSupported(_NET_CLOSE_WINDOW))
        {
            SendClientMessage(
                display,
                service.DefaultRootWindow,
                SubstructureNotifyMask | SubstructureRedirectMask,
                window,
                service.GetAtom(_NET_CLOSE_WINDOW),
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
                service.GetAtom(WM_PROTOCOLS),
                service.GetAtom(WM_DELETE_WINDOW)
            );
        }
    }

    /// <inheritdoc />
    public override void Disable()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (IsEnabled)
        {
            var wmHints = new XWMHints {
                flags = InputHint,
                input = False
            };
            _ = XSetWMHints(Service.Display, _handle, &wmHints);
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="XGetWMHints(Display*, XWindow)" /> failed.</exception>
    public override void Enable()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (!IsEnabled)
        {
            var wmHints = new XWMHints {
                flags = InputHint,
                input = True
            };
            _ = XSetWMHints(Service.Display, _handle, &wmHints);
        }
    }

    /// <inheritdoc />
    public override void Hide()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (WindowState != WindowState.Hidden)
        {
            var service = Service;

            ThrowForLastErrorIfZero(XWithdrawWindow(
                service.Display,
                _handle,
                XScreenNumberOfScreen(service.DefaultScreen)
            ));
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="XGetWindowAttributes(Display*, XWindow, XWindowAttributes*)" /> failed.</exception>
    public override void Maximize()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        var windowState = _windowState;

        if (windowState != WindowState.Maximized)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

            if (windowState == WindowState.Minimized)
            {
                _ = XUnmapWindow(display, window);
                windowState = WindowState.Hidden;
            }

            if (windowState == WindowState.Hidden)
            {
                ShowWindow(display, window);
            }

            if (service.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_HORZ) && service.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_VERT))
            {
                SendClientMessage(
                    display,
                    service.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    service.GetAtom(_NET_WM_STATE),
                    _NET_WM_STATE_ADD,
                    service.GetAtom(_NET_WM_STATE_MAXIMIZED_HORZ),
                    service.GetAtom(_NET_WM_STATE_MAXIMIZED_VERT),
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
    public override void Minimize()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        var windowState = _windowState;

        if (windowState != WindowState.Minimized)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

            if (windowState == WindowState.Hidden)
            {
                ShowWindow(display, window, IconicState);
            }

            ThrowForLastErrorIfZero(XIconifyWindow(
                display,
                window,
                XScreenNumberOfScreen(service.DefaultScreen)
            ));
        }
    }

    /// <inheritdoc />
    public override void Relocate(Vector2 location)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (_bounds.Location != location)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

            if (service.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    service.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    service.GetAtom(_NET_MOVERESIZE_WINDOW),
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
    public override void RelocateClient(Vector2 location)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (_clientBounds.Location != location)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

            if (service.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    service.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    service.GetAtom(_NET_MOVERESIZE_WINDOW),
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
    public override void Resize(Vector2 size)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (_bounds.Size != size)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

            if (service.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    service.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    service.GetAtom(_NET_MOVERESIZE_WINDOW),
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
    public override void ResizeClient(Vector2 size)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (_clientBounds.Size != size)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

            if (service.GetAtomIsSupported(_NET_MOVERESIZE_WINDOW))
            {
                SendClientMessage(
                    display,
                    service.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    service.GetAtom(_NET_MOVERESIZE_WINDOW),
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
    public override void Restore()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        var windowState = _windowState;

        if (windowState != WindowState.Normal)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

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

            if (service.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_HORZ) && service.GetAtomIsSupported(_NET_WM_STATE_MAXIMIZED_VERT))
            {
                SendClientMessage(
                    display,
                    service.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    service.GetAtom(_NET_WM_STATE),
                    _NET_WM_STATE_REMOVE,
                    service.GetAtom(_NET_WM_STATE_MAXIMIZED_HORZ),
                    service.GetAtom(_NET_WM_STATE_MAXIMIZED_VERT),
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
    public override void SetTitle(string title)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (_title != title)
        {
            var service = Service;
            SetWindowTitle(service, service.Display, _handle, title);
        }
    }

    /// <inheritdoc />
    public override void Show()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (_windowState == WindowState.Hidden)
        {
            _ = XMapWindow(Service.Display, _handle);
        }
    }

    /// <inheritdoc />
    public override bool TryActivate()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(XlibWindow));

        if (!_isActive)
        {
            var service = Service;
            var display = service.Display;
            var window = _handle;

            if (service.GetAtomIsSupported(_NET_ACTIVE_WINDOW))
            {
                SendClientMessage(
                    display,
                    service.DefaultRootWindow,
                    SubstructureNotifyMask | SubstructureRedirectMask,
                    window,
                    service.GetAtom(_NET_ACTIVE_WINDOW),
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
                DisposeHandle();
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

    private void DisposeHandle()
    {
        var service = Service;
        var display = service.Display;
        var handle = _handle;

        var gcHandle = GCHandle.Alloc(this, GCHandleType.Normal);
        var gcHandlePtr = (nuint)(nint)GCHandle.ToIntPtr(gcHandle);

        SendClientMessage(
            display,
            handle,
            NoEventMask,
            handle,
            service.GetAtom(_TERRAFX_DISPOSE_WINDOW),
            unchecked((nint)(uint)gcHandlePtr),
            (nint)(gcHandlePtr >> 32)
        );

        _ = XDestroyWindow(display, handle);
    }

    private void HandleXClientMessage(XClientMessageEvent* xclientMessage)
    {
        var service = Service;

        if ((xclientMessage->format != 32) || (xclientMessage->message_type != service.GetAtom(WM_PROTOCOLS)))
        {
            return;
        }

        var eventAtom = (nuint)xclientMessage->data.l[0];

        if (eventAtom == service.GetAtom(WM_DELETE_WINDOW))
        {
            // If we are already disposing, then Dispose is happening on some other thread
            // and Close was called in order for us to continue disposal on the parent thread.
            // Otherwise, this is a normal close call and we should ensure we step through the
            // various states properly.

            if (_state == Disposing)
            {
                DisposeHandle();
            }
            else
            {
                Dispose();
            }
        }
    }

    private void HandleXCirculate(XCirculateEvent* xcirculate) { }

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
                Service.DefaultRootWindow,
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
        _ = Dispatcher.RemoveWindow(_handle);
    }

    private static void HandleXExpose(XExposeEvent* xexpose) { }

    private void HandleXMap(XMapEvent* xmap) => UpdateWindowState(Service, xmap->display, xmap->@event);

    private void HandleXProperty(XPropertyEvent* xproperty)
    {
        var service = Service;
        var atom = xproperty->atom;

        if (atom == service.GetAtom(_NET_FRAME_EXTENTS))
        {
            HandleXPropertyNetFrameExtents(xproperty, service);
        }
        else if (atom == service.GetAtom(_NET_WM_NAME))
        {
            HandleXPropertyNetWmName(xproperty, service);
        }
        else if (atom == service.GetAtom(_NET_WM_STATE))
        {
            HandleXPropertyNetWmState(xproperty, service);
        }
        else if (atom == XA_WM_HINTS)
        {
            HandleXPropertyWmHints(xproperty, service);
        }
        else if (atom == XA_WM_NAME)
        {
            HandleXPropertyWmName(xproperty, service);
        }
        else if (atom == service.GetAtom(WM_STATE))
        {
            HandleXPropertyWmState(xproperty, service);
        }
    }

    private void HandleXPropertyNetFrameExtents(XPropertyEvent* xproperty, XlibUIService service)
        => UpdateFrameExtents(service, xproperty->display, xproperty->window);

    private void HandleXPropertyNetWmState(XPropertyEvent* xproperty, XlibUIService service)
        => UpdateWindowState(Service, xproperty->display, xproperty->window);

    private void HandleXPropertyWmHints(XPropertyEvent* xproperty, XlibUIService service)
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

    private void HandleXPropertyNetWmName(XPropertyEvent* xproperty, XlibUIService service)
        => UpdateWindowTitle(service, xproperty->display, xproperty->window);

    private void HandleXPropertyWmName(XPropertyEvent* xproperty, XlibUIService service)
        => UpdateWindowTitle(service, xproperty->display, xproperty->window);

    private void HandleXPropertyWmState(XPropertyEvent* xproperty, XlibUIService service)
        => UpdateWindowState(service, xproperty->display, xproperty->window);

    private void HandleXUnmap(XUnmapEvent* xunmap)
        => UpdateWindowState(Service, xunmap->display, xunmap->@event);

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

    private void UpdateFrameExtents(XlibUIService service, Display* display, XWindow window)
    {
        if (service.GetAtomIsSupported(_NET_FRAME_EXTENTS))
        {
            Atom actualType;
            int actualFormat;
            nuint itemCount;
            nuint bytesRemaining;
            nint* cardinals;

            _ = XGetWindowProperty(
                display,
                window,
                service.GetAtom(_NET_FRAME_EXTENTS),
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

    private void UpdateWindowState(XlibUIService service, Display* display, XWindow window)
    {
        if (service.GetAtomIsSupported(_NET_WM_STATE))
        {
            Atom actualType;
            int actualFormat;
            nuint itemCount;
            nuint bytesRemaining;
            nuint* netWmState;

            _ = XGetWindowProperty(
                display,
                window,
                service.GetAtom(_NET_WM_STATE),
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
                    if (netWmState[i] == service.GetAtom(_NET_WM_STATE_FOCUSED))
                    {
                        foundNetWmStateFocused = true;
                    }
                    else if (netWmState[i] == service.GetAtom(_NET_WM_STATE_HIDDEN))
                    {
                        foundNetWmStateHidden = true;
                    }
                    else if (netWmState[i] == service.GetAtom(_NET_WM_STATE_MAXIMIZED_HORZ))
                    {
                        foundNetWmStateMaximizedHorz = true;
                    }
                    else if (netWmState[i] == service.GetAtom(_NET_WM_STATE_MAXIMIZED_VERT))
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

    private void UpdateWindowTitle(XlibUIService service, Display* display, XWindow window)
    {
        Atom actualType;
        int actualFormat;
        nuint bytesRemaining;
        nuint itemCount;
        sbyte* wmName;

        if (service.GetAtomIsSupported(_NET_WM_NAME))
        {
            _ = XGetWindowProperty(
                display,
                window,
                service.GetAtom(_NET_WM_NAME),
                0,
                nint.MaxValue,
                False,
                service.GetAtom(UTF8_STRING),
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

        if ((actualType == service.GetAtom(UTF8_STRING)) && (actualFormat == 8) && (bytesRemaining == 0))
        {
            _title = GetUtf8Span(wmName, checked((int)itemCount)).GetString() ?? string.Empty;
        }
        else
        {
            _title = string.Empty;
        }
    }
}
