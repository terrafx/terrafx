// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics texture view.</summary>
public sealed unsafe class GraphicsTextureView : GraphicsResourceView
{
    private readonly uint _bytesPerLayer;
    private readonly uint _bytesPerRow;

    private readonly UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> _d3d12PlacedSubresourceFootprints;

    private readonly GraphicsTextureKind _kind;

    private readonly ushort _mipLevelCount;
    private readonly ushort _mipLevelStart;

    private readonly ushort _pixelDepth;
    private readonly GraphicsFormat _pixelFormat;
    private readonly uint _pixelHeight;
    private readonly uint _pixelWidth;

    internal GraphicsTextureView(GraphicsTexture texture, in GraphicsTextureViewCreateOptions createOptions) : base(texture, in createOptions)
    {
        texture.AddTextureView(this);

        var d3d12PlacedSubresourceFootprints = texture.D3D12PlacedSubresourceFootprints.Slice(createOptions.MipLevelStart, createOptions.MipLevelCount);
        ref readonly var d3d12PlacedSubresourceFootprint = ref d3d12PlacedSubresourceFootprints.GetReferenceUnsafe(0);

        var pixelWidth = d3d12PlacedSubresourceFootprint.Footprint.Width;
        var pixelHeight = d3d12PlacedSubresourceFootprint.Footprint.Height;
        var pixelDepth = (ushort)d3d12PlacedSubresourceFootprint.Footprint.Depth;

        var bytesPerRow = d3d12PlacedSubresourceFootprint.Footprint.RowPitch;
        var bytesPerLayer = bytesPerRow * pixelHeight;

        _bytesPerLayer = bytesPerLayer;
        _bytesPerRow = bytesPerRow;

        _d3d12PlacedSubresourceFootprints = d3d12PlacedSubresourceFootprints;

        _kind = texture.Kind;

        _mipLevelCount = createOptions.MipLevelCount;
        _mipLevelStart = createOptions.MipLevelStart;

        _pixelDepth = pixelDepth;
        _pixelFormat = texture.PixelFormat;
        _pixelHeight = pixelHeight;
        _pixelWidth = pixelWidth;
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsTextureView" /> class.</summary>
    ~GraphicsTextureView() => Dispose(isDisposing: true);

    /// <summary>Gets the number of bytes per layer of the texture view.</summary>
    public uint BytesPerLayer => _bytesPerLayer;

    /// <summary>Gets the number of bytes per row of the texture view.</summary>
    public uint BytesPerRow => _bytesPerRow;

    /// <summary>Gets the texture view kind.</summary>
    public new GraphicsTextureKind Kind => _kind;

    /// <summary>Gets the number of mip levels in the graphics texture view.</summary>
    public ushort MipLevelCount => _mipLevelCount;

    /// <summary>Gets the index of the first mip level in the graphics texture view.</summary>
    public ushort MipLevelIndex => _mipLevelStart;

    /// <summary>Gets the depth, in pixels, of the texture view.</summary>
    public ushort PixelDepth => _pixelDepth;

    /// <summary>Gets the format of the texture view.</summary>
    public GraphicsFormat PixelFormat => _pixelFormat;

    /// <summary>Gets the height, in pixels, of the texture view.</summary>
    public uint PixelHeight => _pixelHeight;

    /// <summary>Gets the width, in pixels, of the texture view.</summary>
    public uint PixelWidth => _pixelWidth;

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new GraphicsTexture Resource => base.Resource.As<GraphicsTexture>();

    internal UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints => _d3d12PlacedSubresourceFootprints;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _ = Resource.RemoveTextureView(this);
    }

    private protected override unsafe byte* MapForReadUnsafe()
    {
        return Resource.MapForReadUnsafe(D3D12SubresourceIndex);
    }

    private protected override unsafe byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        return Resource.MapForReadUnsafe(D3D12SubresourceIndex, byteStart, byteLength);
    }

    private protected override unsafe byte* MapUnsafe()
    {
        return Resource.MapUnsafe(D3D12SubresourceIndex);
    }

    private protected override void UnmapAndWriteUnsafe()
    {
        Resource.UnmapAndWriteUnsafe(D3D12SubresourceIndex);
    }

    private protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    {
        Resource.UnmapAndWriteUnsafe(D3D12SubresourceIndex, byteStart, byteLength);
    }

    private protected override void UnmapUnsafe()
    {
        Resource.UnmapUnsafe(D3D12SubresourceIndex);
    }
}
