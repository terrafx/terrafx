// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkBorderColor;
using static TerraFX.Interop.Vulkan.VkCompareOp;
using static TerraFX.Interop.Vulkan.VkFilter;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkImageTiling;
using static TerraFX.Interop.Vulkan.VkImageType;
using static TerraFX.Interop.Vulkan.VkImageUsageFlags;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
using static TerraFX.Interop.Vulkan.VkSamplerAddressMode;
using static TerraFX.Interop.Vulkan.VkSamplerMipmapMode;
using static TerraFX.Interop.Vulkan.VkSharingMode;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.GraphicsUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsTexture : GraphicsTexture
{
    private readonly nuint _vkNonCoherentAtomSize;

    private VkImage _vkImage;
    private VkSampler _vkSampler;

    private VulkanGraphicsMemoryHeap _memoryHeap;

    private ValueList<VulkanGraphicsTextureView> _textureViews;
    private readonly ValueMutex _textureViewsMutex;

    private volatile uint _mappedCount;
    private readonly ValueMutex _mappedMutex;

    internal VulkanGraphicsTexture(VulkanGraphicsDevice device, in GraphicsTextureCreateOptions createOptions) : base(device)
    {
        device.AddTexture(this);

        TextureInfo.Kind = createOptions.Kind;
        TextureInfo.MipLevelCount = (createOptions.MipLevelCount != 0) ? createOptions.MipLevelCount : (ushort)1;
        TextureInfo.PixelDepth = createOptions.PixelDepth;
        TextureInfo.PixelFormat = createOptions.PixelFormat;
        TextureInfo.PixelHeight = createOptions.PixelHeight;
        TextureInfo.PixelWidth = createOptions.PixelWidth;

        _vkNonCoherentAtomSize = checked((nuint)Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize);

        _vkImage = CreateVkImage(in createOptions);
        _vkSampler = CreateVkSampler();

        var bytesPerRow = createOptions.PixelWidth * createOptions.PixelFormat.GetSize();
        var bytesPerLayer = bytesPerRow * createOptions.PixelHeight;

        TextureInfo.BytesPerLayer = bytesPerLayer;
        TextureInfo.BytesPerRow = bytesPerRow;

        _memoryHeap = MemoryRegion.MemoryAllocator.DeviceObject.As<VulkanGraphicsMemoryHeap>();

        _textureViews = new ValueList<VulkanGraphicsTextureView>();
        _textureViewsMutex = new ValueMutex();

        _mappedCount = 0;
        _mappedMutex = new ValueMutex();

        SetNameUnsafe(Name);

        VkImage CreateVkImage(in GraphicsTextureCreateOptions createOptions)
        {
            var device = Device;

            var vkDevice = device.VkDevice;
            var vkImageCreateInfo = new VkImageCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                pNext = null,
                flags = 0,
                imageType = GetVkImageType(createOptions.Kind),
                format = createOptions.PixelFormat.AsVkFormat(),
                extent = new VkExtent3D {
                    width = createOptions.PixelWidth,
                    height = createOptions.PixelHeight,
                    depth = createOptions.PixelDepth,
                },
                mipLevels = (createOptions.MipLevelCount != 0) ? createOptions.MipLevelCount : (ushort)1,
                arrayLayers = 1,
                samples = VK_SAMPLE_COUNT_1_BIT,
                tiling = VK_IMAGE_TILING_OPTIMAL,
                usage = GetVkImageUsageFlags(createOptions.CpuAccess),
                sharingMode = VK_SHARING_MODE_EXCLUSIVE,
                queueFamilyIndexCount = 0,
                pQueueFamilyIndices = null,
                initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
            };

            VkImage vkImage;
            ThrowExternalExceptionIfNotSuccess(vkCreateImage(vkDevice, &vkImageCreateInfo, pAllocator: null, &vkImage));

            VkMemoryRequirements vkMemoryRequirements;
            vkGetImageMemoryRequirements(vkDevice, vkImage, &vkMemoryRequirements);

            var memoryManager = device.GetMemoryManager(createOptions.CpuAccess, vkMemoryRequirements.memoryTypeBits);

            ResourceInfo.CpuAccess = createOptions.CpuAccess;
            ResourceInfo.MappedAddress = null;

            var memoryAllocationOptions = new GraphicsMemoryAllocationOptions {
                ByteLength = (nuint)vkMemoryRequirements.size,
                ByteAlignment = (nuint)vkMemoryRequirements.alignment,
                AllocationFlags = createOptions.AllocationFlags,
            };
            ResourceInfo.MemoryRegion = memoryManager.Allocate(in memoryAllocationOptions);

            var memoryHeap = MemoryRegion.MemoryAllocator.DeviceObject.As<VulkanGraphicsMemoryHeap>();
            ThrowExternalExceptionIfNotSuccess(vkBindImageMemory(vkDevice, vkImage, memoryHeap.VkDeviceMemory, MemoryRegion.ByteOffset));

            return vkImage;
        }

        VkSampler CreateVkSampler()
        {
            VkSampler vkSampler;

            var vkSamplerCreateInfo = new VkSamplerCreateInfo {
                sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                pNext = null,
                flags = 0,
                magFilter = VK_FILTER_LINEAR,
                minFilter = VK_FILTER_LINEAR,
                mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR,
                addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT,
                addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT,
                addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT,
                mipLodBias = 0.0f,
                anisotropyEnable = VkBool32.FALSE,
                maxAnisotropy = 0.0f,
                compareEnable = VkBool32.FALSE,
                compareOp = VK_COMPARE_OP_NEVER,
                minLod = 0.0f,
                maxLod = 1.0f,
                borderColor = VK_BORDER_COLOR_INT_OPAQUE_WHITE,
                unnormalizedCoordinates = VkBool32.FALSE,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateSampler(device.VkDevice, &vkSamplerCreateInfo, pAllocator: null, &vkSampler));

            return vkSampler;
        }

        VkImageType GetVkImageType(GraphicsTextureKind kind)
        {
            VkImageType vkImageType = 0;

            switch (kind)
            {
                case GraphicsTextureKind.OneDimensional:
                {
                    vkImageType = VK_IMAGE_TYPE_1D;
                    break;
                }

                case GraphicsTextureKind.TwoDimensional:
                {
                    vkImageType = VK_IMAGE_TYPE_2D;
                    break;
                }

                case GraphicsTextureKind.ThreeDimensional:
                {
                    vkImageType = VK_IMAGE_TYPE_3D;
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(kind);
                    break;
                }
            }

            return vkImageType;
        }

        VkImageUsageFlags GetVkImageUsageFlags(GraphicsCpuAccess cpuAccess)
        {
            return cpuAccess switch {
                GraphicsCpuAccess.Read => VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                GraphicsCpuAccess.Write => VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
                _ => VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
            };
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsTexture" /> class.</summary>
    ~VulkanGraphicsTexture() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public VulkanGraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkImage" /> for the buffer.</summary>
    public VkImage VkImage => _vkImage;

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.nonCoherentAtomSize" /> for <see cref="Adapter" />.</summary>
    public nuint VkNonCoherentAtomSize => _vkNonCoherentAtomSize;

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkSampler" /> for the buffer.</summary>
    public VkSampler VkSampler => _vkSampler;

    /// <inheritdoc />
    protected override VulkanGraphicsTextureView CreateViewUnsafe(in GraphicsTextureViewCreateOptions createOptions)
    {
        return new VulkanGraphicsTextureView(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            for (var index = _textureViews.Count - 1; index >= 0; index--)
            {
                var textureView = _textureViews.GetReferenceUnsafe(index);
                textureView.Dispose();
            }
            _textureViews.Clear();

            _memoryHeap = null!;
        }
        _textureViewsMutex.Dispose();

        ResourceInfo.MappedAddress = null;
        ResourceInfo.MemoryRegion.Dispose();

        var vkDevice = Device.VkDevice;

        DisposeVkSampler(vkDevice, _vkSampler);
        _vkSampler = VkSampler.NULL;

        DisposeVulkanImage(vkDevice, _vkImage);
        _vkImage = VkImage.NULL;

        _ = Device.RemoveTexture(this);

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
    protected override byte* MapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapNoMutex();
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex();
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(byteStart, byteLength);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_IMAGE, VkImage, value);
        Device.SetVkObjectName(VK_OBJECT_TYPE_SAMPLER, VkSampler, value);
    }

    /// <inheritdoc />
    protected override void UnmapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapNoMutex();
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex();
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(byteStart, byteLength);
    }

    internal void AddTextureView(VulkanGraphicsTextureView textureView)
    {
        _textureViews.Add(textureView, _textureViewsMutex);
    }

    internal byte* MapView(nuint byteStart)
    {
        return MapUnsafe() + byteStart;
    }

    internal byte* MapViewForRead(nuint byteStart, nuint byteLength)
    {
        return MapForReadUnsafe(byteStart, byteLength);
    }

    internal bool RemoveTextureView(VulkanGraphicsTextureView textureView)
    {
        return IsDisposed || _textureViews.Remove(textureView, _textureViewsMutex);
    }

    internal void UnmapView()
    {
        UnmapUnsafe();
    }

    internal void UnmapViewAndWrite(nuint byteStart, nuint byteLength)
    {
        UnmapAndWriteUnsafe(byteStart, byteLength);
    }

    private byte* MapNoMutex()
    {
        byte* mappedAddress;

        if (Interlocked.Increment(ref _mappedCount) == 1)
        {
            mappedAddress = MemoryHeap.Map();
            ResourceInfo.MappedAddress = mappedAddress;
        }
        else
        {
            mappedAddress = (byte*)ResourceInfo.MappedAddress;
        }

        return mappedAddress;
    }

    private byte* MapForReadNoMutex()
    {
        return MapForReadNoMutex(0, ByteLength);
    }

    private byte* MapForReadNoMutex(nuint byteStart, nuint byteLength)
    {
        var mappedAddress = MapNoMutex();
        var vkNonCoherentAtomSize = VkNonCoherentAtomSize;

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            pNext = null,
            memory = MemoryHeap.VkDeviceMemory,
            offset = AlignDown(MemoryRegion.ByteOffset + byteStart, vkNonCoherentAtomSize),
            size = AlignUp(byteLength, vkNonCoherentAtomSize),
        };
        ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(Device.VkDevice, 1, &vkMappedMemoryRange));

        return mappedAddress;
    }

    private void UnmapNoMutex()
    {
        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        else if (mappedCount == 0)
        {
            MemoryHeap.Unmap();
            ResourceInfo.MappedAddress = null;
        }
    }

    private void UnmapAndWriteNoMutex()
    {
        UnmapAndWriteNoMutex(0, ByteLength);
    }

    private void UnmapAndWriteNoMutex(nuint byteStart, nuint byteLength)
    {
        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        var vkNonCoherentAtomSize = VkNonCoherentAtomSize;

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            pNext = null,
            memory = MemoryHeap.VkDeviceMemory,
            offset = AlignDown(MemoryRegion.ByteOffset + byteStart, vkNonCoherentAtomSize),
            size = AlignUp(byteLength, vkNonCoherentAtomSize),
        };
        ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(Device.VkDevice, 1, &vkMappedMemoryRange));

        if (mappedCount == 0)
        {
            MemoryHeap.Unmap();
            ResourceInfo.MappedAddress = null;
        }
    }
}
