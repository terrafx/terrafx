// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkBorderColor;
using static TerraFX.Interop.VkComponentSwizzle;
using static TerraFX.Interop.VkFilter;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageAspectFlagBits;
using static TerraFX.Interop.VkImageViewType;
using static TerraFX.Interop.VkSamplerMipmapMode;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsTexture<TMetadata> : GraphicsTexture<TMetadata>, IVulkanGraphicsTexture
        where TMetadata : struct, IGraphicsMemoryRegionCollection<IGraphicsResource>.IMetadata
    {
        private const int Binding = 2;
        private const int Bound = 3;

        private VkImage _vulkanImage;
        private ValueLazy<VkImageView> _vulkanImageView;
        private ValueLazy<VkSampler> _vulkanSampler;
        private State _state;

        internal VulkanGraphicsTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, in GraphicsMemoryRegion<IGraphicsMemoryBlock> memoryBlockRegion, uint width, uint height, ushort depth, VkImage vulkanImage)
            : base(kind, cpuAccess, in memoryBlockRegion, width, height, depth)
        {
            _vulkanImage = vulkanImage;
            ThrowExternalExceptionIfNotSuccess(vkBindImageMemory(Allocator.Device.VulkanDevice, vulkanImage, Block.VulkanDeviceMemory, memoryBlockRegion.Offset), nameof(vkBindImageMemory));

            _vulkanImageView = new ValueLazy<VkImageView>(CreateVulkanImageView);
            _vulkanSampler = new ValueLazy<VkSampler>(CreateVulkanSampler);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsTexture{TMetadata}" /> class.</summary>
        ~VulkanGraphicsTexture() => Dispose(isDisposing: true);

        /// <inheritdoc cref="IGraphicsResource.Allocator" />
        public new VulkanGraphicsMemoryAllocator Allocator => (VulkanGraphicsMemoryAllocator)base.Allocator;

        /// <inheritdoc />
        public new IVulkanGraphicsMemoryBlock Block => (IVulkanGraphicsMemoryBlock)base.Block;

        /// <summary>Gets the underlying <see cref="VkImage" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkBindImageMemory(IntPtr, ulong, ulong, ulong)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkImage VulkanImage => _vulkanImage;

        /// <summary>Gets the underlying <see cref="VkImageView" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateImageView(IntPtr, VkImageViewCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkImageView VulkanImageView => _vulkanImageView.Value;

        /// <summary>Gets the underlying <see cref="VkSampler" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateSampler(IntPtr, VkSamplerCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkSampler VulkanSampler => _vulkanSampler.Value;

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(IntPtr, ulong, ulong, ulong, uint, void**)" /> failed.</exception>
        public override T* Map<T>(nuint readRangeOffset, nuint readRangeLength)
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            void* pDestination;
            ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, &pDestination), nameof(vkMapMemory));

            if (readRangeLength != 0)
            {
                var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

                var offset = Offset + readRangeOffset;
                var size = (readRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = offset,
                    size = size,
                };
                ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange), nameof(vkInvalidateMappedMemoryRanges));
            }
            return (T*)pDestination;
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(IntPtr, uint, VkMappedMemoryRange*)" /> failed.</exception>
        public override void Unmap(nuint writtenRangeOffset, nuint writtenRangeLength)
        {
            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            if (writtenRangeLength != 0)
            {
                var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

                var offset = Offset + writtenRangeOffset;
                var size = (writtenRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

                var mappedMemoryRange = new VkMappedMemoryRange {
                    sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                    memory = vulkanDeviceMemory,
                    offset = offset,
                    size = size,
                };
                ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange), nameof(vkFlushMappedMemoryRanges));
            }
            vkUnmapMemory(vulkanDevice, vulkanDeviceMemory);
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanSampler.Dispose(DisposeVulkanSampler);
                _vulkanImageView.Dispose(DisposeVulkanImageView);

                DisposeVulkanImage(_vulkanImage);
                MemoryBlockRegion.Parent.Free(in MemoryBlockRegion);
            }

            _state.EndDispose();
        }

        private VkImageView CreateVulkanImageView()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkImageView vulkanImageView;

            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            var viewType = Kind switch {
                GraphicsTextureKind.OneDimensional => VK_IMAGE_VIEW_TYPE_1D,
                GraphicsTextureKind.TwoDimensional => VK_IMAGE_VIEW_TYPE_2D,
                GraphicsTextureKind.ThreeDimensional => VK_IMAGE_VIEW_TYPE_3D,
                _ => default,
            };

            var imageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                image = VulkanImage,
                viewType = viewType,
                format = VK_FORMAT_R8G8B8A8_UNORM,
                components = new VkComponentMapping {
                    r = VK_COMPONENT_SWIZZLE_R,
                    g = VK_COMPONENT_SWIZZLE_G,
                    b = VK_COMPONENT_SWIZZLE_B,
                    a = VK_COMPONENT_SWIZZLE_A,
                },
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = (uint)VK_IMAGE_ASPECT_COLOR_BIT,
                    levelCount = 1,
                    layerCount = 1,
                },
            };

            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(vulkanDevice, &imageViewCreateInfo, pAllocator: null, (ulong*)&vulkanImageView), nameof(vkCreateImageView));

            return vulkanImageView;
        }

        private VkSampler CreateVulkanSampler()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkSampler vulkanSampler;

            var device = Allocator.Device;

            var vulkanDevice = device.VulkanDevice;
            var vulkanDeviceMemory = Block.VulkanDeviceMemory;

            var samplerCreateInfo = new VkSamplerCreateInfo {
                sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VK_FILTER_LINEAR,
                minFilter = VK_FILTER_LINEAR,
                mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
                maxLod = 1.0f,
                borderColor = VK_BORDER_COLOR_INT_OPAQUE_WHITE
            };

            ThrowExternalExceptionIfNotSuccess(vkCreateSampler(vulkanDevice, &samplerCreateInfo, pAllocator: null, (ulong*)&vulkanSampler), nameof(vkCreateSampler));

            return vulkanSampler;
        }

        private void DisposeVulkanImage(VkImage vulkanImage)
        {
            if (vulkanImage != VK_NULL_HANDLE)
            {
                vkDestroyImage(Allocator.Device.VulkanDevice, vulkanImage, pAllocator: null);
            }
        }

        private void DisposeVulkanImageView(VkImageView vulkanImageView)
        {
            if (vulkanImageView != VK_NULL_HANDLE)
            {
                vkDestroyImageView(Allocator.Device.VulkanDevice, vulkanImageView, pAllocator: null);
            }
        }

        private void DisposeVulkanSampler(VkSampler vulkanSampler)
        {
            if (vulkanSampler != VK_NULL_HANDLE)
            {
                vkDestroySampler(Allocator.Device.VulkanDevice, vulkanSampler, pAllocator: null);
            }
        }
    }
}
