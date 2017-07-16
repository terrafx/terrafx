// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Collections;
using TerraFX.Interop;
using TerraFX.Provider.Win32.Threading;
using TerraFX.Threading;
using TerraFX.UI;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Defines a window.</summary>
    unsafe public sealed class Window : IDisposable, IWindow
    {
        #region Fields
        internal readonly Dispatcher _dispatcher;

        internal readonly PropertySet _properties;

        internal readonly WindowManager _windowManager;

        internal HWND _handle;

        internal Rectangle _bounds;

        internal bool _isActive;

        internal bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        internal Window(WindowManager windowManager, DispatchManager dispatchManager, LPCWSTR className, LPCWSTR windowName, HINSTANCE instanceHandle)
        {
            _dispatcher = (Dispatcher)(dispatchManager.DispatcherForCurrentThread);
            _properties = new PropertySet();
            _windowManager = windowManager;

            _handle = CreateWindowHandle(className, windowName, instanceHandle);

            _bounds = GetWindowBounds(_handle);
            _isActive = IsWindowActive(_handle);
            _isVisible = IsWindowVisible(_handle);
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

        /// <summary>Gets the handle for the instance.</summary>
        public IntPtr Handle
        {
            get
            {
                return (IntPtr)((void*)(_handle));
            }
        }

        /// <summary>Gets a value that indicates whether the flow-direction of the instance is left-to-right.</summary>
        public bool IsLeftToRight
        {
            get
            {
                return false;
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

        /// <summary>Gets the set of properties associated with the instance.</summary>
        public IPropertySet Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>Gets the <see cref="IWindowManager" /> associated with the instance.</summary>
        public IWindowManager WindowManager
        {
            get
            {
                return _windowManager;
            }
        }
        #endregion

        #region Static Methods
        internal static HWND CreateWindowHandle(LPCWSTR className, LPCWSTR windowName, HINSTANCE instanceHandle)
        {
            var hWnd = CreateWindowEx(
                WS_EX_OVERLAPPEDWINDOW,
                className,
                windowName,
                WS_OVERLAPPEDWINDOW,
                X: CW_USEDEFAULT,
                Y: CW_USEDEFAULT,
                nWidth: CW_USEDEFAULT,
                nHeight: CW_USEDEFAULT,
                hWndParent: (HWND)(NULL),
                hMenu: (HMENU)(NULL),
                hInstance: instanceHandle,
                lpParam: (LPVOID)(NULL)
            );

            if (hWnd == (HWND)(NULL))
            {
                ThrowExternalExceptionForLastError(nameof(CreateWindowEx));
            }

            return hWnd;
        }

        internal static bool IsWindowActive(HWND handle)
        {
            var activeWindow = GetActiveWindow();
            return (activeWindow == handle);
        }

        internal static Rectangle GetWindowBounds(HWND handle)
        {
            RECT rect;

            var succeeded = GetWindowRect(handle, &rect);

            if (succeeded == FALSE)
            {
                ThrowExternalExceptionForLastError(nameof(GetWindowRect));
            }

            return new Rectangle(rect.left, rect.top, (rect.right - rect.left), (rect.bottom - rect.top));
        }
        #endregion

        #region Methods
        internal void Dispose(bool isDisposing)
        {
            if (_handle != (HWND)(NULL))
            {
                var succeeded = DestroyWindow(_handle);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(DestroyWindow));
                }

                _windowManager.DestroyWindow(_handle);

                _handle = (HWND)(NULL);
            }
        }

        internal LRESULT HandleWmActivate(WPARAM wParam)
        {
            _isActive = (LOWORD(wParam) != WA_INACTIVE);
            return 0;
        }

        internal LRESULT HandleWmClose()
        {
            Dispose();
            return 0;
        }

        internal LRESULT HandleWmMove(LPARAM lParam)
        {
            _bounds.Location = new Point2D(x: LOWORD(lParam), y: HIWORD(lParam));
            return 0;
        }

        internal LRESULT HandleWmShowWindow(WPARAM wParam)
        {
            _isVisible = (LOWORD(wParam) != FALSE);
            return 0;
        }

        internal LRESULT HandleWmSize(LPARAM lParam)
        {
            _bounds.Size = new Size2D(width: LOWORD(lParam), height: HIWORD(lParam));
            return 0;
        }

        internal LRESULT WindowProc(UINT Msg, WPARAM wParam, LPARAM lParam)
        {
            switch (Msg)
            {
                case WM_MOVE:
                {
                    return HandleWmMove(lParam);
                }

                case WM_SIZE:
                {
                    return HandleWmSize(lParam);
                }

                case WM_ACTIVATE:
                {
                    return HandleWmActivate(wParam);
                }

                case WM_CLOSE:
                {
                    return HandleWmClose();
                }

                case WM_SHOWWINDOW:
                {
                    return HandleWmShowWindow(wParam);
                }

                default:
                {
                    return DefWindowProc(_handle, Msg, wParam, lParam);
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
            if (IsVisible)
            {
                var succeeded = SetForegroundWindow(_handle);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(SetForegroundWindow));
                }
            }
        }

        /// <summary>Closes the instance.</summary>
        public void Close()
        {
            SendMessage(_handle, WM_CLOSE, wParam: 0, lParam: 0);
        }

        /// <summary>Hides the instance.</summary>
        public void Hide()
        {
            if (_isVisible)
            {
                var succeeded = ShowWindow(_handle, SW_HIDE);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }

        /// <summary>Shows the instance.</summary>
        public void Show()
        {
            if (_isVisible == false)
            {
                var succeeded = ShowWindow(_handle, SW_SHOW);

                if (succeeded == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }
        #endregion
    }
}
