// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.CompilerServices;
using TerraFX.Graphics;

namespace TerraFX.Utilities;

/// <summary>Defines extension methods for types in the graphics namespace.</summary>
public static class GraphicsUtilities
{
    private static readonly uint[] s_sizeMap = [
        0,  // Unknown

        16, // R32G32B32A32_SFLOAT
        16, // R32G32B32A32_UINT
        16, // R32G32B32A32_SINT

        12, // R32G32B32_SFLOAT
        12, // R32G32B32_UINT
        12, // R32G32B32_SINT

        8,  // R16G16B16A16_SFLOAT
        8,  // R16G16B16A16_UNORM
        8,  // R16G16B16A16_UINT 
        8,  // R16G16B16A16_SNORM
        8,  // R16G16B16A16_SINT

        8,  // R32G32_SFLOAT
        8,  // R32G32_UINT
        8,  // R32G32_SINT

        8,  // D32_SFLOAT_S8X24_UINT

        4,  // R10G10B10A2_UNORM
        4,  // R10G10B10A2_UINT

        4,  // R11G11B10_UFLOAT

        4,  // R8G8B8A8_UNORM
        4,  // R8G8B8A8_SRGB
        4,  // R8G8B8A8_UINT
        4,  // R8G8B8A8_SNORM
        4,  // R8G8B8A8_SINT

        4,  // R16G16_SFLOAT
        4,  // R16G16_UNORM
        4,  // R16G16_UINT
        4,  // R16G16_SNORM
        4,  // R16G16_SINT

        4,  // D32_SFLOAT

        4,  // R32_SFLOAT
        4,  // R32_UINT
        4,  // R32_SINT

        4,  // D24_UNORM_S8_UINT

        2,  // R8G8_UNORM
        2,  // R8G8_UINT
        2,  // R8G8_SNORM
        2,  // R8G8_SINT

        2,  // D16_UNORM

        2,  // R16_SFLOAT
        2,  // R16_UNORM
        2,  // R16_UINT 
        2,  // R16_SNORM
        2,  // R16_SINT

        1,  // R8_UNORM
        1,  // R8_UINT
        1,  // R8_SNORM
        1,  // R8_SINT

        4,  // R9G9B9E5_UFLOAT

        4,  // R8G8B8G8_UNORM
        4,  // G8R8G8B8_UNORM

        8,  // BC1_UNORM
        8,  // BC1_UNORM_SRGB

        16, // BC2_UNORM
        16, // BC2_UNORM_SRGB

        16, // BC3_UNORM
        16, // BC3_UNORM_SRGB

        8,  // BC4_UNORM
        8,  // BC4_SNORM

        16, // BC5_UNORM
        16, // BC5_SNORM

        2,  // B5G6R5_UNORM

        2,  // B5G5R5A1_UNORM

        4,  // B8G8R8A8_UNORM
        4,  // B8G8R8A8_SRGB

        16, // BC6H_UFLOAT
        16, // BC6H_SFLOAT

        16, // BC7_UNORM
        16, // BC7_SRGB

        3,  // NV12
        4,  // YUY2

        2,  // B4G4R4A4_UNORM
    ];

    /// <summary>Gets the size, in bytes, of a graphics format.</summary>
    /// <param name="format">The format for which to get its size.</param>
    /// <returns>The size, in bytes, of <paramref name="format" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetSize(this GraphicsFormat format) => s_sizeMap[(uint)format];
}
