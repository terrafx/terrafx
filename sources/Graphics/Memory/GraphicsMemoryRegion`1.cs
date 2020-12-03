// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Suballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaSuballocation struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Describes an allocated or free region of memory.</summary>
    public readonly struct GraphicsMemoryRegion<TParent> : IEquatable<GraphicsMemoryRegion<TParent>>
        where TParent : class, IGraphicsMemoryRegionCollection<TParent>
    {
        private readonly ulong _size;
        private readonly ulong _offset;
        private readonly ulong _alignment;
        private readonly TParent _parent;
        private readonly uint _stride;
        private readonly GraphicsMemoryRegionKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryRegion{TParent}" /> struct.</summary>
        /// <param name="parent">The collection which contains the region.</param>
        /// <param name="size">The size of the region, in bytes.</param>
        /// <param name="offset">The offset of the region, in bytes.</param>
        /// <param name="alignment">The alignment of the region, in bytes.</param>
        /// <param name="stride">The stride of the region, in bytes.</param>
        /// <param name="kind">The region kind.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is <c>zero</c>.</exception>
        public GraphicsMemoryRegion(TParent parent, ulong size, ulong offset, ulong alignment, uint stride, GraphicsMemoryRegionKind kind)
        {
            ThrowIfNull(parent, nameof(parent));
            ThrowIfZero(size, nameof(size));
            ThrowIfZero(alignment, nameof(alignment));

            _size = size;
            _offset = offset;
            _alignment = alignment;
            _parent = parent;
            _stride = stride;
            _kind = kind;
        }

        /// <summary>Gets the alignment of the region, in bytes.</summary>
        public ulong Alignment => _alignment;

        /// <summary>Gets <c>true</c> if the region is allocated; otherwise, <c>false</c>.</summary>
        public bool IsAllocated => !IsFree;

        /// <summary>Gets <c>true</c> if the region is free; otherwise, <c>false</c>.</summary>
        public bool IsFree => _kind == GraphicsMemoryRegionKind.Free;

        /// <summary>Gets the region kind.</summary>
        public GraphicsMemoryRegionKind Kind => _kind;

        /// <summary>Gets the offset of region, in bytes.</summary>
        public ulong Offset => _offset;

        /// <summary>Gets the collection which contains the region.</summary>
        public TParent Parent => _parent;

        /// <summary>Gets the size of the region, in bytes.</summary>
        public ulong Size => _size;

        /// <summary>Gets the stride of the region, in bytes.</summary>
        public uint Stride => _stride;

        /// <summary>Compares two regions for equality.</summary>
        /// <param name="left">The region to compare with <paramref name="right" />.</param>
        /// <param name="right">The region to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(in GraphicsMemoryRegion<TParent> left, in GraphicsMemoryRegion<TParent> right)
            => (left.Size == right.Size)
            && (left.Offset == right.Offset)
            && (left.Alignment == right.Alignment)
            && (left.Parent == right.Parent)
            && (left.Kind == right.Kind);

        /// <summary>Compares two regions for inequality.</summary>
        /// <param name="left">The region to compare with <paramref name="right" />.</param>
        /// <param name="right">The region to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(in GraphicsMemoryRegion<TParent> left, in GraphicsMemoryRegion<TParent> right)
            => !(left == right);

        /// <inheritdoc />
        public override bool Equals(object? obj)
            => (obj is GraphicsMemoryRegion<TParent> other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(GraphicsMemoryRegion<TParent> other)
            => this == other;

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(Size, Offset, Alignment, Parent, Kind);

        /// <summary>Creates a region with the specified kind and other properties as their existing values.</summary>
        /// <param name="kind">The new region kind.</param>
        /// <returns>A region with the specified kind and other properties as their existing values.</returns>
        public GraphicsMemoryRegion<TParent> WithKind(GraphicsMemoryRegionKind kind)
            => new GraphicsMemoryRegion<TParent>(Parent, Size, Offset, Alignment, Stride, kind);

        /// <summary>Creates a region with the specified size and other properties as their existing values.</summary>
        /// <param name="size">The new size of the region, in bytes.</param>
        /// <returns>A region with the specified size and other properties as their existing values.</returns>
        public GraphicsMemoryRegion<TParent> WithSize(ulong size)
            => new GraphicsMemoryRegion<TParent>(Parent, size, Offset, Alignment, Stride, Kind);
    }
}
