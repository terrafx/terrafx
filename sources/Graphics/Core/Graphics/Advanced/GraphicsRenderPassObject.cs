// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics render pass.</summary>
public abstract class GraphicsRenderPassObject : GraphicsDeviceObject
{
    private readonly GraphicsRenderPass _renderPass;

    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderPassObject" /> class.</summary>
    /// <param name="renderPass">The render pass for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    protected GraphicsRenderPassObject(GraphicsRenderPass renderPass) : base(renderPass.Device)
    {
        _renderPass = renderPass;
    }

    /// <summary>Gets the render pass for which the object was created.</summary>
    public GraphicsRenderPass RenderPass => _renderPass;
}
