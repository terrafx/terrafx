// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>A set of descriptors for a graphics pipeline.</summary>
public abstract class GraphicsPipelineDescriptorSet : GraphicsPipelineObject
{
    /// <summary>The information for the graphics pipeline descriptors.</summary>
    protected GraphicsPipelineDescriptorSetInfo PipelineDescriptorSetInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineDescriptorSet" /> class.</summary>
    /// <param name="pipeline">The pipeline for which the resource view set is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    protected GraphicsPipelineDescriptorSet(GraphicsPipeline pipeline) : base(pipeline)
    {
    }

    /// <summary>Gets the resource views for the pipeline descriptor.</summary>
    public ReadOnlySpan<GraphicsResourceView> ResourceViews => PipelineDescriptorSetInfo.ResourceViews;
}
