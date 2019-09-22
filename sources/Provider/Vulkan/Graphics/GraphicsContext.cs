// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Interop.VkAttachmentLoadOp;
using static TerraFX.Interop.VkAttachmentStoreOp;
using static TerraFX.Interop.VkColorSpaceKHR;
using static TerraFX.Interop.VkCommandBufferLevel;
using static TerraFX.Interop.VkCommandPoolCreateFlagBits;
using static TerraFX.Interop.VkComponentSwizzle;
using static TerraFX.Interop.VkCompositeAlphaFlagBitsKHR;
using static TerraFX.Interop.VkFenceCreateFlagBits;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageAspectFlagBits;
using static TerraFX.Interop.VkImageLayout;
using static TerraFX.Interop.VkImageUsageFlagBits;
using static TerraFX.Interop.VkImageViewType;
using static TerraFX.Interop.VkPipelineBindPoint;
using static TerraFX.Interop.VkPipelineStageFlagBits;
using static TerraFX.Interop.VkPresentModeKHR;
using static TerraFX.Interop.VkQueueFlagBits;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkSampleCountFlagBits;
using static TerraFX.Interop.VkSharingMode;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkSubpassContents;
using static TerraFX.Interop.VkSurfaceTransformFlagBitsKHR;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public sealed unsafe class GraphicsContext : IGraphicsContext
    {
        private static ReadOnlySpan<sbyte> VK_KHR_swapchain => new sbyte[] {
            0x56,
            0x4B,
            0x5F,
            0x4B,
            0x48,
            0x52,
            0x5F,
            0x73,
            0x77,
            0x61,
            0x70,
            0x63,
            0x68,
            0x61,
            0x69,
            0x6E,
            0x00,
        };

        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IGraphicsSurface _graphicsSurface;

        private ResettableLazy<IntPtr> _acquireNextImageSemaphore;
        private ResettableLazy<IntPtr[]> _commandBuffers;
        private ResettableLazy<IntPtr> _commandPool;
        private ResettableLazy<IntPtr> _device;
        private ResettableLazy<IntPtr> _deviceQueue;
        private ResettableLazy<IntPtr[]> _fences;
        private ResettableLazy<IntPtr[]> _frameBuffers;
        private ResettableLazy<uint> _graphicsQueueFamilyIndex;
        private ResettableLazy<IntPtr> _queueSubmitSemaphore;
        private ResettableLazy<IntPtr> _renderPass;
        private ResettableLazy<IntPtr> _surface;
        private ResettableLazy<IntPtr> _swapChain;
        private ResettableLazy<IntPtr[]> _swapChainImageViews;

        private uint _frameIndex;
        private State _state;
        private VkFormat _swapChainFormat;

        internal GraphicsContext(GraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface)
        {
            _graphicsAdapter = graphicsAdapter;
            _graphicsSurface = graphicsSurface;

            _acquireNextImageSemaphore = new ResettableLazy<IntPtr>(CreateAcquireNextImageSemaphore);
            _commandBuffers = new ResettableLazy<IntPtr[]>(CreateCommandBuffers);
            _commandPool = new ResettableLazy<IntPtr>(CreateCommandPool);
            _device = new ResettableLazy<IntPtr>(CreateDevice);
            _deviceQueue = new ResettableLazy<IntPtr>(CreateDeviceQueue);
            _fences = new ResettableLazy<IntPtr[]>(CreateFences);
            _frameBuffers = new ResettableLazy<IntPtr[]>(CreateFrameBuffers);
            _graphicsQueueFamilyIndex = new ResettableLazy<uint>(FindGraphicsQueueFamilyIndex);
            _queueSubmitSemaphore = new ResettableLazy<IntPtr>(CreateQueueSubmitSemaphore);
            _renderPass = new ResettableLazy<IntPtr>(CreateRenderPass);
            _surface = new ResettableLazy<IntPtr>(CreateSurface);
            _swapChain = new ResettableLazy<IntPtr>(CreateSwapChain);
            _swapChainImageViews = new ResettableLazy<IntPtr[]>(CreateSwapChainImageViews);

            _ = _state.Transition(to: Initialized);

            // Do event hookups after we are in the initialized state, since an event could
            // technically fire while the constructor is still running.

            _graphicsSurface.SizeChanged += HandleGraphicsSurfaceSizeChanged;
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsContext" /> class.</summary>
        ~GraphicsContext()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets a <c>vkSemaphore</c> for the <see cref="vkAcquireNextImageKHR(IntPtr, IntPtr, ulong, IntPtr, IntPtr, uint*)" /> method.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr AcquireNextImageSemaphore
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _acquireNextImageSemaphore.Value;
            }
        }

        /// <summary>Gets an array of <c>VkCommandBuffer</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr[] CommandBuffers
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _commandBuffers.Value;
            }
        }

        /// <summary>Gets the <c>VkCommandPool</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr CommandPool
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _commandPool.Value;
            }
        }

        /// <summary>Gets the <c>VkDevice</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr Device
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _device.Value;
            }
        }

        /// <summary>Gets the <c>VkQueue</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr DeviceQueue
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _deviceQueue.Value;
            }
        }

        /// <summary>Gets an array of <c>VkFence</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr[] Fences
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _fences.Value;
            }
        }

        /// <summary>Gets an array of <c>VkFramebuffer</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr[] FrameBuffers
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _frameBuffers.Value;
            }
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
        public IGraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the index of the graphics queue family for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public uint GraphicsQueueFamilyIndex
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _graphicsQueueFamilyIndex.Value;
            }
        }

        /// <summary>Gets the <see cref="IGraphicsSurface" /> for the instance.</summary>
        public IGraphicsSurface GraphicsSurface => _graphicsSurface;

        /// <summary>Gets a <c>vkSemaphore</c> for the <see cref="vkQueueSubmit(IntPtr, uint, VkSubmitInfo*, ulong)" /> method.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr QueueSubmitSemaphore
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _queueSubmitSemaphore.Value;
            }
        }

        /// <summary>Gets the <c>VkRenderPass</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr RenderPass
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _renderPass.Value;
            }
        }

        /// <summary>Gets the <c>VkSurfaceKHR</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr Surface
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _surface.Value;
            }
        }

        /// <summary>Gets the <c>VkSwapchainKHR</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr SwapChain
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _swapChain.Value;
            }
        }

        /// <summary>Gets an array of <c>VkImageView</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr[] SwapChainImageViews
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _swapChainImageViews.Value;
            }
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Begins a new frame for rendering.</summary>
        /// <param name="backgroundColor">A color to which the background should be cleared.</param>
        public void BeginFrame(ColorRgba backgroundColor)
        {
            uint frameIndex;
            var result = vkAcquireNextImageKHR(Device, SwapChain, timeout: ulong.MaxValue, AcquireNextImageSemaphore, fence: IntPtr.Zero, &frameIndex);
            _frameIndex = frameIndex;

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkAcquireNextImageKHR), (int)result);
            }

            var fence = Fences[frameIndex];
            result = vkWaitForFences(Device, fenceCount: 1, (ulong*)&fence, waitAll: VK_TRUE, timeout: ulong.MaxValue);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkWaitForFences), (int)result);
            }

            result = vkResetFences(Device, 1, (ulong*)&fence);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkResetFences), (int)result);
            }

            var commandBufferBeginInfo = new VkCommandBufferBeginInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                pNext = null,
                flags = 0,
                pInheritanceInfo = null,
            };

            var commandBuffer = CommandBuffers[frameIndex];
            result = vkBeginCommandBuffer(commandBuffer, &commandBufferBeginInfo);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkBeginCommandBuffer), (int)result);
            }

            var clearValue = new VkClearValue {
                color = new VkClearColorValue {
                },
                depthStencil = new VkClearDepthStencilValue {
                    depth = 0.0f,
                    stencil = 0,
                },
            };

            clearValue.color.float32[0] = backgroundColor.Red;
            clearValue.color.float32[1] = backgroundColor.Green;
            clearValue.color.float32[2] = backgroundColor.Blue;
            clearValue.color.float32[3] = backgroundColor.Alpha;

            var renderPassBeginInfo = new VkRenderPassBeginInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                pNext = null,
                renderPass = (ulong)RenderPass,
                framebuffer = (ulong)FrameBuffers[frameIndex],
                renderArea = new VkRect2D {
                    offset = new VkOffset2D {
                        x = 0,
                        y = 0,
                    },
                    extent = new VkExtent2D {
                        width = (uint)_graphicsSurface.Width,
                        height = (uint)_graphicsSurface.Height,
                    },
                },
                clearValueCount = 1,
                pClearValues = &clearValue,
            };

            vkCmdBeginRenderPass(commandBuffer, &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);
        }

        /// <summary>Ends the frame currently be rendered.</summary>
        public void EndFrame()
        {
            var frameIndex = _frameIndex;

            var commandBuffer = CommandBuffers[frameIndex];
            vkCmdEndRenderPass(commandBuffer);

            var result = vkEndCommandBuffer(commandBuffer);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkEndCommandBuffer), (int)result);
            }

            var waitSemaphores = stackalloc IntPtr[1];
            waitSemaphores[0] = AcquireNextImageSemaphore;

            var waitDstStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT;

            var signalSemaphores = stackalloc IntPtr[1];
            signalSemaphores[0] = QueueSubmitSemaphore;

            var submitInfo = new VkSubmitInfo {
                sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
                pNext = null,
                waitSemaphoreCount = 1,
                pWaitSemaphores = (ulong*)waitSemaphores,
                pWaitDstStageMask = (uint*)&waitDstStageMask,
                commandBufferCount = 1,
                pCommandBuffers = &commandBuffer,
                signalSemaphoreCount = 1,
                pSignalSemaphores = (ulong*)signalSemaphores
            };

            result = vkQueueSubmit(DeviceQueue, submitCount: 1, &submitInfo, fence: VK_NULL_HANDLE);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkQueueSubmit), (int)result);
            }
        }

        /// <summary>Presents the last frame rendered.</summary>
        public void PresentFrame()
        {
            var frameIndex = _frameIndex;

            var waitSemaphores = stackalloc IntPtr[1];
            waitSemaphores[0] = QueueSubmitSemaphore;

            var swapChain = SwapChain;

            var signalSemaphores = stackalloc IntPtr[1];
            signalSemaphores[0] = QueueSubmitSemaphore;

            var presentInfo = new VkPresentInfoKHR {
                sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
                pNext = null,
                waitSemaphoreCount = 1,
                pWaitSemaphores = waitSemaphores,
                swapchainCount = 1,
                pSwapchains = &swapChain,
                pImageIndices = &frameIndex,
                pResults = null,
            };

            var result = vkQueuePresentKHR(DeviceQueue, &presentInfo);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkQueuePresentKHR), (int)result);
            }

            result = vkQueueSubmit(DeviceQueue, submitCount: 0, pSubmits: null, (ulong)Fences[frameIndex]);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkQueueSubmit), (int)result);
            }
        }

        private IntPtr CreateAcquireNextImageSemaphore()
        {
            IntPtr acquireNextImageSemaphore;

            var acquireNextImageSemaphoreCreateInfo = new VkSemaphoreCreateInfo {
                sType = VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO,
                pNext = null,
                flags = 0,
            };

            var result = vkCreateSemaphore(Device, &acquireNextImageSemaphoreCreateInfo, pAllocator: null, (ulong*)&acquireNextImageSemaphore);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateSemaphore), (int)result);
            }

            return acquireNextImageSemaphore;
        }

        private IntPtr[] CreateCommandBuffers()
        {
            var commandBuffers = new IntPtr[(uint)_graphicsSurface.BufferCount];

            var commandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                pNext = null,
                commandPool = (ulong)CommandPool,
                level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandBufferCount = (uint)_graphicsSurface.BufferCount,
            };

            fixed (IntPtr* pCommandBuffers = commandBuffers)
            {
                var result = vkAllocateCommandBuffers(Device, &commandBufferAllocateInfo, pCommandBuffers);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkAllocateCommandBuffers), (int)result);
                }
            }

            return commandBuffers;
        }

        private IntPtr CreateCommandPool()
        {
            IntPtr commandPool;

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                pNext = null,
                flags = (uint)VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
                queueFamilyIndex = GraphicsQueueFamilyIndex,
            };

            var result = vkCreateCommandPool(Device, &commandPoolCreateInfo, pAllocator: null, (ulong*)&commandPool);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateCommandPool), (int)result);
            }

            return commandPool;
        }

        private IntPtr CreateDevice()
        {
            IntPtr device;

            var queuePriority = 1.0f;

            var deviceQueueCreateInfo = new VkDeviceQueueCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                pNext = null,
                flags = 0,
                queueFamilyIndex = GraphicsQueueFamilyIndex,
                queueCount = 1,
                pQueuePriorities = &queuePriority,
            };

            var enabledExtensionCount = 1u;

            var enabledExtensionNames = stackalloc sbyte*[(int)enabledExtensionCount];
            enabledExtensionNames[0] = (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in VK_KHR_swapchain[0]));

            var physicalDeviceFeatures = new VkPhysicalDeviceFeatures {
                robustBufferAccess = VK_FALSE,
                fullDrawIndexUint32 = VK_FALSE,
                imageCubeArray = VK_FALSE,
                independentBlend = VK_FALSE,
                geometryShader = VK_FALSE,
                tessellationShader = VK_FALSE,
                sampleRateShading = VK_FALSE,
                dualSrcBlend = VK_FALSE,
                logicOp = VK_FALSE,
                multiDrawIndirect = VK_FALSE,
                drawIndirectFirstInstance = VK_FALSE,
                depthClamp = VK_FALSE,
                depthBiasClamp = VK_FALSE,
                fillModeNonSolid = VK_FALSE,
                depthBounds = VK_FALSE,
                wideLines = VK_FALSE,
                largePoints = VK_FALSE,
                alphaToOne = VK_FALSE,
                multiViewport = VK_FALSE,
                samplerAnisotropy = VK_FALSE,
                textureCompressionETC2 = VK_FALSE,
                textureCompressionASTC_LDR = VK_FALSE,
                textureCompressionBC = VK_FALSE,
                occlusionQueryPrecise = VK_FALSE,
                pipelineStatisticsQuery = VK_FALSE,
                vertexPipelineStoresAndAtomics = VK_FALSE,
                fragmentStoresAndAtomics = VK_FALSE,
                shaderTessellationAndGeometryPointSize = VK_FALSE,
                shaderImageGatherExtended = VK_FALSE,
                shaderStorageImageExtendedFormats = VK_FALSE,
                shaderStorageImageMultisample = VK_FALSE,
                shaderStorageImageReadWithoutFormat = VK_FALSE,
                shaderStorageImageWriteWithoutFormat = VK_FALSE,
                shaderUniformBufferArrayDynamicIndexing = VK_FALSE,
                shaderSampledImageArrayDynamicIndexing = VK_FALSE,
                shaderStorageBufferArrayDynamicIndexing = VK_FALSE,
                shaderStorageImageArrayDynamicIndexing = VK_FALSE,
                shaderClipDistance = VK_FALSE,
                shaderCullDistance = VK_FALSE,
                shaderFloat64 = VK_FALSE,
                shaderInt64 = VK_FALSE,
                shaderInt16 = VK_FALSE,
                shaderResourceResidency = VK_FALSE,
                shaderResourceMinLod = VK_FALSE,
                sparseBinding = VK_FALSE,
                sparseResidencyBuffer = VK_FALSE,
                sparseResidencyImage2D = VK_FALSE,
                sparseResidencyImage3D = VK_FALSE,
                sparseResidency2Samples = VK_FALSE,
                sparseResidency4Samples = VK_FALSE,
                sparseResidency8Samples = VK_FALSE,
                sparseResidency16Samples = VK_FALSE,
                sparseResidencyAliased = VK_FALSE,
                variableMultisampleRate = VK_FALSE,
                inheritedQueries = VK_FALSE,
            };

            var deviceCreateInfo = new VkDeviceCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
                pNext = null,
                flags = 0,
                queueCreateInfoCount = 1,
                pQueueCreateInfos = &deviceQueueCreateInfo,
                enabledLayerCount = 0,
                ppEnabledLayerNames = null,
                enabledExtensionCount = 1,
                ppEnabledExtensionNames = enabledExtensionNames,
                pEnabledFeatures = &physicalDeviceFeatures,
            };

            var result = vkCreateDevice(_graphicsAdapter.PhysicalDevice, &deviceCreateInfo, pAllocator: null, &device);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateDevice), (int)result);
            }

            return device;
        }

        private IntPtr CreateDeviceQueue()
        {
            IntPtr deviceQueue;
            vkGetDeviceQueue(Device, GraphicsQueueFamilyIndex, queueIndex: 0, &deviceQueue);
            return deviceQueue;
        }

        private IntPtr[] CreateFences()
        {
            var fences = new IntPtr[(uint)_graphicsSurface.BufferCount];

            var fenceCreateInfo = new VkFenceCreateInfo {
                sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
                pNext = null,
                flags = (uint)VK_FENCE_CREATE_SIGNALED_BIT,
            };

            for (var i = 0; i < fences.Length; i++)
            {
                IntPtr fence;
                var result = vkCreateFence(Device, &fenceCreateInfo, pAllocator: null, (ulong*)&fence);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkCreateFence), (int)result);
                }

                fences[i] = fence;
            }



            return fences;
        }

        private IntPtr[] CreateFrameBuffers()
        {
            var frameBuffers = new IntPtr[(uint)_graphicsSurface.BufferCount];
            var attachments = stackalloc ulong[1];

            var frameBufferCreateInfo = new VkFramebufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                pNext = null,
                flags = 0,
                renderPass = (ulong)RenderPass,
                attachmentCount = 1,
                pAttachments = attachments,
                width = (uint)_graphicsSurface.Width,
                height = (uint)_graphicsSurface.Height,
                layers = 1,
            };

            for (var i = 0; i < frameBuffers.Length; i++)
            {
                attachments[0] = (ulong)SwapChainImageViews[i];

                IntPtr frameBuffer;
                var result = vkCreateFramebuffer(Device, &frameBufferCreateInfo, pAllocator: null, (ulong*)&frameBuffer);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkCreateFramebuffer), (int)result);
                }

                frameBuffers[i] = frameBuffer;
            }

            return frameBuffers;
        }

        private IntPtr CreateQueueSubmitSemaphore()
        {
            IntPtr queueSubmitSemaphore;

            var queueSubmitSemaphoreCreateInfo = new VkSemaphoreCreateInfo {
                sType = VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO,
                pNext = null,
                flags = 0,
            };

            var result = vkCreateSemaphore(Device, &queueSubmitSemaphoreCreateInfo, pAllocator: null, (ulong*)&queueSubmitSemaphore);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateSemaphore), (int)result);
            }

            return queueSubmitSemaphore;
        }

        private IntPtr CreateRenderPass()
        {
            IntPtr renderPass;

            var attachment = new VkAttachmentDescription {
                flags = 0,
                format = _swapChainFormat,
                samples = VK_SAMPLE_COUNT_1_BIT,
                loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VK_ATTACHMENT_STORE_OP_STORE,
                stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
                initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
                finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
            };

            var colorAttachment = new VkAttachmentReference {
                attachment = 0,
                layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
            };

            var subpass = new VkSubpassDescription {
                flags = 0,
                pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
                inputAttachmentCount = 0,
                pInputAttachments = null,
                colorAttachmentCount = 1,
                pColorAttachments = &colorAttachment,
                pResolveAttachments = null,
                pDepthStencilAttachment = null,
                preserveAttachmentCount = VK_FALSE,
                pPreserveAttachments = null,
            };

            var renderPassCreateInfo = new VkRenderPassCreateInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                pNext = null,
                flags = 0,
                attachmentCount = 1,
                pAttachments = &attachment,
                subpassCount = 1,
                pSubpasses = &subpass,
                dependencyCount = 0,
                pDependencies = null,
            };

            var result = vkCreateRenderPass(Device, &renderPassCreateInfo, pAllocator: null, (ulong*)&renderPass);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateDevice), (int)result);
            }

            return renderPass;
        }

        private IntPtr CreateSurface()
        {
            IntPtr surface;
            VkResult result;

            var graphicsProvider = (GraphicsProvider)_graphicsAdapter.GraphicsProvider;

            switch (_graphicsSurface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                        pNext = null,
                        flags = 0,
                        hinstance = _graphicsSurface.DisplayHandle,
                        hwnd = _graphicsSurface.WindowHandle,
                    };

                    result = vkCreateWin32SurfaceKHR(graphicsProvider.Instance, &surfaceCreateInfo, pAllocator: null, &surface);

                    if (result != VK_SUCCESS)
                    {
                        ThrowExternalException(nameof(vkCreateWin32SurfaceKHR), (int)result);
                    }
                    break;
                }

                case GraphicsSurfaceKind.Xlib:
                {
                    var surfaceCreateInfo = new VkXlibSurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR,
                        pNext = null,
                        flags = 0,
                        dpy = _graphicsSurface.DisplayHandle,
                        window = (UIntPtr)_graphicsSurface.WindowHandle.ToPointer(),
                    };

                    result = vkCreateXlibSurfaceKHR(graphicsProvider.Instance, &surfaceCreateInfo, pAllocator: null, &surface);

                    if (result != VK_SUCCESS)
                    {
                        ThrowExternalException(nameof(vkCreateXlibSurfaceKHR), (int)result);
                    }
                    break;
                }

                default:
                {
                    ThrowArgumentOutOfRangeException(nameof(_graphicsSurface), _graphicsSurface);
                    surface = IntPtr.Zero;
                    break;
                }
            }

            uint supported;
            result = vkGetPhysicalDeviceSurfaceSupportKHR(_graphicsAdapter.PhysicalDevice, GraphicsQueueFamilyIndex, surface, &supported);

            if (supported == VK_FALSE)
            {
                ThrowArgumentOutOfRangeException(nameof(_graphicsSurface), _graphicsSurface);
            }
            return surface;
        }

        private IntPtr CreateSwapChain()
        {
            IntPtr swapChain;

            VkSurfaceCapabilitiesKHR surfaceCapabilities;
            var result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(_graphicsAdapter.PhysicalDevice, Surface, &surfaceCapabilities);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfaceCapabilitiesKHR), (int)result);
            }

            uint presentModeCount;
            result = vkGetPhysicalDeviceSurfacePresentModesKHR(_graphicsAdapter.PhysicalDevice, Surface, &presentModeCount, pPresentModes: null);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), (int)result);
            }

            var presentModes = stackalloc VkPresentModeKHR[(int)presentModeCount];
            result = vkGetPhysicalDeviceSurfacePresentModesKHR(_graphicsAdapter.PhysicalDevice, Surface, &presentModeCount, presentModes);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), (int)result);
            }

            if (((uint)_graphicsSurface.BufferCount < surfaceCapabilities.minImageCount) ||
                ((surfaceCapabilities.maxImageCount != 0) && ((uint)_graphicsSurface.BufferCount > surfaceCapabilities.maxImageCount)))
            {
                ThrowArgumentOutOfRangeException(nameof(_graphicsSurface), _graphicsSurface);
            }

            var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
                sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
                pNext = null,
                flags = 0,
                surface = Surface,
                minImageCount = (uint)_graphicsSurface.BufferCount,
                imageFormat = VK_FORMAT_UNDEFINED,
                imageColorSpace = VK_COLOR_SPACE_SRGB_NONLINEAR_KHR,
                imageExtent = new VkExtent2D {
                    width = (uint)_graphicsSurface.Width,
                    height = (uint)_graphicsSurface.Height,
                },
                imageArrayLayers = 1,
                imageUsage = (uint)VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
                imageSharingMode = VK_SHARING_MODE_EXCLUSIVE,
                queueFamilyIndexCount = 0,
                pQueueFamilyIndices = null,
                preTransform = VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR,
                compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
                presentMode = VK_PRESENT_MODE_FIFO_KHR,
                clipped = VK_TRUE,
                oldSwapchain = IntPtr.Zero,
            };

            if ((surfaceCapabilities.supportedTransforms & (uint)VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR) == 0)
            {
                swapChainCreateInfo.preTransform = surfaceCapabilities.currentTransform;
            }

            uint surfaceFormatCount;
            result = vkGetPhysicalDeviceSurfaceFormatsKHR(_graphicsAdapter.PhysicalDevice, Surface, &surfaceFormatCount, pSurfaceFormats: null);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfaceFormatsKHR), (int)result);
            }

            var surfaceFormats = stackalloc VkSurfaceFormatKHR[(int)surfaceFormatCount];
            result = vkGetPhysicalDeviceSurfaceFormatsKHR(_graphicsAdapter.PhysicalDevice, Surface, &surfaceFormatCount, surfaceFormats);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfaceFormatsKHR), (int)result);
            }

            if ((surfaceFormatCount == 1) && (surfaceFormats[0].format == VK_FORMAT_UNDEFINED))
            {
                swapChainCreateInfo.imageFormat = VK_FORMAT_R8G8B8A8_UNORM;
            }
            else
            {
                swapChainCreateInfo.imageFormat = surfaceFormats[0].format;
            }
            swapChainCreateInfo.imageColorSpace = surfaceFormats[0].colorSpace;

            result = vkCreateSwapchainKHR(Device, &swapChainCreateInfo, pAllocator: null, &swapChain);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateSwapchainKHR), (int)result);
            }
            _swapChainFormat = swapChainCreateInfo.imageFormat;

            return swapChain;
        }

        private IntPtr[] CreateSwapChainImageViews()
        {
            var swapChainImageCount = (uint)_graphicsSurface.BufferCount;
            var swapChainImages = stackalloc IntPtr[(int)swapChainImageCount];

            var result = vkGetSwapchainImagesKHR(Device, SwapChain, &swapChainImageCount, swapChainImages);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetSwapchainImagesKHR), (int)result);
            }

            var swapChainImageViews = new IntPtr[swapChainImageCount];

            var swapChainImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                pNext = null,
                flags = 0,
                image = VK_NULL_HANDLE,
                viewType = VK_IMAGE_VIEW_TYPE_2D,
                format = _swapChainFormat,
                components = new VkComponentMapping {
                    r = VK_COMPONENT_SWIZZLE_IDENTITY,
                    g = VK_COMPONENT_SWIZZLE_IDENTITY,
                    b = VK_COMPONENT_SWIZZLE_IDENTITY,
                    a = VK_COMPONENT_SWIZZLE_IDENTITY,
                },
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = (uint)VK_IMAGE_ASPECT_COLOR_BIT,
                    baseMipLevel = 0,
                    levelCount = 1,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
            };

            for (var i = 0; i < swapChainImageViews.Length; i++)
            {
                swapChainImageViewCreateInfo.image = (ulong)swapChainImages[i];

                IntPtr swapChainImageView;
                result = vkCreateImageView(Device, &swapChainImageViewCreateInfo, pAllocator: null, (ulong)&swapChainImageView);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkCreateImageView), (int)result);
                }

                swapChainImageViews[i] = swapChainImageView;
            }

            return swapChainImageViews;
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeQueueSubmitSemaphore();
                DisposeAcquireNextImageSemaphore();
                DisposeFences();
                DisposeCommandBuffers();
                DisposeCommandPool();
                DisposeFrameBuffers();
                DisposeSwapChainImageViews();
                DisposeRenderPass();
                DisposeSwapChain();
                DisposeSurface();
                DisposeDevice();
            }

            _state.EndDispose();
        }

        private void DisposeAcquireNextImageSemaphore()
        {
            _state.AssertDisposing();

            if (_acquireNextImageSemaphore.IsCreated)
            {
                vkDestroySemaphore(_device.Value, (ulong)_acquireNextImageSemaphore.Value, pAllocator: null);
            }
        }

        private void DisposeCommandBuffers()
        {
            _state.AssertDisposing();

            if (_commandBuffers.IsCreated)
            {
                fixed (IntPtr* commandBuffers = _commandBuffers.Value)
                {
                    vkFreeCommandBuffers(_device.Value, (ulong)_commandPool.Value, (uint)_graphicsSurface.BufferCount, commandBuffers);
                }
            }
        }

        private void DisposeCommandPool()
        {
            _state.AssertDisposing();

            if (_commandPool.IsCreated)
            {
                vkDestroyCommandPool(_device.Value, (ulong)_commandPool.Value, pAllocator: null);
            }
        }

        private void DisposeDevice()
        {
            _state.AssertDisposing();

            if (_device.IsCreated)
            {
                vkDestroyDevice(_device.Value, pAllocator: null);
            }
        }

        private void DisposeFences()
        {
            _state.AssertDisposing();

            if (_fences.IsCreated)
            {
                var fences = _fences.Value;

                foreach (var fence in fences)
                {
                    if (fence != IntPtr.Zero)
                    {
                        vkDestroyFence(_device.Value, (ulong)fence, pAllocator: null);
                    }
                }
            }
        }

        private void DisposeFrameBuffers()
        {
            _state.AssertDisposing();

            if (_frameBuffers.IsCreated)
            {
                var frameBuffers = _frameBuffers.Value;

                foreach (var frameBuffer in frameBuffers)
                {
                    if (frameBuffer != IntPtr.Zero)
                    {
                        vkDestroyFramebuffer(_device.Value, (ulong)frameBuffer, pAllocator: null);
                    }
                }
            }
        }

        private void DisposeQueueSubmitSemaphore()
        {
            _state.AssertDisposing();

            if (_queueSubmitSemaphore.IsCreated)
            {
                vkDestroySemaphore(_device.Value, (ulong)_queueSubmitSemaphore.Value, pAllocator: null);
            }
        }

        private void DisposeRenderPass()
        {
            _state.AssertDisposing();

            if (_renderPass.IsCreated)
            {
                vkDestroyRenderPass(_device.Value, (ulong)_renderPass.Value, pAllocator: null);
            }
        }

        private void DisposeSurface()
        {
            _state.AssertDisposing();

            if (_surface.IsCreated)
            {
                var graphicsProvider = (GraphicsProvider)_graphicsAdapter.GraphicsProvider;
                vkDestroySurfaceKHR(graphicsProvider.Instance, _surface.Value, pAllocator: null);
            }
        }

        private void DisposeSwapChain()
        {
            _state.AssertDisposing();

            if (_swapChain.IsCreated)
            {
                vkDestroySwapchainKHR(_device.Value, _swapChain.Value, pAllocator: null);
            }
        }

        private void DisposeSwapChainImageViews()
        {
            _state.AssertDisposing();

            if (_swapChainImageViews.IsCreated)
            {
                var swapChainImageViews = _swapChainImageViews.Value;

                foreach (var swapChainImageView in swapChainImageViews)
                {
                    if (swapChainImageView != IntPtr.Zero)
                    {
                        vkDestroyImageView(_device.Value, (ulong)swapChainImageView, pAllocator: null);
                    }
                }
            }
        }

        private uint FindGraphicsQueueFamilyIndex()
        {
            var queueFamilyIndex = uint.MaxValue;

            uint queueFamilyPropertyCount;
            vkGetPhysicalDeviceQueueFamilyProperties(_graphicsAdapter.PhysicalDevice, &queueFamilyPropertyCount, pQueueFamilyProperties: null);

            var queueFamilyProperties = stackalloc VkQueueFamilyProperties[(int)queueFamilyPropertyCount];
            vkGetPhysicalDeviceQueueFamilyProperties(_graphicsAdapter.PhysicalDevice, &queueFamilyPropertyCount, queueFamilyProperties);

            for (var i = 0u; i < queueFamilyPropertyCount; i++)
            {
                if ((queueFamilyProperties[i].queueFlags & (uint)VK_QUEUE_GRAPHICS_BIT) != 0)
                {
                    queueFamilyIndex = i;
                    break;
                }
            }

            if (queueFamilyIndex == uint.MaxValue)
            {
                ThrowInvalidOperationException(nameof(queueFamilyIndex), queueFamilyIndex);
            }
            return queueFamilyIndex;
        }

        private void HandleGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> e)
        {
            var result = vkDeviceWaitIdle(Device);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkDeviceWaitIdle), (int)result);
            }

            if (_frameBuffers.IsCreated)
            {
                var frameBuffers = _frameBuffers.Value;

                foreach (var frameBuffer in frameBuffers)
                {
                    if (frameBuffer != IntPtr.Zero)
                    {
                        vkDestroyFramebuffer(_device.Value, (ulong)frameBuffer, pAllocator: null);
                    }
                }

                _frameBuffers.Reset();
            }

            if (_swapChainImageViews.IsCreated)
            {
                var swapChainImageViews = _swapChainImageViews.Value;

                foreach (var swapChainImageView in swapChainImageViews)
                {
                    if (swapChainImageView != IntPtr.Zero)
                    {
                        vkDestroyImageView(_device.Value, (ulong)swapChainImageView, pAllocator: null);
                    }
                }

                _swapChainImageViews.Reset();
            }

            if (_swapChain.IsCreated)
            {
                vkDestroySwapchainKHR(_device.Value, _swapChain.Value, pAllocator: null);
                _swapChain.Reset();
            }
        }
    }
}
