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
        #region External Methods
        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CloseWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int CloseWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
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

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LRESULT")]
        public static extern nint DefWindowProc(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In, ComAliasName("UINT")] uint Msg,
            [In, ComAliasName("WPARAM")] nuint wParam,
            [In, ComAliasName("LPARAM")] nint lParam
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DestroyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int DestroyWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DispatchMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LRESULT")]
        public static extern nint DispatchMessage(
            [In] MSG* lpMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HWND")]
        public static extern IntPtr GetActiveWindow(
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassInfoExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int GetClassInfoEx(
            [In, Optional, ComAliasName("HINSTANCE")] IntPtr hInstance,
            [In, ComAliasName("LPCWSTR")] char* lpszClass,
            [Out, ComAliasName("LPWNDCLASSEX")] WNDCLASSEX* lpwcx
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassNameW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int GetClassName(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [Out, ComAliasName("LPWSTR")] char* lpClassName,
            [In] int nMaxCount
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetDesktopWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HWND")]
        public static extern IntPtr GetDesktopWindow(
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetWindowRect", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int GetWindowRect(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [Out, ComAliasName("LPRECT")] RECT* lpRect
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "IsWindowVisible", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int IsWindowVisible(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PeekMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int PeekMessage(
            [Out, ComAliasName("LPMSG")] MSG* lpMsg,
            [In, Optional, ComAliasName("HWND")] IntPtr hWnd,
            [In, ComAliasName("UINT")] uint wMsgFilterMin,
            [In, ComAliasName("UINT")] uint wMsgFilterMax,
            [In, ComAliasName("UINT")] uint wRemoveMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PostQuitMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void PostQuitMessage(
            [In] int nExitCode
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("ATOM")]
        public static extern ushort RegisterClassEx(
            [In] WNDCLASSEX* lpWndClassEx
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SendMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("LRESULT")]
        public static extern nint SendMessage(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In, ComAliasName("UINT")] uint Msg,
            [In, ComAliasName("WPARAM")] nuint wParam,
            [In, ComAliasName("LPARAM")] nint lParam
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HWND")]
        public static extern IntPtr SetActiveWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetForegroundWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int SetForegroundWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "ShowWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int ShowWindow(
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In] int nCmdShow
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "TranslateMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int TranslateMessage(
            [In] MSG* lpMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "UnregisterClassW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("BOOL")]
        public static extern int UnregisterClass(
            [In, ComAliasName("LPCWSTR")] char* lpClassName,
            [In, ComAliasName("HINSTANCE")] IntPtr hInstance = default
        );
        #endregion
    }
}
