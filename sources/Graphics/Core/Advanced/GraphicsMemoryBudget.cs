// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Budget struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBudget struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Runtime.InteropServices;

namespace TerraFX.Advanced;

/// <summary>Describes data used for budgeting graphics memory.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct GraphicsMemoryBudget
{
    /// <summary>Gets the estimated, in bytes, memory budget.</summary>
    public ulong EstimatedMemoryBudget { get; init; }

    /// <summary>Gets the estimated, in bytes, memory usage.</summary>
    public ulong EstimatedMemoryUsage { get; init; }

    /// <summary>Gets the total size, in bytes, of allocated memory regions used for the memory budget.</summary>
    public ulong TotalAllocatedMemoryRegionSize => TotalSize - TotalFreeMemoryRegionSize;

    /// <summary>Gets the total size, in bytes, of free memory regions used for the memory budget.</summary>
    public ulong TotalFreeMemoryRegionSize { get; init; }

    /// <summary>Gets the total size, in bytes, of memory managers used for the memory budget.</summary>
    public ulong TotalSize { get; init; }
}
