// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkComponentSwizzle;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageViewType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.GraphicsUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsTextureView : GraphicsTextureView
{
    private VkImageView _vkImageView;

    internal VulkanGraphicsTextureView(VulkanGraphicsTexture texture, in GraphicsTextureViewCreateOptions createOptions) : base(texture)
    {
        texture.AddTextureView(this);

        _vkImageView = CreateVkImageView(in createOptions);

        var mipLevelStart = createOptions.MipLevelStart;

        var pixelWidth = texture.PixelWidth >> mipLevelStart;
        var pixelHeight = texture.PixelHeight >> mipLevelStart;
        var pixelDepth = (ushort)(texture.PixelDepth >> mipLevelStart);

        var bytesPerLayer = texture.BytesPerLayer >> mipLevelStart;
        var bytesPerRow = texture.BytesPerRow >> mipLevelStart;

        var byteLength = texture.ByteLength;
        var byteOffset = (nuint)0;

        for (var index = 0; index < mipLevelStart; index++)
        {
            var oldByteLength = byteLength;
            byteLength >>= 1;
            byteOffset += oldByteLength - byteLength;
        }

        var pixelFormat = texture.PixelFormat;

        ResourceViewInfo.ByteLength = byteLength;
        ResourceViewInfo.ByteOffset = byteOffset;
        ResourceViewInfo.BytesPerElement = pixelFormat.GetSize();

        TextureViewInfo.BytesPerLayer = bytesPerLayer;
        TextureViewInfo.BytesPerRow = bytesPerRow;
        TextureViewInfo.MipLevelCount = createOptions.MipLevelCount;
        TextureViewInfo.MipLevelStart = createOptions.MipLevelStart;
        TextureViewInfo.PixelDepth = pixelDepth;
        TextureViewInfo.PixelFormat = pixelFormat;
        TextureViewInfo.PixelHeight = pixelHeight;
        TextureViewInfo.PixelWidth = pixelWidth;
        TextureViewInfo.Kind = texture.Kind;

        SetNameUnsafe(Name);

        VkImageView CreateVkImageView(in GraphicsTextureViewCreateOptions createOptions)
        {
            VkImageView vkImageView;

            var resource = Resource;
            var vkImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                pNext = null,
                flags = 0,
                image = resource.VkImage,
                viewType = GetVkImageViewType(resource.Kind),
                format = resource.PixelFormat.AsVkFormat(),
                components = new VkComponentMapping {
                    r = VK_COMPONENT_SWIZZLE_IDENTITY,
                    g = VK_COMPONENT_SWIZZLE_IDENTITY,
                    b = VK_COMPONENT_SWIZZLE_IDENTITY,
                    a = VK_COMPONENT_SWIZZLE_IDENTITY,
                },
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    baseMipLevel = createOptions.MipLevelStart,
                    levelCount = createOptions.MipLevelCount,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
            };

            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(Device.VkDevice, &vkImageViewCreateInfo, pAllocator: null, &vkImageView));
            return vkImageView;
        }

        static VkImageViewType GetVkImageViewType(GraphicsTextureKind textureKind)
        {
            VkImageViewType vkImageType = 0;

            switch (textureKind)
            {
                case GraphicsTextureKind.OneDimensional:
                {
                    vkImageType = VK_IMAGE_VIEW_TYPE_1D;
                    break;
                }

                case GraphicsTextureKind.TwoDimensional:
                {
                    vkImageType = VK_IMAGE_VIEW_TYPE_2D;
                    break;
                }

                case GraphicsTextureKind.ThreeDimensional:
                {
                    vkImageType = VK_IMAGE_VIEW_TYPE_3D;
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(textureKind);
                    break;
                }
            }

            return vkImageType;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsTextureView" /> class.</summary>
    ~VulkanGraphicsTextureView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new VulkanGraphicsTexture Resource => base.Resource.As<VulkanGraphicsTexture>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkImageView" /> for the buffer.</summary>
    public VkImageView VkImageView => _vkImageView;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        DisposeVkImageView(Device.VkDevice, _vkImageView);
        _vkImageView = VkImageView.NULL;

        _ = Resource.RemoveTextureView(this);

        static void DisposeVkImageView(VkDevice vkDevice, VkImageView vkImageView)
        {
            if (vkImageView != VkImageView.NULL)
            {
                vkDestroyImageView(vkDevice, vkImageView, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override unsafe byte* MapUnsafe()
    {
        return Resource.MapView(ByteOffset);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapForReadUnsafe()
    {
        return Resource.MapViewForRead(ByteOffset, ByteLength);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        return Resource.MapViewForRead(ByteOffset + byteStart, byteLength);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_IMAGE_VIEW, VkImageView, value);
    }

    /// <inheritdoc />
    protected override void UnmapUnsafe()
    {
        Resource.UnmapView();
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe()
    {
        Resource.UnmapViewAndWrite(ByteOffset, ByteLength);
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    {
        Resource.UnmapViewAndWrite(ByteOffset + byteStart, byteLength);
    }
}
