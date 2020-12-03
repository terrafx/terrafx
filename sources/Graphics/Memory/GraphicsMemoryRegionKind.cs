// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the SuballocationType struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaSuballocationType struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX.Graphics
{
    /// <summary>Defines the kind of a memory region.</summary>
    public enum GraphicsMemoryRegionKind
    {
        /// <summary>The memory region is free.</summary>
        Free = 0,

        /// <summary>The memory region is allocated.</summary>
        Allocated = 1,
    }
}
