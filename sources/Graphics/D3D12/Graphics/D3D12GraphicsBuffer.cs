// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsBuffer : GraphicsBuffer
{
    private readonly ValueList<D3D12GraphicsBufferView> _bufferViews;
    private readonly ValueMutex _bufferViewsMutex;
    private readonly ID3D12Resource* _d3d12Resource;
    private readonly D3D12_RESOURCE_DESC _d3d12ResourceDesc;
    private readonly ulong _d3d12ResourceGpuVirtualAddress;
    private readonly D3D12_RESOURCE_STATES _d3d12ResourceState;
    private readonly ValueMutex _mapMutex;
    private readonly GraphicsMemoryAllocator _memoryAllocator;
    private readonly D3D12GraphicsMemoryHeap _memoryHeap;

    private volatile void* _mappedAddress;
    private volatile uint _mappedCount;

    private VolatileState _state;

    internal D3D12GraphicsBuffer(D3D12GraphicsDevice device, in CreateInfo createInfo)
        : base(device, in createInfo.MemoryRegion, createInfo.CpuAccess, createInfo.Kind)
    {
        var d3d12Resource = createInfo.D3D12Resource;

        _bufferViewsMutex = new ValueMutex();
        _d3d12Resource = d3d12Resource;
        _d3d12ResourceDesc = d3d12Resource->GetDesc();
        _d3d12ResourceGpuVirtualAddress = d3d12Resource->GetGPUVirtualAddress();
        _d3d12ResourceState = createInfo.D3D12ResourceState;
        _mapMutex = new ValueMutex();
        _memoryAllocator = createInfo.CreateMemoryAllocator(this, null, createInfo.MemoryRegion.Size, false);
        _memoryHeap = createInfo.MemoryRegion.Allocator.DeviceObject.As<D3D12GraphicsMemoryHeap>();

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBuffer" /> class.</summary>
    ~D3D12GraphicsBuffer() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc />
    public override int Count => _bufferViews.Count;

    /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the buffer.</summary>
    public ID3D12Resource* D3D12Resource
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12Resource;
        }
    }

    /// <summary>Gets the <see cref="D3D12_RESOURCE_DESC" /> for <see cref="D3D12Resource" />.</summary>
    public ref readonly D3D12_RESOURCE_DESC D3D12ResourceDesc => ref _d3d12ResourceDesc;

    /// <summary>Gets the GPU virtual address for <see cref="D3D12Resource" />.</summary>
    public ulong D3D12ResourceGpuVirtualAddress => _d3d12ResourceGpuVirtualAddress;

    /// <summary>Gets the default resource state for <see cref="D3D12Resource" />.</summary>
    public D3D12_RESOURCE_STATES D3D12ResourceState => _d3d12ResourceState;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc />
    public override bool IsMapped => _mappedCount != 0;

    /// <inheritdoc />
    public override unsafe void* MappedAddress => _mappedAddress;

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public D3D12GraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override void DisposeAllViews()
    {
        using var mutex = new DisposableMutex(_bufferViewsMutex, isExternallySynchronized: false);
        DisposeAllViewsInternal();
    }

    /// <inheritdoc />
    public override IEnumerator<D3D12GraphicsBufferView> GetEnumerator() => _bufferViews.GetEnumerator();

    /// <inheritdoc />
    public override void SetName(string value)
    {
        value = D3D12Resource->UpdateD3D12Name(value);
        base.SetName(value);
    }

    /// <inheritdoc />
    public override bool TryCreateView(uint count, uint stride, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsBuffer));

        nuint size = stride;
        size *= count;
        nuint alignment = 0;

        if (Kind == GraphicsBufferKind.Index)
        {
            if (stride is not 2 and not 4)
            {
                ThrowForInvalidKind(Kind);
            }
            alignment = stride;
        }
        else if (Kind == GraphicsBufferKind.Constant)
        {
            alignment = D3D12_CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT;
        }
        else if (Kind == GraphicsBufferKind.Default)
        {
            alignment = D3D12_TEXTURE_DATA_PLACEMENT_ALIGNMENT;
        }

        var succeeded = _memoryAllocator.TryAllocate(size, alignment, out var memoryRegion);
        bufferView = succeeded ? new D3D12GraphicsBufferView(this, in memoryRegion, stride) : null;

        return succeeded;
    }

    /// <inheritdoc />
    public override void Unmap()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        UnmapInternal();
    }

    /// <inheritdoc />
    public override void UnmapAndWrite()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        UnmapAndWriteInternal();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _bufferViewsMutex.Dispose();
            _mapMutex.Dispose();

            DisposeAllViewsInternal();

            if (isDisposing)
            {
                _memoryAllocator.Clear();
            }

            ReleaseIfNotNull(_d3d12Resource);
            MemoryRegion.Dispose();
        }

        _state.EndDispose();
    }

    /// <inheritdoc />
    protected override byte* Map()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        return MapInternal();
    }

    /// <inheritdoc />
    protected override byte* MapForRead()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        return MapForReadInternal();
    }

    /// <inheritdoc />
    protected override byte* MapForRead(nuint offset, nuint size)
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        return MapForReadInternal(offset, size);
    }

    /// <inheritdoc />
    protected override void UnmapAndWrite(nuint offset, nuint size)
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        UnmapAndWriteInternal(offset, size);
    }

    internal void AddView(D3D12GraphicsBufferView bufferView)
    {
        using var mutex = new DisposableMutex(_bufferViewsMutex, isExternallySynchronized: false);
        _bufferViews.Add(bufferView);
    }

    internal bool RemoveView(D3D12GraphicsBufferView bufferView)
    {
        using var mutex = new DisposableMutex(_bufferViewsMutex, isExternallySynchronized: false);
        return _bufferViews.Remove(bufferView);
    }

    private void DisposeAllViewsInternal()
    {
        // This method should only be called under a mutex

        foreach (var bufferView in _bufferViews)
        {
            bufferView.Dispose();
        }

        _bufferViews.Clear();
    }

    private byte* MapInternal()
    {
        // This method should only be called under a mutex

        _ = Interlocked.Increment(ref _mappedCount);
        var readRange = default(D3D12_RANGE);

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, &mappedAddress));
        _mappedAddress = mappedAddress;

        return (byte*)mappedAddress;
    }

    private byte* MapForReadInternal()
    {
        // This method should only be called under a mutex

        _ = Interlocked.Increment(ref _mappedCount);

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, pReadRange: null, &mappedAddress));
        _mappedAddress = mappedAddress;

        return (byte*)mappedAddress;
    }

    private byte* MapForReadInternal(nuint offset, nuint size)
    {
        // This method should only be called under a mutex

        _ = Interlocked.Increment(ref _mappedCount);

        var readRange = new D3D12_RANGE {
            Begin = offset,
            End = offset + size,
        };

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, &mappedAddress));
        _mappedAddress = mappedAddress;

        return (byte*)mappedAddress;
    }

    private void UnmapInternal()
    {
        // This method should only be called under a mutex

        if (Interlocked.Decrement(ref _mappedCount) == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }

        var writtenRange = default(D3D12_RANGE);
        D3D12Resource->Unmap(Subresource: 0, &writtenRange);
    }

    private void UnmapAndWriteInternal()
    {
        // This method should only be called under a mutex

        if (Interlocked.Decrement(ref _mappedCount) == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }

        D3D12Resource->Unmap(Subresource: 0, null);
    }

    private void UnmapAndWriteInternal(nuint offset, nuint size)
    {
        // This method should only be called under a mutex

        if (Interlocked.Decrement(ref _mappedCount) == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }

        var writtenRange = new D3D12_RANGE {
            Begin = offset,
            End = offset + size,
        };
        D3D12Resource->Unmap(Subresource: 0, &writtenRange);
    }
}
