// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum WS_EX : uint
    {
        LEFT = 0x00000000,

        LTRREADING = 0x00000000,

        RIGHTSCROLLBAR = 0x00000000,

        DLGMODALFRAME = 0x00000001,

        NOPARENTNOTIFY = 0x00000004,

        TOPMOST = 0x00000008,

        ACCEPTFILES = 0x00000010,

        TRANSPARENT = 0x00000020,

        MDICHILD = 0x00000040,

        TOOLWINDOW = 0x00000080,

        WINDOWEDGE = 0x00000100,

        PALETTEWINDOW = (TOPMOST | TOOLWINDOW | WINDOWEDGE),

        CLIENTEDGE = 0x00000200,

        OVERLAPPEDWINDOW = (WINDOWEDGE | CLIENTEDGE),

        CONTEXTHELP = 0x00000400,

        RIGHT = 0x00001000,

        RTLREADING = 0x00002000,

        LEFTSCROLLBAR = 0x00004000,

        CONTROLPARENT = 0x00010000,

        STATICEDGE = 0x00020000,

        APPWINDOW = 0x00040000,

        LAYERED = 0x00080000,

        NOINHERITLAYOUT = 0x00100000,

        NOREDIRECTIONBITMAP = 0x00200000,

        LAYOUTRTL = 0x00400000,

        COMPOSITED = 0x02000000,

        NOACTIVATE = 0x08000000
    }
}
