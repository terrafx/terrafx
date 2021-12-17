// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D_PRIMITIVE_TOPOLOGY;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsRenderContext : GraphicsRenderContext
{
    private readonly ID3D12CommandAllocator* _d3d12CommandAllocator;
    private readonly ID3D12GraphicsCommandList* _d3d12GraphicsCommandList;
    private readonly D3D12GraphicsFence _fence;

    private string _name = null!;
    private D3D12GraphicsRenderPass? _renderPass;

    private VolatileState _state;

    internal D3D12GraphicsRenderContext(D3D12GraphicsDevice device)
        : base(device)
    {
        var d3d12CommandAllocator = CreateD3D12CommandAllocator(device);
        _d3d12CommandAllocator = d3d12CommandAllocator;

        _d3d12GraphicsCommandList = CreateD3D12GraphicsCommandList(device, d3d12CommandAllocator);
        _fence = device.CreateFence(isSignalled: true);

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsRenderContext);

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

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

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

    /// <summary>Gets or sets the name for the pipeline signature.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = D3D12CommandAllocator->UpdateD3D12Name(value);
            _ = D3D12GraphicsCommandList->UpdateD3D12Name(value);
        }
    }

    /// <inheritdoc />
    public override D3D12GraphicsRenderPass? RenderPass => _renderPass;

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override void BeginRenderPass(GraphicsRenderPass renderPass, ColorRgba renderTargetClearColor)
        => BeginRenderPass((D3D12GraphicsRenderPass)renderPass, renderTargetClearColor);

    /// <inheritdoc cref="BeginRenderPass(GraphicsRenderPass, ColorRgba)" />
    public void BeginRenderPass(D3D12GraphicsRenderPass renderPass, ColorRgba renderTargetClearColor)
    {
        ThrowIfNull(renderPass);

        if (Interlocked.CompareExchange(ref _renderPass, renderPass, null) is not null)
        {
            ThrowForInvalidState(nameof(RenderPass));
        }

        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;
        var renderTarget = renderPass.Swapchain.RenderTarget;

        var d3d12RtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(renderTarget.D3D12RtvResource, D3D12_RESOURCE_STATE_PRESENT, D3D12_RESOURCE_STATE_RENDER_TARGET);
        d3d12GraphicsCommandList->ResourceBarrier(1, &d3d12RtvResourceBarrier);

        var d3d12RtvDescriptorHandle = renderTarget.D3D12RtvDescriptorHandle;
        d3d12GraphicsCommandList->OMSetRenderTargets(1, &d3d12RtvDescriptorHandle, RTsSingleHandleToDescriptorRange: TRUE, pDepthStencilDescriptor: null);

        d3d12GraphicsCommandList->ClearRenderTargetView(d3d12RtvDescriptorHandle, (float*)&renderTargetClearColor, NumRects: 0, pRects: null);
        d3d12GraphicsCommandList->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
    }

    /// <inheritdoc />
    public override void Draw(GraphicsPrimitive primitive)
        => Draw((D3D12GraphicsPrimitive)primitive);

    /// <inheritdoc cref="Draw(GraphicsPrimitive)" />
    public void Draw(D3D12GraphicsPrimitive primitive)
    {
        ThrowIfNull(primitive);

        var renderPass = RenderPass;

        if (renderPass is null)
        {
            ThrowForInvalidState(nameof(RenderPass));
        }

        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;
        var pipeline = primitive.Pipeline;

        d3d12GraphicsCommandList->SetGraphicsRootSignature(pipeline.Signature.D3D12RootSignature);
        d3d12GraphicsCommandList->SetPipelineState(pipeline.D3D12PipelineState);

        var d3d12DescriptorHeaps = stackalloc ID3D12DescriptorHeap*[1] {
            primitive.D3D12CbvSrvUavDescriptorHeap,
        };
        d3d12GraphicsCommandList->SetDescriptorHeaps(1, d3d12DescriptorHeaps);

        var vertexBufferView = primitive.VertexBufferView;

        var d3d12VertexBufferView = new D3D12_VERTEX_BUFFER_VIEW {
            BufferLocation = vertexBufferView.D3D12ResourceGpuVirtualAddress,
            StrideInBytes = vertexBufferView.Stride,
            SizeInBytes = checked((uint)vertexBufferView.Size),
        };
        d3d12GraphicsCommandList->IASetVertexBuffers(StartSlot: 0, NumViews: 1, &d3d12VertexBufferView);

        var inputResourceViews = primitive.InputResourceViews;

        var rootDescriptorTableIndex = 0;
        var cbvSrvUavDescriptorHandleIncrementSize = Device.D3D12CbvSrvUavDescriptorHandleIncrementSize;

        for (var index = 0; index < inputResourceViews.Length; index++)
        {
            var inputResourceView = inputResourceViews[index];

            if (inputResourceView is D3D12GraphicsBufferView d3d12GraphicsBufferView)
            {
                d3d12GraphicsCommandList->SetGraphicsRootConstantBufferView(unchecked((uint)index), d3d12GraphicsBufferView.D3D12ResourceGpuVirtualAddress);
            }
            else if (inputResourceView is D3D12GraphicsTextureView d3d12GraphicsTextureView)
            {
                var gpuDescriptorHandleForHeapStart = primitive.D3D12CbvSrvUavDescriptorHeap->GetGPUDescriptorHandleForHeapStart();
                d3d12GraphicsCommandList->SetGraphicsRootDescriptorTable(unchecked((uint)index), gpuDescriptorHandleForHeapStart.Offset(rootDescriptorTableIndex, cbvSrvUavDescriptorHandleIncrementSize));
                rootDescriptorTableIndex++;
            }
        }

        var indexBufferView = primitive.IndexBufferView;

        if (indexBufferView is not null)
        {
            var d3d12IndexBufferView = new D3D12_INDEX_BUFFER_VIEW {
                BufferLocation = indexBufferView.D3D12ResourceGpuVirtualAddress,
                SizeInBytes = checked((uint)indexBufferView.Size),
                Format = indexBufferView.Stride == 2 ? DXGI_FORMAT_R16_UINT : DXGI_FORMAT_R32_UINT,
            };
            d3d12GraphicsCommandList->IASetIndexBuffer(&d3d12IndexBufferView);

            d3d12GraphicsCommandList->DrawIndexedInstanced(IndexCountPerInstance: (uint)(indexBufferView.Size / indexBufferView.Stride), InstanceCount: 1, StartIndexLocation: 0, BaseVertexLocation: 0, StartInstanceLocation: 0);
        }
        else
        {
            d3d12GraphicsCommandList->DrawInstanced(VertexCountPerInstance: (uint)(vertexBufferView.Size /  vertexBufferView.Stride), InstanceCount: 1, StartVertexLocation: 0, StartInstanceLocation: 0);
        }
    }

    /// <inheritdoc />
    public override void EndRenderPass()
    {
        var renderPass = Interlocked.Exchange(ref _renderPass, null);

        if (renderPass is null)
        {
            ThrowForInvalidState(nameof(RenderPass));
        }

        var d3d12RtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(renderPass.Swapchain.RenderTarget.D3D12RtvResource, D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
        D3D12GraphicsCommandList->ResourceBarrier(1, &d3d12RtvResourceBarrier);
    }

    /// <inheritdoc />
    public override void Flush()
    {
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        var d3d12CommandQueue = Device.D3D12DirectCommandQueue;
        ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());
        d3d12CommandQueue->ExecuteCommandLists(1, (ID3D12CommandList**)&d3d12GraphicsCommandList);

        var fence = Fence;
        ThrowExternalExceptionIfFailed(d3d12CommandQueue->Signal(fence.D3D12Fence, fence.D3D12FenceSignalValue));
        fence.Wait();
    }

    /// <inheritdoc />
    public override void Reset()
    {
        Fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;

        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset());
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null));
    }

    /// <inheritdoc />
    public override void SetScissor(BoundingRectangle scissor)
    {
        var topLeft = scissor.Location;
        var bottomRight = topLeft + scissor.Size;

        var d3d12Rect = new RECT {
            left = (int)topLeft.X,
            top = (int)topLeft.Y,
            right = (int)bottomRight.X,
            bottom = (int)bottomRight.Y,
        };
        D3D12GraphicsCommandList->RSSetScissorRects(NumRects: 1, &d3d12Rect);
    }

    /// <inheritdoc />
    public override void SetScissors(ReadOnlySpan<BoundingRectangle> scissors)
    {
        var count = (uint)scissors.Length;
        var d3d12Rects = AllocateArray<RECT>(count);

        for (var i = 0u; i < count; i++)
        {
            ref readonly var scissor = ref scissors[(int)i];

            var upperLeft = scissor.Location;
            var bottomRight = upperLeft + scissor.Size;

            d3d12Rects[i] = new RECT {
                left = (int)upperLeft.X,
                top = (int)upperLeft.Y,
                right = (int)bottomRight.X,
                bottom = (int)bottomRight.Y,
            };
        }
        D3D12GraphicsCommandList->RSSetScissorRects(count, d3d12Rects);
    }

    /// <inheritdoc />
    public override void SetViewport(BoundingBox viewport)
    {
        var location = viewport.Location;
        var size = viewport.Size;

        var d3d12Viewport = new D3D12_VIEWPORT {
            TopLeftX = location.X,
            TopLeftY = location.Y,
            Width = size.X,
            Height = size.Y,
            MinDepth = location.Z,
            MaxDepth = size.Z,
        };
        D3D12GraphicsCommandList->RSSetViewports(NumViewports: 1, &d3d12Viewport);
    }

    /// <inheritdoc />
    public override void SetViewports(ReadOnlySpan<BoundingBox> viewports)
    {
        var count = (uint)viewports.Length;
        var d3d12Viewports = AllocateArray<D3D12_VIEWPORT>(count);

        for (var i = 0u; i < count; i++)
        {
            ref readonly var viewport = ref viewports[(int)i];

            var location = viewport.Location;
            var size = viewport.Size;

            d3d12Viewports[i] = new D3D12_VIEWPORT {
                TopLeftX = location.X,
                TopLeftY = location.Y,
                Width = size.X,
                Height = size.Y,
                MinDepth = location.Z,
                MaxDepth = size.Z,
            };
        }
        D3D12GraphicsCommandList->RSSetViewports(count, d3d12Viewports);
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
