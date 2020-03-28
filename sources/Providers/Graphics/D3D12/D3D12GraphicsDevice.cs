// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.DXGI_SWAP_EFFECT;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsDevice : GraphicsDevice
    {
        private readonly D3D12GraphicsFence _idleGraphicsFence;

        private ValueLazy<Pointer<ID3D12CommandQueue>> _d3d12CommandQueue;
        private ValueLazy<Pointer<ID3D12Device>> _d3d12Device;
        private ValueLazy<Pointer<ID3D12DescriptorHeap>> _d3d12RenderTargetDescriptorHeap;
        private ValueLazy<Pointer<ID3D12DescriptorHeap>> _d3d12ShaderResourceDescriptorHeap;
        private ValueLazy<Pointer<IDXGISwapChain3>> _dxgiSwapChain;

        private D3D12GraphicsContext[] _graphicsContexts;
        private int _graphicsContextIndex;
        private DXGI_FORMAT _dxgiSwapChainFormat;

        private State _state;

        internal D3D12GraphicsDevice(D3D12GraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface, int graphicsContextCount)
            : base(graphicsAdapter, graphicsSurface)
        {
            _idleGraphicsFence = new D3D12GraphicsFence(this);

            _d3d12CommandQueue = new ValueLazy<Pointer<ID3D12CommandQueue>>(CreateD3D12CommandQueue);
            _d3d12Device = new ValueLazy<Pointer<ID3D12Device>>(CreateD3D12Device);
            _d3d12RenderTargetDescriptorHeap = new ValueLazy<Pointer<ID3D12DescriptorHeap>>(CreateD3D12RenderTargetDescriptorHeap);
            _d3d12ShaderResourceDescriptorHeap = new ValueLazy<Pointer<ID3D12DescriptorHeap>>(CreateD3D12ShaderResourceDescriptorHeap);
            _dxgiSwapChain = new ValueLazy<Pointer<IDXGISwapChain3>>(CreateDxgiSwapChain);

            _graphicsContexts = CreateGraphicsContexts(this, graphicsContextCount);

            _ = _state.Transition(to: Initialized);

            WaitForIdleGraphicsFence.Reset();
            graphicsSurface.SizeChanged += OnGraphicsSurfaceSizeChanged;

            static D3D12GraphicsContext[] CreateGraphicsContexts(D3D12GraphicsDevice graphicsDevice, int graphicsContextCount)
            {
                var graphicsContexts = new D3D12GraphicsContext[graphicsContextCount];

                for (var index = 0; index < graphicsContexts.Length; index++)
                {
                    graphicsContexts[index] = new D3D12GraphicsContext(graphicsDevice, index);
                }

                return graphicsContexts;
            }
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsDevice" /> class.</summary>
        ~D3D12GraphicsDevice()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="ID3D12CommandQueue" /> used by the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12CommandQueue* D3D12CommandQueue => _d3d12CommandQueue.Value;

        /// <inheritdoc cref="GraphicsDevice.CurrentGraphicsContext" />
        public D3D12GraphicsContext D3D12CurrentGraphicsContext => (D3D12GraphicsContext)CurrentGraphicsContext;

        /// <summary>Gets the underlying <see cref="ID3D12Device" /> for the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12Device* D3D12Device => _d3d12Device.Value;

        /// <inheritdoc cref="GraphicsDevice.GraphicsAdapter" />
        public D3D12GraphicsAdapter D3D12GraphicsAdapter => (D3D12GraphicsAdapter)GraphicsAdapter;

        /// <inheritdoc cref="GraphicsContexts" />
        public ReadOnlySpan<D3D12GraphicsContext> D3D12GraphicsContexts => _graphicsContexts;

        /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the device for render target resources.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12DescriptorHeap* D3D12RenderTargetDescriptorHeap => _d3d12RenderTargetDescriptorHeap.Value;

        /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the device for shader resources.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12DescriptorHeap* D3D12ShaderResourceDescriptorHeap => _d3d12ShaderResourceDescriptorHeap.Value;

        /// <summary>Gets the <see cref="IDXGISwapChain3" /> for the device.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public IDXGISwapChain3* DxgiSwapChain => _dxgiSwapChain.Value;

        /// <summary>Gets the <see cref="DXGI_FORMAT" /> used by <see cref="DxgiSwapChain" />.</summary>
        public DXGI_FORMAT DxgiSwapChainFormat => _dxgiSwapChainFormat;

        /// <inheritdoc />
        public override ReadOnlySpan<GraphicsContext> GraphicsContexts => _graphicsContexts;

        /// <inheritdoc />
        public override int GraphicsContextIndex => _graphicsContextIndex;

        /// <summary>Gets a graphics fence that is used to wait for the device to become idle.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public D3D12GraphicsFence WaitForIdleGraphicsFence => _idleGraphicsFence;

        /// <inheritdoc cref="CreateGraphicsHeap(ulong, GraphicsHeapCpuAccess)" />
        public D3D12GraphicsHeap CreateD3D12GraphicsHeap(ulong size, GraphicsHeapCpuAccess cpuAccess)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsHeap(this, size, cpuAccess);
        }

        /// <inheritdoc cref="CreateGraphicsPipeline(GraphicsPipelineSignature, GraphicsShader?, GraphicsShader?)" />
        public D3D12GraphicsPipeline CreateD3D12GraphicsPipeline(D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader ? vertexShader = null, D3D12GraphicsShader? pixelShader = null)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsPipeline(this, signature, vertexShader, pixelShader);
        }

        /// <inheritdoc cref="CreateGraphicsPrimitive(GraphicsPipeline, GraphicsBuffer, GraphicsBuffer, ReadOnlySpan{GraphicsResource})" />
        public D3D12GraphicsPrimitive CreateD3D12GraphicsPrimitive(D3D12GraphicsPipeline graphicsPipeline, D3D12GraphicsBuffer vertexBuffer, D3D12GraphicsBuffer? indexBuffer = null, ReadOnlySpan<GraphicsResource> inputResources = default)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsPrimitive(this, graphicsPipeline, vertexBuffer, indexBuffer, inputResources);
        }

        /// <inheritdoc cref="CreateGraphicsShader(GraphicsShaderKind, ReadOnlySpan{byte}, string)" />
        public D3D12GraphicsShader CreateD3D12GraphicsShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsShader(this, kind, bytecode, entryPointName);
        }

        /// <inheritdoc />
        public override GraphicsHeap CreateGraphicsHeap(ulong size, GraphicsHeapCpuAccess cpuAccess) => CreateD3D12GraphicsHeap(size, cpuAccess);

        /// <inheritdoc />
        public override GraphicsPipeline CreateGraphicsPipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null) => CreateD3D12GraphicsPipeline((D3D12GraphicsPipelineSignature)signature, (D3D12GraphicsShader?)vertexShader, (D3D12GraphicsShader?)pixelShader);

        /// <inheritdoc />
        public override GraphicsPipelineSignature CreateGraphicsPipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsPipelineSignature(this, inputs, resources);
        }

        /// <inheritdoc />
        public override GraphicsPrimitive CreateGraphicsPrimitive(GraphicsPipeline graphicsPipeline, GraphicsBuffer vertexBuffer, GraphicsBuffer? indexBuffer = null, ReadOnlySpan<GraphicsResource> inputResources = default) => CreateD3D12GraphicsPrimitive((D3D12GraphicsPipeline)graphicsPipeline, (D3D12GraphicsBuffer)vertexBuffer, (D3D12GraphicsBuffer?)indexBuffer, inputResources);

        /// <inheritdoc />
        public override GraphicsShader CreateGraphicsShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName) => CreateD3D12GraphicsShader(kind, bytecode, entryPointName);

        /// <inheritdoc />
        public override void PresentFrame()
        {
            ThrowExternalExceptionIfFailed(nameof(IDXGISwapChain.Present), DxgiSwapChain->Present(1, 0));

            Signal(D3D12CurrentGraphicsContext.D3D12GraphicsFence);
            _graphicsContextIndex = unchecked((int)DxgiSwapChain->GetCurrentBackBufferIndex());
        }

        /// <inheritdoc cref="Signal(GraphicsFence)" />
        public void Signal(D3D12GraphicsFence graphicsFence)
            => ThrowExternalExceptionIfFailed(nameof(ID3D12CommandQueue.Signal), D3D12CommandQueue->Signal(graphicsFence.D3D12Fence, graphicsFence.D3D12FenceSignalValue));

        /// <inheritdoc />
        public override void Signal(GraphicsFence graphicsFence) => Signal((D3D12GraphicsFence)graphicsFence);

        /// <inheritdoc />
        public override void WaitForIdle()
        {
            if (_d3d12CommandQueue.IsCreated)
            {
                var waitForIdleGraphicsFence = WaitForIdleGraphicsFence;

                ThrowExternalExceptionIfFailed(nameof(ID3D12CommandQueue.Signal), D3D12CommandQueue->Signal(waitForIdleGraphicsFence.D3D12Fence, waitForIdleGraphicsFence.D3D12FenceSignalValue));

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
                DisposeIfNotNull(_graphicsContexts);

                _d3d12ShaderResourceDescriptorHeap.Dispose(ReleaseIfNotNull);
                _d3d12RenderTargetDescriptorHeap.Dispose(ReleaseIfNotNull);
                _dxgiSwapChain.Dispose(ReleaseIfNotNull);
                _d3d12CommandQueue.Dispose(ReleaseIfNotNull);

                DisposeIfNotNull(_idleGraphicsFence);

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
            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateCommandQueue), D3D12Device->CreateCommandQueue(&commandQueueDesc, &iid, (void**)&d3d12CommandQueue));

            return d3d12CommandQueue;
        }

        private Pointer<ID3D12Device> CreateD3D12Device()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12Device* d3d12Device;

            var iid = IID_ID3D12Device;
            ThrowExternalExceptionIfFailed(nameof(D3D12CreateDevice), D3D12CreateDevice((IUnknown*)D3D12GraphicsAdapter.DxgiAdapter, D3D_FEATURE_LEVEL_11_0, &iid, (void**)&d3d12Device));

            return d3d12Device;
        }

        private Pointer<ID3D12DescriptorHeap> CreateD3D12RenderTargetDescriptorHeap()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12DescriptorHeap* renderTargetDescriptorHeap;

            var renderTargetDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV,
                NumDescriptors = (uint)D3D12GraphicsContexts.Length,
            };

            var iid = IID_ID3D12DescriptorHeap;
            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateDescriptorHeap), D3D12Device->CreateDescriptorHeap(&renderTargetDescriptorHeapDesc, &iid, (void**)&renderTargetDescriptorHeap));

            return renderTargetDescriptorHeap;
        }

        private Pointer<ID3D12DescriptorHeap> CreateD3D12ShaderResourceDescriptorHeap()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12DescriptorHeap* shaderResourceDescriptorHeap;

            var shaderResourceDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV,
                NumDescriptors = 1,
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE,
            };

            var iid = IID_ID3D12DescriptorHeap;
            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateDescriptorHeap), D3D12Device->CreateDescriptorHeap(&shaderResourceDescriptorHeapDesc, &iid, (void**)&shaderResourceDescriptorHeap));

            return shaderResourceDescriptorHeap;
        }

        private Pointer<IDXGISwapChain3> CreateDxgiSwapChain()
        {
            _state.ThrowIfDisposedOrDisposing();

            IDXGISwapChain3* dxgiSwapChain;

            var graphicsSurface = GraphicsSurface;
            var graphicsSurfaceHandle = graphicsSurface.SurfaceHandle;

            var swapChainDesc = new DXGI_SWAP_CHAIN_DESC1 {
                Width = (uint)graphicsSurface.Width,
                Height = (uint)graphicsSurface.Height,
                Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                BufferCount = (uint)D3D12GraphicsContexts.Length,
                SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
            };

            var graphicsProvider = D3D12GraphicsAdapter.D3D12GraphicsProvider;
            var iid = IID_IDXGISwapChain3;

            switch (graphicsSurface.SurfaceKind)
            {
                case GraphicsSurfaceKind.Win32:
                {
                    ThrowExternalExceptionIfFailed(nameof(IDXGIFactory2.CreateSwapChainForHwnd), graphicsProvider.DxgiFactory->CreateSwapChainForHwnd((IUnknown*)D3D12CommandQueue, graphicsSurfaceHandle, &swapChainDesc, pFullscreenDesc: null, pRestrictToOutput: null, (IDXGISwapChain1**)&dxgiSwapChain));
                    break;
                }

                default:
                {
                    ThrowInvalidOperationException(nameof(graphicsSurface), graphicsSurface);
                    dxgiSwapChain = null;
                    break;
                }
            }

            // Fullscreen transitions are not currently supported
            ThrowExternalExceptionIfFailed(nameof(IDXGIFactory.MakeWindowAssociation), graphicsProvider.DxgiFactory->MakeWindowAssociation(graphicsSurfaceHandle, DXGI_MWA_NO_ALT_ENTER));

            _dxgiSwapChainFormat = DXGI_FORMAT_R8G8B8A8_UNORM;
            return dxgiSwapChain;
        }

        /// <inheritdoc />
        private void OnGraphicsSurfaceSizeChanged(object? sender, PropertyChangedEventArgs<Vector2> eventArgs)
        {
            WaitForIdle();

            foreach (var graphicsContext in D3D12GraphicsContexts)
            {
                graphicsContext.OnGraphicsSurfaceSizeChanged(sender, eventArgs);
            }

            if (_dxgiSwapChain.IsCreated)
            {
                var graphicsSurface = GraphicsSurface;
                ThrowExternalExceptionIfFailed(nameof(IDXGISwapChain.ResizeBuffers), DxgiSwapChain->ResizeBuffers((uint)D3D12GraphicsContexts.Length, (uint)graphicsSurface.Width, (uint)graphicsSurface.Height, DXGI_FORMAT_R8G8B8A8_UNORM, SwapChainFlags: 0));
            }
        }
    }
}
