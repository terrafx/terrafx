// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics buffer which can hold data for a graphics device.</summary>
    public abstract class GraphicsBuffer : GraphicsResource
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
    }
}
