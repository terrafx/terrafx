// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics resource views.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsResourceViewInfo
{
    /// <summary>The offset, in bytes, of the resource view.</summary>
    public nuint ByteOffset;

    /// <summary>The length, in bytes, of the resource view.</summary>
    public nuint ByteLength;

    /// <summary>The number of bytes per element in the resource view.</summary>
    public uint BytesPerElement;

    /// <summary>The resource view kind.</summary>
    public GraphicsResourceKind Kind;
}
