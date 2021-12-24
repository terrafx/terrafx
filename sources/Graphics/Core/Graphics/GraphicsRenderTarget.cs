// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>A graphics render target which defines a swapchain backbuffer on which rendering can occur.</summary>
public abstract class GraphicsRenderTarget : GraphicsSwapchainObject
{
    /// <summary>The information for the graphics render target.</summary>
    protected GraphicsRenderTargetInfo RenderTargetInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderTarget" /> class.</summary>
    /// <param name="swapchain">The swapchain for which the render target is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="swapchain" /> is <c>null</c>.</exception>
    protected GraphicsRenderTarget(GraphicsSwapchain swapchain) : base(swapchain)
    {
    }

    /// <summary>Gets the index for the render target.</summary>
    public int Index => RenderTargetInfo.Index;
}
