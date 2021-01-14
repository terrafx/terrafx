// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Threading;
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
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsDevice : GraphicsDevice
    {
        private readonly VulkanGraphicsContext[] _contexts;
        private readonly VulkanGraphicsFence _presentCompletionFence;

        private ValueLazy<VkQueue> _vulkanCommandQueue;
        private ValueLazy<uint> _vulkanCommandQueueFamilyIndex;
        private ValueLazy<VkDevice> _vulkanDevice;
        private ValueLazy<VkRenderPass> _vulkanRenderPass;
        private ValueLazy<VkSurfaceKHR> _vulkanSurface;
        private ValueLazy<VkSwapchainKHR> _vulkanSwapchain;
        private ValueLazy<VkImage[]> _vulkanSwapchainImages;
        private ValueLazy<VulkanGraphicsMemoryAllocator> _memoryAllocator;

        private int _contextIndex;
        private VkFormat _vulkanSwapchainFormat;

        private VolatileState _state;

        internal VulkanGraphicsDevice(VulkanGraphicsAdapter adapter, IGraphicsSurface surface, int contextCount)
            : base(adapter, surface)
        {
            _presentCompletionFence = new VulkanGraphicsFence(this);

            _vulkanCommandQueue = new ValueLazy<VkQueue>(GetVulkanCommandQueue);
            _vulkanCommandQueueFamilyIndex = new ValueLazy<uint>(GetVulkanCommandQueueFamilyIndex);
            _vulkanDevice = new ValueLazy<VkDevice>(CreateVulkanDevice);
            _vulkanRenderPass = new ValueLazy<VkRenderPass>(CreateVulkanRenderPass);
            _vulkanSurface = new ValueLazy<VkSurfaceKHR>(CreateVulkanSurface);
            _vulkanSwapchain = new ValueLazy<VkSwapchainKHR>(CreateVulkanSwapchain);
            _vulkanSwapchainImages = new ValueLazy<VkImage[]>(GetVulkanSwapchainImages);
            _memoryAllocator = new ValueLazy<VulkanGraphicsMemoryAllocator>(CreateMemoryAllocator);

            _contexts = CreateGraphicsContexts(this, contextCount);

            _ = _state.Transition(to: Initialized);

            PresentCompletionFence.Reset();
            surface.SizeChanged += OnGraphicsSurfaceSizeChanged;

            static VulkanGraphicsContext[] CreateGraphicsContexts(VulkanGraphicsDevice device, int contextCount)
            {
                var contexts = new VulkanGraphicsContext[contextCount];

                for (var index = 0; index < contexts.Length; index++)
                {
                    contexts[index] = new VulkanGraphicsContext(device, index);
                }

                return contexts;
            }
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsDevice" /> class.</summary>
        ~VulkanGraphicsDevice() => Dispose(isDisposing: true);

        /// <inheritdoc cref="GraphicsDevice.Adapter" />
        public new VulkanGraphicsAdapter Adapter => (VulkanGraphicsAdapter)base.Adapter;

        /// <inheritdoc />
        public override ReadOnlySpan<GraphicsContext> Contexts => _contexts;

        /// <inheritdoc cref="GraphicsDevice.CurrentContext" />
        public new VulkanGraphicsContext CurrentContext => (VulkanGraphicsContext)base.CurrentContext;

        /// <inheritdoc />
        public override int ContextIndex => _contextIndex;

        /// <inheritdoc />
        public override VulkanGraphicsMemoryAllocator MemoryAllocator => _memoryAllocator.Value;

        /// <summary>Gets a fence that is used to wait for <see cref="PresentFrame" /> to complete.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VulkanGraphicsFence PresentCompletionFence => _presentCompletionFence;

        /// <summary>Gets the <see cref="VkQueue" /> used by the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VkQueue VulkanCommandQueue => _vulkanCommandQueue.Value;

        /// <summary>Gets the index of the queue family for <see cref="VulkanCommandQueue" />.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public uint VulkanCommandQueueFamilyIndex => _vulkanCommandQueueFamilyIndex.Value;

        /// <summary>Gets the underlying <see cref="VkDevice"/> for the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public VkDevice VulkanDevice => _vulkanDevice.Value;

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

        /// <inheritdoc />
        public override VulkanGraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null)
            => CreatePipeline((VulkanGraphicsPipelineSignature)signature, (VulkanGraphicsShader?)vertexShader, (VulkanGraphicsShader?)pixelShader);

        /// <inheritdoc cref="CreatePipeline(GraphicsPipelineSignature, GraphicsShader?, GraphicsShader?)" />
        public VulkanGraphicsPipeline CreatePipeline(VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader = null, VulkanGraphicsShader? pixelShader = null)
        {
            ThrowIfDisposedOrDisposing(_state);
            return new VulkanGraphicsPipeline(this, signature, vertexShader, pixelShader);
        }

        /// <inheritdoc />
        public override GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default)
        {
            ThrowIfDisposedOrDisposing(_state);
            return new VulkanGraphicsPipelineSignature(this, inputs, resources);
        }

        /// <inheritdoc />
        public override VulkanGraphicsPrimitive CreatePrimitive(GraphicsPipeline pipeline, in GraphicsMemoryRegion<GraphicsResource> vertexBufferView, uint vertexBufferStride, in GraphicsMemoryRegion<GraphicsResource> indexBufferView = default, uint indexBufferStride = 0, ReadOnlySpan<GraphicsMemoryRegion<GraphicsResource>> inputResourceRegions = default)
            => CreatePrimitive((VulkanGraphicsPipeline)pipeline, in vertexBufferView, vertexBufferStride, in indexBufferView, indexBufferStride, inputResourceRegions);

        /// <inheritdoc cref="CreatePrimitive(GraphicsPipeline, in GraphicsMemoryRegion{GraphicsResource}, uint, in GraphicsMemoryRegion{GraphicsResource}, uint, ReadOnlySpan{GraphicsMemoryRegion{GraphicsResource}})" />
        public VulkanGraphicsPrimitive CreatePrimitive(VulkanGraphicsPipeline pipeline, in GraphicsMemoryRegion<GraphicsResource> vertexBufferView, uint vertexBufferStride, in GraphicsMemoryRegion<GraphicsResource> indexBufferView, uint indexBufferStride, ReadOnlySpan<GraphicsMemoryRegion<GraphicsResource>> inputResourceRegions)
        {
            ThrowIfDisposedOrDisposing(_state);
            return new VulkanGraphicsPrimitive(this, pipeline, in vertexBufferView, vertexBufferStride, in indexBufferView, indexBufferStride, inputResourceRegions);
        }

        /// <inheritdoc />
        public override VulkanGraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
        {
            ThrowIfDisposedOrDisposing(_state);
            return new VulkanGraphicsShader(this, kind, bytecode, entryPointName);
        }

        /// <inheritdoc />
        public override void PresentFrame()
        {
            var contextIndex = ContextIndex;
            var vulkanSwapchain = VulkanSwapchain;

            var presentInfo = new VkPresentInfoKHR {
                sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
                swapchainCount = 1,
                pSwapchains = (ulong*)&vulkanSwapchain,
                pImageIndices = (uint*)&contextIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkQueuePresentKHR(VulkanCommandQueue, &presentInfo), nameof(vkQueuePresentKHR));

            Signal(CurrentContext.Fence);

            var presentCompletionGraphicsFence = PresentCompletionFence;
            ThrowExternalExceptionIfNotSuccess(vkAcquireNextImageKHR(VulkanDevice, vulkanSwapchain, timeout: ulong.MaxValue, semaphore: VK_NULL_HANDLE, presentCompletionGraphicsFence.VulkanFence, (uint*)&contextIndex), nameof(vkAcquireNextImageKHR));

            presentCompletionGraphicsFence.Wait();
            presentCompletionGraphicsFence.Reset();

            _contextIndex = contextIndex;
        }

        /// <inheritdoc />
        public override void Signal(GraphicsFence fence)
            => Signal((VulkanGraphicsFence)fence);

        /// <inheritdoc cref="Signal(GraphicsFence)" />
        public void Signal(VulkanGraphicsFence fence)
            => ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(VulkanCommandQueue, submitCount: 0, pSubmits: null, fence.VulkanFence), nameof(vkQueueSubmit));

        /// <inheritdoc />
        public override void WaitForIdle()
        {
            if (_vulkanDevice.IsCreated)
            {
                var result = vkDeviceWaitIdle(_vulkanDevice.Value);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException((int)result, nameof(vkDeviceWaitIdle));
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

                foreach (var context in _contexts)
                {
                    context?.Dispose();
                }

                _memoryAllocator.Dispose(DisposeMemoryAllocator);
                _vulkanRenderPass.Dispose(DisposeVulkanRenderPass);
                _vulkanSwapchain.Dispose(DisposeVulkanSwapchain);
                _vulkanSurface.Dispose(DisposeVulkanSurface);

                _presentCompletionFence?.Dispose();

                _vulkanDevice.Dispose(DisposeVulkanDevice);
            }

            _state.EndDispose();
        }

        private VulkanGraphicsMemoryAllocator CreateMemoryAllocator()
        {
            var allocatorSettings = default(GraphicsMemoryAllocatorSettings);
            return new VulkanGraphicsMemoryAllocator(this, in allocatorSettings);
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

            const int EnabledExtensionNamesCount = 1;

            var enabledExtensionNames = stackalloc sbyte*[EnabledExtensionNamesCount] {
                (sbyte*)VK_KHR_SWAPCHAIN_EXTENSION_NAME.AsPointer(),
            };

            var physicalDeviceFeatures = new VkPhysicalDeviceFeatures();

            var deviceCreateInfo = new VkDeviceCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
                queueCreateInfoCount = 1,
                pQueueCreateInfos = &deviceQueueCreateInfo,
                enabledExtensionCount = EnabledExtensionNamesCount,
                ppEnabledExtensionNames = enabledExtensionNames,
                pEnabledFeatures = &physicalDeviceFeatures,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateDevice(Adapter.VulkanPhysicalDevice, &deviceCreateInfo, pAllocator: null, (IntPtr*)&vulkanDevice), nameof(vkCreateDevice));

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
            ThrowExternalExceptionIfNotSuccess(vkCreateRenderPass(VulkanDevice, &renderPassCreateInfo, pAllocator: null, (ulong*)&vulkanRenderPass), nameof(vkCreateRenderPass));

            return vulkanRenderPass;
        }

        private VkSurfaceKHR CreateVulkanSurface()
        {
            VkSurfaceKHR vulkanSurface;

            var adapter = Adapter;
            var vulkanInstance = adapter.Provider.VulkanInstance;

            switch (Surface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                        hinstance = Surface.ContextHandle,
                        hwnd = Surface.Handle,
                    };

                    ThrowExternalExceptionIfNotSuccess(vkCreateWin32SurfaceKHR(vulkanInstance, &surfaceCreateInfo, pAllocator: null, (ulong*)&vulkanSurface), nameof(vkCreateWin32SurfaceKHR));
                    break;
                }

                case GraphicsSurfaceKind.Xlib:
                {
                    var surfaceCreateInfo = new VkXlibSurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_XLIB_SURFACE_CREATE_INFO_KHR,
                        dpy = Surface.ContextHandle,
                        window = (nuint)(nint)Surface.Handle,
                    };

                    ThrowExternalExceptionIfNotSuccess(vkCreateXlibSurfaceKHR(vulkanInstance, &surfaceCreateInfo, pAllocator: null, (ulong*)&vulkanSurface), nameof(vkCreateXlibSurfaceKHR));
                    break;
                }

                default:
                {
                    ThrowArgumentOutOfRangeException(Surface, nameof(Surface));
                    vulkanSurface = VK_NULL_HANDLE;
                    break;
                }
            }

            uint supported;
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceSupportKHR(adapter.VulkanPhysicalDevice, VulkanCommandQueueFamilyIndex, vulkanSurface, &supported), nameof(vkGetPhysicalDeviceSurfaceSupportKHR));

            if (supported == VK_FALSE)
            {
                ThrowArgumentOutOfRangeException(Surface, nameof(Surface));
            }
            return vulkanSurface;
        }

        private VkSwapchainKHR CreateVulkanSwapchain()
        {
            VkSwapchainKHR vulkanSwapchain;

            var vulkanPhysicalDevice = Adapter.VulkanPhysicalDevice;
            var vulkanSurface = VulkanSurface;

            VkSurfaceCapabilitiesKHR surfaceCapabilities;
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceCapabilitiesKHR(vulkanPhysicalDevice, vulkanSurface, &surfaceCapabilities), nameof(vkGetPhysicalDeviceSurfaceCapabilitiesKHR));

            uint presentModeCount;
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vulkanPhysicalDevice, vulkanSurface, &presentModeCount, pPresentModes: null), nameof(vkGetPhysicalDeviceSurfacePresentModesKHR));

            var presentModes = stackalloc VkPresentModeKHR[(int)presentModeCount];
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfacePresentModesKHR(vulkanPhysicalDevice, vulkanSurface, &presentModeCount, presentModes), nameof(vkGetPhysicalDeviceSurfacePresentModesKHR));

            var surface = Surface;
            var contextsCount = unchecked((uint)Contexts.Length);

            if ((contextsCount < surfaceCapabilities.minImageCount) || ((surfaceCapabilities.maxImageCount != 0) && (contextsCount > surfaceCapabilities.maxImageCount)))
            {
                ThrowArgumentOutOfRangeException(contextsCount, nameof(contextsCount));
            }

            var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
                sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
                surface = vulkanSurface,
                minImageCount = contextsCount,
                imageExtent = new VkExtent2D {
                    width = (uint)surface.Width,
                    height = (uint)surface.Height,
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
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vulkanPhysicalDevice, VulkanSurface, &surfaceFormatCount, pSurfaceFormats: null), nameof(vkGetPhysicalDeviceSurfaceFormatsKHR));

            var surfaceFormats = stackalloc VkSurfaceFormatKHR[(int)surfaceFormatCount];
            ThrowExternalExceptionIfNotSuccess(vkGetPhysicalDeviceSurfaceFormatsKHR(vulkanPhysicalDevice, VulkanSurface, &surfaceFormatCount, surfaceFormats), nameof(vkGetPhysicalDeviceSurfaceFormatsKHR));

            for (uint i = 0; i < surfaceFormatCount; i++)
            {
                if (surfaceFormats[i].format == VK_FORMAT_B8G8R8A8_UNORM)
                {
                    swapChainCreateInfo.imageFormat = surfaceFormats[i].format;
                    swapChainCreateInfo.imageColorSpace = surfaceFormats[i].colorSpace;
                    break;
                }
            }
            ThrowExternalExceptionIfNotSuccess(vkCreateSwapchainKHR(VulkanDevice, &swapChainCreateInfo, pAllocator: null, (ulong*)&vulkanSwapchain), nameof(vkCreateSwapchainKHR));

            _vulkanSwapchainFormat = swapChainCreateInfo.imageFormat;
            return vulkanSwapchain;
        }

        private void DisposeMemoryAllocator(VulkanGraphicsMemoryAllocator memoryAllocator) => memoryAllocator?.Dispose();

        private void DisposeVulkanDevice(VkDevice vulkanDevice)
        {
            AssertDisposing(_state);

            if (vulkanDevice != null)
            {
                vkDestroyDevice(vulkanDevice, pAllocator: null);
            }
        }

        private void DisposeVulkanRenderPass(VkRenderPass vulkanRenderPass)
        {
            AssertDisposing(_state);

            if (vulkanRenderPass != VK_NULL_HANDLE)
            {
                vkDestroyRenderPass(VulkanDevice, vulkanRenderPass, pAllocator: null);
            }
        }

        private void DisposeVulkanSurface(VkSurfaceKHR vulkanSurface)
        {
            AssertDisposing(_state);

            if (vulkanSurface != VK_NULL_HANDLE)
            {
                vkDestroySurfaceKHR(Adapter.Provider.VulkanInstance, vulkanSurface, pAllocator: null);
            }
        }

        private void DisposeVulkanSwapchain(VkSwapchainKHR vulkanSwapchain)
        {
            AssertDisposing(_state);

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

            var vulkanPhysicalDevice = Adapter.VulkanPhysicalDevice;

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
                ThrowInvalidOperationException(vulkanCommandQueueFamilyIndex, nameof(vulkanCommandQueueFamilyIndex));
            }
            return vulkanCommandQueueFamilyIndex;
        }

        private VkImage[] GetVulkanSwapchainImages()
        {
            var vulkanDevice = VulkanDevice;
            var vulkanSwapchain = VulkanSwapchain;

            uint swapchainImageCount;
            ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vulkanDevice, vulkanSwapchain, &swapchainImageCount, pSwapchainImages: null), nameof(vkGetSwapchainImagesKHR));

            var vulkanSwapchainImages = new VkImage[swapchainImageCount];

            fixed (VkImage* pVulkanSwapchainImages = vulkanSwapchainImages)
            {
                ThrowExternalExceptionIfNotSuccess(vkGetSwapchainImagesKHR(vulkanDevice, vulkanSwapchain, &swapchainImageCount, (ulong*)pVulkanSwapchainImages), nameof(vkGetSwapchainImagesKHR));
            }

            var presentCompletionGraphicsFence = PresentCompletionFence;

            int contextIndex;
            ThrowExternalExceptionIfNotSuccess(vkAcquireNextImageKHR(VulkanDevice, vulkanSwapchain, timeout: ulong.MaxValue, semaphore: VK_NULL_HANDLE, presentCompletionGraphicsFence.VulkanFence, (uint*)&contextIndex), nameof(vkAcquireNextImageKHR));
            _contextIndex = contextIndex;

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
                _contextIndex = 0;
            }

            foreach (var context in Contexts)
            {
                ((VulkanGraphicsContext)context).OnGraphicsSurfaceSizeChanged(sender, eventArgs);
            }
        }
    }
}
