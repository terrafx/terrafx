// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using static TerraFX.Utilities.AppContextUtilities;

namespace TerraFX.Advanced;

/// <summary>Provides a way to manage memory for a graphics device.</summary>
public abstract class GraphicsMemoryManager : GraphicsDeviceObject, IReadOnlyCollection<GraphicsMemoryAllocator>
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

    /// <summary>The maximum size, in bytes, of a shared allocator allowed in a memory manager.</summary>
    /// <remarks>This defaults to <c>256MB</c> which allows ~64k small textures, 4k buffers, or 64 MSAA textures per shared allocator.</remarks>
    public static readonly uint MaximumSharedMemoryAllocatorSize = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MaximumSharedMemoryAllocatorSize)}",
        defaultValue: 256U * 1024U * 1024U
    );

    /// <summary>The minimum number of allocators allowed in the memory manager.</summary>
    /// <remarks>This defaults to <c>0</c> so that there is no minimum number of allocators.</remarks>
    public static readonly uint MinimumMemoryAllocatorCount = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MinimumMemoryAllocatorCount)}",
        defaultValue: 0U
    );

    /// <summary>The minimum size, in bytes, of an allocator allowed in a memory manager.</summary>
    /// <remarks>This defaults to <c>32MB</c> which is approx 1/8th the size of the default <see cref="MaximumSharedMemoryAllocatorSize" />.</remarks>
    public static readonly uint MinimumMemoryAllocatorSize = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MinimumMemoryAllocatorSize)}",
        defaultValue: 32U * 1024U * 1024U
    );

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryManager" /> class.</summary>
    /// <param name="device">The device for which the memory managed was created.</param>
    protected GraphicsMemoryManager(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets the number of memory allocators in the memory manager.</summary>
    public abstract int Count { get; }

    /// <summary>Gets <c>true</c> if the manager is empty; otherwise, <c>false</c>.</summary>
    public abstract bool IsEmpty { get; }

    /// <summary>Gets the minimum size, in bytes, of the manager.</summary>
    public abstract nuint MinimumSize { get; }

    /// <summary>Gets the total number of operations performed by the manager.</summary>
    public abstract ulong OperationCount { get; }

    /// <summary>Gets the size, in bytes, of the manager.</summary>
    public abstract ulong Size { get; }

    /// <summary>Gets the total size, in bytes, of allocated memory regions.</summary>
    public ulong TotalAllocatedMemoryRegionSize => Size - TotalFreeMemoryRegionSize;

    /// <summary>Gets the total size, in bytes, of free memory regions.</summary>
    public abstract ulong TotalFreeMemoryRegionSize { get; }

    /// <summary>Allocate a memory region in the manager.</summary>
    /// <param name="size">The size, in bytes, of the memory region to allocate.</param>
    /// <param name="alignment">The alignment, in bytes, of the memory region to allocate.</param>
    /// <param name="memoryAllocationFlags">The flags that modify how the memory region is allocated.</param>
    /// <returns>The allocated memory region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryAllocationFlags" /> has an invalid combination.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public abstract GraphicsMemoryRegion Allocate(nuint size, nuint alignment = 0, GraphicsMemoryAllocationFlags memoryAllocationFlags = GraphicsMemoryAllocationFlags.None);

    /// <summary>Gets an enumerator that can iterate through the memory allocators in the manager.</summary>
    /// <returns>An enumerator that can iterate through the memory allocators in the manager.</returns>
    public abstract IEnumerator<GraphicsMemoryAllocator> GetEnumerator();

    /// <summary>Tries to allocate a memory region in the manager.</summary>
    /// <param name="size">The size, in bytes, of the memory region to allocate.</param>
    /// <param name="alignment">The alignment, in bytes, of the memory region to allocate.</param>
    /// <param name="memoryAllocationFlags">The flags that modify how the memory region is allocated.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryAllocationFlags" /> has an invalid combination.</exception>
    public abstract bool TryAllocate(nuint size, [Optional] nuint alignment, [Optional] GraphicsMemoryAllocationFlags memoryAllocationFlags, out GraphicsMemoryRegion memoryRegion);

    /// <summary>Tries to allocate a set of memory regions in the manager.</summary>
    /// <param name="size">The size, in bytes, of the memory regions to allocate.</param>
    /// <param name="alignment">The alignment, in bytes, of the memory regions to allocate.</param>
    /// <param name="memoryAllocationFlags">The flags that modify how the memory regions are allocated.</param>
    /// <param name="memoryRegions">On return, will be filled with the allocated memory regions.</param>
    /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryAllocationFlags" /> has an invalid combination.</exception>
    public abstract bool TryAllocate(nuint size, [Optional] nuint alignment, [Optional] GraphicsMemoryAllocationFlags memoryAllocationFlags, Span<GraphicsMemoryRegion> memoryRegions);

    /// <summary>Tries to set the minimum size, in bytes, of the manager.</summary>
    /// <param name="minimumSize">The minimum size, in bytes, of the manager.</param>
    /// <returns><c>true</c> if the minimum size was succesfully set; otherwise, <c>false</c>.</returns>
    public abstract bool TrySetMinimumSize(nuint minimumSize);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
