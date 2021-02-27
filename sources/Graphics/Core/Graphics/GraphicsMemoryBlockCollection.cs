// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Threading;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Represents a collection of memory blocks.</summary>
    public abstract class GraphicsMemoryBlockCollection : GraphicsDeviceObject, IReadOnlyCollection<GraphicsMemoryBlock>
    {
        private readonly GraphicsMemoryAllocator _allocator;

        private readonly List<GraphicsMemoryBlock> _blocks;
        private readonly ReaderWriterLockSlim _mutex;

        private GraphicsMemoryBlock? _emptyBlock;

        private ulong _minimumSize;
        private ulong _size;

        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBlockCollection" /> class.</summary>
        /// <param name="device">The device for which the memory block collection is being created</param>
        /// <param name="allocator">The allocator that manages the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="allocator" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="allocator" /> was not created for <paramref name="device" />.</exception>
        protected GraphicsMemoryBlockCollection(GraphicsDevice device, GraphicsMemoryAllocator allocator)
            : base(device)
        {
            ThrowIfNull(allocator, nameof(allocator));

            if (allocator.Device != device)
            {
                ThrowForInvalidParent(allocator.Device, nameof(allocator));
            }

            _allocator = allocator;

            _blocks = new List<GraphicsMemoryBlock>();
            _mutex = new ReaderWriterLockSlim();

            ref readonly var allocatorSettings = ref _allocator.Settings;

            var minimumBlockCount = allocatorSettings.MinimumBlockCountPerCollection;
            var maximumSharedBlockSize = allocatorSettings.MaximumSharedBlockSize.GetValueOrDefault();

            for (var i = 0; i < minimumBlockCount; ++i)
            {
                var blockSize = GetAdjustedBlockSize(maximumSharedBlockSize);
                _ = AddBlock(blockSize);
            }

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Gets the allocator that manages the collection.</summary>
        public GraphicsMemoryAllocator Allocator => _allocator;

        /// <summary>Gets the number of blocks in the collection.</summary>
        public int Count => _blocks.Count;

        /// <summary>Gets <c>true</c> if the block collection is empty; otherwise, <c>false</c>.</summary>
        public bool IsEmpty
        {
            get
            {
                using var mutex = new DisposableReaderLockSlim(_mutex, _allocator.IsExternallySynchronized);
                return _blocks.Count == 0;
            }
        }

        /// <summary>Gets the maximum number of blocks allowed in the collection.</summary>
        public int MaximumBlockCount => _allocator.Settings.MaximumBlockCountPerCollection;

        /// <summary>Gets the maximum size of any new shared memory blocks created for the collection, in bytes.</summary>
        public ulong MaximumSharedBlockSize => _allocator.Settings.MaximumSharedBlockSize.GetValueOrDefault();

        /// <summary>Gets the minimum number of blocks allowed in the collection.</summary>
        public int MinimumBlockCount => _allocator.Settings.MinimumBlockCountPerCollection;

        /// <summary>Gets the minimum size of any new memory blocks created for the collection, in bytes.</summary>
        public ulong MinimumBlockSize => _allocator.Settings.MinimumBlockSize;

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
        public GraphicsMemoryRegion<GraphicsMemoryBlock> Allocate(ulong size, ulong alignment = 1, GraphicsMemoryRegionAllocationFlags flags = GraphicsMemoryRegionAllocationFlags.None)
        {
            var succeeded = TryAllocate(size, alignment, flags, out var region);

            if (!succeeded)
            {
                ThrowOutOfMemoryException(size);
            }
            return region;
        }

        /// <summary>Frees a region of memory from the collection.</summary>
        /// <param name="region">The region to be freed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="region" />.<see cref="GraphicsMemoryRegion{GraphicsMemoryBlock}.Collection" /> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException"><paramref name="region" /> was not found in the collection.</exception>
        public void Free(in GraphicsMemoryRegion<GraphicsMemoryBlock> region)
        {
            var block = region.Collection;
            ThrowIfNull(block, nameof(region));

            using var mutex = new DisposableWriterLockSlim(_mutex, _allocator.IsExternallySynchronized);

            var blocks = _blocks;
            var blockIndex = blocks.IndexOf(block);

            if (blockIndex == -1)
            {
                ThrowKeyNotFoundException(region, nameof(blocks));
            }

            block.Free(in region);

            if (block.IsEmpty)
            {
                var emptyBlock = _emptyBlock;
                var blocksCount = blocks.Count;

                if (emptyBlock is not null)
                {
                    if (blocksCount > MinimumBlockCount)
                    {
                        var size = _size;
                        var minimumSize = _minimumSize;

                        // We have two empty blocks, we want to prefer removing the larger of the two

                        if (block.Size > emptyBlock.Size)
                        {
                            if ((size - block.Size) >= minimumSize)
                            {
                                RemoveBlockAt(blockIndex);
                            }
                            else if ((size - emptyBlock.Size) >= minimumSize)
                            {
                                RemoveBlock(emptyBlock);
                            }
                        }
                        else if ((size - emptyBlock.Size) >= minimumSize)
                        {
                            RemoveBlockAt(blockIndex);
                        }
                        else if ((size - block.Size) >= minimumSize)
                        {
                            RemoveBlock(emptyBlock);
                        }
                        else
                        {
                            // Removing either would put us below the minimum size, so we can't remove
                        }
                    }
                    else if (block.Size > emptyBlock.Size)
                    {
                        // We can't remove the block, so set empty block to the larger
                        _emptyBlock = block;
                    }
                }
                else
                {
                    // We have no existing empty blocks, so we want to set the index to this block unless
                    // we are currently exceeding our memory budget, in which case we want to free the block
                    // instead. However, we still need to maintain the minimum block count and minimum size
                    // placed on the collection, so we will only respect the budget if those can be maintained.

                    _allocator.GetBudget(this, out var budget);

                    if ((budget.EstimatedUsage >= budget.EstimatedBudget) && (blocksCount > MinimumBlockCount) && ((_size - block.Size) >= _minimumSize))
                    {
                        RemoveBlockAt(blockIndex);
                    }
                    else
                    {
                        _emptyBlock = block;
                    }
                }
            }

            IncrementallySortBlocks();
        }

        /// <summary>Gets an enumerator that can be used to iterate through the blocks of the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the blocks of the collection.</returns>
        public IEnumerator<GraphicsMemoryBlock> GetEnumerator() => _blocks.GetEnumerator();

        /// <summary>Tries to allocation a region of memory in the collection.</summary>
        /// <param name="size">The size of the region to allocate, in bytes.</param>
        /// <param name="alignment">The alignment of the region to allocate, in bytes.</param>
        /// <param name="flags">The flags that modify how the region is allocated.</param>
        /// <param name="region">On return, contains the allocated region or <c>default</c> if the allocation failed.</param>
        /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="flags" /> has an invalid combination.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        public bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, [Optional, DefaultParameterValue(GraphicsMemoryRegionAllocationFlags.None)] GraphicsMemoryRegionAllocationFlags flags, out GraphicsMemoryRegion<GraphicsMemoryBlock> region)
        {
            using var mutex = new DisposableWriterLockSlim(_mutex, _allocator.IsExternallySynchronized);
            return TryAllocateRegion(size, alignment, flags, out region);
        }

        /// <summary>Tries to allocate a set of memory regions in the collection.</summary>
        /// <param name="size">The size of the regions to allocate, in bytes.</param>
        /// <param name="alignment">The alignment of the regions to allocate, in bytes.</param>
        /// <param name="flags">The flags that modify how the regions are allocated.</param>
        /// <param name="regions">On return, will be filled with the allocated regions.</param>
        /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="flags" /> has an invalid combination.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        public bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, [Optional, DefaultParameterValue(GraphicsMemoryRegionAllocationFlags.None)] GraphicsMemoryRegionAllocationFlags flags, Span<GraphicsMemoryRegion<GraphicsMemoryBlock>> regions)
        {
            var succeeded = true;
            nuint index;

            using (var mutex = new DisposableWriterLockSlim(_mutex, _allocator.IsExternallySynchronized))
            {
                for (index = 0; index < (nuint)regions.Length; ++index)
                {
                    succeeded = TryAllocateRegion(size, alignment, flags, out regions[(int)index]);

                    if (!succeeded)
                    {
                        break;
                    }
                }
            }

            if (!succeeded)
            {
                // Something failed so free all already allocated regions

                while (index-- != 0)
                {
                    Free(in regions[(int)index]);
                    regions[(int)index] = default;
                }
            }

            return succeeded;
        }

        /// <summary>Tries to set the minimum size of the collection, in bytes.</summary>
        /// <param name="minimumSize">The minimum size of the collection, in bytes.</param>
        /// <returns><c>true</c> if the minimum size was succesfully set; otherwise, <c>false</c>.</returns>
        public bool TrySetMinimumSize(ulong minimumSize)
        {
            using var mutex = new DisposableWriterLockSlim(_mutex, _allocator.IsExternallySynchronized);

            var currentMinimumSize = _minimumSize;

            if (minimumSize == currentMinimumSize)
            {
                return true;
            }

            var size = _size;
            var blockCount = _blocks.Count;

            if (minimumSize < currentMinimumSize)
            {
                // The new minimum size is less than the previous, so we will iterate the
                // blocks from last to first (largest to smallest) to try and free space
                // that may now be available.

                var emptyBlock = default(GraphicsMemoryBlock);
                var minimumBlockCount = MinimumBlockCount;

                for (var blockIndex = blockCount; blockIndex-- != 0;)
                {
                    var block = _blocks[blockIndex];

                    var blockSize = block.Size;
                    var blockIsEmpty = block.IsEmpty;

                    if (blockIsEmpty)
                    {
                        if (((size - blockSize) >= minimumSize) && ((blockCount - 1) >= minimumBlockCount))
                        {
                            RemoveBlockAt(blockIndex);
                            size -= blockSize;
                            --blockCount;
                        }
                        else
                        {
                            emptyBlock ??= block;
                        }
                    }
                }

                _emptyBlock = emptyBlock;
            }
            else
            {
                // The new minimum size is greater than the previous, so we will allocate
                // new blocks until we exceed the minimum size, but ensuring we don't exceed
                // preferredBlockSize while doing so.

                var emptyBlock = default(GraphicsMemoryBlock);
                var maximumBlockCount = MaximumBlockCount;
                var maximumSharedBlockSize = MaximumSharedBlockSize;
                var minimumBlockSize = MinimumBlockSize;

                while (size < minimumSize)
                {
                    if (blockCount < maximumBlockCount)
                    {
                        var blockSize = GetAdjustedBlockSize(maximumSharedBlockSize);

                        if (((size + blockSize) > minimumSize) && (blockSize != minimumBlockSize))
                        {
                            // The current size plus the new block will exceed the minimum
                            // size requested, so adjust it to be just large enough.
                            blockSize = minimumSize - size;
                        }

                        emptyBlock ??= AddBlock(blockSize);
                        size += blockSize;

                        ++blockCount;
                    }
                    else
                    {
                        _emptyBlock ??= emptyBlock;
                        return false;
                    }
                }

                _emptyBlock ??= emptyBlock;
            }

            _minimumSize = minimumSize;
            Assert(AssertionsEnabled && (_size == size));

            return true;
        }

        /// <summary>Adds a new block to the collection.</summary>
        /// <param name="size">The size of the block, in bytes.</param>
        /// <returns>The created graphics memory block.</returns>
        protected abstract GraphicsMemoryBlock CreateBlock(ulong size);

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                foreach (var block in _blocks)
                {
                    block?.Dispose();
                }

                _mutex?.Dispose();
            }

            _state.EndDispose();
        }

        private GraphicsMemoryBlock AddBlock(ulong size)
        {
            var block = CreateBlock(size);

            _blocks.Add(block);
            _size += size;

            return block;
        }

        private ulong GetAdjustedBlockSize(ulong size)
        {
            var maximumSharedBlockSize = MaximumSharedBlockSize;
            var blockSize = size;

            if (blockSize < maximumSharedBlockSize)
            {
                var minimumBlockSize = MinimumBlockSize;
                blockSize = maximumSharedBlockSize;

                if (minimumBlockSize != maximumSharedBlockSize)
                {
                    // Allocate 1/8, 1/4, 1/2 as first blocks, ensuring we don't go smaller than the minimum
                    var largestBlockSize = GetLargestSharedBlockSize();

                    for (var i = 0; i < 3; ++i)
                    {
                        var smallerBlockSize = blockSize / 2;

                        if ((smallerBlockSize > largestBlockSize) && (smallerBlockSize >= (size * 2)))
                        {
                            blockSize = Math.Max(smallerBlockSize, minimumBlockSize);
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
                blockSize = size;
            }

            return blockSize;
        }

        private ulong GetLargestSharedBlockSize()
        {
            var result = 0UL;

            var blocks = CollectionsMarshal.AsSpan(_blocks);
            var maximumSharedBlockSize = MaximumSharedBlockSize;

            for (var i = blocks.Length; i-- != 0;)
            {
                var blockSize = blocks[i].Size;

                if (blockSize < maximumSharedBlockSize)
                {
                    result = Math.Max(result, blockSize);
                }
                else if (blockSize == maximumSharedBlockSize)
                {
                    result = maximumSharedBlockSize;
                    break;
                }
            }

            return result;
        }

        private void IncrementallySortBlocks()
        {
            // Bubble sort only until first swap. This is called after
            // freeing a region and will result in eventual consistency

            var blocks = CollectionsMarshal.AsSpan(_blocks);

            if (blocks.Length >= 2)
            {
                var previousBlock = blocks[0];

                for (var i = 1; i < blocks.Length; ++i)
                {
                    var block = blocks[i];

                    if (previousBlock.TotalFreeRegionSize <= block.TotalFreeRegionSize)
                    {
                        previousBlock = block;
                    }
                    else
                    {
                        blocks[i - 1] = block;
                        blocks[i] = previousBlock;

                        return;
                    }
                }
            }
        }

        private void RemoveBlock(GraphicsMemoryBlock block)
        {
            var blockIndex = _blocks.IndexOf(block);
            RemoveBlockAt(blockIndex);
        }

        private void RemoveBlockAt(int index)
        {
            var block = _blocks[index];
            _blocks.RemoveAt(index);
            _size -= block.Size;
        }

        private bool TryAllocateRegion(ulong size, ulong alignment, GraphicsMemoryRegionAllocationFlags flags, out GraphicsMemoryRegion<GraphicsMemoryBlock> region)
        {
            var useDedicatedBlock = flags.HasFlag(GraphicsMemoryRegionAllocationFlags.DedicatedCollection);
            var useExistingBlock = flags.HasFlag(GraphicsMemoryRegionAllocationFlags.ExistingCollection);

            if (useDedicatedBlock && useExistingBlock)
            {
                ThrowForInvalidFlagsCombination(flags, nameof(flags));
            }

            _allocator.GetBudget(this, out var budget);

            var maximumSharedBlockSize = MaximumSharedBlockSize;
            var sizeWithMargins = size + (2 * _allocator.Settings.MinimumAllocatedRegionMarginSize.GetValueOrDefault());

            var blocks = CollectionsMarshal.AsSpan(_blocks);
            var blocksLength = blocks.Length;

            var availableMemory = (budget.EstimatedUsage < budget.EstimatedBudget) ? (budget.EstimatedBudget - budget.EstimatedUsage) : 0;
            var canCreateNewBlock = !useExistingBlock && (blocksLength < MaximumBlockCount) && (availableMemory >= sizeWithMargins);

            // 1. Search existing blocks

            if (!useDedicatedBlock && (size <= maximumSharedBlockSize))
            {
                for (var blockIndex = 0; blockIndex < blocksLength; ++blockIndex)
                {
                    var currentBlock = blocks[blockIndex];
                    AssertNotNull(currentBlock);

                    if (currentBlock.TryAllocate(size, alignment, out region))
                    {
                        if (currentBlock == _emptyBlock)
                        {
                            _emptyBlock = null;
                        }
                        return true;
                    }
                }
            }

            // 2. Try to create a new block

            if (!canCreateNewBlock)
            {
                region = default;
                return false;
            }

            var blockSize = GetAdjustedBlockSize(sizeWithMargins);

            if (blockSize >= availableMemory)
            {
                region = default;
                return false;
            }

            var block = AddBlock(blockSize);
            return block.TryAllocate(size, alignment, out region);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
