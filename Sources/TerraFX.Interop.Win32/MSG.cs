// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct MSG
    {
        #region Fields
        public HWND hwnd;

        public uint message;

        public WPARAM wParam;

        public LPARAM lParam;

        public DWORD time;

        public POINT pt;
        #endregion
    }
}
