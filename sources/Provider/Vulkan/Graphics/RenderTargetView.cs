// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Utilities;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a render target view.</summary>
    public sealed unsafe class RenderTargetView : IDisposable, IRenderTargetView
    {
        private readonly SwapChain _swapChain;
        private readonly IntPtr _imageView;
        private readonly IntPtr _frameBuffer;
        private readonly IntPtr _commandBuffer;
        private readonly IntPtr _fence;

        private State _state;

        internal RenderTargetView(SwapChain swapChain, IntPtr imageView, IntPtr frameBuffer, IntPtr commandBuffer, IntPtr fence)
        {
            _swapChain = swapChain;
            _imageView = imageView;
            _frameBuffer = frameBuffer;
            _commandBuffer = commandBuffer;
            _fence = fence;
        }

        /// <summary>Finalizes an instance of the <see cref="RenderTargetView" /> class.</summary>
        ~RenderTargetView()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="ISwapChain" /> for the instance.</summary>
        public ISwapChain SwapChain => _swapChain;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => _imageView;

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
                DisposeRenderTargetView();
            }

            _state.EndDispose();
        }

        private void DisposeRenderTargetView()
        {
            _state.AssertDisposing();

            if (_fence != IntPtr.Zero)
            {
                vkDestroyFence(_swapChain.GraphicsDevice.Handle, (ulong)_fence, pAllocator: null);
            }

            if (_frameBuffer != IntPtr.Zero)
            {
                vkDestroyFramebuffer(_swapChain.GraphicsDevice.Handle, (ulong)_frameBuffer, pAllocator: null);
            }

            if (_imageView != IntPtr.Zero)
            {
                vkDestroyImageView(_swapChain.GraphicsDevice.Handle, (ulong)_imageView, pAllocator: null);
            }
        }
    }
}
