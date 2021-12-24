// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Numerics;
using static TerraFX.Interop.Vulkan.VkCommandBufferLevel;
using static TerraFX.Interop.Vulkan.VkDescriptorType;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkIndexType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPipelineBindPoint;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkSubpassContents;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsRenderContext : GraphicsRenderContext
{
    private VkCommandBuffer _vkCommandBuffer;
    private VkCommandPool _vkCommandPool;

    private readonly uint _vkMaxVertexInputBindings;

    private VulkanGraphicsRenderPass? _renderPass;

    internal VulkanGraphicsRenderContext(VulkanGraphicsRenderCommandQueue renderCommandQueue) : base(renderCommandQueue)
    {
        // No need for a ContextPool.AddComputeContext(this) as it will be done by the underlying pool

        ContextInfo.Fence = Device.CreateFence(isSignalled: true);

        _vkCommandPool = CreateVkCommandPool();
        _vkCommandBuffer = CreateVkCommandBuffer();

        _vkMaxVertexInputBindings = Adapter.VkPhysicalDeviceProperties.limits.maxVertexInputBindings;

        SetNameUnsafe(Name);

        VkCommandBuffer CreateVkCommandBuffer()
        {
            VkCommandBuffer vkCommandBuffer;

            var vkCommandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                pNext = null,
                commandPool = _vkCommandPool,
                level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandBufferCount = 1,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateCommandBuffers(Device.VkDevice, &vkCommandBufferAllocateInfo, &vkCommandBuffer));

            return vkCommandBuffer;
        }

        VkCommandPool CreateVkCommandPool()
        {
            VkCommandPool vkCommandPool;

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                pNext = null,
                flags = 0,
                queueFamilyIndex = CommandQueue.VkQueueFamilyIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateCommandPool(Device.VkDevice, &commandPoolCreateInfo, pAllocator: null, &vkCommandPool));

            return vkCommandPool;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsRenderContext" /> class.</summary>
    ~VulkanGraphicsRenderContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new VulkanGraphicsRenderCommandQueue CommandQueue => base.CommandQueue.As<VulkanGraphicsRenderCommandQueue>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsContext{TGraphicsContext}.Fence" />
    public new VulkanGraphicsFence Fence => base.Fence.As<VulkanGraphicsFence>();

    /// <inheritdoc />
    public override uint MaxBoundVertexBufferViewCount => _vkMaxVertexInputBindings;

    /// <inheritdoc />
    public override VulkanGraphicsRenderPass? RenderPass => _renderPass;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandBuffer" /> used by the context.</summary>
    public VkCommandBuffer VkCommandBuffer => _vkCommandBuffer;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandPool" /> used by the context.</summary>
    public VkCommandPool VkCommandPool => _vkCommandPool;

    /// <inheritdoc />
    public override void BeginRenderPass(GraphicsRenderPass renderPass, ColorRgba renderTargetClearColor)
        => BeginRenderPass((VulkanGraphicsRenderPass) renderPass, renderTargetClearColor);

    /// <inheritdoc cref="BeginRenderPass(GraphicsRenderPass, ColorRgba)" />
    public void BeginRenderPass(VulkanGraphicsRenderPass renderPass, ColorRgba renderTargetClearColor)
    {
        ThrowIfNull(renderPass);

        if (Interlocked.CompareExchange(ref _renderPass, renderPass, null) is not null)
        {
            ThrowForInvalidState(nameof(RenderPass));
        }

        var swapchain = renderPass.Swapchain;

        ref readonly var vkSurfaceCapabilities = ref swapchain.VkSurfaceCapabilities;
        var renderTarget = swapchain.CurrentRenderTarget;

        var vkRenderPassBeginInfo = new VkRenderPassBeginInfo {
            sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
            renderPass = renderPass.VkRenderPass,
            framebuffer = renderTarget.VkFramebuffer,
            renderArea = new VkRect2D {
                extent = new VkExtent2D {
                    width = vkSurfaceCapabilities.currentExtent.width,
                    height = vkSurfaceCapabilities.currentExtent.height,
                },
            },
            clearValueCount = 1,
            pClearValues = (VkClearValue*)&renderTargetClearColor,
        };
        vkCmdBeginRenderPass(VkCommandBuffer, &vkRenderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);
    }

    /// <inheritdoc />
    public override void BindIndexBufferView(GraphicsBufferView indexBufferView)
        => BindIndexBufferView((VulkanGraphicsBufferView)indexBufferView);

    /// <inheritdoc cref="BindIndexBufferView(GraphicsBufferView)" />
    public void BindIndexBufferView(VulkanGraphicsBufferView indexBufferView)
    {
        ThrowIfNull(indexBufferView);

        var indexType = (indexBufferView.BytesPerElement == 2) ? VK_INDEX_TYPE_UINT16 : VK_INDEX_TYPE_UINT32;
        vkCmdBindIndexBuffer(VkCommandBuffer, indexBufferView.Resource.VkBuffer, indexBufferView.ByteOffset, indexType);
    }

    /// <inheritdoc />
    public override void BindPipeline(GraphicsPipeline pipeline)
        => BindPipeline((VulkanGraphicsPipeline)pipeline);

    /// <inheritdoc cref="BindPipeline(GraphicsPipeline)" />
    public void BindPipeline(VulkanGraphicsPipeline pipeline)
    {
        ThrowIfNull(pipeline);
        vkCmdBindPipeline(VkCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.VkPipeline);
    }

    /// <inheritdoc />
    public override void BindPipelineDescriptorSet(GraphicsPipelineDescriptorSet pipelineResourceViews)
        => BindPipeline((VulkanGraphicsPipelineDescriptorSet)pipelineResourceViews);

    /// <inheritdoc cref="BindPipelineDescriptorSet(GraphicsPipelineDescriptorSet)" />
    public void BindPipeline(VulkanGraphicsPipelineDescriptorSet pipelineDescriptorSet)
    {
        ThrowIfNull(pipelineDescriptorSet);
        var pipeline = pipelineDescriptorSet.Pipeline;

        var vkDescriptorSet = pipelineDescriptorSet.VkDescriptorSet;
        var resourceViews = pipelineDescriptorSet.ResourceViews;

        for (var index = 0; index < resourceViews.Length; index++)
        {
            var resourceView = resourceViews[index];

            VkWriteDescriptorSet vkWriteDescriptorSet;

            if (resourceView is VulkanGraphicsBufferView vulkanGraphicsBufferView)
            {
                var vkDescriptorBufferInfo = new VkDescriptorBufferInfo {
                    buffer = vulkanGraphicsBufferView.Resource.VkBuffer,
                    offset = vulkanGraphicsBufferView.ByteOffset,
                    range = vulkanGraphicsBufferView.ByteLength,
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
            else if (resourceView is VulkanGraphicsTextureView textureView)
            {
                var vkDescriptorImageInfo = new VkDescriptorImageInfo {
                    sampler = textureView.Resource.VkSampler,
                    imageView = textureView.VkImageView,
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

        vkCmdBindDescriptorSets(VkCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline.Signature.VkPipelineLayout, firstSet: 0, descriptorSetCount: 1, &vkDescriptorSet, dynamicOffsetCount: 0, pDynamicOffsets: null);
    }

    /// <inheritdoc />
    public override void BindVertexBufferView(GraphicsBufferView vertexBufferView, uint bindingSlot = 0)
        => BindVertexBufferView((VulkanGraphicsBufferView)vertexBufferView, bindingSlot);

    /// <inheritdoc cref="BindVertexBufferView(GraphicsBufferView, uint)" />
    public void BindVertexBufferView(VulkanGraphicsBufferView vertexBufferView, uint bindingSlot = 0)
    {
        ThrowIfNull(vertexBufferView);

        var vkVertexBuffer = vertexBufferView.Resource.VkBuffer;
        var vkVertexBufferOffset = (ulong)vertexBufferView.ByteOffset;

        vkCmdBindVertexBuffers(VkCommandBuffer, firstBinding: bindingSlot, bindingCount: 1, &vkVertexBuffer, &vkVertexBufferOffset);
    }

    /// <inheritdoc />
    public override void BindVertexBufferViews(ReadOnlySpan<GraphicsBufferView> vertexBufferViews, uint firstBindingSlot = 0)
    {
        ThrowIfZero(vertexBufferViews.Length);
        ThrowIfNotInInsertBounds(vertexBufferViews.Length, MaxBoundVertexBufferViewCount);

        if (vertexBufferViews.Length <= 32)
        {
            var vkVertexBuffers = stackalloc VkBuffer[32];
            var vkVertexBufferOffsets = stackalloc ulong[32];

            BindVertexBufferViewsInternal(vertexBufferViews, firstBindingSlot, vkVertexBuffers, vkVertexBufferOffsets);
        }
        else
        {
            var vkVertexBuffers = UnmanagedArray<VkBuffer>.Empty;
            var vkVertexBufferOffsets = UnmanagedArray<ulong>.Empty;

            try
            {
                vkVertexBuffers = new UnmanagedArray<VkBuffer>((uint)vertexBufferViews.Length);
                vkVertexBufferOffsets = new UnmanagedArray<ulong>((uint)vertexBufferViews.Length);

                BindVertexBufferViewsInternal(vertexBufferViews, firstBindingSlot, vkVertexBuffers.GetPointerUnsafe(0), vkVertexBufferOffsets.GetPointerUnsafe(0));
            }
            finally
            {
                vkVertexBuffers.Dispose();
                vkVertexBufferOffsets.Dispose();
            }
        }
    }

    /// <inheritdoc />
    public override void Draw(uint verticesPerInstance, uint instanceCount = 1, uint vertexStart = 0, uint instanceStart = 0)
    {
        ThrowIfZero(verticesPerInstance);
        ThrowIfZero(instanceCount);

        vkCmdDraw(VkCommandBuffer, verticesPerInstance, instanceCount, vertexStart, instanceStart);
    }

    /// <inheritdoc />
    public override void DrawIndexed(uint indicesPerInstance, uint instanceCount = 1, uint indexStart = 0, int vertexStart = 0, uint instanceStart = 0)
    {
        ThrowIfZero(indicesPerInstance);
        ThrowIfZero(instanceCount);

        vkCmdDrawIndexed(VkCommandBuffer, indicesPerInstance, instanceCount, indexStart, vertexStart, instanceStart);
    }

    /// <inheritdoc />
    public override void EndRenderPass()
    {
        var renderPass = Interlocked.Exchange(ref _renderPass, null);

        if (renderPass is null)
        {
            ThrowForInvalidState(nameof(RenderPass));
        }

        vkCmdEndRenderPass(VkCommandBuffer);
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
    protected override void CloseUnsafe()
    {
        ThrowExternalExceptionIfNotSuccess(vkEndCommandBuffer(VkCommandBuffer));
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = ContextInfo.Fence;
            fence.Wait();
            fence.Reset();

            fence.Dispose();
            ContextInfo.Fence = null!;
        }

        var vkDevice = Device.VkDevice;
        var vkCommandPool = _vkCommandPool;

        DisposeVkCommandBuffer(vkDevice, vkCommandPool, _vkCommandBuffer);
        _vkCommandBuffer = VkCommandBuffer.NULL;

        DisposeVkCommandPool(vkDevice, vkCommandPool);
        _vkCommandPool = VkCommandPool.NULL;

        _ = CommandQueue.RemoveRenderContext(this);

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

    /// <inheritdoc />
    protected override void ExecuteUnsafe()
    {
        CommandQueue.ExecuteContextUnsafe(this);
    }

    /// <inheritdoc />
    protected override void ResetUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        ThrowExternalExceptionIfNotSuccess(vkResetCommandPool(Device.VkDevice, VkCommandPool, 0));

        var vkCommandBufferBeginInfo = new VkCommandBufferBeginInfo {
            sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            pNext = null,
            flags = 0,
            pInheritanceInfo = null,
        };
        ThrowExternalExceptionIfNotSuccess(vkBeginCommandBuffer(VkCommandBuffer, &vkCommandBufferBeginInfo));
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_COMMAND_BUFFER, VkCommandBuffer, value);
        Device.SetVkObjectName(VK_OBJECT_TYPE_COMMAND_POOL, VkCommandPool, value);
    }

    private void BindVertexBufferViewsInternal(ReadOnlySpan<GraphicsBufferView> vertexBufferViews, uint firstBindingSlot, VkBuffer* vkVertexBuffers, ulong* vkVertexBufferOffsets)
    {
        for (var index = 0; index < vertexBufferViews.Length; index++)
        {
            var vertexBufferView = (VulkanGraphicsBufferView)vertexBufferViews[index];
            vkVertexBuffers[index] = vertexBufferView.Resource.VkBuffer;
            vkVertexBufferOffsets[index] = vertexBufferView.ByteOffset;
        }

        vkCmdBindVertexBuffers(VkCommandBuffer, firstBindingSlot, bindingCount: (uint)vertexBufferViews.Length, vkVertexBuffers, vkVertexBufferOffsets);
    }
}
