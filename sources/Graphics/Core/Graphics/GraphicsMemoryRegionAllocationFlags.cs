// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ALLOCATION_FLAGS enum from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaAllocationCreateFlagBits enum from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics;

/// <summary>Defines flags that modify how a memory region is allocated.</summary>
[Flags]
public enum GraphicsMemoryRegionAllocationFlags
{
    /// <summary>No flags are specified.</summary>
    None = 0,

    /// <summary>Indicates the region should get its own dedicated collection.</summary>
    /// <remarks>This flag cannot be combined with <see cref="ExistingCollection" />.</remarks>
    DedicatedCollection = 1 << 0,

    /// <summary>Indicates the region should only come from an existing collection.</summary>
    /// <remarks>This flag cannot be combined with <see cref="DedicatedCollection" />.</remarks>
    ExistingCollection = 1 << 1,

    /// <summary>Indicates the region should only be allocated if it doesn't exceed the memory budget.</summary>
    WithinBudget = 1 << 2,
}
