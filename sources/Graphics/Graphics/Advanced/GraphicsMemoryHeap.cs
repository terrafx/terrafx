// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;

namespace TerraFX.Graphics.Advanced;

/// <inheritdoc />
public sealed unsafe class GraphicsMemoryHeap : IDisposable, INameable
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsMemoryManager _memoryManager;
    private readonly GraphicsService _service;

    private ID3D12Heap* _d3d12Heap;
    private readonly uint _d3d12HeapVersion;

    private readonly nuint _byteLength;

    private string _name;
    private VolatileState _state;

    internal GraphicsMemoryHeap(GraphicsMemoryManager memoryManager, in GraphicsMemoryHeapCreateOptions createOptions)
    {
        AssertNotNull(memoryManager);
        _memoryManager = memoryManager;

        var device = memoryManager.Device;
        _device = device;

        var adapter = device.Adapter;
        _adapter = adapter;

        var service = adapter.Service;
        _service = service;

        _d3d12Heap = CreateD3D12Heap(in createOptions, out _d3d12HeapVersion);

        _byteLength = createOptions.ByteLength;

        _name = GetType().Name;
        SetNameUnsafe(Name);
        _ = _state.Transition(VolatileState.Initialized);

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
            return ((heapFlags & DenyAllTexturesFlags) != DenyAllTexturesFlags) ? D3D12_DEFAULT_MSAA_RESOURCE_PLACEMENT_ALIGNMENT : (nuint)D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsMemoryHeap" /> class.</summary>
    ~GraphicsMemoryHeap() => Dispose(isDisposing: true);

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the length, in bytes, of the memory heap.</summary>
    public nuint ByteLength => _byteLength;

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <summary>Gets the memory manager which created the memory heap.</summary>
    public GraphicsMemoryManager MemoryManager => _memoryManager;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
            SetNameUnsafe(_name);
        }
    }

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    internal ID3D12Heap* D3D12Heap => _d3d12Heap;

    internal uint D3D12HeapVersion => _d3d12HeapVersion;

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            // Nothing to handle
        }

        ReleaseIfNotNull(_d3d12Heap);
        _d3d12Heap = null;
    }

    private void SetNameUnsafe(string value) => _d3d12Heap->SetD3D12Name(value);
}
