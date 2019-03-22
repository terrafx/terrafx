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
    public unsafe struct CREATESTRUCT
    {
        #region Fields
        [ComAliasName("LPVOID")]
        public void* lpCreateParams;

        [ComAliasName("HINSTANCE")]
        public IntPtr hInstance;

        [ComAliasName("HMENU")]
        public IntPtr hMenu;

        [ComAliasName("HWND")]
        public IntPtr hwndParent;

        int cy;

        int cx;

        int y;

        int x;

        [ComAliasName("LONG")]
        public int style;

        [ComAliasName("LPCWSTR")]
        public char* lpszName;

        [ComAliasName("LPCWSTR")]
        public char* lpszClass;

        [ComAliasName("DWORD")]
        public uint dwExStyle;
        #endregion
    }
}
