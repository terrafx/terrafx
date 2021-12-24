// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics buffer views.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsBufferViewInfo
{
    /// <summary>The buffer view kind.</summary>
    public GraphicsBufferKind Kind;

    /// <summary>The memory region in which the buffer view exists.</summary>
    public GraphicsMemoryRegion MemoryRegion;
}
