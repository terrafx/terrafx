// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics resource bound to a graphics device.</summary>
    public abstract unsafe class GraphicsResource : IDisposable
    {
        private readonly GraphicsMemoryAllocator _allocator;
        private readonly GraphicsMemoryBlockRegion _memoryBlockRegion;
        private readonly GraphicsResourceCpuAccess _cpuAccess;

        /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
        /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
        /// <param name="memoryBlockRegion">The memory block region in which the resource exists.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryBlockRegion" />.<see cref="GraphicsMemoryBlockRegion.Block" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryBlockRegion" />.<see cref="GraphicsMemoryBlockRegion.IsAllocated" /> is <c>false</c>.</exception>
        private protected GraphicsResource(GraphicsResourceCpuAccess cpuAccess, in GraphicsMemoryBlockRegion memoryBlockRegion)
        {
            ThrowIfNull(memoryBlockRegion.Block, nameof(memoryBlockRegion));
            ThrowIfFalse(memoryBlockRegion.IsAllocated, nameof(memoryBlockRegion));

            _allocator = memoryBlockRegion.Block.Collection.Allocator;
            _memoryBlockRegion = memoryBlockRegion;
            _cpuAccess = cpuAccess;
        }

        /// <summary>Gets the allocator which created the resource.</summary>
        public GraphicsMemoryAllocator Allocator => _allocator;

        /// <summary>Gets the block which contains the resource.</summary>
        public GraphicsMemoryBlock Block => _memoryBlockRegion.Block;

        /// <summary>Gets the alignment of the resource.</summary>
        public ulong Alignment => MemoryBlockRegion.Alignment;

        /// <summary>Gets the CPU access capabilities of the resource.</summary>
        public GraphicsResourceCpuAccess CpuAccess => _cpuAccess;

        /// <summary>Gets the memory block region in which the resource exists.</summary>
        public ref readonly GraphicsMemoryBlockRegion MemoryBlockRegion => ref _memoryBlockRegion;

        /// <summary>Gets the offset of the resource, in bytes.</summary>
        public ulong Offset => MemoryBlockRegion.Offset;

        /// <summary>Gets the size of the resource, in bytes.</summary>
        public ulong Size => MemoryBlockRegion.Size;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <returns>A pointer to the mapped resource.</returns>
        /// <remarks>This overload should be used when no memory will be read.</remarks>
        public T* Map<T>()
            where T : unmanaged => Map<T>(0, 0);

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="readRange">The range of memory that will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        public T* Map<T>(Range readRange)
            where T : unmanaged
        {
            var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
            var (readRangeOffset, readRangeLength) = readRange.GetOffsetAndLength(size);
            return Map<T>((nuint)readRangeOffset, (nuint)readRangeLength);
        }

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="readRangeOffset">The offset into the resource at which memory will start being read.</param>
        /// <param name="readRangeLength">The amount of memory which will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        public abstract T* Map<T>(nuint readRangeOffset, nuint readRangeLength)
            where T : unmanaged;

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <remarks>This overload should be used when no memory was written.</remarks>
        public void Unmap() => Unmap(0, 0);

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <param name="writtenRange">The range of memory which was written.</param>
        public void Unmap(Range writtenRange)
        {
            var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
            var (writtenRangeOffset, writtenRangeLength) = writtenRange.GetOffsetAndLength(size);
            Unmap((nuint)writtenRangeOffset, (nuint)writtenRangeLength);
        }

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <param name="writtenRangeOffset">The offset into the resource at which memory started being written.</param>
        /// <param name="writtenRangeLength">The amount of memory which was written.</param>
        public abstract void Unmap(nuint writtenRangeOffset, nuint writtenRangeLength);

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
