// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Numerics;
using TerraFX.Interop;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageType;
using static TerraFX.Interop.VkMemoryPropertyFlagBits;
using static TerraFX.Interop.VkPhysicalDeviceType;
using static TerraFX.Interop.VkSampleCountFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsMemoryAllocator : GraphicsMemoryAllocator
    {
        private readonly VulkanGraphicsMemoryBlockCollection[] _blockCollections;

        internal VulkanGraphicsMemoryAllocator(VulkanGraphicsDevice device, ulong blockPreferredSize)
            : base(device, blockPreferredSize)
        {
            blockPreferredSize = BlockPreferredSize;

            var blockMinimumSize = blockPreferredSize >> 3;

            if (blockMinimumSize == 0)
            {
                blockMinimumSize = blockPreferredSize;
            }

            var memoryTypeCount = Device.Adapter.VulkanPhysicalDeviceMemoryProperties.memoryTypeCount;
            _blockCollections = new VulkanGraphicsMemoryBlockCollection[memoryTypeCount];

            for (uint memoryTypeIndex = 0; memoryTypeIndex < memoryTypeCount; memoryTypeIndex++)
            {
                _blockCollections[memoryTypeIndex] = new VulkanGraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, memoryTypeIndex);
            }

            // TODO: UpdateBudget
        }

        /// <inheritdoc cref="GraphicsMemoryAllocator.Device" />
        public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

        /// <inheritdoc />
        public override VulkanGraphicsBuffer CreateBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment = 0, GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None)
        {
            var vulkanDevice = Device.VulkanDevice;

            var bufferCreateInfo = new VkBufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                size = size,
                usage = GetVulkanBufferUsageKind(kind, cpuAccess)
            };

            VkBuffer vulkanBuffer;
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateBuffer), vkCreateBuffer(vulkanDevice, &bufferCreateInfo, pAllocator: null, (ulong*)&vulkanBuffer));

            VkMemoryRequirements memoryRequirements;
            vkGetBufferMemoryRequirements(vulkanDevice, vulkanBuffer, &memoryRequirements);

            var index = GetBlockCollectionIndex(cpuAccess, memoryRequirements.memoryTypeBits);
            ref readonly var blockCollection = ref _blockCollections[index];

            if (!blockCollection.TryAllocate(memoryRequirements.size, memoryRequirements.alignment, allocationFlags, out var region))
            {
                throw new OutOfMemoryException();
            }
            return new VulkanGraphicsBuffer(kind, cpuAccess, in region, vulkanBuffer);
        }

        /// <inheritdoc />
        public override VulkanGraphicsTexture CreateTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, ulong alignment = 0, GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None)
        {
            var vulkanDevice = Device.VulkanDevice;

            var imageCreateInfo = new VkImageCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                imageType = kind switch {
                    GraphicsTextureKind.OneDimensional => VK_IMAGE_TYPE_1D,
                    GraphicsTextureKind.TwoDimensional => VK_IMAGE_TYPE_2D,
                    GraphicsTextureKind.ThreeDimensional => VK_IMAGE_TYPE_3D,
                    _ => default,
                },
                format = VK_FORMAT_R8G8B8A8_UNORM,
                extent = new VkExtent3D {
                    width = width,
                    height = height,
                    depth = depth,
                },
                mipLevels = 1,
                arrayLayers = 1,
                samples = VK_SAMPLE_COUNT_1_BIT,
                usage = GetVulkanImageUsageKind(kind, cpuAccess),
            };

            VkImage vulkanImage;
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateImage), vkCreateImage(vulkanDevice, &imageCreateInfo, pAllocator: null, (ulong*)&vulkanImage));

            VkMemoryRequirements memoryRequirements;
            vkGetImageMemoryRequirements(vulkanDevice, vulkanImage, &memoryRequirements);

            var index = GetBlockCollectionIndex(cpuAccess, memoryRequirements.memoryTypeBits);
            ref readonly var blockCollection = ref _blockCollections[index];

            if (!blockCollection.TryAllocate(memoryRequirements.size, memoryRequirements.alignment, allocationFlags, out var region))
            {
                throw new OutOfMemoryException();
            }
            return new VulkanGraphicsTexture(kind, cpuAccess, in region, width, height, depth, vulkanImage);
        }

        /// <inheritdoc />
        public override void GetBudget(GraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget) => GetBudget((VulkanGraphicsMemoryBlockCollection)blockCollection, out budget);

        /// <inheritdoc cref="GetBudget(GraphicsMemoryBlockCollection, out GraphicsMemoryBudget)" />
        public void GetBudget(VulkanGraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget)
        {
            budget = new GraphicsMemoryBudget(estimatedBudget: ulong.MaxValue, estimatedUsage: 0, totalAllocatedRegionSize: 0, totalBlockSize: 0);
        }

        private int GetBlockCollectionIndex(GraphicsResourceCpuAccess cpuAccess, uint memoryTypeBits)
        {
            var isIntegratedGpu = Device.Adapter.VulkanPhysicalDeviceProperties.deviceType == VK_PHYSICAL_DEVICE_TYPE_INTEGRATED_GPU;

            VkMemoryPropertyFlagBits requiredMemoryPropertyFlags = 0;
            VkMemoryPropertyFlagBits preferredMemoryPropertyFlags = 0;
            VkMemoryPropertyFlagBits unpreferredMemoryPropertyFlags = 0;

            switch (cpuAccess)
            {
                case GraphicsResourceCpuAccess.GpuOnly:
                {
                    preferredMemoryPropertyFlags |= isIntegratedGpu ? default : VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT;
                    break;
                }

                case GraphicsResourceCpuAccess.GpuToCpu:
                {
                    requiredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT;
                    preferredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_CACHED_BIT;
                    break;
                }

                case GraphicsResourceCpuAccess.CpuToGpu:
                {
                    requiredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT;
                    preferredMemoryPropertyFlags |= isIntegratedGpu ? default : VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT;
                    break;
                }
            }

            var index = -1;
            var lowestCost = int.MaxValue;

            ref readonly var memoryProperties = ref Device.Adapter.VulkanPhysicalDeviceMemoryProperties;

            for (var i = 0; i < _blockCollections.Length; i++)
            {
                var memoryPropertyFlags = memoryProperties.memoryTypes[i].propertyFlags;

                if (((uint)requiredMemoryPropertyFlags & ~memoryPropertyFlags) != 0)
                {
                    continue;
                }

                // The cost is calculated as the number of preferred bits not present
                // added to the the number of unpreferred bits that are present. A value
                // of zero represents an ideal match and allows us to return early.

                var cost = BitOperations.PopCount((uint)preferredMemoryPropertyFlags & ~memoryPropertyFlags)
                         + BitOperations.PopCount((uint)unpreferredMemoryPropertyFlags & memoryPropertyFlags);

                if (cost >= lowestCost)
                {
                    continue;
                }

                index = i;

                if (cost == 0)
                {
                    break;
                }

                lowestCost = cost;
            }

            return index;
        }
    }
}
