// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics buffer.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsBufferCreateOptions
{
    /// <summary>The flags that modify how the graphics texture is allocated.</summary>
    public GraphicsMemoryAllocationFlags AllocationFlags;

    /// <summary>The kind of graphics buffer to create.</summary>
    public GraphicsBufferKind Kind;

    /// <summary>The length, in bytes, of the graphics buffer.</summary>
    public nuint ByteLength;

    /// <summary>A function pointer to a method which creates the backing memory allocator used by the buffer or <c>null</c> to use the system provided default memory allocator.</summary>
    public GraphicsMemoryAllocatorCreateFunc CreateMemorySuballocator;

    /// <summary>The CPU access capabilities of the buffer.</summary>
    public GraphicsCpuAccess CpuAccess;
}
