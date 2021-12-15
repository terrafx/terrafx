// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TerraFX.Collections;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed class VulkanGraphicsMemoryManager : GraphicsMemoryManager
{
    private readonly uint _vkMemoryTypeIndex;
    private readonly ValueMutex _mutex;

    private GraphicsMemoryAllocator? _emptyMemoryAllocator;
    private ValueList<GraphicsMemoryAllocator> _memoryAllocators;

    private ulong _minimumSize;
    private string _name = null!;
    private ulong _size;

    private VolatileState _state;

    internal VulkanGraphicsMemoryManager(VulkanGraphicsDevice device, uint vkMemoryTypeIndex)
        : base(device)
    {
        _vkMemoryTypeIndex = vkMemoryTypeIndex;
        _mutex = new ValueMutex();

        _emptyMemoryAllocator = null;
        _memoryAllocators = new ValueList<GraphicsMemoryAllocator>();

        _minimumSize = 0;
        _size = 0;

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsMemoryManager);

        for (var i = 0; i < MinimumMemoryAllocatorCount; ++i)
        {
            var memoryAllocatorSize = GetAdjustedMemoryAllocatorSize(MaximumSharedMemoryAllocatorSize);
            _ = AddMemoryAllocator(memoryAllocatorSize);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <summary>Gets the number of memory allocators in the memory manager.</summary>
    public override int Count => _memoryAllocators.Count;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <summary>Gets <c>true</c> if the manager is empty; otherwise, <c>false</c>.</summary>
    public override bool IsEmpty => _memoryAllocators.Count == 0;

    /// <summary>Gets the minimum size of the manager, in bytes.</summary>
    public override ulong MinimumSize => _minimumSize;

    /// <inheritdoc />
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? "";
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the size of the manager, in bytes.</summary>
    public override ulong Size => _size;

    /// <summary>Gets the memory type index used when creating the <see cref="VkDeviceMemory" /> instance for a memory heap.</summary>
    public uint VkMemoryTypeIndex => _vkMemoryTypeIndex;

    /// <summary>Allocate a memory region in the manager.</summary>
    /// <param name="size">The size of the memory region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the memory region to allocate, in bytes.</param>
    /// <param name="memoryAllocationFlags">The flags that modify how the memory region is allocated.</param>
    /// <returns>The allocated memory region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryAllocationFlags" /> has an invalid combination.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public override GraphicsMemoryRegion Allocate(ulong size, ulong alignment = 0, GraphicsMemoryAllocationFlags memoryAllocationFlags = GraphicsMemoryAllocationFlags.None)
    {
        if (!TryAllocate(size, alignment, memoryAllocationFlags, out var memoryRegion))
        {
            ThrowOutOfMemoryException(size);
        }

        return memoryRegion;
    }

    /// <summary>Frees a memory region from the manager.</summary>
    /// <param name="memoryRegion">The memory region to be freed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="memoryRegion" />.<see cref="GraphicsMemoryRegion.Allocator" /> is <c>null</c>.</exception>
    /// <exception cref="KeyNotFoundException"><paramref name="memoryRegion" /> was not found in the collection.</exception>
    public override void Free(in GraphicsMemoryRegion memoryRegion)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        FreeInternal(in memoryRegion);
    }

    /// <inheritdoc />
    public override IEnumerator<GraphicsMemoryAllocator> GetEnumerator() => _memoryAllocators.GetEnumerator();

    /// <summary>Tries to allocation a memory region in the manager.</summary>
    /// <param name="size">The size of the memory region to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the memory region to allocate, in bytes.</param>
    /// <param name="memoryAllocationFlags">The flags that modify how the memory region is allocated.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryAllocationFlags" /> has an invalid combination.</exception>
    public override bool TryAllocate(ulong size, [Optional] ulong alignment, [Optional] GraphicsMemoryAllocationFlags memoryAllocationFlags, out GraphicsMemoryRegion memoryRegion)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TryAllocateInternal(size, alignment, memoryAllocationFlags, out memoryRegion);
    }

    /// <summary>Tries to allocate a set of memory regions in the manager.</summary>
    /// <param name="size">The size of the memory regions to allocate, in bytes.</param>
    /// <param name="alignment">The alignment of the memory regions to allocate, in bytes.</param>
    /// <param name="memoryAllocationFlags">The flags that modify how the memory regions are allocated.</param>
    /// <param name="memoryRegions">On return, will be filled with the allocated memory regions.</param>
    /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryAllocationFlags" /> has an invalid combination.</exception>
    public override bool TryAllocate(ulong size, [Optional] ulong alignment, [Optional] GraphicsMemoryAllocationFlags memoryAllocationFlags, Span<GraphicsMemoryRegion> memoryRegions)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TryAllocateInternal(size, alignment, memoryAllocationFlags, memoryRegions);
    }

    /// <summary>Tries to set the minimum size of the manager, in bytes.</summary>
    /// <param name="minimumSize">The minimum size of the manager, in bytes.</param>
    /// <returns><c>true</c> if the minimum size was succesfully set; otherwise, <c>false</c>.</returns>
    public override bool TrySetMinimumSize(ulong minimumSize)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TrySetMinimumSizeInternal(minimumSize);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            var memoryAllocators = _memoryAllocators.AsSpanUnsafe(0, _memoryAllocators.Count);

            for (var index = 0; index < memoryAllocators.Length; index++)
            {
                var memoryAllocator = memoryAllocators[index];
                memoryAllocator.DeviceObject.Dispose();
            }

            _mutex.Dispose();
        }

        _state.EndDispose();
    }

    private GraphicsMemoryAllocator AddMemoryAllocator(ulong size)
    {
        var memoryHeap = new VulkanGraphicsMemoryHeap(Device, size, VkMemoryTypeIndex);
        var memoryAllocator = GraphicsMemoryAllocator.CreateDefault(memoryHeap, size);

        _memoryAllocators.Add(memoryAllocator);
        _size += size;

        return memoryAllocator;
    }

    private void FreeInternal(in GraphicsMemoryRegion memoryRegion)
    {
        // This method should only be called under the mutex

        var memoryAllocator = memoryRegion.Allocator;
        ThrowIfNull(memoryAllocator);

        var memoryAllocatorIndex = _memoryAllocators.IndexOf(memoryAllocator);

        if (memoryAllocatorIndex == -1)
        {
            ThrowKeyNotFoundException(memoryRegion, nameof(_memoryAllocators));
        }

        memoryAllocator.Free(in memoryRegion);

        if (memoryAllocator.IsEmpty)
        {
            var emptyMemoryAllocator = _emptyMemoryAllocator;

            if (emptyMemoryAllocator is not null)
            {
                if (_memoryAllocators.Count > MinimumMemoryAllocatorCount)
                {
                    var size = _size;
                    var minimumSize = _minimumSize;

                    // We have two empty memory allocators, we want to prefer removing the larger of the two

                    if (memoryAllocator.Size > emptyMemoryAllocator.Size)
                    {
                        if ((size - memoryAllocator.Size) >= minimumSize)
                        {
                            RemoveMemoryAllocatorAt(memoryAllocatorIndex);
                        }
                        else if ((size - emptyMemoryAllocator.Size) >= minimumSize)
                        {
                            RemoveMemoryAllocator(emptyMemoryAllocator);
                        }
                    }
                    else if ((size - emptyMemoryAllocator.Size) >= minimumSize)
                    {
                        RemoveMemoryAllocatorAt(memoryAllocatorIndex);
                    }
                    else if ((size - memoryAllocator.Size) >= minimumSize)
                    {
                        RemoveMemoryAllocator(emptyMemoryAllocator);
                    }
                    else
                    {
                        // Removing either would put us below the minimum size, so we can't remove
                    }
                }
                else if (memoryAllocator.Size > emptyMemoryAllocator.Size)
                {
                    // We can't remove the memory allocator, so set empty memory allocator to the larger
                    _emptyMemoryAllocator = memoryAllocator;
                }
            }
            else
            {
                // We have no existing empty memory allocators, so we want to set the index to this memory allocator
                // unless we are currently exceeding our memory budget, in which case we want to free the memory
                // allocator instead. However, we still need to maintain the minimum memory allocator count and
                // minimum size placed on the collection, so we will only respect the budget if those can be maintained.

                var memoryBudget = Device.GetMemoryBudget(this);

                if ((memoryBudget.EstimatedUsage >= memoryBudget.EstimatedBudget) && (_memoryAllocators.Count > MinimumMemoryAllocatorCount) && ((_size - memoryAllocator.Size) >= _minimumSize))
                {
                    RemoveMemoryAllocatorAt(memoryAllocatorIndex);
                }
                else
                {
                    _emptyMemoryAllocator = memoryAllocator;
                }
            }
        }

        var memoryAllocators = _memoryAllocators.AsSpanUnsafe(0, _memoryAllocators.Count);
        IncrementallySort(memoryAllocators);

        static void IncrementallySort(Span<GraphicsMemoryAllocator> memoryAllocators)
        {
            // Bubble sort only until first swap. This is called after
            // freeing a region and will result in eventual consistency

            if (memoryAllocators.Length >= 2)
            {
                var previousMemoryAllocator = memoryAllocators[0];

                for (var i = 1; i < memoryAllocators.Length; ++i)
                {
                    var currentMemoryAllocator = memoryAllocators[i];

                    if (previousMemoryAllocator.TotalFreeMemoryRegionSize <= currentMemoryAllocator.TotalFreeMemoryRegionSize)
                    {
                        previousMemoryAllocator = currentMemoryAllocator;
                    }
                    else
                    {
                        memoryAllocators[i - 1] = currentMemoryAllocator;
                        memoryAllocators[i] = previousMemoryAllocator;

                        return;
                    }
                }
            }
        }
    }

    private ulong GetAdjustedMemoryAllocatorSize(ulong size)
    {
        // This method should only be called under the mutex
        var memoryAllocatorSize = size;

        if (memoryAllocatorSize < MaximumSharedMemoryAllocatorSize)
        {
            memoryAllocatorSize = MaximumSharedMemoryAllocatorSize;

            if (MinimumMemoryAllocatorSize != MaximumSharedMemoryAllocatorSize)
            {
                // Allocate 1/8, 1/4, 1/2 as first memory allocators, ensuring we don't go smaller than the minimum

                var memoryAllocators = _memoryAllocators.AsSpanUnsafe(0, _memoryAllocators.Count);
                var largestMemoryAllocatorSize = GetLargestSharedMemoryAllocatorSize(memoryAllocators);

                for (var i = 0; i < 3; ++i)
                {
                    var smallerMemoryAllocatorSize = memoryAllocatorSize / 2;

                    if ((smallerMemoryAllocatorSize > largestMemoryAllocatorSize) && (smallerMemoryAllocatorSize >= (size * 2)))
                    {
                        memoryAllocatorSize = Math.Max(smallerMemoryAllocatorSize, MinimumMemoryAllocatorSize);
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
            memoryAllocatorSize = size;
        }

        return memoryAllocatorSize;

        static ulong GetLargestSharedMemoryAllocatorSize(Span<GraphicsMemoryAllocator> memoryAllocators)
        {
            var result = 0UL;

            for (var i = memoryAllocators.Length; i-- != 0;)
            {
                var memoryAllocatorSize = memoryAllocators[i].Size;

                if (memoryAllocatorSize < MaximumSharedMemoryAllocatorSize)
                {
                    result = Math.Max(result, memoryAllocatorSize);
                }
                else if (memoryAllocatorSize == MaximumSharedMemoryAllocatorSize)
                {
                    result = MaximumSharedMemoryAllocatorSize;
                    break;
                }
            }

            return result;
        }
    }

    private void RemoveMemoryAllocator(GraphicsMemoryAllocator memoryAllocator)
    {
        var memoryAllocatorIndex = _memoryAllocators.IndexOf(memoryAllocator);
        RemoveMemoryAllocatorAt(memoryAllocatorIndex);
    }

    private void RemoveMemoryAllocatorAt(int index)
    {
        Assert(AssertionsEnabled && unchecked((uint)index <= _memoryAllocators.Count));

        var memoryAllocator = _memoryAllocators[index];
        memoryAllocator.DeviceObject.Dispose();

        _memoryAllocators.RemoveAt(index);
        _size -= memoryAllocator.Size;
    }

    private bool TryAllocateInternal(ulong size, ulong alignment, GraphicsMemoryAllocationFlags allocationFlags, out GraphicsMemoryRegion memoryRegion)
    {
        // This method should only be called under the mutex

        var useDedicatedMemoryAllocator = allocationFlags.HasFlag(GraphicsMemoryAllocationFlags.DedicatedMemoryAllocator);
        var useExistingMemoryAllocator = allocationFlags.HasFlag(GraphicsMemoryAllocationFlags.ExistingMemoryAllocator);

        if (useDedicatedMemoryAllocator && useExistingMemoryAllocator)
        {
            ThrowForInvalidFlagsCombination(allocationFlags);
        }

        var budget = Device.GetMemoryBudget(this);

        var sizeWithMargins = size + (2 * GraphicsMemoryAllocator.MinimumAllocatedMemoryRegionMarginSize);

        var memoryAllocators = _memoryAllocators.AsSpanUnsafe(0, _memoryAllocators.Count);

        var availableMemory = (budget.EstimatedUsage < budget.EstimatedBudget) ? (budget.EstimatedBudget - budget.EstimatedUsage) : 0;
        var canCreateNewMemoryAllocator = !useExistingMemoryAllocator && (memoryAllocators.Length < MaximumMemoryAllocatorCount) && (availableMemory >= sizeWithMargins);

        // 1. Search existing memory allocators

        if (!useDedicatedMemoryAllocator && (size <= MaximumSharedMemoryAllocatorSize))
        {
            for (var memoryAllocatorIndex = 0; memoryAllocatorIndex < memoryAllocators.Length; ++memoryAllocatorIndex)
            {
                var currentMemoryAllocator = memoryAllocators[memoryAllocatorIndex];
                AssertNotNull(currentMemoryAllocator);

                if (currentMemoryAllocator.TryAllocate(size, alignment, out memoryRegion))
                {
                    if (currentMemoryAllocator == _emptyMemoryAllocator)
                    {
                        _emptyMemoryAllocator = null;
                    }
                    return true;
                }
            }
        }

        // 2. Try to create a new memory allocator

        if (!canCreateNewMemoryAllocator)
        {
            memoryRegion = default;
            return false;
        }

        var memoryAllocatorSize = GetAdjustedMemoryAllocatorSize(sizeWithMargins);

        if (memoryAllocatorSize >= availableMemory)
        {
            memoryRegion = default;
            return false;
        }

        var allocator = AddMemoryAllocator(memoryAllocatorSize);
        return allocator.TryAllocate(size, alignment, out memoryRegion);
    }

    private bool TryAllocateInternal(ulong size, ulong alignment, GraphicsMemoryAllocationFlags flags, Span<GraphicsMemoryRegion> memoryRegions)
    {
        // This method should only be called under the mutex

        var succeeded = false;

        for (var index = 0; index < memoryRegions.Length; ++index)
        {
            succeeded = TryAllocateInternal(size, alignment, flags, out memoryRegions[index]);

            if (!succeeded)
            {
                // Something failed so free all already allocated regions

                while (index-- != 0)
                {
                    FreeInternal(in memoryRegions[index]);
                    memoryRegions[index] = default;
                }

                break;
            }
        }

        return succeeded;
    }

    private bool TrySetMinimumSizeInternal(ulong minimumSize)
    {
        // This method should only be called under the mutex

        var currentMinimumSize = _minimumSize;

        if (minimumSize == currentMinimumSize)
        {
            return true;
        }

        var size = _size;
        var memoryAllocatorCount = _memoryAllocators.Count;

        if (minimumSize < currentMinimumSize)
        {
            // The new minimum size is less than the previous, so we will iterate the
            // memory allocators from last to first (largest to smallest) to try and
            // free space that may now be available.

            var emptyMemoryAllocator = null as GraphicsMemoryAllocator;
            var minimumMemoryAllocatorCount = MinimumMemoryAllocatorCount;

            for (var memoryAllocatorIndex = memoryAllocatorCount; memoryAllocatorIndex-- != 0;)
            {
                var memoryAllocator = _memoryAllocators.GetReferenceUnsafe(memoryAllocatorIndex);

                var memoryAllocatorSize = memoryAllocator.Size;
                var memoryAllocatorIsEmpty = memoryAllocator.IsEmpty;

                if (memoryAllocatorIsEmpty)
                {
                    if (((size - memoryAllocatorSize) >= minimumSize) && ((memoryAllocatorCount - 1) >= minimumMemoryAllocatorCount))
                    {
                        RemoveMemoryAllocatorAt(memoryAllocatorIndex);
                        size -= memoryAllocatorSize;
                        --memoryAllocatorCount;
                    }
                    else
                    {
                        emptyMemoryAllocator ??= memoryAllocator;
                    }
                }
            }

            _emptyMemoryAllocator = emptyMemoryAllocator;
        }
        else
        {
            // The new minimum size is greater than the previous, so we will allocate
            // new memory allocators until we exceed the minimum size, but ensuring we
            // don't exceed maximumSharedMemoryAllocatorSize while doing so.

            var emptyMemoryAllocator = null as GraphicsMemoryAllocator;

            while (size < minimumSize)
            {
                if (memoryAllocatorCount < MaximumMemoryAllocatorCount)
                {
                    var memoryAllocatorSize = GetAdjustedMemoryAllocatorSize(MaximumSharedMemoryAllocatorSize);

                    if (((size + memoryAllocatorSize) > minimumSize) && (memoryAllocatorSize != MinimumMemoryAllocatorSize))
                    {
                        // The current size plus the new memroy allocator will exceed the
                        // minimum size requested, so adjust it to be just large enough.
                        memoryAllocatorSize = minimumSize - size;
                    }

                    emptyMemoryAllocator ??= AddMemoryAllocator(memoryAllocatorSize);
                    size += memoryAllocatorSize;

                    ++memoryAllocatorCount;
                }
                else
                {
                    _emptyMemoryAllocator ??= emptyMemoryAllocator;
                    return false;
                }
            }

            _emptyMemoryAllocator ??= emptyMemoryAllocator;
        }

        _minimumSize = minimumSize;
        Assert(AssertionsEnabled && (_size == size));

        return true;
    }
}