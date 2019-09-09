// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a graphics device.</summary>
    public sealed unsafe class GraphicsDevice : IDisposable, IGraphicsDevice
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly ID3D12Device* _device;
        private readonly ID3D12CommandQueue* _commandQueue;

        private State _state;

        internal GraphicsDevice(GraphicsAdapter graphicsAdapter, ID3D12Device* device, ID3D12CommandQueue* commandQueue)
        {
            _graphicsAdapter = graphicsAdapter;
            _device = device;
            _commandQueue = commandQueue;
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsDevice" /> class.</summary>
        ~GraphicsDevice()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
        public IGraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => (IntPtr)_device;

        /// <summary>Creates a new <see cref="ISwapChain" /> for the instance.</summary>
        /// <param name="graphicsSurface">The <see cref="IGraphicsSurface" /> to which the swap chain belongs.</param>
        /// <returns>A new <see cref="ISwapChain" /> for the instance.</returns>
        public ISwapChain CreateSwapChain(IGraphicsSurface graphicsSurface)
        {
            IDXGISwapChain1* swapChain;

            var swapChainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                BufferCount = (uint)graphicsSurface.BufferCount,
                Width = (uint)graphicsSurface.Width,
                Height = (uint)graphicsSurface.Height,
                Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_DISCARD,
                SampleDesc = new DXGI_SAMPLE_DESC {
                    Count = 1,
                },
            };

            if (graphicsSurface.Kind != GraphicsSurfaceKind.Win32)
            {
                ThrowArgumentOutOfRangeException(nameof(graphicsSurface), graphicsSurface.Kind);
            }

            var factory = (IDXGIFactory2*)_graphicsAdapter.GraphicsProvider.Handle;
            ThrowExternalExceptionIfFailed(nameof(IDXGIFactory2.CreateSwapChainForHwnd), factory->CreateSwapChainForHwnd((IUnknown*)_commandQueue, graphicsSurface.WindowHandle, &swapChainDesc, pFullscreenDesc: null, pRestrictToOutput: null, &swapChain));

            return new SwapChain(this, graphicsSurface, swapChain);
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

            if (_commandQueue != null)
            {
                _ = _commandQueue->Release();
            }

            if (_device != null)
            {
                _ = _device->Release();
            }
        }
    }
}
