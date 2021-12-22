// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;

namespace TerraFX.Graphics;

/// <inheritdoc />
public abstract class GraphicsTexture : GraphicsResource
{
    private readonly GraphicsTextureInfo _textureInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsTexture" /> class.</summary>
    /// <param name="device">The device for which the texture was created.</param>
    /// <param name="memoryRegion">The memory region in which the resource resides.</param>
    /// <param name="cpuAccess">The CPU access capabilitites of the resource.</param>
    /// <param name="textureInfo">The texture info that describes the texture.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsTexture(GraphicsDevice device, in GraphicsMemoryRegion memoryRegion, GraphicsResourceCpuAccess cpuAccess, in GraphicsTextureInfo textureInfo)
        : base(device, in memoryRegion, cpuAccess)
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

    /// <summary>Gets the number of mip levels in the graphics texture.</summary>
    public ushort MipLevelCount => _textureInfo.MipLevelCount;

    /// <summary>Gets the row pitch, in bytes, of the texture.</summary>
    public uint RowPitch => _textureInfo.RowPitch;

    /// <summary>Gets the slice pitch, in bytes, of the texture.</summary>
    public uint SlicePitch => _textureInfo.SlicePitch;

    /// <summary>Gets the width, in pixels, of the texture.</summary>
    public uint Width => _textureInfo.Width;

    /// <summary>Creates a view of the texture.</summary>
    /// <param name="mipLevelIndex">The index of the first mip level in the graphics texture view.</param>
    /// <param name="mipLevelCount">The number of mip levels in the graphics texture view.</param>
    /// <returns>The created view.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="mipLevelIndex" /> is greater than or equal to <see cref="MipLevelCount" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="mipLevelIndex" /> plus <paramref name="mipLevelCount" /> is greater than <see cref="MipLevelCount" />.</exception>
    /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
    public abstract GraphicsTextureView CreateView(ushort mipLevelIndex, ushort mipLevelCount);
}
