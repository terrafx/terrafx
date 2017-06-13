// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum DXGI_SWAP_CHAIN_FLAG : uint
    {
        NONE = 0,

        NONPREROTATED = 1,

        ALLOW_MODE_SWITCH = 2,

        GDI_COMPATIBLE = 4,

        RESTRICTED_CONTENT = 8,

        RESTRICT_SHARED_RESOURCE_DRIVER = 16,

        DISPLAY_ONLY = 32,

        FRAME_LATENCY_WAITABLE_OBJECT = 64,

        FOREGROUND_LAYER = 128,

        FULLSCREEN_VIDEO = 256,

        YUV_VIDEO = 512,

        HW_PROTECTED = 1024,

        ALLOW_TEARING = 2048,

        RESTRICTED_TO_ALL_HOLOGRAPHIC_DISPLAYS = 4096
    }
}
