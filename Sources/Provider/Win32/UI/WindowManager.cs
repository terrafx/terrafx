// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics;
using TerraFX.Interop;
using TerraFX.Interop.Desktop;
using TerraFX.Provider.Win32.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Kernel32;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Provider.Win32.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    [Export(typeof(IWindowManager))]
    [Export(typeof(WindowManager))]
    [Shared]
    unsafe public sealed class WindowManager : IDisposable, IWindowManager
    {
        #region Static Fields
        internal static readonly ConcurrentDictionary<HWND, Window> _createdWindows = new ConcurrentDictionary<HWND, Window>();

        internal static readonly HINSTANCE _entryModuleHandle = GetModuleHandle(); 

        internal static readonly NativeDelegate<WNDPROC> _wndProc = new NativeDelegate<WNDPROC>(WindowProc);
        #endregion

        #region Fields
        internal readonly Lazy<DispatchManager> _dispatchManager;

        internal readonly NativeStringUni _className;

        internal readonly NativeStringUni _defaultWindowName;

        internal ATOM _classAtom;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WindowManager" /> class.</summary>
        [ImportingConstructor]
        public WindowManager(
            [Import] Lazy<DispatchManager> dispatchManager
        )
        {
            _dispatchManager = dispatchManager;
            _className = new NativeStringUni($"TerraFX.Interop.Provider.Win32.UI.Window.{_entryModuleHandle}");
            _defaultWindowName = new NativeStringUni("TerraFX Win32 Window");
            _classAtom = RegisterWindowClass((LPCWSTR)(_className));
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="WindowManager" /> class.</summary>
        ~WindowManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Static Methods
        internal static void DisposeCreatedWindows()
        {
            foreach (var createdWindowHandle in _createdWindows.Keys)
            {
                if (_createdWindows.TryGetValue(createdWindowHandle, out var createdWindow))
                {
                    createdWindow.Dispose();
                }
            }
        }

        internal static ATOM RegisterWindowClass(LPCWSTR className)
        {
            var wndClassEx = new WNDCLASSEX() {
                cbSize = SizeOf<WNDCLASSEX>(),
                style = (CS_VREDRAW | CS_HREDRAW | CS_DBLCLKS),
                lpfnWndProc = _wndProc,
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = _entryModuleHandle,
                hIcon = (HICON)(NULL),
                hCursor = (HCURSOR)(NULL),
                hbrBackground = (HBRUSH)(COLOR_WINDOW + 1),
                lpszMenuName = (LPCWSTR)(NULL),
                lpszClassName = className,
                hIconSm = (HICON)(NULL)
            };

            var classAtom = RegisterClassEx(&wndClassEx);

            if (classAtom == NULL)
            {
                ThrowExternalExceptionForLastError(nameof(RegisterClassEx));
            }

            return classAtom;
        }

        internal static LRESULT WindowProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
        {
            if (_createdWindows.TryGetValue(hWnd, out var window))
            {
                return window.WindowProc(Msg, wParam, lParam);
            }

            return DefWindowProc(hWnd, Msg, wParam, lParam);
        }
        #endregion

        #region Methods
        internal void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                DisposeCreatedWindows();
            }

            DisposeClassAtom();

            _defaultWindowName.Dispose();
            _className.Dispose();
        }

        internal void DisposeClassAtom()
        {
            if (_classAtom != NULL)
            {
                var result = UnregisterClass((LPCWSTR)(_classAtom), _entryModuleHandle);

                if (result == FALSE)
                {
                    ThrowExternalExceptionForLastError(nameof(UnregisterClass));
                }

                _classAtom = (ATOM)(NULL);
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

        #region TerraFX.UI.IWindowManager Methods
        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        public IWindow CreateWindow()
        {
            if (_classAtom == NULL)
            {
                ThrowObjectDisposedException(nameof(IWindowManager));
            }

            var window = new Window(this, _dispatchManager.Value, (LPCWSTR)(_className), (LPCWSTR)(_defaultWindowName), _entryModuleHandle);

            var succeeded = _createdWindows.TryAdd((void*)(window.Handle), window);
            Debug.Assert(succeeded);

            return window;
        }
        #endregion
    }
}
