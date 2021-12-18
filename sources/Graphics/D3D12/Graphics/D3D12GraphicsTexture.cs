// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using TerraFX.Collections;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsTexture : GraphicsTexture
{
    private readonly UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> _d3d12PlacedSubresourceFootprints;
    private readonly ID3D12Resource* _d3d12Resource;
    private readonly D3D12_RESOURCE_DESC _d3d12ResourceDesc;
    private readonly D3D12_RESOURCE_STATES _d3d12ResourceState;
    private readonly ValueMutex _mapMutex;
    private readonly D3D12GraphicsMemoryHeap _memoryHeap;
    private readonly ValueList<D3D12GraphicsTextureView> _textureViews;
    private readonly ValueMutex _textureViewsMutex;

    private string _name = null!;
    private VolatileState _state;

    internal D3D12GraphicsTexture(D3D12GraphicsDevice device, in CreateInfo createInfo)
        : base(device, in createInfo.MemoryRegion, createInfo.CpuAccess, in createInfo.TextureInfo)
    {
        var d3d12Resource = createInfo.D3D12Resource;

        _d3d12PlacedSubresourceFootprints = createInfo.D3D12PlacedSubresourceFootprints;
        _d3d12Resource = d3d12Resource;
        _d3d12ResourceDesc = d3d12Resource->GetDesc();
        _d3d12ResourceState = createInfo.D3D12ResourceState;
        _mapMutex = new ValueMutex();
        _memoryHeap = createInfo.MemoryRegion.Allocator.DeviceObject.As<D3D12GraphicsMemoryHeap>();
        _textureViewsMutex = new ValueMutex();

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsTexture);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsTexture" /> class.</summary>
    ~D3D12GraphicsTexture() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc />
    public override int Count => _textureViews.Count;

    /// <summary>Gets the <see cref="D3D12_PLACED_SUBRESOURCE_FOOTPRINT" />s for the texture.</summary>
    public UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints => _d3d12PlacedSubresourceFootprints;

    /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the texture.</summary>
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

    /// <summary>Gets the default resource state for <see cref="D3D12Resource" />.</summary>
    public D3D12_RESOURCE_STATES D3D12ResourceState => _d3d12ResourceState;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc />
    public override bool IsMapped => false;

    /// <inheritdoc />
    public override unsafe void* MappedAddress => null;

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public D3D12GraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <summary>Gets or sets the name for the pipeline signature.</summary>
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
    public override D3D12GraphicsTextureView CreateView(ushort mipLevelIndex, ushort mipLevelCount)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsBuffer));

        ThrowIfNotInBounds(mipLevelIndex, MipLevelCount);
        ThrowIfNotInInsertBounds(mipLevelCount, MipLevelCount - mipLevelIndex);

        var d3d12SubresourceIndex = D3D12ResourceDesc.CalcSubresource(mipLevelIndex, ArraySlice: 0, PlaneSlice: 0);
        var d3d12ResourceDesc = D3D12ResourceDesc;

        var d3d12PlacedSubresourceFootprints = D3D12PlacedSubresourceFootprints.Slice(mipLevelIndex, mipLevelCount);
        var size = 0UL;

        Device.D3D12Device->GetCopyableFootprints(
            &d3d12ResourceDesc,
            FirstSubresource: d3d12SubresourceIndex,
            NumSubresources: mipLevelCount,
            BaseOffset: 0,
            pLayouts: null,
            pNumRows: null,
            pRowSizeInBytes: null,
            pTotalBytes: &size
        );

        ref readonly var d3d12PlacedSubresourceFootprint = ref d3d12PlacedSubresourceFootprints[0];

        var width = d3d12PlacedSubresourceFootprint.Footprint.Width;
        var height = d3d12PlacedSubresourceFootprint.Footprint.Height;
        var depth = (ushort)d3d12PlacedSubresourceFootprint.Footprint.Depth;

        var rowPitch = d3d12PlacedSubresourceFootprint.Footprint.RowPitch;
        var slicePitch = rowPitch * height;

        var textureViewInfo = new GraphicsTextureViewInfo {
            Depth = depth,
            Format = Format,
            Height = height,
            Kind = Kind,
            MipLevelCount = mipLevelCount,
            MipLevelIndex = mipLevelIndex,
            RowPitch = rowPitch,
            SlicePitch = slicePitch,
            Width = width,
        };
        return new D3D12GraphicsTextureView(this, in textureViewInfo, d3d12SubresourceIndex, d3d12PlacedSubresourceFootprints);
    }

    /// <inheritdoc />
    public override void DisposeAllViews()
    {
        using var mutex = new DisposableMutex(_textureViewsMutex, isExternallySynchronized: false);
        DisposeAllViewsInternal();
    }

    /// <inheritdoc />
    public override IEnumerator<D3D12GraphicsTextureView> GetEnumerator() => _textureViews.GetEnumerator();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _d3d12PlacedSubresourceFootprints.Dispose();
            _mapMutex.Dispose();
            _textureViewsMutex.Dispose();

            DisposeAllViewsInternal();

            ReleaseIfNotNull(_d3d12Resource);
            MemoryRegion.Dispose();
        }

        _state.EndDispose();
    }

    internal void AddView(D3D12GraphicsTextureView textureView)
    {
        using var mutex = new DisposableMutex(_textureViewsMutex, isExternallySynchronized: false);
        _textureViews.Add(textureView);
    }

    internal bool RemoveView(D3D12GraphicsTextureView textureView)
    {
        using var mutex = new DisposableMutex(_textureViewsMutex, isExternallySynchronized: false);
        return _textureViews.Remove(textureView);
    }

    private void DisposeAllViewsInternal()
    {
        // This method should only be called under a mutex

        foreach (var textureView in _textureViews)
        {
            textureView.Dispose();
        }

        _textureViews.Clear();
    }
}
