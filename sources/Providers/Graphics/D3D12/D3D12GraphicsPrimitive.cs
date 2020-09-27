// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Utilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsPrimitive : GraphicsPrimitive
    {
        private State _state;

        internal D3D12GraphicsPrimitive(D3D12GraphicsDevice device, D3D12GraphicsPipeline pipeline, in GraphicsBufferView vertexBufferView, in GraphicsBufferView indexBufferView, ReadOnlySpan<GraphicsResource> inputResources)
            : base(device, pipeline, in vertexBufferView, in indexBufferView, inputResources)
        {
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPrimitive" /> class.</summary>
        ~D3D12GraphicsPrimitive() => Dispose(isDisposing: false);

        /// <inheritdoc cref="GraphicsPrimitive.Device" />
        public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

        /// <inheritdoc cref="GraphicsPrimitive.Pipeline" />
        public new D3D12GraphicsPipeline Pipeline => (D3D12GraphicsPipeline)base.Pipeline;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                Pipeline?.Dispose();

                foreach (var inputResource in InputResources)
                {
                    inputResource?.Dispose();
                }

                VertexBufferView.Buffer?.Dispose();
                IndexBufferView.Buffer?.Dispose();
            }

            _state.EndDispose();
        }
    }
}
