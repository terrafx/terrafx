// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics pipeline.</summary>
public abstract class GraphicsPipelineObject : GraphicsRenderPassObject
{
    private readonly GraphicsPipeline _pipeline;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineObject" /> class.</summary>
    /// <param name="pipeline">The pipeline for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    protected GraphicsPipelineObject(GraphicsPipeline pipeline) : base(pipeline.RenderPass)
    {
        _pipeline = pipeline;
    }

    /// <summary>Gets the pipeline for which the object was created.</summary>
    public GraphicsPipeline Pipeline => _pipeline;
}
