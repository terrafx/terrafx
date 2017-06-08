// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.DXGI
{
    [Flags]
    public enum DXGI_PRESENT_FLAG : uint
    {
        NONE = 0x00000000,

        TEST = 0x00000001,

        DO_NOT_SEQUENCE = 0x00000002,

        RESTART = 0x00000004,

        DO_NOT_WAIT = 0x00000008,

        STEREO_PREFER_RIGHT = 0x00000010,

        STEREO_TEMPORARY_MONO = 0x00000020,

        RESTRICT_TO_OUTPUT = 0x00000040,

        // Part of dxgidwm.h
        DDA_PROTECTED_CONTENT = 0x00000080,

        USE_DURATION = 0x00000100,

        ALLOW_TEARING = 0x00000200
    }
}
