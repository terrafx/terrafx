// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.Desktop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    unsafe public /* blittable */ struct WNDCLASSEX
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint cbSize;

        [ComAliasName("UINT")]
        public uint style;

        public IntPtr /* WNDPROC */ lpfnWndProc;

        public int cbClsExtra;

        public int cbWndExtra;

        [ComAliasName("HINSTANCE")]
        public void* hInstance;

        [ComAliasName("HICON")]
        public void* hIcon;

        [ComAliasName("HCURSOR")]
        public void* hCursor;

        [ComAliasName("HBRUSH")]
        public void* hbrBackground;

        [ComAliasName("LPCWSTR")]
        public char* lpszMenuName;

        [ComAliasName("LPCWSTR")]
        public char* lpszClassName;

        [ComAliasName("HICON")]
        public void* hIconSm;
        #endregion
    }
}
