// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
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

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public sealed unsafe class VulkanGraphicsContext : IDisposable, GraphicsContext
    {
        private readonly VulkanGraphicsAdapter _graphicsAdapter;
        private readonly IGraphicsSurface _graphicsSurface;

        private ValueLazy<VkSemaphore> _acquireNextImageSemaphore;
        private ValueLazy<VkCommandBuffer[]> _commandBuffers;
        private ValueLazy<VkCommandPool> _commandPool;
        private ValueLazy<VkDevice> _device;
        private ValueLazy<VkQueue> _deviceQueue;
        private ValueLazy<VkFence[]> _fences;
        private ValueLazy<VkFramebuffer[]> _frameBuffers;
        private ValueLazy<uint> _graphicsQueueFamilyIndex;
        private ValueLazy<VkSemaphore> _queueSubmitSemaphore;
        private ValueLazy<VkRenderPass> _renderPass;
        private ValueLazy<VkSurfaceKHR> _surface;
        private ValueLazy<VkSwapchainKHR> _swapChain;
        private ValueLazy<VkImageView[]> _swapChainImageViews;

        private uint _frameIndex;
        private State _state;
        private VkFormat _swapChainFormat;

        internal VulkanGraphicsContext(VulkanGraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface)
        {
            _graphicsAdapter = graphicsAdapter;
            _graphicsSurface = graphicsSurface;

            _acquireNextImageSemaphore = new ValueLazy<VkSemaphore>(CreateAcquireNextImageSemaphore);
            _commandBuffers = new ValueLazy<VkCommandBuffer[]>(CreateCommandBuffers);
            _commandPool = new ValueLazy<VkCommandPool>(CreateCommandPool);
            _device = new ValueLazy<VkDevice>(CreateDevice);
            _deviceQueue = new ValueLazy<VkQueue>(CreateDeviceQueue);
            _fences = new ValueLazy<VkFence[]>(CreateFences);
            _frameBuffers = new ValueLazy<VkFramebuffer[]>(CreateFrameBuffers);
            _graphicsQueueFamilyIndex = new ValueLazy<uint>(FindGraphicsQueueFamilyIndex);
            _queueSubmitSemaphore = new ValueLazy<VkSemaphore>(CreateQueueSubmitSemaphore);
            _renderPass = new ValueLazy<VkRenderPass>(CreateRenderPass);
            _surface = new ValueLazy<VkSurfaceKHR>(CreateSurface);
            _swapChain = new ValueLazy<VkSwapchainKHR>(CreateSwapChain);
            _swapChainImageViews = new ValueLazy<VkImageView[]>(CreateSwapChainImageViews);

            _ = _state.Transition(to: Initialized);

            // Do event hookups after we are in the initialized state, since an event could
            // technically fire while the constructor is still running.

            _graphicsSurface.SizeChanged += HandleGraphicsSurfaceSizeChanged;
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsContext" /> class.</summary>
        ~VulkanGraphicsContext()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets a <c>vkSemaphore</c> for the <see cref="vkAcquireNextImageKHR(IntPtr, ulong, ulong, ulong, ulong, uint*)" /> method.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public ulong AcquireNextImageSemaphore
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _acquireNextImageSemaphore.Value;
            }
        }

        /// <summary>Gets an array of <c>VkCommandBuffer</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkCommandBuffer[] CommandBuffers
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _commandBuffers.Value;
            }
        }

        /// <summary>Gets the <c>VkCommandPool</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkCommandPool CommandPool
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _commandPool.Value;
            }
        }

        /// <summary>Gets the <c>VkDevice</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkDevice Device
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _device.Value;
            }
        }

        /// <summary>Gets the <c>VkQueue</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkQueue DeviceQueue
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _deviceQueue.Value;
            }
        }

        /// <summary>Gets an array of <c>VkFence</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkFence[] Fences
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _fences.Value;
            }
        }

        /// <summary>Gets an array of <c>VkFramebuffer</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkFramebuffer[] FrameBuffers
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _frameBuffers.Value;
            }
        }

        /// <inheritdoc />
        public GraphicsAdapter GraphicsAdapter => _graphicsAdapter;

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

        /// <inheritdoc />
        public IGraphicsSurface GraphicsSurface => _graphicsSurface;

        /// <summary>Gets a <c>vkSemaphore</c> for the <see cref="vkQueueSubmit(IntPtr, uint, VkSubmitInfo*, ulong)" /> method.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkSemaphore QueueSubmitSemaphore
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _queueSubmitSemaphore.Value;
            }
        }

        /// <summary>Gets the <c>VkRenderPass</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkRenderPass RenderPass
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _renderPass.Value;
            }
        }

        /// <summary>Gets the <c>VkSurfaceKHR</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkSurfaceKHR Surface
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _surface.Value;
            }
        }

        /// <summary>Gets the <c>VkSwapchainKHR</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkSwapchainKHR SwapChain
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _swapChain.Value;
            }
        }

        /// <summary>Gets an array of <c>VkImageView</c> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public VkImageView[] SwapChainImageViews
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _swapChainImageViews.Value;
            }
        }

        /// <inheritdoc />
        public void BeginFrame(ColorRgba backgroundColor)
        {
            uint frameIndex;
            ThrowExternalExceptionIfNotSuccess(nameof(vkAcquireNextImageKHR), vkAcquireNextImageKHR(Device, SwapChain, timeout: ulong.MaxValue, AcquireNextImageSemaphore, fence: VK_NULL_HANDLE, &frameIndex));
            _frameIndex = frameIndex;

            var fence = Fences[frameIndex];
            ThrowExternalExceptionIfNotSuccess(nameof(vkWaitForFences), vkWaitForFences(Device, fenceCount: 1, (ulong*)&fence, waitAll: VK_TRUE, timeout: ulong.MaxValue));
            ThrowExternalExceptionIfNotSuccess(nameof(vkResetFences), vkResetFences(Device, 1, (ulong*)&fence));

            var commandBufferBeginInfo = new VkCommandBufferBeginInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                pNext = null,
                flags = 0,
                pInheritanceInfo = null,
            };

            var commandBuffer = CommandBuffers[frameIndex];
            ThrowExternalExceptionIfNotSuccess(nameof(vkBeginCommandBuffer), vkBeginCommandBuffer(commandBuffer, &commandBufferBeginInfo));

            var clearValue = new VkClearValue {
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
                renderPass = RenderPass,
                framebuffer = FrameBuffers[frameIndex],
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

            var viewport = new VkViewport {
                x = 0.0f,
                y = 0.0f,
                width = _graphicsSurface.Width,
                height = _graphicsSurface.Height,
                minDepth = 0.0f,
                maxDepth = 1.0f,
            };
            vkCmdSetViewport(commandBuffer, firstViewport: 0, viewportCount: 1, &viewport);

            var scissorRect = new VkRect2D {
                offset = new VkOffset2D {
                    x = 0,
                    y = 0,
                },
                extent = new VkExtent2D {
                    width = (uint)_graphicsSurface.Width,
                    height = (uint)_graphicsSurface.Height,
                },
            };
            vkCmdSetScissor(commandBuffer, firstScissor: 0, scissorCount: 1, &scissorRect);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
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

            var waitSemaphore = AcquireNextImageSemaphore;
            var waitDstStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT;
            var signalSemaphore = QueueSubmitSemaphore;

            var submitInfo = new VkSubmitInfo {
                sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
                pNext = null,
                waitSemaphoreCount = 1,
                pWaitSemaphores = &waitSemaphore,
                pWaitDstStageMask = (uint*)&waitDstStageMask,
                commandBufferCount = 1,
                pCommandBuffers = (IntPtr*)&commandBuffer,
                signalSemaphoreCount = 1,
                pSignalSemaphores = (ulong*)&signalSemaphore
            };

            result = vkQueueSubmit(DeviceQueue, submitCount: 1, &submitInfo, fence: VK_NULL_HANDLE);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkQueueSubmit), (int)result);
            }
        }

        /// <inheritdoc />
        public void PresentFrame()
        {
            var frameIndex = _frameIndex;
            var waitSemaphore = QueueSubmitSemaphore;
            var swapChain = SwapChain;
            var signalSemaphore = QueueSubmitSemaphore;

            var presentInfo = new VkPresentInfoKHR {
                sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
                pNext = null,
                waitSemaphoreCount = 1,
                pWaitSemaphores = (ulong*)&waitSemaphore,
                swapchainCount = 1,
                pSwapchains = (ulong*)&swapChain,
                pImageIndices = &frameIndex,
                pResults = null,
            };

            var result = vkQueuePresentKHR(DeviceQueue, &presentInfo);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkQueuePresentKHR), (int)result);
            }

            result = vkQueueSubmit(DeviceQueue, submitCount: 0, pSubmits: null, Fences[frameIndex]);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkQueueSubmit), (int)result);
            }
        }

        private VkSemaphore CreateAcquireNextImageSemaphore()
        {
            VkSemaphore acquireNextImageSemaphore;

            var acquireNextImageSemaphoreCreateInfo = new VkSemaphoreCreateInfo {
                sType = VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO,
                pNext = null,
                flags = 0,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateSemaphore), vkCreateSemaphore(Device, &acquireNextImageSemaphoreCreateInfo, pAllocator: null, (ulong*)&acquireNextImageSemaphore));

            return acquireNextImageSemaphore;
        }

        private VkCommandBuffer[] CreateCommandBuffers()
        {
            var commandBuffers = new VkCommandBuffer[(uint)_graphicsSurface.BufferCount];

            var commandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                pNext = null,
                commandPool = CommandPool,
                level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandBufferCount = (uint)_graphicsSurface.BufferCount,
            };

            fixed (VkCommandBuffer* pCommandBuffers = commandBuffers)
            {
                ThrowExternalExceptionIfNotSuccess(nameof(vkAllocateCommandBuffers), vkAllocateCommandBuffers(Device, &commandBufferAllocateInfo, (IntPtr*)pCommandBuffers));
            }

            return commandBuffers;
        }

        private VkCommandPool CreateCommandPool()
        {
            VkCommandPool commandPool;

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                pNext = null,
                flags = (uint)VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
                queueFamilyIndex = GraphicsQueueFamilyIndex,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateCommandPool), vkCreateCommandPool(Device, &commandPoolCreateInfo, pAllocator: null, (ulong*)&commandPool));

            return commandPool;
        }

        private VkDevice CreateDevice()
        {
            VkDevice device;

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
            enabledExtensionNames[0] = (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(VK_KHR_SWAPCHAIN_EXTENSION_NAME));

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
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateDevice), vkCreateDevice(_graphicsAdapter.PhysicalDevice, &deviceCreateInfo, pAllocator: null, (IntPtr*)&device));

            return device;
        }

        private VkQueue CreateDeviceQueue()
        {
            VkQueue deviceQueue;
            vkGetDeviceQueue(Device, GraphicsQueueFamilyIndex, queueIndex: 0, (IntPtr*)&deviceQueue);
            return deviceQueue;
        }

        private VkFence[] CreateFences()
        {
            var fences = new VkFence[(uint)_graphicsSurface.BufferCount];

            var fenceCreateInfo = new VkFenceCreateInfo {
                sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
                pNext = null,
                flags = (uint)VK_FENCE_CREATE_SIGNALED_BIT,
            };

            for (var i = 0; i < fences.Length; i++)
            {
                VkFence fence;
                ThrowExternalExceptionIfNotSuccess(nameof(vkCreateFence), vkCreateFence(Device, &fenceCreateInfo, pAllocator: null, (ulong*)&fence));
                fences[i] = fence;
            }

            return fences;
        }

        private VkFramebuffer[] CreateFrameBuffers()
        {
            var frameBuffers = new VkFramebuffer[(uint)_graphicsSurface.BufferCount];
            var attachments = stackalloc ulong[1];

            var frameBufferCreateInfo = new VkFramebufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                pNext = null,
                flags = 0,
                renderPass = RenderPass,
                attachmentCount = 1,
                pAttachments = attachments,
                width = (uint)_graphicsSurface.Width,
                height = (uint)_graphicsSurface.Height,
                layers = 1,
            };

            for (var i = 0; i < frameBuffers.Length; i++)
            {
                attachments[0] = SwapChainImageViews[i];

                VkFramebuffer frameBuffer;
                ThrowExternalExceptionIfNotSuccess(nameof(vkCreateFramebuffer), vkCreateFramebuffer(Device, &frameBufferCreateInfo, pAllocator: null, (ulong*)&frameBuffer));
                frameBuffers[i] = frameBuffer;
            }

            return frameBuffers;
        }

        private VkSemaphore CreateQueueSubmitSemaphore()
        {
            VkSemaphore queueSubmitSemaphore;

            var queueSubmitSemaphoreCreateInfo = new VkSemaphoreCreateInfo {
                sType = VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO,
                pNext = null,
                flags = 0,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateSemaphore), vkCreateSemaphore(Device, &queueSubmitSemaphoreCreateInfo, pAllocator: null, (ulong*)&queueSubmitSemaphore));

            return queueSubmitSemaphore;
        }

        private VkRenderPass CreateRenderPass()
        {
            VkRenderPass renderPass;

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
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateRenderPass), vkCreateRenderPass(Device, &renderPassCreateInfo, pAllocator: null, (ulong*)&renderPass));

            return renderPass;
        }

        private VkSurfaceKHR CreateSurface()
        {
            VkSurfaceKHR surface;

            var graphicsProvider = (VulkanGraphicsProvider)_graphicsAdapter.GraphicsProvider;

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

                    ThrowExternalExceptionIfNotSuccess(nameof(vkCreateWin32SurfaceKHR), vkCreateWin32SurfaceKHR(graphicsProvider.Instance, &surfaceCreateInfo, pAllocator: null, (ulong*)&surface));
                    break;
                }

                case GraphicsSurfaceKind.Xlib:
                {
                    var surfaceCreateInfo = new VkXlibSurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR,
                        pNext = null,
                        flags = 0,
                        dpy = (UIntPtr)(void*)_graphicsSurface.DisplayHandle,
                        window = (UIntPtr)(void*)_graphicsSurface.WindowHandle,
                    };

                    ThrowExternalExceptionIfNotSuccess(nameof(vkCreateXlibSurfaceKHR), vkCreateXlibSurfaceKHR(graphicsProvider.Instance, &surfaceCreateInfo, pAllocator: null, (ulong*)&surface));
                    break;
                }

                default:
                {
                    ThrowArgumentOutOfRangeException(nameof(_graphicsSurface), _graphicsSurface);
                    surface = VK_NULL_HANDLE;
                    break;
                }
            }

            uint supported;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceSupportKHR), vkGetPhysicalDeviceSurfaceSupportKHR(_graphicsAdapter.PhysicalDevice, GraphicsQueueFamilyIndex, surface, &supported));

            if (supported == VK_FALSE)
            {
                ThrowArgumentOutOfRangeException(nameof(_graphicsSurface), _graphicsSurface);
            }
            return surface;
        }

        private VkSwapchainKHR CreateSwapChain()
        {
            VkSwapchainKHR swapChain;

            VkSurfaceCapabilitiesKHR surfaceCapabilities;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceCapabilitiesKHR), vkGetPhysicalDeviceSurfaceCapabilitiesKHR(_graphicsAdapter.PhysicalDevice, Surface, &surfaceCapabilities));

            uint presentModeCount;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), vkGetPhysicalDeviceSurfacePresentModesKHR(_graphicsAdapter.PhysicalDevice, Surface, &presentModeCount, pPresentModes: null));

            var presentModes = stackalloc VkPresentModeKHR[(int)presentModeCount];
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), vkGetPhysicalDeviceSurfacePresentModesKHR(_graphicsAdapter.PhysicalDevice, Surface, &presentModeCount, presentModes));

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
                imageUsage = (uint)(VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT),
                imageSharingMode = VK_SHARING_MODE_EXCLUSIVE,
                queueFamilyIndexCount = 0,
                pQueueFamilyIndices = null,
                preTransform = VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR,
                compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
                presentMode = VK_PRESENT_MODE_FIFO_KHR,
                clipped = VK_TRUE,
                oldSwapchain = VK_NULL_HANDLE,
            };

            if ((surfaceCapabilities.supportedTransforms & (uint)VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR) == 0)
            {
                swapChainCreateInfo.preTransform = surfaceCapabilities.currentTransform;
            }

            uint surfaceFormatCount;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceFormatsKHR), vkGetPhysicalDeviceSurfaceFormatsKHR(_graphicsAdapter.PhysicalDevice, Surface, &surfaceFormatCount, pSurfaceFormats: null));

            var surfaceFormats = stackalloc VkSurfaceFormatKHR[(int)surfaceFormatCount];
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceFormatsKHR), vkGetPhysicalDeviceSurfaceFormatsKHR(_graphicsAdapter.PhysicalDevice, Surface, &surfaceFormatCount, surfaceFormats));

            for (var i = 0u; i <surfaceFormatCount; i++)
            {
                if (surfaceFormats[i].format == VK_FORMAT_R8G8B8A8_SRGB)
                {
                    swapChainCreateInfo.imageFormat = surfaceFormats[i].format;
                    swapChainCreateInfo.imageColorSpace = surfaceFormats[i].colorSpace;
                    break;
                }
            }
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateSwapchainKHR), vkCreateSwapchainKHR(Device, &swapChainCreateInfo, pAllocator: null, (ulong*)&swapChain));

            _swapChainFormat = swapChainCreateInfo.imageFormat;
            return swapChain;
        }

        private VkImageView[] CreateSwapChainImageViews()
        {
            var swapChainImageCount = (uint)_graphicsSurface.BufferCount;
            var swapChainImages = stackalloc VkImage[(int)swapChainImageCount];

            ThrowExternalExceptionIfNotSuccess(nameof(vkGetSwapchainImagesKHR), vkGetSwapchainImagesKHR(Device, SwapChain, &swapChainImageCount, (ulong*)swapChainImages));

            var swapChainImageViews = new VkImageView[swapChainImageCount];

            var swapChainImageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                pNext = null,
                flags = 0,
                image = VK_NULL_HANDLE,
                viewType = VK_IMAGE_VIEW_TYPE_2D,
                format = _swapChainFormat,
                components = new VkComponentMapping {
                    r = VK_COMPONENT_SWIZZLE_R,
                    g = VK_COMPONENT_SWIZZLE_G,
                    b = VK_COMPONENT_SWIZZLE_B,
                    a = VK_COMPONENT_SWIZZLE_A,
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
                swapChainImageViewCreateInfo.image = swapChainImages[i];

                VkImageView swapChainImageView;
                ThrowExternalExceptionIfNotSuccess(nameof(vkCreateImageView), vkCreateImageView(Device, &swapChainImageViewCreateInfo, pAllocator: null, (ulong*)&swapChainImageView));

                swapChainImageViews[i] = swapChainImageView;
            }

            return swapChainImageViews;
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                WaitForIdle();
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
                vkDestroySemaphore(_device.Value, _acquireNextImageSemaphore.Value, pAllocator: null);
            }
        }

        private void DisposeCommandBuffers()
        {
            _state.AssertDisposing();

            if (_commandBuffers.IsCreated)
            {
                fixed (VkCommandBuffer* commandBuffers = _commandBuffers.Value)
                {
                    vkFreeCommandBuffers(_device.Value, _commandPool.Value, (uint)_graphicsSurface.BufferCount, (IntPtr*)commandBuffers);
                }
            }
        }

        private void DisposeCommandPool()
        {
            _state.AssertDisposing();

            if (_commandPool.IsCreated)
            {
                vkDestroyCommandPool(_device.Value, _commandPool.Value, pAllocator: null);
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
                    if (fence != VK_NULL_HANDLE)
                    {
                        vkDestroyFence(_device.Value, fence, pAllocator: null);
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
                    if (frameBuffer != VK_NULL_HANDLE)
                    {
                        vkDestroyFramebuffer(_device.Value, frameBuffer, pAllocator: null);
                    }
                }
            }
        }

        private void DisposeQueueSubmitSemaphore()
        {
            _state.AssertDisposing();

            if (_queueSubmitSemaphore.IsCreated)
            {
                vkDestroySemaphore(_device.Value, _queueSubmitSemaphore.Value, pAllocator: null);
            }
        }

        private void DisposeRenderPass()
        {
            _state.AssertDisposing();

            if (_renderPass.IsCreated)
            {
                vkDestroyRenderPass(_device.Value, _renderPass.Value, pAllocator: null);
            }
        }

        private void DisposeSurface()
        {
            _state.AssertDisposing();

            if (_surface.IsCreated)
            {
                var graphicsProvider = (VulkanGraphicsProvider)_graphicsAdapter.GraphicsProvider;
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
                    if (swapChainImageView != VK_NULL_HANDLE)
                    {
                        vkDestroyImageView(_device.Value, swapChainImageView, pAllocator: null);
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
            WaitForIdle();

            if (_frameBuffers.IsCreated)
            {
                var frameBuffers = _frameBuffers.Value;

                foreach (var frameBuffer in frameBuffers)
                {
                    if (frameBuffer != VK_NULL_HANDLE)
                    {
                        vkDestroyFramebuffer(_device.Value, frameBuffer, pAllocator: null);
                    }
                }

                _frameBuffers.Reset(CreateFrameBuffers);
            }

            if (_swapChainImageViews.IsCreated)
            {
                var swapChainImageViews = _swapChainImageViews.Value;

                foreach (var swapChainImageView in swapChainImageViews)
                {
                    if (swapChainImageView != VK_NULL_HANDLE)
                    {
                        vkDestroyImageView(_device.Value, swapChainImageView, pAllocator: null);
                    }
                }

                _swapChainImageViews.Reset(CreateSwapChainImageViews);
            }

            if (_swapChain.IsCreated)
            {
                vkDestroySwapchainKHR(_device.Value, _swapChain.Value, pAllocator: null);
                _swapChain.Reset(CreateSwapChain);
            }
        }

        private void WaitForIdle()
        {
            if (_device.IsCreated)
            {
                var result = vkDeviceWaitIdle(_device.Value);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkDeviceWaitIdle), (int)result);
                }
            }
        }
    }
}
