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
        private readonly GraphicsBufferView _vertexBufferView;
        private readonly GraphicsBufferView _indexBufferView;
        private readonly GraphicsResource[] _inputResources;

        /// <summary>Initializes a new instance of the <see cref="GraphicsPrimitive" /> class.</summary>
        /// <param name="device">The device for which the primitive was created.</param>
        /// <param name="pipeline">The pipeline used for rendering the primitive.</param>
        /// <param name="vertexBufferView">The buffer which holds the vertices for the primitive.</param>
        /// <param name="indexBufferView">The buffer which holds the indices for the primitive or <c>default</c> if none exists.</param>
        /// <param name="inputResources">The resources which hold the input data for the primitive or an empty span if none exist.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vertexBufferView" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="pipeline" /> was not created for <paramref name="device" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferView" /> was not created for <paramref name="device" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBufferView" /> was not created for <paramref name="device" />.</exception>
        protected GraphicsPrimitive(GraphicsDevice device, GraphicsPipeline pipeline, in GraphicsBufferView vertexBufferView, in GraphicsBufferView indexBufferView, ReadOnlySpan<GraphicsResource> inputResources)
        {
            ThrowIfNull(pipeline, nameof(pipeline));
            ThrowIfNull(vertexBufferView.Buffer, nameof(vertexBufferView));

            if (pipeline.Device != device)
            {
                ThrowArgumentOutOfRangeException(nameof(pipeline), pipeline);
            }

            if (vertexBufferView.Buffer.Allocator.Device != device)
            {
                ThrowArgumentOutOfRangeException(nameof(vertexBufferView), vertexBufferView);
            }

            if ((indexBufferView.Buffer is not null) && (indexBufferView.Buffer.Allocator.Device != device))
            {
                ThrowArgumentOutOfRangeException(nameof(indexBufferView), indexBufferView);
            }

            _device = device;
            _pipeline = pipeline;

            _vertexBufferView = vertexBufferView;
            _indexBufferView = indexBufferView;
            _inputResources = inputResources.ToArray();
        }

        /// <summary>Gets the device for which the pipeline was created.</summary>
        public GraphicsDevice Device => _device;

        /// <summary>Gets the resources which hold the input data for the primitive or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
        public ReadOnlySpan<GraphicsResource> InputResources => _inputResources;

        /// <summary>Gets the buffer which holds the indices for the primitive or <c>default</c> if none exists.</summary>
        public ref readonly GraphicsBufferView IndexBufferView => ref _indexBufferView;

        /// <summary>Gets the pipeline used for rendering the primitive.</summary>
        public GraphicsPipeline Pipeline => _pipeline;

        /// <summary>Gets the buffer which holds the vertices for the primitive.</summary>
        public ref readonly GraphicsBufferView VertexBufferView => ref _vertexBufferView;

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
