// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Suballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaSuballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Describes an allocated or free region of a memory block.</summary>
    public readonly struct GraphicsMemoryBlockRegion : IEquatable<GraphicsMemoryBlockRegion>
    {
        private readonly ulong _size;
        private readonly ulong _offset;
        private readonly ulong _alignment;
        private readonly GraphicsMemoryBlock _block;
        private readonly GraphicsMemoryBlockRegionKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBlockRegion" /> struct.</summary>
        /// <param name="block">The block which contains the region.</param>
        /// <param name="size">The size of the region, in bytes.</param>
        /// <param name="offset">The offset of the region, in bytes.</param>
        /// <param name="alignment">The alignment of the region, in bytes.</param>
        /// <param name="kind">The region kind.</param>
        /// <exception cref="ArgumentNullException"><paramref name="block" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is <c>zero</c>.</exception>
        public GraphicsMemoryBlockRegion(GraphicsMemoryBlock block, ulong size, ulong offset, ulong alignment, GraphicsMemoryBlockRegionKind kind)
        {
            ThrowIfNull(block, nameof(block));
            ThrowIfZero(size, nameof(size));
            ThrowIfZero(alignment, nameof(alignment));

            _size = size;
            _offset = offset;
            _alignment = alignment;
            _block = block;
            _kind = kind;
        }

        /// <summary>Gets the alignment of the region, in bytes.</summary>
        public ulong Alignment => _alignment;

        /// <summary>Gets the block which contains the region.</summary>
        public GraphicsMemoryBlock Block => _block;

        /// <summary>Gets <c>true</c> if the region is allocated; otherwise, <c>false</c>.</summary>
        public bool IsAllocated => !IsFree;

        /// <summary>Gets <c>true</c> if the region is free; otherwise, <c>false</c>.</summary>
        public bool IsFree => _kind == GraphicsMemoryBlockRegionKind.Free;

        /// <summary>Gets the region kind.</summary>
        public GraphicsMemoryBlockRegionKind Kind => _kind;

        /// <summary>Gets the offset of region, in bytes.</summary>
        public ulong Offset => _offset;

        /// <summary>Gets the size of the region, in bytes.</summary>
        public ulong Size => _size;

        /// <summary>Compares two regions for equality.</summary>
        /// <param name="left">The region to compare with <paramref name="right" />.</param>
        /// <param name="right">The region to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(in GraphicsMemoryBlockRegion left, in GraphicsMemoryBlockRegion right)
            => (left.Size == right.Size)
            && (left.Offset == right.Offset)
            && (left.Alignment == right.Alignment)
            && (left.Block == right.Block)
            && (left.Kind == right.Kind);

        /// <summary>Compares two regions for inequality.</summary>
        /// <param name="left">The region to compare with <paramref name="right" />.</param>
        /// <param name="right">The region to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(in GraphicsMemoryBlockRegion left, in GraphicsMemoryBlockRegion right)
            => !(left == right);

        /// <inheritdoc />
        public override bool Equals(object? obj)
            => (obj is GraphicsMemoryBlockRegion other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(GraphicsMemoryBlockRegion other)
            => this == other;

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(Size, Offset, Alignment, Block, Kind);

        /// <summary>Creates a region with the specified kind and other properties as their existing values.</summary>
        /// <param name="kind">The new region kind.</param>
        /// <returns>A region with the specified kind and other properties as their existing values.</returns>
        public GraphicsMemoryBlockRegion WithKind(GraphicsMemoryBlockRegionKind kind)
            => new GraphicsMemoryBlockRegion(Block, Size, Offset, Alignment, kind);

        /// <summary>Creates a region with the specified size and other properties as their existing values.</summary>
        /// <param name="size">The new size of the region, in bytes.</param>
        /// <returns>A region with the specified size and other properties as their existing values.</returns>
        public GraphicsMemoryBlockRegion WithSize(ulong size)
            => new GraphicsMemoryBlockRegion(Block, size, Offset, Alignment, Kind);
    }
}
