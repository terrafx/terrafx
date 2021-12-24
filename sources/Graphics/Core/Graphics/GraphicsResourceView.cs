// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A view of memory in a graphics resource.</summary>
public abstract unsafe class GraphicsResourceView : GraphicsResourceObject
{
    /// <summary>The information for the graphics resource view.</summary>
    protected GraphicsResourceViewInfo ResourceViewInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResourceView" /> class.</summary>
    /// <param name="resource">The resource for which the resource view was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="resource" /> is <c>null</c>.</exception>
    protected GraphicsResourceView(GraphicsResource resource) : base(resource)
    {
    }

    /// <summary>Gets the length, in bytes, of the resource view.</summary>
    public nuint ByteLength => ResourceViewInfo.ByteLength;

    /// <summary>Gets the offset, in bytes, of the resource view.</summary>
    public nuint ByteOffset => ResourceViewInfo.ByteOffset;

    /// <summary>Gets the number of bytes per element in the resource view.</summary>
    public uint BytesPerElement => ResourceViewInfo.BytesPerElement;

    /// <summary>Gets the resource view kind.</summary>
    public GraphicsResourceKind Kind => ResourceViewInfo.Kind;

    /// <summary>Maps the resource view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer resource.</typeparam>
    /// <returns>An unmanaged span that represents the mapped buffer resource.</returns>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>()
        where T : unmanaged
    {
        ThrowIfDisposed();

        var mappedAddress = MapUnsafe();
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped resource.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        var mappedAddress = MapUnsafe();
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        var mappedAddress = MapUnsafe();
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the resource view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource view.</returns>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>()
        where T : unmanaged
    {
        ThrowIfDisposed();

        var mappedAddress = MapForReadUnsafe();
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        var mappedAddress = MapForReadUnsafe(byteStart, byteLength);
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        var mappedAddress = MapForReadUnsafe();
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }


    /// <summary>Unmaps the resource view from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public void Unmap()
    {
        ThrowIfDisposed();
        UnmapUnsafe();
    }

    /// <summary>Unmaps the resource view from CPU memory and writes the entire mapped region.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource view has been disposed.</exception>
    public void UnmapAndWrite()
    {
        ThrowIfDisposed();
        UnmapAndWriteUnsafe();
    }

    /// <summary>Unmaps the buffer view from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public void UnmapAndWrite<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        UnmapAndWriteUnsafe(byteStart, byteLength);
    }

    /// <summary>Unmaps the buffer view from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer view is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer view has been disposed.</exception>
    public void UnmapAndWrite<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        UnmapAndWriteUnsafe();
    }

    /// <summary>Maps the entire resource view into CPU memory.</summary>
    /// <returns>A pointer to the mapped resource view.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract byte* MapUnsafe();

    /// <summary>Maps the entire resource view into CPU memory for reading.</summary>
    /// <returns>A pointer to the mapped resource view.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract byte* MapForReadUnsafe();

    /// <summary>Maps the entire buffer view into CPU memory and marks a region for reading.</summary>
    /// <param name="byteStart">The index of the first byte at which which the memory to read will start.</param>
    /// <param name="byteLength">The number of bytes in the memory region that will be read.</param>
    /// <returns>A pointer to the mapped buffer view.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract byte* MapForReadUnsafe(nuint byteStart, nuint byteLength);

    /// <summary>Unmaps the resource view from CPU memory.</summary>
    /// <exception cref="InvalidOperationException">The resource view is not already mapped.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void UnmapUnsafe();

    /// <summary>Unmaps the resource view from CPU memory and marks the entire region as written.</summary>
    /// <exception cref="InvalidOperationException">The resource view is not already mapped.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void UnmapAndWriteUnsafe();

    /// <summary>Unmaps the buffer view from CPU memory and marks a region as written.</summary>
    /// <param name="byteStart">The index of the first byte at which which the written memory starts.</param>
    /// <param name="byteLength">The number of bytes in the written memory region.</param>
    /// <exception cref="InvalidOperationException">The buffer view is not already mapped.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength);
}
