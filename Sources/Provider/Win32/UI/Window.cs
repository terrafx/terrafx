// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Collections;
using TerraFX.Interop;
using TerraFX.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Defines a window.</summary>
    unsafe public sealed class Window : IDisposable, IWindow
    {
        #region Fields
        internal HWND _hWnd;

        internal readonly IDispatcher _dispatcher;

        internal readonly PropertySet _properties;

        internal Rectangle _bounds;

        internal bool _isActive;

        internal bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        internal Window(IDispatchManager dispatchManager, LPWSTR lpClassName, LPWSTR lpWindowName, HINSTANCE hInstance)
        {
            var hWnd = CreateWindowEx(
                WS_EX_OVERLAPPEDWINDOW,
                (WCHAR*)(lpClassName),
                (WCHAR*)(lpWindowName),
                WS_OVERLAPPEDWINDOW,
                unchecked((int)(CW_USEDEFAULT)),
                unchecked((int)(CW_USEDEFAULT)),
                unchecked((int)(CW_USEDEFAULT)),
                unchecked((int)(CW_USEDEFAULT)),
                null,
                null,
                hInstance,
                null
            );

            if (hWnd == null)
            {
                ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(CreateWindowEx));
            }

            _hWnd = hWnd;
            _dispatcher = dispatchManager.DispatcherForCurrentThread;
            _properties = new PropertySet();

            RECT rect;

            var succeeded = GetWindowRect(_hWnd, &rect);

            if (succeeded == 0)
            {
                ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(GetWindowRect));
            }

            _bounds = new Rectangle(rect.left, rect.top, (rect.right - rect.left), (rect.bottom - rect.top));

            var activeWindow = GetActiveWindow();
            _isActive = (activeWindow == _hWnd);

            _isVisible = (IsWindowVisible(hWnd) != 0);
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
                return (IntPtr)((void*)(_hWnd));
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
        #endregion

        #region Methods
        /// <summary>Processes messages sent to the instance.</summary>
        /// <param name="Msg">The message.</param>
        /// <param name="wParam">Additional message information.</param>
        /// <param name="lParam">Additional message information.</param>
        /// <returns>The result of processing <paramref name="Msg" />.</returns>
        public LRESULT WindowProc(UINT Msg, WPARAM wParam, LPARAM lParam)
        {
            switch (Msg)
            {
                case WM_MOVE:
                {
                    var x = LOWORD(lParam);
                    var y = HIWORD(lParam);

                    _bounds.Location = new Point2D(x, y);
                    return 0;
                }

                case WM_SIZE:
                {
                    var width = LOWORD(lParam);
                    var height = HIWORD(lParam);

                    _bounds.Size = new Size2D(width, height);
                    return 0;
                }

                case WM_ACTIVATE:
                {
                    var activateCmd = LOWORD(wParam);
                    _isActive = (activateCmd != WA_INACTIVE);
                    return 0;
                }

                case WM_SHOWWINDOW:
                {
                    var shown = (BOOL)(LOWORD(wParam));
                    _isVisible = (shown != 0);
                    return 0;
                }

                default:
                {
                    return DefWindowProc(_hWnd, Msg, wParam, lParam);
                }
            }
        }

        internal void Dispose(bool isDisposing)
        {
            if (_hWnd != null)
            {
                var succeeded = DestroyWindow(_hWnd);

                if (succeeded == 0)
                {
                    ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(DestroyWindow));
                }

                _hWnd = null;
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
                var succeeded = SetForegroundWindow(_hWnd);

                if (succeeded == 0)
                {
                    ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(SetForegroundWindow));
                }
            }
        }

        /// <summary>Closes the instance.</summary>
        public void Close()
        {
            SendMessage(_hWnd, WM_CLOSE, 0, 0);
        }

        /// <summary>Hides the instance.</summary>
        public void Hide()
        {
            if (_isVisible)
            {
                var succeeded = ShowWindow(_hWnd, SW_HIDE);

                if (succeeded == 0)
                {
                    ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }

        /// <summary>Shows the instance.</summary>
        public void Show()
        {
            if (!_isVisible)
            {
                var succeeded = ShowWindow(_hWnd, SW_SHOW);

                if (succeeded == 0)
                {
                    ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }
        #endregion
    }
}
