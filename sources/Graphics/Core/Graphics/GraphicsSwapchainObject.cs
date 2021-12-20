// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics swapchain.</summary>
public abstract class GraphicsSwapchainObject
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsRenderPass _renderPass;
    private readonly GraphicsSwapchain _swapchain;
    private readonly GraphicsService _service;

    /// <summary>Initializes a new instance of the <see cref="GraphicsSwapchainObject" /> class.</summary>
    /// <param name="swapchain">The swapchain for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="swapchain" /> is <c>null</c>.</exception>
    protected GraphicsSwapchainObject(GraphicsSwapchain swapchain)
    {
        ThrowIfNull(swapchain);

        _adapter = swapchain.Adapter;
        _device = swapchain.Device;
        _renderPass = swapchain.RenderPass;
        _swapchain = swapchain;
        _service = swapchain.Service;
    }

    /// <summary>Gets the underlying adapter for <see cref="Device" />.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the underlying device for <see cref="RenderPass" />.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets or sets the name for the device object.</summary>
    public abstract string Name { get; set; }

    /// <summary>Gets the underlying render pass for <see cref="Swapchain" />.</summary>
    public GraphicsRenderPass RenderPass => _renderPass;

    /// <summary>Gets the underlying service for <see cref="Adapter" />.</summary>
    public GraphicsService Service => _service;

    /// <summary>Gets the swapchain for which the object was created.</summary>
    public GraphicsSwapchain Swapchain => _swapchain;

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
