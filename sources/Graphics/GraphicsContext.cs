// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public abstract class GraphicsContext : IDisposable
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IGraphicsSurface _graphicsSurface;

        /// <summary>Initializes a new instance of the <see cref="GraphicsContext" /> class.</summary>
        /// <param name="graphicsAdapter">The underlying graphics adapter for the context.</param>
        /// <param name="graphicsSurface">The graphics surface on which the context can render.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsAdapter" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsSurface" /> is <c>null</c>.</exception>
        protected GraphicsContext(GraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface)
        {
            ThrowIfNull(graphicsAdapter, nameof(graphicsAdapter));
            ThrowIfNull(graphicsSurface, nameof(graphicsSurface));

            _graphicsAdapter = graphicsAdapter;
            _graphicsSurface = graphicsSurface;
        }

        /// <summary>Gets the <see cref="Graphics.GraphicsAdapter" /> for the instance.</summary>
        public GraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the <see cref="IGraphicsSurface" /> for the instance.</summary>
        public IGraphicsSurface GraphicsSurface => _graphicsSurface;

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

        /// <summary>Ends the frame currently be rendered.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void EndFrame();

        /// <summary>Presents the last frame rendered.</summary>
        /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
        public abstract void PresentFrame();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
