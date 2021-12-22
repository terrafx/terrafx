// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Graphics;

namespace TerraFX.Advanced;

/// <summary>The information required to create a graphics texture.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly unsafe struct GraphicsTextureCreateInfo
{
    /// <summary>The flags that modify how the graphics texture is allocated.</summary>
    public GraphicsMemoryAllocationFlags AllocationFlags { get; init; }

    /// <summary>The CPU access capabilities of the texture.</summary>
    public GraphicsResourceCpuAccess CpuAccess { get; init; }

    /// <summary>The depth, in pixels, of the graphics texture.</summary>
    public ushort Depth { get; init; }

    /// <summary>The format of the texture.</summary>
    public GraphicsFormat Format { get; init; }

    /// <summary>The height, in pixels, of the graphics texture.</summary>
    public uint Height { get; init; }

    /// <summary>The kind of graphics texture to create.</summary>
    public GraphicsTextureKind Kind { get; init; }

    /// <summary>The number of mip levels in the graphics texture.</summary>
    public ushort MipLevelCount { get; init; }

    /// <summary>The width, in pixels, of the graphics texture.</summary>
    public uint Width { get; init; }
}
