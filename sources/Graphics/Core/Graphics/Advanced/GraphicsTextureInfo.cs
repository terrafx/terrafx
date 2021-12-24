// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics textures.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsTextureInfo
{
    /// <summary>The number of bytes per layer of the texture.</summary>
    public uint BytesPerLayer;

    /// <summary>The number of bytes per row of the texture.</summary>
    public uint BytesPerRow;

    /// <summary>The texture kind.</summary>
    public GraphicsTextureKind Kind;

    /// <summary>The number of mip levels in the graphics texture.</summary>
    public ushort MipLevelCount;

    /// <summary>The depth, in pixels, of the texture.</summary>
    /// <remarks>This is the number of layers in the texture.</remarks>
    public ushort PixelDepth;

    /// <summary>The format of pixels in the texture.</summary>
    public GraphicsFormat PixelFormat;

    /// <summary>The height, in pixels, of the texture.</summary>
    /// <remarks>This is the number of rows in the texture.</remarks>
    public uint PixelHeight;

    /// <summary>The width, in pixels, of the texture.</summary>
    public uint PixelWidth;
}
