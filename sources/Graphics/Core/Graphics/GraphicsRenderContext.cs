// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Numerics;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing render commands.</summary>
public abstract unsafe class GraphicsRenderContext : GraphicsContext<GraphicsRenderContext>
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderContext" /> class.</summary>
    /// <param name="renderCommandQueue">The render command queue for which the render context is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderCommandQueue" /> is <c>null</c>.</exception>
    protected GraphicsRenderContext(GraphicsRenderCommandQueue renderCommandQueue) : base(renderCommandQueue)
    {
        ContextInfo.Kind = GraphicsContextKind.Render;
    }

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new GraphicsRenderCommandQueue CommandQueue => base.CommandQueue.As<GraphicsRenderCommandQueue>();

    /// <summary>Gets the maximum number of vertex buffer views that can be bound at one time.</summary>
    public abstract uint MaxBoundVertexBufferViewCount { get; }

    /// <summary>Gets the current render pass for the context or <c>null</c> if one has not been set.</summary>
    public abstract GraphicsRenderPass? RenderPass { get; }

    /// <summary>Begins a render pass.</summary>
    /// <param name="renderPass">The render pass to begin.</param>
    /// <param name="renderTargetClearColor">The color to which the render target should be cleared.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">A render pass is already active.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void BeginRenderPass(GraphicsRenderPass renderPass, ColorRgba renderTargetClearColor);

    /// <summary>Binds an index buffer view to the context.</summary>
    /// <param name="indexBufferView">The index buffer view to set.</param>
    /// <exception cref="ArgumentNullException"><paramref name="indexBufferView" /> is <c>null</c>.</exception>
    public abstract void BindIndexBufferView(GraphicsBufferView indexBufferView);

    /// <summary>Binds a pipeline to the context.</summary>
    /// <param name="pipeline">The pipeline to bind.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    public abstract void BindPipeline(GraphicsPipeline pipeline);

    /// <summary>Binds a pipeline descriptor set to the context.</summary>
    /// <param name="pipelineDescriptorSet">The pipeline descriptor set to bind.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipelineDescriptorSet" /> is <c>null</c>.</exception>
    public abstract void BindPipelineDescriptorSet(GraphicsPipelineDescriptorSet pipelineDescriptorSet);

    /// <summary>Binds a vertex buffer view to the context.</summary>
    /// <param name="vertexBufferView">The vertex buffer view to bind.</param>
    /// <param name="bindingSlot">The binding slot to which <paramref name="vertexBufferView" /> should be bound.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vertexBufferView" /> is <c>null</c>.</exception>
    public abstract void BindVertexBufferView(GraphicsBufferView vertexBufferView, uint bindingSlot = 0);

    /// <summary>Binds vertex buffer views to the context.</summary>
    /// <param name="vertexBufferViews">The vertex buffer views to bind.</param>
    /// <param name="firstBindingSlot">The first binding slot to which <paramref name="vertexBufferViews" /> should be bound.</param>
    /// <exception cref="ArgumentNullException">One of the items in <paramref name="vertexBufferViews" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferViews" /> is <c>empty</c> or greater than <see cref="MaxBoundVertexBufferViewCount" />.</exception>
    public abstract void BindVertexBufferViews(ReadOnlySpan<GraphicsBufferView> vertexBufferViews, uint firstBindingSlot = 0);

    /// <summary>Draws a non-indexed primitive.</summary>
    /// <param name="verticesPerInstance">The number of vertices per instance.</param>
    /// <param name="instanceCount">The number of instances to draw.</param>
    /// <param name="vertexStart">The index at which drawn vertices should start.</param>
    /// <param name="instanceStart">The index at which drawn instances should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="verticesPerInstance" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="instanceCount" /> is <c>zero</c>.</exception>
    public abstract void Draw(uint verticesPerInstance, uint instanceCount = 1, uint vertexStart = 0, uint instanceStart = 0);

    /// <summary>Draws an indexed primitive.</summary>
    /// <param name="indicesPerInstance">The number of indices per instance.</param>
    /// <param name="instanceCount">The number of instances to draw.</param>
    /// <param name="indexStart">The index at which drawn indices should start.</param>
    /// <param name="vertexStart">The index at which drawn vertices should start.</param>
    /// <param name="instanceStart">The index at which drawn instances should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indicesPerInstance" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="instanceCount" /> is <c>zero</c>.</exception>
    public abstract void DrawIndexed(uint indicesPerInstance, uint instanceCount = 1, uint indexStart = 0, int vertexStart = 0, uint instanceStart = 0);

    /// <summary>Ends a render pass.</summary>
    /// <exception cref="InvalidOperationException">A render pass is not active.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void EndRenderPass();

    /// <summary>Sets the scissor for the context.</summary>
    /// <param name="scissor">The scissor to set.</param>
    public abstract void SetScissor(BoundingRectangle scissor);

    /// <summary>Sets the scissors for the context.</summary>
    /// <param name="scissors">The scissors to set.</param>
    public abstract void SetScissors(ReadOnlySpan<BoundingRectangle> scissors);

    /// <summary>Sets the viewport for the context.</summary>
    /// <param name="viewport">The viewport to set.</param>
    public abstract void SetViewport(BoundingBox viewport);

    /// <summary>Sets the viewports for the context.</summary>
    /// <param name="viewports">The viewports to set.</param>
    public abstract void SetViewports(ReadOnlySpan<BoundingBox> viewports);
}
