// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics texture view.</summary>
public abstract class GraphicsTextureView : GraphicsResourceView
{
    private readonly GraphicsTextureViewInfo _textureViewInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsTextureView" /> class.</summary>
    /// <param name="texture">The texture for which the texture view was created.</param>
    /// <param name="textureViewInfo">The texture info that describes the texture view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="texture" /> is <c>null</c></exception>
    protected GraphicsTextureView(GraphicsTexture texture, in GraphicsTextureViewInfo textureViewInfo)
        : base(texture, textureViewInfo.Format.GetSize())
    {
        _textureViewInfo = textureViewInfo;
    }

    /// <summary>Gets the depth, in pixels, of the texture view.</summary>
    public ushort Depth => _textureViewInfo.Depth;

    /// <summary>Gets the format of the texture view.</summary>
    public GraphicsFormat Format => _textureViewInfo.Format;

    /// <summary>Gets the height, in pixels, of the texture view.</summary>
    public uint Height => _textureViewInfo.Height;

    /// <summary>Gets the texture view kind.</summary>
    public GraphicsTextureKind Kind => _textureViewInfo.Kind;

    /// <summary>Gets the number of mip levels in the graphics texture view.</summary>
    public ushort MipLevelCount => _textureViewInfo.MipLevelCount;

    /// <summary>Gets the index of the first mip level in the graphics texture view.</summary>
    public ushort MipLevelIndex => _textureViewInfo.MipLevelIndex;

    /// <inheritdoc cref="GraphicsResourceView.Resource" />
    public new GraphicsTexture Resource => base.Resource.As<GraphicsTexture>();

    /// <summary>Gets the row pitch, in bytes, of the texture view.</summary>
    public uint RowPitch => _textureViewInfo.RowPitch;

    /// <summary>Gets the slice pitch, in bytes, of the texture view.</summary>
    public uint SlicePitch => _textureViewInfo.SlicePitch;

    /// <summary>Gets the width, in pixels, of the texture view.</summary>
    public uint Width => _textureViewInfo.Width;
}
