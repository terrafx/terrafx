// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics buffer which can hold data for a graphics device.</summary>
    public abstract class GraphicsBuffer : IDisposable
    {
        private readonly ulong _size;
        private readonly ulong _stride;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly GraphicsBufferKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsBuffer" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device for which the buffer was created.</param>
        /// <param name="kind">The buffer kind.</param>
        /// <param name="size">The size, in bytes, of the buffer.</param>
        /// <param name="stride">The size, in bytes, of the buffer elements.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is <see cref="GraphicsBufferKind.Index" /> and <paramref name="stride" /> is not <c>2</c> or <c>4</c>.</exception>
        protected GraphicsBuffer(GraphicsDevice graphicsDevice, GraphicsBufferKind kind, ulong size, ulong stride)
        {
            ThrowIfNull(graphicsDevice, nameof(graphicsDevice));

            if ((kind == GraphicsBufferKind.Index) && (stride != 2) && (stride != 4))
            {
                ThrowArgumentOutOfRangeException(nameof(stride), stride);
            }

            _size = size;
            _stride = stride;
            _graphicsDevice = graphicsDevice;
            _kind = kind;
        }

        /// <summary>Gets the graphics device for which the buffer was created.</summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the kind of buffer.</summary>
        public GraphicsBufferKind Kind => _kind;

        /// <summary>Gets the size, in bytes, of the buffer.</summary>
        public ulong Size => _size;

        /// <summary>Gets the size, in bytes, of the buffer elements.</summary>
        public ulong Stride => _stride;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Writes a set of bytes into the buffer.</summary>
        /// <param name="bytes">The bytes which to write.</param>
        public abstract void Write(ReadOnlySpan<byte> bytes);

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
