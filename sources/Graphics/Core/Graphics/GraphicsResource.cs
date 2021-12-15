// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>A graphics resource bound to a graphics device.</summary>
public abstract unsafe partial class GraphicsResource : GraphicsDeviceObject
{
    private readonly ulong _alignment;
    private readonly GraphicsResourceCpuAccess _cpuAccess;
    private readonly GraphicsMemoryRegion _memoryRegion;
    private readonly ulong _size;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
    /// <param name="device">The device for which the resource was created.</param>
    /// <param name="cpuAccess">The CPU access capabilities for the resource.</param>
    /// <param name="size">The size, in bytes, of the resource.</param>
    /// <param name="alignment">The alignment, in bytes, of the resource.</param>
    /// <param name="memoryRegion">The memory region in which the resource exists.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsResource(GraphicsDevice device, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment, in GraphicsMemoryRegion memoryRegion)
        : base(device)
    {
        _alignment = alignment;
        _cpuAccess = cpuAccess;
        _memoryRegion = memoryRegion;
        _size = size;
    }

    /// <summary>Gets the alignment, in bytes, of the resource.</summary>
    public ulong Alignment => _alignment;

    /// <summary>Gets the CPU access capabilitites of the resource.</summary>
    public GraphicsResourceCpuAccess CpuAccess => _cpuAccess;

    /// <summary>Gets the memory region in which the resource exists.</summary>
    public ref readonly GraphicsMemoryRegion MemoryRegion => ref _memoryRegion;

    /// <summary>Gets the size, in bytes, of the resource.</summary>
    public ulong Size => _size;

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
