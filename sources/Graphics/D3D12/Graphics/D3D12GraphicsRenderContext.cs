// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
using static TerraFX.Utilities.UnsafeUtilities;
using System.Diagnostics;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsRenderContext : GraphicsRenderContext
{
    private const uint DrawingInitializing = 2;

    private const uint DrawingInitialized = 3;

    private readonly ID3D12CommandAllocator* _d3d12CommandAllocator;
    private readonly ID3D12GraphicsCommandList* _d3d12GraphicsCommandList;
    private readonly D3D12GraphicsFence _fence;

    private uint _framebufferIndex;
    private D3D12GraphicsSwapchain? _swapchain;

    private VolatileState _state;

    internal D3D12GraphicsRenderContext(D3D12GraphicsDevice device)
        : base(device)
    {
        var d3d12CommandAllocator = CreateD3D12CommandAllocator(device);
        _d3d12CommandAllocator = d3d12CommandAllocator;

        _d3d12GraphicsCommandList = CreateD3D12GraphicsCommandList(device, d3d12CommandAllocator);
        _fence = device.CreateFence(isSignalled: true);

        _ = _state.Transition(to: Initialized);

        static ID3D12CommandAllocator* CreateD3D12CommandAllocator(D3D12GraphicsDevice device)
        {
            ID3D12CommandAllocator* d3d12CommandAllocator;
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, __uuidof<ID3D12CommandAllocator>(), (void**)&d3d12CommandAllocator));
            return d3d12CommandAllocator;
        }

        static ID3D12GraphicsCommandList* CreateD3D12GraphicsCommandList(D3D12GraphicsDevice device, ID3D12CommandAllocator* d3d12CommandAllocator)
        {
            ID3D12GraphicsCommandList* d3d12GraphicsCommandList;
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateCommandList(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_DIRECT, d3d12CommandAllocator, pInitialState: null, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));

            // Command lists are created in the recording state, but there is nothing
            // to record yet. The main loop expects it to be closed, so close it now.
            ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());

            return d3d12GraphicsCommandList;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsRenderContext" /> class.</summary>
    ~D3D12GraphicsRenderContext() => Dispose(isDisposing: false);

    /// <summary>Gets the <see cref="ID3D12CommandAllocator" /> used by the context.</summary>
    public ID3D12CommandAllocator* D3D12CommandAllocator
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12CommandAllocator;
        }
    }

    /// <summary>Gets the <see cref="ID3D12GraphicsCommandList" /> used by the context.</summary>
    public ID3D12GraphicsCommandList* D3D12GraphicsCommandList
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12GraphicsCommandList;
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc />
    public override D3D12GraphicsFence Fence => _fence;

    /// <inheritdoc />
    public override D3D12GraphicsSwapchain? Swapchain => _swapchain;

    /// <inheritdoc />
    public override void BeginDrawing(uint framebufferIndex, ColorRgba backgroundColor)
    {
        _state.Transition(from: Initialized, to: DrawingInitializing);
        Debug.Assert(Swapchain is not null);

        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;
        var swapchain = Swapchain;

        var d3d12RtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(swapchain.D3D12RenderTargetResources[framebufferIndex], D3D12_RESOURCE_STATE_PRESENT, D3D12_RESOURCE_STATE_RENDER_TARGET);
        d3d12GraphicsCommandList->ResourceBarrier(1, &d3d12RtvResourceBarrier);

        var d3d12RtvDescriptor = swapchain.D3D12RtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart().Offset((int)framebufferIndex, Device.D3D12RtvDescriptorHandleIncrementSize);
        d3d12GraphicsCommandList->OMSetRenderTargets(1, &d3d12RtvDescriptor, RTsSingleHandleToDescriptorRange: TRUE, pDepthStencilDescriptor: null);

        var surface = swapchain.Surface;

        var surfaceWidth = surface.Width;
        var surfaceHeight = surface.Height;

        var d3d12Viewport = new D3D12_VIEWPORT {
            Width = surfaceWidth,
            Height = surfaceHeight,
            MinDepth = D3D12_MIN_DEPTH,
            MaxDepth = D3D12_MAX_DEPTH,
        };
        d3d12GraphicsCommandList->RSSetViewports(1, &d3d12Viewport);

        var d3d12ScissorRect = new RECT {
            right = (int)surfaceWidth,
            bottom = (int)surfaceHeight,
        };
        d3d12GraphicsCommandList->RSSetScissorRects(1, &d3d12ScissorRect);

        d3d12GraphicsCommandList->ClearRenderTargetView(d3d12RtvDescriptor, (float*)&backgroundColor, NumRects: 0, pRects: null);
        d3d12GraphicsCommandList->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

        _framebufferIndex = framebufferIndex;
        _state.Transition(from: DrawingInitializing, to: DrawingInitialized);
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

        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        var destinationCpuAccess = destination.CpuAccess;
        var sourceCpuAccess = source.CpuAccess;

        var d3d12DestinationResource = destination.D3D12Resource;
        var d3d12SourceResource = source.D3D12Resource;

        var d3d12DestinationResourceState = destination.D3D12ResourceState;
        var d3d12SourceResourceState = source.D3D12ResourceState;

        BeginCopy();

        d3d12GraphicsCommandList->CopyResource(d3d12DestinationResource, d3d12SourceResource);

        EndCopy();

        void BeginCopy()
        {
            var d3d12ResourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numD3D12ResourceBarriers = 0u;

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: d3d12DestinationResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_DEST
                );
                numD3D12ResourceBarriers++;
            }

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: d3d12SourceResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_SOURCE
                );
                numD3D12ResourceBarriers++;
            }

            if (numD3D12ResourceBarriers != 0)
            {
                d3d12GraphicsCommandList->ResourceBarrier(numD3D12ResourceBarriers, d3d12ResourceBarriers);
            }
        }

        void EndCopy()
        {
            var d3d12ResourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numD3D12ResourceBarriers = 0u;

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_SOURCE,
                    stateAfter: d3d12SourceResourceState
                );
                numD3D12ResourceBarriers++;
            }

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_DEST,
                    stateAfter: d3d12DestinationResourceState
                );
                numD3D12ResourceBarriers++;
            }

            if (numD3D12ResourceBarriers != 0)
            {
                d3d12GraphicsCommandList->ResourceBarrier(numD3D12ResourceBarriers, d3d12ResourceBarriers);
            }
        }
    }

    /// <inheritdoc cref="Copy(GraphicsTexture, GraphicsBuffer)" />
    public void Copy(D3D12GraphicsTexture destination, D3D12GraphicsBuffer source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        var d3d12Device = Device.D3D12Device;
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        var destinationCpuAccess = destination.CpuAccess;
        var sourceCpuAccess = source.CpuAccess;

        var d3d12DestinationResource = destination.D3D12Resource;
        var d3d12SourceResource = source.D3D12Resource;

        var d3d12DestinationResourceState = destination.D3D12ResourceState;
        var d3d12SourceResourceState = source.D3D12ResourceState;

        BeginCopy();

        D3D12_PLACED_SUBRESOURCE_FOOTPRINT sourceFootprint;

        var d3d12DestinationResourceDesc = d3d12DestinationResource->GetDesc();
        d3d12Device->GetCopyableFootprints(&d3d12DestinationResourceDesc, FirstSubresource: 0, NumSubresources: 1, BaseOffset: 0, &sourceFootprint, pNumRows: null, pRowSizeInBytes: null, pTotalBytes: null);

        var d3d12DestinationTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(d3d12DestinationResource, Sub: 0);
        var d3d12SourceTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(d3d12SourceResource, in sourceFootprint);

        d3d12GraphicsCommandList->CopyTextureRegion(&d3d12DestinationTextureCopyLocation, DstX: 0, DstY: 0, DstZ: 0, &d3d12SourceTextureCopyLocation, pSrcBox: null);

        EndCopy();

        void BeginCopy()
        {
            var d3d12ResourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numD3D12ResourceBarriers = 0u;

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: d3d12DestinationResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_DEST
                );
                numD3D12ResourceBarriers++;
            }

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: d3d12SourceResourceState,
                    stateAfter: D3D12_RESOURCE_STATE_COPY_SOURCE
                );
                numD3D12ResourceBarriers++;
            }

            if (numD3D12ResourceBarriers != 0)
            {
                d3d12GraphicsCommandList->ResourceBarrier(numD3D12ResourceBarriers, d3d12ResourceBarriers);
            }
        }

        void EndCopy()
        {
            var d3d12ResourceBarriers = stackalloc D3D12_RESOURCE_BARRIER[2];
            var numD3D12ResourceBarriers = 0u;

            if (sourceCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12SourceResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_SOURCE,
                    stateAfter: d3d12SourceResourceState
                );
                numD3D12ResourceBarriers++;
            }

            if (destinationCpuAccess == GraphicsResourceCpuAccess.None)
            {
                d3d12ResourceBarriers[numD3D12ResourceBarriers] = D3D12_RESOURCE_BARRIER.InitTransition(
                    d3d12DestinationResource,
                    stateBefore: D3D12_RESOURCE_STATE_COPY_DEST,
                    stateAfter: d3d12DestinationResourceState
                );
                numD3D12ResourceBarriers++;
            }

            if (numD3D12ResourceBarriers != 0)
            {
                d3d12GraphicsCommandList->ResourceBarrier(numD3D12ResourceBarriers, d3d12ResourceBarriers);
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

        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;
        var pipeline = primitive.Pipeline;

        d3d12GraphicsCommandList->SetGraphicsRootSignature(pipeline.Signature.D3D12RootSignature);
        d3d12GraphicsCommandList->SetPipelineState(pipeline.D3D12PipelineState);

        var d3d12DescriptorHeaps = stackalloc ID3D12DescriptorHeap*[1] {
            primitive.D3D12CbvSrvUavDescriptorHeap,
        };
        d3d12GraphicsCommandList->SetDescriptorHeaps(1, d3d12DescriptorHeaps);

        ref readonly var vertexBufferView = ref primitive.VertexBufferView;
        var vertexBuffer = vertexBufferView.Resource.As<D3D12GraphicsBuffer>();
        AssertNotNull(vertexBuffer);

        var d3d12VertexBufferView = new D3D12_VERTEX_BUFFER_VIEW {
            BufferLocation = vertexBuffer.D3D12Resource->GetGPUVirtualAddress() + vertexBufferView.Offset,
            StrideInBytes = primitive.VertexBufferView.Stride,
            SizeInBytes = vertexBufferView.Size,
        };
        d3d12GraphicsCommandList->IASetVertexBuffers(StartSlot: 0, NumViews: 1, &d3d12VertexBufferView);

        var inputResourceViews = primitive.InputResourceViews;

        var rootDescriptorTableIndex = 0;
        var cbvSrvUavDescriptorHandleIncrementSize = Device.D3D12CbvSrvUavDescriptorHandleIncrementSize;

        for (var index = 0; index < inputResourceViews.Length; index++)
        {
            ref readonly var inputResourceView = ref inputResourceViews[index];

            if (inputResourceView.Resource is D3D12GraphicsBuffer d3d12GraphicsBuffer)
            {
                var gpuVirtualAddress = d3d12GraphicsBuffer.D3D12Resource->GetGPUVirtualAddress();
                d3d12GraphicsCommandList->SetGraphicsRootConstantBufferView(unchecked((uint)index), gpuVirtualAddress + inputResourceView.Offset);
            }
            else if (inputResourceView.Resource is D3D12GraphicsTexture d3d12GraphicsTexture)
            {
                var gpuDescriptorHandleForHeapStart = primitive.D3D12CbvSrvUavDescriptorHeap->GetGPUDescriptorHandleForHeapStart();
                d3d12GraphicsCommandList->SetGraphicsRootDescriptorTable(unchecked((uint)index), gpuDescriptorHandleForHeapStart.Offset(rootDescriptorTableIndex, cbvSrvUavDescriptorHandleIncrementSize));
                rootDescriptorTableIndex++;
            }
        }

        ref readonly var indexBufferView = ref primitive.IndexBufferView;

        if (indexBufferView.Resource is D3D12GraphicsBuffer indexBuffer)
        {
            var indexBufferStride = indexBufferView.Stride;
            var indexFormat = DXGI_FORMAT_R16_UINT;

            if (indexBufferStride != 2)
            {
                Assert(AssertionsEnabled && (indexBufferStride == 4));
                indexFormat = DXGI_FORMAT_R32_UINT;
            }

            var d3d12IndexBufferView = new D3D12_INDEX_BUFFER_VIEW {
                BufferLocation = indexBuffer.D3D12Resource->GetGPUVirtualAddress() + indexBufferView.Offset,
                SizeInBytes = (uint)indexBufferView.Size,
                Format = indexFormat,
            };
            d3d12GraphicsCommandList->IASetIndexBuffer(&d3d12IndexBufferView);

            d3d12GraphicsCommandList->DrawIndexedInstanced(IndexCountPerInstance: (uint)(indexBufferView.Size / indexBufferStride), InstanceCount: 1, StartIndexLocation: 0, BaseVertexLocation: 0, StartInstanceLocation: 0);
        }
        else
        {
            d3d12GraphicsCommandList->DrawInstanced(VertexCountPerInstance: (uint)(vertexBufferView.Size /  vertexBufferView.Stride), InstanceCount: 1, StartVertexLocation: 0, StartInstanceLocation: 0);
        }
    }

    /// <inheritdoc />
    public override void EndDrawing()
    {
        _state.Transition(from: DrawingInitialized, to: DrawingInitializing);

        Debug.Assert(Swapchain is not null);
        var d3d12RtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(Swapchain.D3D12RenderTargetResources[_framebufferIndex], D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
        D3D12GraphicsCommandList->ResourceBarrier(1, &d3d12RtvResourceBarrier);

        _state.Transition(from: DrawingInitializing, to: Initialized);
    }

    /// <inheritdoc />
    public override void Flush()
    {
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        var d3d12CommandQueue = Device.D3D12CommandQueue;
        ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());
        d3d12CommandQueue->ExecuteCommandLists(1, (ID3D12CommandList**)&d3d12GraphicsCommandList);

        var fence = Fence;
        ThrowExternalExceptionIfFailed(d3d12CommandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue));
        fence.Wait();
    }

    /// <inheritdoc />
    public override void Reset()
    {
        _swapchain = null;

        Fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;

        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset());
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null));
    }

    /// <inheritdoc />
    public override void SetSwapchain(GraphicsSwapchain swapchain)
        => SetSwapchain((D3D12GraphicsSwapchain)swapchain);

    /// <inheritdoc cref="SetSwapchain(GraphicsSwapchain)" />
    public void SetSwapchain(D3D12GraphicsSwapchain swapchain)
    {
        ThrowIfNull(swapchain);

        if (swapchain.Device != Device)
        {
            ThrowForInvalidParent(swapchain.Device);
        }

        _swapchain = swapchain;
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            ReleaseIfNotNull(_d3d12GraphicsCommandList);
            ReleaseIfNotNull(_d3d12CommandAllocator);

            if (isDisposing)
            {
                _fence?.Dispose();
            }
        }

        _state.EndDispose();
    }
}
