// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using static TerraFX.Threading.VolatileState;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsTexture<TMetadata> : VulkanGraphicsTexture
        where TMetadata : struct, IGraphicsMemoryRegionCollection<GraphicsResource>.IMetadata
    {
#pragma warning disable IDE0044
        private TMetadata _metadata;
#pragma warning restore IDE0044

        internal VulkanGraphicsTexture(GraphicsTextureKind kind, in GraphicsMemoryRegion<GraphicsMemoryBlock> blockRegion, GraphicsResourceCpuAccess cpuAccess, uint width, uint height, ushort depth, VkImage vulkanImage)
            : base(kind, in blockRegion, cpuAccess, width, height, depth, vulkanImage)
        {
            var block = blockRegion.Collection;

            var minimumAllocatedRegionMarginSize = block.MinimumAllocatedRegionMarginSize;
            var minimumFreeRegionSizeToRegister = block.MinimumFreeRegionSizeToRegister;

            _metadata = new TMetadata();
            _metadata.Initialize(this, blockRegion.Size, minimumAllocatedRegionMarginSize, minimumFreeRegionSizeToRegister);

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
        public override ulong MinimumFreeRegionSizeToRegister
            => _metadata.MinimumFreeRegionSizeToRegister;

        /// <inheritdoc />
        public override ulong MinimumAllocatedRegionMarginSize
            => _metadata.MinimumAllocatedRegionMarginSize;

        /// <inheritdoc />
        public override ulong TotalFreeRegionSize
            => _metadata.TotalFreeRegionSize;

        /// <inheritdoc />
        public override GraphicsMemoryRegion<GraphicsResource> Allocate(ulong size, ulong alignment = 1)
            => _metadata.Allocate(size, alignment);

        /// <inheritdoc />
        public override void Clear()
            => _metadata.Clear();

        /// <inheritdoc />
        public override void Free(in GraphicsMemoryRegion<GraphicsResource> region)
            => _metadata.Free(in region);

        /// <inheritdoc />
        public override IEnumerator<GraphicsMemoryRegion<GraphicsResource>> GetEnumerator()
            => _metadata.GetEnumerator();

        /// <inheritdoc />
        public override bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, out GraphicsMemoryRegion<GraphicsResource> region)
            => _metadata.TryAllocate(size, alignment, out region);
    }
}
