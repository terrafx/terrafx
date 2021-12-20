// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using TerraFX.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer view.</summary>
public abstract unsafe class GraphicsBufferView : GraphicsResourceView
{
    private readonly GraphicsMemoryRegion _memoryRegion;
    private readonly nuint _offset;
    private readonly nuint _size;

    /// <summary>Initializes a new instance of the <see cref="GraphicsBufferView" /> class.</summary>
    /// <param name="buffer">The buffer for which the buffer view was created.</param>
    /// <param name="memoryRegion">The memory region in which the resource view exists.</param>
    /// <param name="stride">The stride, in bytes, of the elements in the buffer view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="buffer" /> is <c>null</c></exception>
    protected GraphicsBufferView(GraphicsBuffer buffer, in GraphicsMemoryRegion memoryRegion, uint stride)
        : base(buffer, stride)
    {
        _memoryRegion = memoryRegion;
        _offset = memoryRegion.Offset;
        _size = memoryRegion.Size;
    }

    /// <summary>Gets the memory region in which the buffer view exists.</summary>
    public ref readonly GraphicsMemoryRegion MemoryRegion => ref _memoryRegion;

    /// <summary>Gets the offset, in bytes, of the buffer.</summary>
    public nuint Offset => _offset;

    /// <inheritdoc cref="GraphicsResourceView.Resource" />
    public new GraphicsBuffer Resource => base.Resource.As<GraphicsBuffer>();

    /// <summary>Gets the size, in bytes, of the buffer.</summary>
    public nuint Size => _size;

    /// <summary>Maps the resource view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource view.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> Map<T>()
        where T : unmanaged
    {
        var mappedAddress = Resource.Map();
        mappedAddress += Offset;
        return new UnmanagedSpan<T>((T*)mappedAddress, Size / SizeOf<T>());
    }

    /// <summary>Maps the resource view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <returns>An unmanaged span that represents the mapped resource.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> Map<T>(nuint start)
        where T : unmanaged
    {
        var mappedSpan = Map<T>();
        return mappedSpan.Slice(start);
    }

    /// <summary>Maps the resource view into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <param name="length">The length of the span.</param>
    /// <returns>An unmanaged span that represents the mapped resource view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="Size" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> plus <paramref name="start" /> is greater than <see cref="Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> Map<T>(nuint start, nuint length)
        where T : unmanaged
    {
        var mappedSpan = Map<T>();
        return mappedSpan.Slice(start, length);
    }

    /// <summary>Maps the resource view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource view.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> MapForRead<T>()
        where T : unmanaged
    {
        var mappedAddress = Resource.MapForRead(Offset, Size);
        mappedAddress += Offset;
        return new UnmanagedSpan<T>((T*)mappedAddress, Size / SizeOf<T>());
    }

    /// <summary>Maps the resource view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <returns>An unmanaged span that represents the mapped resource view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> MapForRead<T>(nuint start)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        var offset = Offset + start;
        var size = Size - start;

        var mappedAddress = Resource.MapForRead(offset, size);
        mappedAddress += offset;
        return new UnmanagedSpan<T>((T*)mappedAddress, size / SizeOf<T>());
    }

    /// <summary>Maps the resource view into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <param name="start">The index at which which the span should start.</param>
    /// <param name="length">The length of the span.</param>
    /// <returns>An unmanaged span that represents the mapped resource view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="Size" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> plus <paramref name="start" /> is greater than <see cref="Size" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnmanagedSpan<T> MapForRead<T>(nuint start, nuint length)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        length *= SizeOf<T>();
        ThrowIfNotInInsertBounds(length, Size - start);

        var offset = Offset + start;
        var size = length;

        var mappedAddress = Resource.MapForRead(offset, size);
        mappedAddress += offset;
        return new UnmanagedSpan<T>((T*)mappedAddress, size / SizeOf<T>());
    }

    /// <summary>Unmaps the resource view from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException"><see cref="Resource" /> is not already mapped.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Unmap() => Resource.Unmap();

    /// <summary>Unmaps the resource view from CPU memory and writes the entire mapped region.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    /// <exception cref="InvalidOperationException"><see cref="Resource" /> is not already mapped.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnmapAndWrite() => Resource.UnmapAndWrite();

    /// <summary>Unmaps the resource view from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <param name="start">The index at which which the memory to write starts.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="Size" />.</exception>
    /// <exception cref="InvalidOperationException"><see cref="Resource" /> is not already mapped.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnmapAndWrite<T>(nuint start)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        var offset = Offset + start;
        var size = Size - start;

        Resource.UnmapAndWrite(offset, size);
    }

    /// <summary>Unmaps the resource view from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource view.</typeparam>
    /// <param name="start">The index at which which the memory to write starts.</param>
    /// <param name="length">The length of the memory region to write.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> is greater than <see cref="Size" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length" /> plus <paramref name="start" /> is greater than <see cref="Size" />.</exception>
    /// <exception cref="InvalidOperationException"><see cref="Resource" /> is not already mapped.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UnmapAndWrite<T>(nuint start, nuint length)
        where T : unmanaged
    {
        start *= SizeOf<T>();
        ThrowIfNotInBounds(start, Size);

        length *= SizeOf<T>();
        ThrowIfNotInInsertBounds(length, Size - start);

        var offset = Offset + start;
        var size = length;

        Resource.UnmapAndWrite(offset, size);
    }
}
