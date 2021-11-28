// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkBorderColor;
using static TerraFX.Interop.Vulkan.VkComponentSwizzle;
using static TerraFX.Interop.Vulkan.VkFilter;
using static TerraFX.Interop.Vulkan.VkFormat;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageViewType;
using static TerraFX.Interop.Vulkan.VkSamplerMipmapMode;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public abstract unsafe class VulkanGraphicsTexture : GraphicsTexture
{
    private const int Binding = 2;
    private const int Bound = 3;

    private readonly VkImage _vulkanImage;
    private ValueLazy<VkImageView> _vulkanImageView;
    private ValueLazy<VkSampler> _vulkanSampler;
    private protected VolatileState _state;

    private protected VulkanGraphicsTexture(VulkanGraphicsDevice device, GraphicsTextureKind kind, in GraphicsMemoryRegion<GraphicsMemoryHeap> heapRegion, GraphicsResourceCpuAccess cpuAccess, uint width, uint height, ushort depth, VkImage vulkanImage)
        : base(device, kind, in heapRegion, cpuAccess, width, height, depth)
    {
        _vulkanImage = vulkanImage;
        ThrowExternalExceptionIfNotSuccess(vkBindImageMemory(Allocator.Device.VulkanDevice, vulkanImage, Heap.VulkanDeviceMemory, heapRegion.Offset));

        _vulkanImageView = new ValueLazy<VkImageView>(CreateVulkanImageView);
        _vulkanSampler = new ValueLazy<VkSampler>(CreateVulkanSampler);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsTexture{TMetadata}" /> class.</summary>
    ~VulkanGraphicsTexture() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsResource.Allocator" />
    public new VulkanGraphicsMemoryAllocator Allocator => (VulkanGraphicsMemoryAllocator)base.Allocator;

    /// <inheritdoc />
    public new VulkanGraphicsMemoryHeap Heap => (VulkanGraphicsMemoryHeap)base.Heap;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

    /// <summary>Gets the underlying <see cref="VkImage" /> for the buffer.</summary>
    /// <exception cref="ExternalException">The call to <see cref="vkBindImageMemory(VkDevice, VkImage, VkDeviceMemory, ulong)" /> failed.</exception>
    /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
    public VkImage VulkanImage => _vulkanImage;

    /// <summary>Gets the underlying <see cref="VkImageView" /> for the buffer.</summary>
    /// <exception cref="ExternalException">The call to <see cref="vkCreateImageView(VkDevice, VkImageViewCreateInfo*, VkAllocationCallbacks*, VkImageView*)" /> failed.</exception>
    /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
    public VkImageView VulkanImageView => _vulkanImageView.Value;

    /// <summary>Gets the underlying <see cref="VkSampler" /> for the buffer.</summary>
    /// <exception cref="ExternalException">The call to <see cref="vkCreateSampler(VkDevice, VkSamplerCreateInfo*, VkAllocationCallbacks*, VkSampler*)" /> failed.</exception>
    /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
    public VkSampler VulkanSampler => _vulkanSampler.Value;

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* Map<T>()
    {
        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination));

        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* Map<T>(nuint rangeOffset, nuint rangeLength)
    {
        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination));

        return (T*)(pDestination + rangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* MapForRead<T>()
    {
        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

        void* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, &pDestination));

        var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = Offset;
        var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

        var mappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = vulkanDeviceMemory,
            offset = offset,
            size = size,
        };
        ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange));

        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
    {
        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vulkanDevice, vulkanDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination));

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
            ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange));
        }
        return (T*)(pDestination + readRangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void Unmap()
    {
        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

        vkUnmapMemory(vulkanDevice, vulkanDeviceMemory);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void UnmapAndWrite()
    {
        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

        var nonCoherentAtomSize = device.Adapter.VulkanPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = Offset;
        var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

        var mappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = vulkanDeviceMemory,
            offset = offset,
            size = size,
        };
        ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange));

        vkUnmapMemory(vulkanDevice, vulkanDeviceMemory);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength)
    {
        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

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
            ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vulkanDevice, 1, &mappedMemoryRange));
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
            HeapRegion.Collection.Free(in HeapRegion);
        }

        _state.EndDispose();
    }

    private VkImageView CreateVulkanImageView()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsTexture));

        VkImageView vulkanImageView;

        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

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
                aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                levelCount = 1,
                layerCount = 1,
            },
        };

        ThrowExternalExceptionIfNotSuccess(vkCreateImageView(vulkanDevice, &imageViewCreateInfo, pAllocator: null, &vulkanImageView));

        return vulkanImageView;
    }

    private VkSampler CreateVulkanSampler()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsTexture));

        VkSampler vulkanSampler;

        var device = Allocator.Device;

        var vulkanDevice = device.VulkanDevice;
        var vulkanDeviceMemory = Heap.VulkanDeviceMemory;

        var samplerCreateInfo = new VkSamplerCreateInfo {
            sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
            magFilter = VK_FILTER_LINEAR,
            minFilter = VK_FILTER_LINEAR,
            mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
            maxLod = 1.0f,
            borderColor = VK_BORDER_COLOR_INT_OPAQUE_WHITE
        };

        ThrowExternalExceptionIfNotSuccess(vkCreateSampler(vulkanDevice, &samplerCreateInfo, pAllocator: null, &vulkanSampler));

        return vulkanSampler;
    }

    private void DisposeVulkanImage(VkImage vulkanImage)
    {
        if (vulkanImage != VkImage.NULL)
        {
            vkDestroyImage(Allocator.Device.VulkanDevice, vulkanImage, pAllocator: null);
        }
    }

    private void DisposeVulkanImageView(VkImageView vulkanImageView)
    {
        if (vulkanImageView != VkImageView.NULL)
        {
            vkDestroyImageView(Allocator.Device.VulkanDevice, vulkanImageView, pAllocator: null);
        }
    }

    private void DisposeVulkanSampler(VkSampler vulkanSampler)
    {
        if (vulkanSampler != VkSampler.NULL)
        {
            vkDestroySampler(Allocator.Device.VulkanDevice, vulkanSampler, pAllocator: null);
        }
    }
}
