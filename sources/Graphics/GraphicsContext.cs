// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public abstract class GraphicsContext : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly int _index;

        /// <summary>Initializes a new instance of the <see cref="GraphicsDevice" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device for which the context was created.</param>
        /// <param name="index">An index which can be used to lookup the context via <see cref="GraphicsDevice.GraphicsContexts" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is <c>negative</c>.</exception>
        protected GraphicsContext(GraphicsDevice graphicsDevice, int index)
        {
            ThrowIfNull(graphicsDevice, nameof(graphicsDevice));
            ThrowIfNegative(index, nameof(index));

            _graphicsDevice = graphicsDevice;
            _index = index;
        }

        /// <summary>Gets the <see cref="GraphicsDevice" /> for the context.</summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the graphics fence used by the context for synchronization.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract GraphicsFence GraphicsFence { get; }

        /// <summary>Gets an index which can be used to lookup the context via <see cref="GraphicsDevice.GraphicsContexts" />.</summary>
        public int Index => _index;

        /// <summary>Begins a new frame for rendering.</summary>
        /// <param name="backgroundColor">A color to which the background should be cleared.</param>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void BeginFrame(ColorRgba backgroundColor);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Draws a graphics primitive to the render surface.</summary>
        /// <param name="graphicsPrimitive">The graphics primitive to draw.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsPrimitive" /> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void Draw(GraphicsPrimitive graphicsPrimitive);

        /// <summary>Ends the frame currently be rendered.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void EndFrame();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
