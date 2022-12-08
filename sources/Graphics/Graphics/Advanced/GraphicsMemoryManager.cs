// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;

namespace TerraFX.Graphics.Advanced;

/// <summary>Provides a way to manage memory for a graphics device.</summary>
public sealed unsafe class GraphicsMemoryManager : GraphicsDeviceObject
{
    /// <summary><c>true</c> if the memory manager should be externally synchronized; otherwise, <c>false</c>.</summary>
    /// <remarks>This defaults to <c>false</c> causing the manager to be internally synchronized using a multimedia safe locking mechanism.</remarks>
    public static bool IsExternallySynchronized { get; } = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(IsExternallySynchronized)}",
        defaultValue: false
    );

    /// <summary>The maximum number of allocators allowed in a memory manager.</summary>
    /// <remarks>This defaults to <see cref="uint.MaxValue"/> so that there is no maximum number of allocators.</remarks>
    public static uint MaximumMemoryAllocatorCount { get; } = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MaximumMemoryAllocatorCount)}",
        defaultValue: uint.MaxValue
    );

    /// <summary>The maximum length, in bytes, of a shared allocator allowed in a memory manager.</summary>
    /// <remarks>This defaults to <c>256MB</c> which allows ~64k small textures, 4k buffers, or 64 MSAA textures per shared allocator.</remarks>
    public static uint MaximumSharedMemoryAllocatorByteLength { get; } = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MaximumSharedMemoryAllocatorByteLength)}",
        defaultValue: 256U * 1024U * 1024U
    );

    /// <summary>The minimum number of allocators allowed in the memory manager.</summary>
    /// <remarks>This defaults to <c>0</c> so that there is no minimum number of allocators.</remarks>
    public static uint MinimumMemoryAllocatorCount { get; } = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MinimumMemoryAllocatorCount)}",
        defaultValue: 0U
    );

    /// <summary>The minimum length, in bytes, of an allocator allowed in a memory manager.</summary>
    /// <remarks>This defaults to <c>32MB</c> which is approx 1/8th the byte length of the default <see cref="MaximumSharedMemoryAllocatorByteLength" />.</remarks>
    public static uint MinimumMemoryAllocatorByteLength { get; } = GetAppContextData(
        $"{typeof(GraphicsMemoryManager).FullName}.{nameof(MinimumMemoryAllocatorByteLength)}",
        defaultValue: 32U * 1024U * 1024U
    );

    private readonly GraphicsMemoryAllocatorCreateFunc _createMemoryAllocator;
    private readonly D3D12_HEAP_FLAGS _d3d12HeapFlags;
    private readonly D3D12_HEAP_TYPE _d3d12HeapType;
    private readonly ValueMutex _mutex;

    private GraphicsMemoryAllocator? _emptyMemoryAllocator;
    private ValueList<GraphicsMemoryAllocator> _memoryAllocators;

    private GraphicsMemoryManagerInfo _memoryManagerInfo;

    internal GraphicsMemoryManager(GraphicsDevice device, in GraphicsMemoryManagerCreateOptions createOptions) : base(device)
    {
        _createMemoryAllocator = createOptions.CreateMemoryAllocator.IsNotNull ? createOptions.CreateMemoryAllocator : new GraphicsMemoryAllocatorCreateFunc(&GraphicsMemoryAllocator.CreateDefault);

        _d3d12HeapFlags = createOptions.D3D12HeapFlags;
        _d3d12HeapType = createOptions.D3D12HeapType;

        _mutex = new ValueMutex();

        _emptyMemoryAllocator = null;
        _memoryAllocators = new ValueList<GraphicsMemoryAllocator>();

        for (var index = 0; index < MinimumMemoryAllocatorCount; index++)
        {
            var memoryAllocatorByteLength = GetAdjustedMemoryAllocatorByteLength(MaximumSharedMemoryAllocatorByteLength);
            _ = AddMemoryAllocator(memoryAllocatorByteLength, isDedicated: false);
        }
    }

    /// <summary>Gets the length, in bytes, of the manager.</summary>
    public ulong ByteLength => _memoryManagerInfo.ByteLength;

    /// <summary>Gets the minimum length, in bytes, of the manager.</summary>
    public nuint MinimumByteLength => _memoryManagerInfo.MinimumByteLength;

    /// <summary>Gets the total number of operations performed by the manager.</summary>
    public ulong OperationCount => _memoryManagerInfo.OperationCount;

    /// <summary>Gets the total length, in bytes, of allocated memory regions.</summary>
    public ulong TotalAllocatedMemoryRegionByteLength => ByteLength - TotalFreeMemoryRegionByteLength;

    /// <summary>Gets the total length, in bytes, of free memory regions.</summary>
    public ulong TotalFreeMemoryRegionByteLength => _memoryManagerInfo.TotalFreeMemoryRegionByteLength;

    internal D3D12_HEAP_FLAGS D3D12HeapFlags => _d3d12HeapFlags;

    internal D3D12_HEAP_TYPE D3D12HeapType => _d3d12HeapType;

    private static void OnAllocatorFree(in GraphicsMemoryRegion memoryRegion)
    {
        var memoryAllocator = memoryRegion.MemoryAllocator;
        ThrowIfNull(memoryAllocator);

        if (memoryAllocator.DeviceObject is GraphicsMemoryHeap memoryHeap)
        {
            var memoryManager = memoryHeap.MemoryManager;
            memoryManager.Free(memoryAllocator, in memoryRegion);
        }
    }

    /// <summary>Allocate a memory region in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <returns>The allocated memory region.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.AllocationFlags" /> is undefined.</exception>
    /// <exception cref="OutOfMemoryException">There was not a large enough free memory region to complete the allocation.</exception>
    public GraphicsMemoryRegion Allocate(in GraphicsMemoryAllocationOptions allocationOptions)
    {
        if (!TryAllocate(in allocationOptions, out var memoryRegion))
        {
            ThrowOutOfMemoryException(allocationOptions.ByteLength);
        }
        return memoryRegion;
    }

    /// <summary>Tries to allocate a memory region in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <param name="memoryRegion">On return, contains the allocated memory region or <c>default</c> if the allocation failed.</param>
    /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.AllocationFlags" /> is undefined.</exception>
    /// <exception cref="ObjectDisposedException">The memory manager has been disposed.</exception>
    public bool TryAllocate(in GraphicsMemoryAllocationOptions allocationOptions, out GraphicsMemoryRegion memoryRegion)
    {
        ThrowIfNotPow2(allocationOptions.ByteAlignment);
        ThrowIfZero(allocationOptions.ByteLength);
        ThrowIfNotDefined(allocationOptions.AllocationFlags);

        return TryAllocateUnsafe(in allocationOptions, out memoryRegion);
    }

    /// <summary>Tries to allocate a set of memory regions in the manager.</summary>
    /// <param name="allocationOptions">The options to use when allocating the memory.</param>
    /// <param name="memoryRegions">On return, will be filled with the allocated memory regions.</param>
    /// <returns><c>true</c> if the regions were sucesfully allocated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteAlignment" /> is not zero or a <c>power of two</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.ByteLength" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsMemoryAllocationOptions.AllocationFlags" /> is undefined.</exception>
    /// <exception cref="ObjectDisposedException">The memory manager has been disposed.</exception>
    public bool TryAllocate(in GraphicsMemoryAllocationOptions allocationOptions, Span<GraphicsMemoryRegion> memoryRegions)
    {
        ThrowIfNotPow2(allocationOptions.ByteAlignment);
        ThrowIfZero(allocationOptions.ByteLength);
        ThrowIfNotDefined(allocationOptions.AllocationFlags);

        return TryAllocateUnsafe(in allocationOptions, memoryRegions);
    }

    /// <summary>Tries to set the minimum length, in bytes, of the manager.</summary>
    /// <param name="minimumByteLength">The minimum length, in bytes, of the manager.</param>
    /// <returns><c>true</c> if the minimum length was succesfully set; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">The memory manager has been disposed.</exception>
    public bool TrySetMinimumByteLength(nuint minimumByteLength)
    {
        ThrowIfDisposed();
        return TrySetMinimumByteLengthUnsafe(minimumByteLength);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var memoryAllocators = _memoryAllocators.AsSpanUnsafe(0, _memoryAllocators.Count);

        for (var index = 0; index < memoryAllocators.Length; index++)
        {
            var memoryAllocator = memoryAllocators[index];
            memoryAllocator.DeviceObject.Dispose();
        }

        _mutex.Dispose();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    private GraphicsMemoryAllocator AddMemoryAllocator(nuint byteLength, bool isDedicated)
    {
        var memoryHeapCreateOptions = new GraphicsMemoryHeapCreateOptions {
            ByteLength = byteLength,
            D3D12HeapFlags = D3D12HeapFlags,
            D3D12HeapType = D3D12HeapType,
        };
        var memoryHeap = new GraphicsMemoryHeap(this, in memoryHeapCreateOptions);

        var memoryAllocatorCreateOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = byteLength,
            IsDedicated = isDedicated,
            OnFree = new GraphicsMemoryAllocatorOnFreeCallback(&OnAllocatorFree),
        };
        var memoryAllocator = _createMemoryAllocator.Invoke(memoryHeap, in memoryAllocatorCreateOptions);

        _memoryManagerInfo.OperationCount++;
        _memoryAllocators.Add(memoryAllocator);

        _memoryManagerInfo.ByteLength += byteLength;
        _memoryManagerInfo.TotalFreeMemoryRegionByteLength += byteLength;

        return memoryAllocator;
    }

    private void Free(GraphicsMemoryAllocator memoryAllocator, in GraphicsMemoryRegion memoryRegion)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        FreeNoMutex(memoryAllocator, in memoryRegion);
    }

    private void FreeNoMutex(GraphicsMemoryAllocator memoryAllocator, in GraphicsMemoryRegion memoryRegion)
    {
        var memoryAllocatorIndex = _memoryAllocators.IndexOf(memoryAllocator);

        if (memoryAllocatorIndex < 0)
        {
            ThrowKeyNotFoundException(memoryRegion, nameof(_memoryAllocators));
        }

        TrackFree(in memoryRegion);

        if (memoryAllocator.IsEmpty)
        {
            var emptyMemoryAllocator = _emptyMemoryAllocator;

            if (emptyMemoryAllocator is not null)
            {
                if (_memoryAllocators.Count > MinimumMemoryAllocatorCount)
                {
                    var byteLength = _memoryManagerInfo.ByteLength;
                    var minimumByteLength = _memoryManagerInfo.MinimumByteLength;

                    // We have two empty memory allocators, we want to prefer removing the larger of the two

                    if (memoryAllocator.ByteLength > emptyMemoryAllocator.ByteLength)
                    {
                        if (byteLength - memoryAllocator.ByteLength >= minimumByteLength)
                        {
                            RemoveMemoryAllocatorAt(memoryAllocatorIndex);
                        }
                        else if (byteLength - emptyMemoryAllocator.ByteLength >= minimumByteLength)
                        {
                            RemoveMemoryAllocator(emptyMemoryAllocator);
                        }
                    }
                    else if (byteLength - emptyMemoryAllocator.ByteLength >= minimumByteLength)
                    {
                        RemoveMemoryAllocatorAt(memoryAllocatorIndex);
                    }
                    else if (byteLength - memoryAllocator.ByteLength >= minimumByteLength)
                    {
                        RemoveMemoryAllocator(emptyMemoryAllocator);
                    }
                    else
                    {
                        // Removing either would put us below the minimum byte length, so we can't remove
                    }
                }
                else if (memoryAllocator.ByteLength > emptyMemoryAllocator.ByteLength)
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
                // minimum byte length placed on the collection, so we will only respect the budget if those can be maintained.

                var memoryBudget = Device.GetMemoryBudget(this);

                if (memoryBudget.EstimatedMemoryByteUsage >= memoryBudget.EstimatedMemoryByteBudget && _memoryAllocators.Count > MinimumMemoryAllocatorCount && _memoryManagerInfo.ByteLength - memoryAllocator.ByteLength >= _memoryManagerInfo.MinimumByteLength)
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

                for (var index = 1; index < memoryAllocators.Length; index++)
                {
                    var currentMemoryAllocator = memoryAllocators[index];

                    if (previousMemoryAllocator.TotalFreeMemoryRegionByteLength <= currentMemoryAllocator.TotalFreeMemoryRegionByteLength)
                    {
                        previousMemoryAllocator = currentMemoryAllocator;
                    }
                    else
                    {
                        memoryAllocators[index - 1] = currentMemoryAllocator;
                        memoryAllocators[index] = previousMemoryAllocator;

                        return;
                    }
                }
            }
        }
    }

    private nuint GetAdjustedMemoryAllocatorByteLength(nuint byteLength)
    {
        // This method should only be called under the mutex
        var memoryAllocatorByteLength = byteLength;

        if (memoryAllocatorByteLength < MaximumSharedMemoryAllocatorByteLength)
        {
            memoryAllocatorByteLength = MaximumSharedMemoryAllocatorByteLength;

            if (MinimumMemoryAllocatorByteLength != MaximumSharedMemoryAllocatorByteLength)
            {
                // Allocate 1/8, 1/4, 1/2 as first memory allocators, ensuring we don't go smaller than the minimum

                var memoryAllocators = _memoryAllocators.AsSpanUnsafe(0, _memoryAllocators.Count);
                var largestMemoryAllocatorByteLength = GetLargestSharedMemoryAllocatorByteLength(memoryAllocators);

                for (var index = 0; index < 3; index++)
                {
                    var smallerMemoryAllocatorByteLength = memoryAllocatorByteLength / 2;

                    if (smallerMemoryAllocatorByteLength > largestMemoryAllocatorByteLength && smallerMemoryAllocatorByteLength >= byteLength * 2)
                    {
                        memoryAllocatorByteLength = Max(smallerMemoryAllocatorByteLength, MinimumMemoryAllocatorByteLength);
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
            memoryAllocatorByteLength = byteLength;
        }

        return memoryAllocatorByteLength;

        static nuint GetLargestSharedMemoryAllocatorByteLength(Span<GraphicsMemoryAllocator> memoryAllocators)
        {
            nuint result = 0;

            for (var index = memoryAllocators.Length - 1; index >= 0; index--)
            {
                var memoryAllocatorByteLength = memoryAllocators[index].ByteLength;

                if (memoryAllocatorByteLength < MaximumSharedMemoryAllocatorByteLength)
                {
                    result = Max(result, memoryAllocatorByteLength);
                }
                else if (memoryAllocatorByteLength == MaximumSharedMemoryAllocatorByteLength)
                {
                    result = MaximumSharedMemoryAllocatorByteLength;
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
        Assert(unchecked((uint)index <= _memoryAllocators.Count));

        var memoryAllocator = _memoryAllocators[index];
        memoryAllocator.DeviceObject.Dispose();

        _memoryManagerInfo.OperationCount++;
        _memoryAllocators.RemoveAt(index);

        var byteLength = memoryAllocator.ByteLength;
        _memoryManagerInfo.ByteLength -= byteLength;

        Assert(byteLength == memoryAllocator.TotalFreeMemoryRegionByteLength);
        _memoryManagerInfo.TotalFreeMemoryRegionByteLength -= byteLength;
    }

    private void TrackFree(in GraphicsMemoryRegion memoryRegion)
    {
        _memoryManagerInfo.OperationCount++;
        _memoryManagerInfo.TotalFreeMemoryRegionByteLength += memoryRegion.ByteLength;
    }

    private bool TryAllocateUnsafe(in GraphicsMemoryAllocationOptions allocationOptions, out GraphicsMemoryRegion memoryRegion)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TryAllocateNoMutex(in allocationOptions, out memoryRegion);
    }

    private bool TryAllocateNoMutex(in GraphicsMemoryAllocationOptions options, out GraphicsMemoryRegion memoryRegion)
    {
        // This method should only be called under the mutex

        var memoryAllocationFlags = options.AllocationFlags;

        var useDedicatedMemoryAllocator = memoryAllocationFlags.HasFlag(GraphicsMemoryAllocationFlags.DedicatedMemoryAllocator);
        var useExistingMemoryAllocator = memoryAllocationFlags.HasFlag(GraphicsMemoryAllocationFlags.ExistingMemoryAllocator);

        if (useDedicatedMemoryAllocator && useExistingMemoryAllocator)
        {
            ThrowForInvalidFlagsCombination(memoryAllocationFlags);
        }

        var budget = Device.GetMemoryBudget(this);

        var byteLength = options.ByteLength;
        var byteLengthWithMargins = byteLength + (2 * GraphicsMemoryAllocator.MinimumAllocatedMemoryRegionMarginByteLength);

        var memoryAllocators = _memoryAllocators.AsSpanUnsafe(0, _memoryAllocators.Count);

        var availableMemory = budget.EstimatedMemoryByteUsage < budget.EstimatedMemoryByteBudget ? (budget.EstimatedMemoryByteBudget - budget.EstimatedMemoryByteUsage) : 0;
        var canCreateNewMemoryAllocator = !useExistingMemoryAllocator && (memoryAllocators.Length < MaximumMemoryAllocatorCount) && (memoryAllocationFlags.HasFlag(GraphicsMemoryAllocationFlags.CanExceedBudget) || (availableMemory >= byteLengthWithMargins));

        // 1. Search existing memory allocators
        var byteAlignment = options.ByteAlignment;

        if (!useDedicatedMemoryAllocator && (byteLength <= MaximumSharedMemoryAllocatorByteLength))
        {
            for (var memoryAllocatorIndex = 0; memoryAllocatorIndex < memoryAllocators.Length; memoryAllocatorIndex++)
            {
                var currentMemoryAllocator = memoryAllocators[memoryAllocatorIndex];
                AssertNotNull(currentMemoryAllocator);

                if (TryTrackAllocation(currentMemoryAllocator, byteLength, byteAlignment, out memoryRegion))
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

        var memoryAllocatorByteLength = GetAdjustedMemoryAllocatorByteLength(byteLengthWithMargins);

        if (memoryAllocatorByteLength >= availableMemory)
        {
            memoryRegion = default;
            return false;
        }

        var memoryAllocator = AddMemoryAllocator(memoryAllocatorByteLength, isDedicated: useDedicatedMemoryAllocator);
        return TryTrackAllocation(memoryAllocator, byteLength, byteAlignment, out memoryRegion);
    }

    private bool TryAllocateUnsafe(in GraphicsMemoryAllocationOptions allocationOptions, Span<GraphicsMemoryRegion> memoryRegions)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TryAllocateNoMutex(in allocationOptions, memoryRegions);
    }

    private bool TryAllocateNoMutex(in GraphicsMemoryAllocationOptions options, Span<GraphicsMemoryRegion> memoryRegions)
    {
        // This method should only be called under the mutex

        var succeeded = false;

        for (var index = 0; index < memoryRegions.Length; index++)
        {
            succeeded = TryAllocateNoMutex(in options, out memoryRegions[index]);

            if (!succeeded)
            {
                // Something failed so free all already allocated regions

                while (index >= 0)
                {
                    FreeNoMutex(memoryRegions[index].MemoryAllocator, in memoryRegions[index]);
                    memoryRegions[index] = default;
                    index--;
                }

                break;
            }
        }

        return succeeded;
    }

    private bool TrySetMinimumByteLengthUnsafe(nuint minimumByteLength)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TrySetMinimumByteLengthNoMutex(minimumByteLength);
    }

    private bool TrySetMinimumByteLengthNoMutex(nuint minimumByteLength)
    {
        // This method should only be called under the mutex

        var currentMinimumByteLength = _memoryManagerInfo.MinimumByteLength;

        if (minimumByteLength == currentMinimumByteLength)
        {
            return true;
        }

        var byteLength = _memoryManagerInfo.ByteLength;
        var memoryAllocatorCount = _memoryAllocators.Count;

        if (minimumByteLength < currentMinimumByteLength)
        {
            // The new minimum byte length is less than the previous, so we will iterate the
            // memory allocators from last to first (largest to smallest) to try and
            // free space that may now be available.

            var emptyMemoryAllocator = null as GraphicsMemoryAllocator;
            var minimumMemoryAllocatorCount = MinimumMemoryAllocatorCount;

            for (var index = memoryAllocatorCount; index >= 0; index--)
            {
                var memoryAllocator = _memoryAllocators.GetReferenceUnsafe(index);

                var memoryAllocatorByteLength = memoryAllocator.ByteLength;
                var memoryAllocatorIsEmpty = memoryAllocator.IsEmpty;

                if (memoryAllocatorIsEmpty)
                {
                    if (byteLength - memoryAllocatorByteLength >= minimumByteLength && memoryAllocatorCount - 1 >= minimumMemoryAllocatorCount)
                    {
                        RemoveMemoryAllocatorAt(index);
                        byteLength -= memoryAllocatorByteLength;
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
            // The new minimum byte length is greater than the previous, so we will allocate
            // new memory allocators until we exceed the minimum byte length, but ensuring we
            // don't exceed maximumSharedMemoryAllocatorByteLength while doing so.

            var emptyMemoryAllocator = null as GraphicsMemoryAllocator;

            while (byteLength < minimumByteLength)
            {
                if (memoryAllocatorCount < MaximumMemoryAllocatorCount)
                {
                    var memoryAllocatorByteLength = GetAdjustedMemoryAllocatorByteLength(MaximumSharedMemoryAllocatorByteLength);

                    if (byteLength + memoryAllocatorByteLength > minimumByteLength)
                    {
                        // The current byte length plus the new memory allocator will exceed the
                        // minimum byte length requested, so adjust it to be just large enough.

                        var remainingByteLength = (nuint)(minimumByteLength - byteLength);
                        memoryAllocatorByteLength = Clamp(remainingByteLength, MinimumMemoryAllocatorByteLength, memoryAllocatorByteLength);
                    }

                    emptyMemoryAllocator ??= AddMemoryAllocator(memoryAllocatorByteLength, isDedicated: false);
                    byteLength += memoryAllocatorByteLength;

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

        _memoryManagerInfo.MinimumByteLength = minimumByteLength;
        Assert(_memoryManagerInfo.ByteLength == byteLength);

        return true;
    }

    private bool TryTrackAllocation(GraphicsMemoryAllocator memoryAllocator, nuint byteLength, nuint byteAlignment, out GraphicsMemoryRegion memoryRegion)
    {
        var succeeded = memoryAllocator.TryAllocate(byteLength, byteAlignment, out memoryRegion);

        if (succeeded)
        {
            _memoryManagerInfo.OperationCount++;
            _memoryManagerInfo.TotalFreeMemoryRegionByteLength -= memoryRegion.ByteLength;
        }

        return succeeded;
    }
}
