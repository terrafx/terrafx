// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics;

/// <summary>Contains information about a graphics resource.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct GraphicsResourceInfo
{
    /// <summary>The alignment, in bytes, of the resource.</summary>
    public ulong Alignment { get; init; }

    /// <summary>The CPU access capabilitites of the resource.</summary>
    public GraphicsResourceCpuAccess CpuAccess { get; init; }

    /// <summary>The size, in bytes, of the resource.</summary>
    public ulong Size { get; init; }
}
