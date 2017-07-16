// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.Desktop
{
    unsafe public static partial class User32
    {
        #region External Methods
        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CloseWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL CloseWindow(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HWND CreateWindowEx(
            [In] DWORD dwExStyle,
            [In, Optional] LPCWSTR lpClassName,
            [In, Optional] LPCWSTR lpWindowName,
            [In] DWORD dwStyle,
            [In] int X,
            [In] int Y,
            [In] int nWidth,
            [In] int nHeight,
            [In, Optional] HWND hWndParent,
            [In, Optional] HMENU hMenu,
            [In, Optional] HINSTANCE hInstance,
            [In] LPVOID lpParam
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern LRESULT DefWindowProc(
            [In] HWND hWnd,
            [In] UINT Msg,
            [In] WPARAM wParam,
            [In] LPARAM lParam
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DestroyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL DestroyWindow(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DispatchMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern LRESULT DispatchMessage(
            [In] /* readonly */ MSG* lpMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HWND GetActiveWindow(
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassInfoExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL GetClassInfoEx(
            [In, Optional] HINSTANCE hInstance,
            [In] LPCWSTR lpszClass,
            [Out] LPWNDCLASSEX lpwcx
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassNameW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int GetClassName(
            [In] HWND hWnd,
            [Out] LPWSTR lpClassName,
            [In] int nMaxCount
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetDesktopWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HWND GetDesktopWindow(
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetWindowRect", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL GetWindowRect(
            [In] HWND hWnd,
            [Out] LPRECT lpRect
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "IsWindowVisible", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL IsWindowVisible(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PeekMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL PeekMessage(
            [Out] LPMSG lpMsg,
            [In, Optional] HWND hWnd,
            [In] UINT wMsgFilterMin,
            [In] UINT wMsgFilterMax,
            [In] UINT wRemoveMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PostQuitMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void PostQuitMessage(
            [In] int nExitCode
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern ATOM RegisterClassEx(
            [In] /* readonly */ WNDCLASSEX* lpWndClassEx
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SendMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern LRESULT SendMessage(
            [In] HWND hWnd,
            [In] UINT Msg,
            [In] WPARAM wParam,
            [In] LPARAM lParam
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern HWND SetActiveWindow(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetForegroundWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL SetForegroundWindow(
            [In] HWND hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "ShowWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL ShowWindow(
            [In] HWND hWnd,
            [In] int nCmdShow
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "TranslateMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL TranslateMessage(
            [In] /* readonly */ MSG* lpMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "UnregisterClassW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern BOOL UnregisterClass(
            [In] LPCWSTR lpClassName,
            [In, Optional] HINSTANCE hInstance
        );
        #endregion
    }
}
