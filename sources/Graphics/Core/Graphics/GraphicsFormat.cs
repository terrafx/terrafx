// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>Defines the format kind for a graphics resource.</summary>
public enum GraphicsFormat
{
    /// <summary>An unknown graphics format.</summary>
    Unknown,

    /// <summary>A 4-component, 128-bit, format that uses a 32-bit signed floating-point for each component.</summary>
    R32G32B32A32_SFLOAT,

    /// <summary>A 4-component, 128-bit, format that uses a 32-bit unsigned integer for each component.</summary>
    R32G32B32A32_UINT,

    /// <summary>A 4-component, 128-bit, format that uses a 32-bit signed integer for each component.</summary>
    R32G32B32A32_SINT,

    /// <summary>A 3-component, 96-bit, format that uses a 32-bit signed floating-point for each component.</summary>
    R32G32B32_SFLOAT,

    /// <summary>A 3-component, 96-bit, format that uses a 32-bit unsigned integer for each component.</summary>
    R32G32B32_UINT,

    /// <summary>A 3-component, 96-bit, format that uses a 32-bit signed integer for each component.</summary>
    R32G32B32_SINT,

    /// <summary>A 4-component, 64-bit, format that uses a 16-bit signed floating-point for each component.</summary>
    R16G16B16A16_SFLOAT,

    /// <summary>A 4-component, 64-bit, format that uses a 16-bit unsigned normalized integer for each component.</summary>
    R16G16B16A16_UNORM,

    /// <summary>A 4-component, 64-bit, format that uses a 16-bit unsigned integer for each component.</summary>
    R16G16B16A16_UINT,

    /// <summary>A 4-component, 64-bit, format that uses a 16-bit signed normalized integer for each component.</summary>
    R16G16B16A16_SNORM,

    /// <summary>A 4-component, 64-bit, format that uses a 16-bit signed integer for each component.</summary>
    R16G16B16A16_SINT,

    /// <summary>A 2-component, 64-bit, format that uses a 32-bit signed floating-point for each component.</summary>
    R32G32_SFLOAT,

    /// <summary>A 2-component, 64-bit, format that uses a 32-bit unsigned integer for each component.</summary>
    R32G32_UINT,

    /// <summary>A 2-component, 64-bit, format that uses a 32-bit signed integer for each component.</summary>
    R32G32_SINT,

    /// <summary>A 3-component, 64-bit, format that uses a 32-bit signed floating-point for the depth component, an 8-bit signed integer for the stencil component, and a 24-bit unused value for the third component.</summary>
    D32_SFLOAT_S8X24_UINT,

    /// <summary>A 4-component, 32-bit, format that uses a 10-bit unsigned normalized integer for the first three components and a 2-bit unsigned normalized integer for the last component.</summary>
    R10G10B10A2_UNORM,

    /// <summary>A 4-component, 32-bit, format that uses a 10-bit unsigned integer for the first three components and a 2-bit unsigned integer for the last component.</summary>
    R10G10B10A2_UINT,

    /// <summary>A 3-component, 32-bit, format that uses an 11-bit unsigned-floating-point for the first two components and a 10-bit unsigned floating-point for the last component.</summary>
    R11G11B10_UFLOAT,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned normalized integer for each component.</summary>
    R8G8B8A8_UNORM,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned normalized sRGB integer for each component.</summary>
    R8G8B8A8_SRGB,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned integer for each component.</summary>
    R8G8B8A8_UINT,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit signed normalized integer for each component.</summary>
    R8G8B8A8_SNORM,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit signed integer for each component.</summary>
    R8G8B8A8_SINT,

    /// <summary>A 2-component, 32-bit, format that uses a 16-bit signed floating-point for each component.</summary>
    R16G16_SFLOAT,

    /// <summary>A 2-component, 32-bit, format that uses a 16-bit unsigned normalized integer for each component.</summary>
    R16G16_UNORM,

    /// <summary>A 2-component, 32-bit, format that uses a 16-bit unsigned integer for each component.</summary>
    R16G16_UINT,

    /// <summary>A 2-component, 32-bit, format that uses a 16-bit signed normalized integer for each component.</summary>
    R16G16_SNORM,

    /// <summary>A 2-component, 32-bit, format that uses a 16-bit signed integer for each component.</summary>
    R16G16_SINT,

    /// <summary>A 1-component, 32-bit, format that uses a 32-bit signed floating-point for the depth component.</summary>
    D32_SFLOAT,

    /// <summary>A 1-component, 32-bit, format that uses a 32-bit signed floating-point for the component.</summary>
    R32_SFLOAT,

    /// <summary>A 1-component, 32-bit, format that uses a 32-bit unsigned integer for the component.</summary>
    R32_UINT,

    /// <summary>A 1-component, 32-bit, format that uses a 32-bit signed integer for the component.</summary>
    R32_SINT,

    /// <summary>A 2-component, 32-bit, format that uses a 24-bit unsigned normalized integer for the depth component and an 8-bit unsigned integer for the stencil component.</summary>
    D24_UNORM_S8_UINT,

    /// <summary>A 2-component, 16-bit, format that uses an 8-bit unsigned normalized integer for each component.</summary>
    R8G8_UNORM,

    /// <summary>A 2-component, 16-bit, format that uses an 8-bit unsigned integer for each component.</summary>
    R8G8_UINT,

    /// <summary>A 2-component, 16-bit, format that uses an 8-bit signed normalized integer for each component.</summary>
    R8G8_SNORM,

    /// <summary>A 2-component, 16-bit, format that uses an 8-bit signed integer for each component.</summary>
    R8G8_SINT,

    /// <summary>A 1-component, 16-bit, format that uses a 16-bit unsigned normalized integer for the depth component.</summary>
    D16_UNORM,

    /// <summary>A 1-component, 16-bit, format that uses a 16-bit signed floating-point for the component.</summary>
    R16_SFLOAT,

    /// <summary>A 1-component, 16-bit, format that uses a 16-bit unsigned normalized integer for the component.</summary>
    R16_UNORM,

    /// <summary>A 1-component, 16-bit, format that uses a 16-bit unsigned integer for the component.</summary>
    R16_UINT,

    /// <summary>A 1-component, 16-bit, format that uses a 16-bit signed normalized integer for the component.</summary>
    R16_SNORM,

    /// <summary>A 1-component, 16-bit, format that uses a 16-bit signed integer for the component.</summary>
    R16_SINT,

    /// <summary>A 1-component, 8-bit, format that uses an 8-bit unsigned normalized integer for the component.</summary>
    R8_UNORM,

    /// <summary>A 1-component, 8-bit, format that uses an 8-bit unsigned integer for the component.</summary>
    R8_UINT,

    /// <summary>A 1-component, 8-bit, format that uses an 8-bit signed normalized integer for the component.</summary>
    R8_SNORM,

    /// <summary>A 1-component, 8-bit, format that uses an 8-bit signed integer for the component.</summary>
    R8_SINT,

    /// <summary>A 3-component, 32-bit, format that uses a 13-bit unsigned floating-point for each component.</summary>
    /// <remarks>The 5-bit exponent is shared for each component.</remarks>
    R9G9B9E5_UFLOAT,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned normalized integer for each component.</summary>
    /// <remarks>
    ///     <para>This format is analogous to the <c>UYVY</c> format.</para>
    ///     <para>Each block describes two pixels <c>(R8, G8, B8)</c> where the <c>R8</c> and <c>B8</c> pixels are shared.</para>
    /// </remarks>
    R8G8B8G8_UNORM,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned normalized integer for each component.</summary>
    /// <remarks>
    ///     <para>This format is analogous to the <c>YUY2</c> format.</para>
    ///     <para>Each block describes two pixels <c>(R8, G8, B8)</c> where the <c>R8</c> and <c>B8</c> pixels are shared.</para>
    /// </remarks>
    G8R8G8B8_UNORM,

    /// <summary>A 4-component, 64-bit, block-compressed format that uses unsigned normalized integers.</summary>
    BC1_UNORM,

    /// <summary>A 4-component, 64-bit, block-compressed format that uses unsigned normalized sRGB integers.</summary>
    BC1_SRGB,

    /// <summary>A 4-component, 128-bit, block-compressed format that uses unsigned normalized integers.</summary>
    BC2_UNORM,

    /// <summary>A 4-component, 128-bit, block-compressed format that uses unsigned normalized sRGB integers.</summary>
    BC2_SRGB,

    /// <summary>A 4-component, 128-bit, block-compressed format that uses unsigned normalized integers.</summary>
    BC3_UNORM,

    /// <summary>A 4-component, 128-bit, block-compressed format that uses unsigned normalized sRGB integers.</summary>
    BC3_SRGB,

    /// <summary>A 1-component, 64-bit, block-compressed format that uses unsigned normalized integers.</summary>
    BC4_UNORM,

    /// <summary>A 1-component, 64-bit, block-compressed format that uses signed normalized integers.</summary>
    BC4_SNORM,

    /// <summary>A 2-component, 128-bit, block-compressed format that uses unsigned normalized integers.</summary>
    BC5_UNORM,

    /// <summary>A 2-component, 128-bit, block-compressed format that uses signed normalized integers.</summary>
    BC5_SNORM,

    /// <summary>A 3-component, 16-bit, format that uses a 5-bit unsigned normalized integer for the first and last component and a 6-bit unsigned normalized integer for the second component.</summary>
    B5G6R5_UNORM,

    /// <summary>A 4-component, 16-bit, format that uses a 5-bit unsigned normalized integer for the first three components and a 1-bit unsigned normalized integer for the last component.</summary>
    B5G5R5A1_UNORM,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned normalized integer for each component.</summary>
    B8G8R8A8_UNORM,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned normalized sRGB integer for each component.</summary>
    B8G8R8A8_SRGB,

    /// <summary>A 3-component, 128-bit, block-compressed format that uses unsigned floating-points.</summary>
    BC6H_UFLOAT,

    /// <summary>A 3-component, 128-bit, block-compressed format that uses signed floating-points.</summary>
    BC6H_SFLOAT,

    /// <summary>A 4-component, 128-bit, block-compressed format that uses unsigned normalized integers.</summary>
    BC7_UNORM,

    /// <summary>A 4-component, 128-bit, block-compressed format that uses unsigned normalized sRGB integers.</summary>
    BC7_SRGB,

    /// <summary>A 3-component, 24-bit, multi-plane format that uses unsigned normalized integers with 1-component in the first place and 2-components in the second plane.</summary>
    /// <remarks>This is the most common <c>YUV 4:2:0</c> video format.</remarks>
    NV12,

    /// <summary>A 4-component, 32-bit, format that uses an 8-bit unsigned normalized integer for each component.</summary>
    /// <remarks>
    ///     <para>This is the most common <c>YUV 4:2:2</c> video format.</para>
    ///     <para>Each block describes two pixels <c>(R8, G8, B8)</c> where the <c>R8</c> and <c>B8</c> pixels are shared.</para>
    /// </remarks>
    YUY2,

    /// <summary>A 4-component, 16-bit, format that uses a 4-bit unsigned normalized integer for each component.</summary>
    B4G4R4A4_UNORM,
}
