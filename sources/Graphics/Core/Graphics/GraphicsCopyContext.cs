// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing copy commands.</summary>
public abstract class GraphicsCopyContext : GraphicsContext
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsCopyContext" /> class.</summary>
    /// <param name="device">The device for which the copy context is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsCopyContext(GraphicsDevice device)
        : base(device, GraphicsContextKind.Copy)
    {
    }

    /// <summary>Copies the contents of a buffer view to a separate buffer view.</summary>
    /// <param name="destination">The destination buffer view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination" /> is shorter than <paramref name="source" />.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void Copy(GraphicsBufferView destination, GraphicsBufferView source);

    /// <summary>Copies the contents of a buffer view to a texture view.</summary>
    /// <param name="destination">The destination dimensional texture view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination" /> is shorter than <paramref name="source" />.</exception>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract void Copy(GraphicsTextureView destination, GraphicsBufferView source);
}
