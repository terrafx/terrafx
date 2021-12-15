// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsBuffer : GraphicsBuffer
{
    private readonly ID3D12Resource* _d3d12Resource;
    private readonly D3D12_RESOURCE_DESC _d3d12ResourceDesc;
    private readonly ulong _d3d12ResourceGpuVirtualAddress;
    private readonly D3D12_RESOURCE_STATES _d3d12ResourceState;
    private readonly D3D12GraphicsMemoryHeap _memoryHeap;

    private string _name = null!;
    private VolatileState _state;

    internal D3D12GraphicsBuffer(D3D12GraphicsDevice device, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment, in GraphicsMemoryRegion memoryRegion, GraphicsBufferKind kind, ID3D12Resource* d3d12Resource, D3D12_RESOURCE_STATES d3d12ResourceState)
        : base(device, cpuAccess, size, alignment, in memoryRegion, kind)
    {
        _d3d12Resource = d3d12Resource;
        _d3d12ResourceDesc = d3d12Resource->GetDesc();
        _d3d12ResourceGpuVirtualAddress = d3d12Resource->GetGPUVirtualAddress();
        _d3d12ResourceState = d3d12ResourceState;
        _memoryHeap = memoryRegion.Allocator.DeviceObject.As<D3D12GraphicsMemoryHeap>();

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsBuffer);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBuffer" /> class.</summary>
    ~D3D12GraphicsBuffer() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

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

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public D3D12GraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <summary>Gets or sets the name for the buffer.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = D3D12Resource->UpdateD3D12Name(value);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
    public override T* Map<T>()
    {
        var readRange = default(D3D12_RANGE);

        byte* pDestination;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, (void**)&pDestination));

        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
    public override T* Map<T>(nuint rangeOffset, nuint rangeLength)
    {
        var readRange = default(D3D12_RANGE);

        byte* pDestination;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, (void**)&pDestination));

        return (T*)(pDestination + rangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
    public override T* MapForRead<T>()
    {
        byte* pDestination;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, null, (void**)&pDestination));

        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
    public override T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
    {
        var readRange = new D3D12_RANGE {
            Begin = readRangeOffset,
            End = readRangeOffset + readRangeLength,
        };

        byte* pDestination;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, (void**)&pDestination));

        return (T*)(pDestination + readRange.Begin);
    }

    /// <inheritdoc />
    public override void Unmap()
    {
        var writtenRange = default(D3D12_RANGE);
        D3D12Resource->Unmap(Subresource: 0, &writtenRange);
    }

    /// <inheritdoc />
    public override void UnmapAndWrite() => D3D12Resource->Unmap(Subresource: 0, null);

    /// <inheritdoc />
    public override void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength)
    {
        var writtenRange = new D3D12_RANGE {
            Begin = writtenRangeOffset,
            End = writtenRangeOffset + writtenRangeLength,
        };
        D3D12Resource->Unmap(Subresource: 0, &writtenRange);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            ReleaseIfNotNull(_d3d12Resource);
            MemoryRegion.Dispose();
        }

        _state.EndDispose();
    }
}
