// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics render pass.</summary>
public abstract class GraphicsRenderPassObject
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsRenderPass _renderPass;
    private readonly GraphicsService _service;

    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderPassObject" /> class.</summary>
    /// <param name="renderPass">The render pass for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    protected GraphicsRenderPassObject(GraphicsRenderPass renderPass)
    {
        ThrowIfNull(renderPass);

        _adapter = renderPass.Adapter;
        _device = renderPass.Device;
        _renderPass = renderPass;
        _service = renderPass.Service;
    }

    /// <summary>Gets the underlying adapter for <see cref="Device" />.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the underlying device for <see cref="RenderPass" />.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets the name for the render pass object.</summary>
    public abstract string Name { get; set; }

    /// <summary>Gets the render pass for which the object was created.</summary>
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
