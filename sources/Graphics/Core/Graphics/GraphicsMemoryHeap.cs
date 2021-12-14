// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Collections;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Defines a single heap of memory which can contain allocated or free regions.</summary>
public abstract partial class GraphicsMemoryHeap : GraphicsDeviceObject, IReadOnlyCollection<GraphicsMemoryHeapRegion>
{
    private readonly GraphicsMemoryHeapCollection _collection;
    private readonly ulong _minimumFreeRegionSizeToRegister;
    private readonly ulong _minimumAllocatedRegionMarginSize;
    private readonly ulong _size;

    private ValueLinkedList<GraphicsMemoryHeapRegion> _regions;
    private ValueList<ValueLinkedList<GraphicsMemoryHeapRegion>.Node> _freeRegionsBySize;

    private ulong _totalFreeRegionSize;
    private int _freeRegionCount;

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryHeap" /> class.</summary>
    /// <param name="device">The device for which the memory heap is being created</param>
    /// <param name="collection">The memory heap collection which contains the heap.</param>
    /// <param name="size">The size of the memory heap, in bytes.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="collection" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsMemoryHeap(GraphicsDevice device, GraphicsMemoryHeapCollection collection, ulong size)
        : base(device)
    {
        ThrowIfNull(collection);
        ThrowIfZero(size);

        if (collection.Device != device)
        {
            ThrowForInvalidParent(collection.Device);
        }

        _collection = collection;

        _freeRegionsBySize = new ValueList<ValueLinkedList<GraphicsMemoryHeapRegion>.Node>();
        _regions = new ValueLinkedList<GraphicsMemoryHeapRegion>();

        ref readonly var settings = ref collection.Allocator.Settings;

        _minimumAllocatedRegionMarginSize = settings.MinimumAllocatedRegionMarginSize.GetValueOrDefault();
        _minimumFreeRegionSizeToRegister = settings.MinimumFreeRegionSizeToRegister;

        _size = size;

        Clear();
    }

    /// <summary>Gets the number of allocated regions in the heap.</summary>
    public int AllocatedRegionCount => _regions.Count - _freeRegionCount;

    /// <summary>Gets the memory heap collection which contains the heap.</summary>
    public GraphicsMemoryHeapCollection Collection => _collection;

    /// <summary>Gets the number of regions in the heap.</summary>
    public int Count => _regions.Count;

    /// <summary>Gets <c>true</c> if the heap contains no allocated regions; otherwise, <c>false</c>.</summary>
    public bool IsEmpty => (_regions.Count == 1) && (_freeRegionCount == 1);

    /// <summary>Gets the size of the largest free region, in bytes.</summary>
    public ulong LargestFreeRegionSize => (_freeRegionsBySize.Count != 0) ? _freeRegionsBySize[^1].ValueRef.Size : 0;

    /// <summary>Gets the minimum size of free regions to keep on either side of an allocated region, in bytes.</summary>
    public ulong MinimumAllocatedRegionMarginSize => _minimumAllocatedRegionMarginSize;

    /// <summary>Gets the minimum size a free region should be for it to be registered as available, in bytes.</summary>
    public ulong MinimumFreeRegionSizeToRegister => _minimumFreeRegionSizeToRegister;

    /// <summary>Gets the size of the heap, in bytes.</summary>
    public ulong Size => _size;

    /// <summary>Gets the total size of free regions, in bytes.</summary>
    public ulong TotalFreeRegionSize => _totalFreeRegionSize;

    /// <summary>Allocates a region of the specified size and alignment.</summary>
    /// <param name="size">The size of the region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the region to allocate, in bytes.</param>
    /// <returns>The allocated region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free region to complete the allocation.</exception>
    public GraphicsMemoryHeapRegion Allocate(ulong size, ulong alignment = 0)
    {
        var result = TryAllocate(size, alignment, out var region);

        if (!result)
        {
            ThrowOutOfMemoryException(size);
        }
        return region;
    }

    /// <summary>Clears the heap of allocated regions.</summary>
    public void Clear()
    {
        var size = _size;

        _freeRegionCount = 1;
        _totalFreeRegionSize = size;

        _regions.Clear();

        var region = new GraphicsMemoryHeapRegion {
            Alignment = 1,
            Heap = this,
            IsAllocated = false,
            Offset = 0,
            Size = size,
        };

        var regionNode = _regions.AddFirst(region);

        _freeRegionsBySize.Clear();
        _freeRegionsBySize.Add(regionNode);

        Validate();
    }

    /// <summary>Frees a region of memory.</summary>
    /// <param name="region">The region of memory to be freed.</param>
    /// <exception cref="KeyNotFoundException"><paramref name="region" /> was not found in the heap.</exception>
    public void Free(in GraphicsMemoryHeapRegion region)
    {
        var freedRegion = false;

        for (var regionNode = _regions.First; regionNode is not null; regionNode = regionNode.Next)
        {
            if (regionNode.ValueRef != region)
            {
                continue;
            }

            _ = FreeRegion(regionNode);
            freedRegion = true;
        }

        if (!freedRegion)
        {
            ThrowKeyNotFoundException(region, nameof(_regions));
        }
        Validate();
    }

    /// <summary>Gets an enumerator that can be used to iterate through the regions of the heap.</summary>
    /// <returns>An enumerator that can be used to iterate through the regions of the heap.</returns>
    public IEnumerator<GraphicsMemoryHeapRegion> GetEnumerator() => _regions.GetEnumerator();

    /// <summary>Tries to allocate a region of the specified size and alignment.</summary>
    /// <param name="size">The size of the region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the region to allocate, in bytes.</param>
    /// <param name="region">On return, contains the allocated region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    public bool TryAllocate(ulong size, [Optional] ulong alignment, out GraphicsMemoryHeapRegion region)
    {
        ThrowIfZero(size);

        if (alignment == 0)
        {
            alignment = DefaultAlignment;
        }
        ThrowIfNotPow2(alignment);

        SkipInit(out region);

        var sizeWithMargins = size + (2 * MinimumAllocatedRegionMarginSize);
        var wasRegionAllocated = false;

        if (TotalFreeRegionSize >= sizeWithMargins)
        {
            var freeRegionsBySize = _freeRegionsBySize.AsSpanUnsafe().Slice(0, _freeRegionsBySize.Count);

            if (freeRegionsBySize.Length > 0)
            {
                for (var index = BinarySearchFirstRegionNodeWithSizeNotLessThan(sizeWithMargins); index < freeRegionsBySize.Length; ++index)
                {
                    var regionNode = freeRegionsBySize[index];

                    if (TryAllocate(size, alignment, regionNode))
                    {
                        region = regionNode.ValueRef;
                        wasRegionAllocated = true;
                        break;
                    }
                }
            }
        }

        if (!wasRegionAllocated)
        {
            region = default;
        }
        Validate();

        return wasRegionAllocated;
    }

    private int BinarySearchFirstRegionNodeWithSizeNotLessThan(ulong size)
    {
        var freeRegionsBySize = _freeRegionsBySize.AsSpanUnsafe().Slice(0, _freeRegionsBySize.Count);

        var index = 0;
        var endIndex = freeRegionsBySize.Length;

        while (index < endIndex)
        {
            var midIndex = (index + endIndex) / 2;

            if (freeRegionsBySize[midIndex].ValueRef.Size < size)
            {
                index = midIndex + 1;
            }
            else
            {
                endIndex = midIndex;
            }
        }

        return index;
    }

    private ValueLinkedList<GraphicsMemoryHeapRegion>.Node FreeRegion(ValueLinkedList<GraphicsMemoryHeapRegion>.Node regionNode)
    {
        ref var region = ref regionNode.ValueRef;

        if (!region.IsAllocated)
        {
            return regionNode;
        }

        region = new GraphicsMemoryHeapRegion {
            Alignment = region.Alignment,
            Heap = region.Heap,
            IsAllocated = false,
            Offset = region.Offset,
            Size = region.Size,
        };

        // Update totals
        ++_freeRegionCount;
        _totalFreeRegionSize += region.Size;

        // Merge with previous and/or next region if it's also free
        var mergeWithNext = false;
        var mergeWithPrev = false;

        var nextRegionNode = regionNode.Next;

        if ((nextRegionNode is not null) && !nextRegionNode.ValueRef.IsAllocated)
        {
            mergeWithNext = true;
        }

        var prevRegionNode = regionNode.Previous;

        if ((prevRegionNode is not null) && !prevRegionNode.ValueRef.IsAllocated)
        {
            mergeWithPrev = true;
        }

        if (mergeWithNext)
        {
            AssertNotNull(nextRegionNode);
            UnregisterFreeRegion(nextRegionNode);
            MergeFreeRegionWithNext(regionNode);
        }

        if (mergeWithPrev)
        {
            AssertNotNull(prevRegionNode);
            UnregisterFreeRegion(prevRegionNode);
            MergeFreeRegionWithNext(prevRegionNode);
            RegisterFreeRegion(prevRegionNode);
            return prevRegionNode;
        }
        else
        {
            RegisterFreeRegion(regionNode);
            return regionNode;
        }
    }

    private void MergeFreeRegionWithNext(ValueLinkedList<GraphicsMemoryHeapRegion>.Node regionNode)
    {
        AssertNotNull(regionNode);
        Assert(AssertionsEnabled && !regionNode.ValueRef.IsAllocated);

        var nextRegionNode = regionNode.Next;

        AssertNotNull(nextRegionNode);
        Assert(AssertionsEnabled && !nextRegionNode.ValueRef.IsAllocated);

        ref var region = ref regionNode.ValueRef;
        ref readonly var nextRegion = ref nextRegionNode.ValueRef;

        region = new GraphicsMemoryHeapRegion {
            Alignment = region.Alignment,
            Heap = region.Heap,
            IsAllocated = region.IsAllocated,
            Offset = region.Offset,
            Size = region.Size + nextRegion.Size
        };

        --_freeRegionCount;

        _regions.Remove(nextRegionNode);
    }

    private void RegisterFreeRegion(ValueLinkedList<GraphicsMemoryHeapRegion>.Node regionNode)
    {
        Assert(AssertionsEnabled && !regionNode.ValueRef.IsAllocated);
        Assert(AssertionsEnabled && (regionNode.ValueRef.Size > 0));

        ValidateFreeRegionsBySizeList();

        if (regionNode.ValueRef.Size >= MinimumFreeRegionSizeToRegister)
        {
            if (_freeRegionsBySize.Count == 0)
            {
                _freeRegionsBySize.Add(regionNode);
            }
            else
            {
                var index = BinarySearchFirstRegionNodeWithSizeNotLessThan(regionNode.ValueRef.Size);
                _freeRegionsBySize.Insert(index, regionNode);
            }
        }

        ValidateFreeRegionsBySizeList();
    }

    private bool TryAllocate(ulong size, ulong alignment, ValueLinkedList<GraphicsMemoryHeapRegion>.Node regionNode)
    {
        Assert(AssertionsEnabled && (size > 0));
        AssertNotNull(regionNode);

        ref var region = ref regionNode.ValueRef;
        Assert(AssertionsEnabled && !region.IsAllocated);

        if (region.Size < size)
        {
            return false;
        }

        // Start from an offset equal to the beginning of this region.
        var offset = region.Offset;

        // Apply MarginSize at the beginning.
        if (MinimumAllocatedRegionMarginSize > 0)
        {
            offset += MinimumAllocatedRegionMarginSize;
        }

        // Apply alignment.
        offset = AlignUp(offset, alignment);

        // Calculate padding at the beginning based on current offset.
        var paddingBegin = offset - region.Offset;

        // Calculate required margin at the end.
        var requiredEndMargin = MinimumAllocatedRegionMarginSize;

        // Fail if requested size plus margin before and after is bigger than size of this region.
        if ((paddingBegin + size + requiredEndMargin) > region.Size)
        {
            return false;
        }

        var paddingEnd = region.Size - paddingBegin - size;

        UnregisterFreeRegion(regionNode);

        region = new GraphicsMemoryHeapRegion {
            Alignment = alignment,
            Heap = this,
            IsAllocated = true,
            Offset = offset,
            Size = size,
        };

        if (paddingEnd != 0)
        {
            // If there are any free bytes remaining at the end, insert a new free region after the current one

            var paddingRegion = new GraphicsMemoryHeapRegion {
                Alignment = 1,
                Heap = this,
                IsAllocated = false,
                Offset = offset + size,
                Size = paddingEnd,
            };

            var paddingEndItem = _regions.AddAfter(regionNode, paddingRegion);
            RegisterFreeRegion(paddingEndItem);
        }

        if (paddingBegin != 0)
        {
            // If there are any free bytes remaining at the beginning, insert a new free region before the current one

            var paddingRegion = new GraphicsMemoryHeapRegion {
                Alignment = 1,
                Heap = this,
                IsAllocated = false,
                Offset = offset - paddingBegin,
                Size = paddingBegin,
            };

            var paddingBeginItem = _regions.AddBefore(regionNode, paddingRegion);
            RegisterFreeRegion(paddingBeginItem);
        }

        // Update totals

        --_freeRegionCount;

        if (paddingBegin > 0)
        {
            ++_freeRegionCount;
        }

        if (paddingEnd > 0)
        {
            ++_freeRegionCount;
        }

        _totalFreeRegionSize -= size;
        return true;
    }

    private void UnregisterFreeRegion(ValueLinkedList<GraphicsMemoryHeapRegion>.Node regionNode)
    {
        Assert(AssertionsEnabled && !regionNode.ValueRef.IsAllocated);
        Assert(AssertionsEnabled && (regionNode.ValueRef.Size > 0));

        ValidateFreeRegionsBySizeList();

        if (regionNode.ValueRef.Size >= MinimumFreeRegionSizeToRegister)
        {
            for (var index = BinarySearchFirstRegionNodeWithSizeNotLessThan(regionNode.ValueRef.Size); index < _freeRegionsBySize.Count; ++index)
            {
                if (_freeRegionsBySize[index] == regionNode)
                {
                    _freeRegionsBySize.RemoveAt(index);
                    return;
                }
                Assert(AssertionsEnabled && (_freeRegionsBySize[index].ValueRef.Size == regionNode.ValueRef.Size));
            }

            ThrowKeyNotFoundException(regionNode, nameof(_freeRegionsBySize));
        }

        ValidateFreeRegionsBySizeList();
    }

    [Conditional("DEBUG")]
    private void Validate()
    {
        Assert(AssertionsEnabled && (_regions.Count != 0));

        var calculatedSize = 0UL;
        var calculatedTotalFreeRegionSize = 0UL;

        var calculatedFreeRegionCount = 0;
        var calculatedFreeRegionsToRegisterCount = 0;

        // True if previous visited region was free.
        var isPreviousRegionFree = false;

        for (var regionNode = _regions.First; regionNode is not null; regionNode = regionNode.Next)
        {
            ref readonly var region = ref regionNode.ValueRef;

            // The node should immediately procede the previous
            Assert(AssertionsEnabled && (region.Offset == calculatedSize));

            var isCurrentRegionFree = !region.IsAllocated;

            // Two adjacent free regions are invalid, they should have been merged
            Assert(AssertionsEnabled && (!isPreviousRegionFree || !isCurrentRegionFree));

            if (isCurrentRegionFree)
            {
                calculatedTotalFreeRegionSize += region.Size;
                ++calculatedFreeRegionCount;

                if (region.Size >= MinimumFreeRegionSizeToRegister)
                {
                    ++calculatedFreeRegionsToRegisterCount;
                }

                // When margins are required between allocations every free space must be at least that large
                Assert(AssertionsEnabled && (region.Size >= MinimumAllocatedRegionMarginSize));
            }
            else
            {
                // When margins are required between allocations, the previous allocation must be free
                Assert(AssertionsEnabled && ((MinimumAllocatedRegionMarginSize == 0) || isPreviousRegionFree));
            }

            calculatedSize += region.Size;
            isPreviousRegionFree = isCurrentRegionFree;
        }

        ValidateFreeRegionsBySizeList();

        // All totals should match the computed values
        Assert(AssertionsEnabled && (calculatedSize == Size));
        Assert(AssertionsEnabled && (calculatedTotalFreeRegionSize == _totalFreeRegionSize));
        Assert(AssertionsEnabled && (calculatedFreeRegionCount == _freeRegionCount));
        Assert(AssertionsEnabled && (calculatedFreeRegionsToRegisterCount == _freeRegionsBySize.Count));
    }

    [Conditional("DEBUG")]
    private void ValidateFreeRegionsBySizeList()
    {
        var lastRegionSize = 0UL;
        var freeRegionsBySize = _freeRegionsBySize.AsSpanUnsafe().Slice(0, _freeRegionsBySize.Count);

        for (var i = 0; i < freeRegionsBySize.Length; ++i)
        {
            ref readonly var region = ref freeRegionsBySize[i].ValueRef;

            Assert(AssertionsEnabled && !region.IsAllocated);
            Assert(AssertionsEnabled && (region.Size >= MinimumFreeRegionSizeToRegister));
            Assert(AssertionsEnabled && (region.Size >= lastRegionSize));

            lastRegionSize = region.Size;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
