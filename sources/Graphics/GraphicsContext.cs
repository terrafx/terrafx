// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public abstract class GraphicsContext : IDisposable
    {
        private readonly GraphicsDevice _device;
        private readonly int _index;

        /// <summary>Initializes a new instance of the <see cref="GraphicsContext" /> class.</summary>
        /// <param name="device">The device for which the context was created.</param>
        /// <param name="index">An index which can be used to lookup the context via <see cref="GraphicsDevice.Contexts" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is <c>negative</c>.</exception>
        protected GraphicsContext(GraphicsDevice device, int index)
        {
            ThrowIfNull(device, nameof(device));
            ThrowIfNegative(index, nameof(index));

            _device = device;
            _index = index;
        }

        /// <summary>Gets the device which manages the context.</summary>
        public GraphicsDevice Device => _device;

        /// <summary>Gets the fence used by the context for synchronization.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract GraphicsFence Fence { get; }

        /// <summary>Gets an index which can be used to lookup the context via <see cref="GraphicsDevice.Contexts" />.</summary>
        public int Index => _index;

        /// <summary>Begins the drawing stage.</summary>
        /// <param name="backgroundColor">A color to which the background should be cleared.</param>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void BeginDrawing(ColorRgba backgroundColor);

        /// <summary>Begins a new frame.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void BeginFrame();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Copies the contents of a buffer to a separate buffer.</summary>
        /// <param name="destination">The destination buffer.</param>
        /// <param name="source">The source buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void Copy(IGraphicsBuffer destination, IGraphicsBuffer source);

        /// <summary>Copies the contents of a buffer to a two-dimensional texture.</summary>
        /// <param name="destination">The destination two-dimensional texture.</param>
        /// <param name="source">The source buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void Copy(IGraphicsTexture destination, IGraphicsBuffer source);

        /// <summary>Draws a primitive to the render surface.</summary>
        /// <param name="primitive">The primitive to draw.</param>
        /// <exception cref="ArgumentNullException"><paramref name="primitive" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void Draw(GraphicsPrimitive primitive);

        /// <summary>Ends the drawing stage.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void EndDrawing();

        /// <summary>Ends the frame.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void EndFrame();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
