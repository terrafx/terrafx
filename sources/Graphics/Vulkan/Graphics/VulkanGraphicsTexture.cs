// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkBorderColor;
using static TerraFX.Interop.Vulkan.VkComponentSwizzle;
using static TerraFX.Interop.Vulkan.VkFilter;
using static TerraFX.Interop.Vulkan.VkFormat;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageViewType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkSamplerMipmapMode;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsTexture : GraphicsTexture
{
    private readonly VulkanGraphicsMemoryHeap _memoryHeap;
    private readonly VkImage _vkImage;
    private readonly VkImageView _vkImageView;
    private readonly VkSampler _vkSampler;

    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsTexture(VulkanGraphicsDevice device, in CreateInfo createInfo)
        : base(device, in createInfo.MemoryRegion, in createInfo.ResourceInfo, in createInfo.TextureInfo)
    {
        var memoryHeap = createInfo.MemoryRegion.Allocator.DeviceObject.As<VulkanGraphicsMemoryHeap>();
        _memoryHeap = memoryHeap;

        ThrowExternalExceptionIfNotSuccess(vkBindImageMemory(device.VkDevice, createInfo.VkImage, memoryHeap.VkDeviceMemory, createInfo.MemoryRegion.Offset));
        _vkImage = createInfo.VkImage;

        _vkImageView = CreateVkImageView(device, createInfo.TextureInfo.Kind, createInfo.VkImage);
        _vkSampler = CreateVkSampler(device);

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsTexture);

        static VkImageView CreateVkImageView(VulkanGraphicsDevice device, GraphicsTextureKind kind, VkImage vkImage)
        {
            VkImageView vkImageView;

            var vkImageViewType = kind switch {
                GraphicsTextureKind.OneDimensional => VK_IMAGE_VIEW_TYPE_1D,
                GraphicsTextureKind.TwoDimensional => VK_IMAGE_VIEW_TYPE_2D,
                GraphicsTextureKind.ThreeDimensional => VK_IMAGE_VIEW_TYPE_3D,
                _ => default,
            };

            var vkImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                image = vkImage,
                viewType = vkImageViewType,
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
            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(device.VkDevice, &vkImageViewCreateInfo, pAllocator: null, &vkImageView));

            return vkImageView;
        }

        static VkSampler CreateVkSampler(VulkanGraphicsDevice device)
        {
            VkSampler vkSampler;

            var vkSamplerCreateInfo = new VkSamplerCreateInfo {
                sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                magFilter = VK_FILTER_LINEAR,
                minFilter = VK_FILTER_LINEAR,
                mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
                maxLod = 1.0f,
                borderColor = VK_BORDER_COLOR_INT_OPAQUE_WHITE
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateSampler(device.VkDevice, &vkSamplerCreateInfo, pAllocator: null, &vkSampler));

            return vkSampler;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsTexture" /> class.</summary>
    ~VulkanGraphicsTexture() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public VulkanGraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc />
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = Device.UpdateName(VK_OBJECT_TYPE_IMAGE, VkImage, value);
            _ = Device.UpdateName(VK_OBJECT_TYPE_IMAGE_VIEW, VkImageView, value);
            _ = Device.UpdateName(VK_OBJECT_TYPE_SAMPLER, VkSampler, value);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkImage" /> for the buffer.</summary>
    public VkImage VkImage
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkImage;
        }
    }

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkImageView" /> for the buffer.</summary>
    public VkImageView VkImageView
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkImageView;
        }
    }

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkSampler" /> for the buffer.</summary>
    public VkSampler VkSampler
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkSampler;
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* Map<T>()
    {
        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(Device.VkDevice, MemoryHeap.VkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, (void**)&pDestination));
        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* Map<T>(nuint rangeOffset, nuint rangeLength)
    {
        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(Device.VkDevice, MemoryHeap.VkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, (void**)&pDestination));
        return (T*)(pDestination + rangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* MapForRead<T>()
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        void* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vkDevice, vkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, &pDestination));

        var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = MemoryRegion.Offset;
        var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = vkDeviceMemory,
            offset = offset,
            size = size,
        };
        ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));

        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vkDevice, vkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, (void**)&pDestination));

        if (readRangeLength != 0)
        {
            var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = MemoryRegion.Offset + readRangeOffset;
            var size = (readRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

            var vkMappedMemoryRange = new VkMappedMemoryRange {
                sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                memory = vkDeviceMemory,
                offset = offset,
                size = size,
            };
            ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));
        }
        return (T*)(pDestination + readRangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void Unmap()
    {
        vkUnmapMemory(Device.VkDevice, MemoryHeap.VkDeviceMemory);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void UnmapAndWrite()
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = MemoryRegion.Offset;
        var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = vkDeviceMemory,
            offset = offset,
            size = size,
        };
        ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));

        vkUnmapMemory(vkDevice, vkDeviceMemory);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength)
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        if (writtenRangeLength != 0)
        {
            var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = MemoryRegion.Offset + writtenRangeOffset;
            var size = (writtenRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

            var vkMappedMemoryRange = new VkMappedMemoryRange {
                sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                memory = vkDeviceMemory,
                offset = offset,
                size = size,
            };
            ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));
        }
        vkUnmapMemory(vkDevice, vkDeviceMemory);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            var vkDevice = Device.VkDevice;

            DisposeVkSampler(vkDevice, _vkSampler);
            DisposeVkImageView(vkDevice, _vkImageView);
            DisposeVulkanImage(vkDevice, _vkImage);

            MemoryRegion.Dispose();
        }

        _state.EndDispose();

        static void DisposeVulkanImage(VkDevice vkDevice, VkImage vkImage)
        {
            if (vkImage != VkImage.NULL)
            {
                vkDestroyImage(vkDevice, vkImage, pAllocator: null);
            }
        }

        static void DisposeVkImageView(VkDevice vkDevice, VkImageView vkImageView)
        {
            if (vkImageView != VkImageView.NULL)
            {
                vkDestroyImageView(vkDevice, vkImageView, pAllocator: null);
            }
        }

        static void DisposeVkSampler(VkDevice vkDevice, VkSampler vkSampler)
        {
            if (vkSampler != VkSampler.NULL)
            {
                vkDestroySampler(vkDevice, vkSampler, pAllocator: null);
            }
        }
    }
}
