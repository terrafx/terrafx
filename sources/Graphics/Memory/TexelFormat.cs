// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{

    /// <summary>
    /// TexelFormat specifies the format of the texels in a texture.
    /// </summary>
    public enum TexelFormat
    {
        /// <summary>Defines four channels RGBA, stored as 8 bit unsigned int each, to be interpreted as float4.</summary>
        R8G8B8A8_UNORM,

        /// <summary>Defines one channel X, 16 bit signed int.</summary>
        R16_SINT,

        /// <summary>Defines two channels RG, 16 bit unsigned int each.</summary>
        R16G16UINT,
    }
}
