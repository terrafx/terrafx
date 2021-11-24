// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Suballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaSuballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Graphics;

/// <summary>Describes a region of memory within a memory region collection.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct GraphicsMemoryRegion<TCollection> : IEquatable<GraphicsMemoryRegion<TCollection>>
    where TCollection : class, IGraphicsMemoryRegionCollection<TCollection>
{
    /// <summary>Gets the alignment of the region, in bytes.</summary>
    public ulong Alignment { get; init; }

    /// <summary>Gets the collection which contains the region.</summary>
    public TCollection Collection { get; init; }

    /// <summary>Gets the device for which the memory region was created.</summary>
    public GraphicsDevice Device => Collection.Device;

    /// <summary>Gets <c>true</c> if the region is allocated; otherwise, <c>false</c>.</summary>
    public bool IsAllocated { get; init; }

    /// <summary>Gets the offset of region, in bytes.</summary>
    public ulong Offset { get; init; }

    /// <summary>Gets the size of the region, in bytes.</summary>
    public ulong Size { get; init; }

    /// <summary>Compares two regions for equality.</summary>
    /// <param name="left">The region to compare with <paramref name="right" />.</param>
    /// <param name="right">The region to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(in GraphicsMemoryRegion<TCollection> left, in GraphicsMemoryRegion<TCollection> right)
        => (left.Collection == right.Collection)
        && (left.IsAllocated == right.IsAllocated)
        && (left.Offset == right.Offset)
        && (left.Size == right.Size)
        && (left.Alignment == right.Alignment);

    /// <summary>Compares two regions for inequality.</summary>
    /// <param name="left">The region to compare with <paramref name="right" />.</param>
    /// <param name="right">The region to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(in GraphicsMemoryRegion<TCollection> left, in GraphicsMemoryRegion<TCollection> right)
        => !(left == right);

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => (obj is GraphicsMemoryRegion<TCollection> other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(GraphicsMemoryRegion<TCollection> other)
        => this == other;

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(Alignment, Collection, IsAllocated, Offset, Size);
}
