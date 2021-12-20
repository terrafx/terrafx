// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsMemoryHeap : GraphicsDeviceObject
{
    private readonly ID3D12Heap* _d3d12Heap;
    private readonly D3D12_HEAP_DESC _d3d12HeapDesc;
    private readonly D3D12GraphicsMemoryManager _memoryManager;
    private readonly nuint _size;

    internal D3D12GraphicsMemoryHeap(D3D12GraphicsMemoryManager memoryManager, nuint size, D3D12_HEAP_TYPE d3d12HeapType, D3D12_HEAP_FLAGS d3d12HeapFlags)
        : base(memoryManager.Device)
    {
        _d3d12Heap = CreateD3D12Heap(memoryManager.Device, size, d3d12HeapType, d3d12HeapFlags);
        _d3d12HeapDesc = _d3d12Heap->GetDesc();
        _memoryManager = memoryManager;
        _size = size;

        static ID3D12Heap* CreateD3D12Heap(D3D12GraphicsDevice device, nuint size, D3D12_HEAP_TYPE d3d12HeapType, D3D12_HEAP_FLAGS d3d12HeapFlags)
        {
            ID3D12Heap* d3d12Heap;

            var d3d12HeapDesc = new D3D12_HEAP_DESC(size, d3d12HeapType, GetAlignment(d3d12HeapFlags), d3d12HeapFlags);
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateHeap(&d3d12HeapDesc, __uuidof<ID3D12Heap>(), (void**)&d3d12Heap));

            return d3d12Heap;
        }

        static nuint GetAlignment(D3D12_HEAP_FLAGS heapFlags)
        {
            const D3D12_HEAP_FLAGS DenyAllTexturesFlags = D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES;
            var canContainAnyTextures = (heapFlags & DenyAllTexturesFlags) != DenyAllTexturesFlags;

            if (canContainAnyTextures)
            {
                return D3D12_DEFAULT_MSAA_RESOURCE_PLACEMENT_ALIGNMENT;
            }
            else
            {
                return D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;
            }
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsMemoryHeap" /> class.</summary>
    ~D3D12GraphicsMemoryHeap() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="ID3D12Heap" /> for the memory heap.</summary>
    public ID3D12Heap* D3D12Heap
    {
        get
        {
            AssertNotDisposed();
            return _d3d12Heap;
        }
    }

    /// <summary>Gets the <see cref="D3D12_HEAP_DESC" /> for <see cref="D3D12Heap" />.</summary>
    public ref readonly D3D12_HEAP_DESC D3D12HeapDesc => ref _d3d12HeapDesc;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets the memory manager which created the memory heap.</summary>
    public D3D12GraphicsMemoryManager MemoryManager => _memoryManager;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override void SetName(string value)
    {
        value = D3D12Heap->UpdateD3D12Name(value);
        base.SetName(value);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_d3d12Heap);
    }
}
