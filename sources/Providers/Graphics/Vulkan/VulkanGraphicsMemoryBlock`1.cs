// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TerraFX.Threading.VolatileState;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsMemoryBlock<TMetadata> : VulkanGraphicsMemoryBlock
        where TMetadata : struct, IGraphicsMemoryRegionCollection<GraphicsMemoryBlock>.IMetadata
    {
#pragma warning disable IDE0044
        private TMetadata _metadata;
#pragma warning restore IDE0044

        internal VulkanGraphicsMemoryBlock(VulkanGraphicsDevice device, VulkanGraphicsMemoryBlockCollection collection, ulong size)
            : base(device, collection)
        {
            ref readonly var allocatorSettings = ref collection.Allocator.Settings;

            var minimumAllocatedRegionMarginSize = allocatorSettings.MinimumAllocatedRegionMarginSize.GetValueOrDefault();
            var minimumFreeRegionSizeToRegister = allocatorSettings.MinimumFreeRegionSizeToRegister;

            _metadata = new TMetadata();
            _metadata.Initialize(this, size, minimumAllocatedRegionMarginSize, minimumFreeRegionSizeToRegister);

            _ = _state.Transition(to: Initialized);
        }

        /// <inheritdoc />
        public override int AllocatedRegionCount
            => _metadata.AllocatedRegionCount;

        /// <inheritdoc />
        public override int Count
            => _metadata.Count;

        /// <inheritdoc />
        public override bool IsEmpty
            => _metadata.IsEmpty;

        /// <inheritdoc />
        public override ulong LargestFreeRegionSize
            => _metadata.LargestFreeRegionSize;

        /// <inheritdoc />
        public override ulong MinimumAllocatedRegionMarginSize
            => _metadata.MinimumAllocatedRegionMarginSize;

        /// <inheritdoc />
        public override ulong MinimumFreeRegionSizeToRegister
            => _metadata.MinimumFreeRegionSizeToRegister;

        /// <inheritdoc />
        public override ulong Size
            => _metadata.Size;

        /// <inheritdoc />
        public override ulong TotalFreeRegionSize
            => _metadata.TotalFreeRegionSize;

        /// <inheritdoc />
        public override GraphicsMemoryRegion<GraphicsMemoryBlock> Allocate(ulong size, ulong alignment = 1)
            => _metadata.Allocate(size, alignment);

        /// <inheritdoc />
        public override void Clear()
            => _metadata.Clear();

        /// <inheritdoc />
        public override void Free(in GraphicsMemoryRegion<GraphicsMemoryBlock> region)
            => _metadata.Free(in region);

        /// <inheritdoc />
        public override IEnumerator<GraphicsMemoryRegion<GraphicsMemoryBlock>> GetEnumerator()
            => _metadata.GetEnumerator();

        /// <inheritdoc />
        public override bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, out GraphicsMemoryRegion<GraphicsMemoryBlock> region)
            => _metadata.TryAllocate(size, alignment, out region);
    }
}
