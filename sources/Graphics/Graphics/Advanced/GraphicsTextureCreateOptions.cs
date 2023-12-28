// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics texture.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsTextureCreateOptions
{
    /// <summary>The flags that modify how the graphics texture is allocated.</summary>
    public GraphicsMemoryAllocationFlags AllocationFlags;

    /// <summary>The CPU access capabilities of the texture.</summary>
    public GraphicsCpuAccess CpuAccess;

    /// <summary>The number of mip levels in the graphics texture.</summary>
    public ushort MipLevelCount;

    /// <summary>The depth, in pixels, of the graphics texture.</summary>
    /// <remarks>This is the number of layers in the texture.</remarks>
    public ushort PixelDepth;

    /// <summary>The format of pixels in the texture.</summary>
    public GraphicsFormat PixelFormat;

    /// <summary>The height, in pixels, of the graphics texture.</summary>
    /// <remarks>This is the number of rows in the texture.</remarks>
    public uint PixelHeight;

    /// <summary>The width, in pixels, of the graphics texture.</summary>
    public uint PixelWidth;

    /// <summary>The kind of graphics texture to create.</summary>
    public GraphicsTextureKind Kind;
}
