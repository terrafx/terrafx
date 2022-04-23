// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;

namespace TerraFX.Graphics.Advanced;

/// <inheritdoc />
public sealed unsafe class GraphicsMemoryHeap : GraphicsDeviceObject
{
    private readonly GraphicsMemoryManager _memoryManager;

    private ID3D12Heap* _d3d12Heap;
    private readonly uint _d3d12HeapVersion;

    private readonly nuint _byteLength;

    internal GraphicsMemoryHeap(GraphicsMemoryManager memoryManager, in GraphicsMemoryHeapCreateOptions createOptions) : base(memoryManager.Device)
    {
        _memoryManager = memoryManager;

        _d3d12Heap = CreateD3D12Heap(in createOptions, out _d3d12HeapVersion);

        _byteLength = createOptions.ByteLength;

        SetNameUnsafe(Name);

        ID3D12Heap* CreateD3D12Heap(in GraphicsMemoryHeapCreateOptions createOptions, out uint d3d12HeapVersion)
        {
            ID3D12Heap* d3d12Heap;

            var d3d12HeapDesc = new D3D12_HEAP_DESC(
                createOptions.ByteLength,
                createOptions.D3D12HeapType,
                GetAlignment(createOptions.D3D12HeapFlags),
                createOptions.D3D12HeapFlags
            );
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateHeap(&d3d12HeapDesc, __uuidof<ID3D12Heap>(), (void**)&d3d12Heap));

            return GetLatestD3D12Heap(d3d12Heap, out d3d12HeapVersion);
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

    /// <summary>Finalizes an instance of the <see cref="GraphicsMemoryHeap" /> class.</summary>
    ~GraphicsMemoryHeap() => Dispose(isDisposing: true);

    /// <summary>Gets the length, in bytes, of the memory heap.</summary>
    public nuint ByteLength => _byteLength;

    /// <summary>Gets the memory manager which created the memory heap.</summary>
    public GraphicsMemoryManager MemoryManager => _memoryManager;

    internal ID3D12Heap* D3D12Heap => _d3d12Heap;

    internal uint D3D12HeapVersion => _d3d12HeapVersion;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_d3d12Heap);
        _d3d12Heap = null;
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        _d3d12Heap->SetD3D12Name(value);
    }
}
