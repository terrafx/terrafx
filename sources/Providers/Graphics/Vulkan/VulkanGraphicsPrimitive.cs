// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Utilities;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsPrimitive : GraphicsPrimitive
    {
        private State _state;

        internal VulkanGraphicsPrimitive(VulkanGraphicsDevice graphicsDevice, VulkanGraphicsPipeline graphicsPipeline, VulkanGraphicsBuffer vertexBuffer, VulkanGraphicsBuffer? indexBuffer, ReadOnlySpan<GraphicsBuffer> inputBuffers = default)
            : base(graphicsDevice, graphicsPipeline, vertexBuffer, indexBuffer, inputBuffers)
        {
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPrimitive" /> class.</summary>
        ~VulkanGraphicsPrimitive()
        {
            Dispose(isDisposing: true);
        }

        /// <inheritdoc cref="GraphicsPrimitive.GraphicsDevice" />
        public VulkanGraphicsDevice VulkanGraphicsDevice => (VulkanGraphicsDevice)GraphicsDevice;

        /// <inheritdoc cref="GraphicsPrimitive.GraphicsPipeline" />
        public VulkanGraphicsPipeline VulkanGraphicsPipeline => (VulkanGraphicsPipeline)GraphicsPipeline;

        /// <inheritdoc cref="GraphicsPrimitive.IndexBuffer" />
        public VulkanGraphicsBuffer? VulkanIndexBuffer => (VulkanGraphicsBuffer?)IndexBuffer;

        /// <inheritdoc cref="GraphicsPrimitive.VertexBuffer" />
        public VulkanGraphicsBuffer VulkanVertexBuffer => (VulkanGraphicsBuffer)VertexBuffer;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeIfNotNull(GraphicsPipeline);
                DisposeIfNotNull(VertexBuffer);
                DisposeIfNotNull(IndexBuffer);
            }

            _state.EndDispose();
        }
    }
}
