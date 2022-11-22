// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Graphics;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using TerraFX.UI.Advanced;
using TerraFX.Utilities;
using static TerraFX.Interop.Windows.GWL;
using static TerraFX.Interop.Windows.HWND;
using static TerraFX.Interop.Windows.SW;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.WS;
using static TerraFX.UI.UIService;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.Win32Utilities;

namespace TerraFX.UI;

/// <summary>Defines a window.</summary>
public sealed unsafe class UIWindow : UIDispatcherObject, IGraphicsSurface
{
    private readonly UIFlowDirection _flowDirection;
    private readonly HWND _handle;
    private readonly UIReadingDirection _readingDirection;

    private BoundingRectangle _bounds;
    private BoundingRectangle _clientBounds;
    private uint _extendedStyle;
    private bool _isActive;
    private uint _style;
    private string _title = null!;
    private UIWindowState _windowState;

    internal UIWindow(UIDispatcher dispatcher)
        : base(dispatcher)
    {
        _flowDirection = UI.UIFlowDirection.TopToBottom;
        _handle = CreateHandle(this, Service.ClassAtom);
        _readingDirection = UI.UIReadingDirection.LeftToRight;

        static HWND CreateHandle(UIWindow window, ushort classAtom)
        {
            HWND hWnd;

            fixed (char* lpWindowName = nameof(UIWindow))
            {
                ThrowForLastErrorIfZero(hWnd = CreateWindowExW(
                    WS_EX_OVERLAPPEDWINDOW,
                    (ushort*)classAtom,
                    (ushort*)lpWindowName,
                    WS_OVERLAPPEDWINDOW,
                    X: CW_USEDEFAULT,
                    Y: CW_USEDEFAULT,
                    nWidth: CW_USEDEFAULT,
                    nHeight: CW_USEDEFAULT,
                    hWndParent: HWND_DESKTOP,
                    hMenu: HMENU.NULL,
                    hInstance: EntryPointModule,
                    lpParam: (void*)GCHandle.ToIntPtr(GCHandle.Alloc(window))
                ));
            }

            return hWnd;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="UIWindow" /> class.</summary>
    ~UIWindow() => Dispose(isDisposing: false);

    /// <summary>Occurs when the <see cref="ClientLocation" /> property changes.</summary>
    public event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientLocationChanged;

    /// <summary>Occurs when the <see cref="ClientSize" /> property changes.</summary>
    public event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientSizeChanged;

    /// <summary>Occurs when the <see cref="Location" /> property changes.</summary>
    public event EventHandler<PropertyChangedEventArgs<Vector2>>? LocationChanged;

    /// <summary>Occurs when the <see cref="Size" /> property changes.</summary>
    public event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

    event EventHandler<PropertyChangedEventArgs<Vector2>>? IGraphicsSurface.SizeChanged
    {
        add
        {
            ClientSizeChanged += value;
        }

        remove
        {
            ClientSizeChanged -= value;
        }
    }

    /// <summary>Gets the bounds of the window.</summary>
    public BoundingRectangle Bounds => _bounds;

    /// <summary>Gets the bounds of the client area for the window.</summary>
    public BoundingRectangle ClientBounds => _clientBounds;

    /// <summary>Gets the location of the client area for the window.</summary>
    public Vector2 ClientLocation => ClientBounds.Location;

    /// <summary>Gets the size of the client area for the window.</summary>
    public Vector2 ClientSize => ClientBounds.Size;

    /// <summary>Gets flow direction for the window.</summary>
    public UIFlowDirection FlowDirection => _flowDirection;

    /// <summary>Gets <c>true</c> if the window is active; otherwise, <c>false</c>.</summary>
    public bool IsActive => _isActive;

    /// <summary>Gets <c>true</c> if the window is enabled; otherwise, <c>false</c>.</summary>
    public bool IsEnabled => (_style & WS_DISABLED) == 0;

    /// <summary>Gets <c>true</c> if the window is visible; otherwise, <c>false</c>.</summary>
    public bool IsVisible => (_style & WS_VISIBLE) != 0;

    /// <summary>Gets the location of the window.</summary>
    public Vector2 Location => Bounds.Location;

    /// <summary>Gets the reading directionfor the window.</summary>
    public UIReadingDirection ReadingDirection => _readingDirection;

    /// <summary>Gets the size of the window.</summary>
    public Vector2 Size => Bounds.Size;

    /// <summary>Gets the title for the window.</summary>
    public string Title => _title;

    /// <summary>Gets the state for the window.</summary>
    public UIWindowState WindowState => _windowState;

    internal HWND Handle
    {
        get
        {
            ThrowIfDisposed();
            return _handle;
        }
    }

    IntPtr IGraphicsSurface.ContextHandle => EntryPointModule;

    IntPtr IGraphicsSurface.Handle => Handle;

    GraphicsSurfaceKind IGraphicsSurface.Kind => GraphicsSurfaceKind.Win32;

    Vector2 IGraphicsSurface.Size => ClientSize;

    /// <summary>Activates the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Activate()
    {
        if (!TryActivate())
        {
            ExceptionUtilities.ThrowExternalException(nameof(SetForegroundWindow), 0);
        }
    }

    /// <summary>Closes the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Close()
    {
        ThrowIfDisposed();
        _ = SendMessageW(_handle, WM_CLOSE, wParam: 0u, lParam: 0);
    }

    /// <summary>Disables the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Disable()
    {
        ThrowIfDisposed();

        if (IsEnabled)
        {
            _ = EnableWindow(_handle, FALSE);
        }
    }

    /// <summary>Enables the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Enable()
    {
        ThrowIfDisposed();

        if (!IsEnabled)
        {
            _ = EnableWindow(_handle, TRUE);
        }
    }

    /// <summary>Hides the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Hide()
    {
        ThrowIfDisposed();

        if (WindowState != UIWindowState.Hidden)
        {
            _ = ShowWindow(_handle, SW_HIDE);
        }
    }

    /// <summary>Maximizes the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Maximize()
    {
        ThrowIfDisposed();

        if (WindowState != UIWindowState.Maximized)
        {
            _ = ShowWindow(_handle, SW_MAXIMIZE);
        }
    }

    /// <summary>Minimizes the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Minimize()
    {
        ThrowIfDisposed();

        if (WindowState != UIWindowState.Minimized)
        {
            _ = ShowWindow(_handle, SW_MINIMIZE);
        }
    }

    /// <summary>Relocates the window to the specified location.</summary>
    /// <param name="location">The new location for the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Relocate(Vector2 location)
    {
        ThrowIfDisposed();

        if (_bounds.Location != location)
        {
            ThrowExternalExceptionIfFalse(MoveWindow(
                _handle,
                (int)location.X,
                (int)location.Y,
                (int)_bounds.Size.X,
                (int)_bounds.Size.Y,
                bRepaint: TRUE
            ));
        }
    }

    /// <summary>Relocates the client area for the window to the specified location.</summary>
    /// <param name="location">The new location for the client area of the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void RelocateClient(Vector2 location)
    {
        ThrowIfDisposed();

        if (_clientBounds.Location != location)
        {
            var topLeft = location;
            var bottomRight = topLeft + ClientSize;

            var rect = new RECT {
                left = (int)topLeft.X,
                top = (int)topLeft.Y,
                right = (int)bottomRight.X,
                bottom = (int)bottomRight.Y,
            };

            ThrowExternalExceptionIfFalse(AdjustWindowRectEx(&rect, _style, bMenu: FALSE, _extendedStyle));
            ThrowExternalExceptionIfFalse(MoveWindow(_handle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, bRepaint: TRUE));
        }
    }

    /// <summary>Resizes the window to the specified size.</summary>
    /// <param name="size">The new size for the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Resize(Vector2 size)
    {
        ThrowIfDisposed();

        if (_bounds.Size != size)
        {
            ThrowExternalExceptionIfFalse(MoveWindow(
                _handle,
                (int)_bounds.Location.X,
                (int)_bounds.Location.Y,
                (int)size.X,
                (int)size.Y,
                bRepaint: TRUE
            ));
        }
    }

    /// <summary>Resizes the client area for the window to the specified size.</summary>
    /// <param name="size">The new size for the client area of the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void ResizeClient(Vector2 size)
    {
        ThrowIfDisposed();

        if (_clientBounds.Size != size)
        {
            var topLeft = ClientLocation;
            var bottomRight = topLeft + size;

            var rect = new RECT {
                left = (int)topLeft.X,
                top = (int)topLeft.Y,
                right = (int)bottomRight.X,
                bottom = (int)bottomRight.Y,
            };

            ThrowExternalExceptionIfFalse(AdjustWindowRectEx(&rect, _style, bMenu: FALSE, _extendedStyle));
            ThrowExternalExceptionIfFalse(MoveWindow(_handle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, bRepaint: TRUE));
        }
    }

    /// <summary>Restores the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Restore()
    {
        ThrowIfDisposed();

        if (WindowState != UIWindowState.Normal)
        {
            _ = ShowWindow(_handle, SW_RESTORE);
        }
    }

    /// <summary>Sets the title to the specified value.</summary>
    /// <param name="title">The new title for the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void SetTitle(string title)
    {
        ThrowIfDisposed();

        if (_title != title)
        {
            title ??= string.Empty;

            fixed (char* pTitle = title)
            {
                ThrowExternalExceptionIfFalse(SetWindowTextW(_handle, (ushort*)pTitle));
            }
        }
    }

    /// <summary>Shows the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public void Show()
    {
        ThrowIfDisposed();

        if (WindowState == UIWindowState.Hidden)
        {
            _ = ShowWindow(_handle, SW_SHOW);
        }
    }

    /// <summary>Tries to activate the window.</summary>
    /// <returns><c>true</c> if the window was succesfully activated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public bool TryActivate() => _isActive || (SetForegroundWindow(_handle) != FALSE);

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        // We are only allowed to dispose of the window handle from the parent
        // thread. So, if we are on the wrong thread, we will close the window
        // and call DisposeHandle from the appropriate thread.

        if (Thread.CurrentThread != ParentThread)
        {
            Close();
        }
        else
        {
            DisposeHandle(_handle);
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    internal LRESULT ProcessWindowMessage(uint msg, WPARAM wParam, LPARAM lParam)
    {
        ThrowIfNotThread(ParentThread);
        LRESULT result;

        switch (msg)
        {
            case WM_CREATE:
            {
                result = HandleWmCreate(wParam, lParam);
                break;
            }

            case WM_DESTROY:
            {
                result = HandleWmDestroy(wParam, lParam);
                break;
            }

            case WM_MOVE:
            {
                result = HandleWmMove(wParam, lParam);
                break;
            }

            case WM_SIZE:
            {
                result = HandleWmSize(wParam, lParam);
                break;
            }

            case WM_ACTIVATE:
            {
                result = HandleWmActivate(wParam, lParam);
                break;
            }

            case WM_ENABLE:
            {
                result = HandleWmEnable(wParam, lParam);
                break;
            }

            case WM_SETTEXT:
            {
                result = HandleWmSetText(wParam, lParam);
                break;
            }

            case WM_CLOSE:
            {
                result = HandleWmClose(wParam, lParam);
                break;
            }

            case WM_SHOWWINDOW:
            {
                result = HandleWmShowWindow(wParam, lParam);
                break;
            }

            case WM_WINDOWPOSCHANGED:
            {
                result = HandleWmWindowPosChanged(wParam, lParam);
                break;
            }

            case WM_STYLECHANGED:
            {
                result = HandleWmStyleChanged(wParam, lParam);
                break;
            }

            default:
            {
                result = DefWindowProcW(_handle, msg, wParam, lParam);
                break;
            }
        }

        return result;
    }

    private static void DisposeHandle(HWND handle)
    {
        if (handle != HWND.NULL)
        {
            ThrowExternalExceptionIfFalse(DestroyWindow(handle));
        }
    }

    private LRESULT HandleWmActivate(WPARAM wParam, LPARAM lParam)
    {
        _isActive = LOWORD(wParam) != WA_INACTIVE;
        return 0;
    }

    private LRESULT HandleWmClose(WPARAM wParam, LPARAM lParam)
    {
        // If we are already disposing, then Dispose is happening on some other thread
        // and Close was called in order for us to continue disposal on the parent thread.
        // Otherwise, this is a normal close call and we should ensure we step through the
        // various states properly.

        if (IsDisposed)
        {
            DisposeHandle(_handle);
        }
        else
        {
            Dispose();
        }

        return 0;
    }

    private LRESULT HandleWmCreate(WPARAM wParam, LPARAM lParam)
    {
        var createStruct = (CREATESTRUCTW*)lParam;

        _extendedStyle = createStruct->dwExStyle;
        _style = unchecked((uint)createStruct->style);
        _title = GetUtf16Span(createStruct->lpszName).GetString() ?? "";

        return 0;
    }

    private LRESULT HandleWmDestroy(WPARAM wParam, LPARAM lParam)
    {
        // We handle this here to ensure we transition to the appropriate state in the case
        // an end-user called DestroyWindow themselves. The assumption here is that this was
        // done "properly" if we are Disposing, in which case we don't need to do anything.
        // Otherwise, this was triggered externally and we should just switch the state to
        // be disposed.

        MarkDisposed();
        _ = Dispatcher.RemoveWindow(_handle);

        return 0;
    }

    private LRESULT HandleWmEnable(WPARAM wParam, LPARAM lParam)
    {
        if (wParam != (WPARAM)FALSE)
        {
            _style &= ~(uint)WS_DISABLED;
        }
        else
        {
            _style |= WS_DISABLED;
        }
        return 0;
    }

    private LRESULT HandleWmMove(WPARAM wParam, LPARAM lParam)
    {
        var previousClientLocation = _clientBounds.Location;
        var currentClientLocation = Vector2.Create(LOWORD(lParam), HIWORD(lParam));

        _clientBounds.Location = currentClientLocation;
        OnClientLocationChanged(previousClientLocation, currentClientLocation);

        return 0;
    }

    private LRESULT HandleWmSetText(WPARAM wParam, LPARAM lParam)
    {
        var result = DefWindowProcW(_handle, WM_SETTEXT, wParam, lParam);

        if (result == TRUE)
        {
            // We only need to update the title if the text was set
            _title = GetUtf16Span((ushort*)lParam).GetString() ?? "";
        }

        return result;
    }

    private LRESULT HandleWmShowWindow(WPARAM wParam, LPARAM lParam)
    {
        if (wParam != (WPARAM)FALSE)
        {
            _style |= WS_VISIBLE;
        }
        else
        {
            _style &= ~(uint)WS_VISIBLE;
        }
        return 0;
    }

    private LRESULT HandleWmSize(WPARAM wParam, LPARAM lParam)
    {
        _windowState = (uint)wParam switch {
            SIZE_RESTORED => UIWindowState.Normal,
            SIZE_MINIMIZED => UIWindowState.Minimized,
            SIZE_MAXIMIZED => UIWindowState.Maximized,
            SIZE_MAXHIDE => UIWindowState.Hidden,
            _ => _windowState,
        };

        var previousClientSize = _clientBounds.Size;
        var currentClientSize = Vector2.Create(LOWORD(lParam), HIWORD(lParam));

        _clientBounds.Size = currentClientSize;
        OnClientSizeChanged(previousClientSize, currentClientSize);

        return 0;
    }

    private LRESULT HandleWmStyleChanged(WPARAM wParam, LPARAM lParam)
    {
        var styleStruct = (STYLESTRUCT*)lParam;

        if (unchecked((nint)wParam) == GWL_EXSTYLE)
        {
            _extendedStyle = styleStruct->styleNew;
        }
        else if (unchecked((nint)wParam) == GWL_STYLE)
        {
            _style = styleStruct->styleNew;
        }

        return 0;
    }

    private LRESULT HandleWmWindowPosChanged(WPARAM wParam, LPARAM lParam)
    {
        var result = DefWindowProc(_handle, WM_WINDOWPOSCHANGED, wParam, lParam);

        var windowPos = (WINDOWPOS*)lParam;

        var previousLocation = _bounds.Location;
        var previousSize = _bounds.Size;

        var currentLocation = Vector2.Create(windowPos->x, windowPos->y);
        var currentSize = Vector2.Create(windowPos->cx, windowPos->cy);

        _bounds = BoundingRectangle.CreateFromSize(currentLocation, currentSize);

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
