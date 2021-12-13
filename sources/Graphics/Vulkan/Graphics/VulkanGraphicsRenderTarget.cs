// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkComponentSwizzle;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageViewType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsRenderTarget : GraphicsRenderTarget
{
    private readonly VkFramebuffer _vkFramebuffer;
    private readonly VkImageView _vkFramebufferImageView;

    private VolatileState _state;

    internal VulkanGraphicsRenderTarget(VulkanGraphicsSwapchain swapchain, uint index)
        : base(swapchain, index)
    {
        var vkFramebufferImageView = CreateVkFramebufferImageView(swapchain, index);
        _vkFramebufferImageView = vkFramebufferImageView;

        var vkFramebuffer = CreateVkFramebuffer(swapchain, vkFramebufferImageView);
        _vkFramebuffer = vkFramebuffer;

        static VkFramebuffer CreateVkFramebuffer(VulkanGraphicsSwapchain swapchain, VkImageView vkImageView)
        {
            var surface = swapchain.Surface;

            var vkFramebufferCreateInfo = new VkFramebufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                renderPass = swapchain.RenderPass.VkRenderPass,
                attachmentCount = 1,
                pAttachments = &vkImageView,
                width = (uint)surface.Width,
                height = (uint)surface.Height,
                layers = 1,
            };

            VkFramebuffer vkFramebuffer;
            ThrowExternalExceptionIfNotSuccess(vkCreateFramebuffer(swapchain.Device.VkDevice, &vkFramebufferCreateInfo, pAllocator: null, &vkFramebuffer));
            return vkFramebuffer;
        }

        static VkImageView CreateVkFramebufferImageView(VulkanGraphicsSwapchain swapchain, uint index)
        {
            var vkImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                image = swapchain.VkSwapchainImages[index],
                viewType = VK_IMAGE_VIEW_TYPE_2D,
                format = swapchain.RenderTargetFormat.AsVkFormat(),
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

            VkImageView vkImageView;
            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(swapchain.Device.VkDevice, &vkImageViewCreateInfo, pAllocator: null, &vkImageView));
            return vkImageView;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsRenderTarget" /> class.</summary>
    ~VulkanGraphicsRenderTarget() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsSwapchainObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsSwapchainObject.RenderPass" />
    public new VulkanGraphicsRenderPass RenderPass => base.RenderPass.As<VulkanGraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Swapchain" />
    public new VulkanGraphicsSwapchain Swapchain => base.Swapchain.As<VulkanGraphicsSwapchain>();

    /// <summary>Gets the <see cref="VkFramebuffer"/> for the render target.</summary>
    public VkFramebuffer VkFramebuffer
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkFramebuffer;
        }
    }

    /// <summary>Gets the <see cref="VkImageView" /> used by <see cref="VkFramebuffer" />.</summary>
    public VkImageView VkFramebufferImageView
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkFramebufferImageView;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            var vkDevice = Device.VkDevice;

            CleanupVkFramebuffer(vkDevice, _vkFramebuffer);
            CleanupVkFramebufferImageView(vkDevice, _vkFramebufferImageView);
        }

        _state.EndDispose();

        static void CleanupVkFramebuffer(VkDevice vkDevice, VkFramebuffer vkFramebuffer)
        {
            if (vkFramebuffer != VkFramebuffer.NULL)
            {
                vkDestroyFramebuffer(vkDevice, vkFramebuffer, pAllocator: null);
            }
        }

        static void CleanupVkFramebufferImageView(VkDevice vkDevice, VkImageView vkFramebufferImageView)
        {
            if (vkFramebufferImageView != VkImageView.NULL)
            {
                vkDestroyImageView(vkDevice, vkFramebufferImageView, pAllocator: null);
            }
        }
    }
}
