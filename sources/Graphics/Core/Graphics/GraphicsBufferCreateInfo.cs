// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>The information required to create a graphics buffer.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly unsafe struct GraphicsBufferCreateInfo
{
    /// <summary>The flags that modify how the graphics texture is allocated.</summary>
    public GraphicsMemoryAllocationFlags AllocationFlags { get; init; }

    /// <summary>A function pointer to a method which creates the backing memory allocator used by the buffer or <c>null</c> to use the system provided default memory allocator.</summary>
    public delegate*<GraphicsDeviceObject, delegate*<in GraphicsMemoryRegion, void>, nuint, bool, GraphicsMemoryAllocator> CreateMemoryAllocator { get; init; }

    /// <summary>The CPU access capabilities of the buffer.</summary>
    public GraphicsResourceCpuAccess CpuAccess { get; init; }

    /// <summary>The kind of graphics buffer to create.</summary>
    public GraphicsBufferKind Kind { get; init; }

    /// <summary>The size, in bytes, of the graphics buffer.</summary>
    public nuint Size { get; init; }
}
