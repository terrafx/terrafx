// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics primitive which represents the most basic renderable object.</summary>
    public abstract class GraphicsPrimitive : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly GraphicsPipeline _graphicsPipeline;
        private readonly GraphicsBuffer _vertexBuffer;
        private readonly GraphicsBuffer? _indexBuffer;
        private readonly GraphicsResource[] _inputResources;

        /// <summary>Initializes a new instance of the <see cref="GraphicsPrimitive" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device for which the primitive was created.</param>
        /// <param name="graphicsPipeline">The graphics pipeline used for rendering the primitive.</param>
        /// <param name="vertexBuffer">The graphics buffer which holds the vertices for the primitive.</param>
        /// <param name="indexBuffer">The graphics buffer which holds the indices for the primitive or <c>null</c> if none exists.</param>
        /// <param name="inputResources">The graphics resources which hold the input data for the primitive or an empty span if none exist.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsPipeline" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vertexBuffer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="graphicsPipeline" /> was not created for <paramref name="graphicsDevice" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBuffer" /> was not created for <paramref name="graphicsDevice" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBuffer" /> was not created for <paramref name="graphicsDevice" />.</exception>
        protected GraphicsPrimitive(GraphicsDevice graphicsDevice, GraphicsPipeline graphicsPipeline, GraphicsBuffer vertexBuffer, GraphicsBuffer? indexBuffer, ReadOnlySpan<GraphicsResource> inputResources)
        {
            ThrowIfNull(graphicsPipeline, nameof(graphicsPipeline));
            ThrowIfNull(vertexBuffer, nameof(vertexBuffer));

            if (graphicsPipeline.GraphicsDevice != graphicsDevice)
            {
                ThrowArgumentOutOfRangeException(nameof(graphicsPipeline), graphicsPipeline);
            }

            if (vertexBuffer.GraphicsHeap.GraphicsDevice != graphicsDevice)
            {
                ThrowArgumentOutOfRangeException(nameof(vertexBuffer), vertexBuffer);
            }

            if ((indexBuffer != null) && (indexBuffer.GraphicsHeap.GraphicsDevice != graphicsDevice))
            {
                ThrowArgumentOutOfRangeException(nameof(indexBuffer), indexBuffer);
            }

            _graphicsDevice = graphicsDevice;
            _graphicsPipeline = graphicsPipeline;

            _vertexBuffer = vertexBuffer;
            _indexBuffer = indexBuffer;
            _inputResources = inputResources.ToArray();
        }

        /// <summary>Gets the graphics resources which hold the input data for the primitive or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
        public ReadOnlySpan<GraphicsResource> InputResources => _inputResources;

        /// <summary>Gets the graphics device for which the pipeline was created.</summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the graphics pipeline used for rendering the primitive.</summary>
        public GraphicsPipeline GraphicsPipeline => _graphicsPipeline;

        /// <summary>Gets the graphics buffer which holds the indices for the primitive or <c>null</c> if none exists.</summary>
        public GraphicsBuffer? IndexBuffer => _indexBuffer;

        /// <summary>Gets the graphics buffer which holds the vertices for the primitive.</summary>
        public GraphicsBuffer VertexBuffer => _vertexBuffer;

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
