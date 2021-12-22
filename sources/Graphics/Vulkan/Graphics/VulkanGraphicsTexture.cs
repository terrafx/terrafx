// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkBorderColor;
using static TerraFX.Interop.Vulkan.VkFilter;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkSamplerMipmapMode;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsTexture : GraphicsTexture
{
    private readonly ValueMutex _mapMutex;
    private readonly VulkanGraphicsMemoryHeap _memoryHeap;
    private readonly ValueList<VulkanGraphicsTextureView> _textureViews;
    private readonly ValueMutex _textureViewsMutex;
    private readonly VkImage _vkImage;
    private readonly nuint _vkNonCoherentAtomSize;
    private readonly VkSampler _vkSampler;

    internal VulkanGraphicsTexture(VulkanGraphicsDevice device, in CreateInfo createInfo)
        : base(device, in createInfo.MemoryRegion, createInfo.CpuAccess, in createInfo.TextureInfo)
    {
        _mapMutex = new ValueMutex();
        _memoryHeap = createInfo.MemoryRegion.Allocator.DeviceObject.As<VulkanGraphicsMemoryHeap>();
        _textureViewsMutex = new ValueMutex();
        _vkImage = createInfo.VkImage;
        _vkNonCoherentAtomSize = checked((nuint)Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize);
        _vkSampler = CreateVkSampler(device);

        ThrowExternalExceptionIfNotSuccess(vkBindImageMemory(device.VkDevice, createInfo.VkImage, _memoryHeap.VkDeviceMemory, createInfo.MemoryRegion.Offset));

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

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc />
    public override int Count => _textureViews.Count;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc />
    public override bool IsMapped => false;

    /// <inheritdoc />
    public override unsafe void* MappedAddress => null;

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public VulkanGraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkImage" /> for the buffer.</summary>
    public VkImage VkImage
    {
        get
        {
            AssertNotDisposed();
            return _vkImage;
        }
    }

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.nonCoherentAtomSize" /> for <see cref="Adapter" />.</summary>
    public nuint VkNonCoherentAtomSize => _vkNonCoherentAtomSize;

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkSampler" /> for the buffer.</summary>
    public VkSampler VkSampler
    {
        get
        {
            AssertNotDisposed();
            return _vkSampler;
        }
    }

    /// <inheritdoc />
    public override VulkanGraphicsTextureView CreateView(ushort mipLevelIndex, ushort mipLevelCount)
    {
        ThrowIfDisposed();

        ThrowIfNotInBounds(mipLevelIndex, MipLevelCount);
        ThrowIfNotInInsertBounds(mipLevelCount, MipLevelCount - mipLevelIndex);

        var width = Width >> mipLevelIndex;
        var height = Height >> mipLevelIndex;
        var depth = (ushort)(Depth >> mipLevelIndex);

        var rowPitch = RowPitch >> mipLevelIndex;
        var slicePitch = SlicePitch >> mipLevelIndex;

        var textureViewInfo = new GraphicsTextureViewInfo {
            Depth = depth,
            Format = Format,
            Height = height,
            Kind = Kind,
            MipLevelCount = mipLevelCount,
            MipLevelIndex = mipLevelIndex,
            RowPitch = rowPitch,
            SlicePitch = slicePitch,
            Width = width,
        };
        return new VulkanGraphicsTextureView(this, in textureViewInfo);
    }

    /// <inheritdoc />
    public override void DisposeAllViews()
    {
        using var mutex = new DisposableMutex(_textureViewsMutex, isExternallySynchronized: false);
        DisposeAllViewsInternal();
    }

    /// <inheritdoc />
    public override IEnumerator<VulkanGraphicsTextureView> GetEnumerator() => _textureViews.GetEnumerator();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _mapMutex.Dispose();
        _textureViewsMutex.Dispose();

        DisposeAllViewsInternal();

        DisposeVkSampler(Device.VkDevice, _vkSampler);
        DisposeVulkanImage(Device.VkDevice, _vkImage);

        MemoryRegion.Dispose();

        static void DisposeVulkanImage(VkDevice vkDevice, VkImage vkImage)
        {
            if (vkImage != VkImage.NULL)
            {
                vkDestroyImage(vkDevice, vkImage, pAllocator: null);
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

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_IMAGE, VkImage, value);
        Device.SetVkObjectName(VK_OBJECT_TYPE_SAMPLER, VkSampler, value);
    }

    internal void AddView(VulkanGraphicsTextureView textureView)
    {
        using var mutex = new DisposableMutex(_textureViewsMutex, isExternallySynchronized: false);
        _textureViews.Add(textureView);
    }

    internal bool RemoveView(VulkanGraphicsTextureView textureView)
    {
        using var mutex = new DisposableMutex(_textureViewsMutex, isExternallySynchronized: false);
        return _textureViews.Remove(textureView);
    }

    private void DisposeAllViewsInternal()
    {
        // This method should only be called under a mutex

        foreach (var textureView in _textureViews)
        {
            textureView.Dispose();
        }

        _textureViews.Clear();
    }
}
