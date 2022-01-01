// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Utilities.GraphicsUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsTextureView : GraphicsTextureView
{
    private readonly UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> _d3d12PlacedSubresourceFootprints;
    private readonly uint _d3d12SubresourceIndex;

    internal D3D12GraphicsTextureView(D3D12GraphicsTexture texture, in GraphicsTextureViewCreateOptions createOptions) : base(texture)
    {
        texture.AddTextureView(this);

        var d3d12ResourceDesc = texture.D3D12Resource->GetDesc();
        var d3d12SubresourceIndex = d3d12ResourceDesc.CalcSubresource(createOptions.MipLevelStart, ArraySlice: 0, PlaneSlice: 0);

        var d3d12PlacedSubresourceFootprints = texture.D3D12PlacedSubresourceFootprints.Slice(createOptions.MipLevelStart, createOptions.MipLevelCount);
        var byteLength = 0UL;

        Device.D3D12Device->GetCopyableFootprints(
            &d3d12ResourceDesc,
            FirstSubresource: d3d12SubresourceIndex,
            NumSubresources: createOptions.MipLevelCount,
            BaseOffset: 0,
            pLayouts: null,
            pNumRows: null,
            pRowSizeInBytes: null,
            pTotalBytes: &byteLength
        );

        ref readonly var d3d12PlacedSubresourceFootprint = ref d3d12PlacedSubresourceFootprints.GetReferenceUnsafe(0);

        var pixelWidth = d3d12PlacedSubresourceFootprint.Footprint.Width;
        var pixelHeight = d3d12PlacedSubresourceFootprint.Footprint.Height;
        var pixelDepth = (ushort)d3d12PlacedSubresourceFootprint.Footprint.Depth;

        var bytesPerRow = d3d12PlacedSubresourceFootprint.Footprint.RowPitch;
        var bytesPerLayer = bytesPerRow * pixelHeight;

        var pixelFormat = texture.PixelFormat;

        ResourceViewInfo.ByteLength = (nuint)byteLength;
        ResourceViewInfo.ByteOffset = (nuint)d3d12PlacedSubresourceFootprint.Offset;
        ResourceViewInfo.BytesPerElement = pixelFormat.GetSize();

        TextureViewInfo.BytesPerLayer = bytesPerLayer;
        TextureViewInfo.BytesPerRow = bytesPerRow;
        TextureViewInfo.MipLevelCount = createOptions.MipLevelCount;
        TextureViewInfo.MipLevelStart = createOptions.MipLevelStart;
        TextureViewInfo.PixelDepth = pixelDepth;
        TextureViewInfo.PixelFormat = pixelFormat;
        TextureViewInfo.PixelHeight = pixelHeight;
        TextureViewInfo.PixelWidth = pixelWidth;
        TextureViewInfo.Kind = texture.Kind;

        _d3d12PlacedSubresourceFootprints = d3d12PlacedSubresourceFootprints;
        _d3d12SubresourceIndex = d3d12SubresourceIndex;
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsTextureView" /> class.</summary>
    ~D3D12GraphicsTextureView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="D3D12_PLACED_SUBRESOURCE_FOOTPRINT" />s for the texture view.</summary>
    public UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints => _d3d12PlacedSubresourceFootprints;

    /// <summary>Gets the subresource index for the texture view.</summary>
    public uint D3D12SubresourceIndex => _d3d12SubresourceIndex;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new D3D12GraphicsTexture Resource => base.Resource.As<D3D12GraphicsTexture>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _ = Resource.RemoveTextureView(this);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapUnsafe()
    {
        return Resource.MapView(TextureViewInfo.MipLevelStart);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapForReadUnsafe()
    {
        return Resource.MapViewForRead(TextureViewInfo.MipLevelStart);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        return Resource.MapViewForRead(TextureViewInfo.MipLevelStart, byteStart, byteLength);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    /// <inheritdoc />
    protected override void UnmapUnsafe()
    {
        Resource.UnmapView(TextureViewInfo.MipLevelStart);
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe()
    {
        Resource.UnmapViewAndWrite(TextureViewInfo.MipLevelStart);
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    { 
        Resource.UnmapViewAndWrite(TextureViewInfo.MipLevelStart, byteStart, byteLength);
    }
}
