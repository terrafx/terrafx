// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics device which provides state management and isolation for a graphics adapter.</summary>
    public abstract class GraphicsDevice : IDisposable
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IGraphicsSurface _graphicsSurface;

        /// <summary>Initializes a new instance of the <see cref="GraphicsDevice" /> class.</summary>
        /// <param name="graphicsAdapter">The underlying graphics adapter for the device.</param>
        /// <param name="graphicsSurface">The graphics surface on which the device can render.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsAdapter" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsSurface" /> is <c>null</c>.</exception>
        protected GraphicsDevice(GraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface)
        {
            ThrowIfNull(graphicsAdapter, nameof(graphicsAdapter));
            ThrowIfNull(graphicsSurface, nameof(graphicsSurface));

            _graphicsAdapter = graphicsAdapter;
            _graphicsSurface = graphicsSurface;
        }

        /// <summary>Gets the <see cref="Graphics.GraphicsAdapter" /> for the instance.</summary>
        public GraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the graphics contexts for the device.</summary>
        public abstract ReadOnlySpan<GraphicsContext> GraphicsContexts { get; }

        /// <summary>Gets an index which can be used to lookup the current graphics context.</summary>
        public abstract int GraphicsContextIndex { get; }

        /// <summary>Gets the <see cref="IGraphicsSurface" /> for the instance.</summary>
        public IGraphicsSurface GraphicsSurface => _graphicsSurface;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Presents the last frame rendered.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract void PresentFrame();

        /// <summary>Waits for the device to become idle.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract void WaitForIdle();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
