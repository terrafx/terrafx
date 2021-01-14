// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Threading.VolatileState;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public abstract unsafe class VulkanGraphicsMemoryBlock : GraphicsMemoryBlock
    {
        private ValueLazy<VkDeviceMemory> _vulkanDeviceMemory;
        private protected VolatileState _state;

        private protected VulkanGraphicsMemoryBlock(VulkanGraphicsMemoryBlockCollection collection)
            : base(collection)
        {
            _vulkanDeviceMemory = new ValueLazy<VkDeviceMemory>(CreateVulkanDeviceMemory);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsMemoryBlock{TMetadata}" /> class.</summary>
        ~VulkanGraphicsMemoryBlock() => Dispose(isDisposing: true);

        /// <summary>Gets the <see cref="VkDeviceMemory" /> for the memory block.</summary>
        public VkDeviceMemory VulkanDeviceMemory => _vulkanDeviceMemory.Value;

        /// <inheritdoc cref="GraphicsMemoryBlock.Collection" />
        public new VulkanGraphicsMemoryBlockCollection Collection => (VulkanGraphicsMemoryBlockCollection)base.Collection;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanDeviceMemory.Dispose(DisposeVulkanDeviceMemory);
            }

            _state.EndDispose();
        }

        private VkDeviceMemory CreateVulkanDeviceMemory()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkDeviceMemory vulkanDeviceMemory;

            var collection = Collection;
            var vulkanDevice = collection.Allocator.Device.VulkanDevice;

            var memoryAllocateInfo = new VkMemoryAllocateInfo {
                sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = Size,
                memoryTypeIndex = collection.VulkanMemoryTypeIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateMemory(vulkanDevice, &memoryAllocateInfo, pAllocator: null, (ulong*)&vulkanDeviceMemory), nameof(vkAllocateMemory));

            return vulkanDeviceMemory;
        }

        private void DisposeVulkanDeviceMemory(VkDeviceMemory vulkanDeviceMemory)
        {
            if (vulkanDeviceMemory != VK_NULL_HANDLE)
            {
                vkFreeMemory(Collection.Allocator.Device.VulkanDevice, vulkanDeviceMemory, pAllocator: null);
            }
        }
    }
}
