// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the GpuResource class from https://github.com/microsoft/DirectX-Graphics-Samples
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Threading;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.D3D12_TEXTURE_LAYOUT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics resource bound to a graphics device.</summary>
public abstract unsafe class GraphicsResource : GraphicsDeviceObject
{
    private readonly GraphicsCpuAccess _cpuAccess;

    private readonly D3D12_RESOURCE_STATES _d3d12DefaultResourceState;

    private ComPtr<ID3D12Resource> _d3d12Resource;
    private readonly uint _d3d12ResourceVersion;

    private readonly GraphicsResourceKind _kind;

    private volatile void* _mappedAddress;
    private volatile uint _mappedCount;
    private readonly ValueMutex _mappedMutex;

    private GraphicsMemoryHeap _memoryHeap;
    private readonly GraphicsMemoryRegion _memoryRegion;

    private protected GraphicsResource(GraphicsDevice device, in GraphicsBufferCreateOptions createOptions) : this(device, GraphicsResourceKind.Buffer, createOptions.CpuAccess)
    {
        ID3D12Resource* d3d12Resource;

        var d3d12ResourceDesc = D3D12_RESOURCE_DESC.Buffer(
            createOptions.ByteLength,
            D3D12_RESOURCE_FLAG_NONE,
            D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT
        );

        var d3d12Device = device.D3D12Device;
        var d3d12ResourceAllocationInfo = d3d12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);

        var allocationOptions = new GraphicsMemoryAllocationOptions {
            ByteLength = (nuint)d3d12ResourceAllocationInfo.SizeInBytes,
            ByteAlignment = (nuint)d3d12ResourceAllocationInfo.Alignment,
            AllocationFlags = createOptions.AllocationFlags,
        };

        var memoryManager = device.GetMemoryManager(_cpuAccess, GraphicsResourceKind.Buffer);
        var memoryRegion = memoryManager.Allocate(in allocationOptions);
        var memoryHeap = memoryRegion.MemoryAllocator.DeviceObject.As<GraphicsMemoryHeap>();

        ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
            memoryHeap.D3D12Heap,
            memoryRegion.ByteOffset,
            &d3d12ResourceDesc,
            _d3d12DefaultResourceState,
            pOptimizedClearValue: null,
            __uuidof<ID3D12Resource>(),
            (void**)&d3d12Resource
        ));

        d3d12Resource = GetLatestD3D12Resource(d3d12Resource, out _d3d12ResourceVersion);
        _d3d12Resource.Attach(d3d12Resource);

        _memoryRegion = memoryRegion;
        _memoryHeap = memoryHeap;

        SetNameUnsafe(Name);
    }

    private protected GraphicsResource(GraphicsDevice device, in GraphicsTextureCreateOptions createOptions) : this(device, GraphicsResourceKind.Texture, createOptions.CpuAccess)
    {
        ID3D12Resource* d3d12Resource;

        var d3d12ResourceDesc = createOptions.Kind switch {
            GraphicsTextureKind.OneDimensional => D3D12_RESOURCE_DESC.Tex1D(
                createOptions.PixelFormat.AsDxgiFormat(),
                createOptions.PixelWidth,
                arraySize: 1,
                (createOptions.MipLevelCount != 0) ? createOptions.MipLevelCount : (ushort)1,
                D3D12_RESOURCE_FLAG_NONE,
                D3D12_TEXTURE_LAYOUT_UNKNOWN,
                D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT
            ),
            GraphicsTextureKind.TwoDimensional => D3D12_RESOURCE_DESC.Tex2D(
                createOptions.PixelFormat.AsDxgiFormat(),
                createOptions.PixelWidth,
                createOptions.PixelHeight,
                arraySize: 1,
                (createOptions.MipLevelCount != 0) ? createOptions.MipLevelCount : (ushort)1,
                sampleCount: 1,
                sampleQuality: 0,
                D3D12_RESOURCE_FLAG_NONE,
                D3D12_TEXTURE_LAYOUT_UNKNOWN,
                D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT
            ),
            GraphicsTextureKind.ThreeDimensional => D3D12_RESOURCE_DESC.Tex3D(
                createOptions.PixelFormat.AsDxgiFormat(),
                createOptions.PixelWidth,
                createOptions.PixelHeight,
                createOptions.PixelDepth,
                (createOptions.MipLevelCount != 0) ? createOptions.MipLevelCount : (ushort)1,
                D3D12_RESOURCE_FLAG_NONE,
                D3D12_TEXTURE_LAYOUT_UNKNOWN,
                D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT
            ),
            _ => default,
        };

        var d3d12Device = device.D3D12Device;
        var d3d12ResourceAllocationInfo = d3d12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);

        var memoryAllocationOptions = new GraphicsMemoryAllocationOptions {
            ByteLength = (nuint)d3d12ResourceAllocationInfo.SizeInBytes,
            ByteAlignment = (nuint)d3d12ResourceAllocationInfo.Alignment,
            AllocationFlags = createOptions.AllocationFlags,
        };

        var memoryManager = device.GetMemoryManager(createOptions.CpuAccess, GraphicsResourceKind.Texture);
        var memoryRegion = memoryManager.Allocate(in memoryAllocationOptions);
        var memoryHeap = memoryRegion.MemoryAllocator.DeviceObject.As<GraphicsMemoryHeap>();

        ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
            memoryHeap.D3D12Heap,
            memoryRegion.ByteOffset,
            &d3d12ResourceDesc,
            _d3d12DefaultResourceState,
            pOptimizedClearValue: null,
            __uuidof<ID3D12Resource>(),
            (void**)&d3d12Resource
        ));

        d3d12Resource = GetLatestD3D12Resource(d3d12Resource, out _d3d12ResourceVersion);
        _d3d12Resource.Attach(d3d12Resource);

        _memoryRegion = memoryRegion;
        _memoryHeap = memoryHeap;

        SetNameUnsafe(Name);
    }

    private GraphicsResource(GraphicsDevice device, GraphicsResourceKind kind, GraphicsCpuAccess cpuAccess) : base(device)
    {
        _cpuAccess = cpuAccess;

        _d3d12DefaultResourceState = cpuAccess switch {
            GraphicsCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
            GraphicsCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
            _ => D3D12_RESOURCE_STATE_COMMON,
        };

        _kind = kind;

        _mappedAddress = null;
        _mappedCount = 0;
        _mappedMutex = new ValueMutex();

        _memoryHeap = null!;
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsResource" /> class.</summary>
    ~GraphicsResource() => Dispose(isDisposing: false);

    /// <summary>Gets the length, in bytes, of the resource.</summary>
    public nuint ByteLength => _memoryRegion.ByteLength;

    /// <summary>Gets the CPU access capabilities of the resource.</summary>
    public GraphicsCpuAccess CpuAccess => _cpuAccess;

    /// <summary>Gets <c>true</c> if the resource is mapped; otherwise, <c>false</c>.</summary>
    public bool IsMapped => _mappedAddress is not null;

    /// <summary>Gets the resource kind.</summary>
    public GraphicsResourceKind Kind => _kind;

    /// <summary>Gets the mapped address of the resource or <c>null</c> if the resource is not currently mapped.</summary>
    public void* MappedAddress => _mappedAddress;

    /// <summary>Gets the memory heap in which the resource exists.</summary>
    public GraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <summary>Gets the memory region in which the resource exists.</summary>
    public ref readonly GraphicsMemoryRegion MemoryRegion => ref _memoryRegion;

    internal D3D12_RESOURCE_STATES D3D12DefaultResourceState => _d3d12DefaultResourceState;

    internal ID3D12Resource* D3D12Resource => _d3d12Resource;

    internal uint D3D12ResourceVersion => _d3d12ResourceVersion;

    /// <summary>Maps the resource into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource.</returns>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>()
        where T : unmanaged
    {
        ThrowIfDisposed();

        var mappedAddress = MapUnsafe(subresource: 0);
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        var mappedAddress = MapUnsafe(subresource: 0);
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer into CPU memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public UnmanagedSpan<T> Map<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        var mappedAddress = MapUnsafe(subresource: 0);
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the resource into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the resource.</typeparam>
    /// <returns>An unmanaged span that represents the mapped resource.</returns>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>()
        where T : unmanaged
    {
        ThrowIfDisposed();

        var mappedAddress = MapForReadUnsafe(subresource: 0);
        return new UnmanagedSpan<T>((T*)mappedAddress, ByteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        var mappedAddress = MapForReadUnsafe(subresource: 0, byteStart, byteLength);
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Maps the buffer into CPU memory for reading.</summary>
    /// <typeparam name="T">The type of data contained by the buffer.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <returns>An unmanaged span that represents the mapped buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public UnmanagedSpan<T> MapForRead<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        var mappedAddress = MapForReadUnsafe(subresource: 0);
        return new UnmanagedSpan<T>((T*)(mappedAddress + byteStart), byteLength / SizeOf<T>());
    }

    /// <summary>Unmaps the resource from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public void Unmap()
    {
        ThrowIfDisposed();
        UnmapUnsafe(subresource: 0);
    }

    /// <summary>Unmaps the resource from CPU memory and writes the entire mapped region.</summary>
    /// <remarks>This overload should be used when all memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The resource is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The resource has been disposed.</exception>
    public void UnmapAndWrite()
    {
        ThrowIfDisposed();
        UnmapAndWriteUnsafe(subresource: 0);
    }

    /// <summary>Unmaps the buffer from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public void UnmapAndWrite<T>(nuint start)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = ByteLength - byteStart;

        UnmapAndWriteUnsafe(subresource: 0, byteStart, byteLength);
    }

    /// <summary>Unmaps the buffer from CPU memory and writes a region of memory.</summary>
    /// <typeparam name="T">The type of data contained by the buffer view.</typeparam>
    /// <param name="start">The index of the first element at which which the mapping should start.</param>
    /// <param name="length">The number of elements in the mapping.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="start" /> * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">(<paramref name="length" /> + <paramref name="start" />) * sizeof(<typeparamref name="T" />) is greater than <see cref="GraphicsResource.ByteLength" />.</exception>
    /// <exception cref="InvalidOperationException">The buffer is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
    public void UnmapAndWrite<T>(nuint start, nuint length)
        where T : unmanaged
    {
        ThrowIfDisposed();

        var byteStart = SizeOf<T>() * start;
        ThrowIfNotInBounds(byteStart, ByteLength);

        var byteLength = SizeOf<T>() * length;
        ThrowIfNotInInsertBounds(byteLength, ByteLength - byteStart);

        UnmapAndWriteUnsafe(subresource: 0);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _mappedAddress = null;
        _mappedCount = 0;
        _mappedMutex.Dispose();

        _memoryHeap = null!;
        _memoryRegion.Dispose();

        _ = _d3d12Resource.Reset();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value) => D3D12Resource->SetD3D12Name(value);

    internal byte* MapForReadUnsafe(uint subresource)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(subresource);

        byte* MapForReadNoMutex(uint subresource)
        {
            _ = Interlocked.Increment(ref _mappedCount);

            void* mappedAddress;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(subresource, pReadRange: null, &mappedAddress));

            _mappedAddress = mappedAddress;
            return (byte*)mappedAddress;
        }
    }

    internal byte* MapForReadUnsafe(uint subresource, nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(subresource, byteStart, byteLength);

        byte* MapForReadNoMutex(uint subresource, nuint byteStart, nuint byteLength)
        {
            _ = Interlocked.Increment(ref _mappedCount);

            var readRange = new D3D12_RANGE {
                Begin = byteStart,
                End = byteStart + byteLength,
            };

            void* mappedAddress;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(subresource, &readRange, &mappedAddress));

            _mappedAddress = mappedAddress;
            return (byte*)mappedAddress;
        }
    }

    internal byte* MapUnsafe(uint subresource)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapNoMutex(subresource);

        byte* MapNoMutex(uint subresource)
        {
            _ = Interlocked.Increment(ref _mappedCount);
            var readRange = default(D3D12_RANGE);

            void* mappedAddress;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(subresource, &readRange, &mappedAddress));

            _mappedAddress = mappedAddress;
            return (byte*)mappedAddress;
        }
    }

    internal void UnmapAndWriteUnsafe(uint subresource)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(subresource);

        void UnmapAndWriteNoMutex(uint subresource)
        {
            var mappedCount = Interlocked.Decrement(ref _mappedCount);

            if (mappedCount == uint.MaxValue)
            {
                ThrowForInvalidState(nameof(IsMapped));
            }
            else if (mappedCount == 0)
            {
                _mappedAddress = null;
            }

            D3D12Resource->Unmap(subresource, null);
        }
    }

    internal void UnmapAndWriteUnsafe(uint subresource, nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(subresource, byteStart, byteLength);

        void UnmapAndWriteNoMutex(uint subresource, nuint byteStart, nuint byteLength)
        {
            var mappedCount = Interlocked.Decrement(ref _mappedCount);

            if (mappedCount == uint.MaxValue)
            {
                ThrowForInvalidState(nameof(IsMapped));
            }
            else if (mappedCount == 0)
            {
                _mappedAddress = null;
            }

            var writtenRange = new D3D12_RANGE {
                Begin = byteStart,
                End = byteStart + byteLength,
            };
            D3D12Resource->Unmap(subresource, &writtenRange);
        }
    }

    internal void UnmapUnsafe(uint subresource)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapNoMutex(subresource);

        void UnmapNoMutex(uint subresource)
        {
            var mappedCount = Interlocked.Decrement(ref _mappedCount);

            if (mappedCount == uint.MaxValue)
            {
                ThrowForInvalidState(nameof(IsMapped));
            }
            else if (mappedCount == 0)
            {
                _mappedAddress = null;
            }

            var writtenRange = default(D3D12_RANGE);
            D3D12Resource->Unmap(subresource, &writtenRange);
        }
    }
}
