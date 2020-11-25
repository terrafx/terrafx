// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the ALLOCATION_FLAGS enum from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaAllocationCreateFlagBits enum from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics
{
    /// <summary>Defines flags that modify how a region of memory is allocated.</summary>
    [Flags]
    public enum GraphicsMemoryAllocationFlags
    {
        /// <summary>No flags are specified.</summary>
        None = 0,

        /// <summary>Indicates the allocation should get its own dedicated memory block.</summary>
        /// <remarks>This flag cannot be combined with <see cref="ExistingBlock" />.</remarks>
        DedicatedBlock = 1 << 0,

        /// <summary>Indicates the allocation should only use an existing memory block.</summary>
        /// <remarks>This flag cannot be combined with <see cref="DedicatedBlock" />.</remarks>
        ExistingBlock = 1 << 1,

        /// <summary>Indicates the allocation should only succeed if it doesn't exceed the memory budget.</summary>
        WithinBudget = 1 << 2,
    }
}
