// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing copy commands.</summary>
public abstract class GraphicsCopyContext : GraphicsContext<GraphicsCopyContext>
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsCopyContext" /> class.</summary>
    /// <param name="copyCommandQueue">The copy command queue for which the copy context is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="copyCommandQueue" /> is <c>null</c>.</exception>
    protected GraphicsCopyContext(GraphicsCopyCommandQueue copyCommandQueue) : base(copyCommandQueue)
    {
        ContextInfo.Kind = GraphicsContextKind.Copy;
    }

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new GraphicsCopyCommandQueue CommandQueue => base.CommandQueue.As<GraphicsCopyCommandQueue>();

    /// <summary>Copies the contents from a buffer view to a buffer view.</summary>
    /// <param name="destination">The destination buffer view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The size of <paramref name="destination" /> is less than the size of <paramref name="source" />.</exception>
    /// <exception cref="ObjectDisposedException">The copy context has been disposed.</exception>
    public void Copy(GraphicsBufferView destination, GraphicsBufferView source)
    {
        ThrowIfDisposed();

        ThrowIfNull(destination);
        ThrowIfNull(source);

        ThrowIfNotInInsertBounds(source.ByteLength, destination.ByteLength);

        CopyUnsafe(destination, source);
    }

    /// <summary>Copies the contents from a buffer view to a texture view.</summary>
    /// <param name="destination">The destination texture view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The size <paramref name="destination" /> is less than the size of <paramref name="source" />.</exception>
    /// <exception cref="ObjectDisposedException">The copy context has been disposed.</exception>
    public void Copy(GraphicsTextureView destination, GraphicsBufferView source)
    {
        ThrowIfDisposed();

        ThrowIfNull(destination);
        ThrowIfNull(source);

        ThrowIfNotInInsertBounds(source.ByteLength, destination.ByteLength);

        CopyUnsafe(destination, source);
    }

    /// <summary>Copies the contents from a buffer view to a buffer view.</summary>
    /// <param name="destination">The destination buffer view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void CopyUnsafe(GraphicsBufferView destination, GraphicsBufferView source);

    /// <summary>Copies the contents from a buffer view to a texture view.</summary>
    /// <param name="destination">The destination texture view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void CopyUnsafe(GraphicsTextureView destination, GraphicsBufferView source);
}
