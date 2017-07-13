// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Interop.Desktop;
using TerraFX.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Desktop.User32;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    [Export(typeof(IWindowManager))]
    [Shared]
    unsafe public sealed class WindowManager : IDisposable, IWindowManager
    {
        #region Fields
        internal static readonly ConcurrentDictionary<HWND, Window> CreatedWindows = new ConcurrentDictionary<HWND, Window>();

        internal static readonly HINSTANCE EntryModuleHandle = GetModuleHandle(); 

        internal static readonly WNDPROC WndProc = WindowProc;

        private WCHAR* _lpClassName;

        private WCHAR* _lpWindowName;

        private Lazy<IDispatchManager> _dispatchManager;

        private ATOM _classAtom;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WindowManager" /> class.</summary>
        [ImportingConstructor]
        public WindowManager(
            [Import] Lazy<IDispatchManager> dispatchManager
        )
        {
            _lpClassName = (WCHAR*)(Marshal.StringToHGlobalUni($"TerraFX.Interop.Provider.Win32.UI.Window.{EntryModuleHandle}"));
            _lpWindowName = (WCHAR*)(Marshal.StringToHGlobalUni($"TerraFX Win32 Window"));
            _dispatchManager = dispatchManager;

            var wndClassEx = new WNDCLASSEX() {
                cbSize = unchecked((uint)(Marshal.SizeOf<WNDCLASSEX>())),
                style = CS_VREDRAW | CS_HREDRAW,
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(WndProc),
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = EntryModuleHandle,
                hIcon = null,
                hCursor = null,
                hbrBackground = (void*)(COLOR_WINDOW + 1),
                lpszMenuName = (WCHAR*)(null),
                lpszClassName = _lpClassName,
                hIconSm = null
            };

            var classAtom = RegisterClassEx(&wndClassEx);

            if (classAtom == 0)
            {
                ExceptionUtilities.ThrowExternalExceptionForLastError(nameof(RegisterClassEx));
            }

            _classAtom = classAtom;
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="WindowManager" /> class.</summary>
        ~WindowManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Methods
        private static LRESULT WindowProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
        {
            if (CreatedWindows.TryGetValue(hWnd, out var window))
            {
                return window.WindowProc(Msg, wParam, lParam);
            }

            return DefWindowProc(hWnd, Msg, wParam, lParam);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var createdWindow in CreatedWindows.Values)
                {
                    createdWindow.Dispose();
                }
            }

            if (_classAtom != 0)
            {
                var lpClassName = (LPWSTR)(_classAtom);
                UnregisterClass((WCHAR*)(lpClassName), EntryModuleHandle);
                _classAtom = 0;
            }

            if (_lpWindowName != null)
            {
                Marshal.FreeHGlobal((IntPtr)(_lpWindowName));
                _lpWindowName = null;
            }

            if (_lpClassName != null)
            {
                Marshal.FreeHGlobal((IntPtr)(_lpClassName));
                _lpClassName = null;
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

        #region TerraFX.UI.IWindowManager
        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        public IWindow CreateWindow()
        {
            if (_classAtom == 0)
            {
                ExceptionUtilities.ThrowObjectDisposedException(nameof(IWindowManager));
            }

            var lpClassName = (LPWSTR)(_classAtom);
            var window = new Window(_dispatchManager.Value, lpClassName, _lpWindowName, EntryModuleHandle);

            var succeeded = CreatedWindows.TryAdd((void*)(window.Handle), window);
            Debug.Assert(succeeded);

            return window;
        }
        #endregion
    }
}
