// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics resource bound to a graphics device.</summary>
public abstract unsafe class GraphicsResource : GraphicsDeviceObject
{
    /// <summary>The information for the graphics resource.</summary>
    protected GraphicsResourceInfo ResourceInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
    /// <param name="device">The device for which the resource was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsResource(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets the length, in bytes, of the resource.</summary>
    public nuint ByteLength => ResourceInfo.MemoryRegion.ByteLength;

    /// <summary>Gets the CPU access capabilitites of the resource.</summary>
    public GraphicsCpuAccess CpuAccess => ResourceInfo.CpuAccess;

    /// <summary>Gets <c>true</c> if the resource is mapped; otherwise, <c>false</c>.</summary>
    public bool IsMapped => MappedAddress is not null;

    /// <summary>Gets the resource kind.</summary>
    public GraphicsResourceKind Kind => ResourceInfo.Kind;

    /// <summary>Gets the mapped address of the resouce or <c>null</c> if the resource is not currently mapped.</summary>
    public void* MappedAddress => ResourceInfo.MappedAddress;

    /// <summary>Gets the memory region in which the resource exists.</summary>
    public ref readonly GraphicsMemoryRegion MemoryRegion => ref ResourceInfo.MemoryRegion;

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource.</returns>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>()
        where T : unmanaged
    {
        ThrowIfDisposed();

        var mappedAddress = MapUnsafe();
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }

    /// <summary>Maps the resource into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource.</returns>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>()
        where T : unmanaged
    {
        ThrowIfDisposed();

        var mappedAddress = MapForReadUnsafe();
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }


    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
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

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
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

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
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

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
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

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public void Unmap()
    {
        ThrowIfDisposed();
        UnmapUnsafe();
    }

    /// <summary>Unmaps the resource from CPU memory and writes the entire mapped region.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public void UnmapAndWrite()
    {
        ThrowIfDisposed();
        UnmapAndWriteUnsafe();
    }

    /// <summary>Unmaps the buffer from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public void UnmapAndWrite<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        UnmapAndWriteUnsafe(byteStart, byteLength);
    }

    /// <summary>Unmaps the buffer from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
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

    /// <summary>Maps the entire resource into CPU memory.</summary>
    /// <returns>A pointer to the mapped resource.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract byte* MapUnsafe();

    /// <summary>Maps the entire resource into CPU memory for reading.</summary>
    /// <returns>A pointer to the mapped resource.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract byte* MapForReadUnsafe();

    /// <summary>Maps the entire buffer into CPU memory and marks a region for reading.</summary>
    /// <param name="byteStart">The index of the first byte at which which the memory to read will start.</param>
    /// <param name="byteLength">The number of bytes in the memory region that will be read.</param>
    /// <returns>A pointer to the mapped buffer.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract byte* MapForReadUnsafe(nuint byteStart, nuint byteLength);

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <exception cref="InvalidOperationException">The resource is not already mapped.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void UnmapUnsafe();

    /// <summary>Unmaps the resource from CPU memory and marks the entire region as written.</summary>
    /// <exception cref="InvalidOperationException">The resource is not already mapped.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void UnmapAndWriteUnsafe();

    /// <summary>Unmaps the buffer into CPU memory and marks a region as written.</summary>
    /// <param name="byteStart">The index of the first byte at which which the written memory starts.</param>
    /// <param name="byteLength">The number of bytes in the written memory region.</param>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength);
}
