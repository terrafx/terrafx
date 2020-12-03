// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;

namespace TerraFX.Graphics
{
    /// <summary>Defines a collection which can contain allocated or free regions.</summary>
    public partial interface IGraphicsMemoryRegionCollection<TSelf>
        where TSelf : class, IGraphicsMemoryRegionCollection<TSelf>
    {
        /// <summary>Gets the number of allocated regions in the collection.</summary>
        nuint AllocatedRegionCount { get; }

        /// <summary>Gets <c>true</c> if the collection contains no allocated regions; otherwise, <c>false</c>.</summary>
        bool IsEmpty { get; }

        /// <summary>Gets the size of the largest free region, in bytes.</summary>
        ulong LargestFreeRegionSize { get; }

        /// <summary>Gets the minimum size of free regions to keep on either side of an allocated region, in bytes.</summary>
        ulong MarginSize { get; }

        /// <summary>Gets the minimum size of a free region for it to be registered as available, in bytes.</summary>
        ulong MinimumFreeRegionSizeToRegister { get; }

        /// <summary>Gets the size of the collection, in bytes.</summary>
        ulong Size { get; }

        /// <summary>Gets the total size of free regions, in bytes.</summary>
        ulong TotalFreeRegionSize { get; }

        /// <summary>Allocates a region of the specified size and alignment.</summary>
        /// <param name="size">The size of the region to allocate, in bytes.</param>
        /// <param name="alignment">The alignment of the region to allocate, in bytes.</param>
        /// <param name="stride">The stride of the region, in bytes.</param>
        /// <returns>The allocated region.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="stride" /> is <c>zero</c>.</exception>
        GraphicsMemoryRegion<TSelf> Allocate(ulong size, ulong alignment, uint stride);

        /// <summary>Clears the collection of allocated regions.</summary>
        void Clear();

        /// <summary>Frees a region of memory.</summary>
        /// <param name="region">The region of memory to be freed.</param>
        /// <exception cref="KeyNotFoundException"><paramref name="region" /> was not found in the collection.</exception>
        void Free(in GraphicsMemoryRegion<TSelf> region);

        /// <summary>Tries to allocate a region of the specified size and alignment.</summary>
        /// <param name="size">The size of the region to allocate, in bytes.</param>
        /// <param name="alignment">The alignment of the region to allocate, in bytes.</param>
        /// <param name="stride">The stride of the region, in bytes.</param>
        /// <param name="region">On return, contains the allocated region or <c>default</c> if the allocation failed.</param>
        /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="stride" /> is <c>zero</c>.</exception>
        bool TryAllocate(ulong size, ulong alignment, uint stride, out GraphicsMemoryRegion<TSelf> region);

        /// <summary>Performs validation of the collection to ensure it is correct.</summary>
        void Validate();
    }
}
