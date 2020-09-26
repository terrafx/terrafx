// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaBlockMetadata class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics
{
    public abstract partial class GraphicsMemoryBlock
    {
        /// <summary>Defines the metadata for a single block of memory which can contain allocated or free regions.</summary>
        public interface IMetadata
        {
            /// <inheritdoc cref="GraphicsMemoryBlock.AllocatedRegionCount" />
            nuint AllocatedRegionCount { get; }

            /// <summary>Gets the block for which the metadata tracks allocated or free regions.</summary>
            GraphicsMemoryBlock Block { get; }

            /// <inheritdoc cref="GraphicsMemoryBlock.IsEmpty" />
            bool IsEmpty { get; }

            /// <inheritdoc cref="GraphicsMemoryBlock.LargestFreeRegionSize" />
            ulong LargestFreeRegionSize { get; }

            /// <inheritdoc cref="GraphicsMemoryBlock.MarginSize" />
            ulong MarginSize { get; }

            /// <inheritdoc cref="GraphicsMemoryBlock.MinimumFreeRegionSizeToRegister" />
            ulong MinimumFreeRegionSizeToRegister { get; }

            /// <inheritdoc cref="GraphicsMemoryBlock.Size" />
            ulong Size { get; }

            /// <inheritdoc cref="GraphicsMemoryBlock.TotalFreeRegionSize" />
            ulong TotalFreeRegionSize { get; }

            /// <inheritdoc cref="GraphicsMemoryBlock.Clear()" />
            void Clear();

            /// <inheritdoc cref="GraphicsMemoryBlock.Free(in GraphicsMemoryBlockRegion)" />
            void Free(in GraphicsMemoryBlockRegion region);

            /// <summary>Initializes an instance of the <see cref="IMetadata" /> interface.</summary>
            /// <param name="block">The block for which the metadata tracks allocated or free regions.</param>
            /// <param name="size">The size of the memory block, in bytes.</param>
            /// <param name="marginSize">The minimum size of free regions to keep on either side of an allocated region, in bytes.</param>
            /// <param name="minimumFreeRegionSizeToRegister">The minimum size of a free region for it to be registered as available, in bytes.</param>
            /// <exception cref="ArgumentNullException"><paramref name="block" /> is <c>null</c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is zero.</exception>
            void Initialize(GraphicsMemoryBlock block, ulong size, ulong marginSize, ulong minimumFreeRegionSizeToRegister);

            /// <inheritdoc cref="GraphicsMemoryBlock.TryAllocate(ulong, ulong, out GraphicsMemoryBlockRegion)" />
            bool TryAllocate(ulong size, ulong alignment, out GraphicsMemoryBlockRegion region);

            /// <inheritdoc cref="GraphicsMemoryBlock.Validate()" />
            void Validate();
        }
    }
}
