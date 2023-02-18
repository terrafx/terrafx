// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class GraphicsTexture : GraphicsResource
{
    private readonly uint _bytesPerLayer;
    private readonly uint _bytesPerRow;

    private readonly UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> _d3d12PlacedSubresourceFootprints;

    private readonly GraphicsTextureKind _kind;

    private readonly ushort _mipLevelCount;

    private readonly ushort _pixelDepth;
    private readonly GraphicsFormat _pixelFormat;
    private readonly uint _pixelHeight;
    private readonly uint _pixelWidth;

    private ValueList<GraphicsTextureView> _textureViews;
    private readonly ValueMutex _textureViewsMutex;

    internal GraphicsTexture(GraphicsDevice device, in GraphicsTextureCreateOptions createOptions) : base(device, in createOptions)
    {
        device.AddTexture(this);

        _kind = createOptions.Kind;
        _mipLevelCount = (createOptions.MipLevelCount != 0) ? createOptions.MipLevelCount : (ushort)1;
        _pixelDepth = createOptions.PixelDepth;
        _pixelFormat = createOptions.PixelFormat;
        _pixelHeight = createOptions.PixelHeight;
        _pixelWidth = createOptions.PixelWidth;

        _d3d12PlacedSubresourceFootprints = GetD3D12PlacedSubresourceFootprints();

        ref readonly var d3d12PlacedSubresourceFootprint = ref _d3d12PlacedSubresourceFootprints.GetReferenceUnsafe(0);

        var bytesPerRow = d3d12PlacedSubresourceFootprint.Footprint.RowPitch;
        var bytesPerLayer = bytesPerRow * createOptions.PixelHeight;

        _bytesPerLayer = bytesPerLayer;
        _bytesPerRow = bytesPerRow;

        _textureViews = new ValueList<GraphicsTextureView>();
        _textureViewsMutex = new ValueMutex();

        UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> GetD3D12PlacedSubresourceFootprints()
        {
            var mipLevelCount = _mipLevelCount;
            var d3d12PlacedSubresourceFootprints = new UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT>(mipLevelCount);
            var d3d12ResourceDesc = D3D12Resource->GetDesc();

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

    /// <summary>Finalizes an instance of the <see cref="GraphicsTexture" /> class.</summary>
    ~GraphicsTexture() => Dispose(isDisposing: false);

    /// <summary>Gets number of bytes per layer of the texture.</summary>
    public uint BytesPerLayer => _bytesPerLayer;

    /// <summary>Gets number of bytes per row of the texture.</summary>
    public uint BytesPerRow => _bytesPerRow;

    /// <summary>Gets the texture kind.</summary>
    public new GraphicsTextureKind Kind => _kind;

    /// <summary>Gets the number of mip levels in the graphics texture.</summary>
    public ushort MipLevelCount => _mipLevelCount;

    /// <summary>Gets the depth, in pixels, of the texture.</summary>
    public ushort PixelDepth => _pixelDepth;

    /// <summary>Gets the format of pixels in the texture.</summary>
    public GraphicsFormat PixelFormat => _pixelFormat;

    /// <summary>Gets the height, in pixels, of the texture.</summary>
    public uint PixelHeight => _pixelHeight;

    /// <summary>Gets the width, in pixels, of the texture.</summary>
    public uint PixelWidth => _pixelWidth;

    internal UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints => _d3d12PlacedSubresourceFootprints;

    /// <summary>Creates a view of the texture.</summary>
    /// <param name="mipLevelStart">The index of the first mip level of the texture view.</param>
    /// <param name="mipLevelCount">The number of mip levels in the texture view.</param>
    /// <returns>The created texture view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="mipLevelStart" /> is greater than or equal to <see cref="MipLevelCount" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="mipLevelStart" /> + <paramref name="mipLevelCount" /> is greater than <see cref="MipLevelCount" />.</exception>
    /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GraphicsTextureView CreateView(ushort mipLevelStart, ushort mipLevelCount)
    {
        ThrowIfDisposed();

        ThrowIfNotInBounds(mipLevelStart, MipLevelCount);
        ThrowIfNotInInsertBounds(mipLevelCount, MipLevelCount - mipLevelStart);

        var createOptions = new GraphicsTextureViewCreateOptions {
            MipLevelCount = mipLevelCount,
            MipLevelStart = mipLevelStart,
        };
        return new GraphicsTextureView(this, in createOptions);
    }

    /// <summary>Creates a view of the texture.</summary>
    /// <param name="createOptions">The options to use when creating the texture view.</param>
    /// <returns>The created texture view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureViewCreateOptions.MipLevelStart" /> is greater than or equal to <see cref="MipLevelCount" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsTextureViewCreateOptions.MipLevelStart" /> + <see cref="GraphicsTextureViewCreateOptions.MipLevelCount" /> is greater than <see cref="MipLevelCount" />.</exception>
    /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GraphicsTextureView CreateView(in GraphicsTextureViewCreateOptions createOptions)
    {
        ThrowIfDisposed();

        ThrowIfNotInBounds(createOptions.MipLevelStart, MipLevelCount);
        ThrowIfNotInInsertBounds(createOptions.MipLevelCount, MipLevelCount - createOptions.MipLevelStart);

        return new GraphicsTextureView(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _textureViews.Dispose();
        }
        _textureViewsMutex.Dispose();

        base.Dispose(isDisposing);
        _d3d12PlacedSubresourceFootprints.Dispose();

        _ = Device.RemoveTexture(this);
    }

    internal void AddTextureView(GraphicsTextureView textureView) => _textureViews.Add(textureView, _textureViewsMutex);

    internal bool RemoveTextureView(GraphicsTextureView textureView) => IsDisposed || _textureViews.Remove(textureView, _textureViewsMutex);
}
