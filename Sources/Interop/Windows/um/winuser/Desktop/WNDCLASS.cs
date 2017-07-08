// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.Desktop
{
    public struct WNDCLASS
    {
        #region Fields
        public UINT style;

        public IntPtr /* WNDPROC */ lpfnWndProc;

        public int cbClsExtra;

        public int cbWndExtra;

        public HINSTANCE hInstance;

        public HICON hIcon;

        public HCURSOR hCursor;

        public HBRUSH hbrBackground;

        public LPCWSTR lpszMenuName;

        public LPCWSTR lpszClassName;
        #endregion
    }
}
