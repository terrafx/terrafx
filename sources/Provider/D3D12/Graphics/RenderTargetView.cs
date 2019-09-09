// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a render target view.</summary>
    public sealed unsafe class RenderTargetView : IDisposable, IRenderTargetView
    {
        private readonly SwapChain _swapChain;
        private readonly ID3D12Resource* _renderTarget;

        private State _state;

        internal RenderTargetView(SwapChain swapChain, ID3D12Resource* renderTarget)
        {
            _swapChain = swapChain;
            _renderTarget = renderTarget;
        }

        /// <summary>Finalizes an instance of the <see cref="RenderTargetView" /> class.</summary>
        ~RenderTargetView()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="ISwapChain" /> for the instance.</summary>
        public ISwapChain SwapChain => _swapChain;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => (IntPtr)_renderTarget;

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

            if (_renderTarget != null)
            {
                _ = _renderTarget->Release();
            }
        }
    }
}
