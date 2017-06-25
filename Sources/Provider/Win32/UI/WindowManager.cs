// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Interop.Desktop;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    [Export(typeof(IWindowManager))]
    [Shared]
    unsafe public sealed class WindowManager : IDisposable, IWindowManager
    {
        #region Fields
        internal static readonly ConcurrentDictionary<HWND, Window> CreatedWindows = new ConcurrentDictionary<HWND, Window>();

        internal static readonly HINSTANCE EntryModuleHandle = Kernel32.GetModuleHandle(); 

        internal static readonly WNDPROC WndProc = WindowProc;

        private ATOM _classAtom;

        private WCHAR* _lpClassName;

        private WCHAR* _lpWindowName;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WindowManager" /> class.</summary>
        public WindowManager()
        {
            _lpClassName = (WCHAR*)(Marshal.StringToHGlobalUni($"TerraFX.Interop.Provider.Win32.UI.Window.{EntryModuleHandle}"));
            _lpWindowName = (WCHAR*)(Marshal.StringToHGlobalUni($"TerraFX Win32 Window"));

            var wndClassEx = new WNDCLASSEX() {
                cbSize = unchecked((uint)(Marshal.SizeOf<WNDCLASSEX>())),
                style = CS.VREDRAW | CS.HREDRAW,
                lpfnWndProc = WndProc,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = EntryModuleHandle,
                hIcon = HICON.NULL,
                hCursor = HCURSOR.NULL,
                hbrBackground = (UIntPtr)(COLOR.WINDOW + 1),
                lpszMenuName = LPWSTR.NULL,
                lpszClassName = _lpClassName,
                hIconSm = HICON.NULL
            };

            var classAtom = User32.RegisterClassEx(ref wndClassEx);

            if (classAtom == 0)
            {
                var hresult = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hresult);
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
        private static LRESULT WindowProc(HWND hWnd, WM Msg, WPARAM wParam, LPARAM lParam)
        {
            if (CreatedWindows.TryGetValue(hWnd, out var window))
            {
                return window.WindowProc(Msg, wParam, lParam);
            }

            return User32.DefWindowProc(hWnd, Msg, wParam, lParam);
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
                User32.UnregisterClass(lpClassName, EntryModuleHandle);
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
            var window = new Window(lpClassName, _lpWindowName, EntryModuleHandle);

            var succeeded = CreatedWindows.TryAdd(window.Handle, window);
            Debug.Assert(succeeded);

            return window;
        }
        #endregion
    }
}
