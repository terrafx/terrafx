// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop.DirectX;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed class D3D12GraphicsMemoryHeapCollection : GraphicsMemoryHeapCollection
{
    private readonly D3D12_HEAP_FLAGS _d3d12HeapFlags;
    private readonly D3D12_HEAP_TYPE _d3d12HeapType;

    internal D3D12GraphicsMemoryHeapCollection(D3D12GraphicsDevice device, D3D12GraphicsMemoryAllocator allocator, D3D12_HEAP_FLAGS d3d12HeapFlags, D3D12_HEAP_TYPE d3d12HeapType)
        : base(device, allocator)
    {
        _d3d12HeapFlags = d3d12HeapFlags;
        _d3d12HeapType = d3d12HeapType;
    }

    /// <inheritdoc cref="GraphicsMemoryHeapCollection.Allocator" />
    public new D3D12GraphicsMemoryAllocator Allocator => (D3D12GraphicsMemoryAllocator)base.Allocator;

    /// <summary>Gets the heap flags used when creating the <see cref="ID3D12Heap" /> instance for a memory heap.</summary>
    public D3D12_HEAP_FLAGS D3D12HeapFlags => _d3d12HeapFlags;

    /// <summary>Gets the heap type used when creating the <see cref="ID3D12Heap" /> instance for a memory heap.</summary>
    public D3D12_HEAP_TYPE D3D12HeapType => _d3d12HeapType;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

    /// <inheritdoc />
    protected override D3D12GraphicsMemoryHeap CreateHeap(ulong size) => new D3D12GraphicsMemoryHeap(Device, this, size);
}
