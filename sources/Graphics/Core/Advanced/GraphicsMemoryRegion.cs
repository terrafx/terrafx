// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Suballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaSuballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.InteropServices;
using TerraFX.Graphics;

namespace TerraFX.Advanced;

/// <summary>Describes a region of memory within an allocator.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct GraphicsMemoryRegion : IDisposable, IEquatable<GraphicsMemoryRegion>
{
    /// <summary>Gets the alignment, in bytes, of the memory region.</summary>
    public nuint Alignment { get; init; }

    /// <summary>Gets the allocator which contains the memory region.</summary>
    public GraphicsMemoryAllocator Allocator { get; init; }

    /// <summary>Gets <c>true</c> if the memory region is allocated; otherwise, <c>false</c>.</summary>
    public bool IsAllocated { get; init; }

    /// <summary>Gets the offset, in bytes, of the memory region.</summary>
    public nuint Offset { get; init; }

    /// <summary>Gets the size, in bytes, of the memory region.</summary>
    public nuint Size { get; init; }

    /// <summary>Compares two memory regions for equality.</summary>
    /// <param name="left">The memory region to compare with <paramref name="right" />.</param>
    /// <param name="right">The memory region to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(in GraphicsMemoryRegion left, in GraphicsMemoryRegion right)
    {
        return left.Alignment == right.Alignment
            && left.Allocator == right.Allocator
            && left.IsAllocated == right.IsAllocated
            && left.Offset == right.Offset
            && left.Size == right.Size;
    }

    /// <summary>Compares two memory regions for inequality.</summary>
    /// <param name="left">The memory region to compare with <paramref name="right" />.</param>
    /// <param name="right">The memory region to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(in GraphicsMemoryRegion left, in GraphicsMemoryRegion right)
    {
        return left.Alignment != right.Alignment
            || left.Allocator != right.Allocator
            || left.IsAllocated != right.IsAllocated
            || left.Offset != right.Offset
            || left.Size != right.Size;
    }

    /// <inheritdoc />
    public void Dispose() => Allocator.Free(in this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is GraphicsMemoryRegion other && Equals(other);

    /// <inheritdoc />
    public bool Equals(GraphicsMemoryRegion other)
    {
        return Alignment.Equals(other.Alignment)
            && Allocator.Equals(other.Allocator)
            && IsAllocated.Equals(other.IsAllocated)
            && Offset.Equals(other.Offset)
            && Size.Equals(other.Size);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Alignment, Allocator, IsAllocated, Offset, Size);
}
