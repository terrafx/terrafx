// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum CS : uint
    {
        NONE = 0x00000000,

        VREDRAW = 0x00000001,

        HREDRAW = 0x00000002,

        DBLCLKS = 0x00000008,

        OWNDC = 0x00000020,

        CLASSDC = 0x00000040,

        PARENTDC = 0x00000080,

        NOCLOSE = 0x00000200,

        SAVEBITS = 0x00000800,

        BYTEALIGNCLIENT = 0x00001000,

        BYTEALIGNWINDOW = 0x00002000,

        GLOBALCLASS = 0x00004000,

        IME = 0x00010000,

        DROPSHADOW = 0x00020000
    }
}
