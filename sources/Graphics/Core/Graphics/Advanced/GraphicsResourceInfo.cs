// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics resources.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsResourceInfo
{
    /// <summary>The CPU access capabilitites of the resource.</summary>
    public GraphicsCpuAccess CpuAccess;

    /// <summary>The mapped address of the resource or <c>null</c> if the resource is not currently mapped.</summary>
    public volatile void* MappedAddress;

    /// <summary>The memory region in which the resource exists.</summary>
    public GraphicsMemoryRegion MemoryRegion;

    /// <summary>The resource kind.</summary>
    public GraphicsResourceKind Kind;
}
