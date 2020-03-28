// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkAccessFlagBits;
using static TerraFX.Interop.VkCommandPoolCreateFlagBits;
using static TerraFX.Interop.VkComponentSwizzle;
using static TerraFX.Interop.VkDescriptorType;
using static TerraFX.Interop.VkImageAspectFlagBits;
using static TerraFX.Interop.VkImageLayout;
using static TerraFX.Interop.VkImageViewType;
using static TerraFX.Interop.VkIndexType;
using static TerraFX.Interop.VkPipelineBindPoint;
using static TerraFX.Interop.VkPipelineStageFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkSubpassContents;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsContext : GraphicsContext
    {
        private readonly VulkanGraphicsFence _graphicsFence;
        private readonly VulkanGraphicsFence _waitForExecuteCompletionGraphicsFence;

        private ValueLazy<VkCommandBuffer> _vulkanCommandBuffer;
        private ValueLazy<VkCommandPool> _vulkanCommandPool;
        private ValueLazy<VkFramebuffer> _vulkanFramebuffer;
        private ValueLazy<VkImageView> _vulkanSwapChainImageView;

        private State _state;

        internal VulkanGraphicsContext(VulkanGraphicsDevice graphicsDevice, int index)
            : base(graphicsDevice, index)
        {
            _graphicsFence = new VulkanGraphicsFence(graphicsDevice);
            _waitForExecuteCompletionGraphicsFence = new VulkanGraphicsFence(graphicsDevice);

            _vulkanCommandBuffer = new ValueLazy<VkCommandBuffer>(CreateVulkanCommandBuffer);
            _vulkanCommandPool = new ValueLazy<VkCommandPool>(CreateVulkanCommandPool);
            _vulkanFramebuffer = new ValueLazy<VkFramebuffer>(CreateVulkanFramebuffer);
            _vulkanSwapChainImageView = new ValueLazy<VkImageView>(CreateVulkanSwapChainImageView);

            _ = _state.Transition(to: Initialized);

            WaitForExecuteCompletionGraphicsFence.Reset();
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsContext" /> class.</summary>
        ~VulkanGraphicsContext()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
        public override GraphicsFence GraphicsFence => VulkanGraphicsFence;

        /// <summary>Gets the <see cref="VkCommandBuffer" /> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkCommandBuffer VulkanCommandBuffer => _vulkanCommandBuffer.Value;

        /// <summary>Gets the <see cref="VkCommandPool" /> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkCommandPool VulkanCommandPool => _vulkanCommandPool.Value;

        /// <summary>Gets the <see cref="VkFramebuffer"/> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkFramebuffer VulkanFramebuffer => _vulkanFramebuffer.Value;

        /// <inheritdoc cref="GraphicsContext.GraphicsDevice" />
        public VulkanGraphicsDevice VulkanGraphicsDevice => (VulkanGraphicsDevice)GraphicsDevice;

        /// <inheritdoc cref="VulkanGraphicsContext.GraphicsFence" />
        public VulkanGraphicsFence VulkanGraphicsFence => _graphicsFence;

        /// <summary>Gets the <see cref="VkImageView" /> used by the context.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public VkImageView VulkanSwapChainImageView => _vulkanSwapChainImageView.Value;

        /// <summary>Gets a graphics fence that is used to wait for the context to finish execution.</summary>
        public VulkanGraphicsFence WaitForExecuteCompletionGraphicsFence => _waitForExecuteCompletionGraphicsFence;

        /// <inheritdoc />
        public override void BeginDrawing(ColorRgba backgroundColor)
        {
            var clearValue = new VkClearValue();

            clearValue.color.float32[0] = backgroundColor.Red;
            clearValue.color.float32[1] = backgroundColor.Green;
            clearValue.color.float32[2] = backgroundColor.Blue;
            clearValue.color.float32[3] = backgroundColor.Alpha;

            var graphicsDevice = VulkanGraphicsDevice;
            var graphicsSurface = graphicsDevice.GraphicsSurface;

            var graphicsSurfaceWidth = graphicsSurface.Width;
            var graphicsSurfaceHeight = graphicsSurface.Height;

            var renderPassBeginInfo = new VkRenderPassBeginInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = graphicsDevice.VulkanRenderPass,
                framebuffer = VulkanFramebuffer,
                renderArea = new VkRect2D {
                    extent = new VkExtent2D {
                        width = (uint)graphicsSurface.Width,
                        height = (uint)graphicsSurface.Height,
                    },
                },
                clearValueCount = 1,
                pClearValues = &clearValue,
            };

            var commandBuffer = VulkanCommandBuffer;
            vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);

            var viewport = new VkViewport {
                width = graphicsSurface.Width,
                height = graphicsSurface.Height,
                minDepth = 0.0f,
                maxDepth = 1.0f,
            };
            vkCmdSetViewport(commandBuffer, firstViewport: 0, viewportCount: 1, &viewport);

            var scissorRect = new VkRect2D {
                extent = new VkExtent2D {
                    width = (uint)graphicsSurface.Width,
                    height = (uint)graphicsSurface.Height,
                },
            };
            vkCmdSetScissor(commandBuffer, firstScissor: 0, scissorCount: 1, &scissorRect);
        }

        /// <inheritdoc />
        public override void BeginFrame()
        {
            var graphicsFence = VulkanGraphicsFence;

            graphicsFence.Wait();
            graphicsFence.Reset();

            var commandBufferBeginInfo = new VkCommandBufferBeginInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            };

            ThrowExternalExceptionIfNotSuccess(nameof(vkBeginCommandBuffer), vkBeginCommandBuffer(VulkanCommandBuffer, &commandBufferBeginInfo));
        }

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
                    aspectMask = (uint)VK_IMAGE_ASPECT_COLOR_BIT,
                    layerCount = 1,
                },
                imageExtent = new VkExtent3D {
                    width = (uint)destination.Width,
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
                    dstAccessMask = (uint)VK_ACCESS_TRANSFER_WRITE_BIT,
                    oldLayout = VK_IMAGE_LAYOUT_UNDEFINED,
                    newLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                    image = vulkanImage,
                    subresourceRange = new VkImageSubresourceRange {
                        aspectMask = (uint)VK_IMAGE_ASPECT_COLOR_BIT,
                        levelCount = 1,
                        layerCount = 1,
                    },
                };

                vkCmdPipelineBarrier(vulkanCommandBuffer, (uint)VK_PIPELINE_STAGE_HOST_BIT, (uint)VK_PIPELINE_STAGE_TRANSFER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vulkanImageMemoryBarrier);
            }

            void EndCopy()
            {
                var vulkanImageMemoryBarrier = new VkImageMemoryBarrier {
                    sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                    srcAccessMask = (uint)VK_ACCESS_TRANSFER_WRITE_BIT,
                    dstAccessMask = (uint)VK_ACCESS_SHADER_READ_BIT,
                    oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                    newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                    image = vulkanImage,
                    subresourceRange = new VkImageSubresourceRange {
                        aspectMask = (uint)VK_IMAGE_ASPECT_COLOR_BIT,
                        levelCount = 1,
                        layerCount = 1,
                    },
                };

                vkCmdPipelineBarrier(vulkanCommandBuffer, (uint)VK_PIPELINE_STAGE_TRANSFER_BIT, (uint)VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vulkanImageMemoryBarrier);
            }
        }

        /// <inheritdoc />
        public override void Copy(GraphicsBuffer destination, GraphicsBuffer source) => Copy((VulkanGraphicsBuffer)destination, (VulkanGraphicsBuffer)source);

        /// <inheritdoc />
        public override void Copy(GraphicsTexture destination, GraphicsBuffer source) => Copy((VulkanGraphicsTexture)destination, (VulkanGraphicsBuffer)source);

        /// <inheritdoc cref="Draw(GraphicsPrimitive)" />
        public void Draw(VulkanGraphicsPrimitive graphicsPrimitive)
        {
            ThrowIfNull(graphicsPrimitive, nameof(graphicsPrimitive));

            var graphicsCommandBuffer = VulkanCommandBuffer;
            var graphicsPipeline = graphicsPrimitive.VulkanGraphicsPipeline;
            var graphicsPipelineSignature = graphicsPipeline.VulkanSignature;
            var vulkanPipeline = graphicsPipeline.VulkanPipeline;
            var vertexBuffer = graphicsPrimitive.VulkanVertexBuffer;
            var vulkanVertexBuffer = vertexBuffer.VulkanBuffer;
            var vulkanVertexBufferOffset = 0ul;

            vkCmdBindPipeline(graphicsCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, vulkanPipeline);
            vkCmdBindVertexBuffers(graphicsCommandBuffer, firstBinding: 0, bindingCount: 1, (ulong*)&vulkanVertexBuffer, &vulkanVertexBufferOffset);

            var vulkanDescriptorSet = graphicsPipelineSignature.VulkanDescriptorSet;

            if (vulkanDescriptorSet != VK_NULL_HANDLE)
            {
                var inputResources = graphicsPrimitive.InputResources;
                var inputResourcesLength = inputResources.Length;

                for (var index = 0; index < inputResourcesLength; index++)
                {
                    var inputResource = inputResources[index];

                    VkWriteDescriptorSet writeDescriptorSet;

                    if (inputResource is VulkanGraphicsBuffer vulkanGraphicsBuffer)
                    {
                        var descriptorBufferInfo = new VkDescriptorBufferInfo {
                            buffer = vulkanGraphicsBuffer.VulkanBuffer,
                            offset = 0,
                            range = vulkanGraphicsBuffer.Size,
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
                    else if (inputResource is VulkanGraphicsTexture vulkanGraphicsTexture)
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

                    vkUpdateDescriptorSets(VulkanGraphicsDevice.VulkanDevice, 1, &writeDescriptorSet, 0, pDescriptorCopies: null);
                }

                vkCmdBindDescriptorSets(graphicsCommandBuffer, VK_PIPELINE_BIND_POINT_GRAPHICS, graphicsPipelineSignature.VulkanPipelineLayout, firstSet: 0, 1, (ulong*)&vulkanDescriptorSet, dynamicOffsetCount: 0, pDynamicOffsets: null);
            }

            var indexBuffer = graphicsPrimitive.VulkanIndexBuffer;

            if (indexBuffer != null)
            {
                var indexBufferStride = indexBuffer.Stride;
                var indexType = VK_INDEX_TYPE_UINT16;

                if (indexBufferStride != 2)
                {
                    Assert(indexBufferStride == 4, "Index Buffer has an unsupported stride.");
                    indexType = VK_INDEX_TYPE_UINT32;
                }
                vkCmdBindIndexBuffer(graphicsCommandBuffer, indexBuffer.VulkanBuffer, offset: 0, indexType);

                vkCmdDrawIndexed(graphicsCommandBuffer, indexCount: (uint)(indexBuffer.Size / indexBufferStride), instanceCount: 1, firstIndex: 0, vertexOffset: 0, firstInstance: 0);
            }
            else
            {
                vkCmdDraw(graphicsCommandBuffer, vertexCount: (uint)(vertexBuffer.Size / vertexBuffer.Stride), instanceCount: 1, firstVertex: 0, firstInstance: 0);
            }
        }

        /// <inheritdoc />
        public override void Draw(GraphicsPrimitive graphicsPrimitive) => Draw((VulkanGraphicsPrimitive)graphicsPrimitive);

        /// <inheritdoc />
        public override void EndDrawing() => vkCmdEndRenderPass(VulkanCommandBuffer);

        /// <inheritdoc />
        public override void EndFrame()
        {
            var commandBuffer = VulkanCommandBuffer;
            ThrowExternalExceptionIfNotSuccess(nameof(vkEndCommandBuffer), vkEndCommandBuffer(commandBuffer));

            var submitInfo = new VkSubmitInfo {
                sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
                commandBufferCount = 1,
                pCommandBuffers = (IntPtr*)&commandBuffer,
            };

            var executeGraphicsFence = WaitForExecuteCompletionGraphicsFence;
            ThrowExternalExceptionIfNotSuccess(nameof(vkQueueSubmit), vkQueueSubmit(VulkanGraphicsDevice.VulkanCommandQueue, submitCount: 1, &submitInfo, executeGraphicsFence.VulkanFence));

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

                DisposeIfNotNull(_waitForExecuteCompletionGraphicsFence);
                DisposeIfNotNull(_graphicsFence);
            }

            _state.EndDispose();
        }

        internal void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
        {
            if (_vulkanFramebuffer.IsCreated)
            {
                var vulkanFramebuffer = _vulkanFramebuffer.Value;

                if (vulkanFramebuffer != VK_NULL_HANDLE)
                {
                    vkDestroyFramebuffer(VulkanGraphicsDevice.VulkanDevice, vulkanFramebuffer, pAllocator: null);
                }

                _vulkanFramebuffer.Reset(CreateVulkanFramebuffer);
            }

            if (_vulkanSwapChainImageView.IsCreated)
            {
                var vulkanSwapChainImageView = _vulkanSwapChainImageView.Value;

                if (vulkanSwapChainImageView != VK_NULL_HANDLE)
                {
                    vkDestroyImageView(VulkanGraphicsDevice.VulkanDevice, vulkanSwapChainImageView, pAllocator: null);
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
            ThrowExternalExceptionIfNotSuccess(nameof(vkAllocateCommandBuffers), vkAllocateCommandBuffers(VulkanGraphicsDevice.VulkanDevice, &commandBufferAllocateInfo, (IntPtr*)&vulkanCommandBuffer));

            return vulkanCommandBuffer;
        }

        private VkCommandPool CreateVulkanCommandPool()
        {
            VkCommandPool vulkanCommandPool;

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                flags = (uint)VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
                queueFamilyIndex = VulkanGraphicsDevice.VulkanCommandQueueFamilyIndex,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateCommandPool), vkCreateCommandPool(VulkanGraphicsDevice.VulkanDevice, &commandPoolCreateInfo, pAllocator: null, (ulong*)&vulkanCommandPool));

            return vulkanCommandPool;
        }

        private VkFramebuffer CreateVulkanFramebuffer()
        {
            VkFramebuffer vulkanFramebuffer;

            var graphicsDevice = VulkanGraphicsDevice;
            var graphicsSurface = graphicsDevice.GraphicsSurface;
            var swapChainImageView = VulkanSwapChainImageView;

            var frameBufferCreateInfo = new VkFramebufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                renderPass = graphicsDevice.VulkanRenderPass,
                attachmentCount = 1,
                pAttachments = (ulong*)&swapChainImageView,
                width = (uint)graphicsSurface.Width,
                height = (uint)graphicsSurface.Height,
                layers = 1,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateFramebuffer), vkCreateFramebuffer(graphicsDevice.VulkanDevice, &frameBufferCreateInfo, pAllocator: null, (ulong*)&vulkanFramebuffer));

            return vulkanFramebuffer;
        }

        private VkImageView CreateVulkanSwapChainImageView()
        {
            VkImageView swapChainImageView;

            var graphicsDevice = VulkanGraphicsDevice;

            var swapChainImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                image = graphicsDevice.VulkanSwapchainImages[Index],
                viewType = VK_IMAGE_VIEW_TYPE_2D,
                format = graphicsDevice.VulkanSwapchainFormat,
                components = new VkComponentMapping {
                    r = VK_COMPONENT_SWIZZLE_R,
                    g = VK_COMPONENT_SWIZZLE_G,
                    b = VK_COMPONENT_SWIZZLE_B,
                    a = VK_COMPONENT_SWIZZLE_A,
                },
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = (uint)VK_IMAGE_ASPECT_COLOR_BIT,
                    levelCount = 1,
                    layerCount = 1,
                },
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateImageView), vkCreateImageView(graphicsDevice.VulkanDevice, &swapChainImageViewCreateInfo, pAllocator: null, (ulong*)&swapChainImageView));

            return swapChainImageView;
        }

        private void DisposeVulkanCommandBuffer(VkCommandBuffer vulkanCommandBuffer)
        {
            _state.AssertDisposing();

            if (vulkanCommandBuffer != null)
            {
                vkFreeCommandBuffers(VulkanGraphicsDevice.VulkanDevice, VulkanCommandPool, 1, (IntPtr*)&vulkanCommandBuffer);
            }
        }

        private void DisposeVulkanCommandPool(VkCommandPool vulkanCommandPool)
        {
            _state.AssertDisposing();

            if (vulkanCommandPool != VK_NULL_HANDLE)
            {
                vkDestroyCommandPool(VulkanGraphicsDevice.VulkanDevice, vulkanCommandPool, pAllocator: null);
            }
        }

        private void DisposeVulkanFramebuffer(VkFramebuffer vulkanFramebuffer)
        {
            _state.AssertDisposing();

            if (vulkanFramebuffer != VK_NULL_HANDLE)
            {
                vkDestroyFramebuffer(VulkanGraphicsDevice.VulkanDevice, vulkanFramebuffer, pAllocator: null);
            }
        }

        private void DisposeVulkanSwapChainImageView(VkImageView vulkanSwapchainImageView)
        {
            _state.AssertDisposing();

            if (vulkanSwapchainImageView != VK_NULL_HANDLE)
            {
                vkDestroyImageView(VulkanGraphicsDevice.VulkanDevice, vulkanSwapchainImageView, pAllocator: null);
            }
        }
    }
}
