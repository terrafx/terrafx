// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Budget struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBudget struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System.Runtime.InteropServices;

namespace TerraFX.Graphics;

/// <summary>Describes data used for budgeting graphics memory.</summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct GraphicsMemoryBudget
{
    /// <summary>Gets the estimated budget, in bytes, of memory.</summary>
    public ulong EstimatedBudget { get; init; }

    /// <summary>Gets the estimated usage, in bytes, of memory.</summary>
    public ulong EstimatedUsage { get; init; }

    /// <summary>Gets the size, in bytes, of all allocated memory regions for a given set of managers.</summary>
    public ulong TotalAllocatedMemoryRegionSize { get; init; }

    /// <summary>Gets the size, in bytes, of all allocators for a given set of managers.</summary>
    public ulong TotalAllocatorSize { get; init; }
}
