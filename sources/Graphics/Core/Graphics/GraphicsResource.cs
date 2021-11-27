// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics resource bound to a graphics device.</summary>
public abstract unsafe partial class GraphicsResource : GraphicsDeviceObject, IGraphicsMemoryRegionCollection<GraphicsResource>
{
    private readonly GraphicsMemoryAllocator _allocator;
    private readonly GraphicsMemoryRegion<GraphicsMemoryBlock> _blockRegion;
    private readonly GraphicsResourceCpuAccess _cpuAccess;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
    /// <param name="device">The device for which the resource was created.</param>
    /// <param name="blockRegion">The memory block region in which the resource exists.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    /// <exception cref="ArgumentNullException"><paramref name="blockRegion" />.<see cref="GraphicsMemoryRegion{TCollection}.Collection"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="blockRegion" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsResource(GraphicsDevice device, in GraphicsMemoryRegion<GraphicsMemoryBlock> blockRegion, GraphicsResourceCpuAccess cpuAccess)
        : base(device)
    {
        ThrowIfNull(blockRegion.Collection);

        if (blockRegion.Device != device)
        {
            ThrowForInvalidParent(blockRegion.Device);
        }

        _allocator = blockRegion.Collection.Collection.Allocator;
        _blockRegion = blockRegion;
        _cpuAccess = cpuAccess;
    }

    /// <summary>Gets the alignment of the resource, in bytes.</summary>
    public ulong Alignment => BlockRegion.Alignment;

    /// <inheritdoc />
    public abstract int AllocatedRegionCount { get; }

    /// <summary>Gets the allocator which created the resource.</summary>
    public GraphicsMemoryAllocator Allocator => _allocator;

    /// <summary>Gets the block which contains the resource.</summary>
    public GraphicsMemoryBlock Block => BlockRegion.Collection;

    /// <summary>Gets the memory block region in which the resource exists.</summary>
    public ref readonly GraphicsMemoryRegion<GraphicsMemoryBlock> BlockRegion => ref _blockRegion;

    /// <summary>Gets the number of regions in the resource.</summary>
    public abstract int Count { get; }

    /// <summary>Gets the CPU access capabilities of the resource.</summary>
    public GraphicsResourceCpuAccess CpuAccess => _cpuAccess;

    /// <inheritdoc />
    public abstract bool IsEmpty { get; }

    /// <inheritdoc />
    public abstract ulong LargestFreeRegionSize { get; }

    /// <inheritdoc />
    public abstract ulong MinimumFreeRegionSizeToRegister { get; }

    /// <inheritdoc />
    public abstract ulong MinimumAllocatedRegionMarginSize { get; }

    /// <summary>Gets the offset of the resource, in bytes.</summary>
    public ulong Offset => BlockRegion.Offset;

    /// <inheritdoc />
    public ulong Size => BlockRegion.Size;

    /// <inheritdoc />
    public abstract ulong TotalFreeRegionSize { get; }

    /// <inheritdoc />
    public abstract GraphicsMemoryRegion<GraphicsResource> Allocate(ulong size, ulong alignment = 1);

    /// <inheritdoc />
    public abstract void Clear();

    /// <inheritdoc />
    public abstract void Free(in GraphicsMemoryRegion<GraphicsResource> region);

    /// <summary>Gets an enumerator that can be used to iterate through the regions of the resource.</summary>
    /// <returns>An enumerator that can be used to iterate through the regions of the resource.</returns>
    public abstract IEnumerator<GraphicsMemoryRegion<GraphicsResource>> GetEnumerator();

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <returns>A pointer to the mapped resource.</returns>
    /// <remarks>This overload should be used when all memory should be mapped.</remarks>
    public abstract T* Map<T>()
        where T : unmanaged;

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <param name="region">The region of memory that will be mapped.</param>
    /// <returns>A pointer to the mapped resource.</returns>
    public T* Map<T>(in GraphicsMemoryRegion<GraphicsResource> region)
        where T : unmanaged => Map<T>((nuint)region.Offset, (nuint)region.Size);

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
    /// <param name="readRegion">The region of memory that will be read.</param>
    /// <returns>A pointer to the mapped resource.</returns>
    public T* MapForRead<T>(in GraphicsMemoryRegion<GraphicsResource> readRegion)
        where T : unmanaged => Map<T>((nuint)readRegion.Offset, (nuint)readRegion.Size);

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

    /// <inheritdoc />
    public abstract bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, out GraphicsMemoryRegion<GraphicsResource> region);

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    public abstract void Unmap();

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    public abstract void UnmapAndWrite();

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <param name="writtenRegion">The region of memory which was written.</param>
    public void UnmapAndWrite(in GraphicsMemoryRegion<GraphicsResource> writtenRegion)
        => UnmapAndWrite((nuint)writtenRegion.Offset, (nuint)writtenRegion.Size);

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
