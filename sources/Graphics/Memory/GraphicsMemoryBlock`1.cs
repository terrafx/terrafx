// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Defines a single block of memory which can contain allocated or free regions.</summary>
    /// <typeparam name="TMetadata">The type used for metadata in the single block of memory.</typeparam>
    public abstract class GraphicsMemoryBlock<TMetadata> : IGraphicsMemoryBlock
        where TMetadata : struct, IGraphicsMemoryRegionCollection<IGraphicsMemoryBlock>.IMetadata
    {
        private readonly GraphicsMemoryBlockCollection _collection;

#pragma warning disable CS0649, IDE0044
        private TMetadata _metadata;
#pragma warning restore CS0649, IDE0044

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBlock{TMetadata}" /> class.</summary>
        /// <param name="collection">The block collection which contains the memory block.</param>
        /// <param name="size">The size of the memory block, in bytes.</param>
        /// <param name="marginSize">The minimum size of free regions to keep on either side of an allocated region, in bytes.</param>
        /// <param name="minimumFreeRegionSizeToRegister">The minimum size of a free region for it to be registered as available, in bytes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is zero.</exception>
        protected GraphicsMemoryBlock(GraphicsMemoryBlockCollection collection, ulong size, ulong marginSize, ulong minimumFreeRegionSizeToRegister)
        {
            ThrowIfNull(collection, nameof(collection));

            _collection = collection;
            _metadata.Initialize(this, size, marginSize, minimumFreeRegionSizeToRegister);
        }

        /// <inheritdoc />
        public nuint AllocatedRegionCount => _metadata.AllocatedRegionCount;

        /// <inheritdoc />
        public GraphicsMemoryBlockCollection Collection => _collection;

        /// <inheritdoc />
        public bool IsEmpty => _metadata.IsEmpty;

        /// <inheritdoc />
        public ulong LargestFreeRegionSize => _metadata.LargestFreeRegionSize;

        /// <inheritdoc />
        public ulong MarginSize => _metadata.MarginSize;

        /// <inheritdoc />
        public ulong MinimumFreeRegionSizeToRegister => _metadata.MinimumFreeRegionSizeToRegister;

        /// <inheritdoc />
        public ulong Size => _metadata.Size;

        /// <inheritdoc />
        public ulong TotalFreeRegionSize => _metadata.TotalFreeRegionSize;

        /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.Allocate(ulong, ulong, uint)" />
        public GraphicsMemoryRegion<IGraphicsMemoryBlock> Allocate(ulong size, ulong alignment) => _metadata.Allocate(size, alignment, stride: 1);

        /// <inheritdoc />
        public void Clear() => _metadata.Clear();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Free(in GraphicsMemoryRegion<IGraphicsMemoryBlock> region) => _metadata.Free(in region);

        /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.TryAllocate(ulong, ulong, uint, out GraphicsMemoryRegion{TSelf})" />
        public bool TryAllocate(ulong size, ulong alignment, out GraphicsMemoryRegion<IGraphicsMemoryBlock> region) => _metadata.TryAllocate(size, alignment, stride: 1, out region);

        /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.Validate" />
        [Conditional("DEBUG")]
        public void Validate() => _metadata.Validate();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);

        GraphicsMemoryRegion<IGraphicsMemoryBlock> IGraphicsMemoryRegionCollection<IGraphicsMemoryBlock>.Allocate(ulong size, ulong alignment, uint stride) => _metadata.Allocate(size, alignment, stride);

        bool IGraphicsMemoryRegionCollection<IGraphicsMemoryBlock>.TryAllocate(ulong size, ulong alignment, uint stride, out GraphicsMemoryRegion<IGraphicsMemoryBlock> region) => _metadata.TryAllocate(size, alignment, stride, out region);

        void IGraphicsMemoryRegionCollection<IGraphicsMemoryBlock>.Validate() => Validate();
    }
}
