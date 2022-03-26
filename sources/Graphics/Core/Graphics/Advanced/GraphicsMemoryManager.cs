// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics.Advanced;

/// <summary>Provides a way to manage memory for a graphics device.</summary>
public abstract class GraphicsMemoryManager : GraphicsDeviceObject
{
    /// <summary><c>true</c> if the memory manager should be externally synchronized; otherwise, <c>false</c>.</summary>
    /// <remarks>This defaults to <c>false</c> causing the manager to be internally synchronized using a multimedia safe locking mechanism.</remarks>
    public static readonly bool IsExternallySynchronized = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(IsExternallySynchronized)}",
        defaultValue: false
    );

    /// <summary>The maximum number of allocators allowed in a memory manager.</summary>
    /// <remarks>This defaults to <see cref="uint.MaxValue"/> so that there is no maximum number of allocators.</remarks>
    public static readonly uint MaximumMemoryAllocatorCount = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MaximumMemoryAllocatorCount)}",
        defaultValue: uint.MaxValue
    );

    /// <summary>The maximum length, in bytes, of a shared allocator allowed in a memory manager.</summary>
    /// <remarks>This defaults to <c>256MB</c> which allows ~64k small textures, 4k buffers, or 64 MSAA textures per shared allocator.</remarks>
    public static readonly uint MaximumSharedMemoryAllocatorByteLength = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MaximumSharedMemoryAllocatorByteLength)}",
        defaultValue: 256U * 1024U * 1024U
    );

    /// <summary>The minimum number of allocators allowed in the memory manager.</summary>
    /// <remarks>This defaults to <c>0</c> so that there is no minimum number of allocators.</remarks>
    public static readonly uint MinimumMemoryAllocatorCount = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MinimumMemoryAllocatorCount)}",
        defaultValue: 0U
    );

    /// <summary>The minimum length, in bytes, of an allocator allowed in a memory manager.</summary>
    /// <remarks>This defaults to <c>32MB</c> which is approx 1/8th the byte length of the default <see cref="MaximumSharedMemoryAllocatorByteLength" />.</remarks>
    public static readonly uint MinimumMemoryAllocatorByteLength = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MinimumMemoryAllocatorByteLength)}",
        defaultValue: 32U * 1024U * 1024U
    );

    /// <summary>The information for the graphics memory manager.</summary>
    protected GraphicsMemoryManagerInfo MemoryManagerInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryManager" /> class.</summary>
    /// <param name="device">The device for which the memory managed was created.</param>
    protected GraphicsMemoryManager(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets the length, in bytes, of the manager.</summary>
    public ulong ByteLength => MemoryManagerInfo.ByteLength;

    /// <summary>Gets the minimum length, in bytes, of the manager.</summary>
    public nuint MinimumByteLength => MemoryManagerInfo.MinimumByteLength;

    /// <summary>Gets the total number of operations performed by the manager.</summary>
    public ulong OperationCount => MemoryManagerInfo.OperationCount;

    /// <summary>Gets the total length, in bytes, of allocated memory regions.</summary>
    public ulong TotalAllocatedMemoryRegionByteLength => ByteLength - TotalFreeMemoryRegionByteLength;

    /// <summary>Gets the total length, in bytes, of free memory regions.</summary>
    public ulong TotalFreeMemoryRegionByteLength => MemoryManagerInfo.TotalFreeMemoryRegionByteLength;

    /// <summary>Allocate a memory region in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <returns>The allocated memory region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.AllocationFlags" /> is undefined.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsMemoryRegion Allocate(in GraphicsMemoryAllocationOptions allocationOptions)
    {
        if (!TryAllocate(in allocationOptions, out var memoryRegion))
        {
            ThrowOutOfMemoryException(allocationOptions.ByteLength);
        }
        return memoryRegion;
    }

    /// <summary>Tries to allocate a memory region in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.AllocationFlags" /> is undefined.</exception>
    /// <exception cref="ObjectDisposedException">The memory manager has been disposed.</exception>
    public bool TryAllocate(in GraphicsMemoryAllocationOptions allocationOptions, out GraphicsMemoryRegion memoryRegion)
    {
        ThrowIfNotPow2(allocationOptions.ByteAlignment);
        ThrowIfZero(allocationOptions.ByteLength);
        ThrowIfNotDefined(allocationOptions.AllocationFlags);

        return TryAllocateUnsafe(in allocationOptions, out memoryRegion);
    }

    /// <summary>Tries to allocate a set of memory regions in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <param name="memoryRegions">On return, will be filled with the allocated memory regions.</param>
    /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.AllocationFlags" /> is undefined.</exception>
    /// <exception cref="ObjectDisposedException">The memory manager has been disposed.</exception>
    public bool TryAllocate(in GraphicsMemoryAllocationOptions allocationOptions, Span<GraphicsMemoryRegion> memoryRegions)
    {
        ThrowIfNotPow2(allocationOptions.ByteAlignment);
        ThrowIfZero(allocationOptions.ByteLength);
        ThrowIfNotDefined(allocationOptions.AllocationFlags);

        return TryAllocateUnsafe(in allocationOptions, memoryRegions);
    }

    /// <summary>Tries to set the minimum length, in bytes, of the manager.</summary>
    /// <param name="minimumByteLength">The minimum length, in bytes, of the manager.</param>
    /// <returns><c>true</c> if the minimum length was succesfully set; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">The memory manager has been disposed.</exception>
    public bool TrySetMinimumByteLength(nuint minimumByteLength)
    {
        ThrowIfDisposed();
        return TrySetMinimumByteLengthUnsafe(minimumByteLength);
    }

    /// <summary>Tries to allocate a memory region in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract bool TryAllocateUnsafe(in GraphicsMemoryAllocationOptions allocationOptions, out GraphicsMemoryRegion memoryRegion);

    /// <summary>Tries to allocate a set of memory regions in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <param name="memoryRegions">On return, will be filled with the allocated memory regions.</param>
    /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract bool TryAllocateUnsafe(in GraphicsMemoryAllocationOptions allocationOptions, Span<GraphicsMemoryRegion> memoryRegions);

    /// <summary>Tries to set the minimum length, in bytes, of the manager.</summary>
    /// <param name="minimumByteLength">The minimum length, in bytes, of the manager.</param>
    /// <returns><c>true</c> if the minimum length was succesfully set; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract bool TrySetMinimumByteLengthUnsafe(nuint minimumByteLength);
}
