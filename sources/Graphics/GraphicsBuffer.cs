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
        /// <param name="graphicsDevice">The graphics device for which the buffer was created.</param>
        /// <param name="size">The size, in bytes, of the buffer.</param>
        /// <param name="stride">The size, in bytes, of the buffer elements.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="stride" /> is not <c>2</c> or <c>4</c>.</exception>
        protected GraphicsBuffer(GraphicsBufferKind kind, GraphicsDevice graphicsDevice, ulong size, ulong stride)
            : base(graphicsDevice, size)
        {
            if ((kind == GraphicsBufferKind.Index) && (stride != 2) && (stride != 4))
            {
                ThrowArgumentOutOfRangeException(nameof(stride), stride);
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
