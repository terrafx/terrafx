// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from Win32Application.h in https://github.com/Microsoft/DirectX-Graphics-Samples
// Original source is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Interop.Desktop;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Samples.DirectX.D3D12
{
    public static unsafe class Win32Application
    {
        #region Static Fields
        private static readonly WNDPROC _wndProc = WindowProc;

        private static IntPtr _hwnd;
        #endregion

        #region Static Properties
        public static IntPtr Hwnd
        {
            get
            {
                return _hwnd;
            }
        }
        #endregion

        #region Static Methods
        public static int Run(DXSample pSample, IntPtr hInstance, int nCmdShow)
        {
            // Parse the command line parameters
            pSample.ParseCommandLineArgs(Environment.GetCommandLineArgs());

            fixed (char* lpszClassName = "DXSampleClass")
            fixed (char* lpWindowName = pSample.Title)
            {
                // Initialize the window class.
                var windowClass = new WNDCLASSEX {
                    cbSize = SizeOf<WNDCLASSEX>(),
                    style = CS_HREDRAW | CS_VREDRAW,
                    lpfnWndProc = Marshal.GetFunctionPointerForDelegate(_wndProc),
                    hInstance = hInstance,
                    hCursor = LoadCursor(IntPtr.Zero, (char*)(IDC_ARROW)),
                    lpszClassName = lpszClassName
                };

                RegisterClassEx(&windowClass);

                var windowRect = new RECT {
                    left = 0,
                    top = 0,
                    right = unchecked((int)(pSample.Width)),
                    bottom = unchecked((int)(pSample.Height))
                };

                AdjustWindowRect(&windowRect, WS_OVERLAPPEDWINDOW, FALSE);

                // Create the window and store a handle to it.
                _hwnd = CreateWindowEx(
                    0,
                    windowClass.lpszClassName,
                    lpWindowName,
                    WS_OVERLAPPEDWINDOW,
                    CW_USEDEFAULT,
                    CW_USEDEFAULT,
                    windowRect.right - windowRect.left,
                    windowRect.bottom - windowRect.top,
                    IntPtr.Zero,                            // We have no parent window.
                    IntPtr.Zero,                            // We aren't using menus.
                    hInstance,
                    ((IntPtr)(GCHandle.Alloc(pSample))).ToPointer()
                );
            }

            // Initialize the sample. OnInit is defined in each child-implementation of DXSample.
            pSample.OnInit();

            ShowWindow(_hwnd, nCmdShow);

            // Main sample loop.
            var msg = new MSG();
            while (msg.message != WM_QUIT)
            {
                // Process any messages in the queue.
                if (PeekMessage(&msg, IntPtr.Zero, 0, 0, PM_REMOVE) != 0)
                {
                    TranslateMessage(&msg);
                    DispatchMessage(&msg);
                }
            }

            pSample.OnDestroy();

            // Return this part of the WM_QUIT message to Windows.
            return (int)(msg.wParam);
        }

        // Main message handler for the sample
        private static nint WindowProc(IntPtr hWnd, uint message, nuint wParam, nint lParam)
        {
            var handle = GetWindowLongPtr(hWnd, GWLP_USERDATA);
            var pSample = (handle != 0) ? (DXSample)(GCHandle.FromIntPtr(handle).Target) : null;

            switch (message)
            {
                case WM_CREATE:
                {
                    // Save the DXSample* passed in to CreateWindow.
                    var pCreateStruct = (CREATESTRUCT*)(lParam);
                    SetWindowLongPtr(hWnd, GWLP_USERDATA, (nint)(pCreateStruct->lpCreateParams));
                }
                return 0;

                case WM_KEYDOWN:
                {
                    if (pSample != null)
                    {
                        pSample.OnKeyDown((byte)(wParam));
                    }
                    return 0;
                }

                case WM_KEYUP:
                {
                    pSample?.OnKeyUp((byte)(wParam));
                    return 0;
                }

                case WM_PAINT:
                {
                    if (pSample != null)
                    {
                        pSample.OnUpdate();
                        pSample.OnRender();
                    }
                    return 0;
                }

                case WM_DESTROY:
                {
                    PostQuitMessage(0);
                    return 0;
                }
            }

            // Handle any messages the switch statement didn't.
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
        #endregion
    }
}
