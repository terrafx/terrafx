// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>Defines a single heap of memory which can contain allocated or free regions.</summary>
public abstract partial class GraphicsMemoryHeap : GraphicsDeviceObject, IGraphicsMemoryRegionCollection<GraphicsMemoryHeap>
{
    private readonly GraphicsMemoryHeapCollection _collection;

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryHeap" /> class.</summary>
    /// <param name="device">The device for which the memory heap is being created</param>
    /// <param name="collection">The memory heap collection which contains the heap.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="collection" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsMemoryHeap(GraphicsDevice device, GraphicsMemoryHeapCollection collection)
        : base(device)
    {
        ThrowIfNull(collection);

        if (collection.Device != device)
        {
            ThrowForInvalidParent(collection.Device);
        }

        _collection = collection;
    }

    /// <inheritdoc />
    public abstract int AllocatedRegionCount { get; }

    /// <summary>Gets the memory heap collection which contains the heap.</summary>
    public GraphicsMemoryHeapCollection Collection => _collection;

    /// <summary>Gets the number of regions in the heap.</summary>
    public abstract int Count { get; }

    /// <inheritdoc />
    public abstract bool IsEmpty { get; }

    /// <inheritdoc />
    public abstract ulong LargestFreeRegionSize { get; }

    /// <inheritdoc />
    public abstract ulong MinimumAllocatedRegionMarginSize { get; }

    /// <inheritdoc />
    public abstract ulong MinimumFreeRegionSizeToRegister { get; }

    /// <inheritdoc />
    public abstract ulong Size { get; }

    /// <inheritdoc />
    public abstract ulong TotalFreeRegionSize { get; }

    /// <inheritdoc />
    public abstract GraphicsMemoryRegion<GraphicsMemoryHeap> Allocate(ulong size, ulong alignment = 1);

    /// <inheritdoc />
    public abstract void Clear();

    /// <inheritdoc />
    public abstract void Free(in GraphicsMemoryRegion<GraphicsMemoryHeap> region);

    /// <summary>Gets an enumerator that can be used to iterate through the regions of the heap.</summary>
    /// <returns>An enumerator that can be used to iterate through the regions of the heap.</returns>
    public abstract IEnumerator<GraphicsMemoryRegion<GraphicsMemoryHeap>> GetEnumerator();

    /// <inheritdoc />
    public abstract bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, out GraphicsMemoryRegion<GraphicsMemoryHeap> region);
}
