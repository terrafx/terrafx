// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Interop.Desktop;

namespace TerraFX.Interop
{
    unsafe public static partial class User32
    {
        #region Methods
        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CloseWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL CloseWindow(
            [In] HWND hWnd
        );

        public static HWND CreateWindow(
            [In, Optional] LPWSTR lpClassName,
            [In, Optional] LPWSTR lpWindowName,
            [In] WS dwStyle,
            [In] int X,
            [In] int Y,
            [In] int nWidth,
            [In] int nHeight,
            [In, Optional] HWND hWndParent,
            [In, Optional] HMENU hMenu,
            [In, Optional] HINSTANCE hInstance,
            [In] void* lpParam
        )
        {
            return CreateWindowEx(
                (WS_EX.LEFT | WS_EX.LTRREADING | WS_EX.LEFTSCROLLBAR),
                lpClassName,
                lpWindowName,
                dwStyle,
                X,
                Y,
                nWidth,
                nHeight,
                hWndParent,
                hMenu,
                hInstance,
                lpParam
            );
        }

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern HWND CreateWindowEx(
            [In] WS_EX dwExStyle,
            [In, Optional] LPWSTR lpClassName,
            [In, Optional] LPWSTR lpWindowName,
            [In] WS dwStyle,
            [In] int X,
            [In] int Y,
            [In] int nWidth,
            [In] int nHeight,
            [In, Optional] HWND hWndParent,
            [In, Optional] HMENU hMenu,
            [In, Optional] HINSTANCE hInstance,
            [In] void* lpParam
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern LRESULT DefWindowProc(
            [In] HWND hWnd,
            [In] WM Msg,
            [In] WPARAM wParam,
            [In] LPARAM lParam
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DestroyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL DestroyWindow(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DispatchMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern LRESULT DispatchMessage(
            [In] ref /* readonly */ MSG lpMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern HWND GetActiveWindow(
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetWindowRect", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL GetWindowRect(
            [In] HWND hWnd,
            [Out] out RECT lpRect
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "IsWindowVisible", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL IsWindowVisible(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PeekMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL PeekMessage(
            [Out] out MSG lpMsg,
            [In, Optional] HWND hWnd,
            [In] WM wMsgFilterMin,
            [In] WM wMsgFilterMax,
            [In] PM wRemoveMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PostQuitMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern void PostQuitMessage(
            [In] int nExitCode
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern ATOM RegisterClass(
            [In] ref /* readonly */ WNDCLASS lpWndClass
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern ATOM RegisterClassEx(
            [In] ref /* readonly */ WNDCLASSEX lpWndClassEx
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern HWND SetActiveWindow(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "ShowWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL ShowWindow(
            [In] HWND hWnd,
            [In] SW nCmdShow
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "TranslateMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL TranslateMessage(
            [In] ref /* readonly */ MSG lpMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "UnregisterClassW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL UnregisterClass(
            [In] LPWSTR lpClassName,
            [In, Optional] HINSTANCE hInstance
        );
        #endregion
    }
}
