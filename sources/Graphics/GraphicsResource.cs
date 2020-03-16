// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics resource bound to a graphics device.</summary>
    public abstract class GraphicsResource : IDisposable
    {
        private readonly ulong _offset;
        private readonly ulong _size;
        private readonly GraphicsHeap _graphicsHeap;

        /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
        /// <param name="graphicsHeap">The graphics heap on which the resource was created.</param>
        /// <param name="offset">The offset, in bytes, of the resource in relation to <paramref name="graphicsHeap" />.</param>
        /// <param name="size">The size, in bytes, of the resource.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsHeap" /> is <c>null</c>.</exception>
        private protected GraphicsResource(GraphicsHeap graphicsHeap, ulong offset, ulong size)
        {
            ThrowIfNull(graphicsHeap, nameof(graphicsHeap));

            _graphicsHeap = graphicsHeap;
            _offset = offset;
            _size = size;
        }

        /// <summary>Gets the graphics heap on which the resource was created.</summary>
        public GraphicsHeap GraphicsHeap => _graphicsHeap;

        /// <summary>Gets the offset, in bytes, of the resource in relation to <see cref="GraphicsHeap" />.</summary>
        public ulong Offset => _offset;

        /// <summary>Gets the size, in bytes, of the resource.</summary>
        public ulong Size => _size;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
