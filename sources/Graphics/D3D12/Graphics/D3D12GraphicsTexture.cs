// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DirectX.D3D12_TEXTURE_LAYOUT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsTexture : GraphicsTexture
{
    private readonly D3D12_RESOURCE_STATES _defaultResourceState;

    private ComPtr<ID3D12Resource> _d3d12Resource;
    private readonly uint _d3d12ResourceVersion;

    private readonly UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> _d3d12PlacedSubresourceFootprints;
    private D3D12GraphicsMemoryHeap _memoryHeap;

    private ValueList<D3D12GraphicsTextureView> _textureViews;
    private readonly ValueMutex _textureViewsMutex;

    private volatile uint _mappedCount;
    private readonly ValueMutex _mappedMutex;

    internal D3D12GraphicsTexture(D3D12GraphicsDevice device, in GraphicsTextureCreateOptions createOptions) : base(device)
    {
        device.AddTexture(this);

        TextureInfo.Kind = createOptions.Kind;
        TextureInfo.MipLevelCount = (createOptions.MipLevelCount != 0) ? createOptions.MipLevelCount : (ushort)1;
        TextureInfo.PixelDepth = createOptions.PixelDepth;
        TextureInfo.PixelFormat = createOptions.PixelFormat;
        TextureInfo.PixelHeight = createOptions.PixelHeight;
        TextureInfo.PixelWidth = createOptions.PixelWidth;

        _defaultResourceState = createOptions.CpuAccess switch {
            GraphicsCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
            GraphicsCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
            _ => D3D12_RESOURCE_STATE_COMMON,
        };

        _d3d12Resource = CreateD3D12Resource(in createOptions, out _d3d12ResourceVersion);
        _d3d12PlacedSubresourceFootprints = GetD3D12PlacedSubresourceFootprints();

        ref readonly var d3d12PlacedSubresourceFootprint = ref _d3d12PlacedSubresourceFootprints.GetReferenceUnsafe(0);

        var bytesPerRow = d3d12PlacedSubresourceFootprint.Footprint.RowPitch;
        var bytesPerLayer = bytesPerRow * createOptions.PixelHeight;

        TextureInfo.BytesPerLayer = bytesPerLayer;
        TextureInfo.BytesPerRow = bytesPerRow;

        _memoryHeap = MemoryRegion.MemoryAllocator.DeviceObject.As<D3D12GraphicsMemoryHeap>();

        _textureViews = new ValueList<D3D12GraphicsTextureView>();
        _textureViewsMutex = new ValueMutex();

        _mappedCount = 0;
        _mappedMutex = new ValueMutex();

        SetNameUnsafe(Name);

        ID3D12Resource* CreateD3D12Resource(in GraphicsTextureCreateOptions createOptions, out uint d3d12ResourceVersion)
        {
            ID3D12Resource* d3d12Resource;

            var d3d12Device = device.D3D12Device;

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
            var d3d12ResourceAllocationInfo = d3d12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &d3d12ResourceDesc);

            var memoryManager = device.GetMemoryManager(createOptions.CpuAccess, GraphicsResourceKind.Texture);

            ResourceInfo.CpuAccess = createOptions.CpuAccess;
            ResourceInfo.MappedAddress = null;

            var memoryAllocationOptions = new GraphicsMemoryAllocationOptions {
                ByteLength = (nuint)d3d12ResourceAllocationInfo.SizeInBytes,
                ByteAlignment = (nuint)d3d12ResourceAllocationInfo.Alignment,
                AllocationFlags = createOptions.AllocationFlags,
            };
            ResourceInfo.MemoryRegion = memoryManager.Allocate(in memoryAllocationOptions);

            ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
                ResourceInfo.MemoryRegion.MemoryAllocator.DeviceObject.As<D3D12GraphicsMemoryHeap>().D3D12Heap,
                ResourceInfo.MemoryRegion.ByteOffset,
                &d3d12ResourceDesc,
                _defaultResourceState,
                pOptimizedClearValue: null,
                __uuidof<ID3D12Resource>(),
                (void**)&d3d12Resource
            ));

            return GetLatestD3D12Resource(d3d12Resource, out d3d12ResourceVersion);
        }

        UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> GetD3D12PlacedSubresourceFootprints()
        {
            var mipLevelCount = TextureInfo.MipLevelCount;
            var d3d12PlacedSubresourceFootprints = new UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT>(mipLevelCount);
            var d3d12ResourceDesc = _d3d12Resource.Get()->GetDesc();

            Device.D3D12Device->GetCopyableFootprints(
                &d3d12ResourceDesc,
                FirstSubresource: 0,
                NumSubresources: mipLevelCount,
                BaseOffset: 0,
                d3d12PlacedSubresourceFootprints.GetPointerUnsafe(0),
                pNumRows: null,
                pRowSizeInBytes: null,
                pTotalBytes: null
            );

            return d3d12PlacedSubresourceFootprints;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsTexture" /> class.</summary>
    ~D3D12GraphicsTexture() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="D3D12_PLACED_SUBRESOURCE_FOOTPRINT" />s for the texture.</summary>
    public UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints => _d3d12PlacedSubresourceFootprints;

    /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the buffer.</summary>
    public ID3D12Resource* D3D12Resource => _d3d12Resource;

    /// <summary>Gets the interface version of <see cref="D3D12Resource" />.</summary>
    public uint D3D12ResourceVersion => _d3d12ResourceVersion;

    /// <summary>Gets the default resource state for <see cref="D3D12Resource" />.</summary>
    public D3D12_RESOURCE_STATES DefaultResourceState => _defaultResourceState;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets the memory heap in which the texture exists.</summary>
    public D3D12GraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override D3D12GraphicsTextureView CreateViewUnsafe(in GraphicsTextureViewCreateOptions createOptions)
    {
        return new D3D12GraphicsTextureView(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _textureViews.Dispose();
            _memoryHeap = null!;
        }
        _textureViewsMutex.Dispose();

        _d3d12PlacedSubresourceFootprints.Dispose();

        _mappedCount = 0;
        _mappedMutex.Dispose();

        ResourceInfo.MappedAddress = null;
        ResourceInfo.MemoryRegion.Dispose();

        _ = _d3d12Resource.Reset();

        _ = Device.RemoveTexture(this);
    }

    /// <inheritdoc />
    protected override byte* MapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapNoMutex(mipLevelStart: 0);
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(mipLevelStart: 0);
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(mipLevelStart: 0, byteStart, byteLength);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12Resource->SetD3D12Name(value);
    }

    /// <inheritdoc />
    protected override void UnmapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapNoMutex(mipLevelStart: 0);
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(mipLevelStart: 0);
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(mipLevelStart: 0, byteStart, byteLength);
    }

    internal void AddTextureView(D3D12GraphicsTextureView textureView)
    {
        _textureViews.Add(textureView, _textureViewsMutex);
    }

    internal byte* MapView(ushort mipLevelStart)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapNoMutex(mipLevelStart);
    }

    internal byte* MapViewForRead(ushort mipLevelStart)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(mipLevelStart);
    }

    internal byte* MapViewForRead(ushort mipLevelStart, nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(mipLevelStart, byteStart, byteLength) + byteStart;
    }

    internal bool RemoveTextureView(D3D12GraphicsTextureView textureView)
    {
        return IsDisposed || _textureViews.Remove(textureView, _textureViewsMutex);
    }

    internal void UnmapView(ushort mipLevelStart)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapNoMutex(mipLevelStart);
    }

    internal void UnmapViewAndWrite(ushort mipLevelStart)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(mipLevelStart);
    }

    internal void UnmapViewAndWrite(ushort mipLevelStart, nuint byteStart, nuint byteLength)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(mipLevelStart, byteStart, byteLength);
    }

    private byte* MapNoMutex(ushort mipLevelStart)
    {
        _ = Interlocked.Increment(ref _mappedCount);
        var readRange = default(D3D12_RANGE);

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(mipLevelStart, &readRange, &mappedAddress));

        ResourceInfo.MappedAddress = mappedAddress;
        return (byte*)mappedAddress;
    }

    private byte* MapForReadNoMutex(ushort mipLevelStart)
    {
        _ = Interlocked.Increment(ref _mappedCount);

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(mipLevelStart, pReadRange: null, &mappedAddress));

        ResourceInfo.MappedAddress = mappedAddress;
        return (byte*)mappedAddress;
    }

    private byte* MapForReadNoMutex(ushort mipLevelStart, nuint byteStart, nuint byteLength)
    {
        _ = Interlocked.Increment(ref _mappedCount);

        var readRange = new D3D12_RANGE {
            Begin = byteStart,
            End = byteStart + byteLength,
        };

        void* mappedAddress;
        ThrowExternalExceptionIfFailed(D3D12Resource->Map(mipLevelStart, &readRange, &mappedAddress));

        ResourceInfo.MappedAddress = mappedAddress;
        return (byte*)mappedAddress;
    }

    private void UnmapNoMutex(ushort mipLevelStart)
    {
        if (Interlocked.Decrement(ref _mappedCount) == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }

        var writtenRange = default(D3D12_RANGE);
        D3D12Resource->Unmap(mipLevelStart, &writtenRange);
    }

    private void UnmapAndWriteNoMutex(ushort mipLevelStart )
    {
        if (Interlocked.Decrement(ref _mappedCount) == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }

        D3D12Resource->Unmap(mipLevelStart, null);
    }

    private void UnmapAndWriteNoMutex(ushort mipLevelStart, nuint byteStart, nuint byteLength)
    {
        if (Interlocked.Decrement(ref _mappedCount) == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }

        var writtenRange = new D3D12_RANGE {
            Begin = byteStart,
            End = byteStart + byteLength,
        };
        D3D12Resource->Unmap(mipLevelStart, &writtenRange);
    }
}
