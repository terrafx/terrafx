// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer which can hold data for a graphics device.</summary>
public abstract unsafe class GraphicsBuffer : GraphicsResource
{
    /// <summary>The information for the graphics buffer.</summary>
    protected GraphicsBufferInfo BufferInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsBuffer" /> class.</summary>
    /// <param name="device">The device for which the buffer was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsBuffer(GraphicsDevice device) : base(device)
    {
        ResourceInfo.Kind = GraphicsResourceKind.Buffer;
    }

    /// <summary>Gets the buffer kind.</summary>
    public new GraphicsBufferKind Kind => BufferInfo.Kind;

    /// <summary>Creates a view of the buffer.</summary>
    /// <param name="createOptions">The options to use when creating the buffer view.</param>
    /// <returns>The created buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.ElementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public GraphicsBufferView CreateBufferView(in GraphicsBufferViewCreateOptions createOptions)
    {
        if (!TryCreateBufferView(in createOptions, out var bufferView))
        {
            ThrowOutOfMemoryException(createOptions.ElementCount, createOptions.BytesPerElement);
        }
        return bufferView;
    }

    /// <summary>Creates a view of the buffer.</summary>
    /// <param name="elementCount">The number of elements in the buffer view.</param>
    /// <param name="bytesPerElement">The number of bytes per element in the buffer view.</param>
    /// <returns>The created buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="bytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBufferView CreateBufferView(uint elementCount, uint bytesPerElement)
    {
        if (!TryCreateBufferView(elementCount, bytesPerElement, out var bufferView))
        {
            ThrowOutOfMemoryException(elementCount, bytesPerElement);
        }
        return bufferView;
    }

    /// <summary>Creates a view of the buffer.</summary>
    /// <typeparam name="T">The type used to compute the number of bytes per element in the buffer view.</typeparam>
    /// <param name="elementCount">The number of elements, of type <typeparamref name="T" />, in the buffer view.</param>
    /// <returns>The created buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and the size of <typeparamref name="T" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBufferView CreateBufferView<T>(uint elementCount)
    {
        if (!TryCreateBufferView<T>(elementCount, out var bufferView))
        {
            ThrowOutOfMemoryException(elementCount);
        }
        return bufferView;
    }

    /// <summary>Disposes all buffer views allocated by the buffer.</summary>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public void DisposeAllViews()
    {
        ThrowIfDisposed();
        DisposeAllViewsUnsafe();
    }

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <param name="createOptions">The options to use when creating the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was succesfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the view was succesfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsBufferViewCreateOptions.ElementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <see cref="GraphicsBufferViewCreateOptions.BytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public bool TryCreateBufferView(in GraphicsBufferViewCreateOptions createOptions, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposed();

        ThrowIfZero(createOptions.BytesPerElement);
        ThrowIfZero(createOptions.ElementCount);

        if ((BufferInfo.Kind == GraphicsBufferKind.Index) && ((createOptions.BytesPerElement != 2) || (createOptions.BytesPerElement != 4)))
        {
            ThrowForInvalidKind(Kind);
        }

        return TryCreateBufferViewUnsafe(in createOptions, out bufferView);
    }

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <param name="elementCount">The number of elements in the buffer view.</param>
    /// <param name="bytesPerElement">The number of bytes per element in the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was succesfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the buffer view was succesfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytesPerElement" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="bytesPerElement" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public bool TryCreateBufferView(uint elementCount, uint bytesPerElement, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposed();

        ThrowIfZero(bytesPerElement);
        ThrowIfZero(elementCount);

        if ((BufferInfo.Kind == GraphicsBufferKind.Index) && ((bytesPerElement != 2) || (bytesPerElement != 4)))
        {
            ThrowForInvalidKind(Kind);
        }

        var createOptions = new GraphicsBufferViewCreateOptions {
            BytesPerElement = bytesPerElement,
            ElementCount = elementCount,
        };
        return TryCreateBufferViewUnsafe(in createOptions, out bufferView);
    }

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the buffer view.</typeparam>
    /// <param name="elementCount">The number of elements, of type <typeparamref name="T" />, in the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was succesfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the view was succesfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="elementCount" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and the size of <typeparamref name="T" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public bool TryCreateBufferView<T>(uint elementCount, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposed();

        ThrowIfZero(elementCount);

        var bytesPerElement = SizeOf<T>();

        if ((Kind == GraphicsBufferKind.Index) && (bytesPerElement != 2) && (bytesPerElement != 4))
        {
            ThrowForInvalidKind(Kind);
        }

        var createOptions = new GraphicsBufferViewCreateOptions {
            BytesPerElement = bytesPerElement,
            ElementCount = elementCount,
        };
        return TryCreateBufferViewUnsafe(in createOptions, out bufferView);
    }

    /// <summary>Disposes all buffer views allocated by the buffer.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void DisposeAllViewsUnsafe();

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <param name="createOptions">The options to use when creating the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was succesfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the view was succesfully created; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract bool TryCreateBufferViewUnsafe(in GraphicsBufferViewCreateOptions createOptions, [NotNullWhen(true)] out GraphicsBufferView? bufferView);
}
