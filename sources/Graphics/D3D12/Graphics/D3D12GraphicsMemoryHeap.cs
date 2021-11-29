// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsMemoryHeap : GraphicsMemoryHeap
{
    private readonly ID3D12Heap* _d3d12Heap;

    private VolatileState _state;

    internal D3D12GraphicsMemoryHeap(D3D12GraphicsDevice device, D3D12GraphicsMemoryHeapCollection collection, ulong size)
        : base(device, collection, size)
    {
        _d3d12Heap = CreateD3D12Heap(device, collection, size);

        _ = _state.Transition(to: Initialized);

        static ID3D12Heap* CreateD3D12Heap(D3D12GraphicsDevice device, D3D12GraphicsMemoryHeapCollection collection, ulong size)
        {
            ID3D12Heap* d3d12Heap;

            var d3d12Device = device.D3D12Device;
            var d3d12HeapFlags = collection.D3D12HeapFlags;
            var d3d12HeapType = collection.D3D12HeapType;
            var d3d12HeapDesc = new D3D12_HEAP_DESC(size, d3d12HeapType, GetAlignment(d3d12HeapFlags), d3d12HeapFlags);

            ThrowExternalExceptionIfFailed(d3d12Device->CreateHeap(&d3d12HeapDesc, __uuidof<ID3D12Heap>(), (void**)&d3d12Heap));
            return d3d12Heap;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsMemoryHeap" /> class.</summary>
    ~D3D12GraphicsMemoryHeap() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsMemoryHeap.Collection" />
    public new D3D12GraphicsMemoryHeapCollection Collection => base.Collection.As<D3D12GraphicsMemoryHeapCollection>();

    /// <summary>Gets the <see cref="ID3D12Heap" /> for the memory heap.</summary>
    public ID3D12Heap* D3D12Heap
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12Heap;
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    private static ulong GetAlignment(D3D12_HEAP_FLAGS heapFlags)
    {
        const D3D12_HEAP_FLAGS DenyAllTexturesFlags = D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES;
        var canContainAnyTextures = (heapFlags & DenyAllTexturesFlags) != DenyAllTexturesFlags;

        var alignment = canContainAnyTextures ? D3D12_DEFAULT_MSAA_RESOURCE_PLACEMENT_ALIGNMENT : D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;
        return (ulong)alignment;
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            ReleaseIfNotNull(_d3d12Heap);
        }

        _state.EndDispose();
    }
}
