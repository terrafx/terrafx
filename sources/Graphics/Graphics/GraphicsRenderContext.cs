// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Numerics;
using static TerraFX.Interop.DirectX.D3D_PRIMITIVE_TOPOLOGY;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing render commands.</summary>
public sealed unsafe class GraphicsRenderContext : GraphicsContext
{
    private GraphicsRenderPass? _renderPass;

    internal GraphicsRenderContext(GraphicsRenderCommandQueue renderCommandQueue) : base(renderCommandQueue, GraphicsContextKind.Render)
    {
    }

    /// <inheritdoc cref="GraphicsCommandQueueObject.CommandQueue" />
    public new GraphicsRenderCommandQueue CommandQueue => base.CommandQueue.As<GraphicsRenderCommandQueue>();

    /// <summary>Gets the maximum number of vertex buffer views that can be bound at one time.</summary>
    public uint MaxBoundVertexBufferViewCount => D3D12_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT;

    /// <summary>Gets the current render pass for the context or <c>null</c> if one has not been set.</summary>
    public GraphicsRenderPass? RenderPass => _renderPass;

    /// <summary>Begins a render pass.</summary>
    /// <param name="renderPass">The render pass to begin.</param>
    /// <param name="renderTargetClearColor">The color to which the render target should be cleared.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">A render pass is already active.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void BeginRenderPass(GraphicsRenderPass renderPass, ColorRgba renderTargetClearColor)
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

    /// <summary>Binds an index buffer view to the context.</summary>
    /// <param name="indexBufferView">The index buffer view to set.</param>
    /// <exception cref="ArgumentNullException"><paramref name="indexBufferView" /> is <c>null</c>.</exception>
    public void BindIndexBufferView(GraphicsBufferView indexBufferView)
    {
        ThrowIfNull(indexBufferView);

        var d3d12IndexBufferView = new D3D12_INDEX_BUFFER_VIEW {
            BufferLocation = indexBufferView.D3D12GpuVirtualAddress,
            SizeInBytes = checked((uint)indexBufferView.ByteLength),
            Format = indexBufferView.BytesPerElement == 2 ? DXGI_FORMAT_R16_UINT : DXGI_FORMAT_R32_UINT,
        };

        D3D12GraphicsCommandList->IASetIndexBuffer(&d3d12IndexBufferView);
    }

    /// <summary>Binds a pipeline to the context.</summary>
    /// <param name="pipeline">The pipeline to bind.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    public void BindPipeline(GraphicsPipeline pipeline)
    {
        ThrowIfNull(pipeline);
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        d3d12GraphicsCommandList->SetPipelineState(pipeline.D3D12PipelineState);
        d3d12GraphicsCommandList->SetGraphicsRootSignature(pipeline.Signature.D3D12RootSignature);
    }

    /// <summary>Binds a pipeline descriptor set to the context.</summary>
    /// <param name="pipelineDescriptorSet">The pipeline descriptor set to bind.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipelineDescriptorSet" /> is <c>null</c>.</exception>
    public void BindPipelineDescriptorSet(GraphicsPipelineDescriptorSet pipelineDescriptorSet)
    {
        ThrowIfNull(pipelineDescriptorSet);
        var d3d12GraphicsCommandList = D3D12GraphicsCommandList;

        var d3d12CbvSrvUavDescriptorHeap = pipelineDescriptorSet.D3D12CbvSrvUavDescriptorHeap;
        d3d12GraphicsCommandList->SetDescriptorHeaps(1, &d3d12CbvSrvUavDescriptorHeap);

        var resourceViews = pipelineDescriptorSet.ResourceViews;

        var rootDescriptorTableIndex = 0;
        var cbvSrvUavDescriptorHandleIncrementSize = Device.D3D12CbvSrvUavDescriptorSize;

        for (var index = 0; index < resourceViews.Length; index++)
        {
            var gpuDescriptorHandleForHeapStart = d3d12CbvSrvUavDescriptorHeap->GetGPUDescriptorHandleForHeapStart();
            d3d12GraphicsCommandList->SetGraphicsRootDescriptorTable(unchecked((uint)index), gpuDescriptorHandleForHeapStart.Offset(rootDescriptorTableIndex, cbvSrvUavDescriptorHandleIncrementSize));
            rootDescriptorTableIndex++;
        }
    }

    /// <summary>Binds a vertex buffer view to the context.</summary>
    /// <param name="vertexBufferView">The vertex buffer view to bind.</param>
    /// <param name="bindingSlot">The binding slot to which <paramref name="vertexBufferView" /> should be bound.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vertexBufferView" /> is <c>null</c>.</exception>
    public void BindVertexBufferView(GraphicsBufferView vertexBufferView, uint bindingSlot = 0)
    {
        ThrowIfNull(vertexBufferView);

        var d3d12VertexBufferView = new D3D12_VERTEX_BUFFER_VIEW {
            BufferLocation = vertexBufferView.D3D12GpuVirtualAddress,
            StrideInBytes = vertexBufferView.BytesPerElement,
            SizeInBytes = checked((uint)vertexBufferView.ByteLength),
        };

        D3D12GraphicsCommandList->IASetVertexBuffers(bindingSlot, NumViews: 1, &d3d12VertexBufferView);
    }

    /// <summary>Binds vertex buffer views to the context.</summary>
    /// <param name="vertexBufferViews">The vertex buffer views to bind.</param>
    /// <param name="firstBindingSlot">The first binding slot to which <paramref name="vertexBufferViews" /> should be bound.</param>
    /// <exception cref="ArgumentNullException">One of the items in <paramref name="vertexBufferViews" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferViews" /> is <c>empty</c> or greater than <see cref="MaxBoundVertexBufferViewCount" />.</exception>
    public void BindVertexBufferViews(ReadOnlySpan<GraphicsBufferView> vertexBufferViews, uint firstBindingSlot)
    {
        ThrowIfZero(vertexBufferViews.Length);
        ThrowIfNotInInsertBounds(vertexBufferViews.Length, D3D12_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT);

        var d3d12VertexBufferViews = stackalloc D3D12_VERTEX_BUFFER_VIEW[D3D12_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT];

        for (var index = 0; index < vertexBufferViews.Length; index++)
        {
            var vertexBufferView = (GraphicsBufferView)vertexBufferViews[index];
            ThrowIfNull(vertexBufferView);

            d3d12VertexBufferViews[index] = new D3D12_VERTEX_BUFFER_VIEW {
                BufferLocation = vertexBufferView.D3D12GpuVirtualAddress,
                StrideInBytes = vertexBufferView.BytesPerElement,
                SizeInBytes = checked((uint)vertexBufferView.ByteLength),
            };
        }

        D3D12GraphicsCommandList->IASetVertexBuffers(firstBindingSlot, NumViews: (uint)vertexBufferViews.Length, d3d12VertexBufferViews);
    }

    /// <summary>Draws a non-indexed primitive.</summary>
    /// <param name="verticesPerInstance">The number of vertices per instance.</param>
    /// <param name="instanceCount">The number of instances to draw.</param>
    /// <param name="vertexStart">The index at which drawn vertices should start.</param>
    /// <param name="instanceStart">The index at which drawn instances should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="verticesPerInstance" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="instanceCount" /> is <c>zero</c>.</exception>
    public void Draw(uint verticesPerInstance, uint instanceCount = 1, uint vertexStart = 0, uint instanceStart = 0)
    {
        ThrowIfZero(verticesPerInstance);
        ThrowIfZero(instanceCount);

        D3D12GraphicsCommandList->DrawInstanced(verticesPerInstance, instanceCount, vertexStart, instanceStart);
    }

    /// <summary>Draws an indexed primitive.</summary>
    /// <param name="indicesPerInstance">The number of indices per instance.</param>
    /// <param name="instanceCount">The number of instances to draw.</param>
    /// <param name="indexStart">The index at which drawn indices should start.</param>
    /// <param name="vertexStart">The index at which drawn vertices should start.</param>
    /// <param name="instanceStart">The index at which drawn instances should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indicesPerInstance" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="instanceCount" /> is <c>zero</c>.</exception>
    public void DrawIndexed(uint indicesPerInstance, uint instanceCount = 1, uint indexStart = 0, int vertexStart = 0, uint instanceStart = 0)
    {
        ThrowIfZero(indicesPerInstance);
        ThrowIfZero(instanceCount);

        D3D12GraphicsCommandList->DrawIndexedInstanced(indicesPerInstance, instanceCount, indexStart, vertexStart, instanceStart);
    }

    /// <summary>Ends a render pass.</summary>
    /// <exception cref="InvalidOperationException">A render pass is not active.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void EndRenderPass()
    {
        var renderPass = Interlocked.Exchange(ref _renderPass, null);

        if (renderPass is null)
        {
            ThrowForInvalidState(nameof(RenderPass));
        }

        var d3d12RtvResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(renderPass.Swapchain.CurrentRenderTarget.D3D12RtvResource, D3D12_RESOURCE_STATE_RENDER_TARGET, D3D12_RESOURCE_STATE_PRESENT);
        D3D12GraphicsCommandList->ResourceBarrier(1, &d3d12RtvResourceBarrier);
    }

    /// <summary>Sets the scissor for the context.</summary>
    /// <param name="scissor">The scissor to set.</param>
    public void SetScissor(BoundingRectangle scissor)
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

    /// <summary>Sets the scissors for the context.</summary>
    /// <param name="scissors">The scissors to set.</param>
    public void SetScissors(ReadOnlySpan<BoundingRectangle> scissors)
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

    /// <summary>Sets the viewport for the context.</summary>
    /// <param name="viewport">The viewport to set.</param>
    public void SetViewport(BoundingBox viewport)
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

    /// <summary>Sets the viewports for the context.</summary>
    /// <param name="viewports">The viewports to set.</param>
    public void SetViewports(ReadOnlySpan<BoundingBox> viewports)
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
}
