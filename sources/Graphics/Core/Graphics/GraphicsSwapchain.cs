// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>A swapchain which manages framebuffers for a graphics device.</summary>
public abstract class GraphicsSwapchain : GraphicsRenderPassObject
{
    /// <summary>The information for the graphics swapchain.</summary>
    protected GraphicsSwapchainInfo SwapchainInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsSwapchain" /> class.</summary>
    /// <param name="renderPass">The render pass for which the swapchain is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    protected GraphicsSwapchain(GraphicsRenderPass renderPass) : base(renderPass)
    {
    }

    /// <summary>Gets the current render target of the swapchain.</summary>
    public GraphicsRenderTarget CurrentRenderTarget => SwapchainInfo.RenderTargets[SwapchainInfo.CurrentRenderTargetIndex];

    /// <summary>Gets the index of the current render target.</summary>
    public int CurrentRenderTargetIndex => SwapchainInfo.CurrentRenderTargetIndex;

    /// <summary>Gets the fence used to synchronize the swapchain.</summary>
    public GraphicsFence Fence => SwapchainInfo.Fence;

    /// <summary>Gets the render targets for the swapchain.</summary>
    public ReadOnlySpan<GraphicsRenderTarget> RenderTargets => SwapchainInfo.RenderTargets;

    /// <summary>Gets the format for the render targets of the swapchain.</summary>
    public GraphicsFormat RenderTargetFormat => SwapchainInfo.RenderTargetFormat;

    /// <summary>Gets the surface on which the swapchain can render.</summary>
    public IGraphicsSurface Surface => SwapchainInfo.Surface;

    /// <summary>Presents the current framebuffer.</summary>
    /// <exception cref="ObjectDisposedException">The swapchain has been disposed.</exception>
    public void Present()
    {
        ThrowIfDisposed();
        PresentUnsafe();
    }

    /// <summary>Presents the current framebuffer.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void PresentUnsafe();
}
