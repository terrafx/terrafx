// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum WS : uint
    {
        OVERLAPPED = 0x00000000,

        TILED = OVERLAPPED,

        TABSTOP = 0x00010000,

        MAXIMIZEBOX = 0x00010000,

        GROUP = 0x00020000,

        MINIMIZEBOX = 0x00020000,

        ICONIC = MINIMIZE,

        THICKFRAME = 0x00040000,

        SIZEBOX = THICKFRAME,

        SYSMENU = 0x00080000,

        HSCROLL = 0x00100000,

        VSCROLL = 0x00200000,

        DLGFRAME = 0x00400000,

        BORDER = 0x00800000,

        CAPTION = 0x00C00000,

        OVERLAPPEDWINDOW = (OVERLAPPED | MAXIMIZEBOX | MINIMIZEBOX | THICKFRAME | SYSMENU | CAPTION),

        TILEDWINDOW = OVERLAPPEDWINDOW,

        MAXIMIZE = 0x01000000,

        CLIPCHILDREN = 0x02000000,

        CLIPSIBLINGS = 0x04000000,

        DISABLED = 0x08000000,

        VISIBLE = 0x10000000,

        MINIMIZE = 0x20000000,

        CHILD = 0x40000000,

        CHILDWINDOW = CHILD,

        POPUP = 0x80000000,

        POPUPWINDOW = (SYSMENU | BORDER | POPUP)
    }
}
