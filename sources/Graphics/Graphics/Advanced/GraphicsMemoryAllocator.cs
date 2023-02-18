// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TerraFX.Graphics.Advanced;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Advanced;

/// <summary>An allocator for graphics memory.</summary>
public abstract unsafe partial class GraphicsMemoryAllocator
{
    /// <summary>The minimum length, in bytes, of free memory regions to keep on either side of an allocated region.</summary>
    /// <remarks>This defaults to <c>0</c> so that no free regions are preserved around allocations.</remarks>
    public static uint MinimumAllocatedMemoryRegionMarginByteLength { get; } = GetAppContextData(
        $"{typeof(GraphicsMemoryAllocator).FullName}.{nameof(MinimumAllocatedMemoryRegionMarginByteLength)}",
        defaultValue: 0U
    );

    /// <summary>The minimum length, in bytes, a free memory region should be for it to be registered as available.</summary>
    /// <remarks>This defaults to <c>4096</c> which is the minimum allocation byte length allowed for small resource textures on DirectX.</remarks>
    public static uint MinimumFreeMemoryRegionByteLengthToRegister { get; } = GetAppContextData(
        $"{typeof(GraphicsMemoryAllocator).FullName}.{nameof(MinimumFreeMemoryRegionByteLengthToRegister)}",
        defaultValue: 4096U
    );

    /// <summary>The information for the graphics memory allocator.</summary>
    protected GraphicsMemoryAllocatorInfo MemoryAllocatorInfo;

    private readonly GraphicsDeviceObject _deviceObject;

    /// <summary>Creates a new instance of a memory allocator that uses a system provided default algorithm.</summary>
    /// <param name="deviceObject">The device object for which the allocator is managing memory.</param>
    /// <param name="memoryAllocatorCreateOptions">The options to use when creating the memory allocator.</param>
    /// <returns>A new memory allocator that uses a system provided default algorithm.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="deviceObject" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocatorCreateOptions.ByteLength" /> is <c>zero</c>.</exception>
    public static GraphicsMemoryAllocator CreateDefault(GraphicsDeviceObject deviceObject, in GraphicsMemoryAllocatorCreateOptions memoryAllocatorCreateOptions)
        => new DefaultMemoryAllocator(deviceObject, in memoryAllocatorCreateOptions);

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryAllocator" /> class.</summary>
    /// <param name="deviceObject">The device object for which the allocator is managing memory.</param>
    /// <exception cref="ArgumentNullException"><paramref name="deviceObject" /> is <c>null</c>.</exception>
    protected GraphicsMemoryAllocator(GraphicsDeviceObject deviceObject)
    {
        ThrowIfNull(deviceObject);
        _deviceObject = deviceObject;
    }

    /// <summary>Gets the length, in bytes, of the memory being managed.</summary>
    public nuint ByteLength => MemoryAllocatorInfo.ByteLength;

    /// <summary>Gets the device object for which the memory allocator is managing memory.</summary>
    public GraphicsDeviceObject DeviceObject => _deviceObject;

    /// <summary>Gets <c>true</c> if the allocator is dedicated to a single allocation; otherwise, <c>false</c>.</summary>
    public bool IsDedicated => MemoryAllocatorInfo.IsDedicated;

    /// <summary>Gets <c>true</c> if there are no allocated memory regions; otherwise, <c>false</c>.</summary>
    public bool IsEmpty => MemoryAllocatorInfo.IsEmpty;

    /// <summary>Gets the length, in bytes, of the largest free memory region.</summary>
    public nuint LargestFreeMemoryRegionByteLength => MemoryAllocatorInfo.LargestFreeMemoryRegionByteLength;

    /// <summary>Gets the total length, in bytes, of allocated memory regions.</summary>
    public nuint TotalAllocatedMemoryRegionByteLength => ByteLength - TotalFreeMemoryRegionByteLength;

    /// <summary>Gets the total length, in bytes, of free memory regions.</summary>
    public nuint TotalFreeMemoryRegionByteLength => MemoryAllocatorInfo.TotalFreeMemoryRegionByteLength;

    /// <summary>Allocates a memory region of the specified size and alignment.</summary>
    /// <param name="byteLength">The length, in bytes, of the memory region to allocate.</param>
    /// <param name="byteAlignment">The alignment, in bytes, of the memory region to allocate.</param>
    /// <returns>The allocated memory region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsMemoryRegion Allocate(nuint byteLength, nuint byteAlignment = 0)
    {
        if (!TryAllocate(byteLength, byteAlignment, out var memoryRegion))
        {
            ThrowOutOfMemoryException(byteLength);
        }
        return memoryRegion;
    }

    /// <summary>Clears the memory allocator of all allocated regions.</summary>
    public void Clear()
    {
        ClearUnsafe();
    }

    /// <summary>Frees a memory region of memory.</summary>
    /// <param name="memoryRegion">The memory region to be freed.</param>
    /// <exception cref="KeyNotFoundException"><paramref name="memoryRegion" /> was not found in the allocator.</exception>
    public void Free(in GraphicsMemoryRegion memoryRegion)
    {
        if (!TryFreeUnsafe(in memoryRegion))
        {
            ThrowKeyNotFoundException(memoryRegion, nameof(GraphicsMemoryAllocator));
        }

        var onFree = MemoryAllocatorInfo.OnFree;

        if (onFree.IsNotNull)
        {
            onFree.Invoke(in memoryRegion);
        }
    }

    /// <summary>Tries to allocate a memory region of the specified size and alignment.</summary>
    /// <param name="byteLength">The length, in bytes, of the memory region to allocate.</param>
    /// <param name="byteAlignment">The alignment, in bytes, of the memory region to allocate.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a memory region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="byteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    public bool TryAllocate(nuint byteLength, [Optional] nuint byteAlignment, out GraphicsMemoryRegion memoryRegion)
    {
        ThrowIfZero(byteLength);

        if (byteAlignment == 0)
        {
            byteAlignment = DefaultAlignment;
        }
        ThrowIfNotPow2(byteAlignment);

        return TryAllocateUnsafe(byteLength, byteAlignment, out memoryRegion);
    }

    /// <summary>Clears the memory allocator of all allocated regions.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void ClearUnsafe();

    /// <summary>Tries to allocate a memory region of the specified size and alignment.</summary>
    /// <param name="byteLength">The length, in bytes, of the memory region to allocate.</param>
    /// <param name="byteAlignment">The alignment, in bytes, of the memory region to allocate.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a memory region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract bool TryAllocateUnsafe(nuint byteLength, nuint byteAlignment, out GraphicsMemoryRegion memoryRegion);

    /// <summary>Tries to free a memory region.</summary>
    /// <param name="memoryRegion">The memory region to be freed.</param>
    /// <returns><c>true</c> if <paramref name="memoryRegion" /> was successfully freed; otherwise, <c>false</c>.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract bool TryFreeUnsafe(in GraphicsMemoryRegion memoryRegion);
}
