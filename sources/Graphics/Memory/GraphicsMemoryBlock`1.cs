// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics
{
    /// <summary>Defines a single block of memory which can contain allocated or free regions.</summary>
    public abstract class GraphicsMemoryBlock<TMetadata> : GraphicsMemoryBlock
        where TMetadata : struct, GraphicsMemoryBlock.IMetadata
    {
#pragma warning disable IDE0044
        private TMetadata _metadata;
#pragma warning restore IDE0044

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBlock{TMetadata}" /> class.</summary>
        /// <param name="collection">The block collection which contains the memory block.</param>
        /// <param name="size">The size of the memory block, in bytes.</param>
        /// <param name="marginSize">The minimum size of free regions to keep on either side of an allocated region, in bytes.</param>
        /// <param name="minimumFreeRegionSizeToRegister">The minimum size of a free region for it to be registered as available, in bytes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is zero.</exception>
        protected GraphicsMemoryBlock(GraphicsMemoryBlockCollection collection, ulong size, ulong marginSize, ulong minimumFreeRegionSizeToRegister)
            : base(collection)
        {
            _metadata = new TMetadata();
            _metadata.Initialize(this, size, marginSize, minimumFreeRegionSizeToRegister);
        }

        /// <inheritdoc />
        public override nuint AllocatedRegionCount => _metadata.AllocatedRegionCount;

        /// <inheritdoc />
        public override bool IsEmpty => _metadata.IsEmpty;

        /// <inheritdoc />
        public override ulong LargestFreeRegionSize => _metadata.LargestFreeRegionSize;

        /// <inheritdoc />
        public override ulong MarginSize => _metadata.MarginSize;

        /// <inheritdoc />
        public override ulong MinimumFreeRegionSizeToRegister => _metadata.MinimumFreeRegionSizeToRegister;

        /// <inheritdoc />
        public override ulong Size => _metadata.Size;

        /// <inheritdoc />
        public override ulong TotalFreeRegionSize => _metadata.TotalFreeRegionSize;

        /// <inheritdoc />
        public override void Clear() => _metadata.Clear();

        /// <inheritdoc />
        public override void Free(in GraphicsMemoryBlockRegion region) => _metadata.Free(in region);

        /// <inheritdoc />
        public override bool TryAllocate(ulong size, ulong alignment, out GraphicsMemoryBlockRegion region) => _metadata.TryAllocate(size, alignment, out region);

        /// <inheritdoc />
        public override void Validate() => _metadata.Validate();
    }
}
