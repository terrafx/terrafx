// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics primitive which represents the most basic renderable object.</summary>
public abstract class GraphicsPrimitive : GraphicsDeviceObject
{
    private readonly GraphicsResourceView? _indexBufferView;
    private readonly GraphicsPipeline _pipeline;
    private readonly GraphicsResourceView _vertexBufferView;

    // TODO: Make this UnmanagedArray<GraphicsResourceView>
    private readonly GraphicsResourceView[] _inputResourceViews;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPrimitive" /> class.</summary>
    /// <param name="device">The device which manages the primitive.</param>
    /// <param name="pipeline">The pipeline used for rendering the primitive.</param>
    /// <param name="vertexBufferView">The vertex buffer view which holds the vertices for the primitive.</param>
    /// <param name="indexBufferView">The index buffer view which holds the indices for the primitive or <c>null</c> if none exists.</param>
    /// <param name="inputResourceViews">The resource views which hold the input data for the primitive or an empty span if none exist.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="vertexBufferView" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pipeline" /> is incompatible as it belongs to a different device.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferView" /> was not created for a <see cref="GraphicsBufferKind.Vertex" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferView" /> was not created for <paramref name="device" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBufferView" /> was not created for a <see cref="GraphicsBufferKind.Index" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBufferView" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsPrimitive(GraphicsDevice device, GraphicsPipeline pipeline, GraphicsBufferView vertexBufferView, GraphicsBufferView? indexBufferView, ReadOnlySpan<GraphicsResourceView> inputResourceViews)
        : base(device)
    {
        ThrowIfNull(pipeline);
        ThrowIfNull(vertexBufferView);

        if (pipeline.Device != device)
        {
            ThrowForInvalidParent(pipeline.Device);
        }

        if (vertexBufferView.Resource.Kind != GraphicsBufferKind.Vertex)
        {
            ThrowForInvalidKind(vertexBufferView.Resource.Kind);
        }

        if (vertexBufferView.Device != device)
        {
            ThrowForInvalidParent(vertexBufferView.Device);
        }

        if (indexBufferView is not null)
        {
            if (indexBufferView.Resource.Kind != GraphicsBufferKind.Index)
            {
                ThrowForInvalidKind(indexBufferView.Resource.Kind);
            }

            if (indexBufferView.Device != device)
            {
                ThrowForInvalidParent(indexBufferView.Device);
            }
        }

        _pipeline = pipeline;

        _vertexBufferView = vertexBufferView;
        _indexBufferView = indexBufferView;
        _inputResourceViews = inputResourceViews.ToArray();
    }

    /// <summary>Gets the index buffer view which holds the indices for the primitive or <c>null</c> if none exists.</summary>
    public GraphicsResourceView? IndexBufferView => _indexBufferView;

    /// <summary>Gets the resource views which hold the input data for the primitive or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public ReadOnlySpan<GraphicsResourceView> InputResourceViews => _inputResourceViews;

    /// <summary>Gets the pipeline used for rendering the primitive.</summary>
    public GraphicsPipeline Pipeline => _pipeline;

    /// <summary>Gets the vertex buffer view which holds the vertices for the primitive.</summary>
    public GraphicsResourceView VertexBufferView => _vertexBufferView;
}
