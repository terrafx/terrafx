// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Utilities;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Vulkan.Graphics
{
    /// <summary>Represents a graphics device.</summary>
    public sealed unsafe class SwapChain : IDisposable, ISwapChain
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IntPtr _surface;
        private readonly IntPtr _swapChain;

        private State _state;

        internal SwapChain(GraphicsDevice graphicsDevice, IntPtr surface, IntPtr swapChain)
        {
            _graphicsDevice = graphicsDevice;
            _surface = surface;
            _swapChain = swapChain;
        }

        /// <summary>Gets the <see cref="IGraphicsDevice" /> for the instance.</summary>
        public IGraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => _swapChain;

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
