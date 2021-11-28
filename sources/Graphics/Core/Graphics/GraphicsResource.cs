// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics resource bound to a graphics device.</summary>
public abstract unsafe partial class GraphicsResource : GraphicsDeviceObject
{
    private readonly GraphicsMemoryAllocator _allocator;
    private readonly GraphicsMemoryHeapRegion _heapRegion;
    private readonly GraphicsResourceCpuAccess _cpuAccess;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
    /// <param name="device">The device for which the resource was created.</param>
    /// <param name="heapRegion">The memory heap region in which the resource exists.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    /// <exception cref="ArgumentNullException"><paramref name="heapRegion" />.<see cref="GraphicsMemoryHeapRegion.Heap" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="heapRegion" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsResource(GraphicsDevice device, in GraphicsMemoryHeapRegion heapRegion, GraphicsResourceCpuAccess cpuAccess)
        : base(device)
    {
        ThrowIfNull(heapRegion.Heap);

        if (heapRegion.Device != device)
        {
            ThrowForInvalidParent(heapRegion.Device);
        }

        _allocator = heapRegion.Heap.Collection.Allocator;
        _heapRegion = heapRegion;
        _cpuAccess = cpuAccess;
    }

    /// <summary>Gets the alignment of the resource, in bytes.</summary>
    public ulong Alignment => HeapRegion.Alignment;

    /// <summary>Gets the allocator which created the resource.</summary>
    public GraphicsMemoryAllocator Allocator => _allocator;

    /// <summary>Gets the CPU access capabilities of the resource.</summary>
    public GraphicsResourceCpuAccess CpuAccess => _cpuAccess;

    /// <summary>Gets the heap which contains the resource.</summary>
    public GraphicsMemoryHeap Heap => HeapRegion.Heap;

    /// <summary>Gets the memory heap region in which the resource exists.</summary>
    public ref readonly GraphicsMemoryHeapRegion HeapRegion => ref _heapRegion;

    /// <summary>Gets the offset of the resource, in bytes.</summary>
    public ulong Offset => HeapRegion.Offset;

    /// <inheritdoc />
    public ulong Size => HeapRegion.Size;

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <returns>A pointer to the mapped resource.</returns>
    /// <remarks>This overload should be used when all memory should be mapped.</remarks>
    public abstract T* Map<T>()
        where T : unmanaged;

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <param name="range">The range of memory that will be mapped.</param>
    /// <returns>A pointer to the mapped resource.</returns>
    public T* Map<T>(Range range)
        where T : unmanaged
    {
        var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
        var (rangeOffset, rangeLength) = range.GetOffsetAndLength(size);
        return Map<T>((nuint)rangeOffset, (nuint)rangeLength);
    }

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <param name="rangeOffset">The offset into the resource at which memory will start being read.</param>
    /// <param name="rangeLength">The amount of memory which will be read.</param>
    /// <returns>A pointer to the mapped resource.</returns>
    public abstract T* Map<T>(nuint rangeOffset, nuint rangeLength)
        where T : unmanaged;

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <returns>A pointer to the mapped resource.</returns>
    /// <remarks>This overload should be used when all memory will be read.</remarks>
    public abstract T* MapForRead<T>()
        where T : unmanaged;

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <param name="readRange">The range of memory that will be read.</param>
    /// <returns>A pointer to the mapped resource.</returns>
    public T* MapForRead<T>(Range readRange)
        where T : unmanaged
    {
        var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
        var (readRangeOffset, readRangeLength) = readRange.GetOffsetAndLength(size);
        return MapForRead<T>((nuint)readRangeOffset, (nuint)readRangeLength);
    }

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <param name="readRangeOffset">The offset into the resource at which memory will start being read.</param>
    /// <param name="readRangeLength">The amount of memory which will be read.</param>
    /// <returns>A pointer to the mapped resource.</returns>
    public abstract T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
        where T : unmanaged;

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    public abstract void Unmap();

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    public abstract void UnmapAndWrite();

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <param name="writtenRange">The range of memory which was written.</param>
    public void UnmapAndWrite(Range writtenRange)
    {
        var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
        var (writtenRangeOffset, writtenRangeLength) = writtenRange.GetOffsetAndLength(size);
        UnmapAndWrite((nuint)writtenRangeOffset, (nuint)writtenRangeLength);
    }

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <param name="writtenRangeOffset">The offset into the resource at which memory started being written.</param>
    /// <param name="writtenRangeLength">The amount of memory which was written.</param>
    public abstract void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength);
}
