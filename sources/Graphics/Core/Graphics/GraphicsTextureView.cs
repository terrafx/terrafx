// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics texture view.</summary>
public abstract class GraphicsTextureView : GraphicsResourceView
{
    /// <summary>The information for the graphics texture view.</summary>
    protected GraphicsTextureViewInfo TextureViewInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsTextureView" /> class.</summary>
    /// <param name="texture">The texture for which the texture view was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="texture" /> is <c>null</c></exception>
    protected GraphicsTextureView(GraphicsTexture texture) : base(texture)
    {
        ResourceViewInfo.Kind = GraphicsResourceKind.Texture;
    }

    /// <summary>Gets the number of bytes per layer of the texture view.</summary>
    public uint BytesPerLayer => TextureViewInfo.BytesPerLayer;

    /// <summary>Gets the number of bytes per row of the texture view.</summary>
    public uint BytesPerRow => TextureViewInfo.BytesPerRow;

    /// <summary>Gets the texture view kind.</summary>
    public new GraphicsTextureKind Kind => TextureViewInfo.Kind;

    /// <summary>Gets the number of mip levels in the graphics texture view.</summary>
    public ushort MipLevelCount => TextureViewInfo.MipLevelCount;

    /// <summary>Gets the index of the first mip level in the graphics texture view.</summary>
    public ushort MipLevelIndex => TextureViewInfo.MipLevelStart;

    /// <summary>Gets the depth, in pixels, of the texture view.</summary>
    public ushort PixelDepth => TextureViewInfo.PixelDepth;

    /// <summary>Gets the format of the texture view.</summary>
    public GraphicsFormat PixelFormat => TextureViewInfo.PixelFormat;

    /// <summary>Gets the height, in pixels, of the texture view.</summary>
    public uint PixelHeight => TextureViewInfo.PixelHeight;

    /// <summary>Gets the width, in pixels, of the texture view.</summary>
    public uint PixelWidth => TextureViewInfo.PixelWidth;

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new GraphicsTexture Resource => base.Resource.As<GraphicsTexture>();
}
