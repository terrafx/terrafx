// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Defines a single block of memory which can contain allocated or free regions.</summary>
    public abstract unsafe partial class GraphicsMemoryBlock : IDisposable
    {
        private readonly GraphicsMemoryBlockCollection _collection;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryBlock" /> class.</summary>
        /// <param name="collection">The block collection which contains the memory block.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
        internal GraphicsMemoryBlock(GraphicsMemoryBlockCollection collection)
        {
            ThrowIfNull(collection, nameof(collection));
            _collection = collection;
        }

        /// <summary>Gets the number of allocated regions in the memory block.</summary>
        public abstract nuint AllocatedRegionCount { get; }

        /// <summary>Gets the block collection which contains the memory block.</summary>
        public GraphicsMemoryBlockCollection Collection => _collection;

        /// <summary>Gets <c>true</c> if the memory block contains no allocated regions; otherwise, <c>false</c>.</summary>
        public abstract bool IsEmpty { get; }

        /// <summary>Gets the size of the largest free region, in bytes.</summary>
        public abstract ulong LargestFreeRegionSize { get; }

        /// <summary>Gets the minimum size of free regions to keep on either side of an allocated region, in bytes.</summary>
        public abstract ulong MarginSize { get; }

        /// <summary>Gets the minimum size of a free region for it to be registered as available, in bytes.</summary>
        public abstract ulong MinimumFreeRegionSizeToRegister { get; }

        /// <summary>Gets the size of the memory block, in bytes.</summary>
        public abstract ulong Size { get; }

        /// <summary>Gets the total size of free regions, in bytes.</summary>
        public abstract ulong TotalFreeRegionSize { get; }

        /// <summary>Clears the memory block of allocated regions.</summary>
        public abstract void Clear();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Frees a region of memory.</summary>
        /// <param name="region">The region of memory to be freed.</param>
        /// <exception cref="KeyNotFoundException"><paramref name="region" /> was not found in the memory block.</exception>
        public abstract void Free(in GraphicsMemoryBlockRegion region);

        /// <summary>Gets the underlying handle used to manage state for the memory block.</summary>
        /// <typeparam name="T">The type of the handle.</typeparam>
        /// <returns>The underlying handle used to manage state for the memory block.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T" /> is not a supported type.</exception>
        public abstract T GetHandle<T>()
            where T : unmanaged;

        /// <summary>Tries to allocate a region of the specified size and alignment.</summary>
        /// <param name="size">The size of the region to allocate, in bytes.</param>
        /// <param name="alignment">The alignment of the region to allocate.</param>
        /// <param name="region">On return, contains the allocated region or <c>default</c> if the allocation failed.</param>
        /// <returns><c>true</c> if a region was sucesfully allocated; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment" /> is not a <c>power of two</c>.</exception>
        public abstract bool TryAllocate(ulong size, ulong alignment, out GraphicsMemoryBlockRegion region);

        /// <summary>Performs validation of the memory block to ensure it is correct.</summary>
        [Conditional("DEBUG")]
        public abstract void Validate();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
