// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Defines a single block of memory which can contain allocated or free regions.</summary>
    public abstract partial class GraphicsMemoryBlock : IDisposable, IGraphicsMemoryRegionCollection<GraphicsMemoryBlock>
    {
        private readonly GraphicsMemoryBlockCollection _collection;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBlock" /> class.</summary>
        /// <param name="collection">The memory block collection which contains the block.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
        protected GraphicsMemoryBlock(GraphicsMemoryBlockCollection collection)
        {
            ThrowIfNull(collection, nameof(collection));
            _collection = collection;
        }

        /// <inheritdoc />
        public abstract int AllocatedRegionCount { get; }

        /// <summary>Gets the memory block collection which contains the block.</summary>
        public GraphicsMemoryBlockCollection Collection => _collection;

        /// <summary>Gets the number of regions in the block.</summary>
        public abstract int Count { get; }

        /// <inheritdoc />
        public abstract bool IsEmpty { get; }

        /// <inheritdoc />
        public abstract ulong LargestFreeRegionSize { get; }

        /// <inheritdoc />
        public abstract ulong MinimumAllocatedRegionMarginSize { get; }

        /// <inheritdoc />
        public abstract ulong MinimumFreeRegionSizeToRegister { get; }

        /// <inheritdoc />
        public abstract ulong Size { get; }

        /// <inheritdoc />
        public abstract ulong TotalFreeRegionSize { get; }

        /// <inheritdoc />
        public abstract GraphicsMemoryRegion<GraphicsMemoryBlock> Allocate(ulong size, ulong alignment = 1);

        /// <inheritdoc />
        public abstract void Clear();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public abstract void Free(in GraphicsMemoryRegion<GraphicsMemoryBlock> region);

        /// <summary>Gets an enumerator that can be used to iterate through the regions of the block.</summary>
        /// <returns>An enumerator that can be used to iterate through the regions of the block.</returns>
        public abstract IEnumerator<GraphicsMemoryRegion<GraphicsMemoryBlock>> GetEnumerator();

        /// <inheritdoc />
        public abstract bool TryAllocate(ulong size, [Optional, DefaultParameterValue(1UL)] ulong alignment, out GraphicsMemoryRegion<GraphicsMemoryBlock> region);

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
