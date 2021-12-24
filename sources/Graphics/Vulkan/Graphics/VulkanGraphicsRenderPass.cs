// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkAttachmentLoadOp;
using static TerraFX.Interop.Vulkan.VkAttachmentStoreOp;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPipelineBindPoint;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsRenderPass : GraphicsRenderPass
{
    private VkRenderPass _vkRenderPass;

    internal VulkanGraphicsRenderPass(VulkanGraphicsDevice device, in GraphicsRenderPassCreateOptions createOptions) : base(device)
    {
        device.AddRenderPass(this);

        RenderPassInfo.RenderTargetFormat = createOptions.RenderTargetFormat;
        RenderPassInfo.Surface = createOptions.Surface;

        _vkRenderPass = CreateVkRenderPass(in createOptions);

        var swapchainCreateOptions = new VulkanGraphicsSwapchainCreateOptions {
            MinimumRenderTargetCount = createOptions.MinimumRenderTargetCount,
            RenderTargetFormat = createOptions.RenderTargetFormat,
            Surface = createOptions.Surface,
        };
        RenderPassInfo.Swapchain = new VulkanGraphicsSwapchain(this, in swapchainCreateOptions);

        VkRenderPass CreateVkRenderPass(in GraphicsRenderPassCreateOptions createOptions)
        {
            VkRenderPass vkRenderPass;

            var vkAttachmentDescription = new VkAttachmentDescription {
                flags = 0,
                format = createOptions.RenderTargetFormat.AsVkFormat(),
                samples = VK_SAMPLE_COUNT_1_BIT,
                loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VK_ATTACHMENT_STORE_OP_STORE,
                stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
                initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
                finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
            };

            var vkColorAttachmentReference = new VkAttachmentReference {
                attachment = 0,
                layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
            };

            var vkSubpassDescription = new VkSubpassDescription {
                flags = 0,
                pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
                inputAttachmentCount = 0,
                pInputAttachments = null,
                colorAttachmentCount = 1,
                pColorAttachments = &vkColorAttachmentReference,
                pResolveAttachments = null,
                pDepthStencilAttachment = null,
                preserveAttachmentCount = 0,
                pPreserveAttachments = null,
            };

            var vkRenderPassCreateInfo = new VkRenderPassCreateInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                pNext = null,
                flags = 0,
                attachmentCount = 1,
                pAttachments = &vkAttachmentDescription,
                subpassCount = 1,
                pSubpasses = &vkSubpassDescription,
                dependencyCount = 0,
                pDependencies = null,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateRenderPass(Device.VkDevice, &vkRenderPassCreateInfo, pAllocator: null, &vkRenderPass));

            return vkRenderPass;
        }
    }

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc cref="GraphicsSwapchainObject.Swapchain" />
    public new VulkanGraphicsSwapchain Swapchain => base.Swapchain.As<VulkanGraphicsSwapchain>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkRenderPass" /> used by the device.</summary>
    public VkRenderPass VkRenderPass => _vkRenderPass.Value;

    /// <inheritdoc />
    protected override VulkanGraphicsPipeline CreatePipelineUnsafe(in GraphicsPipelineCreateOptions createOptions)
    {
        return new VulkanGraphicsPipeline(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            RenderPassInfo.Swapchain.Dispose();
            RenderPassInfo.Swapchain = null!;
        }

        DisposeVkRenderPass(Device.VkDevice, _vkRenderPass);
        _vkRenderPass = VkRenderPass.NULL;

        _ = Device.RemoveRenderPass(this);

        static void DisposeVkRenderPass(VkDevice vkDevice, VkRenderPass vkRenderPass)
        {
            if (vkRenderPass != VkRenderPass.NULL)
            {
                vkDestroyRenderPass(vkDevice, vkRenderPass, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_RENDER_PASS, VkRenderPass, value);
    }
}
