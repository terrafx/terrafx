// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public static partial class User32
    {
        #region Methods
        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DispatchMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern LRESULT DispatchMessage(
            [In] ref /* readonly */ MSG lpMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "PeekMessageW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL PeekMessage(
            [Out] out MSG lpMsg,
            [In, Optional] HWND hWnd,
            [In] WM wMsgFilterMin,
            [In] WM wMsgFilterMax,
            [In] PM wRemoveMsg
        );

        [DllImport("User32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "TranslateMessage", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL TranslateMessage(
            [In] ref /* readonly */ MSG lpMsg
        );
        #endregion
    }
}
