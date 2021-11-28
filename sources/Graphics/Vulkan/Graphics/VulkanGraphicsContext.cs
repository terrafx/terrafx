// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkAccessFlags;
using static TerraFX.Interop.Vulkan.VkCommandPoolCreateFlags;
using static TerraFX.Interop.Vulkan.VkComponentSwizzle;
using static TerraFX.Interop.Vulkan.VkDescriptorType;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkImageViewType;
using static TerraFX.Interop.Vulkan.VkIndexType;
using static TerraFX.Interop.Vulkan.VkPipelineBindPoint;
using static TerraFX.Interop.Vulkan.VkPipelineStageFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkSubpassContents;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsContext : GraphicsContext
{
    private const uint FrameInitializing = 2;

    private const uint FrameInitialized = 3;

    private const uint DrawingInitializing = 4;

    private const uint DrawingInitialized = 5;

    private readonly VulkanGraphicsFence _fence;

    private uint _framebufferIndex;
    private VulkanGraphicsSwapchain? _swapchain;

    private ValueLazy<VkCommandBuffer> _vulkanCommandBuffer;
    private ValueLazy<VkCommandPool> _vulkanCommandPool;

    private VolatileState _state;

    internal VulkanGraphicsContext(VulkanGraphicsDevice device)
        : base(device)
    {
        _fence = new VulkanGraphicsFence(device, isSignaled: true);

        _vulkanCommandBuffer = new ValueLazy<VkCommandBuffer>(CreateVulkanCommandBuffer);
        _vulkanCommandPool = new ValueLazy<VkCommandPool>(CreateVulkanCommandPool);

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsContext" /> class.</summary>
    ~VulkanGraphicsContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

    /// <inheritdoc />
    public override VulkanGraphicsFence Fence => _fence;

    /// <inheritdoc />
    public override VulkanGraphicsSwapchain? Swapchain => _swapchain;

    /// <summary>Gets the <see cref="VkCommandBuffer" /> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public VkCommandBuffer VulkanCommandBuffer => _vulkanCommandBuffer.Value;

    /// <summary>Gets the <see cref="VkCommandPool" /> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public VkCommandPool VulkanCommandPool => _vulkanCommandPool.Value;

    /// <inheritdoc />
    public override void BeginDrawing(uint framebufferIndex, ColorRgba backgroundColor)
    {
        _state.Transition(from: FrameInitialized, to: DrawingInitializing);
        Debug.Assert(Swapchain is not null);

        var clearValue = new VkClearValue();

        clearValue.color.float32[0] = backgroundColor.Red;
        clearValue.color.float32[1] = backgroundColor.Green;
        clearValue.color.float32[2] = backgroundColor.Blue;
        clearValue.color.float32[3] = backgroundColor.Alpha;

        var device = Device;
        var surface = Swapchain.Surface;

        var surfaceWidth = surface.Width;
        var surfaceHeight = surface.Height;

        var renderPassBeginInfo = new VkRenderPassBeginInfo {
            sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
            renderPass = device.VulkanRenderPass,
            framebuffer = Swapchain.VulkanFramebuffers[framebufferIndex],
            renderArea = new VkRect2D {
                extent = new VkExtent2D {
                    width = (uint)surface.Width,
                    height = (uint)surface.Height,
                },
            },
            clearValueCount = 1,
            pClearValues = &clearValue,
        };

        var commandBuffer = VulkanCommandBuffer;
        vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);

        var viewport = new VkViewport {
            x = 0,
            y = surface.Height,
            width = surface.Width,
            height = -surface.Height,
            minDepth = 0.0f,
            maxDepth = 1.0f,
        };
        vkCmdSetViewport(commandBuffer, firstViewport: 0, viewportCount: 1, &viewport);

        var scissorRect = new VkRect2D {
            extent = new VkExtent2D {
                width = (uint)surface.Width,
                height = (uint)surface.Height,
            },
        };
        vkCmdSetScissor(commandBuffer, firstScissor: 0, scissorCount: 1, &scissorRect);

        _framebufferIndex = framebufferIndex;
        _state.Transition(from: DrawingInitializing, to: DrawingInitialized);
    }

    /// <inheritdoc />
    public override void BeginFrame(GraphicsSwapchain swapchain)
        => BeginFrame((VulkanGraphicsSwapchain)swapchain);

    /// <inheritdoc cref="BeginFrame(GraphicsSwapchain)" />
    public void BeginFrame(VulkanGraphicsSwapchain swapchain)
    {
        ThrowIfNull(swapchain);

        _state.Transition(from: Initialized, to: FrameInitializing);
        _swapchain = swapchain;

        Fence.Reset();

        var commandBufferBeginInfo = new VkCommandBufferBeginInfo {
            sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        };

        ThrowExternalExceptionIfNotSuccess(vkBeginCommandBuffer(VulkanCommandBuffer, &commandBufferBeginInfo));

        _state.Transition(from: FrameInitializing, to: FrameInitialized);
    }

    /// <inheritdoc />
    public override void Copy(GraphicsBuffer destination, GraphicsBuffer source)
        => Copy((VulkanGraphicsBuffer)destination, (VulkanGraphicsBuffer)source);

    /// <inheritdoc />
    public override void Copy(GraphicsTexture destination, GraphicsBuffer source)
        => Copy((VulkanGraphicsTexture)destination, (VulkanGraphicsBuffer)source);

    /// <inheritdoc cref="Copy(GraphicsBuffer, GraphicsBuffer)" />
    public void Copy(VulkanGraphicsBuffer destination, VulkanGraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        if (_state < FrameInitialized)
        {
            ThrowInvalidOperationException("GraphicsContext.BeginFrame has not been called");
        }

        var vulkanBufferCopy = new VkBufferCopy {
            srcOffset = 0,
            dstOffset = 0,
            size = Math.Min(destination.Size, source.Size),
        };
        vkCmdCopyBuffer(VulkanCommandBuffer, source.VulkanBuffer, destination.VulkanBuffer, 1, &vulkanBufferCopy);
    }

    /// <inheritdoc cref="Copy(GraphicsTexture, GraphicsBuffer)" />
    public void Copy(VulkanGraphicsTexture destination, VulkanGraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        if (_state < FrameInitialized)
        {
            ThrowInvalidOperationException("GraphicsContext.BeginFrame has not been called");
        }

        var vulkanCommandBuffer = VulkanCommandBuffer;
        var vulkanImage = destination.VulkanImage;

        BeginCopy();

        var vulkanBufferImageCopy = new VkBufferImageCopy {
            imageSubresource = new VkImageSubresourceLayers {
                aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                layerCount = 1,
            },
            imageExtent = new VkExtent3D {
                width = destination.Width,
                height = destination.Height,
                depth = destination.Depth,
            },
        };

        vkCmdCopyBufferToImage(vulkanCommandBuffer, source.VulkanBuffer, vulkanImage, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &vulkanBufferImageCopy);

        EndCopy();

        void BeginCopy()
        {
            var vulkanImageMemoryBarrier = new VkImageMemoryBarrier {
                sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                oldLayout = VK_IMAGE_LAYOUT_UNDEFINED,
                newLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                image = vulkanImage,
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    levelCount = 1,
                    layerCount = 1,
                },
            };

            vkCmdPipelineBarrier(vulkanCommandBuffer, VK_PIPELINE_STAGE_HOST_BIT, VK_PIPELINE_STAGE_TRANSFER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vulkanImageMemoryBarrier);
        }

        void EndCopy()
        {
            var vulkanImageMemoryBarrier = new VkImageMemoryBarrier {
                sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                dstAccessMask = VK_ACCESS_SHADER_READ_BIT,
                oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                image = vulkanImage,
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    levelCount = 1,
                    layerCount = 1,
                },
            };

            vkCmdPipelineBarrier(vulkanCommandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vulkanImageMemoryBarrier);
        }
    }

    /// <inheritdoc />
    public override void Draw(GraphicsPrimitive primitive)
        => Draw((VulkanGraphicsPrimitive)primitive);

    /// <inheritdoc cref="Draw(GraphicsPrimitive)" />
    public void Draw(VulkanGraphicsPrimitive primitive)
    {
        ThrowIfNull(primitive);

        if (_state < DrawingInitialized)
        {
            ThrowInvalidOperationException("GraphicsContext.BeginDraw has not been called");
        }

        var vulkanCommandBuffer = VulkanCommandBuffer;
        var pipeline = primitive.Pipeline;
        var pipelineSignature = pipeline.Signature;
        var vulkanPipeline = pipeline.VulkanPipeline;

        vkCmdBindPipeline(vulkanCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, vulkanPipeline);

        ref readonly var vertexBufferView = ref primitive.VertexBufferView;
        var vertexBuffer = vertexBufferView.Resource as VulkanGraphicsBuffer;
        AssertNotNull(vertexBuffer);

        var vulkanVertexBuffer = vertexBuffer.VulkanBuffer;
        var vulkanVertexBufferOffset = (ulong)vertexBufferView.Offset;

        vkCmdBindVertexBuffers(vulkanCommandBuffer, firstBinding: 0, bindingCount: 1, &vulkanVertexBuffer, &vulkanVertexBufferOffset);

        var vulkanDescriptorSet = pipelineSignature.VulkanDescriptorSet;

        if (vulkanDescriptorSet != VkDescriptorSet.NULL)
        {
            var inputResourceRegions = primitive.InputResourceViews;
            var inputResourceRegionsLength = inputResourceRegions.Length;

            for (var index = 0; index < inputResourceRegionsLength; index++)
            {
                var inputResourceRegion = inputResourceRegions[index];

                VkWriteDescriptorSet writeDescriptorSet;

                if (inputResourceRegion.Resource is VulkanGraphicsBuffer vulkanGraphicsBuffer)
                {
                    var descriptorBufferInfo = new VkDescriptorBufferInfo {
                        buffer = vulkanGraphicsBuffer.VulkanBuffer,
                        offset = inputResourceRegion.Offset,
                        range = inputResourceRegion.Size,
                    };

                    writeDescriptorSet = new VkWriteDescriptorSet {
                        sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        dstSet = vulkanDescriptorSet,
                        dstBinding = unchecked((uint)index),
                        descriptorCount = 1,
                        descriptorType = VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                        pBufferInfo = &descriptorBufferInfo,
                    };
                }
                else if (inputResourceRegion.Resource is VulkanGraphicsTexture vulkanGraphicsTexture)
                {
                    var descriptorImageInfo = new VkDescriptorImageInfo {
                        sampler = vulkanGraphicsTexture.VulkanSampler,
                        imageView = vulkanGraphicsTexture.VulkanImageView,
                        imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                    };

                    writeDescriptorSet = new VkWriteDescriptorSet {
                        sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        dstSet = vulkanDescriptorSet,
                        dstBinding = unchecked((uint)index),
                        descriptorCount = 1,
                        descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                        pImageInfo = &descriptorImageInfo,
                    };
                }

                vkUpdateDescriptorSets(Device.VulkanDevice, 1, &writeDescriptorSet, 0, pDescriptorCopies: null);
            }

            vkCmdBindDescriptorSets(vulkanCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineSignature.VulkanPipelineLayout, firstSet: 0, 1, &vulkanDescriptorSet, dynamicOffsetCount: 0, pDynamicOffsets: null);
        }

        ref readonly var indexBufferView = ref primitive.IndexBufferView;

        if (indexBufferView.Resource is VulkanGraphicsBuffer indexBuffer)
        {
            var indexBufferStride = indexBufferView.Stride;
            var indexType = VK_INDEX_TYPE_UINT16;

            if (indexBufferStride != 2)
            {
                Assert(AssertionsEnabled && (indexBufferStride == 4));
                indexType = VK_INDEX_TYPE_UINT32;
            }
            vkCmdBindIndexBuffer(vulkanCommandBuffer, indexBuffer.VulkanBuffer, indexBufferView.Offset, indexType);

            vkCmdDrawIndexed(vulkanCommandBuffer, indexCount: (uint)(indexBufferView.Size / indexBufferStride), instanceCount: 1, firstIndex: 0, vertexOffset: 0, firstInstance: 0);
        }
        else
        {
            vkCmdDraw(vulkanCommandBuffer, vertexCount: (uint)(vertexBufferView.Size / vertexBufferView.Stride), instanceCount: 1, firstVertex: 0, firstInstance: 0);
        }
    }

    /// <inheritdoc />
    public override void EndDrawing()
    {
        _state.Transition(from: DrawingInitialized, to: DrawingInitializing);
        vkCmdEndRenderPass(VulkanCommandBuffer);
        _state.Transition(from: DrawingInitializing, to: FrameInitialized);
    }

    /// <inheritdoc />
    public override void EndFrame()
    {
        _state.Transition(from: FrameInitialized, to: FrameInitializing);

        var commandBuffer = VulkanCommandBuffer;
        ThrowExternalExceptionIfNotSuccess(vkEndCommandBuffer(commandBuffer));

        var submitInfo = new VkSubmitInfo {
            sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
            commandBufferCount = 1,
            pCommandBuffers = &commandBuffer,
        };

        var fence = Fence;
        ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(Device.VulkanCommandQueue, submitCount: 1, &submitInfo, fence.VulkanFence));
        fence.Wait();

        _swapchain = null;
        _state.Transition(from: FrameInitializing, to: Initialized);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _vulkanCommandBuffer.Dispose(DisposeVulkanCommandBuffer);
            _vulkanCommandPool.Dispose(DisposeVulkanCommandPool);
            _fence?.Dispose();
        }

        _state.EndDispose();
    }

    private VkCommandBuffer CreateVulkanCommandBuffer()
    {
        VkCommandBuffer vulkanCommandBuffer;

        var commandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
            sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            commandPool = VulkanCommandPool,
            commandBufferCount = 1,
        };
        ThrowExternalExceptionIfNotSuccess(vkAllocateCommandBuffers(Device.VulkanDevice, &commandBufferAllocateInfo, &vulkanCommandBuffer));

        return vulkanCommandBuffer;
    }

    private VkCommandPool CreateVulkanCommandPool()
    {
        VkCommandPool vulkanCommandPool;

        var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
            sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
            flags = VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
            queueFamilyIndex = Device.VulkanCommandQueueFamilyIndex,
        };
        ThrowExternalExceptionIfNotSuccess(vkCreateCommandPool(Device.VulkanDevice, &commandPoolCreateInfo, pAllocator: null, &vulkanCommandPool));

        return vulkanCommandPool;
    }

    private void DisposeVulkanCommandBuffer(VkCommandBuffer vulkanCommandBuffer)
    {
        AssertDisposing(_state);

        if (vulkanCommandBuffer != VkCommandBuffer.NULL)
        {
            vkFreeCommandBuffers(Device.VulkanDevice, VulkanCommandPool, 1, &vulkanCommandBuffer);
        }
    }

    private void DisposeVulkanCommandPool(VkCommandPool vulkanCommandPool)
    {
        AssertDisposing(_state);

        if (vulkanCommandPool != VkCommandPool.NULL)
        {
            vkDestroyCommandPool(Device.VulkanDevice, vulkanCommandPool, pAllocator: null);
        }
    }
}
