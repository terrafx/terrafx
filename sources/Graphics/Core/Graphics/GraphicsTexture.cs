// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <inheritdoc />
public abstract class GraphicsTexture : GraphicsResource
{
    private readonly GraphicsTextureInfo _textureInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsTexture" /> class.</summary>
    /// <param name="device">The device for which the texture was created.</param>
    /// <param name="memoryRegion">The memory region in which the resource resides.</param>
    /// <param name="resourceInfo">The resource info that describes the resource.</param>
    /// <param name="textureInfo">The texture info that describes the texture.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsTexture(GraphicsDevice device, in GraphicsMemoryRegion memoryRegion, in GraphicsResourceInfo resourceInfo, in GraphicsTextureInfo textureInfo)
        : base(device, in memoryRegion, in resourceInfo)
    {
        _textureInfo = textureInfo;
    }

    /// <summary>Gets the depth, in pixels, of the texture.</summary>
    public ushort Depth => _textureInfo.Depth;

    /// <summary>Gets the format of the texture.</summary>
    public GraphicsFormat Format => _textureInfo.Format;

    /// <summary>Gets the height, in pixels, of the texture.</summary>
    public uint Height => _textureInfo.Height;

    /// <summary>Gets the texture kind.</summary>
    public GraphicsTextureKind Kind => _textureInfo.Kind;

    /// <summary>Gets the width, in pixels, of the texture.</summary>
    public uint Width => _textureInfo.Width;
}
