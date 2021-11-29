// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A swapchain which manages framebuffers for a graphics device.</summary>
public abstract class GraphicsSwapchain : GraphicsDeviceObject
{
    private readonly IGraphicsSurface _surface;
    private readonly GraphicsFence _fence;

    /// <summary>Initializes a new instance of the <see cref="GraphicsSwapchain" /> class.</summary>
    /// <param name="device">The device for which the swapchain is being created.</param>
    /// <param name="surface">The surface on which the swapchain can render.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="surface" /> is <c>null</c>.</exception>
    protected GraphicsSwapchain(GraphicsDevice device, IGraphicsSurface surface)
        : base(device)
    {
        ThrowIfNull(surface);
        _surface = surface;

        _fence = Device.CreateFence(isSignalled: false);
    }

    /// <summary>Gets the fence used to synchronize the swapchain.</summary>
    public GraphicsFence Fence => _fence;

    /// <summary>Gets the index of the current framebuffer.</summary>
    public abstract uint FramebufferIndex { get; }

    /// <summary>Gets the surface on which the swapchain can render.</summary>
    public IGraphicsSurface Surface => _surface;

    /// <summary>Presents the current framebuffer.</summary>
    /// <exception cref="ObjectDisposedException">The swapchain has been disposed.</exception>
    public abstract void Present();
}
