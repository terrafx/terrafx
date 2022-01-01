// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;

namespace TerraFX.Graphics.Advanced;

/// <summary>An object which is created for a graphics swapchain.</summary>
public abstract class GraphicsSwapchainObject : GraphicsRenderPassObject
{
    private readonly GraphicsSwapchain _swapchain;

    /// <summary>Initializes a new instance of the <see cref="GraphicsSwapchainObject" /> class.</summary>
    /// <param name="swapchain">The swapchain for which the object is being created.</param>
    /// <param name="name">The name of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="swapchain" /> is <c>null</c>.</exception>
    protected GraphicsSwapchainObject(GraphicsSwapchain swapchain, string? name = null) : base(swapchain.RenderPass, name)
    {
        _swapchain = swapchain;
    }

    /// <summary>Gets the swapchain for which the object was created.</summary>
    public GraphicsSwapchain Swapchain => _swapchain;
}
