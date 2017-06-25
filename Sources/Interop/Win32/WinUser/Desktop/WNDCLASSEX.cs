// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.Desktop
{
    public struct WNDCLASSEX
    {
        #region Fields
        public uint cbSize;

        public CS style;

        public WNDPROC lpfnWndProc;

        public int cbClsExtra;

        public int cbWndExtra;

        public HINSTANCE hInstance;

        public HICON hIcon;

        public HCURSOR hCursor;

        public HBRUSH hbrBackground;

        public LPWSTR lpszMenuName;

        public LPWSTR lpszClassName;

        public HICON hIconSm;
        #endregion
    }
}
