// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

#pragma warning disable CA1062 // Validate arguments of public methods

namespace TerraFX.Graphics;

/// <summary>Represents a set of vertices and indices used as part of a single draw operation.</summary>
public sealed class GraphicsPrimitive : GraphicsPipelineObject
{
    private readonly GraphicsBufferView? _indexBufferView;
    private readonly GraphicsPipelineDescriptorSet? _pipelineDescriptorSet;
    private readonly GraphicsBufferView _vertexBufferView;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPrimitive" /> class.</summary>
    /// <param name="pipeline">The pipeline for which the graphics primitive was created.</param>
    /// <param name="vertexBufferView">The vertex buffer view for the primitive.</param>
    /// <param name="indexBufferView">The index buffer view for the primitive or <c>null</c> if none exists.</param>
    /// <param name="resourceViews">The resource views for the primitive or <c>empty</c> if none exists.</param>
    public GraphicsPrimitive(GraphicsPipeline pipeline, GraphicsBufferView vertexBufferView, GraphicsBufferView? indexBufferView = null, ReadOnlySpan<GraphicsResourceView> resourceViews = default) : base(pipeline)
    {
        _indexBufferView = indexBufferView;
        _pipelineDescriptorSet = !resourceViews.IsEmpty ? pipeline.CreateDescriptorSet(resourceViews) : null;
        _vertexBufferView = vertexBufferView;
    }

    /// <summary>Gets the index buffer view for the primitive or <c>null</c> if none exists.</summary>
    public GraphicsBufferView? IndexBufferView => _indexBufferView;

    /// <summary>Gets the pipeline descriptor set for the primitive or <c>null</c> if none exists.</summary>
    public GraphicsPipelineDescriptorSet? PipelineDescriptorSet => _pipelineDescriptorSet;

    /// <summary>Gets the vertex buffer view for the primitive.</summary>
    public GraphicsBufferView VertexBufferView => _vertexBufferView;

    /// <summary>Draws the graphics primitive using a given graphics render context.</summary>
    /// <param name="renderContext">The render context that should be used to draw the graphics primitive.</param>
    /// <param name="instanceCount">The number of instances that should be drawn.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderContext" /> is <c>null</c>.</exception>
    public void Draw(GraphicsRenderContext renderContext, uint instanceCount = 1)
    {
        ThrowIfNull(renderContext);

        renderContext.BindPipeline(Pipeline);

        if (PipelineDescriptorSet is GraphicsPipelineDescriptorSet pipelineDescriptorSet)
        {
            renderContext.BindPipelineDescriptorSet(pipelineDescriptorSet);
        }

        var vertexBufferView = VertexBufferView;
        renderContext.BindVertexBufferView(vertexBufferView);

        if (IndexBufferView is GraphicsBufferView indexBufferView)
        {
            renderContext.BindIndexBufferView(indexBufferView);
            renderContext.DrawIndexed(indicesPerInstance: (uint)(indexBufferView.ByteLength / indexBufferView.BytesPerElement), instanceCount);
        }
        else
        {
            renderContext.Draw(verticesPerInstance: (uint)(vertexBufferView.ByteLength / vertexBufferView.BytesPerElement), instanceCount);
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        // In a real app this wouldn't necessarily be responsible for cleaning up the resource
        // views as they may be shared across multiple primitives or other device objects.

        _indexBufferView?.Dispose();

        if (_pipelineDescriptorSet is not null)
        {
            foreach (var resourceView in _pipelineDescriptorSet.ResourceViews)
            {
                resourceView.Dispose();
            }
            _pipelineDescriptorSet.Dispose();
        }

        _vertexBufferView?.Dispose();

        Pipeline?.Dispose();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }
}
