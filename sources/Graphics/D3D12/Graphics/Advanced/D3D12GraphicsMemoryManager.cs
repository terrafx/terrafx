// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics.Advanced;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsMemoryManager : GraphicsMemoryManager
{
    private readonly GraphicsMemoryAllocatorCreateFunc _createMemoryAllocator;
    private readonly D3D12_HEAP_FLAGS _d3d12HeapFlags;
    private readonly D3D12_HEAP_TYPE _d3d12HeapType;
    private readonly ValueMutex _mutex;

    private GraphicsMemoryAllocator? _emptyMemoryAllocator;
    private ValueList<GraphicsMemoryAllocator> _memoryAllocators;

    internal D3D12GraphicsMemoryManager(D3D12GraphicsDevice device, in D3D12GraphicsMemoryManagerCreateOptions createOptions) : base(device)
    {
        _createMemoryAllocator = createOptions.CreateMemoryAllocator.IsNotNull ? createOptions.CreateMemoryAllocator : new GraphicsMemoryAllocatorCreateFunc(&GraphicsMemoryAllocator.CreateDefault);

        _d3d12HeapFlags = createOptions.D3D12HeapFlags;
        _d3d12HeapType = createOptions.D3D12HeapType;

        _mutex = new ValueMutex();

        _emptyMemoryAllocator = null;
        _memoryAllocators = new ValueList<GraphicsMemoryAllocator>();

        for (var i = 0; i < MinimumMemoryAllocatorCount; ++i)
        {
            var memoryAllocatorByteLength = GetAdjustedMemoryAllocatorByteLength(MaximumSharedMemoryAllocatorByteLength);
            _ = AddMemoryAllocator(memoryAllocatorByteLength, isDedicated: false);
        }
    }

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="D3D12_HEAP_FLAGS" /> used when creating the <see cref="ID3D12Heap" /> for the memory manager.</summary>
    public D3D12_HEAP_FLAGS D3D12HeapFlags => _d3d12HeapFlags;

    /// <summary>Gets the <see cref="D3D12_HEAP_TYPE" /> used when creating the <see cref="ID3D12Heap" /> for the memory manager.</summary>
    public D3D12_HEAP_TYPE D3D12HeapType => _d3d12HeapType;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    private static void OnAllocatorFree(in GraphicsMemoryRegion memoryRegion)
    {
        var memoryAllocator = memoryRegion.MemoryAllocator;
        ThrowIfNull(memoryAllocator);

        if (memoryAllocator.DeviceObject is D3D12GraphicsMemoryHeap memoryHeap)
        {
            var memoryManager = memoryHeap.MemoryManager;
            memoryManager.Free(memoryAllocator, in memoryRegion);
        }
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

    /// <inheritdoc />
    protected override bool TryAllocateUnsafe(in GraphicsMemoryAllocationOptions memoryAllocationOptions, out GraphicsMemoryRegion memoryRegion)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TryAllocateNoMutex(in memoryAllocationOptions, out memoryRegion);
    }

    /// <inheritdoc />
    protected override bool TryAllocateUnsafe(in GraphicsMemoryAllocationOptions memoryAllocationOptions, Span<GraphicsMemoryRegion> memoryRegions)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TryAllocateNoMutex(in memoryAllocationOptions, memoryRegions);
    }

    /// <inheritdoc />
    protected override bool TrySetMinimumByteLengthUnsafe(nuint minimumByteLength)
    {
        using var mutex = new DisposableMutex(_mutex, IsExternallySynchronized);
        return TrySetMinimumByteLengthNoMutex(minimumByteLength);
    }

    private GraphicsMemoryAllocator AddMemoryAllocator(nuint byteLength, bool isDedicated)
    {
        var memoryHeapCreateOptions = new D3D12GraphicsMemoryHeapCreateOptions {
            ByteLength = byteLength,
            D3D12HeapFlags = D3D12HeapFlags,
            D3D12HeapType = D3D12HeapType,
        };
        var memoryHeap = new D3D12GraphicsMemoryHeap(this, in memoryHeapCreateOptions);

        var memoryAllocatorCreateOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = byteLength,
            IsDedicated = isDedicated,
            OnFree = new GraphicsMemoryAllocatorOnFreeCallback(&OnAllocatorFree),
        };
        var memoryAllocator = _createMemoryAllocator.Invoke(memoryHeap, in memoryAllocatorCreateOptions);

        MemoryManagerInfo.OperationCount++;
        _memoryAllocators.Add(memoryAllocator);

        MemoryManagerInfo.ByteLength += byteLength;
        MemoryManagerInfo.TotalFreeMemoryRegionByteLength += byteLength;

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

        if (memoryAllocatorIndex == -1)
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
                    var byteLength = MemoryManagerInfo.ByteLength;
                    var minimumByteLength = MemoryManagerInfo.MinimumByteLength;

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

                if (memoryBudget.EstimatedMemoryByteUsage >= memoryBudget.EstimatedMemoryByteBudget && _memoryAllocators.Count > MinimumMemoryAllocatorCount && MemoryManagerInfo.ByteLength - memoryAllocator.ByteLength >= MemoryManagerInfo.MinimumByteLength)
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

                for (var index = 1; index < memoryAllocators.Length; ++index)
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

                for (var index = 0; index < 3; ++index)
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
        Assert(AssertionsEnabled && unchecked((uint)index <= _memoryAllocators.Count));

        var memoryAllocator = _memoryAllocators[index];
        memoryAllocator.DeviceObject.Dispose();

        MemoryManagerInfo.OperationCount++;
        _memoryAllocators.RemoveAt(index);

        var byteLength = memoryAllocator.ByteLength;
        MemoryManagerInfo.ByteLength -= byteLength;

        Assert(AssertionsEnabled && byteLength == memoryAllocator.TotalFreeMemoryRegionByteLength);
        MemoryManagerInfo.TotalFreeMemoryRegionByteLength -= byteLength;
    }

    private bool TrackAllocation(GraphicsMemoryAllocator memoryAllocator, nuint byteLength, nuint byteAlignment, out GraphicsMemoryRegion memoryRegion)
    {
        var succeeded = memoryAllocator.TryAllocate(byteLength, byteAlignment, out memoryRegion);

        if (succeeded)
        {
            MemoryManagerInfo.OperationCount++;
            MemoryManagerInfo.TotalFreeMemoryRegionByteLength -= memoryRegion.ByteLength;
        }

        return succeeded;
    }

    private void TrackFree(in GraphicsMemoryRegion memoryRegion)
    {
        MemoryManagerInfo.OperationCount++;
        MemoryManagerInfo.TotalFreeMemoryRegionByteLength += memoryRegion.ByteLength;
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

        var availableMemory = budget.EstimatedMemoryByteUsage < budget.EstimatedMemoryByteBudget ? budget.EstimatedMemoryByteBudget - budget.EstimatedMemoryByteUsage : 0;
        var canCreateNewMemoryAllocator = !useExistingMemoryAllocator && memoryAllocators.Length < MaximumMemoryAllocatorCount && (memoryAllocationFlags.HasFlag(GraphicsMemoryAllocationFlags.CanExceedBudget) || availableMemory >= byteLengthWithMargins);

        // 1. Search existing memory allocators
        var byteAlignment = options.ByteAlignment;

        if (!useDedicatedMemoryAllocator && byteLength <= MaximumSharedMemoryAllocatorByteLength)
        {
            for (var memoryAllocatorIndex = 0; memoryAllocatorIndex < memoryAllocators.Length; ++memoryAllocatorIndex)
            {
                var currentMemoryAllocator = memoryAllocators[memoryAllocatorIndex];
                AssertNotNull(currentMemoryAllocator);

                if (TrackAllocation(currentMemoryAllocator, byteLength, byteAlignment, out memoryRegion))
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
        return TrackAllocation(memoryAllocator, byteLength, byteAlignment, out memoryRegion);
    }

    private bool TryAllocateNoMutex(in GraphicsMemoryAllocationOptions options, Span<GraphicsMemoryRegion> memoryRegions)
    {
        // This method should only be called under the mutex

        var succeeded = false;

        for (var index = 0; index < memoryRegions.Length; ++index)
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

    private bool TrySetMinimumByteLengthNoMutex(nuint minimumByteLength)
    {
        // This method should only be called under the mutex

        var currentMinimumByteLength = MemoryManagerInfo.MinimumByteLength;

        if (minimumByteLength == currentMinimumByteLength)
        {
            return true;
        }

        var byteLength = MemoryManagerInfo.ByteLength;
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

        MemoryManagerInfo.MinimumByteLength = minimumByteLength;
        Assert(AssertionsEnabled && MemoryManagerInfo.ByteLength == byteLength);

        return true;
    }
}
