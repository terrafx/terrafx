// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Advanced;
using TerraFX.Collections;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Graphics;

public partial class GraphicsMemoryAllocator
{
    private sealed unsafe class DefaultMemoryAllocator : GraphicsMemoryAllocator
    {
        private ValueLinkedList<GraphicsMemoryRegion> _memoryRegions;
        private ValueList<ValueLinkedList<GraphicsMemoryRegion>.Node> _freeMemoryRegionsBySize;
        private int _freeMemoryRegionCount;
        private nuint _totalFreeMemoryRegionSize;

        public DefaultMemoryAllocator(GraphicsDeviceObject deviceObject, delegate*<in GraphicsMemoryRegion, void> onFree, nuint size, bool isDedicated)
            : base(deviceObject, onFree, size, isDedicated)
        {
            _memoryRegions = new ValueLinkedList<GraphicsMemoryRegion>();
            _freeMemoryRegionsBySize = new ValueList<ValueLinkedList<GraphicsMemoryRegion>.Node>();

            Clear();
        }

        public override int AllocatedMemoryRegionCount => _memoryRegions.Count - _freeMemoryRegionCount;

        public override int Count => _memoryRegions.Count;

        public override bool IsEmpty => (_memoryRegions.Count == 1) && (_freeMemoryRegionCount == 1);

        public override nuint LargestFreeMemoryRegionSize => (_freeMemoryRegionsBySize.Count != 0) ? _freeMemoryRegionsBySize[^1].ValueRef.Size : 0;

        public override nuint TotalFreeMemoryRegionSize => _totalFreeMemoryRegionSize;

        public override void Clear()
        {
            var size = _size;

            _freeMemoryRegionCount = 1;
            _totalFreeMemoryRegionSize = size;

            _memoryRegions.Clear();

            var memoryRegion = new GraphicsMemoryRegion {
                Alignment = 1,
                Allocator = this,
                IsAllocated = false,
                Offset = 0,
                Size = size,
            };

            var memoryRegionNode = _memoryRegions.AddFirst(memoryRegion);

            _freeMemoryRegionsBySize.Clear();
            _freeMemoryRegionsBySize.Add(memoryRegionNode);

            Validate();
        }

        public override IEnumerator<GraphicsMemoryRegion> GetEnumerator() => _memoryRegions.GetEnumerator();

        public override bool TryAllocate(nuint size, [Optional] nuint alignment, out GraphicsMemoryRegion memoryRegion)
        {
            ThrowIfZero(size);

            if (alignment == 0)
            {
                alignment = DefaultAlignment;
            }
            ThrowIfNotPow2(alignment);

            Unsafe.SkipInit(out memoryRegion);

            var sizeWithMargins = size + (2 * MinimumAllocatedMemoryRegionMarginSize);
            var wasMemoryRegionAllocated = false;

            if (TotalFreeMemoryRegionSize >= sizeWithMargins)
            {
                var freeMemoryRegionsBySize = _freeMemoryRegionsBySize.AsSpanUnsafe(0, _freeMemoryRegionsBySize.Count);

                if (freeMemoryRegionsBySize.Length > 0)
                {
                    for (var index = BinarySearchFirstMemoryRegionNodeWithSizeNotLessThan(sizeWithMargins); index < freeMemoryRegionsBySize.Length; ++index)
                    {
                        var memoryRegionNode = freeMemoryRegionsBySize[index];

                        if (TryAllocate(size, alignment, memoryRegionNode))
                        {
                            memoryRegion = memoryRegionNode.ValueRef;
                            wasMemoryRegionAllocated = true;
                            break;
                        }
                    }
                }
            }

            if (!wasMemoryRegionAllocated)
            {
                memoryRegion = default;
            }
            Validate();

            return wasMemoryRegionAllocated;
        }

        protected override bool TryFree(in GraphicsMemoryRegion memoryRegion)
        {
            var freedRegion = false;

            for (var memoryRegionNode = _memoryRegions.First; memoryRegionNode is not null; memoryRegionNode = memoryRegionNode.Next)
            {
                if (memoryRegionNode.ValueRef != memoryRegion)
                {
                    continue;
                }

                _ = FreeRegion(memoryRegionNode);
                freedRegion = true;
            }

            Validate();
            return freedRegion;
        }

        private int BinarySearchFirstMemoryRegionNodeWithSizeNotLessThan(nuint size)
        {
            var freeMemoryRegionsBySize = _freeMemoryRegionsBySize.AsSpanUnsafe(0, _freeMemoryRegionsBySize.Count);

            var index = 0;
            var endIndex = freeMemoryRegionsBySize.Length;

            while (index < endIndex)
            {
                var midIndex = (index + endIndex) / 2;

                if (freeMemoryRegionsBySize[midIndex].ValueRef.Size < size)
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

        private ValueLinkedList<GraphicsMemoryRegion>.Node FreeRegion(ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            ref var memoryRegion = ref memoryRegionNode.ValueRef;

            if (!memoryRegion.IsAllocated)
            {
                return memoryRegionNode;
            }

            memoryRegion = new GraphicsMemoryRegion {
                Alignment = memoryRegion.Alignment,
                Allocator = memoryRegion.Allocator,
                IsAllocated = false,
                Offset = memoryRegion.Offset,
                Size = memoryRegion.Size,
            };

            // Update totals
            ++_freeMemoryRegionCount;
            _totalFreeMemoryRegionSize += memoryRegion.Size;

            // Merge with previous and/or next region if it's also free
            var mergeWithNext = false;
            var mergeWithPrev = false;

            var nextMemoryRegionNode = memoryRegionNode.Next;

            if ((nextMemoryRegionNode is not null) && !nextMemoryRegionNode.ValueRef.IsAllocated)
            {
                mergeWithNext = true;
            }

            var previousMemoryRegionNode = memoryRegionNode.Previous;

            if ((previousMemoryRegionNode is not null) && !previousMemoryRegionNode.ValueRef.IsAllocated)
            {
                mergeWithPrev = true;
            }

            if (mergeWithNext)
            {
                AssertNotNull(nextMemoryRegionNode);
                UnregisterFreeMemoryRegion(nextMemoryRegionNode);
                MergeFreeMemoryRegionWithNext(memoryRegionNode);
            }

            if (mergeWithPrev)
            {
                AssertNotNull(previousMemoryRegionNode);
                UnregisterFreeMemoryRegion(previousMemoryRegionNode);
                MergeFreeMemoryRegionWithNext(previousMemoryRegionNode);
                RegisterFreeMemoryRegion(previousMemoryRegionNode);
                return previousMemoryRegionNode;
            }
            else
            {
                RegisterFreeMemoryRegion(memoryRegionNode);
                return memoryRegionNode;
            }
        }

        private void MergeFreeMemoryRegionWithNext(ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            AssertNotNull(memoryRegionNode);
            Assert(AssertionsEnabled && !memoryRegionNode.ValueRef.IsAllocated);

            var nextMemoryRegionNode = memoryRegionNode.Next;

            AssertNotNull(nextMemoryRegionNode);
            Assert(AssertionsEnabled && !nextMemoryRegionNode.ValueRef.IsAllocated);

            ref var memoryRegion = ref memoryRegionNode.ValueRef;
            ref readonly var nextMemoryRegion = ref nextMemoryRegionNode.ValueRef;

            memoryRegion = new GraphicsMemoryRegion {
                Alignment = memoryRegion.Alignment,
                Allocator = memoryRegion.Allocator,
                IsAllocated = memoryRegion.IsAllocated,
                Offset = memoryRegion.Offset,
                Size = memoryRegion.Size + nextMemoryRegion.Size
            };

            --_freeMemoryRegionCount;

            _memoryRegions.Remove(nextMemoryRegionNode);
        }

        private void RegisterFreeMemoryRegion(ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            Assert(AssertionsEnabled && !memoryRegionNode.ValueRef.IsAllocated);
            Assert(AssertionsEnabled && (memoryRegionNode.ValueRef.Size > 0));

            ValidateFreeMemoryRegionsBySizeList();

            if (memoryRegionNode.ValueRef.Size >= MinimumFreeMemoryRegionSizeToRegister)
            {
                if (_freeMemoryRegionsBySize.Count == 0)
                {
                    _freeMemoryRegionsBySize.Add(memoryRegionNode);
                }
                else
                {
                    var index = BinarySearchFirstMemoryRegionNodeWithSizeNotLessThan(memoryRegionNode.ValueRef.Size);
                    _freeMemoryRegionsBySize.Insert(index, memoryRegionNode);
                }
            }

            ValidateFreeMemoryRegionsBySizeList();
        }

        private bool TryAllocate(nuint size, nuint alignment, ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            Assert(AssertionsEnabled && (size > 0));
            AssertNotNull(memoryRegionNode);

            ref var memoryRegion = ref memoryRegionNode.ValueRef;
            Assert(AssertionsEnabled && !memoryRegion.IsAllocated);

            if (memoryRegion.Size < size)
            {
                return false;
            }

            // Start from an offset equal to the beginning of this memory region.
            var offset = memoryRegion.Offset;

            // Apply MarginSize at the beginning.
            if (MinimumAllocatedMemoryRegionMarginSize > 0)
            {
                offset += MinimumAllocatedMemoryRegionMarginSize;
            }

            // Apply alignment.
            offset = AlignUp(offset, alignment);

            // Calculate padding at the beginning based on current offset.
            var paddingBegin = offset - memoryRegion.Offset;

            // Calculate required margin at the end.
            var requiredEndMargin = MinimumAllocatedMemoryRegionMarginSize;

            // Fail if requested size plus margin before and after is bigger than size of this memory region.
            if ((paddingBegin + size + requiredEndMargin) > memoryRegion.Size)
            {
                return false;
            }

            var paddingEnd = memoryRegion.Size - paddingBegin - size;

            UnregisterFreeMemoryRegion(memoryRegionNode);

            memoryRegion = new GraphicsMemoryRegion {
                Alignment = alignment,
                Allocator = this,
                IsAllocated = true,
                Offset = offset,
                Size = size,
            };

            if (paddingEnd != 0)
            {
                // If there are any free bytes remaining at the end, insert a new free memory region after the current one

                var paddingMemoryRegion = new GraphicsMemoryRegion {
                    Alignment = 1,
                    Allocator = this,
                    IsAllocated = false,
                    Offset = offset + size,
                    Size = paddingEnd,
                };

                var paddingEndItem = _memoryRegions.AddAfter(memoryRegionNode, paddingMemoryRegion);
                RegisterFreeMemoryRegion(paddingEndItem);
            }

            if (paddingBegin != 0)
            {
                // If there are any free bytes remaining at the beginning, insert a new free region before the current one

                var paddingMemoryRegion = new GraphicsMemoryRegion {
                    Alignment = 1,
                    Allocator = this,
                    IsAllocated = false,
                    Offset = offset - paddingBegin,
                    Size = paddingBegin,
                };

                var paddingBeginItem = _memoryRegions.AddBefore(memoryRegionNode, paddingMemoryRegion);
                RegisterFreeMemoryRegion(paddingBeginItem);
            }

            // Update totals

            --_freeMemoryRegionCount;

            if (paddingBegin > 0)
            {
                ++_freeMemoryRegionCount;
            }

            if (paddingEnd > 0)
            {
                ++_freeMemoryRegionCount;
            }

            _totalFreeMemoryRegionSize -= size;
            return true;
        }

        private void UnregisterFreeMemoryRegion(ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            Assert(AssertionsEnabled && !memoryRegionNode.ValueRef.IsAllocated);
            Assert(AssertionsEnabled && (memoryRegionNode.ValueRef.Size > 0));

            ValidateFreeMemoryRegionsBySizeList();

            if (memoryRegionNode.ValueRef.Size >= MinimumFreeMemoryRegionSizeToRegister)
            {
                for (var index = BinarySearchFirstMemoryRegionNodeWithSizeNotLessThan(memoryRegionNode.ValueRef.Size); index < _freeMemoryRegionsBySize.Count; ++index)
                {
                    if (_freeMemoryRegionsBySize[index] == memoryRegionNode)
                    {
                        _freeMemoryRegionsBySize.RemoveAt(index);
                        return;
                    }
                    Assert(AssertionsEnabled && (_freeMemoryRegionsBySize[index].ValueRef.Size == memoryRegionNode.ValueRef.Size));
                }

                ThrowKeyNotFoundException(memoryRegionNode, nameof(_freeMemoryRegionsBySize));
            }

            ValidateFreeMemoryRegionsBySizeList();
        }

        [Conditional("DEBUG")]
        private void Validate()
        {
            Assert(AssertionsEnabled && (_memoryRegions.Count != 0));

            nuint calculatedSize = 0;
            nuint calculatedTotalFreeRegionSize = 0;

            var calculatedFreeRegionCount = 0;
            var calculatedFreeRegionsToRegisterCount = 0;

            // True if previous visited memory region was free.
            var isPreviousMemoryRegionFree = false;

            for (var memoryRegionNode = _memoryRegions.First; memoryRegionNode is not null; memoryRegionNode = memoryRegionNode.Next)
            {
                ref readonly var memoryRegion = ref memoryRegionNode.ValueRef;

                // The node should immediately procede the previous
                Assert(AssertionsEnabled && (memoryRegion.Offset == calculatedSize));

                var isCurrentMemoryRegionFree = !memoryRegion.IsAllocated;

                // Two adjacent free memory regions are invalid, they should have been merged
                Assert(AssertionsEnabled && (!isPreviousMemoryRegionFree || !isCurrentMemoryRegionFree));

                if (isCurrentMemoryRegionFree)
                {
                    calculatedTotalFreeRegionSize += memoryRegion.Size;
                    ++calculatedFreeRegionCount;

                    if (memoryRegion.Size >= MinimumFreeMemoryRegionSizeToRegister)
                    {
                        ++calculatedFreeRegionsToRegisterCount;
                    }

                    // When margins are required between allocations every free space must be at least that large
                    Assert(AssertionsEnabled && (memoryRegion.Size >= MinimumAllocatedMemoryRegionMarginSize));
                }
                else
                {
                    // When margins are required between allocations, the previous allocation must be free
                    Assert(AssertionsEnabled && ((MinimumAllocatedMemoryRegionMarginSize == 0) || isPreviousMemoryRegionFree));
                }

                calculatedSize += memoryRegion.Size;
                isPreviousMemoryRegionFree = isCurrentMemoryRegionFree;
            }

            ValidateFreeMemoryRegionsBySizeList();

            // All totals should match the computed values
            Assert(AssertionsEnabled && (calculatedSize == Size));
            Assert(AssertionsEnabled && (calculatedTotalFreeRegionSize == _totalFreeMemoryRegionSize));
            Assert(AssertionsEnabled && (calculatedFreeRegionCount == _freeMemoryRegionCount));
            Assert(AssertionsEnabled && (calculatedFreeRegionsToRegisterCount == _freeMemoryRegionsBySize.Count));
        }

        [Conditional("DEBUG")]
        private void ValidateFreeMemoryRegionsBySizeList()
        {
            nuint lastMemoryRegionSize = 0;
            var freeMemoryRegionsBySize = _freeMemoryRegionsBySize.AsSpanUnsafe(0, _freeMemoryRegionsBySize.Count);

            for (var i = 0; i < freeMemoryRegionsBySize.Length; ++i)
            {
                ref readonly var memoryRegion = ref freeMemoryRegionsBySize[i].ValueRef;

                Assert(AssertionsEnabled && !memoryRegion.IsAllocated);
                Assert(AssertionsEnabled && (memoryRegion.Size >= MinimumFreeMemoryRegionSizeToRegister));
                Assert(AssertionsEnabled && (memoryRegion.Size >= lastMemoryRegionSize));

                lastMemoryRegionSize = memoryRegion.Size;
            }
        }
    }
}
