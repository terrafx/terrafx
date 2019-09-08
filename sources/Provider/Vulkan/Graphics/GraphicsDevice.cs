// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.VkCompositeAlphaFlagBitsKHR;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageUsageFlagBits;
using static TerraFX.Interop.VkPresentModeKHR;
using static TerraFX.Interop.VkResult;
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

        private State _state;

        internal GraphicsDevice(GraphicsAdapter graphicsAdapter, IntPtr device, IntPtr queue)
        {
            _graphicsAdapter = graphicsAdapter;
            _device = device;
            _queue = queue;
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
            VkResult result;

            switch (graphicsSurface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR {
                        sType = VK_STRUCTURE_TYPE_DISPLAY_SURFACE_CREATE_INFO_KHR,
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
                        sType = VK_STRUCTURE_TYPE_DISPLAY_SURFACE_CREATE_INFO_KHR,
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

            var swapChainCreateInfo = new VkSwapchainCreateInfoKHR {
                sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
                surface = surface,
                minImageCount = 2,
                imageFormat = VK_FORMAT_A8B8G8R8_UNORM_PACK32,
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

            return new SwapChain(this, surface, swapChain);
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
