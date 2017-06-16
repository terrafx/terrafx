// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum DXGI_SWAP_CHAIN_FLAG : uint
    {
        NONE = 0x00000000,

        NONPREROTATED = 0x00000001,

        ALLOW_MODE_SWITCH = 0x00000002,

        GDI_COMPATIBLE = 0x00000004,

        RESTRICTED_CONTENT = 0x00000008,

        RESTRICT_SHARED_RESOURCE_DRIVER = 0x00000010,

        DISPLAY_ONLY = 0x00000020,

        FRAME_LATENCY_WAITABLE_OBJECT = 0x00000040,

        FOREGROUND_LAYER = 0x00000080,

        FULLSCREEN_VIDEO = 0x00000100,

        YUV_VIDEO = 0x00000200,

        HW_PROTECTED = 0x00000400,

        ALLOW_TEARING = 0x00000800,

        RESTRICTED_TO_ALL_HOLOGRAPHIC_DISPLAYS = 0x00001000
    }
}
