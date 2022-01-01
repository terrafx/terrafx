// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using static TerraFX.Interop.DirectX.D3D_PRIMITIVE_TOPOLOGY;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Interop.DirectX.D3D12_COMMAND_LIST_FLAGS;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsRenderContext : GraphicsRenderContext
{
    private ID3D12CommandAllocator* _d3d12CommandAllocator;
    private readonly uint _d3d12CommandAllocatorVersion;

    private ID3D12GraphicsCommandList* _d3d12GraphicsCommandList;
    private readonly uint _d3d12GraphicsCommandListVersion;

    private D3D12GraphicsRenderPass? _renderPass;

    internal D3D12GraphicsRenderContext(D3D12GraphicsRenderCommandQueue renderCommandQueue) : base(renderCommandQueue)
    {
        // No need for a ContextPool.AddComputeContext(this) as it will be done by the underlying pool

        ContextInfo.Fence = Device.CreateFence(isSignalled: true);

        _d3d12CommandAllocator = CreateD3D12CommandAllocator(out _d3d12CommandAllocatorVersion);
        _d3d12GraphicsCommandList = CreateD3D12GraphicsCommandList(out _d3d12GraphicsCommandListVersion);

        SetNameUnsafe(Name);

        ID3D12CommandAllocator* CreateD3D12CommandAllocator(out uint d3d12CommandAllocatorVersion)
        {
            ID3D12CommandAllocator* d3d12CommandAllocator;
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, __uuidof<ID3D12CommandAllocator>(), (void**)&d3d12CommandAllocator));
            return GetLatestD3D12CommandAllocator(d3d12CommandAllocator, out d3d12CommandAllocatorVersion);
        }

        ID3D12GraphicsCommandList* CreateD3D12GraphicsCommandList(out uint d3d12GraphicsCommandListVersion)
        {
            ID3D12GraphicsCommandList* d3d12GraphicsCommandList;

            if (Device.D3D12DeviceVersion >= 4)
            {
                var d3d12Device4 = (ID3D12Device4*)Device.D3D12Device;
                ThrowExternalExceptionIfFailed(d3d12Device4->CreateCommandList1(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_DIRECT, D3D12_COMMAND_LIST_FLAG_NONE, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));
            }
            else
            {
                var d3d12Device = Device.D3D12Device;
                ThrowExternalExceptionIfFailed(d3d12Device->CreateCommandList(nodeMask: 0, D3D12_COMMAND_LIST_TYPE_DIRECT, _d3d12CommandAllocator, pInitialState: null, __uuidof<ID3D12GraphicsCommandList>(), (void**)&d3d12GraphicsCommandList));

                // Command lists are created in the recording state, but there is nothing
                // to record yet. The main loop expects it to be closed, so close it now.
                ThrowExternalExceptionIfFailed(d3d12GraphicsCommandList->Close());
            }

            return GetLatestD3D12GraphicsCommandList(d3d12GraphicsCommandList, out d3d12GraphicsCommandListVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsRenderContext" /> class.</summary>
    ~D3D12GraphicsRenderContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new D3D12GraphicsRenderCommandQueue CommandQueue => base.CommandQueue.As<D3D12GraphicsRenderCommandQueue>();

    /// <summary>Gets the <see cref="ID3D12CommandAllocator" /> used by the copy context.</summary>
    public ID3D12CommandAllocator* D3D12CommandAllocator => _d3d12CommandAllocator;

    /// <summary>Gets the interface version of <see cref="D3D12CommandAllocator" />.</summary>
    public uint D3D12CommandAllocatorVersion => _d3d12CommandAllocatorVersion;

    /// <summary>Gets the <see cref="ID3D12GraphicsCommandList" /> used by the copy context.</summary>
    public ID3D12GraphicsCommandList* D3D12GraphicsCommandList => _d3d12GraphicsCommandList;

    /// <summary>Gets the interface version of <see cref="D3D12GraphicsCommandList" />.</summary>
    public uint D3D12GraphicsCommandListVersion => _d3d12GraphicsCommandListVersion;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsContext{TGraphicsContext}.Fence" />
    public new D3D12GraphicsFence Fence => base.Fence.As<D3D12GraphicsFence>();

    /// <inheritdoc />
    public override uint MaxBoundVertexBufferViewCount => D3D12_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT;

    /// <inheritdoc />
    public override D3D12GraphicsRenderPass? RenderPass => _renderPass;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
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
        var renderTarget = renderPass.Swapchain.CurrentRenderTarget;

        var d3d12RtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(renderTarget.D3D12RtvResource, D3D12_RESOURCE_STATE_PRESENT, D3D12_RESOURCE_STATE_RENDER_TARGET);
        d3d12GraphicsCommandList->ResourceBarrier(1, &d3d12RtvResourceBarrier);

        var d3d12RtvDescriptorHandle = renderTarget.D3D12RtvDescriptorHandle;
        d3d12GraphicsCommandList->OMSetRenderTargets(1, &d3d12RtvDescriptorHandle, RTsSingleHandleToDescriptorRange: TRUE, pDepthStencilDescriptor: null);

        d3d12GraphicsCommandList->ClearRenderTargetView(d3d12RtvDescriptorHandle, (float*)&renderTargetClearColor, NumRects: 0, pRects: null);
        d3d12GraphicsCommandList->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
    }

    /// <inheritdoc />
    public override void BindIndexBufferView(GraphicsBufferView indexBufferView)
        => BindIndexBufferView((D3D12GraphicsBufferView)indexBufferView);

    /// <inheritdoc cref="BindIndexBufferView(GraphicsBufferView)" />
    public void BindIndexBufferView(D3D12GraphicsBufferView indexBufferView)
    {
        ThrowIfNull(indexBufferView);

        var d3d12IndexBufferView = new D3D12_INDEX_BUFFER_VIEW {
            BufferLocation = indexBufferView.GpuVirtualAddress,
            SizeInBytes = checked((uint)indexBufferView.ByteLength),
            Format = indexBufferView.BytesPerElement == 2 ? DXGI_FORMAT_R16_UINT : DXGI_FORMAT_R32_UINT,
        };

        D3D12GraphicsCommandList->IASetIndexBuffer(&d3d12IndexBufferView);
    }

    /// <inheritdoc />
    public override void BindPipeline(GraphicsPipeline pipeline)
        => BindPipeline((D3D12GraphicsPipeline)pipeline);

    /// <inheritdoc cref="BindPipeline(GraphicsPipeline)" />
    public void BindPipeline(D3D12GraphicsPipeline pipeline)
    {
        ThrowIfNull(pipeline);
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        d3d12GraphicsCommandList->SetPipelineState(pipeline.D3D12PipelineState);
        d3d12GraphicsCommandList->SetGraphicsRootSignature(pipeline.Signature.D3D12RootSignature);
    }

    /// <inheritdoc />
    public override void BindPipelineDescriptorSet(GraphicsPipelineDescriptorSet pipelineDescriptorSet)
        => BindPipeline((D3D12GraphicsPipelineDescriptorSet)pipelineDescriptorSet);

    /// <inheritdoc cref="BindPipelineDescriptorSet(GraphicsPipelineDescriptorSet)" />
    public void BindPipeline(D3D12GraphicsPipelineDescriptorSet pipelineDescriptorSet)
    {
        ThrowIfNull(pipelineDescriptorSet);
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        var d3d12CbvSrvUavDescriptorHeap = pipelineDescriptorSet.D3D12CbvSrvUavDescriptorHeap;
        d3d12GraphicsCommandList->SetDescriptorHeaps(1, &d3d12CbvSrvUavDescriptorHeap);

        var resourceViews = pipelineDescriptorSet.ResourceViews;

        var rootDescriptorTableIndex = 0;
        var cbvSrvUavDescriptorHandleIncrementSize = Device.CbvSrvUavDescriptorSize;

        for (var index = 0; index < resourceViews.Length; index++)
        {
            var resourceView = resourceViews[index];

            if (resourceView is D3D12GraphicsBufferView d3d12GraphicsBufferView)
            {
                d3d12GraphicsCommandList->SetGraphicsRootConstantBufferView(unchecked((uint)index), d3d12GraphicsBufferView.GpuVirtualAddress);
            }
            else if (resourceView is D3D12GraphicsTextureView d3d12GraphicsTextureView)
            {
                var gpuDescriptorHandleForHeapStart = d3d12CbvSrvUavDescriptorHeap->GetGPUDescriptorHandleForHeapStart();
                d3d12GraphicsCommandList->SetGraphicsRootDescriptorTable(unchecked((uint)index), gpuDescriptorHandleForHeapStart.Offset(rootDescriptorTableIndex, cbvSrvUavDescriptorHandleIncrementSize));
                rootDescriptorTableIndex++;
            }
        }
    }

    /// <inheritdoc />
    public override void BindVertexBufferView(GraphicsBufferView vertexBufferView, uint bindingSlot = 0)
        => BindVertexBufferView((D3D12GraphicsBufferView)vertexBufferView, bindingSlot);

    /// <inheritdoc cref="BindVertexBufferView(GraphicsBufferView, uint)" />
    public void BindVertexBufferView(D3D12GraphicsBufferView vertexBufferView, uint bindingSlot = 0)
    {
        ThrowIfNull(vertexBufferView);

        var d3d12VertexBufferView = new D3D12_VERTEX_BUFFER_VIEW {
            BufferLocation = vertexBufferView.GpuVirtualAddress,
            StrideInBytes = vertexBufferView.BytesPerElement,
            SizeInBytes = checked((uint)vertexBufferView.ByteLength),
        };

        D3D12GraphicsCommandList->IASetVertexBuffers(bindingSlot, NumViews: 1, &d3d12VertexBufferView);
    }

    /// <inheritdoc />
    public override void BindVertexBufferViews(ReadOnlySpan<GraphicsBufferView> vertexBufferViews, uint firstBindingSlot)
    {
        ThrowIfZero(vertexBufferViews.Length);
        ThrowIfNotInInsertBounds(vertexBufferViews.Length, D3D12_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT);

        var d3d12VertexBufferViews = stackalloc D3D12_VERTEX_BUFFER_VIEW[D3D12_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT];

        for (var index = 0; index < vertexBufferViews.Length; index++)
        {
            var vertexBufferView = (D3D12GraphicsBufferView)vertexBufferViews[index];
            ThrowIfNull(vertexBufferView);

            d3d12VertexBufferViews[index] = new D3D12_VERTEX_BUFFER_VIEW {
                BufferLocation = vertexBufferView.GpuVirtualAddress,
                StrideInBytes = vertexBufferView.BytesPerElement,
                SizeInBytes = checked((uint)vertexBufferView.ByteLength),
            };
        }

        D3D12GraphicsCommandList->IASetVertexBuffers(firstBindingSlot, NumViews: (uint)vertexBufferViews.Length, d3d12VertexBufferViews);
    }

    /// <inheritdoc />
    public override void Draw(uint verticesPerInstance, uint instanceCount = 1, uint vertexStart = 0, uint instanceStart = 0)
    {
        ThrowIfZero(verticesPerInstance);
        ThrowIfZero(instanceCount);

        D3D12GraphicsCommandList->DrawInstanced(verticesPerInstance, instanceCount, vertexStart, instanceStart);
    }

    /// <inheritdoc />
    public override void DrawIndexed(uint indicesPerInstance, uint instanceCount = 1, uint indexStart = 0, int vertexStart = 0, uint instanceStart = 0)
    {
        ThrowIfZero(indicesPerInstance);
        ThrowIfZero(instanceCount);

        D3D12GraphicsCommandList->DrawIndexedInstanced(indicesPerInstance, instanceCount, indexStart, vertexStart, instanceStart);
    }

    /// <inheritdoc />
    public override void EndRenderPass()
    {
        var renderPass = Interlocked.Exchange(ref _renderPass, null);

        if (renderPass is null)
        {
            ThrowForInvalidState(nameof(RenderPass));
        }

        var d3d12RtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(renderPass.Swapchain.CurrentRenderTarget.D3D12RtvResource, D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
        D3D12GraphicsCommandList->ResourceBarrier(1, &d3d12RtvResourceBarrier);
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
    protected override void CloseUnsafe()
    {
        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Close());
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = ContextInfo.Fence;
            fence.Wait();
            fence.Reset();

            fence.Dispose();
            ContextInfo.Fence = null!;
        }

        ReleaseIfNotNull(_d3d12GraphicsCommandList);
        _d3d12GraphicsCommandList = null;

        ReleaseIfNotNull(_d3d12CommandAllocator);
        _d3d12CommandAllocator = null;

        _ = CommandQueue.RemoveRenderContext(this);
    }

    /// <inheritdoc />
    protected override void ExecuteUnsafe()
    {
        CommandQueue.ExecuteContextUnsafe(this);
    }

    /// <inheritdoc />
    protected override void ResetUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        var d3d12CommandAllocator = D3D12CommandAllocator;
        ThrowExternalExceptionIfFailed(d3d12CommandAllocator->Reset());

        ThrowExternalExceptionIfFailed(D3D12GraphicsCommandList->Reset(d3d12CommandAllocator, pInitialState: null));
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12CommandAllocator->SetD3D12Name(value);
        D3D12GraphicsCommandList->SetD3D12Name(value);
    }
}
