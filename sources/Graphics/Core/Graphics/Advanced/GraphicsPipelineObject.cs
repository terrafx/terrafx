// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics pipeline.</summary>
public abstract class GraphicsPipelineObject
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsPipeline _pipeline;
    private readonly GraphicsRenderPass _renderPass;
    private readonly GraphicsService _service;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineObject" /> class.</summary>
    /// <param name="pipeline">The pipeline for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    protected GraphicsPipelineObject(GraphicsPipeline pipeline)
    {
        ThrowIfNull(pipeline);

        _adapter = pipeline.Adapter;
        _device = pipeline.Device;
        _pipeline = pipeline;
        _renderPass = pipeline.RenderPass;
        _service = pipeline.Service;
    }

    /// <summary>Gets the underlying adapter for <see cref="Device" />.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the underlying device for <see cref="RenderPass" />.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets the name for the pipeline object.</summary>
    public abstract string Name { get; set; }

    /// <summary>Gets the pipeline for which the object was created.</summary>
    public GraphicsPipeline Pipeline => _pipeline;

    /// <summary>Gets the underlying render pass for <see cref="Pipeline" />.</summary>
    public GraphicsRenderPass RenderPass => _renderPass;

    /// <summary>Gets the underlying service for <see cref="Adapter" />.</summary>
    public GraphicsService Service => _service;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);
}
