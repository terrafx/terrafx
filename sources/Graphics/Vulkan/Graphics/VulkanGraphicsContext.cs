// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Utilities.VulkanUtilities;
using static TerraFX.Interop.VkAccessFlags;
using static TerraFX.Interop.VkCommandPoolCreateFlags;
using static TerraFX.Interop.VkComponentSwizzle;
using static TerraFX.Interop.VkDescriptorType;
using static TerraFX.Interop.VkImageAspectFlags;
using static TerraFX.Interop.VkImageLayout;
using static TerraFX.Interop.VkImageViewType;
using static TerraFX.Interop.VkIndexType;
using static TerraFX.Interop.VkPipelineBindPoint;
using static TerraFX.Interop.VkPipelineStageFlags;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkSubpassContents;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsContext : GraphicsContext
    {
        private readonly VulkanGraphicsFence _fence;
        private readonly VulkanGraphicsFence _waitForExecuteCompletionFence;

        private ValueLazy<VkCommandBuffer> _vulkanCommandBuffer;
        private ValueLazy<VkCommandPool> _vulkanCommandPool;
        private ValueLazy<VkFramebuffer> _vulkanFramebuffer;
        private ValueLazy<VkImageView> _vulkanSwapChainImageView;

        private VolatileState _state;

        internal VulkanGraphicsContext(VulkanGraphicsDevice device, int index)
            : base(device, index)
        {
            _fence = new VulkanGraphicsFence(device, isSignaled: true);
            _waitForExecuteCompletionFence = new VulkanGraphicsFence(device, isSignaled: false);

            _vulkanCommandBuffer = new ValueLazy<VkCommandBuffer>(CreateVulkanCommandBuffer);
            _vulkanCommandPool = new ValueLazy<VkCommandPool>(CreateVulkanCommandPool);
            _vulkanFramebuffer = new ValueLazy<VkFramebuffer>(CreateVulkanFramebuffer);
            _vulkanSwapChainImageView = new ValueLazy<VkImageView>(CreateVulkanSwapChainImageView);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsContext" /> class.</summary>
        ~VulkanGraphicsContext() => Dispose(isDisposing: false);

        /// <inheritdoc cref="GraphicsDeviceObject.Device" />
        public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

        /// <inheritdoc />
        public override VulkanGraphicsFence Fence => _fence;

        /// <summary>Gets the <see cref="VkCommandBuffer" /> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkCommandBuffer VulkanCommandBuffer => _vulkanCommandBuffer.Value;

        /// <summary>Gets the <see cref="VkCommandPool" /> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkCommandPool VulkanCommandPool => _vulkanCommandPool.Value;

        /// <summary>Gets the <see cref="VkFramebuffer"/> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkFramebuffer VulkanFramebuffer => _vulkanFramebuffer.Value;

        /// <summary>Gets the <see cref="VkImageView" /> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkImageView VulkanSwapChainImageView => _vulkanSwapChainImageView.Value;

        /// <summary>Gets a fence that is used to wait for the context to finish execution.</summary>
        public VulkanGraphicsFence WaitForExecuteCompletionFence => _waitForExecuteCompletionFence;

        /// <inheritdoc />
        public override void BeginDrawing(ColorRgba backgroundColor)
        {
            var clearValue = new VkClearValue();

            clearValue.color.float32[0] = backgroundColor.Red;
            clearValue.color.float32[1] = backgroundColor.Green;
            clearValue.color.float32[2] = backgroundColor.Blue;
            clearValue.color.float32[3] = backgroundColor.Alpha;

            var device = Device;
            var surface = device.Surface;

            var surfaceWidth = surface.Width;
            var surfaceHeight = surface.Height;

            var renderPassBeginInfo = new VkRenderPassBeginInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = device.VulkanRenderPass,
                framebuffer = VulkanFramebuffer,
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
        }

        /// <inheritdoc />
        public override void BeginFrame()
        {
            var fence = Fence;

            fence.Wait();
            fence.Reset();

            var commandBufferBeginInfo = new VkCommandBufferBeginInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            };

            ThrowExternalExceptionIfNotSuccess(vkBeginCommandBuffer(VulkanCommandBuffer, &commandBufferBeginInfo), nameof(vkBeginCommandBuffer));
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
            ThrowIfNull(destination, nameof(destination));
            ThrowIfNull(source, nameof(source));

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
            ThrowIfNull(destination, nameof(destination));
            ThrowIfNull(source, nameof(source));

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
            ThrowIfNull(primitive, nameof(primitive));

            var vulkanCommandBuffer = VulkanCommandBuffer;
            var pipeline = primitive.Pipeline;
            var pipelineSignature = pipeline.Signature;
            var vulkanPipeline = pipeline.VulkanPipeline;

            vkCmdBindPipeline(vulkanCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, vulkanPipeline);

            ref readonly var vertexBufferRegion = ref primitive.VertexBufferRegion;
            var vertexBuffer = (VulkanGraphicsBuffer)vertexBufferRegion.Collection;
            var vulkanVertexBuffer = vertexBuffer.VulkanBuffer;
            var vulkanVertexBufferOffset = vertexBufferRegion.Offset;

            vkCmdBindVertexBuffers(vulkanCommandBuffer, firstBinding: 0, bindingCount: 1, (ulong*)&vulkanVertexBuffer, &vulkanVertexBufferOffset);

            var vulkanDescriptorSet = pipelineSignature.VulkanDescriptorSet;

            if (vulkanDescriptorSet != VK_NULL_HANDLE)
            {
                var inputResourceRegions = primitive.InputResourceRegions;
                var inputResourceRegionsLength = inputResourceRegions.Length;

                for (var index = 0; index < inputResourceRegionsLength; index++)
                {
                    var inputResourceRegion = inputResourceRegions[index];

                    VkWriteDescriptorSet writeDescriptorSet;

                    if (inputResourceRegion.Collection is VulkanGraphicsBuffer vulkanGraphicsBuffer)
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
                    else if (inputResourceRegion.Collection is VulkanGraphicsTexture vulkanGraphicsTexture)
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

                vkCmdBindDescriptorSets(vulkanCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineSignature.VulkanPipelineLayout, firstSet: 0, 1, (ulong*)&vulkanDescriptorSet, dynamicOffsetCount: 0, pDynamicOffsets: null);
            }

            ref readonly var indexBufferRegion = ref primitive.IndexBufferRegion;

            if (indexBufferRegion.Collection is VulkanGraphicsBuffer indexBuffer)
            {
                var indexBufferStride = primitive.IndexBufferStride;
                var indexType = VK_INDEX_TYPE_UINT16;

                if (indexBufferStride != 2)
                {
                    Assert(AssertionsEnabled && (indexBufferStride == 4));
                    indexType = VK_INDEX_TYPE_UINT32;
                }
                vkCmdBindIndexBuffer(vulkanCommandBuffer, indexBuffer.VulkanBuffer, indexBufferRegion.Offset, indexType);

                vkCmdDrawIndexed(vulkanCommandBuffer, indexCount: (uint)(indexBufferRegion.Size / indexBufferStride), instanceCount: 1, firstIndex: 0, vertexOffset: 0, firstInstance: 0);
            }
            else
            {
                vkCmdDraw(vulkanCommandBuffer, vertexCount: (uint)(vertexBufferRegion.Size / primitive.VertexBufferStride), instanceCount: 1, firstVertex: 0, firstInstance: 0);
            }
        }

        /// <inheritdoc />
        public override void EndDrawing() => vkCmdEndRenderPass(VulkanCommandBuffer);

        /// <inheritdoc />
        public override void EndFrame()
        {
            var commandBuffer = VulkanCommandBuffer;
            ThrowExternalExceptionIfNotSuccess(vkEndCommandBuffer(commandBuffer), nameof(vkEndCommandBuffer));

            var submitInfo = new VkSubmitInfo {
                sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
                commandBufferCount = 1,
                pCommandBuffers = (IntPtr*)&commandBuffer,
            };

            var executeGraphicsFence = WaitForExecuteCompletionFence;
            ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(Device.VulkanCommandQueue, submitCount: 1, &submitInfo, executeGraphicsFence.VulkanFence), nameof(vkQueueSubmit));

            executeGraphicsFence.Wait();
            executeGraphicsFence.Reset();
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanCommandBuffer.Dispose(DisposeVulkanCommandBuffer);
                _vulkanCommandPool.Dispose(DisposeVulkanCommandPool);
                _vulkanFramebuffer.Dispose(DisposeVulkanFramebuffer);
                _vulkanSwapChainImageView.Dispose(DisposeVulkanSwapChainImageView);

                _waitForExecuteCompletionFence?.Dispose();
                _fence?.Dispose();
            }

            _state.EndDispose();
        }

        internal void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
        {
            if (_vulkanFramebuffer.IsValueCreated)
            {
                var vulkanFramebuffer = _vulkanFramebuffer.Value;

                if (vulkanFramebuffer != VK_NULL_HANDLE)
                {
                    vkDestroyFramebuffer(Device.VulkanDevice, vulkanFramebuffer, pAllocator: null);
                }

                _vulkanFramebuffer.Reset(CreateVulkanFramebuffer);
            }

            if (_vulkanSwapChainImageView.IsValueCreated)
            {
                var vulkanSwapChainImageView = _vulkanSwapChainImageView.Value;

                if (vulkanSwapChainImageView != VK_NULL_HANDLE)
                {
                    vkDestroyImageView(Device.VulkanDevice, vulkanSwapChainImageView, pAllocator: null);
                }

                _vulkanSwapChainImageView.Reset(CreateVulkanSwapChainImageView);
            }
        }

        private VkCommandBuffer CreateVulkanCommandBuffer()
        {
            VkCommandBuffer vulkanCommandBuffer;

            var commandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                commandPool = VulkanCommandPool,
                commandBufferCount = 1,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateCommandBuffers(Device.VulkanDevice, &commandBufferAllocateInfo, (IntPtr*)&vulkanCommandBuffer), nameof(vkAllocateCommandBuffers));

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
            ThrowExternalExceptionIfNotSuccess(vkCreateCommandPool(Device.VulkanDevice, &commandPoolCreateInfo, pAllocator: null, (ulong*)&vulkanCommandPool), nameof(vkCreateCommandPool));

            return vulkanCommandPool;
        }

        private VkFramebuffer CreateVulkanFramebuffer()
        {
            VkFramebuffer vulkanFramebuffer;

            var device = Device;
            var surface = device.Surface;
            var swapChainImageView = VulkanSwapChainImageView;

            var frameBufferCreateInfo = new VkFramebufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                renderPass = device.VulkanRenderPass,
                attachmentCount = 1,
                pAttachments = (ulong*)&swapChainImageView,
                width = (uint)surface.Width,
                height = (uint)surface.Height,
                layers = 1,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateFramebuffer(device.VulkanDevice, &frameBufferCreateInfo, pAllocator: null, (ulong*)&vulkanFramebuffer), nameof(vkCreateFramebuffer));

            return vulkanFramebuffer;
        }

        private VkImageView CreateVulkanSwapChainImageView()
        {
            VkImageView swapChainImageView;

            var device = Device;

            var swapChainImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                image = device.VulkanSwapchainImages[Index],
                viewType = VK_IMAGE_VIEW_TYPE_2D,
                format = device.VulkanSwapchainFormat,
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
            ThrowExternalExceptionIfNotSuccess(vkCreateImageView(device.VulkanDevice, &swapChainImageViewCreateInfo, pAllocator: null, (ulong*)&swapChainImageView), nameof(vkCreateImageView));

            return swapChainImageView;
        }

        private void DisposeVulkanCommandBuffer(VkCommandBuffer vulkanCommandBuffer)
        {
            AssertDisposing(_state);

            if (vulkanCommandBuffer != null)
            {
                vkFreeCommandBuffers(Device.VulkanDevice, VulkanCommandPool, 1, (IntPtr*)&vulkanCommandBuffer);
            }
        }

        private void DisposeVulkanCommandPool(VkCommandPool vulkanCommandPool)
        {
            AssertDisposing(_state);

            if (vulkanCommandPool != VK_NULL_HANDLE)
            {
                vkDestroyCommandPool(Device.VulkanDevice, vulkanCommandPool, pAllocator: null);
            }
        }

        private void DisposeVulkanFramebuffer(VkFramebuffer vulkanFramebuffer)
        {
            AssertDisposing(_state);

            if (vulkanFramebuffer != VK_NULL_HANDLE)
            {
                vkDestroyFramebuffer(Device.VulkanDevice, vulkanFramebuffer, pAllocator: null);
            }
        }

        private void DisposeVulkanSwapChainImageView(VkImageView vulkanSwapchainImageView)
        {
            AssertDisposing(_state);

            if (vulkanSwapchainImageView != VK_NULL_HANDLE)
            {
                vkDestroyImageView(Device.VulkanDevice, vulkanSwapchainImageView, pAllocator: null);
            }
        }
    }
}
