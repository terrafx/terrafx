// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics pipeline.</summary>
public abstract class GraphicsPipelineObject : GraphicsRenderPassObject
{
    private readonly GraphicsPipeline _pipeline;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineObject" /> class.</summary>
    /// <param name="pipeline">The pipeline for which the object is being created.</param>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    protected GraphicsPipelineObject(GraphicsPipeline pipeline, string? name = null) : base(pipeline.RenderPass, name)
    {
        _pipeline = pipeline;
    }

    /// <summary>Gets the pipeline for which the object was created.</summary>
    public GraphicsPipeline Pipeline => _pipeline;
}
