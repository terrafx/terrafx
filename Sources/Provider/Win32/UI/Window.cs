// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using TerraFX.Collections;
using TerraFX.Interop;
using TerraFX.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.WinUser;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Defines a window.</summary>
    unsafe public sealed class Window : IDisposable, IWindow
    {
        #region Fields
        private HWND _hWnd;

        private IDispatcher _dispatcher;

        private PropertySet _properties;

        private Rectangle _bounds;

        private bool _isActive;

        private bool _isVisible;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        internal Window(IDispatchManager dispatchManager, LPWSTR lpClassName, LPWSTR lpWindowName, HINSTANCE hInstance)
        {
            var hWnd = CreateWindowEx(
                WS_EX.OVERLAPPEDWINDOW,
                lpClassName,
                lpWindowName,
                WS.OVERLAPPEDWINDOW,
                unchecked((int)(CW.USEDEFAULT)),
                unchecked((int)(CW.USEDEFAULT)),
                unchecked((int)(CW.USEDEFAULT)),
                unchecked((int)(CW.USEDEFAULT)),
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

            var succeeded = GetWindowRect(_hWnd, out var lpRect);

            if (succeeded == 0)
            {
                ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(GetWindowRect));
            }

            _bounds = new Rectangle(lpRect.left, lpRect.top, (lpRect.right - lpRect.left), (lpRect.bottom - lpRect.top));

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

        #region Methods
        /// <summary>Processes messages sent to the instance.</summary>
        /// <param name="Msg">The message.</param>
        /// <param name="wParam">Additional message information.</param>
        /// <param name="lParam">Additional message information.</param>
        /// <returns>The result of processing <paramref name="Msg" />.</returns>
        public LRESULT WindowProc(WM Msg, WPARAM wParam, LPARAM lParam)
        {
            switch (Msg)
            {
                case WM.MOVE:
                {
                    var x = (ushort)(lParam);
                    var y = (ushort)(lParam >> 16);

                    _bounds.Location = new Point2D(x, y);
                    return 0;
                }

                case WM.SIZE:
                {
                    var width = (ushort)(lParam);
                    var height = (ushort)(lParam >> 16);

                    _bounds.Size = new Size2D(width, height);
                    return 0;
                }

                case WM.ACTIVATE:
                {
                    var activateCmd = (WA)((ushort)(wParam));
                    Debug.Assert(Enum.IsDefined(typeof(WA), activateCmd));

                    _isActive = (activateCmd != WA.INACTIVE);
                    return 0;
                }

                case WM.SHOWWINDOW:
                {
                    var shown = (BOOL)(unchecked((int)((uint)(wParam))));
                    _isVisible = (shown != 0);
                    return 0;
                }

                default:
                {
                    return DefWindowProc(_hWnd, Msg, wParam, lParam);
                }
            }
        }

        private void Dispose(bool isDisposing)
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

        #region System.IDisposable
        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region TerraFX.UI.IWindow
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
            SendMessage(_hWnd, WM.CLOSE, 0, 0);
        }

        /// <summary>Hides the instance.</summary>
        public void Hide()
        {
            if (_isVisible)
            {
                var succeeded = ShowWindow(_hWnd, SW.HIDE);

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
                var succeeded = ShowWindow(_hWnd, SW.SHOW);

                if (succeeded == 0)
                {
                    ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(ShowWindow));
                }
            }
        }
        #endregion
    }
}
