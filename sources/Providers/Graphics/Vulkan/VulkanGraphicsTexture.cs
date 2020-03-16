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
using static TerraFX.Interop.VkImageType;
using static TerraFX.Interop.VkImageViewType;
using static TerraFX.Interop.VkImageUsageFlagBits;
using static TerraFX.Interop.VkSampleCountFlagBits;
using static TerraFX.Interop.VkSamplerMipmapMode;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsTexture : GraphicsTexture
    {
        private ValueLazy<VkImage> _vulkanImage;
        private ValueLazy<VkImageView> _vulkanImageView;
        private ValueLazy<VkSampler> _vulkanSampler;
        private State _state;

        internal VulkanGraphicsTexture(GraphicsTextureKind kind, VulkanGraphicsHeap graphicsHeap, ulong offset, ulong size, ulong width, uint height, ushort depth)
            : base(kind, graphicsHeap, offset, size, width, height, depth)
        {
            _vulkanImage = new ValueLazy<VkImage>(CreateVulkanImage);
            _vulkanImageView = new ValueLazy<VkImageView>(CreateVulkanImageView);
            _vulkanSampler = new ValueLazy<VkSampler>(CreateVulkanSampler);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsTexture" /> class.</summary>
        ~VulkanGraphicsTexture()
        {
            Dispose(isDisposing: true);
        }

        /// <summary>Gets the underlying <see cref="VkImage" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateImage(IntPtr, VkImageCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="vkBindImageMemory(IntPtr, ulong, ulong, ulong)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkImage VulkanImage => _vulkanImage.Value;

        /// <summary>Gets the underlying <see cref="VkImageView" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateImageView(IntPtr, VkImageViewCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkImageView VulkanImageView => _vulkanImageView.Value;

        /// <summary>Gets the underlying <see cref="VkSampler" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateSampler(IntPtr, VkSamplerCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkSampler VulkanSampler => _vulkanSampler.Value;

        /// <inheritdoc cref="GraphicsResource.GraphicsHeap" />
        public VulkanGraphicsHeap VulkanGraphicsHeap => (VulkanGraphicsHeap)GraphicsHeap;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanSampler.Dispose(DisposeVulkanSampler);
                _vulkanImageView.Dispose(DisposeVulkanImageView);
                _vulkanImage.Dispose(DisposeVulkanImage);
            }

            _state.EndDispose();
        }

        private VkImage CreateVulkanImage()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkImage vulkanImage;

            var vulkanGraphicsHeap = VulkanGraphicsHeap;
            var vulkanDeviceMemory = vulkanGraphicsHeap.VulkanDeviceMemory;

            var vulkanGraphicsDevice = vulkanGraphicsHeap.VulkanGraphicsDevice;
            var vulkanDevice = vulkanGraphicsDevice.VulkanDevice;

            var imageType = Kind switch {
                GraphicsTextureKind.OneDimensional => VK_IMAGE_TYPE_1D,
                GraphicsTextureKind.TwoDimensional => VK_IMAGE_TYPE_2D,
                GraphicsTextureKind.ThreeDimensional => VK_IMAGE_TYPE_3D,
                _ => default,
            };

            var imageCreateInfo = new VkImageCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                imageType = imageType,
                format = VK_FORMAT_R8G8B8A8_UNORM,
                extent = new VkExtent3D {
                    width = (uint)Width,
                    height = Height,
                    depth = Depth,
                },
                mipLevels = 1,
                arrayLayers = 1,
                samples = VK_SAMPLE_COUNT_1_BIT,
                usage = GetVulkanImageUsageKind(vulkanGraphicsHeap.CpuAccess, Kind),
            };

            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateImage), vkCreateImage(vulkanDevice, &imageCreateInfo, pAllocator: null, (ulong*)&vulkanImage));
            ThrowExternalExceptionIfNotSuccess(nameof(vkBindImageMemory), vkBindImageMemory(vulkanDevice, vulkanImage, vulkanDeviceMemory, Offset));

            return vulkanImage;

            static uint GetVulkanImageUsageKind(GraphicsHeapCpuAccess cpuAccess, GraphicsTextureKind kind)
            {
                var vulkanBufferUsageKind = VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_TRANSFER_SRC_BIT;
                vulkanBufferUsageKind |= VK_IMAGE_USAGE_SAMPLED_BIT;
                return (uint)vulkanBufferUsageKind;
            }
        }

        private VkImageView CreateVulkanImageView()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkImageView vulkanImageView;

            var vulkanGraphicsHeap = VulkanGraphicsHeap;
            var vulkanDeviceMemory = vulkanGraphicsHeap.VulkanDeviceMemory;

            var vulkanGraphicsDevice = vulkanGraphicsHeap.VulkanGraphicsDevice;
            var vulkanDevice = vulkanGraphicsDevice.VulkanDevice;

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

            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateImageView), vkCreateImageView(vulkanDevice, &imageViewCreateInfo, pAllocator: null, (ulong*)&vulkanImageView));

            return vulkanImageView;
        }

        private VkSampler CreateVulkanSampler()
        {
            _state.ThrowIfDisposedOrDisposing();

            VkSampler vulkanSampler;

            var vulkanGraphicsHeap = VulkanGraphicsHeap;
            var vulkanDeviceMemory = vulkanGraphicsHeap.VulkanDeviceMemory;

            var vulkanGraphicsDevice = vulkanGraphicsHeap.VulkanGraphicsDevice;
            var vulkanDevice = vulkanGraphicsDevice.VulkanDevice;

            var samplerCreateInfo = new VkSamplerCreateInfo {
                sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VK_FILTER_LINEAR,
                minFilter = VK_FILTER_LINEAR,
                mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
                maxLod = 1.0f,
                borderColor = VK_BORDER_COLOR_INT_OPAQUE_WHITE
            };

            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateSampler), vkCreateSampler(vulkanDevice, &samplerCreateInfo, pAllocator: null, (ulong*)&vulkanSampler));

            return vulkanSampler;
        }

        private void DisposeVulkanImage(VkImage vulkanImage)
        {
            if (vulkanImage != VK_NULL_HANDLE)
            {
                vkDestroyImage(VulkanGraphicsHeap.VulkanGraphicsDevice.VulkanDevice, vulkanImage, pAllocator: null);
            }
        }

        private void DisposeVulkanImageView(VkImageView vulkanImageView)
        {
            if (vulkanImageView != VK_NULL_HANDLE)
            {
                vkDestroyImageView(VulkanGraphicsHeap.VulkanGraphicsDevice.VulkanDevice, vulkanImageView, pAllocator: null);
            }
        }

        private void DisposeVulkanSampler(VkSampler vulkanSampler)
        {
            if (vulkanSampler != VK_NULL_HANDLE)
            {
                vkDestroySampler(VulkanGraphicsHeap.VulkanGraphicsDevice.VulkanDevice, vulkanSampler, pAllocator: null);
            }
        }
    }
}
