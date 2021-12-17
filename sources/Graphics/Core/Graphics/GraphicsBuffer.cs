// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer which can hold data for a graphics device.</summary>
public abstract unsafe class GraphicsBuffer : GraphicsResource
{
    private readonly GraphicsBufferKind _kind;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
    /// <param name="device">The device for which the buffer was created.</param>
    /// <param name="memoryRegion">The memory region in which the buffer resides.</param>
    /// <param name="cpuAccess">The CPU access capabilitites of the buffer.</param>
    /// <param name="kind">The buffer kind.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsBuffer(GraphicsDevice device, in GraphicsMemoryRegion memoryRegion, GraphicsResourceCpuAccess cpuAccess, GraphicsBufferKind kind)
        : base(device, in memoryRegion, cpuAccess)
    {
        _kind = kind;
    }

    /// <summary>Gets the buffer kind.</summary>
    public GraphicsBufferKind Kind => _kind;

    /// <summary>Creates a view of the buffer.</summary>
    /// <param name="count">The number of elements in the buffer view.</param>
    /// <param name="stride">The size, in bytes, of the elements in the buffer view.</param>
    /// <returns>The created view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="stride" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="stride" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBufferView CreateView(uint count, uint stride)
    {
        if (!TryCreateView(count, stride, out var bufferView))
        {
            ThrowOutOfMemoryException(count, stride);
        }
        return bufferView;
    }

    /// <summary>Creates a view of the buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the buffer view.</typeparam>
    /// <param name="count">The number of elements, of type <typeparamref name="T" />, in the buffer view.</param>
    /// <returns>The created view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and the size of <typeparamref name="T" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsBufferView CreateView<T>(uint count) => CreateView(count, SizeOf<T>());

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> Map<T>()
        where T : unmanaged
    {
        var mappedAddress = Map();
        return new UnmanagedSpan<T>((T*)mappedAddress, Size / SizeOf<T>());
    }

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> Map<T>(nuint start)
        where T : unmanaged
    {
        var mappedSpan = Map<T>();
        return mappedSpan.Slice(start);
    }

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <param name="length">The length of the span.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> plus <paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> Map<T>(nuint start, nuint length)
        where T : unmanaged
    {
        var mappedSpan = Map<T>();
        return mappedSpan.Slice(start, length);
    }

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> MapForRead<T>()
        where T : unmanaged
    {
        var mappedAddress = MapForRead();
        return new UnmanagedSpan<T>((T*)mappedAddress, Size / SizeOf<T>());
    }

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> MapForRead<T>(nuint start)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        var offset = start;
        var size = Size - start;

        var mappedAddress = MapForRead(offset, size);
        mappedAddress += offset;
        return new UnmanagedSpan<T>((T*)mappedAddress, size / SizeOf<T>());
    }

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <param name="length">The length of the span.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> plus <paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> MapForRead<T>(nuint start, nuint length)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        length *= SizeOf<T>();
        ThrowIfNotInInsertBounds(length, Size - start);

        var offset = start;
        var size = length;

        var mappedAddress = MapForRead(offset, size);
        mappedAddress += offset;
        return new UnmanagedSpan<T>((T*)mappedAddress, size / SizeOf<T>());
    }

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <param name="count">The number of elements in the buffer view.</param>
    /// <param name="stride">The size, in bytes, of the elements in the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was succesfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the view was succesfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="stride" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="stride" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public abstract bool TryCreateView(uint count, uint stride, [NotNullWhen(true)] out GraphicsBufferView? bufferView);

    /// <summary>Tries to creates a view of the buffer.</summary>
    /// <typeparam name="T">The type used to compute the size, in bytes, of the elements in the buffer view.</typeparam>
    /// <param name="count">The number of elements, of type <typeparamref name="T" />, in the buffer view.</param>
    /// <param name="bufferView">On return, contains the buffer view if it was succesfully created; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the view was succesfully created; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Kind" /> is <see cref="GraphicsBufferKind.Index" /> and the size of <typeparamref name="T" /> is not <c>2</c> or <c>4</c>.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public bool TryCreateView<T>(uint count, [NotNullWhen(true)] out GraphicsBufferView? bufferView) => TryCreateView(count, SizeOf<T>(), out bufferView);

    /// <summary>Unmaps the buffer from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    public abstract void Unmap();

    /// <summary>Unmaps the buffer from CPU memory and writes the entire mapped region.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    public abstract void UnmapAndWrite();

    /// <summary>Unmaps the buffer from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index at which which the memory to write starts.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnmapAndWrite<T>(nuint start)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        var offset = start;
        var size = Size - start;

        UnmapAndWrite(offset, size);
    }

    /// <summary>Unmaps the buffer from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index at which which the memory to write starts.</param>
    /// <param name="length">The length of the memory region to write.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> plus <paramref name="start" /> is greater than <see cref="GraphicsResource.Size" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnmapAndWrite<T>(nuint start, nuint length)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        length *= SizeOf<T>();
        ThrowIfNotInInsertBounds(length, Size - start);

        var offset = start;
        var size = length;

        UnmapAndWrite(offset, size);
    }

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <returns>A pointer to the mapped buffer.</returns>
    protected internal abstract byte* Map();

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <returns>A pointer to the mapped buffer.</returns>
    /// <remarks>This overload should be used when all memory will be read.</remarks>
    protected internal abstract byte* MapForRead();

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <param name="offset">The offset, in bytes, at which memory will start being read.</param>
    /// <param name="size">The size, in bytes, of memory will be read.</param>
    /// <returns>A pointer to the mapped buffer.</returns>
    protected internal abstract byte* MapForRead(nuint offset, nuint size);

    /// <summary>Unmaps the buffer into CPU memory and writes a region of bytes.</summary>
    /// <param name="offset">The offset, in bytes, at which memory will start being written.</param>
    /// <param name="size">The size, in bytes, of memory will be written.</param>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    protected internal abstract void UnmapAndWrite(nuint offset, nuint size);
}
