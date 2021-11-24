// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics;

public partial interface IGraphicsMemoryRegionCollection<TSelf>
{
    /// <summary>Defines metadata for a collection of memory regions.</summary>
    public interface IMetadata : IGraphicsMemoryRegionCollection<TSelf>
    {
        /// <summary>Initializes an instance of the <see cref="IMetadata" /> interface.</summary>
        /// <param name="collection">The collection which contains the metadata.</param>
        /// <param name="size">The size of the collection, in bytes.</param>
        /// <param name="minimumAllocatedRegionMarginSize">The minimum size of free regions to keep on either side of an allocated region, in bytes.</param>
        /// <param name="minimumFreeRegionSizeToRegister">The minimum size of a free region for it to be registered as available, in bytes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        void Initialize(TSelf collection, ulong size, ulong minimumAllocatedRegionMarginSize, ulong minimumFreeRegionSizeToRegister);
    }
}
