// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.VkAttachmentLoadOp;
using static TerraFX.Interop.VkAttachmentStoreOp;
using static TerraFX.Interop.VkCommandPoolCreateFlagBits;
using static TerraFX.Interop.VkCompositeAlphaFlagBitsKHR;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageLayout;
using static TerraFX.Interop.VkImageUsageFlagBits;
using static TerraFX.Interop.VkPipelineBindPoint;
using static TerraFX.Interop.VkPresentModeKHR;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.VkSampleCountFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkSurfaceTransformFlagBitsKHR;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a graphics device.</summary>
    public sealed unsafe class GraphicsDevice : IDisposable, IGraphicsDevice
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IntPtr _device;
        private readonly IntPtr _queue;
        private readonly uint _queueFamilyIndex;

        private State _state;

        internal GraphicsDevice(GraphicsAdapter graphicsAdapter, IntPtr device, IntPtr queue, uint queueFamilyIndex)
        {
            _graphicsAdapter = graphicsAdapter;
            _device = device;
            _queue = queue;
            _queueFamilyIndex = queueFamilyIndex;
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsDevice" /> class.</summary>
        ~GraphicsDevice()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
        public IGraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => _device;

        /// <summary>Creates a new <see cref="ISwapChain" /> for the instance.</summary>
        /// <param name="graphicsSurface">The <see cref="IGraphicsSurface" /> to which the swap chain belongs.</param>
        /// <returns>A new <see cref="ISwapChain" /> for the instance.</returns>
        public ISwapChain CreateSwapChain(IGraphicsSurface graphicsSurface)
        {
            IntPtr surface;
            IntPtr swapChain;
            IntPtr renderPass;
            IntPtr commandPool;

            VkResult result;

            switch (graphicsSurface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                        hinstance = graphicsSurface.WindowProviderHandle,
                        hwnd = graphicsSurface.WindowHandle
                    };

                    result = vkCreateWin32SurfaceKHR(_graphicsAdapter.GraphicsProvider.Handle, &surfaceCreateInfo, pAllocator: null, &surface);

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
                        dpy = graphicsSurface.WindowProviderHandle,
                        window = (UIntPtr)graphicsSurface.WindowHandle.ToPointer()
                    };

                    result = vkCreateXlibSurfaceKHR(_graphicsAdapter.GraphicsProvider.Handle, &surfaceCreateInfo, pAllocator: null, &surface);

                    if (result != VK_SUCCESS)
                    {
                        ThrowExternalException(nameof(vkCreateXlibSurfaceKHR), (int)result);
                    }
                    break;
                }

                default:
                {
                    ThrowArgumentOutOfRangeException(nameof(graphicsSurface), graphicsSurface.Kind);
                    surface = IntPtr.Zero;
                    break;
                }
            }

            uint supported;
            result = vkGetPhysicalDeviceSurfaceSupportKHR(_graphicsAdapter.Handle, _queueFamilyIndex, surface, &supported);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfaceSupportKHR), (int)result);
            }

            if (supported == VK_FALSE)
            {
                ThrowInvalidOperationException(nameof(supported), supported);
            }

            VkSurfaceCapabilitiesKHR surfaceCapabilities;
            result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(_graphicsAdapter.Handle, surface, &surfaceCapabilities);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfaceCapabilitiesKHR), (int)result);
            }

            uint presentModeCount;
            result = vkGetPhysicalDeviceSurfacePresentModesKHR(_graphicsAdapter.Handle, surface, &presentModeCount, pPresentModes: null);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), (int)result);
            }

            var presentModes = stackalloc VkPresentModeKHR[(int)presentModeCount];
            result = vkGetPhysicalDeviceSurfacePresentModesKHR(_graphicsAdapter.Handle, surface, &presentModeCount, presentModes);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetPhysicalDeviceSurfacePresentModesKHR), (int)result);
            }

            var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
                sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
                surface = surface,
                minImageCount = (uint)graphicsSurface.BufferCount,
                imageFormat = VK_FORMAT_R8G8B8A8_UNORM,
                imageExtent = new VkExtent2D {
                    width = (uint)graphicsSurface.Width,
                    height = (uint)graphicsSurface.Height,
                },
                imageArrayLayers = 1,
                imageUsage = (uint)VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT,
                preTransform = VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR,
                compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
                presentMode = VK_PRESENT_MODE_FIFO_KHR,
                clipped = VK_TRUE,
            };

            result = vkCreateSwapchainKHR(_device, &swapChainCreateInfo, pAllocator: null, &swapChain);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateSwapchainKHR), (int)result);
            }

            var attachment = new VkAttachmentDescription {
                format = VK_FORMAT_R8G8B8A8_UNORM,
                samples = VK_SAMPLE_COUNT_1_BIT,
                loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VK_ATTACHMENT_STORE_OP_STORE,
                stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
                finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
            };

            var colorAttachment = new VkAttachmentReference {
                layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
            };

            var subpass = new VkSubpassDescription {
                pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
                colorAttachmentCount = 1,
                pColorAttachments = &colorAttachment,
            };

            var renderPassCreateInfo = new VkRenderPassCreateInfo {
                sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                attachmentCount = 1,
                pAttachments = &attachment,
                subpassCount = 1,
                pSubpasses = &subpass,
            };

            result = vkCreateRenderPass(_device, &renderPassCreateInfo, pAllocator: null, (ulong*)&renderPass);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateRenderPass), (int)result);
            }

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                flags = (uint)VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
                queueFamilyIndex = _queueFamilyIndex,
            };

            result = vkCreateCommandPool(_device, &commandPoolCreateInfo, pAllocator: null, (ulong*)&commandPool);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkCreateCommandPool), (int)result);
            }

            return new SwapChain(this, graphicsSurface, surface, swapChain, renderPass, commandPool);
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeDevice();
            }

            _state.EndDispose();
        }

        private void DisposeDevice()
        {
            _state.AssertDisposing();

            if (_device != IntPtr.Zero)
            {
                vkDestroyDevice(_device, pAllocator: null);
            }
        }
    }
}
