// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a swap chain.</summary>
    public sealed unsafe class SwapChain : IDisposable, ISwapChain
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IDXGISwapChain1* _swapChain;

        private State _state;

        internal SwapChain(GraphicsDevice graphicsDevice, IDXGISwapChain1* swapChain)
        {
            _graphicsDevice = graphicsDevice;
            _swapChain = swapChain;
        }

        /// <summary>Gets the <see cref="IGraphicsDevice" /> for the instance.</summary>
        public IGraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => (IntPtr)_swapChain;

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

            if (_swapChain != null)
            {
                _ = _swapChain->Release();
            }
        }
    }
}
