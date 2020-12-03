// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed class VulkanGraphicsMemoryBlockCollection : GraphicsMemoryBlockCollection
    {
        private readonly uint _vulkanMemoryTypeIndex;

        internal VulkanGraphicsMemoryBlockCollection(ulong blockMinimumSize, ulong blockPreferredSize, ulong blockMarginSize, ulong blockMinimumFreeRegionSizeToRegister, GraphicsMemoryAllocator allocator, nuint minimumBlockCount, nuint maximumBlockCount, uint memoryTypeIndex)
            : base(blockMinimumSize, blockPreferredSize, blockMarginSize, blockMinimumFreeRegionSizeToRegister, allocator, minimumBlockCount, maximumBlockCount)
        {
            _vulkanMemoryTypeIndex = memoryTypeIndex;
        }

        /// <inheritdoc cref="GraphicsMemoryBlockCollection.Allocator" />
        public new VulkanGraphicsMemoryAllocator Allocator => (VulkanGraphicsMemoryAllocator)base.Allocator;

        /// <summary>Gets the memory type index used when creating the <see cref="VkDeviceMemory" /> instance for a memory block.</summary>
        public uint VulkanMemoryTypeIndex => _vulkanMemoryTypeIndex;

        /// <inheritdoc />
        protected override VulkanGraphicsMemoryBlock<TMetadata> CreateBlock<TMetadata>(ulong size) => new VulkanGraphicsMemoryBlock<TMetadata>(
            collection: this,
            size,
            BlockMarginSize,
            BlockMinimumFreeRegionSizeToRegister
        );
    }
}
