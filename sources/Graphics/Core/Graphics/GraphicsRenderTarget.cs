// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>A graphics render target which defines a swapchain backbuffer on which rendering can occur.</summary>
public abstract class GraphicsRenderTarget : GraphicsSwapchainObject
{
    private readonly uint _index;

    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderTarget" /> class.</summary>
    /// <param name="swapchain">The swapchain for which the render target is being created.</param>
    /// <param name="index">The index of the render target.</param>
    /// <exception cref="ArgumentNullException"><paramref name="swapchain" /> is <c>null</c>.</exception>
    protected GraphicsRenderTarget(GraphicsSwapchain swapchain, uint index)
        : base(swapchain)
    {
        _index = index;
    }

    /// <summary>Gets the index for the render target.</summary>
    public uint Index => _index;
}
