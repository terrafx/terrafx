// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics texture views.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsTextureViewInfo
{
    /// <summary>The number of bytes per layer of the texture view.</summary>
    public uint BytesPerLayer;

    /// <summary>The number of bytes per row of the texture view.</summary>
    public uint BytesPerRow;

    /// <summary>The texture view kind.</summary>
    public GraphicsTextureKind Kind;

    /// <summary>The number of mip levels in the graphics texture view.</summary>
    public ushort MipLevelCount;

    /// <summary>The index of the first mip level in the graphics texture view.</summary>
    public ushort MipLevelStart;

    /// <summary>The depth, in pixels, of the texture view.</summary>
    /// <remarks>This is the number of layers in the texture view.</remarks>
    public ushort PixelDepth;

    /// <summary>The format of pixels in the texture view.</summary>
    public GraphicsFormat PixelFormat;

    /// <summary>The height, in pixels, of the texture view.</summary>
    /// <remarks>This is the number of rows in the texture view.</remarks>
    public uint PixelHeight;

    /// <summary>The width, in pixels, of the texture view.</summary>
    public uint PixelWidth;
}
