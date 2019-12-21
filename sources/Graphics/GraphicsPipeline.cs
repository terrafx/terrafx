// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics pipeline which defines how a graphics primitive should be rendered.</summary>
    public abstract class GraphicsPipeline : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly GraphicsPipelineSignature _signature;
        private readonly GraphicsShader? _vertexShader;
        private readonly GraphicsShader? _pixelShader;

        /// <summary>Initializes a new instance of the <see cref="GraphicsPipeline" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device for which the pipeline was created.</param>
        /// <param name="signature">The signature which details the inputs given and resources available to the pipeline.</param>
        /// <param name="vertexShader">The vertex shader for the pipeline or <c>null</c> if none exists.</param>
        /// <param name="pixelShader">The pixel shader for the pipeline or <c>null</c> if none exists.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="signature" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexShader" /> is not <see cref="GraphicsShaderKind.Vertex"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexShader" /> was not created for <paramref name="graphicsDevice" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelShader" /> is not <see cref="GraphicsShaderKind.Pixel"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelShader" />was not created for <paramref name="graphicsDevice" />.</exception>
        protected GraphicsPipeline(GraphicsDevice graphicsDevice, GraphicsPipelineSignature signature, GraphicsShader? vertexShader, GraphicsShader? pixelShader)
        {
            ThrowIfNull(graphicsDevice, nameof(graphicsDevice));
            ThrowIfNull(signature, nameof(signature));

            if ((vertexShader != null) && ((vertexShader.Kind != GraphicsShaderKind.Vertex) || (vertexShader.GraphicsDevice != graphicsDevice)))
            {
                ThrowArgumentOutOfRangeException(nameof(vertexShader), vertexShader);
            }

            if ((pixelShader != null) && ((pixelShader.Kind != GraphicsShaderKind.Pixel) || (pixelShader.GraphicsDevice != graphicsDevice)))
            {
                ThrowArgumentOutOfRangeException(nameof(pixelShader), pixelShader);;
            }

            _graphicsDevice = graphicsDevice;
            _signature = signature;
            _vertexShader = vertexShader;
            _pixelShader = pixelShader;
        }

        /// <summary>Gets the graphics device for which the pipeline was created.</summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets <c>true</c> if the pipeline has a pixel shader; otherwise, <c>false</c>.</summary>
        public bool HasPixelShader => _pixelShader != null;

        /// <summary>Gets <c>true</c> if the pipeline has a vertex shader; otherwise, <c>false</c>.</summary>
        public bool HasVertexShader => _vertexShader != null;

        /// <summary>Gets the pixel shader for the pipeline or <c>null</c> if none exists.</summary>
        public GraphicsShader? PixelShader => _pixelShader;

        /// <summary>Gets the signature of the pipeline.</summary>
        public GraphicsPipelineSignature Signature => _signature;

        /// <summary>Gets the vertex shader for the pipeline or <c>null</c> if none exists.</summary>
        public GraphicsShader? VertexShader => _vertexShader;

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
