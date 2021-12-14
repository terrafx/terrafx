// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TerraFX.Collections;
using TerraFX.Threading;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a collection of memory heaps.</summary>
public abstract class GraphicsMemoryHeapCollection : GraphicsDeviceObject, IReadOnlyCollection<GraphicsMemoryHeap>
{
    private readonly GraphicsMemoryAllocator _allocator;
    private readonly ValueReaderWriterLock _rwLock;

    private ValueList<GraphicsMemoryHeap> _heaps;
    private GraphicsMemoryHeap? _emptyHeap;

    private ulong _minimumSize;
    private ulong _size;

    private VolatileState _state;

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryHeapCollection" /> class.</summary>
    /// <param name="device">The device for which the memory heap collection is being created</param>
    /// <param name="allocator">The allocator that manages the collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="allocator" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="allocator" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsMemoryHeapCollection(GraphicsDevice device, GraphicsMemoryAllocator allocator)
        : base(device)
    {
        ThrowIfNull(allocator);

        if (allocator.Device != device)
        {
            ThrowForInvalidParent(allocator.Device);
        }

        _allocator = allocator;

        _heaps = new ValueList<GraphicsMemoryHeap>();
        _rwLock = new ValueReaderWriterLock();

        ref readonly var allocatorSettings = ref _allocator.Settings;

        var minimumHeapCount = allocatorSettings.MinimumHeapCountPerCollection;
        var maximumSharedHeapSize = allocatorSettings.MaximumSharedHeapSize.GetValueOrDefault();

        for (var i = 0; i < minimumHeapCount; ++i)
        {
            var heapSize = GetAdjustedHeapSize(maximumSharedHeapSize);
            _ = AddHeap(heapSize);
        }

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Gets the allocator that manages the collection.</summary>
    public GraphicsMemoryAllocator Allocator => _allocator;

    /// <summary>Gets the number of heaps in the collection.</summary>
    public int Count => _heaps.Count;

    /// <summary>Gets <c>true</c> if the heap collection is empty; otherwise, <c>false</c>.</summary>
    public bool IsEmpty
    {
        get
        {
            using var rwlock = new DisposableReaderLock(_rwLock, _allocator.IsExternallySynchronized);
            return _heaps.Count == 0;
        }
    }

    /// <summary>Gets the maximum number of heaps allowed in the collection.</summary>
    public int MaximumHeapCount => _allocator.Settings.MaximumHeapCountPerCollection;

    /// <summary>Gets the maximum size of any new shared memory heaps created for the collection, in bytes.</summary>
    public ulong MaximumSharedHeapSize => _allocator.Settings.MaximumSharedHeapSize.GetValueOrDefault();

    /// <summary>Gets the minimum number of heaps allowed in the collection.</summary>
    public int MinimumHeapCount => _allocator.Settings.MinimumHeapCountPerCollection;

    /// <summary>Gets the minimum size of any new memory heaps created for the collection, in bytes.</summary>
    public ulong MinimumHeapSize => _allocator.Settings.MinimumHeapSize;

    /// <summary>Gets the minimum size of the collection, in bytes.</summary>
    public ulong MinimumSize => _minimumSize;

    /// <summary>Gets the size of the collection, in bytes.</summary>
    public ulong Size => _size;

    /// <summary>Allocates a region of memory for the collection.</summary>
    /// <param name="size">The size of the region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the region to allocate, in bytes.</param>
    /// <param name="flags">The flags that modify how the region is allocated.</param>
    /// <returns>The allocated region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="flags" /> has an invalid combination.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough region of free memory to complete the allocation.</exception>
    public GraphicsMemoryHeapRegion Allocate(ulong size, ulong alignment = 1, GraphicsMemoryHeapRegionAllocationFlags flags = GraphicsMemoryHeapRegionAllocationFlags.None)
    {
        var succeeded = TryAllocate(size, alignment, flags, out var heapRegion);

        if (!succeeded)
        {
            ThrowOutOfMemoryException(size);
        }
        return heapRegion;
    }

    /// <summary>Frees a region of memory from the collection.</summary>
    /// <param name="heapRegion">The region to be freed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="heapRegion" />.<see cref="GraphicsMemoryHeapRegion.Heap" /> is <c>null</c>.</exception>
    /// <exception cref="KeyNotFoundException"><paramref name="heapRegion" /> was not found in the collection.</exception>
    public void Free(in GraphicsMemoryHeapRegion heapRegion)
    {
        using var writeLock = new DisposableWriterLock(_rwLock, _allocator.IsExternallySynchronized);
        FreeInternal(in heapRegion);
    }

    /// <summary>Gets an enumerator that can be used to iterate through the heaps of the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the heaps of the collection.</returns>
    public IEnumerator<GraphicsMemoryHeap> GetEnumerator() => _heaps.GetEnumerator();

    /// <summary>Tries to allocation a region of memory in the collection.</summary>
    /// <param name="size">The size of the region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the region to allocate, in bytes.</param>
    /// <param name="flags">The flags that modify how the region is allocated.</param>
    /// <param name="heapRegion">On return, contains the allocated region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="flags" /> has an invalid combination.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    public bool TryAllocate(ulong size, [Optional] ulong alignment, [Optional] GraphicsMemoryHeapRegionAllocationFlags flags, out GraphicsMemoryHeapRegion heapRegion)
    {
        using var writeLock = new DisposableWriterLock(_rwLock, _allocator.IsExternallySynchronized);
        return TryAllocateInternal(size, alignment, flags, out heapRegion);
    }

    /// <summary>Tries to allocate a set of memory regions in the collection.</summary>
    /// <param name="size">The size of the regions to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the regions to allocate, in bytes.</param>
    /// <param name="flags">The flags that modify how the regions are allocated.</param>
    /// <param name="heapRegions">On return, will be filled with the allocated regions.</param>
    /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="flags" /> has an invalid combination.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    public bool TryAllocate(ulong size, [Optional] ulong alignment, [Optional] GraphicsMemoryHeapRegionAllocationFlags flags, Span<GraphicsMemoryHeapRegion> heapRegions)
    {
        using var writeLock = new DisposableWriterLock(_rwLock, _allocator.IsExternallySynchronized);
        return TryAllocateInternal(size, alignment, flags, heapRegions);
    }

    /// <summary>Tries to set the minimum size of the collection, in bytes.</summary>
    /// <param name="minimumSize">The minimum size of the collection, in bytes.</param>
    /// <returns><c>true</c> if the minimum size was succesfully set; otherwise, <c>false</c>.</returns>
    public bool TrySetMinimumSize(ulong minimumSize)
    {
        using var writeLock = new DisposableWriterLock(_rwLock, _allocator.IsExternallySynchronized);
        return TrySetMinimumSizeInternal(minimumSize);
    }

    /// <summary>Adds a new heap to the collection.</summary>
    /// <param name="size">The size of the heap, in bytes.</param>
    /// <returns>The created graphics memory heap.</returns>
    protected abstract GraphicsMemoryHeap CreateHeap(ulong size);

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            foreach (var heap in _heaps)
            {
                heap?.Dispose();
            }

            _rwLock.Dispose();
        }

        _state.EndDispose();
    }

    private GraphicsMemoryHeap AddHeap(ulong size)
    {
        var heap = CreateHeap(size);

        _heaps.Add(heap);
        _size += size;

        return heap;
    }

    private void FreeInternal(in GraphicsMemoryHeapRegion heapRegion)
    {
        // This method should only be called under a write-lock

        var heap = heapRegion.Heap;
        ThrowIfNull(heap);

        var heaps = _heaps;
        var heapIndex = heaps.IndexOf(heap);

        if (heapIndex == -1)
        {
            ThrowKeyNotFoundException(heapRegion, nameof(heaps));
        }

        heap.Free(in heapRegion);

        if (heap.IsEmpty)
        {
            var emptyHeap = _emptyHeap;
            var heapsCount = heaps.Count;

            if (emptyHeap is not null)
            {
                if (heapsCount > MinimumHeapCount)
                {
                    var size = _size;
                    var minimumSize = _minimumSize;

                    // We have two empty heaps, we want to prefer removing the larger of the two

                    if (heap.Size > emptyHeap.Size)
                    {
                        if ((size - heap.Size) >= minimumSize)
                        {
                            RemoveHeapAt(heapIndex);
                        }
                        else if ((size - emptyHeap.Size) >= minimumSize)
                        {
                            RemoveHeap(emptyHeap);
                        }
                    }
                    else if ((size - emptyHeap.Size) >= minimumSize)
                    {
                        RemoveHeapAt(heapIndex);
                    }
                    else if ((size - heap.Size) >= minimumSize)
                    {
                        RemoveHeap(emptyHeap);
                    }
                    else
                    {
                        // Removing either would put us below the minimum size, so we can't remove
                    }
                }
                else if (heap.Size > emptyHeap.Size)
                {
                    // We can't remove the heap, so set empty heap to the larger
                    _emptyHeap = heap;
                }
            }
            else
            {
                // We have no existing empty heaps, so we want to set the index to this heap unless
                // we are currently exceeding our memory budget, in which case we want to free the heap
                // instead. However, we still need to maintain the minimum heap count and minimum size
                // placed on the collection, so we will only respect the budget if those can be maintained.

                _allocator.GetBudget(this, out var budget);

                if ((budget.EstimatedUsage >= budget.EstimatedBudget) && (heapsCount > MinimumHeapCount) && ((_size - heap.Size) >= _minimumSize))
                {
                    RemoveHeapAt(heapIndex);
                }
                else
                {
                    _emptyHeap = heap;
                }
            }
        }

        IncrementallySortHeaps();
    }

    private ulong GetAdjustedHeapSize(ulong size)
    {
        var maximumSharedHeapSize = MaximumSharedHeapSize;
        var heapSize = size;

        if (heapSize < maximumSharedHeapSize)
        {
            var minimumHeapSize = MinimumHeapSize;
            heapSize = maximumSharedHeapSize;

            if (minimumHeapSize != maximumSharedHeapSize)
            {
                // Allocate 1/8, 1/4, 1/2 as first heaps, ensuring we don't go smaller than the minimum
                var largestHeapSize = GetLargestSharedHeapSize();

                for (var i = 0; i < 3; ++i)
                {
                    var smallerHeapSize = heapSize / 2;

                    if ((smallerHeapSize > largestHeapSize) && (smallerHeapSize >= (size * 2)))
                    {
                        heapSize = Math.Max(smallerHeapSize, minimumHeapSize);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            heapSize = size;
        }

        return heapSize;
    }

    private ulong GetLargestSharedHeapSize()
    {
        var result = 0UL;

        var heaps = _heaps.AsSpanUnsafe(0, _heaps.Count);
        var maximumSharedHeapSize = MaximumSharedHeapSize;

        for (var i = heaps.Length; i-- != 0;)
        {
            var heapSize = heaps[i].Size;

            if (heapSize < maximumSharedHeapSize)
            {
                result = Math.Max(result, heapSize);
            }
            else if (heapSize == maximumSharedHeapSize)
            {
                result = maximumSharedHeapSize;
                break;
            }
        }

        return result;
    }

    private void IncrementallySortHeaps()
    {
        // Bubble sort only until first swap. This is called after
        // freeing a region and will result in eventual consistency

        var heaps = _heaps.AsSpanUnsafe(0, _heaps.Count);

        if (heaps.Length >= 2)
        {
            var previousHeap = heaps[0];

            for (var i = 1; i < heaps.Length; ++i)
            {
                var heap = heaps[i];

                if (previousHeap.TotalFreeRegionSize <= heap.TotalFreeRegionSize)
                {
                    previousHeap = heap;
                }
                else
                {
                    heaps[i - 1] = heap;
                    heaps[i] = previousHeap;

                    return;
                }
            }
        }
    }

    private void RemoveHeap(GraphicsMemoryHeap heap)
    {
        var heapIndex = _heaps.IndexOf(heap);
        RemoveHeapAt(heapIndex);
    }

    private void RemoveHeapAt(int index)
    {
        var heap = _heaps[index];
        _heaps.RemoveAt(index);
        _size -= heap.Size;
    }

    private bool TryAllocateInternal(ulong size, ulong alignment, GraphicsMemoryHeapRegionAllocationFlags flags, out GraphicsMemoryHeapRegion heapRegion)
    {
        // This method should only be called under a write-lock

        var useDedicatedHeap = flags.HasFlag(GraphicsMemoryHeapRegionAllocationFlags.DedicatedCollection);
        var useExistingHeap = flags.HasFlag(GraphicsMemoryHeapRegionAllocationFlags.ExistingCollection);

        if (useDedicatedHeap && useExistingHeap)
        {
            ThrowForInvalidFlagsCombination(flags);
        }

        _allocator.GetBudget(this, out var budget);

        var maximumSharedHeapSize = MaximumSharedHeapSize;
        var sizeWithMargins = size + (2 * _allocator.Settings.MinimumAllocatedRegionMarginSize.GetValueOrDefault());

        var heaps = _heaps.AsSpanUnsafe(0, _heaps.Count);

        var availableMemory = (budget.EstimatedUsage < budget.EstimatedBudget) ? (budget.EstimatedBudget - budget.EstimatedUsage) : 0;
        var canCreateNewHeap = !useExistingHeap && (heaps.Length < MaximumHeapCount) && (availableMemory >= sizeWithMargins);

        // 1. Search existing heaps

        if (!useDedicatedHeap && (size <= maximumSharedHeapSize))
        {
            for (var heapIndex = 0; heapIndex < heaps.Length; ++heapIndex)
            {
                var currentHeap = heaps[heapIndex];
                AssertNotNull(currentHeap);

                if (currentHeap.TryAllocate(size, alignment, out heapRegion))
                {
                    if (currentHeap == _emptyHeap)
                    {
                        _emptyHeap = null;
                    }
                    return true;
                }
            }
        }

        // 2. Try to create a new heap

        if (!canCreateNewHeap)
        {
            heapRegion = default;
            return false;
        }

        var heapSize = GetAdjustedHeapSize(sizeWithMargins);

        if (heapSize >= availableMemory)
        {
            heapRegion = default;
            return false;
        }

        var heap = AddHeap(heapSize);
        return heap.TryAllocate(size, alignment, out heapRegion);
    }

    private bool TryAllocateInternal(ulong size, ulong alignment, GraphicsMemoryHeapRegionAllocationFlags flags, Span<GraphicsMemoryHeapRegion> heapRegions)
    {
        // This method should only be called under a write-lock

        var succeeded = false;

        for (var index = 0; index < heapRegions.Length; ++index)
        {
            succeeded = TryAllocateInternal(size, alignment, flags, out heapRegions[index]);

            if (!succeeded)
            {
                // Something failed so free all already allocated regions

                while (index-- != 0)
                {
                    FreeInternal(in heapRegions[index]);
                    heapRegions[index] = default;
                }

                break;
            }
        }

        return succeeded;
    }

    private bool TrySetMinimumSizeInternal(ulong minimumSize)
    {
        // This method should only be called under a write-lock

        var currentMinimumSize = _minimumSize;

        if (minimumSize == currentMinimumSize)
        {
            return true;
        }

        var size = _size;
        var heapCount = _heaps.Count;

        if (minimumSize < currentMinimumSize)
        {
            // The new minimum size is less than the previous, so we will iterate the
            // heaps from last to first (largest to smallest) to try and free space
            // that may now be available.

            var emptyHeap = default(GraphicsMemoryHeap);
            var minimumHeapCount = MinimumHeapCount;

            for (var heapIndex = heapCount; heapIndex-- != 0;)
            {
                var heap = _heaps[heapIndex];

                var heapSize = heap.Size;
                var heapIsEmpty = heap.IsEmpty;

                if (heapIsEmpty)
                {
                    if (((size - heapSize) >= minimumSize) && ((heapCount - 1) >= minimumHeapCount))
                    {
                        RemoveHeapAt(heapIndex);
                        size -= heapSize;
                        --heapCount;
                    }
                    else
                    {
                        emptyHeap ??= heap;
                    }
                }
            }

            _emptyHeap = emptyHeap;
        }
        else
        {
            // The new minimum size is greater than the previous, so we will allocate
            // new heaps until we exceed the minimum size, but ensuring we don't exceed
            // preferredHeapSize while doing so.

            var emptyHeap = default(GraphicsMemoryHeap);
            var maximumHeapCount = MaximumHeapCount;
            var maximumSharedHeapSize = MaximumSharedHeapSize;
            var minimumHeapSize = MinimumHeapSize;

            while (size < minimumSize)
            {
                if (heapCount < maximumHeapCount)
                {
                    var heapSize = GetAdjustedHeapSize(maximumSharedHeapSize);

                    if (((size + heapSize) > minimumSize) && (heapSize != minimumHeapSize))
                    {
                        // The current size plus the new heap will exceed the minimum
                        // size requested, so adjust it to be just large enough.
                        heapSize = minimumSize - size;
                    }

                    emptyHeap ??= AddHeap(heapSize);
                    size += heapSize;

                    ++heapCount;
                }
                else
                {
                    _emptyHeap ??= emptyHeap;
                    return false;
                }
            }

            _emptyHeap ??= emptyHeap;
        }

        _minimumSize = minimumSize;
        Assert(AssertionsEnabled && (_size == size));

        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
