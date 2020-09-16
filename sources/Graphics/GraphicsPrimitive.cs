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
        private readonly GraphicsBufferView _vertexBuffer;
        private readonly GraphicsBufferView _indexBuffer;
        private readonly GraphicsResource[] _inputResources;

        /// <summary>Initializes a new instance of the <see cref="GraphicsPrimitive" /> class.</summary>
        /// <param name="device">The device for which the primitive was created.</param>
        /// <param name="pipeline">The pipeline used for rendering the primitive.</param>
        /// <param name="vertexBuffer">The buffer which holds the vertices for the primitive.</param>
        /// <param name="indexBuffer">The buffer which holds the indices for the primitive or <c>default</c> if none exists.</param>
        /// <param name="inputResources">The resources which hold the input data for the primitive or an empty span if none exist.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vertexBuffer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="pipeline" /> was not created for <paramref name="device" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBuffer" /> was not created for <paramref name="device" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBuffer" /> was not created for <paramref name="device" />.</exception>
        protected GraphicsPrimitive(GraphicsDevice device, GraphicsPipeline pipeline, in GraphicsBufferView vertexBuffer, in GraphicsBufferView indexBuffer, ReadOnlySpan<GraphicsResource> inputResources)
        {
            ThrowIfNull(pipeline, nameof(pipeline));
            ThrowIfNull(vertexBuffer.Buffer, nameof(vertexBuffer));

            if (pipeline.Device != device)
            {
                ThrowArgumentOutOfRangeException(nameof(pipeline), pipeline);
            }

            if (vertexBuffer.Buffer.Allocator.Device != device)
            {
                ThrowArgumentOutOfRangeException(nameof(vertexBuffer), vertexBuffer);
            }

            if ((indexBuffer.Buffer is not null) && (indexBuffer.Buffer.Allocator.Device != device))
            {
                ThrowArgumentOutOfRangeException(nameof(indexBuffer), indexBuffer);
            }

            _device = device;
            _pipeline = pipeline;

            _vertexBuffer = vertexBuffer;
            _indexBuffer = indexBuffer;
            _inputResources = inputResources.ToArray();
        }

        /// <summary>Gets the device for which the pipeline was created.</summary>
        public GraphicsDevice Device => _device;

        /// <summary>Gets the resources which hold the input data for the primitive or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
        public ReadOnlySpan<GraphicsResource> InputResources => _inputResources;

        /// <summary>Gets the buffer which holds the indices for the primitive or <c>default</c> if none exists.</summary>
        public ref readonly GraphicsBufferView IndexBufferView => ref _indexBuffer;

        /// <summary>Gets the pipeline used for rendering the primitive.</summary>
        public GraphicsPipeline Pipeline => _pipeline;

        /// <summary>Gets the buffer which holds the vertices for the primitive.</summary>
        public ref readonly GraphicsBufferView VertexBufferView => ref _vertexBuffer;

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
