// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkComponentSwizzle;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageViewType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsTextureView : GraphicsTextureView
{
    private readonly VkImageView _vkImageView;

    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsTextureView(VulkanGraphicsTexture texture, in GraphicsTextureViewInfo textureViewInfo)
        : base(texture, in textureViewInfo)
    {
        texture.AddView(this);
        _vkImageView = CreateVkImageView(Device, in textureViewInfo, texture.VkImage);

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsTextureView);

        static VkImageView CreateVkImageView(VulkanGraphicsDevice device, in GraphicsTextureViewInfo textureViewInfo, VkImage vkImage)
        {
            VkImageView vkImageView;

            var vkImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                image = vkImage,
                viewType = GetVkImageViewType(textureViewInfo.Kind),
                format = textureViewInfo.Format.AsVkFormat(),
                components = new VkComponentMapping {
                    r = VK_COMPONENT_SWIZZLE_IDENTITY,
                    g = VK_COMPONENT_SWIZZLE_IDENTITY,
                    b = VK_COMPONENT_SWIZZLE_IDENTITY,
                    a = VK_COMPONENT_SWIZZLE_IDENTITY,
                },
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    baseMipLevel = textureViewInfo.MipLevelIndex,
                    levelCount = textureViewInfo.MipLevelCount,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(device.VkDevice, &vkImageViewCreateInfo, pAllocator: null, &vkImageView));

            return vkImageView;
        }

        static VkImageViewType GetVkImageViewType(GraphicsTextureKind kind)
        {
            VkImageViewType vkImageType = 0;

            switch (kind)
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
                    ThrowForInvalidKind(kind);
                    break;
                }
            }

            return vkImageType;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsTextureView" /> class.</summary>
    ~VulkanGraphicsTextureView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsResourceView.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsResourceView.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <summary>Gets or sets the name for the buffer view.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = Device.UpdateName(VK_OBJECT_TYPE_IMAGE_VIEW, VkImageView, value);
        }
    }

    /// <inheritdoc cref="GraphicsResourceView.Resource" />
    public new VulkanGraphicsTexture Resource => base.Resource.As<VulkanGraphicsTexture>();

    /// <inheritdoc cref="GraphicsResourceView.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkImageView" /> for the buffer.</summary>
    public VkImageView VkImageView
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkImageView;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                _ = Resource.RemoveView(this);
            }
            DisposeVkImageView(Device.VkDevice, _vkImageView);
        }

        _state.EndDispose();

        static void DisposeVkImageView(VkDevice vkDevice, VkImageView vkImageView)
        {
            if (vkImageView != VkImageView.NULL)
            {
                vkDestroyImageView(vkDevice, vkImageView, pAllocator: null);
            }
        }
    }
}
