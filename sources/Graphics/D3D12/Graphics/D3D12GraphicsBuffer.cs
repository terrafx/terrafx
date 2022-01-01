// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsBuffer : GraphicsBuffer
{
    private readonly D3D12_RESOURCE_STATES _defaultResourceState;

    private ID3D12Resource* _d3d12Resource;
    private readonly uint _d3d12ResourceVersion;

    private ulong _gpuVirtualAddress;

    private GraphicsMemoryAllocator _memoryAllocator;
    private D3D12GraphicsMemoryHeap _memoryHeap;

    private ValueList<D3D12GraphicsBufferView> _bufferViews;
    private readonly ValueMutex _bufferViewsMutex;

    private volatile uint _mappedCount;
    private readonly ValueMutex _mappedMutex;

    internal D3D12GraphicsBuffer(D3D12GraphicsDevice device, in GraphicsBufferCreateOptions createOptions) : base(device)
    {
        device.AddBuffer(this);

        BufferInfo.Kind = createOptions.Kind;

        _defaultResourceState = createOptions.CpuAccess switch {
            GraphicsCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
            GraphicsCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
            _ => D3D12_RESOURCE_STATE_COMMON,
        };

        _d3d12Resource = CreateD3D12Resource(in createOptions, out _d3d12ResourceVersion);
        _gpuVirtualAddress = _d3d12Resource->GetGPUVirtualAddress();

        var memoryAllocatorCreateOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = MemoryRegion.ByteLength,
            IsDedicated = false,
            OnFree = default,
        };

        if (createOptions.CreateMemorySuballocator.IsNotNull)
        {
            _memoryAllocator = createOptions.CreateMemorySuballocator.Invoke(this, in memoryAllocatorCreateOptions);
        }
        else
        {
            _memoryAllocator = GraphicsMemoryAllocator.CreateDefault(this, in memoryAllocatorCreateOptions);
        }

        _memoryHeap = MemoryRegion.MemoryAllocator.DeviceObject.As<D3D12GraphicsMemoryHeap>();

        _bufferViews = new ValueList<D3D12GraphicsBufferView>();
        _bufferViewsMutex = new ValueMutex();

        _mappedCount = 0;
        _mappedMutex = new ValueMutex();

        SetNameUnsafe(Name);

        ID3D12Resource* CreateD3D12Resource(in GraphicsBufferCreateOptions createOptions, out uint d3d12ResourceVersion)
        {
            ID3D12Resource* d3d12Resource;

            var d3d12Device = device.D3D12Device;
            var cpuAccess = createOptions.CpuAccess;

            var d3d12ResourceDesc = D3D12_RESOURCE_DESC.Buffer(
                AlignUp(createOptions.ByteLength, D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT),
                D3D12_RESOURCE_FLAG_NONE,
                D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT
            );
            var d3d12ResourceAllocationInfo = d3d12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);

            ResourceInfo.CpuAccess = cpuAccess;
            ResourceInfo.MappedAddress = null;

            var memoryManager = device.GetMemoryManager(cpuAccess, GraphicsResourceKind.Buffer);

            var allocationOptions = new GraphicsMemoryAllocationOptions {
                ByteLength = (nuint)d3d12ResourceAllocationInfo.SizeInBytes,
                ByteAlignment = (nuint)d3d12ResourceAllocationInfo.Alignment,
                AllocationFlags = createOptions.AllocationFlags,
            };
            var memoryRegion = memoryManager.Allocate(in allocationOptions);
            ResourceInfo.MemoryRegion = memoryRegion;

            var d3d12Heap = ResourceInfo.MemoryRegion.MemoryAllocator.DeviceObject.As<D3D12GraphicsMemoryHeap>().D3D12Heap;
            ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
                d3d12Heap,
                memoryRegion.ByteOffset,
                &d3d12ResourceDesc,
                _defaultResourceState,
                pOptimizedClearValue: null,
                __uuidof<ID3D12Resource>(),
                (void**)&d3d12Resource
            ));

            return GetLatestD3D12Resource(d3d12Resource, out d3d12ResourceVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBuffer" /> class.</summary>
    ~D3D12GraphicsBuffer() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the buffer.</summary>
    public ID3D12Resource* D3D12Resource => _d3d12Resource;

    /// <summary>Gets the interface version of <see cref="D3D12Resource" />.</summary>
    public uint D3D12ResourceVersion => _d3d12ResourceVersion;

    /// <summary>Gets the default resource state for <see cref="D3D12Resource" />.</summary>
    public D3D12_RESOURCE_STATES DefaultResourceState => _defaultResourceState;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets the GPU virtual address for <see cref="D3D12Resource" />.</summary>
    public ulong GpuVirtualAddress => _gpuVirtualAddress;

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public D3D12GraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            DisposeAllViewsUnsafe();
            _bufferViews.Clear();

            _memoryAllocator.Clear();

            _memoryAllocator = null!;
            _memoryHeap = null!;
        }
        _bufferViewsMutex.Dispose();

        ReleaseIfNotNull(_d3d12Resource);
        _d3d12Resource = null;

        _gpuVirtualAddress = 0;

        ResourceInfo.MappedAddress = null;
        _mappedCount = 0;
        _mappedMutex.Dispose();

        ResourceInfo.MemoryRegion.Dispose();

        _ = Device.RemoveBuffer(this);
    }

    /// <inheritdoc />
    protected override void DisposeAllViewsUnsafe()
    {
        for (var index = _bufferViews.Count - 1; index >= 0; index--)
        {
            var bufferView = _bufferViews.GetReferenceUnsafe(index);
            bufferView.Dispose();
        }
    }

    /// <inheritdoc />
    protected override byte* MapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapNoMutex();
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex();
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(byteStart, byteLength);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12Resource->SetD3D12Name(value);
    }

    /// <inheritdoc />
    protected override bool TryCreateBufferViewUnsafe(in GraphicsBufferViewCreateOptions createOptions, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        nuint byteLength = createOptions.BytesPerElement;
        byteLength *= createOptions.ElementCount;
        nuint byteAlignment = 0;

        if (Kind == GraphicsBufferKind.Index)
        {
            byteAlignment = createOptions.BytesPerElement;
            byteLength = AlignUp(byteLength, createOptions.BytesPerElement);
        }
        else if (Kind == GraphicsBufferKind.Constant)
        {
            byteAlignment = D3D12_CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT;
            byteLength = AlignUp(byteLength, D3D12_CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT);
        }
        else if (Kind == GraphicsBufferKind.Default)
        {
            byteAlignment = D3D12_TEXTURE_DATA_PLACEMENT_ALIGNMENT;
        }

        var succeeded = _memoryAllocator.TryAllocate(byteLength, byteAlignment, out var memoryRegion);
        bufferView = succeeded ? new D3D12GraphicsBufferView(this, in createOptions, in memoryRegion) : null;

        return succeeded;
    }

    /// <inheritdoc />
    protected override void UnmapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapNoMutex();
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex();
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(byteStart, byteLength);
    }

    internal void AddBufferView(D3D12GraphicsBufferView bufferView)
    {
        _bufferViews.Add(bufferView, _bufferViewsMutex);
    }

    internal byte* MapView(nuint byteStart)
    {
        return MapUnsafe() + byteStart;
    }

    internal byte* MapViewForRead(nuint byteStart, nuint byteLength)
    {
        return MapForReadUnsafe(byteStart, byteLength) + byteStart;
    }

    internal bool RemoveBufferView(D3D12GraphicsBufferView bufferView)
    {
        return IsDisposed || _bufferViews.Remove(bufferView, _bufferViewsMutex);
    }

    internal void UnmapView()
    {
        UnmapUnsafe();
    }

    internal void UnmapViewAndWrite(nuint byteStart, nuint byteLength)
    {
        UnmapAndWriteUnsafe(byteStart, byteLength);
    }

    private byte* MapNoMutex()
    {
        _ = Interlocked.Increment(ref _mappedCount);
        var readRange = default(D3D12_RANGE);

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, &mappedAddress));

        ResourceInfo.MappedAddress = mappedAddress;
        return (byte*)mappedAddress;
    }

    private byte* MapForReadNoMutex()
    {
        _ = Interlocked.Increment(ref _mappedCount);

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, pReadRange: null, &mappedAddress));

        ResourceInfo.MappedAddress = mappedAddress;
        return (byte*)mappedAddress;
    }

    private byte* MapForReadNoMutex(nuint byteStart, nuint byteLength)
    {
        _ = Interlocked.Increment(ref _mappedCount);

        var readRange = new D3D12_RANGE {
            Begin = byteStart,
            End = byteStart + byteLength,
        };

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, &mappedAddress));

        ResourceInfo.MappedAddress = mappedAddress;
        return (byte*)mappedAddress;
    }

    private void UnmapNoMutex()
    {
        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        else if (mappedCount == 0)
        {
            ResourceInfo.MappedAddress = null;
        }

        var writtenRange = default(D3D12_RANGE);
        D3D12Resource->Unmap(Subresource: 0, &writtenRange);
    }

    private void UnmapAndWriteNoMutex()
    {
        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        else if (mappedCount == 0)
        {
            ResourceInfo.MappedAddress = null;
        }

        D3D12Resource->Unmap(Subresource: 0, null);
    }

    private void UnmapAndWriteNoMutex(nuint byteStart, nuint byteLength)
    {
        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        else if (mappedCount == 0)
        {
            ResourceInfo.MappedAddress = null;
        }

        var writtenRange = new D3D12_RANGE {
            Begin = byteStart,
            End = byteStart + byteLength,
        };
        D3D12Resource->Unmap(Subresource: 0, &writtenRange);
    }
}
