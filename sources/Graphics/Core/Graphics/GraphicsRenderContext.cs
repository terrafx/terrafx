// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using TerraFX.Numerics;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing render commands.</summary>
public abstract unsafe class GraphicsRenderContext : GraphicsContext
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsContext" /> class.</summary>
    /// <param name="device">The device for which the context is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsRenderContext(GraphicsDevice device)
        : base(device)
    {
    }

    /// <summary>Gets the swapchain which provides framebuffers for the context or <c>null</c> if one has not been set.</summary>
    public abstract GraphicsSwapchain? Swapchain { get; }

    /// <summary>Begins the drawing stage.</summary>
    /// <param name="framebufferIndex">The framebuffer index of <see cref="Swapchain" /> on which the context should draw.</param>
    /// <param name="backgroundColor">A color to which the background should be cleared.</param>
    /// <exception cref="ArgumentNullException"><see cref="Swapchain" /> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void BeginDrawing(uint framebufferIndex, ColorRgba backgroundColor);

    /// <summary>Draws a primitive to the render surface.</summary>
    /// <param name="primitive">The primitive to draw.</param>
    /// <exception cref="ArgumentNullException"><paramref name="primitive" /> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void Draw(GraphicsPrimitive primitive);

    /// <summary>Ends the drawing stage.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void EndDrawing();

    /// <summary>Sets the scissor for the context.</summary>
    /// <param name="scissor">The scissor to set.</param>
    public abstract void SetScissor(BoundingRectangle scissor);

    /// <summary>Sets the scissors for the context.</summary>
    /// <param name="scissors">The scissors to set.</param>
    public abstract void SetScissors(ReadOnlySpan<BoundingRectangle> scissors);

    /// <summary>Sets the swapchain which provides framebuffers for the context.</summary>
    /// <param name="swapchain">The swapchain which provides framebuffers for the context.</param>
    /// <exception cref="ArgumentNullException"><paramref name="swapchain" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="swapchain" /> doesn't belong to <see cref="GraphicsDeviceObject.Device" />.</exception>
    public abstract void SetSwapchain(GraphicsSwapchain swapchain);

    /// <summary>Sets the viewport for the context.</summary>
    /// <param name="viewport">The viewport to set.</param>
    public abstract void SetViewport(BoundingBox viewport);

    /// <summary>Sets the viewport for the context.</summary>
    /// <param name="viewports">The viewports to set.</param>
    public abstract void SetViewports(ReadOnlySpan<BoundingBox> viewports);
}
