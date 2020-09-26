// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockMetadata_Generic class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockMetadata_Generic class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.IntegerUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    public abstract partial class GraphicsMemoryBlock
    {
        /// <summary>The default metadata for a single block of memory which can contain allocated or free regions.</summary>
        public struct DefaultMetadata : IMetadata
        {
            private ulong _size;
            private ulong _marginSize;
            private ulong _minimumFreeRegionSizeToRegister;
            private ulong _totalFreeRegionSize;
            private GraphicsMemoryBlock _block;
            private List<LinkedListNode<GraphicsMemoryBlockRegion>> _freeRegionsBySize;
            private LinkedList<GraphicsMemoryBlockRegion> _regions;
            private uint _freeRegionCount;

            /// <inheritdoc />
            public nuint AllocatedRegionCount => ((nuint)(uint)_regions.Count) - _freeRegionCount;

            /// <inheritdoc />
            public GraphicsMemoryBlock Block => _block;

            /// <inheritdoc />
            public bool IsEmpty => (_regions.Count == 1) && (_freeRegionCount == 1);

            /// <inheritdoc />
            public ulong LargestFreeRegionSize => (_freeRegionsBySize.Count != 0) ? _freeRegionsBySize[^1].ValueRef.Size : 0;

            /// <inheritdoc />
            public ulong MarginSize => _marginSize;

            /// <inheritdoc />
            public ulong MinimumFreeRegionSizeToRegister => _minimumFreeRegionSizeToRegister;

            /// <inheritdoc />
            public ulong Size => _size;

            /// <inheritdoc />
            public ulong TotalFreeRegionSize => _totalFreeRegionSize;

            /// <inheritdoc />
            public void Clear()
            {
                _freeRegionCount = 1;
                _totalFreeRegionSize = Size;
                _regions.Clear();

                var region = new GraphicsMemoryBlockRegion(
                    Block,
                    Size,
                    offset: 0,
                    alignment: 1,
                    GraphicsMemoryBlockRegionKind.Free
                );
                var regionNode = _regions.AddFirst(region);

                _freeRegionsBySize.Clear();
                _freeRegionsBySize.Add(regionNode);
            }

            /// <inheritdoc />
            public void Free(in GraphicsMemoryBlockRegion region)
            {
                for (var regionNode = _regions.First; regionNode is not null; regionNode = regionNode.Next)
                {
                    if (regionNode.ValueRef != region)
                    {
                        continue;
                    }

                    _ = FreeRegion(regionNode);
                    return;
                }

                ThrowKeyNotFoundException(nameof(region));
            }

            /// <inheritdoc />
            public void Initialize(GraphicsMemoryBlock block, ulong size, ulong marginSize, ulong minimumFreeRegionSizeToRegister)
            {
                ThrowIfNull(block, nameof(block));
                ThrowIfZero(size, nameof(size));

                _size = size;
                _marginSize = marginSize;
                _minimumFreeRegionSizeToRegister = minimumFreeRegionSizeToRegister;
                _block = block;
                _freeRegionsBySize = new List<LinkedListNode<GraphicsMemoryBlockRegion>>();
                _regions = new LinkedList<GraphicsMemoryBlockRegion>();

                Clear();
            }

            /// <inheritdoc />
            public bool TryAllocate(ulong size, ulong alignment, out GraphicsMemoryBlockRegion region)
            {
                ThrowIfZero(size, nameof(size));
                ThrowIfNotPow2(alignment, nameof(alignment));

                Validate();

                var allocationSizeWithMargins = size + (2 * MarginSize);

                if (TotalFreeRegionSize < allocationSizeWithMargins)
                {
                    region = default;
                    return false;
                }

                var freeRegionsCount = (nuint)_freeRegionsBySize.Count;

                if (freeRegionsCount > 0)
                {
                    for (var index = BinarySearchFirstRegionNodeWithSizeNotLessThan(allocationSizeWithMargins); index < freeRegionsCount; ++index)
                    {
                        var regionNode = _freeRegionsBySize[(int)index];

                        if (TryAllocate(size, alignment, regionNode))
                        {
                            region = regionNode.ValueRef;
                            return true;
                        }
                    }
                }

                region = default;
                return false;
            }

            /// <inheritdoc />
            public void Validate()
            {
                Assert(_regions.Count != 0);

                ulong calculatedSize = 0;
                ulong calculatedTotalFreeRegionSize = 0;

                nuint calculatedFreeRegionCount = 0;
                nuint calculatedFreeRegionsToRegisterCount = 0;

                // True if previous visited region was free.
                var isPreviousRegionFree = false;

                for (var regionNode = _regions.First; regionNode is not null; regionNode = regionNode.Next)
                {
                    ref readonly GraphicsMemoryBlockRegion region = ref regionNode.ValueRef;

                    // The node should immediately procede the previous
                    Assert(region.Offset == calculatedSize);

                    var isCurrentRegionFree = (region.Kind == GraphicsMemoryBlockRegionKind.Free);

                    // Two adjacent free regions are invalid, they should have been merged
                    Assert(!isPreviousRegionFree || !isCurrentRegionFree);

                    if (isCurrentRegionFree)
                    {
                        calculatedTotalFreeRegionSize += region.Size;
                        ++calculatedFreeRegionCount;

                        if (region.Size >= MinimumFreeRegionSizeToRegister)
                        {
                            ++calculatedFreeRegionsToRegisterCount;
                        }

                        // When margins are required between allocations every free space must be at least that large
                        Assert(region.Size >= MarginSize);
                    }
                    else
                    {
                        // When margins are required between allocations, the previous allocation must be free
                        Assert((MarginSize == 0) || isPreviousRegionFree);
                    }

                    calculatedSize += region.Size;
                    isPreviousRegionFree = isCurrentRegionFree;
                }

                ValidateFreeRegionsBySizeList();

                // All totals should match the computed values
                Assert(calculatedSize == Size);
                Assert(calculatedTotalFreeRegionSize == _totalFreeRegionSize);
                Assert(calculatedFreeRegionCount == _freeRegionCount);
                Assert(calculatedFreeRegionsToRegisterCount == (nuint)_freeRegionsBySize.Count);
            }

            private nuint BinarySearchFirstRegionNodeWithSizeNotLessThan(ulong size)
            {
                var freeRegionsBySize = CollectionsMarshal.AsSpan(_freeRegionsBySize);

                nuint index = 0;
                nuint endIndex = (nuint)freeRegionsBySize.Length;

                while (index < endIndex)
                {
                    nuint midIndex = (index + endIndex) / 2;

                    if (freeRegionsBySize[(int)midIndex].ValueRef.Size < size)
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

            private LinkedListNode<GraphicsMemoryBlockRegion> FreeRegion(LinkedListNode<GraphicsMemoryBlockRegion> regionNode)
            {
                ref GraphicsMemoryBlockRegion region = ref regionNode.ValueRef;

                if (region.Kind == GraphicsMemoryBlockRegionKind.Free)
                {
                    return regionNode;
                }

                region = region.WithKind(GraphicsMemoryBlockRegionKind.Free);

                // Update totals
                ++_freeRegionCount;
                _totalFreeRegionSize += region.Size;

                // Merge with previous and/or next region if it's also free
                var mergeWithNext = false;
                var mergeWithPrev = false;

                var nextRegionNode = regionNode.Next;

                if ((nextRegionNode is not null) && (nextRegionNode.ValueRef.Kind == GraphicsMemoryBlockRegionKind.Free))
                {
                    mergeWithNext = true;
                }

                var prevRegionNode = regionNode.Previous;

                if ((prevRegionNode is not null) && (prevRegionNode.ValueRef.Kind == GraphicsMemoryBlockRegionKind.Free))
                {
                    mergeWithPrev = true;
                }

                if (mergeWithNext)
                {
                    AssertNotNull(nextRegionNode, nameof(nextRegionNode));
                    UnregisterFreeRegion(nextRegionNode);
                    MergeFreeRegionWithNext(regionNode);
                }

                if (mergeWithPrev)
                {
                    AssertNotNull(prevRegionNode, nameof(prevRegionNode));
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

            private void MergeFreeRegionWithNext(LinkedListNode<GraphicsMemoryBlockRegion> regionNode)
            {
                AssertNotNull(regionNode, nameof(regionNode));
                Assert(regionNode.ValueRef.Kind == GraphicsMemoryBlockRegionKind.Free);

                var nextRegionNode = regionNode.Next;

                AssertNotNull(nextRegionNode, nameof(nextRegionNode));
                Assert(nextRegionNode.ValueRef.Kind == GraphicsMemoryBlockRegionKind.Free);

                ref GraphicsMemoryBlockRegion region = ref regionNode.ValueRef;
                ref readonly GraphicsMemoryBlockRegion nextRegion = ref nextRegionNode.ValueRef;

                region = region.WithSize(region.Size + nextRegion.Size);
                ;
                --_freeRegionCount;

                _regions.Remove(nextRegionNode);
            }

            private void RegisterFreeRegion(LinkedListNode<GraphicsMemoryBlockRegion> regionNode)
            {
                Assert(regionNode.ValueRef.Kind == GraphicsMemoryBlockRegionKind.Free);
                Assert(regionNode.ValueRef.Size > 0);

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
                        _freeRegionsBySize.Insert((int)index, regionNode);
                    }
                }

                ValidateFreeRegionsBySizeList();
            }

            private bool TryAllocate(ulong size, ulong alignment, LinkedListNode<GraphicsMemoryBlockRegion> regionNode)
            {
                Assert(size > 0);
                AssertNotNull(regionNode, nameof(regionNode));

                ref GraphicsMemoryBlockRegion region = ref regionNode.ValueRef;
                Assert(region.Kind == GraphicsMemoryBlockRegionKind.Free);

                if (region.Size < size)
                {
                    return false;
                }

                // Start from an offset equal to the beginning of this region.
                var offset = region.Offset;

                // Apply MarginSize at the beginning.
                if (MarginSize > 0)
                {
                    offset += MarginSize;
                }

                // Apply alignment.
                offset = AlignUp(offset, alignment);

                // Calculate padding at the beginning based on current offset.
                ulong paddingBegin = offset - region.Offset;

                // Calculate required margin at the end.
                ulong requiredEndMargin = MarginSize;

                // Fail if requested size plus margin before and after is bigger than size of this region.
                if ((paddingBegin + size + requiredEndMargin) > region.Size)
                {
                    return false;
                }

                ulong paddingEnd = region.Size - paddingBegin - size;

                UnregisterFreeRegion(regionNode);

                region = new GraphicsMemoryBlockRegion(
                    Block,
                    size,
                    offset,
                    alignment,
                    GraphicsMemoryBlockRegionKind.Allocated
                );

                if (paddingEnd != 0)
                {
                    // If there are any free bytes remaining at the end, insert a new free region after the current one

                    var paddingRegion = new GraphicsMemoryBlockRegion(
                        Block,
                        paddingEnd,
                        offset + size,
                        alignment: 1,
                        GraphicsMemoryBlockRegionKind.Free
                    );

                    var paddingEndItem = _regions.AddAfter(regionNode, paddingRegion);
                    RegisterFreeRegion(paddingEndItem);
                }

                if (paddingBegin != 0)
                {
                    // If there are any free bytes remaining at the beginning, insert a new free region before the current one

                    var paddingRegion = new GraphicsMemoryBlockRegion(
                        Block,
                        paddingBegin,
                        offset - paddingBegin,
                        alignment: 1,
                        GraphicsMemoryBlockRegionKind.Free
                    );

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

            private void UnregisterFreeRegion(LinkedListNode<GraphicsMemoryBlockRegion> regionNode)
            {
                Assert(regionNode.ValueRef.Kind == GraphicsMemoryBlockRegionKind.Free);
                Assert(regionNode.ValueRef.Size > 0);

                ValidateFreeRegionsBySizeList();

                if (regionNode.ValueRef.Size >= MinimumFreeRegionSizeToRegister)
                {
                    for (nuint index = BinarySearchFirstRegionNodeWithSizeNotLessThan(regionNode.ValueRef.Size); index < (nuint)_freeRegionsBySize.Count; ++index)
                    {
                        if (_freeRegionsBySize[(int)index] == regionNode)
                        {
                            _freeRegionsBySize.RemoveAt((int)index);
                            return;
                        }
                        Assert(_freeRegionsBySize[(int)index].ValueRef.Size == regionNode.ValueRef.Size);
                    }

                    ThrowKeyNotFoundException(nameof(regionNode));
                }

                ValidateFreeRegionsBySizeList();
            }

            [Conditional("DEBUG")]
            private void ValidateFreeRegionsBySizeList()
            {
                ulong lastRegionSize = 0;

                for (nuint i = 0, count = (nuint)_freeRegionsBySize.Count; i < count; ++i)
                {
                    ref readonly GraphicsMemoryBlockRegion region = ref _freeRegionsBySize[(int)i].ValueRef;

                    Assert(region.Kind == GraphicsMemoryBlockRegionKind.Free);
                    Assert(region.Size >= MinimumFreeRegionSizeToRegister);
                    Assert(region.Size >= lastRegionSize);

                    lastRegionSize = region.Size;
                }
            }
        }
    }
}
