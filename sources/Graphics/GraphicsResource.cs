// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics resource bound to a graphics device.</summary>
    public abstract unsafe class GraphicsResource : IDisposable
    {
        private readonly ulong _size;
        private readonly GraphicsDevice _graphicsDevice;

        /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device for which the resource was created.</param>
        /// <param name="size">The size, in bytes, of the resource.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        private protected GraphicsResource(GraphicsDevice graphicsDevice, ulong size)
        {
            ThrowIfNull(graphicsDevice, nameof(graphicsDevice));

            _graphicsDevice = graphicsDevice;
            _size = size;
        }

        /// <summary>Gets the graphics device to which the resource is bound.</summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the size, in bytes, of the resource.</summary>
        public ulong Size => _size;

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

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
