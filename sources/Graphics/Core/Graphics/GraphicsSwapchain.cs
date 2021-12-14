// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A swapchain which manages framebuffers for a graphics device.</summary>
public abstract class GraphicsSwapchain : GraphicsRenderPassObject
{
    private readonly GraphicsFence _fence;
    private readonly IGraphicsSurface _surface;

    /// <summary>Initializes a new instance of the <see cref="GraphicsSwapchain" /> class.</summary>
    /// <param name="renderPass">The render pass for which the swapchain is being created.</param>
    /// <param name="surface">The surface on which the swapchain can render.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="surface" /> is <c>null</c>.</exception>
    protected GraphicsSwapchain(GraphicsRenderPass renderPass, IGraphicsSurface surface)
        : base(renderPass)
    {
        ThrowIfNull(renderPass);
        ThrowIfNull(surface);

        _fence = Device.CreateFence(isSignalled: false);
        _surface = surface;
    }

    /// <summary>Gets the fence used to synchronize the swapchain.</summary>
    public GraphicsFence Fence => _fence;

    /// <summary>Gets the current render target of the swapchain.</summary>
    public abstract GraphicsRenderTarget RenderTarget { get; }

    /// <summary>Gets the number of render targets for the swapchain.</summary>
    public abstract uint RenderTargetCount { get; }

    /// <summary>Gets the backing-format for the render targets of the swapchain.</summary>
    public abstract GraphicsFormat RenderTargetFormat { get; }

    /// <summary>Gets the index of the current render target.</summary>
    public abstract uint RenderTargetIndex { get; }

    /// <summary>Gets the surface on which the swapchain can render.</summary>
    public IGraphicsSurface Surface => _surface;

    /// <summary>Gets the render target at the specified index.</summary>
    /// <param name="index">The index of the render target to retrieve.</param>
    /// <returns>The render target at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is greater than <see cref="RenderTargetCount" />.</exception>
    public abstract GraphicsRenderTarget GetRenderTarget(uint index);

    /// <summary>Presents the current framebuffer.</summary>
    /// <exception cref="ObjectDisposedException">The swapchain has been disposed.</exception>
    public abstract void Present();
}
