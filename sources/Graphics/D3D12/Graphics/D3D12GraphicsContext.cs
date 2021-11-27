// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.D3D_PRIMITIVE_TOPOLOGY;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using System.Diagnostics;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsContext : GraphicsContext
{
    private const uint FrameInitializing = 2;

    private const uint FrameInitialized = 3;

    private const uint DrawingInitializing = 4;

    private const uint DrawingInitialized = 5;

    private readonly D3D12GraphicsFence _fence;

    private uint _framebufferIndex;
    private D3D12GraphicsSwapchain? _swapchain;

    private ValueLazy<Pointer<ID3D12CommandAllocator>> _d3d12CommandAllocator;
    private ValueLazy<Pointer<ID3D12GraphicsCommandList>> _d3d12GraphicsCommandList;

    private VolatileState _state;

    internal D3D12GraphicsContext(D3D12GraphicsDevice device)
        : base(device)
    {
        _fence = new D3D12GraphicsFence(device);

        _d3d12CommandAllocator = new ValueLazy<Pointer<ID3D12CommandAllocator>>(CreateD3D12CommandAllocator);
        _d3d12GraphicsCommandList = new ValueLazy<Pointer<ID3D12GraphicsCommandList>>(CreateD3D12GraphicsCommandList);

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsContext" /> class.</summary>
    ~D3D12GraphicsContext() => Dispose(isDisposing: false);

    /// <summary>Gets the <see cref="ID3D12CommandAllocator" /> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public ID3D12CommandAllocator* D3D12CommandAllocator => _d3d12CommandAllocator.Value;

    /// <summary>Gets the <see cref="ID3D12GraphicsCommandList" /> used by the context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public ID3D12GraphicsCommandList* D3D12GraphicsCommandList => _d3d12GraphicsCommandList.Value;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

    /// <inheritdoc />
    public override D3D12GraphicsFence Fence => _fence;

    /// <inheritdoc />
    public override D3D12GraphicsSwapchain? Swapchain => _swapchain;

    /// <inheritdoc />
    public override void BeginDrawing(uint framebufferIndex, ColorRgba backgroundColor)
    {
        _state.Transition(from: FrameInitialized, to: DrawingInitializing);
        Debug.Assert(Swapchain is not null);

        var commandList = D3D12GraphicsCommandList;
        var swapchain = Swapchain;

        var rtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(swapchain.D3D12RenderTargetResources[framebufferIndex], D3D12_RESOURCE_STATE_PRESENT, D3D12_RESOURCE_STATE_RENDER_TARGET);
        commandList->ResourceBarrier(1, &rtvResourceBarrier);

        var rtvDescriptor = swapchain.D3D12RtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart().Offset((int)framebufferIndex, Device.D3D12RtvDescriptorHandleIncrementSize);
        commandList->OMSetRenderTargets(1, &rtvDescriptor, RTsSingleHandleToDescriptorRange: TRUE, pDepthStencilDescriptor: null);

        var surface = swapchain.Surface;

        var surfaceWidth = surface.Width;
        var surfaceHeight = surface.Height;

        var viewport = new D3D12_VIEWPORT {
            Width = surfaceWidth,
            Height = surfaceHeight,
            MinDepth = D3D12_MIN_DEPTH,
            MaxDepth = D3D12_MAX_DEPTH,
        };
        commandList->RSSetViewports(1, &viewport);

        var scissorRect = new RECT {
            right = (int)surfaceWidth,
            bottom = (int)surfaceHeight,
        };
        commandList->RSSetScissorRects(1, &scissorRect);

        commandList->ClearRenderTargetView(rtvDescriptor, (float*)&backgroundColor, NumRects: 0, pRects: null);
        commandList->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

        _framebufferIndex = framebufferIndex;
        _state.Transition(from: DrawingInitializing, to: DrawingInitialized);
    }

    /// <inheritdoc />
    public override void BeginFrame(GraphicsSwapchain swapchain)
        => BeginFrame((D3D12GraphicsSwapchain)swapchain);

    /// <inheritdoc cref="BeginFrame(GraphicsSwapchain)" />
    public void BeginFrame(D3D12GraphicsSwapchain swapchain)
    {
        ThrowIfNull(swapchain);

        _state.Transition(from: Initialized, to: FrameInitializing);
        _swapchain = swapchain;

        Fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;

        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset());
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null));

        _state.Transition(from: FrameInitializing, to: FrameInitialized);
    }

    /// <inheritdoc />
    public override void Copy(GraphicsBuffer destination, GraphicsBuffer source)
        => Copy((D3D12GraphicsBuffer)destination, (D3D12GraphicsBuffer)source);

    /// <inheritdoc />
    public override void Copy(GraphicsTexture destination, GraphicsBuffer source)
        => Copy((D3D12GraphicsTexture)destination, (D3D12GraphicsBuffer)source);

    /// <inheritdoc cref="Copy(GraphicsBuffer, GraphicsBuffer)" />
    public void Copy(D3D12GraphicsBuffer destination, D3D12GraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        if (_state < FrameInitialized)
        {
            ThrowInvalidOperationException("GraphicsContext.BeginFrame has not been called");
        }

        var commandList = D3D12GraphicsCommandList;

        var destinationCpuAccess = destination.CpuAccess;
        var sourceCpuAccess = source.CpuAccess;

        var d3d12DestinationResource = destination.D3D12Resource;
        var d3d12SourceResource = source.D3D12Resource;

        var d3d12DestinationResourceState = destination.D3D12ResourceState;
        var d3d12SourceResourceState = source.D3D12ResourceState;

        BeginCopy();

        commandList->CopyResource(d3d12DestinationResource, d3d12SourceResource);

        EndCopy();

        void BeginCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: d3d12DestinationResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_DEST
                );
                numResourceBarriers++;
            }

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: d3d12SourceResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_SOURCE
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }

        void EndCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_SOURCE,
                    stateAfter: d3d12SourceResourceState
                );
                numResourceBarriers++;
            }

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_DEST,
                    stateAfter: d3d12DestinationResourceState
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }
    }

    /// <inheritdoc cref="Copy(GraphicsTexture, GraphicsBuffer)" />
    public void Copy(D3D12GraphicsTexture destination, D3D12GraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        if (_state < FrameInitialized)
        {
            ThrowInvalidOperationException("GraphicsContext.BeginFrame has not been called");
        }

        var device = Device.D3D12Device;
        var commandList = D3D12GraphicsCommandList;

        var destinationCpuAccess = destination.CpuAccess;
        var sourceCpuAccess = source.CpuAccess;

        var d3d12DestinationResource = destination.D3D12Resource;
        var d3d12SourceResource = source.D3D12Resource;

        var d3d12DestinationResourceState = destination.D3D12ResourceState;
        var d3d12SourceResourceState = source.D3D12ResourceState;

        BeginCopy();

        D3D12_PLACED_SUBRESOURCE_FOOTPRINT sourceFootprint;

        var destinationDesc = d3d12DestinationResource->GetDesc();
        device->GetCopyableFootprints(&destinationDesc, FirstSubresource: 0, NumSubresources: 1, BaseOffset: 0, &sourceFootprint, pNumRows: null, pRowSizeInBytes: null, pTotalBytes: null);

        var d3d12DestinationTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(d3d12DestinationResource, Sub: 0);
        var d3d12SourceTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(d3d12SourceResource, in sourceFootprint);

        commandList->CopyTextureRegion(&d3d12DestinationTextureCopyLocation, DstX: 0, DstY: 0, DstZ: 0, &d3d12SourceTextureCopyLocation, pSrcBox: null);

        EndCopy();

        void BeginCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: d3d12DestinationResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_DEST
                );
                numResourceBarriers++;
            }

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: d3d12SourceResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_SOURCE
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }

        void EndCopy()
        {
            var resourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numResourceBarriers = 0u;

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_SOURCE,
                    stateAfter: d3d12SourceResourceState
                );
                numResourceBarriers++;
            }

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                resourceBarriers[numResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_DEST,
                    stateAfter: d3d12DestinationResourceState
                );
                numResourceBarriers++;
            }

            if (numResourceBarriers != 0)
            {
                commandList->ResourceBarrier(numResourceBarriers, resourceBarriers);
            }
        }
    }

    /// <inheritdoc />
    public override void Draw(GraphicsPrimitive primitive)
        => Draw((D3D12GraphicsPrimitive)primitive);

    /// <inheritdoc cref="Draw(GraphicsPrimitive)" />
    public void Draw(D3D12GraphicsPrimitive primitive)
    {
        ThrowIfNull(primitive);

        if (_state < DrawingInitialized)
        {
            ThrowInvalidOperationException("GraphicsContext.BeginDraw has not been called");
        }

        var commandList = D3D12GraphicsCommandList;
        var pipeline = primitive.Pipeline;

        commandList->SetGraphicsRootSignature(pipeline.Signature.D3D12RootSignature);
        commandList->SetPipelineState(pipeline.D3D12PipelineState);

        var descriptorHeaps = stackalloc ID3D12DescriptorHeap*[1] {
            primitive.D3D12CbvSrvUavDescriptorHeap,
        };
        commandList->SetDescriptorHeaps(1, descriptorHeaps);

        ref readonly var vertexBufferRegion = ref primitive.VertexBufferRegion;
        var vertexBuffer = (D3D12GraphicsBuffer)vertexBufferRegion.Collection;

        var d3d12VertexBufferView = new D3D12_VERTEX_BUFFER_VIEW {
            BufferLocation = vertexBuffer.D3D12Resource->GetGPUVirtualAddress() + vertexBufferRegion.Offset,
            StrideInBytes = primitive.VertexBufferStride,
            SizeInBytes = (uint)vertexBufferRegion.Size,
        };
        commandList->IASetVertexBuffers(StartSlot: 0, NumViews: 1, &d3d12VertexBufferView);

        var inputResourceRegions = primitive.InputResourceRegions;
        var inputResourceRegionsLength = inputResourceRegions.Length;

        var rootDescriptorTableIndex = 0;
        var cbvSrvUavDescriptorHandleIncrementSize = Device.D3D12CbvSrvUavDescriptorHandleIncrementSize;

        for (var index = 0; index < inputResourceRegionsLength; index++)
        {
            var inputResourceRegion = inputResourceRegions[index];

            if (inputResourceRegion.Collection is D3D12GraphicsBuffer d3d12GraphicsBuffer)
            {
                var gpuVirtualAddress = d3d12GraphicsBuffer.D3D12Resource->GetGPUVirtualAddress();
                commandList->SetGraphicsRootConstantBufferView(unchecked((uint)index), gpuVirtualAddress + inputResourceRegion.Offset);
            }
            else if (inputResourceRegion.Collection is D3D12GraphicsTexture d3d12GraphicsTexture)
            {
                var gpuDescriptorHandleForHeapStart = primitive.D3D12CbvSrvUavDescriptorHeap->GetGPUDescriptorHandleForHeapStart();
                commandList->SetGraphicsRootDescriptorTable(unchecked((uint)index), gpuDescriptorHandleForHeapStart.Offset(rootDescriptorTableIndex, cbvSrvUavDescriptorHandleIncrementSize));
                rootDescriptorTableIndex++;
            }
        }

        ref readonly var indexBufferRegion = ref primitive.IndexBufferRegion;

        if (indexBufferRegion.Collection is D3D12GraphicsBuffer indexBuffer)
        {
            var indexBufferStride = primitive.IndexBufferStride;
            var indexFormat = DXGI_FORMAT_R16_UINT;

            if (indexBufferStride != 2)
            {
                Assert(AssertionsEnabled && (indexBufferStride == 4));
                indexFormat = DXGI_FORMAT_R32_UINT;
            }

            var d3d12IndexBufferView = new D3D12_INDEX_BUFFER_VIEW {
                BufferLocation = indexBuffer.D3D12Resource->GetGPUVirtualAddress() + indexBufferRegion.Offset,
                SizeInBytes = (uint)indexBufferRegion.Size,
                Format = indexFormat,
            };
            commandList->IASetIndexBuffer(&d3d12IndexBufferView);

            commandList->DrawIndexedInstanced(IndexCountPerInstance: (uint)(indexBufferRegion.Size / indexBufferStride), InstanceCount: 1, StartIndexLocation: 0, BaseVertexLocation: 0, StartInstanceLocation: 0);
        }
        else
        {
            commandList->DrawInstanced(VertexCountPerInstance: (uint)(vertexBufferRegion.Size / primitive.VertexBufferStride), InstanceCount: 1, StartVertexLocation: 0, StartInstanceLocation: 0);
        }
    }

    /// <inheritdoc />
    public override void EndDrawing()
    {
        _state.Transition(from: DrawingInitialized, to: DrawingInitializing);

        Debug.Assert(Swapchain is not null);
        var renderTargetResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(Swapchain.D3D12RenderTargetResources[_framebufferIndex], D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
        D3D12GraphicsCommandList->ResourceBarrier(1, &renderTargetResourceBarrier);

        _state.Transition(from: DrawingInitializing, to: FrameInitialized);
    }

    /// <inheritdoc />
    public override void EndFrame()
    {
        _state.Transition(from: FrameInitialized, to: FrameInitializing);
        var commandList = D3D12GraphicsCommandList;

        var commandQueue = Device.D3D12CommandQueue;
        ThrowExternalExceptionIfFailed(commandList->Close());
        commandQueue->ExecuteCommandLists(1, (ID3D12CommandList**)&commandList);

        var fence = Fence;
        ThrowExternalExceptionIfFailed(commandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue));
        fence.Wait();

        _swapchain = null;
        _state.Transition(from: FrameInitializing, to: Initialized);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _d3d12GraphicsCommandList.Dispose(ReleaseIfNotNull);
            _d3d12CommandAllocator.Dispose(ReleaseIfNotNull);
            _fence?.Dispose();
        }

        _state.EndDispose();
    }

    private Pointer<ID3D12CommandAllocator> CreateD3D12CommandAllocator()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsContext));

        ID3D12CommandAllocator* d3d12CommandAllocator;
        ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, __uuidof<ID3D12CommandAllocator>(), (void**)&d3d12CommandAllocator));

        return d3d12CommandAllocator;
    }

    private Pointer<ID3D12GraphicsCommandList> CreateD3D12GraphicsCommandList()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsContext));

        ID3D12GraphicsCommandList* d3d12GraphicsCommandList;
        ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandList(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_DIRECT, D3D12CommandAllocator, pInitialState: null, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));

        // Command lists are created in the recording state, but there is nothing
        // to record yet. The main loop expects it to be closed, so close it now.
        ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());

        return d3d12GraphicsCommandList;
    }
}
