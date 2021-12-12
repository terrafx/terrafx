// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_FEATURE;
using static TerraFX.Interop.DirectX.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsDevice : GraphicsDevice
{
    private readonly ID3D12CommandQueue* _d3d12CommandQueue;
    private readonly ID3D12Device* _d3d12Device;
    private readonly D3D12_FEATURE_DATA_D3D12_OPTIONS _d3d12Options;

    private readonly uint _d3d12CbvSrvUavDescriptorHandleIncrementSize;
    private readonly uint _d3d12RtvDescriptorHandleIncrementSize;

    private readonly D3D12GraphicsMemoryAllocator _memoryAllocator;
    private readonly D3D12GraphicsFence _waitForIdleFence;

    private ContextPool<D3D12GraphicsDevice, D3D12GraphicsRenderContext> _renderContextPool;
    private VolatileState _state;

    internal D3D12GraphicsDevice(D3D12GraphicsAdapter adapter)
        : base(adapter)
    {
        var d3d12Device = CreateD3D12Device(adapter);

        _d3d12CommandQueue = CreateD3D12CommandQueue(d3d12Device);
        _d3d12Device = d3d12Device;
        _d3d12Options = GetD3D12Options(d3d12Device);

        _d3d12CbvSrvUavDescriptorHandleIncrementSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);
        _d3d12RtvDescriptorHandleIncrementSize = d3d12Device->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);

        _memoryAllocator = CreateMemoryAllocator(this);
        _waitForIdleFence = CreateFence(isSignalled: false);

        _renderContextPool = new ContextPool<D3D12GraphicsDevice, D3D12GraphicsRenderContext>();
        _ = _state.Transition(to: Initialized);

        static ID3D12CommandQueue* CreateD3D12CommandQueue(ID3D12Device* d3d12Device)
        {
            ID3D12CommandQueue* d3d12CommandQueue;
            var commandQueueDesc = new D3D12_COMMAND_QUEUE_DESC();

            ThrowExternalExceptionIfFailed(d3d12Device->CreateCommandQueue(&commandQueueDesc, __uuidof<ID3D12CommandQueue>(), (void**)&d3d12CommandQueue));
            return d3d12CommandQueue;
        }

        static ID3D12Device* CreateD3D12Device(D3D12GraphicsAdapter adapter)
        {
            ID3D12Device* d3d12Device;
            ThrowExternalExceptionIfFailed(D3D12CreateDevice((IUnknown*)adapter.DxgiAdapter, D3D_FEATURE_LEVEL_11_0, __uuidof<ID3D12Device>(), (void**)&d3d12Device));
            return d3d12Device;
        }

        static D3D12GraphicsMemoryAllocator CreateMemoryAllocator(D3D12GraphicsDevice device)
        {
            var allocatorSettings = default(GraphicsMemoryAllocatorSettings);
            return new D3D12GraphicsMemoryAllocator(device, in allocatorSettings);
        }

        static D3D12_FEATURE_DATA_D3D12_OPTIONS GetD3D12Options(ID3D12Device* d3d12Device)
        {
            D3D12_FEATURE_DATA_D3D12_OPTIONS d3d12Options;
            ThrowExternalExceptionIfFailed(d3d12Device->CheckFeatureSupport(D3D12_FEATURE_D3D12_OPTIONS, &d3d12Options, SizeOf<D3D12_FEATURE_DATA_D3D12_OPTIONS>()));
            return d3d12Options;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsDevice" /> class.</summary>
    ~D3D12GraphicsDevice() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDevice.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="ID3D12CommandQueue" /> used by the device.</summary>
    public ID3D12CommandQueue* D3D12CommandQueue
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12CommandQueue;
        }
    }

    /// <summary>Gets the underlying <see cref="ID3D12Device" /> for the device.</summary>
    public ID3D12Device* D3D12Device
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12Device;
        }
    }

    /// <summary>Gets the <see cref="D3D12_FEATURE_DATA_D3D12_OPTIONS" /> for the device.</summary>
    public ref readonly D3D12_FEATURE_DATA_D3D12_OPTIONS D3D12Options => ref _d3d12Options;

    /// <summary>Gets the descriptor handle increment size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV" />.</summary>
    public uint D3D12CbvSrvUavDescriptorHandleIncrementSize => _d3d12CbvSrvUavDescriptorHandleIncrementSize;

    /// <summary>Gets the descriptor handle increment size for <see cref="D3D12_DESCRIPTOR_HEAP_TYPE_RTV" />.</summary>
    public uint D3D12RtvDescriptorHandleIncrementSize => _d3d12RtvDescriptorHandleIncrementSize;

    /// <inheritdoc />
    public override D3D12GraphicsMemoryAllocator MemoryAllocator => _memoryAllocator;

    /// <inheritdoc cref="GraphicsDevice.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <summary>Gets a fence that is used to wait for the device to become idle.</summary>
    public D3D12GraphicsFence WaitForIdleFence => _waitForIdleFence;

    /// <inheritdoc />
    public override D3D12GraphicsFence CreateFence(bool isSignalled)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsFence(this, isSignalled);
    }

    /// <inheritdoc />
    public override D3D12GraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null)
        => CreatePipeline((D3D12GraphicsPipelineSignature)signature, (D3D12GraphicsShader?)vertexShader, (D3D12GraphicsShader?)pixelShader);

    private D3D12GraphicsPipeline CreatePipeline(D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader? vertexShader, D3D12GraphicsShader? pixelShader)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsPipeline(this, signature, vertexShader, pixelShader);
    }

    /// <inheritdoc />
    public override D3D12GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsPipelineSignature(this, inputs, resources);
    }

    /// <inheritdoc />
    public override D3D12GraphicsPrimitive CreatePrimitive(GraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView = default, ReadOnlySpan<GraphicsResourceView> inputResourceViews = default)
        => CreatePrimitive((D3D12GraphicsPipeline)pipeline, in vertexBufferView, in indexBufferView, inputResourceViews);

    /// <inheritdoc cref="CreatePrimitive(GraphicsPipeline, in GraphicsResourceView, in GraphicsResourceView, ReadOnlySpan{GraphicsResourceView})" />
    private D3D12GraphicsPrimitive CreatePrimitive(D3D12GraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView = default, ReadOnlySpan<GraphicsResourceView> inputResourceViews = default)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsPrimitive(this, pipeline, in vertexBufferView, in indexBufferView, inputResourceViews);
    }

    /// <inheritdoc />
    public override D3D12GraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsShader(this, kind, bytecode, entryPointName);
    }

    /// <inheritdoc />
    public override D3D12GraphicsSwapchain CreateSwapchain(IGraphicsSurface surface)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsSwapchain(this, surface);
    }

    /// <inheritdoc />
    public override D3D12GraphicsRenderContext RentRenderContext()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return _renderContextPool.Rent(this, &CreateRenderContext);

        static D3D12GraphicsRenderContext CreateRenderContext(D3D12GraphicsDevice device)
        {
            AssertNotNull(device);
            return new D3D12GraphicsRenderContext(device);
        }
    }

    /// <inheritdoc />
    public override void ReturnRenderContext(GraphicsRenderContext renderContext)
        => ReturnRenderContext((D3D12GraphicsRenderContext)renderContext);

    /// <inheritdoc cref="ReturnRenderContext(GraphicsRenderContext)" />
    public void ReturnRenderContext(D3D12GraphicsRenderContext renderContext)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        ThrowIfNull(renderContext);

        if (renderContext.Device != this)
        {
            ThrowForInvalidParent(renderContext.Device);
        }
        _renderContextPool.Return(renderContext);
    }

    /// <inheritdoc />
    public override void Signal(GraphicsFence fence)
        => Signal((D3D12GraphicsFence)fence);

    /// <inheritdoc cref="Signal(GraphicsFence)" />
    public void Signal(D3D12GraphicsFence fence)
        => ThrowExternalExceptionIfFailed(D3D12CommandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue));

    /// <inheritdoc />
    public override void WaitForIdle()
    {
        var waitForIdleFence = WaitForIdleFence;

        ThrowExternalExceptionIfFailed(_d3d12CommandQueue->Signal(waitForIdleFence.D3D12Fence, waitForIdleFence.D3D12FenceSignalValue));

        waitForIdleFence.Wait();
        waitForIdleFence.Reset();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            WaitForIdle();

            if (isDisposing)
            {
                _renderContextPool.Dispose();
                _memoryAllocator?.Dispose();
                _waitForIdleFence?.Dispose();
            }

            ReleaseIfNotNull(_d3d12CommandQueue);
            ReleaseIfNotNull(_d3d12Device);
        }

        _state.EndDispose();
    }
}
