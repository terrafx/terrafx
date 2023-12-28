// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Budget struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>Describes data used for budgeting graphics memory.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsMemoryBudget
{
    /// <summary>Gets the estimated memory budget, in bytes.</summary>
    public ulong EstimatedMemoryByteBudget;

    /// <summary>Gets the estimated memory usage, in bytes.</summary>
    public ulong EstimatedMemoryByteUsage;

    /// <summary>Gets the total length, in bytes, of allocated memory regions used for the memory budget.</summary>
    public ulong TotalAllocatedMemoryRegionByteLength => TotalByteLength - TotalFreeMemoryRegionByteLength;

    /// <summary>Gets the total length, in bytes, of memory managers used for the memory budget.</summary>
    public ulong TotalByteLength;

    /// <summary>Gets the total length, in bytes, of free memory regions used for the memory budget.</summary>
    public ulong TotalFreeMemoryRegionByteLength;
}
