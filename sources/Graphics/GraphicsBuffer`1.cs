// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <inheritdoc />
    public abstract unsafe class GraphicsBuffer<TMetadata> : IGraphicsBuffer
        where TMetadata : struct, IGraphicsMemoryRegionCollection<IGraphicsResource>.IMetadata
    {
        private readonly GraphicsMemoryAllocator _allocator;
        private readonly GraphicsMemoryRegion<IGraphicsMemoryBlock> _memoryBlockRegion;
        private readonly GraphicsResourceCpuAccess _cpuAccess;
        private readonly GraphicsBufferKind _kind;

#pragma warning disable CS0649, IDE0044
        private TMetadata _metadata;
#pragma warning restore CS0649, IDE0044

        /// <summary>Initializes a new instance of the <see cref="IGraphicsBuffer" /> class.</summary>
        /// <param name="kind">The buffer kind.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
        /// <param name="memoryBlockRegion">The memory block region in which the resource exists.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryBlockRegion" />.<see cref="GraphicsMemoryRegion{GraphicsMemoryBlock}.Parent" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="memoryBlockRegion" />.<see cref="GraphicsMemoryRegion{GraphicsMemoryBlock}.IsAllocated" /> is <c>false</c>.</exception>
        protected GraphicsBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, in GraphicsMemoryRegion<IGraphicsMemoryBlock> memoryBlockRegion)
        {
            ThrowIfNull(memoryBlockRegion.Parent, nameof(memoryBlockRegion));
            ThrowIfFalse(memoryBlockRegion.IsAllocated, nameof(memoryBlockRegion));

            _allocator = memoryBlockRegion.Parent.Collection.Allocator;
            _memoryBlockRegion = memoryBlockRegion;
            _cpuAccess = cpuAccess;
            _kind = kind;

            var size = memoryBlockRegion.Size;
            var parent = memoryBlockRegion.Parent;
            var marginSize = parent.MarginSize;
            var minimumFreeRegionSizeToRegister = parent.MinimumFreeRegionSizeToRegister;

            if (size < minimumFreeRegionSizeToRegister)
            {
                // Some platforms, such as Vulkan, don't really have a "minimum" resource size
                // and so we need to handle the case where the region is actually smaller than
                // the parents minimum free region size by adjusting the values accordingly.

                marginSize = 0;
                minimumFreeRegionSizeToRegister = size;
            }

            _metadata.Initialize(this, size, marginSize, minimumFreeRegionSizeToRegister);
        }

        /// <inheritdoc />
        public ulong Alignment => _memoryBlockRegion.Alignment;

        /// <inheritdoc />
        public nuint AllocatedRegionCount => _metadata.AllocatedRegionCount;

        /// <inheritdoc />
        public GraphicsMemoryAllocator Allocator => _allocator;

        /// <inheritdoc />
        public IGraphicsMemoryBlock Block => _memoryBlockRegion.Parent;

        /// <inheritdoc />
        public GraphicsResourceCpuAccess CpuAccess => _cpuAccess;

        /// <inheritdoc />
        public bool IsEmpty => _metadata.IsEmpty;

        /// <inheritdoc />
        public GraphicsBufferKind Kind => _kind;

        /// <inheritdoc />
        public ulong LargestFreeRegionSize => _metadata.LargestFreeRegionSize;

        /// <inheritdoc />
        public ulong MarginSize => _metadata.MarginSize;

        /// <inheritdoc />
        public ref readonly GraphicsMemoryRegion<IGraphicsMemoryBlock> MemoryBlockRegion => ref _memoryBlockRegion;

        /// <inheritdoc />
        public ulong MinimumFreeRegionSizeToRegister => _metadata.MinimumFreeRegionSizeToRegister;

        /// <inheritdoc />
        public ulong Offset => _memoryBlockRegion.Offset;

        /// <inheritdoc />
        public ulong Size => _memoryBlockRegion.Size;

        /// <inheritdoc />
        public ulong TotalFreeRegionSize => _metadata.TotalFreeRegionSize;

        /// <inheritdoc />
        public GraphicsMemoryRegion<IGraphicsResource> Allocate(ulong size, ulong alignment, uint stride) => _metadata.Allocate(size, alignment, stride);

        /// <inheritdoc />
        public void Clear() => _metadata.Clear();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Free(in GraphicsMemoryRegion<IGraphicsResource> region) => _metadata.Free(in region);

        /// <inheritdoc />
        public abstract T* Map<T>()
            where T : unmanaged;

        /// <inheritdoc />
        public T* Map<T>(in GraphicsMemoryRegion<IGraphicsResource> region)
            where T : unmanaged => Map<T>((nuint)region.Offset, (nuint)region.Size);

        /// <inheritdoc />
        public T* Map<T>(Range range)
            where T : unmanaged
        {
            var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
            var (rangeOffset, rangeLength) = range.GetOffsetAndLength(size);
            return Map<T>((nuint)rangeOffset, (nuint)rangeLength);
        }

        /// <inheritdoc />
        public abstract T* Map<T>(nuint rangeOffset, nuint rangeLength)
            where T : unmanaged;

        /// <inheritdoc />
        public abstract T* MapForRead<T>()
            where T : unmanaged;

        /// <inheritdoc />
        public T* MapForRead<T>(in GraphicsMemoryRegion<IGraphicsResource> readRegion)
            where T : unmanaged => Map<T>((nuint)readRegion.Offset, (nuint)readRegion.Size);

        /// <inheritdoc />
        public T* MapForRead<T>(Range readRange)
            where T : unmanaged
        {
            var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
            var (readRangeOffset, readRangeLength) = readRange.GetOffsetAndLength(size);
            return MapForRead<T>((nuint)readRangeOffset, (nuint)readRangeLength);
        }

        /// <inheritdoc />
        public abstract T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
            where T : unmanaged;

        /// <inheritdoc />
        public bool TryAllocate(ulong size, ulong alignment, uint stride, out GraphicsMemoryRegion<IGraphicsResource> region) => _metadata.TryAllocate(size, alignment, stride, out region);

        /// <inheritdoc />
        public abstract void Unmap();

        /// <inheritdoc />
        public abstract void UnmapAndWrite();

        /// <inheritdoc />
        public void UnmapAndWrite(in GraphicsMemoryRegion<IGraphicsResource> writtenRegion)
            => UnmapAndWrite((nuint)writtenRegion.Offset, (nuint)writtenRegion.Size);

        /// <inheritdoc />
        public void UnmapAndWrite(Range writtenRange)
        {
            var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
            var (writtenRangeOffset, writtenRangeLength) = writtenRange.GetOffsetAndLength(size);
            UnmapAndWrite((nuint)writtenRangeOffset, (nuint)writtenRangeLength);
        }

        /// <inheritdoc />
        public abstract void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength);

        /// <inheritdoc />
        [Conditional("DEBUG")]
        public void Validate() => _metadata.Validate();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);

        void IGraphicsMemoryRegionCollection<IGraphicsResource>.Validate() => Validate();
    }
}
