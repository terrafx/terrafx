// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared/dxgiformat.h in the Windows SDK for Windows 10.0.19041.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Graphics
{
    /// <summary>
    /// TEXEL_FORMAT specifies the format of the texels in a texture.
    /// </summary>
    public enum TEXEL_FORMAT : uint
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        TEXEL_FORMAT_UNKNOWN = 0,
        TEXEL_FORMAT_R32G32B32A32_TYPELESS = 1,
        TEXEL_FORMAT_R32G32B32A32_FLOAT = 2,
        TEXEL_FORMAT_R32G32B32A32_UINT = 3,
        TEXEL_FORMAT_R32G32B32A32_SINT = 4,
        TEXEL_FORMAT_R32G32B32_TYPELESS = 5,
        TEXEL_FORMAT_R32G32B32_FLOAT = 6,
        TEXEL_FORMAT_R32G32B32_UINT = 7,
        TEXEL_FORMAT_R32G32B32_SINT = 8,
        TEXEL_FORMAT_R16G16B16A16_TYPELESS = 9,
        TEXEL_FORMAT_R16G16B16A16_FLOAT = 10,
        TEXEL_FORMAT_R16G16B16A16_UNORM = 11,
        TEXEL_FORMAT_R16G16B16A16_UINT = 12,
        TEXEL_FORMAT_R16G16B16A16_SNORM = 13,
        TEXEL_FORMAT_R16G16B16A16_SINT = 14,
        TEXEL_FORMAT_R32G32_TYPELESS = 15,
        TEXEL_FORMAT_R32G32_FLOAT = 16,
        TEXEL_FORMAT_R32G32_UINT = 17,
        TEXEL_FORMAT_R32G32_SINT = 18,
        TEXEL_FORMAT_R32G8X24_TYPELESS = 19,
        TEXEL_FORMAT_D32_FLOAT_S8X24_UINT = 20,
        TEXEL_FORMAT_R32_FLOAT_X8X24_TYPELESS = 21,
        TEXEL_FORMAT_X32_TYPELESS_G8X24_UINT = 22,
        TEXEL_FORMAT_R10G10B10A2_TYPELESS = 23,
        TEXEL_FORMAT_R10G10B10A2_UNORM = 24,
        TEXEL_FORMAT_R10G10B10A2_UINT = 25,
        TEXEL_FORMAT_R11G11B10_FLOAT = 26,
        TEXEL_FORMAT_R8G8B8A8_TYPELESS = 27,
        TEXEL_FORMAT_R8G8B8A8_UNORM = 28,
        TEXEL_FORMAT_R8G8B8A8_UNORM_SRGB = 29,
        TEXEL_FORMAT_R8G8B8A8_UINT = 30,
        TEXEL_FORMAT_R8G8B8A8_SNORM = 31,
        TEXEL_FORMAT_R8G8B8A8_SINT = 32,
        TEXEL_FORMAT_R16G16_TYPELESS = 33,
        TEXEL_FORMAT_R16G16_FLOAT = 34,
        TEXEL_FORMAT_R16G16_UNORM = 35,
        TEXEL_FORMAT_R16G16_UINT = 36,
        TEXEL_FORMAT_R16G16_SNORM = 37,
        TEXEL_FORMAT_R16G16_SINT = 38,
        TEXEL_FORMAT_R32_TYPELESS = 39,
        TEXEL_FORMAT_D32_FLOAT = 40,
        TEXEL_FORMAT_R32_FLOAT = 41,
        TEXEL_FORMAT_R32_UINT = 42,
        TEXEL_FORMAT_R32_SINT = 43,
        TEXEL_FORMAT_R24G8_TYPELESS = 44,
        TEXEL_FORMAT_D24_UNORM_S8_UINT = 45,
        TEXEL_FORMAT_R24_UNORM_X8_TYPELESS = 46,
        TEXEL_FORMAT_X24_TYPELESS_G8_UINT = 47,
        TEXEL_FORMAT_R8G8_TYPELESS = 48,
        TEXEL_FORMAT_R8G8_UNORM = 49,
        TEXEL_FORMAT_R8G8_UINT = 50,
        TEXEL_FORMAT_R8G8_SNORM = 51,
        TEXEL_FORMAT_R8G8_SINT = 52,
        TEXEL_FORMAT_R16_TYPELESS = 53,
        TEXEL_FORMAT_R16_FLOAT = 54,
        TEXEL_FORMAT_D16_UNORM = 55,
        TEXEL_FORMAT_R16_UNORM = 56,
        TEXEL_FORMAT_R16_UINT = 57,
        TEXEL_FORMAT_R16_SNORM = 58,
        TEXEL_FORMAT_R16_SINT = 59,
        TEXEL_FORMAT_R8_TYPELESS = 60,
        TEXEL_FORMAT_R8_UNORM = 61,
        TEXEL_FORMAT_R8_UINT = 62,
        TEXEL_FORMAT_R8_SNORM = 63,
        TEXEL_FORMAT_R8_SINT = 64,
        TEXEL_FORMAT_A8_UNORM = 65,
        TEXEL_FORMAT_R1_UNORM = 66,
        TEXEL_FORMAT_R9G9B9E5_SHAREDEXP = 67,
        TEXEL_FORMAT_R8G8_B8G8_UNORM = 68,
        TEXEL_FORMAT_G8R8_G8B8_UNORM = 69,
        TEXEL_FORMAT_BC1_TYPELESS = 70,
        TEXEL_FORMAT_BC1_UNORM = 71,
        TEXEL_FORMAT_BC1_UNORM_SRGB = 72,
        TEXEL_FORMAT_BC2_TYPELESS = 73,
        TEXEL_FORMAT_BC2_UNORM = 74,
        TEXEL_FORMAT_BC2_UNORM_SRGB = 75,
        TEXEL_FORMAT_BC3_TYPELESS = 76,
        TEXEL_FORMAT_BC3_UNORM = 77,
        TEXEL_FORMAT_BC3_UNORM_SRGB = 78,
        TEXEL_FORMAT_BC4_TYPELESS = 79,
        TEXEL_FORMAT_BC4_UNORM = 80,
        TEXEL_FORMAT_BC4_SNORM = 81,
        TEXEL_FORMAT_BC5_TYPELESS = 82,
        TEXEL_FORMAT_BC5_UNORM = 83,
        TEXEL_FORMAT_BC5_SNORM = 84,
        TEXEL_FORMAT_B5G6R5_UNORM = 85,
        TEXEL_FORMAT_B5G5R5A1_UNORM = 86,
        TEXEL_FORMAT_B8G8R8A8_UNORM = 87,
        TEXEL_FORMAT_B8G8R8X8_UNORM = 88,
        TEXEL_FORMAT_R10G10B10_XR_BIAS_A2_UNORM = 89,
        TEXEL_FORMAT_B8G8R8A8_TYPELESS = 90,
        TEXEL_FORMAT_B8G8R8A8_UNORM_SRGB = 91,
        TEXEL_FORMAT_B8G8R8X8_TYPELESS = 92,
        TEXEL_FORMAT_B8G8R8X8_UNORM_SRGB = 93,
        TEXEL_FORMAT_BC6H_TYPELESS = 94,
        TEXEL_FORMAT_BC6H_UF16 = 95,
        TEXEL_FORMAT_BC6H_SF16 = 96,
        TEXEL_FORMAT_BC7_TYPELESS = 97,
        TEXEL_FORMAT_BC7_UNORM = 98,
        TEXEL_FORMAT_BC7_UNORM_SRGB = 99,
        TEXEL_FORMAT_AYUV = 100,
        TEXEL_FORMAT_Y410 = 101,
        TEXEL_FORMAT_Y416 = 102,
        TEXEL_FORMAT_NV12 = 103,
        TEXEL_FORMAT_P010 = 104,
        TEXEL_FORMAT_P016 = 105,
        TEXEL_FORMAT_420_OPAQUE = 106,
        TEXEL_FORMAT_YUY2 = 107,
        TEXEL_FORMAT_Y210 = 108,
        TEXEL_FORMAT_Y216 = 109,
        TEXEL_FORMAT_NV11 = 110,
        TEXEL_FORMAT_AI44 = 111,
        TEXEL_FORMAT_IA44 = 112,
        TEXEL_FORMAT_P8 = 113,
        TEXEL_FORMAT_A8P8 = 114,
        TEXEL_FORMAT_B4G4R4A4_UNORM = 115,
        TEXEL_FORMAT_P208 = 130,
        TEXEL_FORMAT_V208 = 131,
        TEXEL_FORMAT_V408 = 132,
        TEXEL_FORMAT_SAMPLER_FEEDBACK_MIN_MIP_OPAQUE = 189,
        TEXEL_FORMAT_SAMPLER_FEEDBACK_MIP_REGION_USED_OPAQUE = 190,
        TEXEL_FORMAT_FORCE_UINT = 0xffffffff,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
