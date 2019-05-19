// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop.Desktop
{
    public static unsafe partial class User32
    {
        private const string DllName = nameof(User32);

        #region External Methods
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "AdjustWindowRect", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int AdjustWindowRect(
            [In, Out, NativeTypeName("LPRECT")] RECT* lpRect,
            [In, NativeTypeName("DWORD")] uint dwStyle,
            [In, NativeTypeName("BOOL")] int bMenu
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CloseWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int CloseWindow(
            [In, NativeTypeName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("HWND")]
        public static extern IntPtr CreateWindowEx(
            [In, NativeTypeName("DWORD")] uint dwExStyle,
            [In, Optional, NativeTypeName("LPCWSTR")] char* lpClassName,
            [In, Optional, NativeTypeName("LPCWSTR")] char* lpWindowName,
            [In, NativeTypeName("DWORD")] uint dwStyle,
            [In] int X,
            [In] int Y,
            [In] int nWidth,
            [In] int nHeight,
            [In, Optional, NativeTypeName("HWND")] IntPtr hWndParent,
            [In, Optional, NativeTypeName("HMENU")] IntPtr hMenu,
            [In, Optional, NativeTypeName("HINSTANCE")] IntPtr hInstance,
            [In, NativeTypeName("LPVOID")] void* lpParam
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("LRESULT")]
        public static extern IntPtr DefWindowProc(
            [In, NativeTypeName("HWND")] IntPtr hWnd,
            [In, NativeTypeName("UINT")] uint Msg,
            [In, NativeTypeName("WPARAM")] UIntPtr wParam,
            [In, NativeTypeName("LPARAM")] IntPtr lParam
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DestroyWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int DestroyWindow(
            [In, NativeTypeName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DispatchMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("LRESULT")]
        public static extern IntPtr DispatchMessage(
            [In] MSG* lpMsg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("HWND")]
        public static extern IntPtr GetActiveWindow(
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassInfoExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int GetClassInfoEx(
            [In, Optional, NativeTypeName("HINSTANCE")] IntPtr hInstance,
            [In, NativeTypeName("LPCWSTR")] char* lpszClass,
            [Out, NativeTypeName("LPWNDCLASSEX")] WNDCLASSEX* lpwcx
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetClassNameW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int GetClassName(
            [In, NativeTypeName("HWND")] IntPtr hWnd,
            [Out, NativeTypeName("LPWSTR")] char* lpClassName,
            [In] int nMaxCount
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetDesktopWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("HWND")]
        public static extern IntPtr GetDesktopWindow(
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtrW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("LONG_PTR")]
        public static extern IntPtr GetWindowLongPtr(
            [In, NativeTypeName("HWND")] IntPtr hWnd,
            [In] int nIndex
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetWindowRect", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int GetWindowRect(
            [In, NativeTypeName("HWND")] IntPtr hWnd,
            [Out, NativeTypeName("LPRECT")] RECT* lpRect
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "IsWindowVisible", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int IsWindowVisible(
            [In, NativeTypeName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "LoadCursorW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("HCURSOR")]
        public static extern IntPtr LoadCursor(
            [In, Optional, NativeTypeName("HINSTANCE")] IntPtr hInstance,
            [In, NativeTypeName("LPCWSTR")] char* lpCursorName
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PeekMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int PeekMessage(
            [Out, NativeTypeName("LPMSG")] MSG* lpMsg,
            [In, Optional, NativeTypeName("HWND")] IntPtr hWnd,
            [In, NativeTypeName("UINT")] uint wMsgFilterMin,
            [In, NativeTypeName("UINT")] uint wMsgFilterMax,
            [In, NativeTypeName("UINT")] uint wRemoveMsg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PostQuitMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void PostQuitMessage(
            [In] int nExitCode
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "RegisterClassExW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("ATOM")]
        public static extern ushort RegisterClassEx(
            [In] WNDCLASSEX* lpWndClassEx
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SendMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("LRESULT")]
        public static extern IntPtr SendMessage(
            [In, NativeTypeName("HWND")] IntPtr hWnd,
            [In, NativeTypeName("UINT")] uint Msg,
            [In, NativeTypeName("WPARAM")] UIntPtr wParam,
            [In, NativeTypeName("LPARAM")] IntPtr lParam
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetActiveWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("HWND")]
        public static extern IntPtr SetActiveWindow(
            [In, NativeTypeName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetForegroundWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int SetForegroundWindow(
            [In, NativeTypeName("HWND")] IntPtr hWnd
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "SetWindowLongPtrW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("LONG_PTR")]
        public static extern IntPtr SetWindowLongPtr(
            [In, NativeTypeName("HWND")] IntPtr hWnd,
            [In] int nIndex,
            [In, NativeTypeName("LONG_PTR")] IntPtr dwNewLong
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "ShowWindow", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int ShowWindow(
            [In, NativeTypeName("HWND")] IntPtr hWnd,
            [In] int nCmdShow
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "TranslateMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int TranslateMessage(
            [In] MSG* lpMsg
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "UnregisterClassW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("BOOL")]
        public static extern int UnregisterClass(
            [In, NativeTypeName("LPCWSTR")] char* lpClassName,
            [In, NativeTypeName("HINSTANCE")] IntPtr hInstance = default
        );
        #endregion
    }
}
