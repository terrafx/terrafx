// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a swap chain.</summary>
    public sealed unsafe class SwapChain : IDisposable, ISwapChain
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly IGraphicsSurface _graphicsSurface;
        private readonly IDXGISwapChain1* _swapChain;

        private State _state;

        internal SwapChain(GraphicsDevice graphicsDevice, IGraphicsSurface graphicsSurface, IDXGISwapChain1* swapChain)
        {
            _graphicsDevice = graphicsDevice;
            _graphicsSurface = graphicsSurface;
            _swapChain = swapChain;
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
        public IntPtr Handle => (IntPtr)_swapChain;

        /// <summary>Creates an array of <see cref="IRenderTargetView" /> for the instance.</summary>
        /// <returns>An array of <see cref="IRenderTargetView" /> for the instance.</returns>
        public IRenderTargetView[] CreateRenderTargetViews()
        {
            ID3D12DescriptorHeap* rtvHeap;
            ID3D12Device* device = (ID3D12Device*)_graphicsDevice.Handle;

            var rtvHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                NumDescriptors = (uint)_graphicsSurface.BufferCount,
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
            };

            var iid = IID_ID3D12DescriptorHeap;
            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateDescriptorHeap), device->CreateDescriptorHeap(&rtvHeapDesc, &iid, (void**)&rtvHeap));

            var rtvDescriptorSize = device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
            var rtvHandle = rtvHeap->GetCPUDescriptorHandleForHeapStart();

            var renderTargetViews = new RenderTargetView[rtvHeapDesc.NumDescriptors];

            for (uint i = 0; i < rtvHeapDesc.NumDescriptors; i++)
            {
                ID3D12Resource* renderTarget;
                ThrowExternalExceptionIfFailed(nameof(IDXGISwapChain.GetBuffer), _swapChain->GetBuffer(i, &iid, (void**)&renderTarget));

                device->CreateRenderTargetView(renderTarget, null, rtvHandle);
                rtvHandle.ptr = (UIntPtr)((byte*)rtvHandle.ptr + rtvDescriptorSize);

                renderTargetViews[i] = new RenderTargetView(this, renderTarget);
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

            if (_swapChain != null)
            {
                _ = _swapChain->Release();
            }
        }
    }
}
