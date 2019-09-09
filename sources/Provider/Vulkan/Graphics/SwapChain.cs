// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageAspectFlagBits;
using static TerraFX.Interop.VkImageViewType;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkResult;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a graphics device.</summary>
    public sealed unsafe class SwapChain : IDisposable, ISwapChain
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IGraphicsSurface _graphicsSurface;
        private readonly IntPtr _surface;
        private readonly IntPtr _swapChain;
        private readonly IntPtr _renderPass;

        private State _state;

        internal SwapChain(GraphicsDevice graphicsDevice, IGraphicsSurface graphicsSurface, IntPtr surface, IntPtr swapChain, IntPtr renderPass)
        {
            _graphicsDevice = graphicsDevice;
            _graphicsSurface = graphicsSurface;
            _surface = surface;
            _swapChain = swapChain;
            _renderPass = renderPass;
        }

        /// <summary>Finalizes an instance of the <see cref="SwapChain" /> class.</summary>
        ~SwapChain()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsDevice" /> for the instance.</summary>
        public IGraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the <see cref="IGraphicsSurface" /> for the instance.</summary>
        public IGraphicsSurface GraphicsSurface => _graphicsSurface;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => _swapChain;

        /// <summary>Creates an array of <see cref="IRenderTargetView" /> for the instance.</summary>
        /// <returns>An array of <see cref="IRenderTargetView" /> for the instance.</returns>
        public IRenderTargetView[] CreateRenderTargetViews()
        {
            uint swapChainImageCount;
            var result = vkGetSwapchainImagesKHR(_graphicsDevice.Handle, _swapChain, &swapChainImageCount, pSwapchainImages: null);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetSwapchainImagesKHR), (int)result);
            }

            var swapChainImages = stackalloc IntPtr[(int)swapChainImageCount];
            result = vkGetSwapchainImagesKHR(_graphicsDevice.Handle, _swapChain, &swapChainImageCount, swapChainImages);

            if (result != VK_SUCCESS)
            {
                ThrowExternalException(nameof(vkGetSwapchainImagesKHR), (int)result);
            }

            var imageViewCreateInfo = new VkImageViewCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
                viewType = VK_IMAGE_VIEW_TYPE_2D,
                format = VK_FORMAT_R8G8B8A8_UNORM,
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = (uint)VK_IMAGE_ASPECT_COLOR_BIT,
                    levelCount = 1,
                    layerCount = 1,
                },
            };

            var frameBufferCreateInfo = new VkFramebufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                renderPass = (ulong)_renderPass,
                attachmentCount = 1,
                width = (uint)_graphicsSurface.Width,
                height = (uint)_graphicsSurface.Height,
                layers = 1
            };

            var renderTargetViews = new RenderTargetView[swapChainImageCount];

            for (uint i = 0; i < swapChainImageCount; i++)
            {
                imageViewCreateInfo.image = (ulong)swapChainImages[i];

                IntPtr imageView;
                result = vkCreateImageView(_graphicsDevice.Handle, &imageViewCreateInfo, pAllocator: null, (ulong)&imageView);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkCreateImageView), (int)result);
                }

                frameBufferCreateInfo.pAttachments = (ulong*)&imageView;

                IntPtr frameBuffer;
                result = vkCreateFramebuffer(_graphicsDevice.Handle, &frameBufferCreateInfo, pAllocator: null, (ulong*)&frameBuffer);

                if (result != VK_SUCCESS)
                {
                    ThrowExternalException(nameof(vkCreateFramebuffer), (int)result);
                }

                renderTargetViews[i] = new RenderTargetView(this, imageView, frameBuffer);
            }

            return renderTargetViews;
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
                DisposeSwapChain();
            }

            _state.EndDispose();
        }

        private void DisposeSwapChain()
        {
            _state.AssertDisposing();

            if (_renderPass != IntPtr.Zero)
            {
                vkDestroyRenderPass(_graphicsDevice.Handle, (ulong)_renderPass, pAllocator: null);
            }

            if (_swapChain != IntPtr.Zero)
            {
                vkDestroySwapchainKHR(_graphicsDevice.Handle, _swapChain, pAllocator: null);
            }

            if (_surface != IntPtr.Zero)
            {
                vkDestroySurfaceKHR(_graphicsDevice.GraphicsAdapter.GraphicsProvider.Handle, _surface, pAllocator: null);
            }
        }
    }
}
