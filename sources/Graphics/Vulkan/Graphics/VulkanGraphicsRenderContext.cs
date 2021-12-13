// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using TerraFX.Interop.Vulkan;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkAccessFlags;
using static TerraFX.Interop.Vulkan.VkCommandPoolCreateFlags;
using static TerraFX.Interop.Vulkan.VkDescriptorType;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageLayout;
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
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsRenderContext : GraphicsRenderContext
{
    private const uint DrawingInitializing = 2;

    private const uint DrawingInitialized = 3;

    private readonly VulkanGraphicsFence _fence;
    private readonly VkCommandBuffer _vkCommandBuffer;
    private readonly VkCommandPool _vkCommandPool;

    private uint _framebufferIndex;
    private VulkanGraphicsSwapchain? _swapchain;

    private VolatileState _state;

    internal VulkanGraphicsRenderContext(VulkanGraphicsDevice device)
        : base(device)
    {
        var vkCommandPool = CreateVkCommandPool(device);
        _vkCommandPool = vkCommandPool;

        _vkCommandBuffer = CreateVkCommandBuffer(device, vkCommandPool);
        _fence = device.CreateFence(isSignalled: true);

        _ = _state.Transition(to: Initialized);

        static VkCommandBuffer CreateVkCommandBuffer(VulkanGraphicsDevice device, VkCommandPool vkCommandPool)
        {
            VkCommandBuffer vkCommandBuffer;

            var vkCommandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                commandPool = vkCommandPool,
                commandBufferCount = 1,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateCommandBuffers(device.VkDevice, &vkCommandBufferAllocateInfo, &vkCommandBuffer));

            return vkCommandBuffer;
        }

        static VkCommandPool CreateVkCommandPool(VulkanGraphicsDevice device)
        {
            VkCommandPool vkCommandPool;

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                flags = VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
                queueFamilyIndex = device.VkCommandQueueFamilyIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateCommandPool(device.VkDevice, &commandPoolCreateInfo, pAllocator: null, &vkCommandPool));

            return vkCommandPool;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsRenderContext" /> class.</summary>
    ~VulkanGraphicsRenderContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc />
    public override VulkanGraphicsFence Fence => _fence;

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    public override VulkanGraphicsSwapchain? Swapchain => _swapchain;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandBuffer" /> used by the context.</summary>
    public VkCommandBuffer VkCommandBuffer
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkCommandBuffer;
        }
    }

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandPool" /> used by the context.</summary>
    public VkCommandPool VkCommandPool
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkCommandPool;
        }
    }

    /// <inheritdoc />
    public override void BeginDrawing(uint framebufferIndex, ColorRgba backgroundColor)
    {
        _state.Transition(from: Initialized, to: DrawingInitializing);
        Debug.Assert(Swapchain is not null);

        var vkClearValue = new VkClearValue();

        vkClearValue.color.float32[0] = backgroundColor.Red;
        vkClearValue.color.float32[1] = backgroundColor.Green;
        vkClearValue.color.float32[2] = backgroundColor.Blue;
        vkClearValue.color.float32[3] = backgroundColor.Alpha;

        var device = Device;
        var surface = Swapchain.Surface;

        var surfaceWidth = surface.Width;
        var surfaceHeight = surface.Height;

        var vkRenderPassBeginInfo = new VkRenderPassBeginInfo {
            sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
            renderPass = device.VkRenderPass,
            framebuffer = Swapchain.VkFramebuffers[framebufferIndex],
            renderArea = new VkRect2D {
                extent = new VkExtent2D {
                    width = (uint)surface.Width,
                    height = (uint)surface.Height,
                },
            },
            clearValueCount = 1,
            pClearValues = &vkClearValue,
        };

        var vkCommandBuffer = VkCommandBuffer;
        vkCmdBeginRenderPass(vkCommandBuffer, &vkRenderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);

        _framebufferIndex = framebufferIndex;
        _state.Transition(from: DrawingInitializing, to: DrawingInitialized);
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

        var vkBufferCopy = new VkBufferCopy {
            srcOffset = 0,
            dstOffset = 0,
            size = Math.Min(destination.Size, source.Size),
        };
        vkCmdCopyBuffer(VkCommandBuffer, source.VkBuffer, destination.VkBuffer, 1, &vkBufferCopy);
    }

    /// <inheritdoc cref="Copy(GraphicsTexture, GraphicsBuffer)" />
    public void Copy(VulkanGraphicsTexture destination, VulkanGraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        var vkCommandBuffer = VkCommandBuffer;
        var vkImage = destination.VkImage;

        BeginCopy();

        var vkBufferImageCopy = new VkBufferImageCopy {
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

        vkCmdCopyBufferToImage(vkCommandBuffer, source.VkBuffer, vkImage, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &vkBufferImageCopy);

        EndCopy();

        void BeginCopy()
        {
            var vkImageMemoryBarrier = new VkImageMemoryBarrier {
                sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                oldLayout = VK_IMAGE_LAYOUT_UNDEFINED,
                newLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                image = vkImage,
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    levelCount = 1,
                    layerCount = 1,
                },
            };

            vkCmdPipelineBarrier(vkCommandBuffer, VK_PIPELINE_STAGE_HOST_BIT, VK_PIPELINE_STAGE_TRANSFER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vkImageMemoryBarrier);
        }

        void EndCopy()
        {
            var vkImageMemoryBarrier = new VkImageMemoryBarrier {
                sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                dstAccessMask = VK_ACCESS_SHADER_READ_BIT,
                oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                image = vkImage,
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    levelCount = 1,
                    layerCount = 1,
                },
            };

            vkCmdPipelineBarrier(vkCommandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vkImageMemoryBarrier);
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

        var vkCommandBuffer = VkCommandBuffer;
        var pipeline = primitive.Pipeline;
        var pipelineSignature = pipeline.Signature;
        var vkPipeline = pipeline.VkPipeline;

        vkCmdBindPipeline(vkCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, vkPipeline);

        ref readonly var vertexBufferView = ref primitive.VertexBufferView;
        var vertexBuffer = vertexBufferView.Resource.As<VulkanGraphicsBuffer>();
        AssertNotNull(vertexBuffer);

        var vkVertexBuffer = vertexBuffer.VkBuffer;
        var vkVertexBufferOffset = (ulong)vertexBufferView.Offset;

        vkCmdBindVertexBuffers(vkCommandBuffer, firstBinding: 0, bindingCount: 1, &vkVertexBuffer, &vkVertexBufferOffset);

        var vkDescriptorSet = pipelineSignature.VkDescriptorSet;

        if (vkDescriptorSet != VkDescriptorSet.NULL)
        {
            var inputResourceViews = primitive.InputResourceViews;

            for (var index = 0; index < inputResourceViews.Length; index++)
            {
                ref readonly var inputResourceView = ref inputResourceViews[index];

                VkWriteDescriptorSet vkWriteDescriptorSet;

                if (inputResourceView.Resource is VulkanGraphicsBuffer vulkanGraphicsBuffer)
                {
                    var vkDescriptorBufferInfo = new VkDescriptorBufferInfo {
                        buffer = vulkanGraphicsBuffer.VkBuffer,
                        offset = inputResourceView.Offset,
                        range = inputResourceView.Size,
                    };

                    vkWriteDescriptorSet = new VkWriteDescriptorSet {
                        sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        dstSet = vkDescriptorSet,
                        dstBinding = unchecked((uint)index),
                        descriptorCount = 1,
                        descriptorType = VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                        pBufferInfo = &vkDescriptorBufferInfo,
                    };
                }
                else if (inputResourceView.Resource is VulkanGraphicsTexture vulkanGraphicsTexture)
                {
                    var vkDescriptorImageInfo = new VkDescriptorImageInfo {
                        sampler = vulkanGraphicsTexture.VkSampler,
                        imageView = vulkanGraphicsTexture.VkImageView,
                        imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                    };

                    vkWriteDescriptorSet = new VkWriteDescriptorSet {
                        sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                        dstSet = vkDescriptorSet,
                        dstBinding = unchecked((uint)index),
                        descriptorCount = 1,
                        descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                        pImageInfo = &vkDescriptorImageInfo,
                    };
                }

                vkUpdateDescriptorSets(Device.VkDevice, 1, &vkWriteDescriptorSet, 0, pDescriptorCopies: null);
            }

            vkCmdBindDescriptorSets(vkCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineSignature.VkPipelineLayout, firstSet: 0, 1, &vkDescriptorSet, dynamicOffsetCount: 0, pDynamicOffsets: null);
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
            vkCmdBindIndexBuffer(vkCommandBuffer, indexBuffer.VkBuffer, indexBufferView.Offset, indexType);

            vkCmdDrawIndexed(vkCommandBuffer, indexCount: indexBufferView.Size / indexBufferStride, instanceCount: 1, firstIndex: 0, vertexOffset: 0, firstInstance: 0);
        }
        else
        {
            vkCmdDraw(vkCommandBuffer, vertexCount: vertexBufferView.Size / vertexBufferView.Stride, instanceCount: 1, firstVertex: 0, firstInstance: 0);
        }
    }

    /// <inheritdoc />
    public override void EndDrawing()
    {
        _state.Transition(from: DrawingInitialized, to: DrawingInitializing);
        vkCmdEndRenderPass(VkCommandBuffer);
        _state.Transition(from: DrawingInitializing, to: Initialized);
    }

    /// <inheritdoc />
    public override void Flush()
    {
        var vkCommandBuffer = VkCommandBuffer;
        ThrowExternalExceptionIfNotSuccess(vkEndCommandBuffer(vkCommandBuffer));

        var vkSubmitInfo = new VkSubmitInfo {
            sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
            commandBufferCount = 1,
            pCommandBuffers = &vkCommandBuffer,
        };

        var fence = Fence;
        ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(Device.VkCommandQueue, submitCount: 1, &vkSubmitInfo, fence.VkFence));
        fence.Wait();
    }

    /// <inheritdoc />
    public override void Reset()
    {
        _swapchain = null;
        Fence.Reset();

        var vkCommandBufferBeginInfo = new VkCommandBufferBeginInfo {
            sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        };
        ThrowExternalExceptionIfNotSuccess(vkBeginCommandBuffer(VkCommandBuffer, &vkCommandBufferBeginInfo));
    }

    /// <inheritdoc />
    public override void SetScissor(BoundingRectangle scissor)
    {
        var location = scissor.Location;
        var size = scissor.Size;

        var vkRect2D = new VkRect2D {
            offset = new VkOffset2D {
                x = (int)location.X,
                y = (int)location.Y,
            },
            extent = new VkExtent2D {
                width = (uint)size.X,
                height = (uint)size.Y
            },
        };
        vkCmdSetScissor(VkCommandBuffer, firstScissor: 0, scissorCount: 1, &vkRect2D);
    }

    /// <inheritdoc />
    public override void SetScissors(ReadOnlySpan<BoundingRectangle> scissors)
    {
        var count = (uint)scissors.Length;
        var vkRect2Ds = AllocateArray<VkRect2D>(count);

        for (var i = 0u; i < count; i++)
        {
            ref readonly var scissor = ref scissors[(int)i];

            var location = scissor.Location;
            var size = scissor.Size;

            vkRect2Ds[i] = new VkRect2D {
                offset = new VkOffset2D {
                    x = (int)location.X,
                    y = (int)location.Y,
                },
                extent = new VkExtent2D {
                    width = (uint)size.X,
                    height = (uint)size.Y
                },
            };
        }
        vkCmdSetScissor(VkCommandBuffer, firstScissor: 0, count, vkRect2Ds);
    }

    /// <inheritdoc />
    public override void SetSwapchain(GraphicsSwapchain swapchain)
        => SetSwapchain((VulkanGraphicsSwapchain)swapchain);

    /// <inheritdoc cref="SetSwapchain(GraphicsSwapchain)" />
    public void SetSwapchain(VulkanGraphicsSwapchain swapchain)
    {
        ThrowIfNull(swapchain);

        if (swapchain.Device != Device)
        {
            ThrowForInvalidParent(swapchain.Device);
        }

        _swapchain = swapchain;
    }

    /// <inheritdoc />
    public override void SetViewport(BoundingBox viewport)
    {
        var location = viewport.Location;
        var size = viewport.Size;

        var vkViewport = new VkViewport {
            x = location.X,
            y = location.Y + size.Y,
            width = size.X,
            height = -size.Y,
            minDepth = location.Z,
            maxDepth = size.Z,
        };
        vkCmdSetViewport(VkCommandBuffer, firstViewport: 0, viewportCount: 1, &vkViewport);
    }

    /// <inheritdoc />
    public override void SetViewports(ReadOnlySpan<BoundingBox> viewports)
    {
        var count = (uint)viewports.Length;
        var vkViewports = AllocateArray<VkViewport>(count);

        for (var i = 0u; i < count; i++)
        {
            ref readonly var viewport = ref viewports[(int)i];

            var location = viewport.Location;
            var size = viewport.Size;

            vkViewports[i] = new VkViewport {
                x = location.X,
                y = location.Y + size.Y,
                width = size.X,
                height = -size.Y,
                minDepth = location.Z,
                maxDepth = size.Z,
            };
        }
        vkCmdSetViewport(VkCommandBuffer, firstViewport: 0, count, vkViewports);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            var vkDevice = Device.VkDevice;
            var vkCommandPool = _vkCommandPool;

            DisposeVkCommandBuffer(vkDevice, vkCommandPool, _vkCommandBuffer);
            DisposeVkCommandPool(vkDevice, vkCommandPool);

            if (isDisposing)
            {
                _fence?.Dispose();
            }
        }

        _state.EndDispose();

        static void DisposeVkCommandBuffer(VkDevice vkDevice, VkCommandPool vkCommandPool, VkCommandBuffer vkCommandBuffer)
        {
            if (vkCommandBuffer != VkCommandBuffer.NULL)
            {
                vkFreeCommandBuffers(vkDevice, vkCommandPool, 1, &vkCommandBuffer);
            }
        }

        static void DisposeVkCommandPool(VkDevice vkDevice, VkCommandPool vkCommandPool)
        {
            if (vkCommandPool != VkCommandPool.NULL)
            {
                vkDestroyCommandPool(vkDevice, vkCommandPool, pAllocator: null);
            }
        }
    }
}
