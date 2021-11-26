// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Graphics;

public partial interface IGraphicsMemoryRegionCollection<TSelf>
{
    /// <summary>The default metadata for a collection of memory regions.</summary>
    public struct DefaultMetadata : IMetadata
    {
        private TSelf _collection;
        private List<LinkedListNode<GraphicsMemoryRegion<TSelf>>> _freeRegionsBySize;
        private LinkedList<GraphicsMemoryRegion<TSelf>> _regions;
        private ulong _minimumFreeRegionSizeToRegister;
        private ulong _minimumAllocatedRegionMarginSize;
        private ulong _size;
        private ulong _totalFreeRegionSize;
        private int _freeRegionCount;

        /// <inheritdoc />
        public int AllocatedRegionCount => _regions.Count - _freeRegionCount;

        /// <summary>Gets the number of regions in the collection.</summary>
        public int Count => _regions.Count;

        /// <inheritdoc />
        public GraphicsDevice Device => _collection.Device;

        /// <inheritdoc />
        public bool IsEmpty => (_regions.Count == 1) && (_freeRegionCount == 1);

        /// <inheritdoc />
        public ulong LargestFreeRegionSize => (_freeRegionsBySize.Count != 0) ? _freeRegionsBySize[^1].ValueRef.Size : 0;

        /// <inheritdoc />
        public ulong MinimumAllocatedRegionMarginSize => _minimumAllocatedRegionMarginSize;

        /// <inheritdoc />
        public ulong MinimumFreeRegionSizeToRegister => _minimumFreeRegionSizeToRegister;

        /// <inheritdoc />
        public ulong Size => _size;

        /// <inheritdoc />
        public ulong TotalFreeRegionSize => _totalFreeRegionSize;

        /// <inheritdoc />
        public GraphicsMemoryRegion<TSelf> Allocate(ulong size, ulong alignment = 1)
        {
            var result = TryAllocate(size, alignment, out var region);

            if (!result)
            {
                ThrowOutOfMemoryException(size);
            }
            return region;
        }

        /// <inheritdoc />
        public void Clear()
        {
            var size = _size;

            _freeRegionCount = 1;
            _totalFreeRegionSize = size;

            var regions = _regions;
            regions.Clear();

            var region = new GraphicsMemoryRegion<TSelf> {
                Alignment = 1,
                Collection = _collection,
                IsAllocated = false,
                Offset = 0,
                Size = size,
            };
            var regionNode = regions.AddFirst(region);

            var freeRegionsBySize = _freeRegionsBySize;

            freeRegionsBySize.Clear();
            freeRegionsBySize.Add(regionNode);

            Validate();
        }

        /// <inheritdoc />
        public void Free(in GraphicsMemoryRegion<TSelf> region)
        {
            var freedRegion = false;
            var regions = _regions;

            for (var regionNode = regions.First; regionNode is not null; regionNode = regionNode.Next)
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
                ThrowKeyNotFoundException(region, nameof(regions));
            }
            Validate();
        }

        /// <summary>Gets an enumerator that can be used to iterate through the regions of the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the regions of the collection.</returns>
        public IEnumerator<GraphicsMemoryRegion<TSelf>> GetEnumerator()
            => _regions.GetEnumerator();

        /// <inheritdoc />
        public void Initialize(TSelf collection, ulong size, ulong minimumAllocatedRegionMarginSize, ulong minimumFreeRegionSizeToRegister)
        {
            ThrowIfNull(collection, nameof(collection));
            ThrowIfZero(size, nameof(size));

            _collection = collection;

            _freeRegionsBySize = new List<LinkedListNode<GraphicsMemoryRegion<TSelf>>>();
            _regions = new LinkedList<GraphicsMemoryRegion<TSelf>>();

            _minimumAllocatedRegionMarginSize = minimumAllocatedRegionMarginSize;
            _minimumFreeRegionSizeToRegister = minimumFreeRegionSizeToRegister;

            _size = size;

            Clear();
        }

        /// <inheritdoc />
        public bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, out GraphicsMemoryRegion<TSelf> region)
        {
            ThrowIfZero(size, nameof(size));
            ThrowIfNotPow2(alignment, nameof(alignment));

            Unsafe.SkipInit(out region);

            var sizeWithMargins = size + (2 * MinimumAllocatedRegionMarginSize);
            var allocatedRegion = false;

            if (TotalFreeRegionSize >= sizeWithMargins)
            {
                var freeRegionsBySize = CollectionsMarshal.AsSpan(_freeRegionsBySize);
                var freeRegionsBySizeLength = freeRegionsBySize.Length;

                if (freeRegionsBySizeLength > 0)
                {
                    for (var index = BinarySearchFirstRegionNodeWithSizeNotLessThan(sizeWithMargins); index < freeRegionsBySizeLength; ++index)
                    {
                        var regionNode = freeRegionsBySize[index];

                        if (TryAllocate(size, alignment, regionNode))
                        {
                            region = regionNode.ValueRef;
                            allocatedRegion = true;
                            break;
                        }
                    }
                }
            }

            if (!allocatedRegion)
            {
                region = default;
            }
            Validate();

            return allocatedRegion;
        }

        /// <summary>Performs validation of the collection to ensure it is correct.</summary>
        [Conditional("DEBUG")]
        public void Validate()
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

        private int BinarySearchFirstRegionNodeWithSizeNotLessThan(ulong size)
        {
            var freeRegionsBySize = CollectionsMarshal.AsSpan(_freeRegionsBySize);

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

        private LinkedListNode<GraphicsMemoryRegion<TSelf>> FreeRegion(LinkedListNode<GraphicsMemoryRegion<TSelf>> regionNode)
        {
            ref var region = ref regionNode.ValueRef;

            if (!region.IsAllocated)
            {
                return regionNode;
            }

            region = new GraphicsMemoryRegion<TSelf> {
                Alignment = region.Alignment,
                Collection = region.Collection,
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

        private void MergeFreeRegionWithNext(LinkedListNode<GraphicsMemoryRegion<TSelf>> regionNode)
        {
            AssertNotNull(regionNode);
            Assert(AssertionsEnabled && !regionNode.ValueRef.IsAllocated);

            var nextRegionNode = regionNode.Next;

            AssertNotNull(nextRegionNode);
            Assert(AssertionsEnabled && !nextRegionNode.ValueRef.IsAllocated);

            ref var region = ref regionNode.ValueRef;
            ref readonly var nextRegion = ref nextRegionNode.ValueRef;

            region = new GraphicsMemoryRegion<TSelf> {
                Alignment = region.Alignment,
                Collection = region.Collection,
                IsAllocated = region.IsAllocated,
                Offset = region.Offset,
                Size = region.Size + nextRegion.Size
            };

            --_freeRegionCount;

            _regions.Remove(nextRegionNode);
        }

        private void RegisterFreeRegion(LinkedListNode<GraphicsMemoryRegion<TSelf>> regionNode)
        {
            Assert(AssertionsEnabled && !regionNode.ValueRef.IsAllocated);
            Assert(AssertionsEnabled && (regionNode.ValueRef.Size > 0));

            ValidateFreeRegionsBySizeList();

            if (regionNode.ValueRef.Size >= MinimumFreeRegionSizeToRegister)
            {
                var freeRegionsBySize = _freeRegionsBySize;

                if (freeRegionsBySize.Count == 0)
                {
                    freeRegionsBySize.Add(regionNode);
                }
                else
                {
                    var index = BinarySearchFirstRegionNodeWithSizeNotLessThan(regionNode.ValueRef.Size);
                    freeRegionsBySize.Insert(index, regionNode);
                }
            }

            ValidateFreeRegionsBySizeList();
        }

        private bool TryAllocate(ulong size, ulong alignment, LinkedListNode<GraphicsMemoryRegion<TSelf>> regionNode)
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

            region = new GraphicsMemoryRegion<TSelf> {
                Alignment = alignment,
                Collection = _collection,
                IsAllocated = true,
                Offset = offset,
                Size = size,
            };

            if (paddingEnd != 0)
            {
                // If there are any free bytes remaining at the end, insert a new free region after the current one

                var paddingRegion = new GraphicsMemoryRegion<TSelf> {
                    Alignment = 1,
                    Collection = _collection,
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

                var paddingRegion = new GraphicsMemoryRegion<TSelf> {
                    Alignment = 1,
                    Collection = _collection,
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

        private void UnregisterFreeRegion(LinkedListNode<GraphicsMemoryRegion<TSelf>> regionNode)
        {
            Assert(AssertionsEnabled && !regionNode.ValueRef.IsAllocated);
            Assert(AssertionsEnabled && (regionNode.ValueRef.Size > 0));

            ValidateFreeRegionsBySizeList();

            if (regionNode.ValueRef.Size >= MinimumFreeRegionSizeToRegister)
            {
                var freeRegionsBySize = _freeRegionsBySize;

                for (var index = BinarySearchFirstRegionNodeWithSizeNotLessThan(regionNode.ValueRef.Size); index < freeRegionsBySize.Count; ++index)
                {
                    if (freeRegionsBySize[index] == regionNode)
                    {
                        freeRegionsBySize.RemoveAt(index);
                        return;
                    }
                    Assert(AssertionsEnabled && (freeRegionsBySize[index].ValueRef.Size == regionNode.ValueRef.Size));
                }

                ThrowKeyNotFoundException(regionNode, nameof(freeRegionsBySize));
            }

            ValidateFreeRegionsBySizeList();
        }

        [Conditional("DEBUG")]
        private void ValidateFreeRegionsBySizeList()
        {
            var lastRegionSize = 0UL;
            var freeRegionsBySize = CollectionsMarshal.AsSpan(_freeRegionsBySize);

            for (var i = 0; i < freeRegionsBySize.Length; ++i)
            {
                ref readonly var region = ref freeRegionsBySize[i].ValueRef;

                Assert(AssertionsEnabled && !region.IsAllocated);
                Assert(AssertionsEnabled && (region.Size >= MinimumFreeRegionSizeToRegister));
                Assert(AssertionsEnabled && (region.Size >= lastRegionSize));

                lastRegionSize = region.Size;
            }
        }
    }
}
