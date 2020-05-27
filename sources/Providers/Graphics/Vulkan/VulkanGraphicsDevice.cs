// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkAttachmentLoadOp;
using static TerraFX.Interop.VkAttachmentStoreOp;
using static TerraFX.Interop.VkCompositeAlphaFlagBitsKHR;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageLayout;
using static TerraFX.Interop.VkImageUsageFlagBits;
using static TerraFX.Interop.VkPipelineBindPoint;
using static TerraFX.Interop.VkPresentModeKHR;
using static TerraFX.Interop.VkQueueFlagBits;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkSampleCountFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkSurfaceTransformFlagBitsKHR;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsDevice : GraphicsDevice
    {
        private readonly VulkanGraphicsFence _presentCompletionGraphicsFence;

        private ValueLazy<VkQueue> _vulkanCommandQueue;
        private ValueLazy<uint> _vulkanCommandQueueFamilyIndex;
        private ValueLazy<VkDevice> _vulkanDevice;
        private ValueLazy<VkRenderPass> _vulkanRenderPass;
        private ValueLazy<VkSurfaceKHR> _vulkanSurface;
        private ValueLazy<VkSwapchainKHR> _vulkanSwapchain;
        private ValueLazy<VkImage[]> _vulkanSwapchainImages;

        private VulkanGraphicsContext[] _graphicsContexts;
        private int _graphicsContextIndex;
        private VkFormat _vulkanSwapchainFormat;

        private State _state;

        internal VulkanGraphicsDevice(VulkanGraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface, int graphicsContextCount)
            : base(graphicsAdapter, graphicsSurface)
        {
            _presentCompletionGraphicsFence = new VulkanGraphicsFence(this);

            _vulkanCommandQueue = new ValueLazy<VkQueue>(GetVulkanCommandQueue);
            _vulkanCommandQueueFamilyIndex = new ValueLazy<uint>(GetVulkanCommandQueueFamilyIndex);
            _vulkanDevice = new ValueLazy<VkDevice>(CreateVulkanDevice);
            _vulkanRenderPass = new ValueLazy<VkRenderPass>(CreateVulkanRenderPass);
            _vulkanSurface = new ValueLazy<VkSurfaceKHR>(CreateVulkanSurface);
            _vulkanSwapchain = new ValueLazy<VkSwapchainKHR>(CreateVulkanSwapchain);
            _vulkanSwapchainImages = new ValueLazy<VkImage[]>(GetVulkanSwapchainImages);

            _graphicsContexts = CreateGraphicsContexts(this, graphicsContextCount);

            _ = _state.Transition(to: Initialized);

            PresentCompletionGraphicsFence.Reset();
            graphicsSurface.SizeChanged += OnGraphicsSurfaceSizeChanged;

            static VulkanGraphicsContext[] CreateGraphicsContexts(VulkanGraphicsDevice graphicsDevice, int graphicsContextCount)
            {
                var graphicsContexts = new VulkanGraphicsContext[graphicsContextCount];

                for (var index = 0; index < graphicsContexts.Length; index++)
                {
                    graphicsContexts[index] = new VulkanGraphicsContext(graphicsDevice, index);
                }

                return graphicsContexts;
            }
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsDevice" /> class.</summary>
        ~VulkanGraphicsDevice()
        {
            Dispose(isDisposing: true);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<GraphicsContext> GraphicsContexts => _graphicsContexts;

        /// <inheritdoc />
        public override int GraphicsContextIndex => _graphicsContextIndex;

        /// <summary>Gets a graphics fence that is used to wait for <see cref="PresentFrame" /> to complete.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VulkanGraphicsFence PresentCompletionGraphicsFence => _presentCompletionGraphicsFence;

        /// <summary>Gets the <see cref="VkQueue" /> used by the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VkQueue VulkanCommandQueue => _vulkanCommandQueue.Value;

        /// <summary>Gets the index of the queue family for <see cref="VulkanCommandQueue" />.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public uint VulkanCommandQueueFamilyIndex => _vulkanCommandQueueFamilyIndex.Value;

        /// <inheritdoc cref="GraphicsDevice.CurrentGraphicsContext" />
        public VulkanGraphicsContext VulkanCurrentGraphicsContext => (VulkanGraphicsContext)CurrentGraphicsContext;

        /// <summary>Gets the underlying <see cref="VkDevice"/> for the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VkDevice VulkanDevice => _vulkanDevice.Value;

        /// <inheritdoc cref="GraphicsDevice.GraphicsAdapter" />
        public VulkanGraphicsAdapter VulkanGraphicsAdapter => (VulkanGraphicsAdapter)GraphicsAdapter;

        /// <inheritdoc cref="GraphicsContexts" />
        public ReadOnlySpan<VulkanGraphicsContext> VulkanGraphicsContexts => _graphicsContexts;

        /// <summary>Gets the <see cref="VkRenderPass" /> used by the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VkRenderPass VulkanRenderPass => _vulkanRenderPass.Value;

        /// <summary>Gets the <see cref="VkSurfaceKHR" /> used by the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VkSurfaceKHR VulkanSurface => _vulkanSurface.Value;

        /// <summary>Gets the <see cref="VkSwapchainKHR" /> used by the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VkSwapchainKHR VulkanSwapchain => _vulkanSwapchain.Value;

        /// <summary>Gets the <see cref="VkFormat" /> used by <see cref="VulkanSwapchain" />.</summary>
        public VkFormat VulkanSwapchainFormat => _vulkanSwapchainFormat;

        /// <summary>Gets a readonly span of the <see cref="VkImage" /> used by <see cref="VulkanSwapchain" />.</summary>
        public ReadOnlySpan<VkImage> VulkanSwapchainImages => _vulkanSwapchainImages.Value;

        /// <inheritdoc cref="CreateGraphicsHeap(ulong, GraphicsHeapCpuAccess)" />
        public VulkanGraphicsHeap CreateVulkanGraphicsHeap(ulong size, GraphicsHeapCpuAccess cpuAccess)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new VulkanGraphicsHeap(this, size, cpuAccess);
        }

        /// <inheritdoc cref="CreateGraphicsPipeline(GraphicsPipelineSignature, GraphicsShader?, GraphicsShader?)" />
        public VulkanGraphicsPipeline CreateVulkanGraphicsPipeline(VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader = null, VulkanGraphicsShader? pixelShader = null)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new VulkanGraphicsPipeline(this, signature, vertexShader, pixelShader);
        }

        /// <inheritdoc cref="CreateGraphicsPrimitive(GraphicsPipeline, GraphicsBuffer, GraphicsBuffer, ReadOnlySpan{GraphicsResource})" />
        public VulkanGraphicsPrimitive CreateVulkanGraphicsPrimitive(VulkanGraphicsPipeline graphicsPipeline, VulkanGraphicsBuffer vertexBuffer, VulkanGraphicsBuffer? indexBuffer = null, ReadOnlySpan<GraphicsResource> inputResource = default)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new VulkanGraphicsPrimitive(this, graphicsPipeline, vertexBuffer, indexBuffer, inputResource);
        }

        /// <inheritdoc cref="CreateGraphicsShader(GraphicsShaderKind, ReadOnlySpan{byte}, string)" />
        public VulkanGraphicsShader CreateVulkanGraphicsShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new VulkanGraphicsShader(this, kind, bytecode, entryPointName);
        }

        /// <inheritdoc />
        public override GraphicsHeap CreateGraphicsHeap(ulong size, GraphicsHeapCpuAccess cpuAccess) => CreateVulkanGraphicsHeap(size, cpuAccess);

        /// <inheritdoc />
        public override GraphicsPipeline CreateGraphicsPipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null) => CreateVulkanGraphicsPipeline((VulkanGraphicsPipelineSignature)signature, (VulkanGraphicsShader?)vertexShader, (VulkanGraphicsShader?)pixelShader);

        /// <inheritdoc />
        public override GraphicsPipelineSignature CreateGraphicsPipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new VulkanGraphicsPipelineSignature(this, inputs, resources);
        }

        /// <inheritdoc />
        public override GraphicsPrimitive CreateGraphicsPrimitive(GraphicsPipeline graphicsPipeline, GraphicsBuffer vertexBuffer, GraphicsBuffer? indexBuffer = null, ReadOnlySpan<GraphicsResource> inputResources = default) => CreateVulkanGraphicsPrimitive((VulkanGraphicsPipeline)graphicsPipeline, (VulkanGraphicsBuffer)vertexBuffer, (VulkanGraphicsBuffer?)indexBuffer, inputResources);

        /// <inheritdoc />
        public override GraphicsShader CreateGraphicsShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName) => CreateVulkanGraphicsShader(kind, bytecode, entryPointName);

        /// <inheritdoc />
        public override void PresentFrame()
        {
            var graphicsContextIndex = GraphicsContextIndex;
            var vulkanSwapchain = VulkanSwapchain;

            var presentInfo = new VkPresentInfoKHR {
                sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
                swapchainCount = 1,
                pSwapchains = (ulong*)&vulkanSwapchain,
                pImageIndices = (uint*)&graphicsContextIndex,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkQueuePresentKHR), vkQueuePresentKHR(VulkanCommandQueue, &presentInfo));

            Signal(VulkanCurrentGraphicsContext.VulkanGraphicsFence);

            var presentCompletionGraphicsFence = PresentCompletionGraphicsFence;
            ThrowExternalExceptionIfNotSuccess(nameof(vkAcquireNextImageKHR), vkAcquireNextImageKHR(VulkanDevice, vulkanSwapchain, timeout: ulong.MaxValue, semaphore: VK_NULL_HANDLE, presentCompletionGraphicsFence.VulkanFence, (uint*)&graphicsContextIndex));

            presentCompletionGraphicsFence.Wait();
            presentCompletionGraphicsFence.Reset();

            _graphicsContextIndex = graphicsContextIndex;
        }

        /// <inheritdoc cref="Signal(GraphicsFence)" />
        public void Signal(VulkanGraphicsFence graphicsFence)
            => ThrowExternalExceptionIfNotSuccess(nameof(vkQueueSubmit), vkQueueSubmit(VulkanCommandQueue, submitCount: 0, pSubmits: null, graphicsFence.VulkanFence));

        /// <inheritdoc />
        public override void Signal(GraphicsFence graphicsFence) => Signal((VulkanGraphicsFence)graphicsFence);

        /// <inheritdoc />
        public override void WaitForIdle()
        {
            if (_vulkanDevice.IsCreated)
            {
                var result = vkDeviceWaitIdle(_vulkanDevice.Value);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkDeviceWaitIdle), (int)result);
                }
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                WaitForIdle();
                DisposeIfNotNull(_graphicsContexts);

                _vulkanRenderPass.Dispose(DisposeVulkanRenderPass);
                _vulkanSwapchain.Dispose(DisposeVulkanSwapchain);
                _vulkanSurface.Dispose(DisposeVulkanSurface);

                DisposeIfNotNull(_presentCompletionGraphicsFence);

                _vulkanDevice.Dispose(DisposeVulkanDevice);
            }

            _state.EndDispose();
        }

        private VkDevice CreateVulkanDevice()
        {
            VkDevice vulkanDevice;

            var queuePriority = 1.0f;

            var deviceQueueCreateInfo = new VkDeviceQueueCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                queueFamilyIndex = VulkanCommandQueueFamilyIndex,
                queueCount = 1,
                pQueuePriorities = &queuePriority,
            };

            const int enabledExtensionNamesCount = 1;

            var enabledExtensionNames = stackalloc sbyte*[enabledExtensionNamesCount] {
                VK_KHR_SWAPCHAIN_EXTENSION_NAME.AsPointer(),
            };

            var physicalDeviceFeatures = new VkPhysicalDeviceFeatures();

            var deviceCreateInfo = new VkDeviceCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
                queueCreateInfoCount = 1,
                pQueueCreateInfos = &deviceQueueCreateInfo,
                enabledExtensionCount = enabledExtensionNamesCount,
                ppEnabledExtensionNames = enabledExtensionNames,
                pEnabledFeatures = &physicalDeviceFeatures,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateDevice), vkCreateDevice(VulkanGraphicsAdapter.VulkanPhysicalDevice, &deviceCreateInfo, pAllocator: null, (IntPtr*)&vulkanDevice));

            return vulkanDevice;
        }

        private VkRenderPass CreateVulkanRenderPass()
        {
            VkRenderPass vulkanRenderPass;

            // The swap chain needs to be created first to ensure we know the format
            _ = VulkanSwapchain;

            var attachmentDescription = new VkAttachmentDescription {
                format = _vulkanSwapchainFormat,
                samples = VK_SAMPLE_COUNT_1_BIT,
                loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
                stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
                finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
            };

            var colorAttachmentReference = new VkAttachmentReference {
                layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
            };

            var subpass = new VkSubpassDescription {
                pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
                colorAttachmentCount = 1,
                pColorAttachments = &colorAttachmentReference,
            };

            var renderPassCreateInfo = new VkRenderPassCreateInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                attachmentCount = 1,
                pAttachments = &attachmentDescription,
                subpassCount = 1,
                pSubpasses = &subpass,
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateRenderPass), vkCreateRenderPass(VulkanDevice, &renderPassCreateInfo, pAllocator: null, (ulong*)&vulkanRenderPass));

            return vulkanRenderPass;
        }

        private VkSurfaceKHR CreateVulkanSurface()
        {
            VkSurfaceKHR vulkanSurface;

            var graphicsAdapter = VulkanGraphicsAdapter;
            var vulkanInstance = graphicsAdapter.VulkanGraphicsProvider.VulkanInstance;

            switch (GraphicsSurface.SurfaceKind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                        hinstance = GraphicsSurface.SurfaceContextHandle,
                        hwnd = GraphicsSurface.SurfaceHandle,
                    };

                    ThrowExternalExceptionIfNotSuccess(nameof(vkCreateWin32SurfaceKHR), vkCreateWin32SurfaceKHR(vulkanInstance, &surfaceCreateInfo, pAllocator: null, (ulong*)&vulkanSurface));
                    break;
                }

                case GraphicsSurfaceKind.Xlib:
                {
                    var surfaceCreateInfo = new VkXlibSurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR,
                        dpy = (UIntPtr)(void*)GraphicsSurface.SurfaceContextHandle,
                        window = (UIntPtr)(void*)GraphicsSurface.SurfaceHandle,
                    };

                    ThrowExternalExceptionIfNotSuccess(nameof(vkCreateXlibSurfaceKHR), vkCreateXlibSurfaceKHR(vulkanInstance, &surfaceCreateInfo, pAllocator: null, (ulong*)&vulkanSurface));
                    break;
                }

                default:
                {
                    ThrowArgumentOutOfRangeException(nameof(GraphicsSurface), GraphicsSurface);
                    vulkanSurface = VK_NULL_HANDLE;
                    break;
                }
            }

            uint supported;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceSupportKHR), vkGetPhysicalDeviceSurfaceSupportKHR(graphicsAdapter.VulkanPhysicalDevice, VulkanCommandQueueFamilyIndex, vulkanSurface, &supported));

            if (supported == VK_FALSE)
            {
                ThrowArgumentOutOfRangeException(nameof(GraphicsSurface), GraphicsSurface);
            }
            return vulkanSurface;
        }

        private VkSwapchainKHR CreateVulkanSwapchain()
        {
            VkSwapchainKHR vulkanSwapchain;

            var vulkanPhysicalDevice = VulkanGraphicsAdapter.VulkanPhysicalDevice;
            var vulkanSurface = VulkanSurface;

            VkSurfaceCapabilitiesKHR surfaceCapabilities;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceCapabilitiesKHR), vkGetPhysicalDeviceSurfaceCapabilitiesKHR(vulkanPhysicalDevice, vulkanSurface, &surfaceCapabilities));

            uint presentModeCount;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), vkGetPhysicalDeviceSurfacePresentModesKHR(vulkanPhysicalDevice, vulkanSurface, &presentModeCount, pPresentModes: null));

            var presentModes = stackalloc VkPresentModeKHR[(int)presentModeCount];
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), vkGetPhysicalDeviceSurfacePresentModesKHR(vulkanPhysicalDevice, vulkanSurface, &presentModeCount, presentModes));

            var graphicsSurface = GraphicsSurface;
            var graphicsContextsCount = unchecked((uint)VulkanGraphicsContexts.Length);

            if ((graphicsContextsCount < surfaceCapabilities.minImageCount) || ((surfaceCapabilities.maxImageCount != 0) && (graphicsContextsCount > surfaceCapabilities.maxImageCount)))
            {
                ThrowArgumentOutOfRangeException(nameof(graphicsContextsCount), graphicsContextsCount);
            }

            var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
                sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
                surface = vulkanSurface,
                minImageCount = graphicsContextsCount,
                imageExtent = new VkExtent2D {
                    width = (uint)graphicsSurface.Width,
                    height = (uint)graphicsSurface.Height,
                },
                imageArrayLayers = 1,
                imageUsage = (uint)(VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT),
                preTransform = VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR,
                compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
                presentMode = VK_PRESENT_MODE_FIFO_KHR,
                clipped = VK_TRUE,
            };

            if ((surfaceCapabilities.supportedTransforms & (uint)VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR) == 0)
            {
                swapChainCreateInfo.preTransform = surfaceCapabilities.currentTransform;
            }

            uint surfaceFormatCount;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceFormatsKHR), vkGetPhysicalDeviceSurfaceFormatsKHR(vulkanPhysicalDevice, VulkanSurface, &surfaceFormatCount, pSurfaceFormats: null));

            var surfaceFormats = stackalloc VkSurfaceFormatKHR[(int)surfaceFormatCount];
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetPhysicalDeviceSurfaceFormatsKHR), vkGetPhysicalDeviceSurfaceFormatsKHR(vulkanPhysicalDevice, VulkanSurface, &surfaceFormatCount, surfaceFormats));

            for (uint i = 0; i < surfaceFormatCount; i++)
            {
                if (surfaceFormats[i].format == VK_FORMAT_R8G8B8A8_UNORM)
                {
                    swapChainCreateInfo.imageFormat = surfaceFormats[i].format;
                    swapChainCreateInfo.imageColorSpace = surfaceFormats[i].colorSpace;
                    break;
                }
            }
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreateSwapchainKHR), vkCreateSwapchainKHR(VulkanDevice, &swapChainCreateInfo, pAllocator: null, (ulong*)&vulkanSwapchain));

            _vulkanSwapchainFormat = swapChainCreateInfo.imageFormat;
            return vulkanSwapchain;
        }

        private void DisposeVulkanDevice(VkDevice vulkanDevice)
        {
            _state.AssertDisposing();

            if (vulkanDevice != null)
            {
                vkDestroyDevice(vulkanDevice, pAllocator: null);
            }
        }

        private void DisposeVulkanRenderPass(VkRenderPass vulkanRenderPass)
        {
            _state.AssertDisposing();

            if (vulkanRenderPass != VK_NULL_HANDLE)
            {
                vkDestroyRenderPass(VulkanDevice, vulkanRenderPass, pAllocator: null);
            }
        }

        private void DisposeVulkanSurface(VkSurfaceKHR vulkanSurface)
        {
            _state.AssertDisposing();

            if (vulkanSurface != VK_NULL_HANDLE)
            {
                vkDestroySurfaceKHR(VulkanGraphicsAdapter.VulkanGraphicsProvider.VulkanInstance, vulkanSurface, pAllocator: null);
            }
        }

        private void DisposeVulkanSwapchain(VkSwapchainKHR vulkanSwapchain)
        {
            _state.AssertDisposing();

            if (vulkanSwapchain != VK_NULL_HANDLE)
            {
                vkDestroySwapchainKHR(VulkanDevice, vulkanSwapchain, pAllocator: null);
            }
        }

        private VkQueue GetVulkanCommandQueue()
        {
            VkQueue vulkanCommandQueue;
            vkGetDeviceQueue(VulkanDevice, VulkanCommandQueueFamilyIndex, queueIndex: 0, (IntPtr*)&vulkanCommandQueue);
            return vulkanCommandQueue;
        }

        private uint GetVulkanCommandQueueFamilyIndex()
        {
            var vulkanCommandQueueFamilyIndex = uint.MaxValue;

            var vulkanPhysicalDevice = VulkanGraphicsAdapter.VulkanPhysicalDevice;

            uint queueFamilyPropertyCount;
            vkGetPhysicalDeviceQueueFamilyProperties(vulkanPhysicalDevice, &queueFamilyPropertyCount, pQueueFamilyProperties: null);

            var queueFamilyProperties = stackalloc VkQueueFamilyProperties[(int)queueFamilyPropertyCount];
            vkGetPhysicalDeviceQueueFamilyProperties(vulkanPhysicalDevice, &queueFamilyPropertyCount, queueFamilyProperties);

            for (uint i = 0; i < queueFamilyPropertyCount; i++)
            {
                if ((queueFamilyProperties[i].queueFlags & (uint)VK_QUEUE_GRAPHICS_BIT) != 0)
                {
                    vulkanCommandQueueFamilyIndex = i;
                    break;
                }
            }

            if (vulkanCommandQueueFamilyIndex == uint.MaxValue)
            {
                ThrowInvalidOperationException(nameof(vulkanCommandQueueFamilyIndex), vulkanCommandQueueFamilyIndex);
            }
            return vulkanCommandQueueFamilyIndex;
        }

        private VkImage[] GetVulkanSwapchainImages()
        {
            var vulkanDevice = VulkanDevice;
            var vulkanSwapchain = VulkanSwapchain;

            uint swapchainImageCount;
            ThrowExternalExceptionIfNotSuccess(nameof(vkGetSwapchainImagesKHR), vkGetSwapchainImagesKHR(vulkanDevice, vulkanSwapchain, &swapchainImageCount, pSwapchainImages: null));

            var vulkanSwapchainImages = new VkImage[swapchainImageCount];

            fixed (VkImage* pVulkanSwapchainImages = vulkanSwapchainImages)
            {
                ThrowExternalExceptionIfNotSuccess(nameof(vkGetSwapchainImagesKHR), vkGetSwapchainImagesKHR(vulkanDevice, vulkanSwapchain, &swapchainImageCount, (ulong*)pVulkanSwapchainImages));
            }

            var presentCompletionGraphicsFence = PresentCompletionGraphicsFence;

            int graphicsContextIndex;
            ThrowExternalExceptionIfNotSuccess(nameof(vkAcquireNextImageKHR), vkAcquireNextImageKHR(VulkanDevice, vulkanSwapchain, timeout: ulong.MaxValue, semaphore: VK_NULL_HANDLE, presentCompletionGraphicsFence.VulkanFence, (uint*)&graphicsContextIndex));
            _graphicsContextIndex = graphicsContextIndex;

            presentCompletionGraphicsFence.Wait();
            presentCompletionGraphicsFence.Reset();

            return vulkanSwapchainImages;
        }

        private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
        {
            WaitForIdle();

            if (_vulkanSwapchainImages.IsCreated)
            {
                _vulkanSwapchainImages.Reset(GetVulkanSwapchainImages);
            }

            if (_vulkanSwapchain.IsCreated)
            {
                vkDestroySwapchainKHR(_vulkanDevice.Value, _vulkanSwapchain.Value, pAllocator: null);
                _vulkanSwapchain.Reset(CreateVulkanSwapchain);
            }

            foreach (var graphicsContext in VulkanGraphicsContexts)
            {
                graphicsContext.OnGraphicsSurfaceSizeChanged(sender, eventArgs);
            }
        }
    }
}
