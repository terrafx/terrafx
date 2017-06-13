// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgicommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum DXGI_COLOR_SPACE_TYPE : uint
    {
        RGB_FULL_G22_NONE_P709 = 0,

        RGB_FULL_G10_NONE_P709 = 1,

        RGB_STUDIO_G22_NONE_P709 = 2,

        RGB_STUDIO_G22_NONE_P2020 = 3,

        RESERVED = 4,

        YCBCR_FULL_G22_NONE_P709_X601 = 5,

        YCBCR_STUDIO_G22_LEFT_P601 = 6,

        YCBCR_FULL_G22_LEFT_P601 = 7,

        YCBCR_STUDIO_G22_LEFT_P709 = 8,

        YCBCR_FULL_G22_LEFT_P709 = 9,

        YCBCR_STUDIO_G22_LEFT_P2020 = 10,

        YCBCR_FULL_G22_LEFT_P2020 = 11,

        RGB_FULL_G2084_NONE_P2020 = 12,

        YCBCR_STUDIO_G2084_LEFT_P2020 = 13,

        RGB_STUDIO_G2084_NONE_P2020 = 14,

        YCBCR_STUDIO_G22_TOPLEFT_P2020 = 15,

        YCBCR_STUDIO_G2084_TOPLEFT_P2020 = 16,

        RGB_FULL_G22_NONE_P2020 = 17,

        YCBCR_STUDIO_GHLG_TOPLEFT_P2020 = 18,

        YCBCR_FULL_GHLG_TOPLEFT_P2020 = 19,

        CUSTOM = 0xFFFFFFFF
    }
}
