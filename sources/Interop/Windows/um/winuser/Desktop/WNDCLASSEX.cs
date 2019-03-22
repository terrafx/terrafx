// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop.Desktop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [Unmanaged]
    public unsafe struct WNDCLASSEX
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint cbSize;

        [ComAliasName("UINT")]
        public uint style;

        [ComAliasName("WNDPROC")]
        public IntPtr lpfnWndProc;

        public int cbClsExtra;

        public int cbWndExtra;

        [ComAliasName("HINSTANCE")]
        public IntPtr hInstance;

        [ComAliasName("HICON")]
        public IntPtr hIcon;

        [ComAliasName("HCURSOR")]
        public IntPtr hCursor;

        [ComAliasName("HBRUSH")]
        public IntPtr hbrBackground;

        [ComAliasName("LPCWSTR")]
        public char* lpszMenuName;

        [ComAliasName("LPCWSTR")]
        public char* lpszClassName;

        [ComAliasName("HICON")]
        public IntPtr hIconSm;
        #endregion
    }
}
