// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>An allocator for graphics memory.</summary>
public abstract partial class GraphicsMemoryAllocator : IReadOnlyCollection<GraphicsMemoryRegion>
{
    /// <summary>The minimum size of free memory regions to keep on either side of an allocated region, in bytes.</summary>
    /// <remarks>This defaults to <c>0</c> so that no free regions are preserved around allocations.</remarks>
    public static readonly ulong MinimumAllocatedMemoryRegionMarginSize = GetAppContextData(
        $"{typeof(GraphicsMemoryAllocator).FullName}.{nameof(MinimumAllocatedMemoryRegionMarginSize)}",
        defaultValue: 0UL
    );

    /// <summary>The minimum size a free memory region should be for it to be registered as available, in bytes.</summary>
    /// <remarks>This defaults to <c>4096</c> which is the minimum allocation size allowed for small resource textures on DirectX.</remarks>
    public static readonly ulong MinimumFreeMemoryRegionSizeToRegister = GetAppContextData(
        $"{typeof(GraphicsMemoryAllocator).FullName}.{nameof(MinimumFreeMemoryRegionSizeToRegister)}",
        defaultValue: 4096UL
    );

    private readonly GraphicsDeviceObject _deviceObject;
    private readonly ulong _size;

    /// <summary>Creates a new instance of a memory allocator that uses a system provided default algorithm.</summary>
    /// <param name="deviceObject">The device object for which the allocator is managing memory.</param>
    /// <param name="size">The size, in bytes, of the memory that is to be managed.</param>
    /// <returns>A new memory allocator that uses a system provided default algorithm.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="deviceObject" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    public static GraphicsMemoryAllocator CreateDefault(GraphicsDeviceObject deviceObject, ulong size) => new DefaultMemoryAllocator(deviceObject, size);

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryAllocator" /> class.</summary>
    /// <param name="deviceObject">The device object for which the allocator is managing memory.</param>
    /// <param name="size">The size, in bytes, of the memory that is to be managed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="deviceObject" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    protected GraphicsMemoryAllocator(GraphicsDeviceObject deviceObject, ulong size)
    {
        ThrowIfNull(deviceObject);
        ThrowIfZero(size);

        _deviceObject = deviceObject;
        _size = size;
    }

    /// <summary>Gets the number of allocated memory regions.</summary>
    public abstract int AllocatedMemoryRegionCount { get; }

    /// <summary>Gets the number of memory regions in the allocator.</summary>
    public abstract int Count { get; }

    /// <summary>Gets the device object for which the allocator is managing memory.</summary>
    public GraphicsDeviceObject DeviceObject => _deviceObject;

    /// <summary>Gets <c>true</c> if there are no allocated memory regions; otherwise, <c>false</c>.</summary>
    public abstract bool IsEmpty { get; }

    /// <summary>Gets the size of the largest free memory region, in bytes.</summary>
    public abstract ulong LargestFreeMemoryRegionSize { get; }

    /// <summary>Gets the size, in bytes, of the memory being managed.</summary>
    public ulong Size => _size;

    /// <summary>Gets the total size of free memory regions, in bytes.</summary>
    public abstract ulong TotalFreeMemoryRegionSize { get; }

    /// <summary>Allocates a memory region of the specified size and alignment.</summary>
    /// <param name="size">The size of the memory region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the memory region to allocate, in bytes.</param>
    /// <returns>The allocated memory region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsMemoryRegion Allocate(ulong size, ulong alignment = 0)
    {
        var result = TryAllocate(size, alignment, out var memoryRegion);

        if (!result)
        {
            ThrowOutOfMemoryException(size);
        }
        return memoryRegion;
    }

    /// <summary>Clears the allocator of allocated memory regions.</summary>
    public abstract void Clear();

    /// <summary>Frees a memory region of memory.</summary>
    /// <param name="memoryRegion">The memory region to be freed.</param>
    /// <exception cref="KeyNotFoundException"><paramref name="memoryRegion" /> was not found in the allocator.</exception>
    public abstract void Free(in GraphicsMemoryRegion memoryRegion);

    /// <summary>Gets an enumerator that can be used to iterate through the memory regions of the allocator.</summary>
    /// <returns>An enumerator that can be used to iterate through the memory regions of the allocator.</returns>
    public abstract IEnumerator<GraphicsMemoryRegion> GetEnumerator();

    /// <summary>Tries to allocate a memory region of the specified size and alignment.</summary>
    /// <param name="size">The size of the memory region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the memory region to allocate, in bytes.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a memory region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    public abstract bool TryAllocate(ulong size, [Optional] ulong alignment, out GraphicsMemoryRegion memoryRegion);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
