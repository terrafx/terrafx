// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    public partial interface IGraphicsMemoryRegionCollection<TSelf>
    {
        /// <summary>Defines the metadata for a memory region collection.</summary>
        public interface IMetadata
        {
            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.AllocatedRegionCount" />
            nuint AllocatedRegionCount { get; }

            /// <summary>Gets the collection for which the metadata tracks allocated or free regions.</summary>
            TSelf Collection { get; }

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.IsEmpty" />
            bool IsEmpty { get; }

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.LargestFreeRegionSize" />
            ulong LargestFreeRegionSize { get; }

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.MarginSize" />
            ulong MarginSize { get; }

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.MinimumFreeRegionSizeToRegister" />
            ulong MinimumFreeRegionSizeToRegister { get; }

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.Size" />
            ulong Size { get; }

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.TotalFreeRegionSize" />
            ulong TotalFreeRegionSize { get; }

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.Allocate(ulong, ulong, uint)" />
            GraphicsMemoryRegion<TSelf> Allocate(ulong size, ulong alignment, uint stride);

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.Clear()" />
            void Clear();

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.Free(in GraphicsMemoryRegion{TSelf})" />
            void Free(in GraphicsMemoryRegion<TSelf> region);

            /// <summary>Initializes an instance of the <see cref="IMetadata" /> interface.</summary>
            /// <param name="collection">The collection for which the metadata tracks allocated or free regions.</param>
            /// <param name="size">The size of the collection, in bytes.</param>
            /// <param name="marginSize">The minimum size of free regions to keep on either side of an allocated region, in bytes.</param>
            /// <param name="minimumFreeRegionSizeToRegister">The minimum size of a free region for it to be registered as available, in bytes.</param>
            /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is zero.</exception>
            void Initialize(TSelf collection, ulong size, ulong marginSize, ulong minimumFreeRegionSizeToRegister);

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.TryAllocate(ulong, ulong, uint, out GraphicsMemoryRegion{TSelf})" />
            bool TryAllocate(ulong size, ulong alignment, uint stride, out GraphicsMemoryRegion<TSelf> region);

            /// <inheritdoc cref="IGraphicsMemoryRegionCollection{TSelf}.Validate()" />
            void Validate();
        }
    }
}
