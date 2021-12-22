// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;

namespace TerraFX.Graphics;

/// <summary>A set of resource views for a graphics pipeline.</summary>
public abstract class GraphicsPipelineResourceViewSet : GraphicsPipelineObject
{
    private readonly GraphicsResourceView[] _resourceViews;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineResourceViewSet" /> class.</summary>
    /// <param name="pipeline">The pipeline for which the resource view set is being created.</param>
    /// <param name="resourceViews">The resource views in the resource view set.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    protected GraphicsPipelineResourceViewSet(GraphicsPipeline pipeline, ReadOnlySpan<GraphicsResourceView> resourceViews)
        : base(pipeline)
    {
        _resourceViews = resourceViews.ToArray();
    }

    /// <inheritdoc cref="this[nuint]" />
    public GraphicsResourceView this[int index] => _resourceViews[index];

    /// <inheritdoc cref="this[nuint]" />
    public GraphicsResourceView this[nint index] => _resourceViews[index];

    /// <inheritdoc cref="this[nuint]" />
    public GraphicsResourceView this[uint index] => _resourceViews[index];

    /// <summary>Gets the resource view at the given index.</summary>
    /// <param name="index">The index of the resource view to retrieve.</param>
    /// <returns>The resource view at <paramref name="index" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is out of range for <see cref="ResourceViews" />.</exception>
    public GraphicsResourceView this[nuint index] => _resourceViews[index];

    /// <summary>Gets the resource views in the resource view set.</summary>
    public ReadOnlySpan<GraphicsResourceView> ResourceViews => _resourceViews;
}
