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
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsMemoryAllocator : GraphicsMemoryAllocator
{
    private readonly D3D12GraphicsMemoryHeapCollection[] _heapCollections;
    private readonly bool _supportsD3D12ResourceHeapTier2;

    private VolatileState _state;

    internal D3D12GraphicsMemoryAllocator(D3D12GraphicsDevice device, in GraphicsMemoryAllocatorSettings settings)
        : base(device, in settings)
    {
        var supportsD3D12ResourceHeapTier2 = Device.D3D12Options.ResourceHeapTier >= D3D12_RESOURCE_HEAP_TIER_2;

        _heapCollections = supportsD3D12ResourceHeapTier2
            ? new D3D12GraphicsMemoryHeapCollection[3] {
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_DEFAULT),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_UPLOAD),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_READBACK),
            }
            : new D3D12GraphicsMemoryHeapCollection[9] {
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                new D3D12GraphicsMemoryHeapCollection(Device, this, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
            };

        _supportsD3D12ResourceHeapTier2 = supportsD3D12ResourceHeapTier2;

        // TODO: UpdateBudget
        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsMemoryAllocator" /> class.</summary>
    ~D3D12GraphicsMemoryAllocator() => Dispose(isDisposing: true);

    /// <inheritdoc />
    public override int Count => _heapCollections.Length;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets <c>true</c> if <see cref="Device" /> supports <see cref="D3D12_RESOURCE_HEAP_TIER_2" />; otherwise, <c>false</c>.</summary>
    public bool SupportsD3D12ResourceHeapTier2 => _supportsD3D12ResourceHeapTier2;

    /// <inheritdoc />
    public override D3D12GraphicsBuffer CreateBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, GraphicsMemoryHeapRegionAllocationFlags allocationFlags = GraphicsMemoryHeapRegionAllocationFlags.None)
    {
        var heapCollectionIndex = GetHeapCollectionIndex(cpuAccess, 0);

        var d3d12ResourceDesc = D3D12_RESOURCE_DESC.Buffer(size, D3D12_RESOURCE_FLAG_NONE, D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT);
        var d3d12ResourceAllocationInfo = Device.D3D12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);
        ref readonly var heapCollection = ref _heapCollections[heapCollectionIndex];

        var heapRegion = heapCollection.Allocate(d3d12ResourceAllocationInfo.SizeInBytes, d3d12ResourceAllocationInfo.Alignment, allocationFlags);
        return new D3D12GraphicsBuffer(Device, kind, in heapRegion, cpuAccess);
    }

    /// <inheritdoc />
    public override D3D12GraphicsTexture CreateTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, GraphicsMemoryHeapRegionAllocationFlags allocationFlags = GraphicsMemoryHeapRegionAllocationFlags.None, TexelFormat texelFormat = default)
    {
        var dxgiFormat = Map(texelFormat);
        var heapCollectionIndex = GetHeapCollectionIndex(cpuAccess, 1);

        D3D12_RESOURCE_DESC d3d12ResourceDesc;

        switch (kind)
        {
            case GraphicsTextureKind.OneDimensional:
            {
                d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex1D(dxgiFormat, width, mipLevels: 1, alignment: D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT);
                break;
            }

            case GraphicsTextureKind.TwoDimensional:
            {
                d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex2D(dxgiFormat, width, height, mipLevels: 1, alignment: D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT);
                break;
            }

            case GraphicsTextureKind.ThreeDimensional:
            {
                d3d12ResourceDesc = D3D12_RESOURCE_DESC.Tex3D(dxgiFormat, width, height, depth, mipLevels: 1, alignment: D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT);
                break;
            }

            default:
            {
                ThrowForInvalidKind(kind);
                break;
            }
        }

        var d3d12ResourceAllocationInfo = Device.D3D12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);
        ref readonly var heapCollection = ref _heapCollections[heapCollectionIndex];

        var heapRegion = heapCollection.Allocate(d3d12ResourceAllocationInfo.SizeInBytes, d3d12ResourceAllocationInfo.Alignment, allocationFlags);
        return new D3D12GraphicsTexture(Device, kind, in heapRegion, cpuAccess, width, height, depth);
    }

    /// <inheritdoc />
    public override void GetBudget(GraphicsMemoryHeapCollection heapCollection, out GraphicsMemoryBudget budget)
        => GetBudget((D3D12GraphicsMemoryHeapCollection)heapCollection, out budget);

    /// <inheritdoc cref="GetBudget(GraphicsMemoryHeapCollection, out GraphicsMemoryBudget)" />
    public void GetBudget(D3D12GraphicsMemoryHeapCollection heapCollection, out GraphicsMemoryBudget budget) => budget = new GraphicsMemoryBudget {
        EstimatedBudget = ulong.MaxValue,
        EstimatedUsage = 0,
        TotalAllocatedRegionSize = 0,
        TotalHeapSize = 0,
    };

    /// <inheritdoc />
    public override IEnumerator<D3D12GraphicsMemoryHeapCollection> GetEnumerator() => throw new NotImplementedException();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                foreach (var heapCollection in _heapCollections)
                {
                    heapCollection?.Dispose();
                }
            }
        }

        _state.EndDispose();
    }

    private int GetHeapCollectionIndex(GraphicsResourceCpuAccess cpuAccess, int kind)
    {
        var heapCollectionIndex = cpuAccess switch {
            GraphicsResourceCpuAccess.None => 0,        // DEFAULT
            GraphicsResourceCpuAccess.Read => 2,        // READBACK
            GraphicsResourceCpuAccess.Write => 1,       // UPLOAD
            _ => -1,
        };

        Assert(AssertionsEnabled && (heapCollectionIndex >= 0));

        if (!SupportsD3D12ResourceHeapTier2)
        {
            // Scale to account for resource kind
            heapCollectionIndex *= 3;
            heapCollectionIndex += kind;
        }

        return heapCollectionIndex;
    }
}
