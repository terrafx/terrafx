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
    /// <summary>Gets the estimated budget of memory, in bytes.</summary>
    public ulong EstimatedBudget { get; init; }

    /// <summary>Gets the estimated usage of memory, in bytes.</summary>
    public ulong EstimatedUsage { get; init; }

    /// <summary>Gets the size of all allocated regions for a given set of collections, in bytes.</summary>
    public ulong TotalAllocatedRegionSize { get; init; }

    /// <summary>Gets the size of all blocks for a given set of collections, in bytes.</summary>
    public ulong TotalBlockSize { get; init; }
}
