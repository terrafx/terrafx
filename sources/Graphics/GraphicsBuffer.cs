// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics buffer which can hold data for a graphics device.</summary>
    public abstract unsafe class GraphicsBuffer : GraphicsResource
    {
        private readonly ulong _stride;
        private readonly GraphicsBufferKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsBuffer" /> class.</summary>
        /// <param name="kind">The buffer kind.</param>
        /// <param name="graphicsHeap">The graphics heap on which the buffer was created.</param>
        /// <param name="offset">The offset, in bytes, of the buffer in relation to <paramref name="graphicsHeap" />.</param>
        /// <param name="size">The size, in bytes, of the buffer.</param>
        /// <param name="stride">The size, in bytes, of the elements contained by the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsHeap" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="stride" /> is not <c>2</c> or <c>4</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is <see cref="GraphicsBufferKind.Staging" /> and <paramref name="graphicsHeap" /> is not <see cref="GraphicsHeapCpuAccess.Write" />.</exception>
        protected GraphicsBuffer(GraphicsBufferKind kind, GraphicsHeap graphicsHeap, ulong offset, ulong size, ulong stride)
            : base(graphicsHeap, offset, size)
        {
            switch (kind)
            {
                case GraphicsBufferKind.Index:
                {
                    if ((stride != 2) && (stride != 4))
                    {
                        ThrowArgumentOutOfRangeException(nameof(stride), stride);
                    }
                    break;
                }

                case GraphicsBufferKind.Staging:
                {
                    if (graphicsHeap.CpuAccess != GraphicsHeapCpuAccess.Write)
                    {
                        ThrowArgumentOutOfRangeException(nameof(graphicsHeap), graphicsHeap);
                    }
                    break;
                }
            }

            _stride = stride;
            _kind = kind;
        }

        /// <summary>Gets the kind of buffer.</summary>
        public GraphicsBufferKind Kind => _kind;

        /// <summary>Gets the size, in bytes, of the buffer elements.</summary>
        public ulong Stride => _stride;

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <returns>A pointer to the mapped resource.</returns>
        /// <remarks>This overload should be used when no memory will be read.</remarks>
        public T* Map<T>()
            where T : unmanaged => Map<T>(UIntPtr.Zero, UIntPtr.Zero);

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="readRange">The range of memory that will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        public T* Map<T>(Range readRange)
            where T : unmanaged
        {
            var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
            var (readRangeOffset, readRangeLength) = readRange.GetOffsetAndLength(size);
            return Map<T>((UIntPtr)readRangeOffset, (UIntPtr)readRangeLength);
        }

        /// <summary>Maps the resource into CPU memory.</summary>
        /// <typeparam name="T">The type of data contained by the resource.</typeparam>
        /// <param name="readRangeOffset">The offset into the resource at which memory will start being read.</param>
        /// <param name="readRangeLength">The amount of memory which will be read.</param>
        /// <returns>A pointer to the mapped resource.</returns>
        public abstract T* Map<T>(UIntPtr readRangeOffset, UIntPtr readRangeLength)
            where T : unmanaged;

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <remarks>This overload should be used when no memory was written.</remarks>
        public void Unmap() => Unmap(UIntPtr.Zero, UIntPtr.Zero);

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <param name="writtenRange">The range of memory which was written.</param>
        public void Unmap(Range writtenRange)
        {
            var size = (Size < int.MaxValue) ? (int)Size : int.MaxValue;
            var (writtenRangeOffset, writtenRangeLength) = writtenRange.GetOffsetAndLength(size);
            Unmap((UIntPtr)writtenRangeOffset, (UIntPtr)writtenRangeLength);
        }

        /// <summary>Unmaps the resource from CPU memory.</summary>
        /// <param name="writtenRangeOffset">The offset into the resource at which memory started being written.</param>
        /// <param name="writtenRangeLength">The amount of memory which was written.</param>
        public abstract void Unmap(UIntPtr writtenRangeOffset, UIntPtr writtenRangeLength);
    }
}
