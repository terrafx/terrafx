// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_HEAP_TIER;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsMemoryAllocator : GraphicsMemoryAllocator
{
    private readonly D3D12GraphicsMemoryBlockCollection[] _blockCollections;
    private readonly bool _supportsResourceHeapTier2;

    private VolatileState _state;

    internal D3D12GraphicsMemoryAllocator(D3D12GraphicsDevice device, in GraphicsMemoryAllocatorSettings settings)
        : base(device, in settings)
    {
        var supportsResourceHeapTier2 = Device.D3D12Options.ResourceHeapTier >= D3D12_RESOURCE_HEAP_TIER_2;

        _blockCollections = supportsResourceHeapTier2
            ? new D3D12GraphicsMemoryBlockCollection[3] {
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_READBACK),
            }
            : new D3D12GraphicsMemoryBlockCollection[9] {
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                    new D3D12GraphicsMemoryBlockCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
            };

        _supportsResourceHeapTier2 = supportsResourceHeapTier2;

        // TODO: UpdateBudget
        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsMemoryAllocator" /> class.</summary>
    ~D3D12GraphicsMemoryAllocator() => Dispose(isDisposing: true);

    /// <inheritdoc />
    public override int Count => _blockCollections.Length;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

    /// <summary>Gets <c>true</c> if <see cref="Device" /> supports <see cref="D3D12_RESOURCE_HEAP_TIER_2" />; otherwise, <c>false</c>.</summary>
    public bool SupportsResourceHeapTier2 => _supportsResourceHeapTier2;

    /// <inheritdoc />
    public override D3D12GraphicsBuffer<TMetadata> CreateBuffer<TMetadata>(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, GraphicsMemoryRegionAllocationFlags allocationFlags = GraphicsMemoryRegionAllocationFlags.None)
    {
        var index = GetBlockCollectionIndex(cpuAccess, 0);
        var alignment = (ulong)D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;

        var resourceDesc = D3D12_RESOURCE_DESC.Buffer(size, D3D12_RESOURCE_FLAG_NONE, alignment);
        var resourceAllocationInfo = Device.D3D12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &resourceDesc);
        ref readonly var blockCollection = ref _blockCollections[index];

        var memoryBlockRegion = blockCollection.Allocate(resourceAllocationInfo.SizeInBytes, resourceAllocationInfo.Alignment, allocationFlags);
        return new D3D12GraphicsBuffer<TMetadata>(Device, kind, in memoryBlockRegion, cpuAccess);
    }

    /// <inheritdoc />
    public override D3D12GraphicsTexture<TMetadata> CreateTexture<TMetadata>(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, GraphicsMemoryRegionAllocationFlags allocationFlags = GraphicsMemoryRegionAllocationFlags.None, TexelFormat texelFormat = default)
    {
        var dxgiFormat = Map(texelFormat);

        var index = GetBlockCollectionIndex(cpuAccess, 1);
        var alignment = (ulong)D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;

        var resourceDesc = kind switch {
            GraphicsTextureKind.OneDimensional => D3D12_RESOURCE_DESC.Tex1D(dxgiFormat, width, mipLevels: 1, alignment: alignment),
            GraphicsTextureKind.TwoDimensional => D3D12_RESOURCE_DESC.Tex2D(dxgiFormat, width, height, mipLevels: 1, alignment: alignment),
            GraphicsTextureKind.ThreeDimensional => D3D12_RESOURCE_DESC.Tex3D(dxgiFormat, width, height, depth, mipLevels: 1, alignment: alignment),
            _ => default,
        };

        var resourceAllocationInfo = Device.D3D12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &resourceDesc);
        ref readonly var blockCollection = ref _blockCollections[index];

        var memoryBlockRegion = blockCollection.Allocate(resourceAllocationInfo.SizeInBytes, resourceAllocationInfo.Alignment, allocationFlags);
        return new D3D12GraphicsTexture<TMetadata>(Device, kind, in memoryBlockRegion, cpuAccess, width, height, depth);
    }

    /// <inheritdoc />
    public override void GetBudget(GraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget)
        => GetBudget((D3D12GraphicsMemoryBlockCollection)blockCollection, out budget);

    /// <inheritdoc cref="GetBudget(GraphicsMemoryBlockCollection, out GraphicsMemoryBudget)" />
    public void GetBudget(D3D12GraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget) => budget = new GraphicsMemoryBudget {
        EstimatedBudget = ulong.MaxValue,
        EstimatedUsage = 0,
        TotalAllocatedRegionSize = 0,
        TotalBlockSize = 0,
    };

    /// <inheritdoc />
    public override IEnumerator<D3D12GraphicsMemoryBlockCollection> GetEnumerator() => throw new NotImplementedException();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            foreach (var blockCollection in _blockCollections)
            {
                blockCollection?.Dispose();
            }
        }

        _state.EndDispose();
    }

    private int GetBlockCollectionIndex(GraphicsResourceCpuAccess cpuAccess, int kind)
    {
        var index = cpuAccess switch {
            GraphicsResourceCpuAccess.None => 0,        // DEFAULT
            GraphicsResourceCpuAccess.Read => 2,        // READBACK
            GraphicsResourceCpuAccess.Write => 1,       // UPLOAD
            _ => -1,
        };

        Assert(AssertionsEnabled && (index >= 0));

        if (!_supportsResourceHeapTier2)
        {
            // Scale to account for resource kind
            index *= 3;
            index += kind;
        }

        return index;
    }
}
