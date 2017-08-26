// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct MSG
    {
        #region Fields
        [ComAliasName("HWND")]
        public IntPtr hwnd;

        [ComAliasName("UINT")]
        public uint message;

        [ComAliasName("WPARAM")]
        public nuint wParam;

        [ComAliasName("LPARAM")]
        public nint lParam;

        [ComAliasName("DWORD")]
        public uint time;

        public POINT pt;
        #endregion
    }
}
