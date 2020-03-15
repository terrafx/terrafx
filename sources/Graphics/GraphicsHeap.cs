// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics heap which provides access to a contiguous region of device memory.</summary>
    public abstract class GraphicsHeap : IDisposable
    {
        private readonly ulong _size;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly GraphicsHeapCpuAccess _cpuAccess;

        /// <summary>Initializes a new instance of the <see cref="GraphicsHeap" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device for which the heap was created.</param>
        /// <param name="size">The size, in bytes, of the heap.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the heap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        protected GraphicsHeap(GraphicsDevice graphicsDevice, ulong size, GraphicsHeapCpuAccess cpuAccess)
        {
            ThrowIfNull(graphicsDevice, nameof(graphicsDevice));

            _graphicsDevice = graphicsDevice;
            _size = size;
            _cpuAccess = cpuAccess;
        }

        /// <summary>Gets the CPU access capabilities of the heap.</summary>
        public GraphicsHeapCpuAccess CpuAccess => _cpuAccess;

        /// <summary>Gets the graphics device to which the resource is bound.</summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the size, in bytes, of the heap.</summary>
        public ulong Size => _size;

        /// <summary>Creates a new graphics buffer for the heap.</summary>
        /// <param name="kind">The kind of graphics buffer to create.</param>
        /// <param name="size">The size, in bytes, of the graphics buffer.</param>
        /// <param name="stride">The size, in bytes, of the elements contained by the buffer.</param>
        /// <returns>A new graphics buffer created for the heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
        /// <exception cref="ObjectDisposedException">The heap has been disposed.</exception>
        public abstract GraphicsBuffer CreateGraphicsBuffer(GraphicsBufferKind kind, ulong size, ulong stride);

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
