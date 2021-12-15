// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <inheritdoc />
public abstract unsafe class GraphicsTexture : GraphicsResource
{
    private readonly ushort _depth;
    private readonly GraphicsFormat _format;
    private readonly uint _height;
    private readonly GraphicsTextureKind _kind;
    private readonly uint _width;

    /// <summary>Initializes a new instance of the <see cref="GraphicsTexture" /> class.</summary>
    /// <param name="device">The device for which the texture was created.</param>
    /// <param name="cpuAccess">The CPU access capabilities for the texture.</param>
    /// <param name="size">The size, in bytes, of the texture.</param>
    /// <param name="alignment">The alignment, in bytes, of the texture.</param>
    /// <param name="memoryRegion">The memory region in which the texture exists.</param>
    /// <param name="kind">The texture kind.</param>
    /// <param name="format">The format of the texture.</param>
    /// <param name="width">The width, in pixels, of the texture.</param>
    /// <param name="height">The height, in pixels, of the texture.</param>
    /// <param name="depth">The depth, in pixels, of the texture.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsTexture(GraphicsDevice device, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment, in GraphicsMemoryRegion memoryRegion, GraphicsTextureKind kind, GraphicsFormat format, uint width, uint height, ushort depth)
        : base(device, cpuAccess, size, alignment, in memoryRegion)
    {
        _kind = kind;
        _format = format;
        _width = width;
        _height = height;
        _depth = depth;
    }

    /// <summary>Gets the depth, in pixels, of the texture.</summary>
    public ushort Depth => _depth;

    /// <summary>Gets the format of the texture.</summary>
    public GraphicsFormat Format => _format;

    /// <summary>Gets the height, in pixels, of the texture.</summary>
    public uint Height => _height;

    /// <summary>Gets the texture kind.</summary>
    public GraphicsTextureKind Kind => _kind;

    /// <summary>Gets the width, in pixels, of the texture.</summary>
    public uint Width => _width;
}
