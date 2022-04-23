// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics buffer view.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsBufferViewCreateOptions
{
    /// <summary>The number of bytes per element in the buffer view.</summary>
    public uint BytesPerElement;

    /// <summary>The number of elements in the buffer view.</summary>
    public uint ElementCount;
}
