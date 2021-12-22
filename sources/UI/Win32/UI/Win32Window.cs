// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Graphics;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.Windows.GWL;
using static TerraFX.Interop.Windows.HWND;
using static TerraFX.Interop.Windows.SW;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.WM;
using static TerraFX.Interop.Windows.WS;
using static TerraFX.Threading.VolatileState;
using static TerraFX.UI.Win32UIService;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.Win32Utilities;

namespace TerraFX.UI;

/// <summary>Defines a window.</summary>
public sealed unsafe class Win32Window : Window
{
    private readonly FlowDirection _flowDirection;
    private readonly HWND _handle;
    private readonly ReadingDirection _readingDirection;

    private BoundingRectangle _bounds;
    private BoundingRectangle _clientBounds;
    private uint _extendedStyle;
    private bool _isActive;
    private uint _style;
    private string _title = null!;
    private WindowState _windowState;

    private VolatileState _state;

    internal Win32Window(Win32UIDispatcher dispatcher)
        : base(dispatcher)
    {
        _flowDirection = FlowDirection.TopToBottom;
        _handle = CreateHandle(this, Service.ClassAtom);
        _readingDirection = ReadingDirection.LeftToRight;

        _ = _state.Transition(to: Initialized);

        static HWND CreateHandle(Win32Window window, ushort classAtom)
        {
            HWND hWnd;

            fixed (char* lpWindowName = nameof(Win32Window))
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

    /// <summary>Finalizes an instance of the <see cref="Win32Window" /> class.</summary>
    ~Win32Window() => Dispose(isDisposing: false);

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
    public new Win32UIDispatcher Dispatcher => base.Dispatcher.As<Win32UIDispatcher>();

    /// <inheritdoc />
    public override FlowDirection FlowDirection => _flowDirection;

    /// <summary>Gets the underlying handle for the window.</summary>
    public HWND Handle
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
    public override bool IsEnabled => (_style & WS_DISABLED) == 0;

    /// <inheritdoc />
    public override bool IsVisible => (_style & WS_VISIBLE) != 0;

    /// <inheritdoc />
    public override ReadingDirection ReadingDirection => _readingDirection;

    /// <inheritdoc cref="UIDispatcherObject.Service" />
    public new Win32UIService Service => base.Service.As<Win32UIService>();

    /// <inheritdoc />
    public override string Title => _title;

    /// <inheritdoc />
    public override WindowState WindowState => _windowState;

    /// <inheritdoc />
    protected override IntPtr SurfaceContextHandle => EntryPointModule;

    /// <inheritdoc />
    protected override IntPtr SurfaceHandle => Handle;

    /// <inheritdoc />
    protected override GraphicsSurfaceKind SurfaceKind => GraphicsSurfaceKind.Win32;

    private static void DisposeHandle(HWND handle)
    {
        if (handle != HWND.NULL)
        {
            ThrowExternalExceptionIfFalse(DestroyWindow(handle));
        }
    }

    /// <inheritdoc />
    public override void Activate()
    {
        if (!TryActivate())
        {
            ThrowExternalException(nameof(SetForegroundWindow), 0);
        }
    }

    /// <inheritdoc />
    public override void Close()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));
        _ = SendMessageW(_handle, WM_CLOSE, wParam: 0u, lParam: 0);
    }

    /// <inheritdoc />
    public override void Disable()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (IsEnabled)
        {
            _ = EnableWindow(_handle, FALSE);
        }
    }

    /// <inheritdoc />
    public override void Enable()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (!IsEnabled)
        {
            _ = EnableWindow(_handle, TRUE);
        }
    }

    /// <inheritdoc />
    public override void Hide()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (WindowState != WindowState.Hidden)
        {
            _ = ShowWindow(_handle, SW_HIDE);
        }
    }

    /// <inheritdoc />
    public override void Maximize()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (WindowState != WindowState.Maximized)
        {
            _ = ShowWindow(_handle, SW_MAXIMIZE);
        }
    }

    /// <inheritdoc />
    public override void Minimize()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (WindowState != WindowState.Minimized)
        {
            _ = ShowWindow(_handle, SW_MINIMIZE);
        }
    }

    /// <inheritdoc />
    public override void Relocate(Vector2 location)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

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

    /// <inheritdoc />
    public override void RelocateClient(Vector2 location)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

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

    /// <inheritdoc />
    public override void Resize(Vector2 size)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

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

    /// <inheritdoc />
    public override void ResizeClient(Vector2 size)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

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

    /// <inheritdoc />
    public override void Restore()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (WindowState != WindowState.Normal)
        {
            _ = ShowWindow(_handle, SW_RESTORE);
        }
    }

    /// <inheritdoc />
    public override void SetTitle(string title)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (_title != title)
        {
            title ??= string.Empty;

            fixed (char* pTitle = title)
            {
                ThrowExternalExceptionIfFalse(SetWindowTextW(_handle, (ushort*)pTitle));
            }
        }
    }

    /// <inheritdoc />
    public override void Show()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(Win32Window));

        if (WindowState == WindowState.Hidden)
        {
            _ = ShowWindow(_handle, SW_SHOW);
        }
    }

    /// <inheritdoc />
    public override bool TryActivate() => _isActive || (SetForegroundWindow(_handle) != FALSE);

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
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

        _state.EndDispose();
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

        if (_state == Disposing)
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

        if (_state != Disposing)
        {
            _ = _state.Transition(to: Disposed);
        }
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
            SIZE_RESTORED => WindowState.Normal,
            SIZE_MINIMIZED => WindowState.Minimized,
            SIZE_MAXIMIZED => WindowState.Maximized,
            SIZE_MAXHIDE => WindowState.Hidden,
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
