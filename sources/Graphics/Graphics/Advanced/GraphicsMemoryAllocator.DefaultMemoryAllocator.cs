// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Advanced;

public partial class GraphicsMemoryAllocator
{
    private sealed unsafe class DefaultMemoryAllocator : GraphicsMemoryAllocator
    {
        private ValueLinkedList<GraphicsMemoryRegion> _memoryRegions;
        private ValueList<ValueLinkedList<GraphicsMemoryRegion>.Node> _freeMemoryRegionsByByteLength;
        private uint _freeMemoryRegionCount;

        public DefaultMemoryAllocator(GraphicsDeviceObject deviceObject, in GraphicsMemoryAllocatorCreateOptions createOptions) : base(deviceObject)
        {
            ThrowIfZero(createOptions.ByteLength);

            MemoryAllocatorInfo.ByteLength = createOptions.ByteLength;
            MemoryAllocatorInfo.IsDedicated = createOptions.IsDedicated;
            MemoryAllocatorInfo.OnFree = createOptions.OnFree;

            _memoryRegions = new ValueLinkedList<GraphicsMemoryRegion>();
            _freeMemoryRegionsByByteLength = new ValueList<ValueLinkedList<GraphicsMemoryRegion>.Node>();

            ClearUnsafe();
        }

        /// <inheritdoc />
        protected override void ClearUnsafe()
        {
            var byteLength = MemoryAllocatorInfo.ByteLength;

            MemoryAllocatorInfo.IsEmpty = true;
            MemoryAllocatorInfo.TotalFreeMemoryRegionByteLength = byteLength;

            _memoryRegions.Clear();

            var memoryRegion = new GraphicsMemoryRegion {
                ByteAlignment = 1,
                MemoryAllocator = this,
                IsAllocated = false,
                ByteOffset = 0,
                ByteLength = byteLength,
            };

            var memoryRegionNode = _memoryRegions.AddFirst(memoryRegion);

            _freeMemoryRegionsByByteLength.Clear();
            _freeMemoryRegionsByByteLength.Add(memoryRegionNode);
            _freeMemoryRegionCount = 1;

            Validate();
        }

        protected override bool TryAllocateUnsafe(nuint byteLength, [Optional] nuint byteAlignment, out GraphicsMemoryRegion memoryRegion)
        {
            Unsafe.SkipInit(out memoryRegion);

            var byteLengthWithMargins = byteLength + (2 * MinimumAllocatedMemoryRegionMarginByteLength);
            var wasMemoryRegionAllocated = false;

            if (TotalFreeMemoryRegionByteLength >= byteLengthWithMargins)
            {
                var freeMemoryRegionsByByteLength = _freeMemoryRegionsByByteLength.AsSpanUnsafe(0, _freeMemoryRegionsByByteLength.Count);

                if (freeMemoryRegionsByByteLength.Length > 0)
                {
                    for (var index = BinarySearchFirstMemoryRegionNodeWithSizeNotLessThan(byteLengthWithMargins); index < freeMemoryRegionsByByteLength.Length; index++)
                    {
                        var memoryRegionNode = freeMemoryRegionsByByteLength[index];

                        if (TryAllocate(byteLength, byteAlignment, memoryRegionNode))
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

        protected override bool TryFreeUnsafe(in GraphicsMemoryRegion memoryRegion)
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
            var freeMemoryRegionsByByteLength = _freeMemoryRegionsByByteLength.AsSpanUnsafe(0, _freeMemoryRegionsByByteLength.Count);

            var index = 0;
            var endIndex = freeMemoryRegionsByByteLength.Length;

            while (index < endIndex)
            {
                var midIndex = (index + endIndex) / 2;

                if (freeMemoryRegionsByByteLength[midIndex].ValueRef.ByteLength < size)
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
                ByteAlignment = memoryRegion.ByteAlignment,
                MemoryAllocator = memoryRegion.MemoryAllocator,
                IsAllocated = false,
                ByteOffset = memoryRegion.ByteOffset,
                ByteLength = memoryRegion.ByteLength,
            };

            // Update totals

            var freeMemoryRegionCount = ++_freeMemoryRegionCount;
            MemoryAllocatorInfo.IsEmpty = freeMemoryRegionCount == 1;

            MemoryAllocatorInfo.TotalFreeMemoryRegionByteLength += memoryRegion.ByteLength;

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
            Assert(!memoryRegionNode.ValueRef.IsAllocated);

            var nextMemoryRegionNode = memoryRegionNode.Next;

            AssertNotNull(nextMemoryRegionNode);
            Assert(!nextMemoryRegionNode.ValueRef.IsAllocated);

            ref var memoryRegion = ref memoryRegionNode.ValueRef;
            ref readonly var nextMemoryRegion = ref nextMemoryRegionNode.ValueRef;

            memoryRegion = new GraphicsMemoryRegion {
                ByteAlignment = memoryRegion.ByteAlignment,
                MemoryAllocator = memoryRegion.MemoryAllocator,
                IsAllocated = memoryRegion.IsAllocated,
                ByteOffset = memoryRegion.ByteOffset,
                ByteLength = memoryRegion.ByteLength + nextMemoryRegion.ByteLength
            };

            _memoryRegions.Remove(nextMemoryRegionNode);

            var freeMemoryRegionCount = --_freeMemoryRegionCount;
            MemoryAllocatorInfo.IsEmpty = freeMemoryRegionCount == 1;
        }

        private void RegisterFreeMemoryRegion(ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            Assert(!memoryRegionNode.ValueRef.IsAllocated);
            Assert(memoryRegionNode.ValueRef.ByteLength > 0);

            ValidateFreeMemoryRegionsBySizeList();

            if (memoryRegionNode.ValueRef.ByteLength >= MinimumFreeMemoryRegionByteLengthToRegister)
            {
                if (_freeMemoryRegionsByByteLength.Count == 0)
                {
                    _freeMemoryRegionsByByteLength.Add(memoryRegionNode);
                }
                else
                {
                    var index = BinarySearchFirstMemoryRegionNodeWithSizeNotLessThan(memoryRegionNode.ValueRef.ByteLength);
                    _freeMemoryRegionsByByteLength.Insert(index, memoryRegionNode);
                }
            }

            ValidateFreeMemoryRegionsBySizeList();
        }

        private bool TryAllocate(nuint byteLength, nuint byteAlignment, ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            Assert(byteLength > 0);
            AssertNotNull(memoryRegionNode);

            ref var memoryRegion = ref memoryRegionNode.ValueRef;
            Assert(!memoryRegion.IsAllocated);

            if (memoryRegion.ByteLength < byteLength)
            {
                return false;
            }

            // Start from an offset equal to the beginning of this memory region.
            var byteOffset = memoryRegion.ByteOffset;

            // Apply MarginSize at the beginning.
            if (MinimumAllocatedMemoryRegionMarginByteLength > 0)
            {
                byteOffset += MinimumAllocatedMemoryRegionMarginByteLength;
            }

            // Apply alignment.
            byteOffset = AlignUp(byteOffset, byteAlignment);

            // Calculate padding at the beginning based on current offset.
            var paddingBegin = byteOffset - memoryRegion.ByteOffset;

            // Calculate required margin at the end.
            var requiredEndMargin = MinimumAllocatedMemoryRegionMarginByteLength;

            // Fail if requested size plus margin before and after is bigger than size of this memory region.
            if ((paddingBegin + byteLength + requiredEndMargin) > memoryRegion.ByteLength)
            {
                return false;
            }

            var paddingEnd = memoryRegion.ByteLength - paddingBegin - byteLength;

            UnregisterFreeMemoryRegion(memoryRegionNode);

            memoryRegion = new GraphicsMemoryRegion {
                ByteAlignment = byteAlignment,
                MemoryAllocator = this,
                IsAllocated = true,
                ByteOffset = byteOffset,
                ByteLength = byteLength,
            };

            if (paddingEnd != 0)
            {
                // If there are any free bytes remaining at the end, insert a new free memory region after the current one

                var paddingMemoryRegion = new GraphicsMemoryRegion {
                    ByteAlignment = 1,
                    MemoryAllocator = this,
                    IsAllocated = false,
                    ByteOffset = byteOffset + byteLength,
                    ByteLength = paddingEnd,
                };

                var paddingEndItem = _memoryRegions.AddAfter(memoryRegionNode, paddingMemoryRegion);
                RegisterFreeMemoryRegion(paddingEndItem);
            }

            if (paddingBegin != 0)
            {
                // If there are any free bytes remaining at the beginning, insert a new free region before the current one

                var paddingMemoryRegion = new GraphicsMemoryRegion {
                    ByteAlignment = 1,
                    MemoryAllocator = this,
                    IsAllocated = false,
                    ByteOffset = byteOffset - paddingBegin,
                    ByteLength = paddingBegin,
                };

                var paddingBeginItem = _memoryRegions.AddBefore(memoryRegionNode, paddingMemoryRegion);
                RegisterFreeMemoryRegion(paddingBeginItem);
            }

            // Update totals

            var freeMemoryRegionCount = _freeMemoryRegionCount - 1;

            if (paddingBegin > 0)
            {
                freeMemoryRegionCount += 1;
            }

            if (paddingEnd > 0)
            {
                freeMemoryRegionCount += 1;
            }

            _freeMemoryRegionCount = freeMemoryRegionCount;
            MemoryAllocatorInfo.IsEmpty = freeMemoryRegionCount == 1;

            MemoryAllocatorInfo.TotalFreeMemoryRegionByteLength -= byteLength;
            return true;
        }

        private void UnregisterFreeMemoryRegion(ValueLinkedList<GraphicsMemoryRegion>.Node memoryRegionNode)
        {
            Assert(!memoryRegionNode.ValueRef.IsAllocated);
            Assert(memoryRegionNode.ValueRef.ByteLength > 0);

            ValidateFreeMemoryRegionsBySizeList();

            if (memoryRegionNode.ValueRef.ByteLength >= MinimumFreeMemoryRegionByteLengthToRegister)
            {
                for (var index = BinarySearchFirstMemoryRegionNodeWithSizeNotLessThan(memoryRegionNode.ValueRef.ByteLength); index < _freeMemoryRegionsByByteLength.Count; index++)
                {
                    if (_freeMemoryRegionsByByteLength[index] == memoryRegionNode)
                    {
                        _freeMemoryRegionsByByteLength.RemoveAt(index);
                        return;
                    }
                    Assert(_freeMemoryRegionsByByteLength[index].ValueRef.ByteLength == memoryRegionNode.ValueRef.ByteLength);
                }

                ThrowKeyNotFoundException(memoryRegionNode, nameof(_freeMemoryRegionsByByteLength));
            }

            ValidateFreeMemoryRegionsBySizeList();
        }

        [Conditional("DEBUG")]
        private void Validate()
        {
            Assert(_memoryRegions.Count != 0);

            nuint calculatedByteLength = 0;
            nuint calculatedTotalFreeRegionByteLength = 0;

            var calculatedFreeRegionCount = 0;
            var calculatedFreeRegionsToRegisterCount = 0;

            // True if previous visited memory region was free.
            var isPreviousMemoryRegionFree = false;

            for (var memoryRegionNode = _memoryRegions.First; memoryRegionNode is not null; memoryRegionNode = memoryRegionNode.Next)
            {
                ref readonly var memoryRegion = ref memoryRegionNode.ValueRef;

                // The node should immediately procede the previous
                Assert(memoryRegion.ByteOffset == calculatedByteLength);

                var isCurrentMemoryRegionFree = !memoryRegion.IsAllocated;

                // Two adjacent free memory regions are invalid, they should have been merged
                Assert(!isPreviousMemoryRegionFree || !isCurrentMemoryRegionFree);

                if (isCurrentMemoryRegionFree)
                {
                    calculatedTotalFreeRegionByteLength += memoryRegion.ByteLength;
                    ++calculatedFreeRegionCount;

                    if (memoryRegion.ByteLength >= MinimumFreeMemoryRegionByteLengthToRegister)
                    {
                        ++calculatedFreeRegionsToRegisterCount;
                    }

                    // When margins are required between allocations every free space must be at least that large
                    Assert(memoryRegion.ByteLength >= MinimumAllocatedMemoryRegionMarginByteLength);
                }
                else
                {
                    // When margins are required between allocations, the previous allocation must be free
                    Assert((MinimumAllocatedMemoryRegionMarginByteLength == 0) || isPreviousMemoryRegionFree);
                }

                calculatedByteLength += memoryRegion.ByteLength;
                isPreviousMemoryRegionFree = isCurrentMemoryRegionFree;
            }

            ValidateFreeMemoryRegionsBySizeList();

            // All totals should match the computed values
            Assert(calculatedByteLength == ByteLength);
            Assert(calculatedTotalFreeRegionByteLength == MemoryAllocatorInfo.TotalFreeMemoryRegionByteLength);
            Assert(calculatedFreeRegionCount == _freeMemoryRegionCount);
            Assert(calculatedFreeRegionsToRegisterCount == _freeMemoryRegionsByByteLength.Count);
        }

        [Conditional("DEBUG")]
        private void ValidateFreeMemoryRegionsBySizeList()
        {
            nuint lastMemoryRegionSize = 0;
            var freeMemoryRegionsBySize = _freeMemoryRegionsByByteLength.AsSpanUnsafe(0, _freeMemoryRegionsByByteLength.Count);

            for (var index = 0; index < freeMemoryRegionsBySize.Length; index++)
            {
                ref readonly var memoryRegion = ref freeMemoryRegionsBySize[index].ValueRef;

                Assert(!memoryRegion.IsAllocated);
                Assert(memoryRegion.ByteLength >= MinimumFreeMemoryRegionByteLengthToRegister);
                Assert(memoryRegion.ByteLength >= lastMemoryRegionSize);

                lastMemoryRegionSize = memoryRegion.ByteLength;
            }
        }
    }
}
