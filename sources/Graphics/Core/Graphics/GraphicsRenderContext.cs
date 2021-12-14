// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
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

    /// <summary>Gets the current render pass for the context or <c>null</c> if one has not been set.</summary>
    public abstract GraphicsRenderPass? RenderPass { get; }

    /// <summary>Begins a render pass.</summary>
    /// <param name="renderPass">The render pass to begin.</param>
    /// <param name="renderTargetClearColor">The color to which the render target should be cleared.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">A render pass is already active.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void BeginRenderPass(GraphicsRenderPass renderPass, ColorRgba renderTargetClearColor);

    /// <summary>Draws a primitive to the render surface.</summary>
    /// <param name="primitive">The primitive to draw.</param>
    /// <exception cref="ArgumentNullException"><paramref name="primitive" /> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">A render pass is not active.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void Draw(GraphicsPrimitive primitive);

    /// <summary>Ends a render pass.</summary>
    /// <exception cref="InvalidOperationException">A render pass is not active.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void EndRenderPass();

    /// <summary>Sets the scissor for the context.</summary>
    /// <param name="scissor">The scissor to set.</param>
    public abstract void SetScissor(BoundingRectangle scissor);

    /// <summary>Sets the scissors for the context.</summary>
    /// <param name="scissors">The scissors to set.</param>
    public abstract void SetScissors(ReadOnlySpan<BoundingRectangle> scissors);

    /// <summary>Sets the viewport for the context.</summary>
    /// <param name="viewport">The viewport to set.</param>
    public abstract void SetViewport(BoundingBox viewport);

    /// <summary>Sets the viewport for the context.</summary>
    /// <param name="viewports">The viewports to set.</param>
    public abstract void SetViewports(ReadOnlySpan<BoundingBox> viewports);
}
