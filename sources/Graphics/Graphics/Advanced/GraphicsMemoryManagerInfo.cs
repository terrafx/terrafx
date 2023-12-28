// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics memory managers.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsMemoryManagerInfo
{
    /// <summary>The length, in bytes, of the manager.</summary>
    public ulong ByteLength;

    /// <summary>The minimum length, in bytes, of the manager.</summary>
    public nuint MinimumByteLength;

    /// <summary>The total number of operations performed by the manager.</summary>
    public ulong OperationCount;

    /// <summary>The total length, in bytes, of free memory regions.</summary>
    public ulong TotalFreeMemoryRegionByteLength;
}
