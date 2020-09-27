// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics
{
    /// <summary>Represents a collection of memory blocks.</summary>
    public abstract class GraphicsMemoryBlockCollection : IDisposable
    {
        private readonly ulong _blockMarginSize;
        private readonly ulong _blockMinimumSize;
        private readonly ulong _blockMinimumFreeRegionSizeToRegister;
        private readonly ulong _blockPreferredSize;
        private readonly GraphicsMemoryAllocator _allocator;
        private readonly List<GraphicsMemoryBlock> _blocks;
        private readonly ReaderWriterLockSlim _mutex;
        private readonly nuint _maximumBlockCount;
        private readonly nuint _minimumBlockCount;

        private ulong _minimumSize;
        private GraphicsMemoryBlock? _emptyBlock;
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBlockCollection" /> class.</summary>
        /// <param name="blockMinimumSize">The minimum size of any new memory blocks created for the collection, in bytes.</param>
        /// <param name="blockPreferredSize">The preferred size of any new memory blocks created for the collection, in bytes.</param>
        /// <param name="blockMarginSize">The minimum size of free regions to keep on either side of an allocated region, in bytes.</param>
        /// <param name="blockMinimumFreeRegionSizeToRegister">The minimum size of a free region for it to be registered as available, in bytes.</param>
        /// <param name="allocator">The allocator that manages the collection.</param>
        /// <param name="minimumBlockCount">The minimum number of blocks allowed in the collection.</param>
        /// <param name="maximumBlockCount">The maximum number of blocks allowed in the collection.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="blockMinimumSize" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="blockPreferredSize" /> is less than <paramref name="blockMinimumSize"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="allocator" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maximumBlockCount" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="blockMinimumSize"/> is greater than <paramref name="maximumBlockCount"/>.</exception>
        protected GraphicsMemoryBlockCollection(ulong blockMinimumSize, ulong blockPreferredSize, ulong blockMarginSize, ulong blockMinimumFreeRegionSizeToRegister, GraphicsMemoryAllocator allocator, nuint minimumBlockCount, nuint maximumBlockCount)
        {
            ThrowIfZero(blockMinimumSize, nameof(blockMinimumSize));

            if (blockPreferredSize < blockMinimumSize)
            {
                ThrowArgumentOutOfRangeException(nameof(blockPreferredSize), blockPreferredSize);
            }

            ThrowIfNull(allocator, nameof(allocator));

            ThrowIfZero(maximumBlockCount, nameof(maximumBlockCount));

            if (minimumBlockCount > maximumBlockCount)
            {
                ThrowArgumentOutOfRangeException(nameof(minimumBlockCount), minimumBlockCount);
            }

            _blockMarginSize = blockMarginSize;
            _blockMinimumSize = blockMinimumSize;
            _blockMinimumFreeRegionSizeToRegister = blockMinimumFreeRegionSizeToRegister;
            _blockPreferredSize = blockPreferredSize;
            _allocator = allocator;
            _blocks = new List<GraphicsMemoryBlock>();
            _mutex = new ReaderWriterLockSlim();
            _maximumBlockCount = maximumBlockCount;
            _minimumBlockCount = minimumBlockCount;

            for (nuint i = 0; i < _minimumBlockCount; ++i)
            {
                var block = CreateBlock(blockMinimumSize);
                _blocks.Add(block);
            }

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsMemoryBlockCollection" /> class.</summary>
        ~GraphicsMemoryBlockCollection() => Dispose(isDisposing: true);

        /// <summary>Gets the allocator that manages the collection.</summary>
        public GraphicsMemoryAllocator Allocator => _allocator;

        /// <summary>Gets the minimum size of free regions to keep on either side of an allocated region, in bytes.</summary>
        public ulong BlockMarginSize => _blockMarginSize;

        /// <summary>Gets the minimum size of any new memory blocks created for the collection, in bytes.</summary>
        public ulong BlockMinimumSize => _blockMinimumSize;

        /// <summary>Gets the minimum size of a free region for it to be registered as available, in bytes.</summary>
        public ulong BlockMinimumFreeRegionSizeToRegister => _blockMinimumFreeRegionSizeToRegister;

        /// <summary>Gets the preferred size of any new memory blocks created for the collection, in bytes.</summary>
        public ulong BlockPreferredSize => _blockPreferredSize;

        /// <summary>Gets <c>true</c> if the block collection is empty; otherwise, <c>false</c>.</summary>
        public bool IsEmpty
        {
            get
            {
                using var mutex = new ReaderLockSlim(_mutex, _allocator.IsExternallySynchronized);
                return _blocks.Count == 0;
            }
        }

        /// <summary>Gets the minimum size of the collection, in bytes.</summary>
        public ulong MinimumSize => _minimumSize;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Frees a region from the collection.</summary>
        /// <param name="region">The region to be freed.</param>
        /// <exception cref="KeyNotFoundException"><paramref name="region" /> was not found in the collection.</exception>
        public void Free(in GraphicsMemoryBlockRegion region)
        {
            _allocator.GetBudget(this, out GraphicsMemoryBudget budget);
            var isBudgetExceeded = budget.EstimatedUsage >= budget.EstimatedBudget;

            using var mutex = new WriterLockSlim(_mutex, _allocator.IsExternallySynchronized);
            var block = region.Block;

            if (!_blocks.Contains(block))
            {
                ThrowKeyNotFoundException(nameof(region));
            }

            block.Free(in region);
            block.Validate();

            nuint blockCount = (nuint)_blocks.Count;
            ulong totalBlockSize = GetTotalBlockSize();

            if (block.IsEmpty)
            {
                if (((_emptyBlock is not null) || isBudgetExceeded) && (blockCount > _minimumBlockCount) && ((totalBlockSize - block.Size) >= _minimumSize))
                {
                    _blocks.Remove(block);
                }
                else
                {
                    _emptyBlock = block;
                }
            }
            else if ((_emptyBlock is not null) && (blockCount > _minimumBlockCount))
            {
                var lastBlock = _blocks[^1];

                if (lastBlock.IsEmpty && ((totalBlockSize - lastBlock.Size) >= _minimumSize))
                {
                    _blocks.RemoveAt(_blocks.Count - 1);
                    _emptyBlock = null;
                }
            }

            IncrementallySortBlocks();
        }

        /// <summary>Tries to allocation a region of memory in the collection.</summary>
        /// <param name="size">The size of the region to allocate.</param>
        /// <param name="alignment">The alignment of the region to allocate.</param>
        /// <param name="flags">The flags that modify how the region is allocated.</param>
        /// <param name="region">On return, contains the allocated region or <c>default</c> if the allocation failed.</param>
        /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="flags" /> has an invalid combination.</exception>
        public bool TryAllocate(ulong size, ulong alignment, GraphicsMemoryAllocationFlags flags, out GraphicsMemoryBlockRegion region)
        {
            using var mutex = new WriterLockSlim(_mutex, _allocator.IsExternallySynchronized);
            return TryAllocateRegion(size, alignment, flags, out region);
        }

        /// <summary>Tries to allocation a set of memory regions in the collection.</summary>
        /// <param name="size">The size of the regions to allocate.</param>
        /// <param name="alignment">The alignment of the regions to allocate.</param>
        /// <param name="flags">The flags that modify how the regions are allocated.</param>
        /// <param name="regions">On return, will be filled with the allocated regions.</param>
        /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="flags" /> has an invalid combination.</exception>
        public bool TryAllocate(ulong size, ulong alignment, GraphicsMemoryAllocationFlags flags, Span<GraphicsMemoryBlockRegion> regions)
        {
            var succeeded = true;
            nuint index;

            using var mutex = new WriterLockSlim(_mutex, _allocator.IsExternallySynchronized);
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
        /// <param name="value">The minimum size of the collection, in bytes.</param>
        /// <returns><c>true</c> if the minimum size was succesfully set; otherwise, <c>false</c>.</returns>
        public bool TrySetMinimumSize(ulong value)
        {
            using var mutex = new WriterLockSlim(_mutex, _allocator.IsExternallySynchronized);

            if (value == _minimumSize)
            {
                return true;
            }

            ulong totalBlockSize = GetTotalBlockSize();
            nuint blockCount = (nuint)_blocks.Count;

            if (value < _minimumSize)
            {
                _emptyBlock = null;

                for (nuint index = blockCount; index-- != 0;)
                {
                    var block = _blocks[(int)index];

                    ulong size = block.Size;
                    bool isEmpty = block.IsEmpty;

                    if (isEmpty && ((totalBlockSize - size) >= value) && ((blockCount - 1) >= _minimumBlockCount))
                    {
                        _blocks.RemoveAt((int)index);
                        totalBlockSize -= size;
                        --blockCount;
                    }
                    else if (isEmpty)
                    {
                        _emptyBlock ??= block;
                    }
                }
            }
            else
            {
                ulong minimumBlockSize = _blockMinimumSize;

                while (totalBlockSize < value)
                {
                    if (blockCount < _maximumBlockCount)
                    {
                        ulong blockSize = _blockPreferredSize;

                        if (blockSize != _blockMinimumSize)
                        {
                            if ((totalBlockSize + blockSize) > value)
                            {
                                blockSize = value - totalBlockSize;
                            }
                            else if (((blockCount + 1) < _maximumBlockCount) && ((totalBlockSize + blockSize + minimumBlockSize) > value))
                            {
                                blockSize -= (minimumBlockSize + totalBlockSize + _blockPreferredSize - value);
                            }
                        }

                        var block = CreateBlock(blockSize);
                        _blocks.Add(block);

                        _emptyBlock ??= block;
                        totalBlockSize += blockSize;
                        ++blockCount;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            _minimumSize = value;
            return true;
        }

        /// <summary>Creates a new block for the collection.</summary>
        /// <param name="size">The size of the block, in bytes.</param>
        /// <returns>The created graphics memory block.</returns>
        protected abstract GraphicsMemoryBlock CreateBlock(ulong size);

        /// <inheritdoc />
        protected virtual void Dispose(bool isDisposing)
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

        private ulong GetLargestBlockSize()
        {
            ulong result = 0;

            for (nuint i = (nuint)_blocks.Count; unchecked(i--) != 0;)
            {
                result = Math.Max(result, _blocks[(int)i].Size);

                if (result >= BlockPreferredSize)
                {
                    break;
                }
            }

            return result;
        }

        private ulong GetTotalBlockSize()
        {
            ulong result = 0;

            for (nuint i = (nuint)_blocks.Count; i-- != 0;)
            {
                result += _blocks[(int)i].Size;
            }

            return result;
        }

        private void IncrementallySortBlocks()
        {
            // Bubble sort only until first swap
            var blocks = CollectionsMarshal.AsSpan(_blocks);

            for (nuint i = 1; i < (nuint)_blocks.Count; ++i)
            {
                var previousBlock = blocks[(int)(i - 1)];
                var block = blocks[(int)(i)];

                if (previousBlock.TotalFreeRegionSize > block.TotalFreeRegionSize)
                {
                    blocks[(int)(i - 1)] = block;
                    blocks[(int)(i)] = previousBlock;
                    return;
                }
            }
        }

        private bool TryAllocateRegion(ulong size, ulong alignment, GraphicsMemoryAllocationFlags flags, out GraphicsMemoryBlockRegion region)
        {
            if ((size + (2 * BlockMarginSize) > _blockPreferredSize))
            {
                // The requested size is larger than the maximum block size
                Unsafe.SkipInit(out region);
                return false;
            }

            var useDedicatedBlock = flags.HasFlag(GraphicsMemoryAllocationFlags.DedicatedBlock);
            var useExistingBlock = flags.HasFlag(GraphicsMemoryAllocationFlags.ExistingBlock);

            if (useDedicatedBlock && useExistingBlock)
            {
                ThrowArgumentOutOfRangeException(nameof(flags), flags);
            }

            _allocator.GetBudget(this, out GraphicsMemoryBudget budget);

            var availableMemory = (budget.EstimatedUsage < budget.EstimatedBudget) ? (budget.EstimatedBudget - budget.EstimatedUsage) : 0;
            var canCreateNewBlock = !useExistingBlock && ((nuint)_blocks.Count < _maximumBlockCount) && (availableMemory >= size);

            // 1. Search existing blocks

            for (nuint index = 0; index < (nuint)_blocks.Count; ++index)
            {
                var currentBlock = _blocks[(int)index];
                AssertNotNull(currentBlock, nameof(currentBlock));
                    
                if (currentBlock.TryAllocate(size, alignment, out region))
                {
                    if (currentBlock == _emptyBlock)
                    {
                        _emptyBlock = null;
                    }
                    return true;
                }
            }

            // 2. Try to create a new block

            if (!canCreateNewBlock)
            {
                Unsafe.SkipInit(out region);
                return false;
            }

            ulong blockSize = _blockPreferredSize;

            if (_blockMinimumSize != _blockPreferredSize)
            {
                // Allocate 1/8, 1/4, 1/2 as first blocks, ensuring we don't go smaller than the minimum
                ulong largestBlockSize = GetLargestBlockSize();

                for (uint i = 0; i < 3; ++i)
                {
                    ulong smallerBlockSize = blockSize / 2;

                    if ((smallerBlockSize > largestBlockSize) && (smallerBlockSize >= (size * 2)))
                    {
                        blockSize = Math.Max(smallerBlockSize, _blockMinimumSize);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (blockSize >= availableMemory)
            {
                Unsafe.SkipInit(out region);
                return false;
            }

            var block = CreateBlock(blockSize);
            _blocks.Add(block);

            return block.TryAllocate(size, alignment, out region);
        }
    }
}
