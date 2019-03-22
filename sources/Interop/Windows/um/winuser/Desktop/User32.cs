// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.Desktop
{
    public static unsafe partial class User32
    {
        private const string DllName = nameof(User32);

        #region External Methods
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "AdjustWindowRect", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int AdjustWindowRect(
            [In, Out, ComAliasName("LPRECT")] RECT* lpRect,
            [In, ComAliasName("DWORD")] uint dwStyle,
            [In, ComAliasName("BOOL")] int bMenu
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CloseWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int CloseWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HWND")]
        public static extern IntPtr CreateWindowEx(
            [In, ComAliasName("DWORD")] uint dwExStyle,
            [In, Optional, ComAliasName("LPCWSTR")] char* lpClassName,
            [In, Optional, ComAliasName("LPCWSTR")] char* lpWindowName,
            [In, ComAliasName("DWORD")] uint dwStyle,
            [In] int X,
            [In] int Y,
            [In] int nWidth,
            [In] int nHeight,
            [In, Optional, ComAliasName("HWND")] IntPtr hWndParent,
            [In, Optional, ComAliasName("HMENU")] IntPtr hMenu,
            [In, Optional, ComAliasName("HINSTANCE")] IntPtr hInstance,
            [In, ComAliasName("LPVOID")] void* lpParam
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LRESULT")]
        public static extern nint DefWindowProc(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In, ComAliasName("UINT")] uint Msg,
            [In, ComAliasName("WPARAM")] nuint wParam,
            [In, ComAliasName("LPARAM")] nint lParam
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DestroyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int DestroyWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DispatchMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LRESULT")]
        public static extern nint DispatchMessage(
            [In] in MSG lpMsg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HWND")]
        public static extern IntPtr GetActiveWindow(
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassInfoExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int GetClassInfoEx(
            [In, Optional, ComAliasName("HINSTANCE")] IntPtr hInstance,
            [In, ComAliasName("LPCWSTR")] char* lpszClass,
            [Out, ComAliasName("LPWNDCLASSEX")] out WNDCLASSEX lpwcx
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassNameW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int GetClassName(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [Out, ComAliasName("LPWSTR")] char* lpClassName,
            [In] int nMaxCount
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetDesktopWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HWND")]
        public static extern IntPtr GetDesktopWindow(
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtrW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LONG_PTR")]
        public static extern nint GetWindowLongPtr(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In] int nIndex
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetWindowRect", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int GetWindowRect(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [Out, ComAliasName("LPRECT")] out RECT lpRect
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "IsWindowVisible", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int IsWindowVisible(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "LoadCursorW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HCURSOR")]
        public static extern IntPtr LoadCursor(
            [In, Optional, ComAliasName("HINSTANCE")] IntPtr hInstance,
            [In, ComAliasName("LPCWSTR")] char* lpCursorName
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PeekMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int PeekMessage(
            [Out, ComAliasName("LPMSG")] out MSG lpMsg,
            [In, Optional, ComAliasName("HWND")] IntPtr hWnd,
            [In, ComAliasName("UINT")] uint wMsgFilterMin,
            [In, ComAliasName("UINT")] uint wMsgFilterMax,
            [In, ComAliasName("UINT")] uint wRemoveMsg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PostQuitMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void PostQuitMessage(
            [In] int nExitCode
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("ATOM")]
        public static extern ushort RegisterClassEx(
            [In] in WNDCLASSEX lpWndClassEx
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SendMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LRESULT")]
        public static extern nint SendMessage(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In, ComAliasName("UINT")] uint Msg,
            [In, ComAliasName("WPARAM")] nuint wParam,
            [In, ComAliasName("LPARAM")] nint lParam
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HWND")]
        public static extern IntPtr SetActiveWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetForegroundWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int SetForegroundWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongPtrW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LONG_PTR")]
        public static extern nint SetWindowLongPtr(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In] int nIndex,
            [In, ComAliasName("LONG_PTR")] nint dwNewLong
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "ShowWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int ShowWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In] int nCmdShow
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "TranslateMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int TranslateMessage(
            [In] in MSG lpMsg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "UnregisterClassW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int UnregisterClass(
            [In, ComAliasName("LPCWSTR")] char* lpClassName,
            [In, ComAliasName("HINSTANCE")] IntPtr hInstance = default
        );
        #endregion
    }
}
