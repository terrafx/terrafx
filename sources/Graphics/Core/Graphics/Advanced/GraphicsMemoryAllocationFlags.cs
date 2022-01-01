// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ALLOCATION_FLAGS enum from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaAllocationCreateFlagBits enum from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines flags that modify how a memory region is allocated.</summary>
[Flags]
public enum GraphicsMemoryAllocationFlags
{
    /// <summary>No flags are specified.</summary>
    None = 0,

    /// <summary>Indicates the memory region should get its own dedicated memory allocator.</summary>
    /// <remarks>This flag cannot be combined with <see cref="ExistingMemoryAllocator" />.</remarks>
    DedicatedMemoryAllocator = 1 << 0,

    /// <summary>Indicates the memory region should only come from an existing memory allocator.</summary>
    /// <remarks>This flag cannot be combined with <see cref="DedicatedMemoryAllocator" />.</remarks>
    ExistingMemoryAllocator = 1 << 1,

    /// <summary>Indicates the memory region can be allocated even if it exceeds the memory budget.</summary>
    CanExceedBudget = 1 << 2,
}
