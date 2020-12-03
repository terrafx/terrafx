// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.D3D12_FEATURE;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.DXGI_SWAP_EFFECT;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsDevice : GraphicsDevice
    {
        private readonly D3D12GraphicsContext[] _contexts;
        private readonly D3D12GraphicsFence _idleFence;

        private ValueLazy<Pointer<ID3D12CommandQueue>> _d3d12CommandQueue;
        private ValueLazy<Pointer<ID3D12Device>> _d3d12Device;
        private ValueLazy<D3D12_FEATURE_DATA_D3D12_OPTIONS> _d3d12Options;
        private ValueLazy<Pointer<ID3D12DescriptorHeap>> _d3d12RenderTargetDescriptorHeap;
        private ValueLazy<Pointer<IDXGISwapChain3>> _dxgiSwapChain;
        private ValueLazy<D3D12GraphicsMemoryAllocator> _memoryAllocator;
        private ValueLazy<uint> _cbvSrvUavDescriptorHandleIncrementSize;

        private int _contextIndex;
        private DXGI_FORMAT _swapChainFormat;

        private State _state;

        internal D3D12GraphicsDevice(D3D12GraphicsAdapter adapter, IGraphicsSurface surface, int contextCount)
            : base(adapter, surface)
        {
            _idleFence = new D3D12GraphicsFence(this);

            _d3d12CommandQueue = new ValueLazy<Pointer<ID3D12CommandQueue>>(CreateD3D12CommandQueue);
            _d3d12Device = new ValueLazy<Pointer<ID3D12Device>>(CreateD3D12Device);
            _d3d12Options = new ValueLazy<D3D12_FEATURE_DATA_D3D12_OPTIONS>(GetD3D12Options);
            _d3d12RenderTargetDescriptorHeap = new ValueLazy<Pointer<ID3D12DescriptorHeap>>(CreateD3D12RenderTargetDescriptorHeap);
            _dxgiSwapChain = new ValueLazy<Pointer<IDXGISwapChain3>>(CreateDxgiSwapChain);
            _memoryAllocator = new ValueLazy<D3D12GraphicsMemoryAllocator>(CreateMemoryAllocator);
            _cbvSrvUavDescriptorHandleIncrementSize = new ValueLazy<uint>(GetCbvSrvUavDescriptorHandleIncrementSize);

            _contexts = CreateContexts(this, contextCount);

            _ = _state.Transition(to: Initialized);

            WaitForIdleGraphicsFence.Reset();
            surface.SizeChanged += OnGraphicsSurfaceSizeChanged;

            static D3D12GraphicsContext[] CreateContexts(D3D12GraphicsDevice device, int contextCount)
            {
                var contexts = new D3D12GraphicsContext[contextCount];

                for (var index = 0; index < contexts.Length; index++)
                {
                    contexts[index] = new D3D12GraphicsContext(device, index);
                }

                return contexts;
            }
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsDevice" /> class.</summary>
        ~D3D12GraphicsDevice() => Dispose(isDisposing: false);

        /// <inheritdoc cref="GraphicsDevice.Adapter" />
        public new D3D12GraphicsAdapter Adapter => (D3D12GraphicsAdapter)base.Adapter;

        /// <summary>Gets the descriptor handle increment size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV" />.</summary>
        public uint CbvSrvUavDescriptorHandleIncrementSize => _cbvSrvUavDescriptorHandleIncrementSize.Value;

        /// <inheritdoc />
        public override int ContextIndex => _contextIndex;

        /// <inheritdoc />
        public override ReadOnlySpan<GraphicsContext> Contexts => _contexts;

        /// <inheritdoc cref="GraphicsDevice.CurrentContext" />
        public new D3D12GraphicsContext CurrentContext => (D3D12GraphicsContext)base.CurrentContext;

        /// <summary>Gets the <see cref="ID3D12CommandQueue" /> used by the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12CommandQueue* D3D12CommandQueue => _d3d12CommandQueue.Value;

        /// <summary>Gets the underlying <see cref="ID3D12Device" /> for the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12Device* D3D12Device => _d3d12Device.Value;

        /// <summary>Gets the <see cref="D3D12_FEATURE_DATA_D3D12_OPTIONS" /> for the device.</summary>
        public ref readonly D3D12_FEATURE_DATA_D3D12_OPTIONS D3D12Options => ref _d3d12Options.ValueRef;

        /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the device for render target resources.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12DescriptorHeap* D3D12RenderTargetDescriptorHeap => _d3d12RenderTargetDescriptorHeap.Value;

        /// <summary>Gets the <see cref="IDXGISwapChain3" /> for the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public IDXGISwapChain3* DxgiSwapChain => _dxgiSwapChain.Value;

        /// <inheritdoc />
        public override D3D12GraphicsMemoryAllocator MemoryAllocator => _memoryAllocator.Value;

        /// <summary>Gets the <see cref="DXGI_FORMAT" /> used by <see cref="DxgiSwapChain" />.</summary>
        public DXGI_FORMAT SwapChainFormat => _swapChainFormat;

        /// <summary>Gets a fence that is used to wait for the device to become idle.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public D3D12GraphicsFence WaitForIdleGraphicsFence => _idleFence;

        /// <inheritdoc />
        public override D3D12GraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null)
            => CreatePipeline((D3D12GraphicsPipelineSignature)signature, (D3D12GraphicsShader?)vertexShader, (D3D12GraphicsShader?)pixelShader);

        private D3D12GraphicsPipeline CreatePipeline(D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader? vertexShader, D3D12GraphicsShader? pixelShader)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsPipeline(this, signature, vertexShader, pixelShader);
        }

        /// <inheritdoc />
        public override D3D12GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsPipelineSignature(this, inputs, resources);
        }

        /// <inheritdoc />
        public override D3D12GraphicsPrimitive CreatePrimitive(GraphicsPipeline pipeline, in GraphicsMemoryRegion<IGraphicsResource> vertexBufferView, in GraphicsMemoryRegion<IGraphicsResource> indexBufferView = default, ReadOnlySpan<GraphicsMemoryRegion<IGraphicsResource>> inputResourceRegions = default)
            => CreatePrimitive((D3D12GraphicsPipeline)pipeline, in vertexBufferView, in indexBufferView, inputResourceRegions);

        /// <inheritdoc cref="CreatePrimitive(GraphicsPipeline, in GraphicsMemoryRegion{IGraphicsResource}, in GraphicsMemoryRegion{IGraphicsResource}, ReadOnlySpan{GraphicsMemoryRegion{IGraphicsResource}})" />
        private D3D12GraphicsPrimitive CreatePrimitive(D3D12GraphicsPipeline pipeline, in GraphicsMemoryRegion<IGraphicsResource> vertexBufferView, in GraphicsMemoryRegion<IGraphicsResource> indexBufferView, ReadOnlySpan<GraphicsMemoryRegion<IGraphicsResource>> inputResourceRegions)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsPrimitive(this, pipeline, in vertexBufferView, in indexBufferView, inputResourceRegions);
        }

        /// <inheritdoc />
        public override D3D12GraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsShader(this, kind, bytecode, entryPointName);
        }

        /// <inheritdoc />
        public override void PresentFrame()
        {
            ThrowExternalExceptionIfFailed(DxgiSwapChain->Present(SyncInterval: 1, Flags: 0), nameof(IDXGISwapChain.Present));

            Signal(CurrentContext.Fence);
            _contextIndex = unchecked((int)DxgiSwapChain->GetCurrentBackBufferIndex());
        }

        /// <inheritdoc />
        public override void Signal(GraphicsFence fence)
            => Signal((D3D12GraphicsFence)fence);

        /// <inheritdoc cref="Signal(GraphicsFence)" />
        public void Signal(D3D12GraphicsFence fence)
            => ThrowExternalExceptionIfFailed(D3D12CommandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue), nameof(ID3D12CommandQueue.Signal));

        /// <inheritdoc />
        public override void WaitForIdle()
        {
            if (_d3d12CommandQueue.IsCreated)
            {
                var waitForIdleGraphicsFence = WaitForIdleGraphicsFence;

                ThrowExternalExceptionIfFailed(D3D12CommandQueue->Signal(waitForIdleGraphicsFence.D3D12Fence, waitForIdleGraphicsFence.D3D12FenceSignalValue), nameof(ID3D12CommandQueue.Signal));

                waitForIdleGraphicsFence.Wait();
                waitForIdleGraphicsFence.Reset();
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
                _d3d12RenderTargetDescriptorHeap.Dispose(ReleaseIfNotNull);
                _dxgiSwapChain.Dispose(ReleaseIfNotNull);
                _d3d12CommandQueue.Dispose(ReleaseIfNotNull);

                _idleFence?.Dispose();

                _d3d12Device.Dispose(ReleaseIfNotNull);
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12CommandQueue> CreateD3D12CommandQueue()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12CommandQueue* d3d12CommandQueue;

            var commandQueueDesc = new D3D12_COMMAND_QUEUE_DESC();
            var iid = IID_ID3D12CommandQueue;
            ThrowExternalExceptionIfFailed(D3D12Device->CreateCommandQueue(&commandQueueDesc, &iid, (void**)&d3d12CommandQueue), nameof(ID3D12Device.CreateCommandQueue));

            return d3d12CommandQueue;
        }

        private Pointer<ID3D12Device> CreateD3D12Device()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12Device* d3d12Device;

            var iid = IID_ID3D12Device;
            ThrowExternalExceptionIfFailed(D3D12CreateDevice((IUnknown*)Adapter.DxgiAdapter, D3D_FEATURE_LEVEL_11_0, &iid, (void**)&d3d12Device), nameof(D3D12CreateDevice));

            return d3d12Device;
        }

        private Pointer<ID3D12DescriptorHeap> CreateD3D12RenderTargetDescriptorHeap()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12DescriptorHeap* renderTargetDescriptorHeap;

            var renderTargetDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                NumDescriptors = (uint)Contexts.Length,
            };

            var iid = IID_ID3D12DescriptorHeap;
            ThrowExternalExceptionIfFailed(D3D12Device->CreateDescriptorHeap(&renderTargetDescriptorHeapDesc, &iid, (void**)&renderTargetDescriptorHeap), nameof(ID3D12Device.CreateDescriptorHeap));

            return renderTargetDescriptorHeap;
        }

        private Pointer<IDXGISwapChain3> CreateDxgiSwapChain()
        {
            _state.ThrowIfDisposedOrDisposing();

            IDXGISwapChain3* dxgiSwapChain;

            var surface = Surface;
            var surfaceHandle = surface.Handle;

            var swapChainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                Width = (uint)surface.Width,
                Height = (uint)surface.Height,
                Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                BufferCount = (uint)Contexts.Length,
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
            };

            var provider = Adapter.Provider;
            var iid = IID_IDXGISwapChain3;

            switch (surface.Kind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    ThrowExternalExceptionIfFailed(provider.DxgiFactory->CreateSwapChainForHwnd((IUnknown*)D3D12CommandQueue, surfaceHandle, &swapChainDesc, pFullscreenDesc: null, pRestrictToOutput: null, (IDXGISwapChain1**)&dxgiSwapChain), nameof(IDXGIFactory2.CreateSwapChainForHwnd));
                    break;
                }

                default:
                {
                    ThrowInvalidOperationException(surface, nameof(surface));
                    dxgiSwapChain = null;
                    break;
                }
            }

            // Fullscreen transitions are not currently supported
            ThrowExternalExceptionIfFailed(provider.DxgiFactory->MakeWindowAssociation(surfaceHandle, DXGI_MWA_NO_ALT_ENTER), nameof(IDXGIFactory.MakeWindowAssociation));

            _swapChainFormat = swapChainDesc.Format;
            _contextIndex = unchecked((int)dxgiSwapChain->GetCurrentBackBufferIndex());

            return dxgiSwapChain;
        }

        private D3D12GraphicsMemoryAllocator CreateMemoryAllocator()
            => new D3D12GraphicsMemoryAllocator(this, blockPreferredSize: 0);

        private uint GetCbvSrvUavDescriptorHandleIncrementSize()
            => D3D12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);

        private void DisposeMemoryAllocator(D3D12GraphicsMemoryAllocator memoryAllocator) => memoryAllocator?.Dispose();

        private D3D12_FEATURE_DATA_D3D12_OPTIONS GetD3D12Options()
        {
            _state.ThrowIfDisposedOrDisposing();

            D3D12_FEATURE_DATA_D3D12_OPTIONS d3d12Options;
            ThrowExternalExceptionIfFailed(D3D12Device->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS, &d3d12Options, SizeOf<D3D12_FEATURE_DATA_D3D12_OPTIONS>()), nameof(ID3D12Device.CheckFeatureSupport));
            return d3d12Options;
        }

        private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
        {
            WaitForIdle();

            foreach (var context in Contexts)
            {
                ((D3D12GraphicsContext)context).OnGraphicsSurfaceSizeChanged(sender, eventArgs);
            }

            if (_dxgiSwapChain.IsCreated)
            {
                var surface = Surface;
                ThrowExternalExceptionIfFailed(DxgiSwapChain->ResizeBuffers((uint)Contexts.Length, (uint)surface.Width, (uint)surface.Height, DXGI_FORMAT_R8G8B8A8_UNORM, SwapChainFlags: 0), nameof(IDXGISwapChain.ResizeBuffers));
                _contextIndex = unchecked((int)DxgiSwapChain->GetCurrentBackBufferIndex());
            }
        }
    }
}
