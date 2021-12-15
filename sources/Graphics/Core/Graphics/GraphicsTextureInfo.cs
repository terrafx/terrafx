// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics;

/// <summary>Contains information about a graphics texture.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct GraphicsTextureInfo
{
    /// <summary>The depth, in pixels, of the texture.</summary>
    public ushort Depth { get; init; }

    /// <summary>the format of the texture.</summary>
    public GraphicsFormat Format { get; init; }

    /// <summary>The height, in pixels, of the texture.</summary>
    public uint Height { get; init; }

    /// <summary>The texture kind.</summary>
    public GraphicsTextureKind Kind { get; init; }

    /// <summary>The width, in pixels, of the texture.</summary>
    public uint Width { get; init; }
}
