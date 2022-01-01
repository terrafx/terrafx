// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public abstract class GraphicsTexture : GraphicsResource
{
    /// <summary>The information for the graphics texture.</summary>
    protected GraphicsTextureInfo TextureInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsTexture" /> class.</summary>
    /// <param name="device">The device for which the texture was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsTexture(GraphicsDevice device) : base(device)
    {
        ResourceInfo.Kind = GraphicsResourceKind.Texture;
    }

    /// <summary>Gets number of bytes per layer of the texture.</summary>
    public uint BytesPerLayer => TextureInfo.BytesPerLayer;

    /// <summary>Gets number of bytes per row of the texture.</summary>
    public uint BytesPerRow => TextureInfo.BytesPerRow;

    /// <summary>Gets the texture kind.</summary>
    public new GraphicsTextureKind Kind => TextureInfo.Kind;

    /// <summary>Gets the number of mip levels in the graphics texture.</summary>
    public ushort MipLevelCount => TextureInfo.MipLevelCount;

    /// <summary>Gets the depth, in pixels, of the texture.</summary>
    public ushort PixelDepth => TextureInfo.PixelDepth;

    /// <summary>Gets the format of pixels in the texture.</summary>
    public GraphicsFormat PixelFormat => TextureInfo.PixelFormat;

    /// <summary>Gets the height, in pixels, of the texture.</summary>
    public uint PixelHeight => TextureInfo.PixelHeight;

    /// <summary>Gets the width, in pixels, of the texture.</summary>
    public uint PixelWidth => TextureInfo.PixelWidth;

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
        return CreateViewUnsafe(in createOptions);
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

        return CreateViewUnsafe(in createOptions);
    }

    /// <summary>Creates a view of the texture.</summary>
    /// <param name="createOptions">The options to use when creating the texture view.</param>
    /// <returns>The created texture view.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsTextureView CreateViewUnsafe(in GraphicsTextureViewCreateOptions createOptions);
}
