// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics primitive which represents the most basic renderable object.</summary>
    public abstract class GraphicsPrimitive : IDisposable
    {
        private readonly GraphicsDevice _device;
        private readonly GraphicsPipeline _pipeline;
        private readonly GraphicsMemoryRegion<IGraphicsResource> _vertexBufferRegion;
        private readonly GraphicsMemoryRegion<IGraphicsResource> _indexBufferRegion;
        private readonly GraphicsMemoryRegion<IGraphicsResource>[] _inputResourceRegions;

        /// <summary>Initializes a new instance of the <see cref="GraphicsPrimitive" /> class.</summary>
        /// <param name="device">The device for which the primitive was created.</param>
        /// <param name="pipeline">The pipeline used for rendering the primitive.</param>
        /// <param name="vertexBufferRegion">The buffer region which holds the vertices for the primitive.</param>
        /// <param name="indexBufferRegion">The buffer region which holds the indices for the primitive or <c>default</c> if none exists.</param>
        /// <param name="inputResourceRegions">The resource regions which hold the input data for the primitive or an empty span if none exist.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vertexBufferRegion" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="pipeline" /> was not created for <paramref name="device" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferRegion" /> was not created for <paramref name="device" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBufferRegion" /> was not created for <paramref name="device" />.</exception>
        protected GraphicsPrimitive(GraphicsDevice device, GraphicsPipeline pipeline, in GraphicsMemoryRegion<IGraphicsResource> vertexBufferRegion, in GraphicsMemoryRegion<IGraphicsResource> indexBufferRegion, ReadOnlySpan<GraphicsMemoryRegion<IGraphicsResource>> inputResourceRegions)
        {
            ThrowIfNull(pipeline, nameof(pipeline));
            ThrowIfNull(vertexBufferRegion.Parent, nameof(vertexBufferRegion));

            if (pipeline.Device != device)
            {
                ThrowArgumentOutOfRangeException(pipeline, nameof(pipeline));
            }

            if (vertexBufferRegion.Parent.Allocator.Device != device)
            {
                ThrowArgumentOutOfRangeException(vertexBufferRegion, nameof(vertexBufferRegion));
            }

            if ((indexBufferRegion.Parent is not null) && (indexBufferRegion.Parent.Allocator.Device != device))
            {
                ThrowArgumentOutOfRangeException(indexBufferRegion, nameof(indexBufferRegion));
            }

            _device = device;
            _pipeline = pipeline;

            _vertexBufferRegion = vertexBufferRegion;
            _indexBufferRegion = indexBufferRegion;
            _inputResourceRegions = inputResourceRegions.ToArray();
        }

        /// <summary>Gets the device for which the pipeline was created.</summary>
        public GraphicsDevice Device => _device;

        /// <summary>Gets the resource regions which hold the input data for the primitive or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
        public ReadOnlySpan<GraphicsMemoryRegion<IGraphicsResource>> InputResourceRegions => _inputResourceRegions;

        /// <summary>Gets the buffer region which holds the indices for the primitive or <c>default</c> if none exists.</summary>
        public ref readonly GraphicsMemoryRegion<IGraphicsResource> IndexBufferRegion => ref _indexBufferRegion;

        /// <summary>Gets the pipeline used for rendering the primitive.</summary>
        public GraphicsPipeline Pipeline => _pipeline;

        /// <summary>Gets the buffer region which holds the vertices for the primitive.</summary>
        public ref readonly GraphicsMemoryRegion<IGraphicsResource> VertexBufferRegion => ref _vertexBufferRegion;

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
