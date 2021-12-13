// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkAttachmentLoadOp;
using static TerraFX.Interop.Vulkan.VkAttachmentStoreOp;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkPipelineBindPoint;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsRenderPass : GraphicsRenderPass
{
    private readonly VulkanGraphicsSwapchain _swapchain;
    private readonly VkRenderPass _vkRenderPass;

    private VolatileState _state;

    internal VulkanGraphicsRenderPass(VulkanGraphicsDevice device, IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0)
        : base(device, surface, renderTargetFormat)
    {
        _vkRenderPass = CreateVkRenderPass(device, renderTargetFormat);
        _swapchain = new VulkanGraphicsSwapchain(this, surface, renderTargetFormat, minimumRenderTargetCount);

        _ = _state.Transition(to: Initialized);

        static VkRenderPass CreateVkRenderPass(VulkanGraphicsDevice device, GraphicsFormat renderTargetFormat)
        {
            VkRenderPass vkRenderPass;

            var vkAttachmentDescription = new VkAttachmentDescription {
                format = renderTargetFormat.AsVkFormat(),
                samples = VK_SAMPLE_COUNT_1_BIT,
                loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
                stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
                finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
            };

            var vkColorAttachmentReference = new VkAttachmentReference {
                layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
            };

            var vkSubpassDescription = new VkSubpassDescription {
                pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
                colorAttachmentCount = 1,
                pColorAttachments = &vkColorAttachmentReference,
            };

            var vkRenderPassCreateInfo = new VkRenderPassCreateInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                attachmentCount = 1,
                pAttachments = &vkAttachmentDescription,
                subpassCount = 1,
                pSubpasses = &vkSubpassDescription,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateRenderPass(device.VkDevice, &vkRenderPassCreateInfo, pAllocator: null, &vkRenderPass));

            return vkRenderPass;
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    public override VulkanGraphicsSwapchain Swapchain => _swapchain;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkRenderPass" /> used by the device.</summary>
    public VkRenderPass VkRenderPass
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkRenderPass.Value;
        }
    }

    /// <inheritdoc />
    public override VulkanGraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null)
        => CreatePipeline((VulkanGraphicsPipelineSignature)signature, (VulkanGraphicsShader?)vertexShader, (VulkanGraphicsShader?)pixelShader);

    /// <inheritdoc cref="CreatePipeline(GraphicsPipelineSignature, GraphicsShader?, GraphicsShader?)" />
    public VulkanGraphicsPipeline CreatePipeline(VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader = null, VulkanGraphicsShader? pixelShader = null)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsPipeline(this, signature, vertexShader, pixelShader);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                _swapchain?.Dispose();
            }

            DisposeVkRenderPass(Device.VkDevice, _vkRenderPass);
        }

        _state.EndDispose();

        static void DisposeVkRenderPass(VkDevice vkDevice, VkRenderPass vkRenderPass)
        {
            if (vkRenderPass != VkRenderPass.NULL)
            {
                vkDestroyRenderPass(vkDevice, vkRenderPass, pAllocator: null);
            }
        }
    }
}
