// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Utilities;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsPrimitive : GraphicsPrimitive
    {
        private State _state;

        internal D3D12GraphicsPrimitive(D3D12GraphicsDevice graphicsDevice, D3D12GraphicsPipeline graphicsPipeline, D3D12GraphicsBuffer vertexBuffer, D3D12GraphicsBuffer? indexBuffer, ReadOnlySpan<GraphicsResource> inputResources)
            : base(graphicsDevice, graphicsPipeline, vertexBuffer, indexBuffer, inputResources)
        {
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPrimitive" /> class.</summary>
        ~D3D12GraphicsPrimitive()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc cref="GraphicsPrimitive.GraphicsDevice" />
        public D3D12GraphicsDevice D3D12GraphicsDevice => (D3D12GraphicsDevice)GraphicsDevice;

        /// <inheritdoc cref="GraphicsPrimitive.GraphicsPipeline" />
        public D3D12GraphicsPipeline D3D12GraphicsPipeline => (D3D12GraphicsPipeline)GraphicsPipeline;

        /// <inheritdoc cref="GraphicsPrimitive.IndexBuffer" />
        public D3D12GraphicsBuffer? D3D12IndexBuffer => (D3D12GraphicsBuffer?)IndexBuffer;

        /// <inheritdoc cref="GraphicsPrimitive.VertexBuffer" />
        public D3D12GraphicsBuffer D3D12VertexBuffer => (D3D12GraphicsBuffer)VertexBuffer;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeIfNotNull(GraphicsPipeline);
                DisposeIfNotNull(VertexBuffer);
                DisposeIfNotNull(IndexBuffer);

                foreach (var inputResource in InputResources)
                {
                    DisposeIfNotNull(inputResource);
                }
            }

            _state.EndDispose();
        }
    }
}
