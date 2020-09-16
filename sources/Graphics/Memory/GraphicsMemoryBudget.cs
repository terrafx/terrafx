// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Budget struct from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBudget struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX.Graphics
{
    /// <summary>Describes data used for budgeting graphics memory.</summary>
    public readonly struct GraphicsMemoryBudget
    {
        private readonly ulong _estimatedBudget;
        private readonly ulong _estimatedUsage;
        private readonly ulong _totalAllocatedRegionSize;
        private readonly ulong _totalBlockSize;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBudget" /> struct.</summary>
        /// <param name="estimatedBudget">The estimated budget of memory, in bytes.</param>
        /// <param name="estimatedUsage">The estimated usage of memory, in bytes.</param>
        /// <param name="totalAllocatedRegionSize">The size of all allocated regions for a given set of collections, in bytes.</param>
        /// <param name="totalBlockSize">The size of all blocks for a given set of collections, in bytes.</param>
        public GraphicsMemoryBudget(ulong estimatedBudget, ulong estimatedUsage, ulong totalAllocatedRegionSize, ulong totalBlockSize)
        {
            _estimatedBudget = estimatedBudget;
            _estimatedUsage = estimatedUsage;
            _totalAllocatedRegionSize = totalAllocatedRegionSize;
            _totalBlockSize = totalBlockSize;
        }

        /// <summary>Gets the estimated budget of memory, in bytes.</summary>
        public ulong EstimatedBudget => _estimatedBudget;

        /// <summary>Gets the estimated usage of memory, in bytes.</summary>
        public ulong EstimatedUsage => _estimatedUsage;

        /// <summary>Gets the size of all allocated regions for a given set of collections, in bytes.</summary>
        public ulong TotalAllocatedRegionSize => _totalAllocatedRegionSize;

        /// <summary>Gets the size of all blocks for a given set of collections, in bytes.</summary>
        public ulong TotalBlockSize => _totalBlockSize;
    }
}
