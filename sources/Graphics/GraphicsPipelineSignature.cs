// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics pipeline signature which details the inputs given and resources available to a graphics pipeline.</summary>
    public abstract class GraphicsPipelineSignature : IDisposable
    {
        private readonly GraphicsDevice _device;
        private readonly GraphicsPipelineInput[] _inputs;
        private readonly GraphicsPipelineResource[] _resources;

        /// <summary>Creates a new instance of the <see cref="GraphicsPipelineSignature" /> class.</summary>
        /// <param name="device">The device for which the pipeline signature was created.</param>
        /// <param name="inputs">The inputs given to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
        /// <param name="resources">The resources available to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        protected GraphicsPipelineSignature(GraphicsDevice device, ReadOnlySpan<GraphicsPipelineInput> inputs, ReadOnlySpan<GraphicsPipelineResource> resources)
        {
            ThrowIfNull(device, nameof(device));

            _device = device;
            _inputs = inputs.ToArray();
            _resources = resources.ToArray();
        }

        /// <summary>Gets the device for which the pipeline was created.</summary>
        public GraphicsDevice Device => _device;

        /// <summary>Gets the inputs given to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
        public ReadOnlySpan<GraphicsPipelineInput> Inputs => _inputs;

        /// <summary>Gets the resources available to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</summary>
        public ReadOnlySpan<GraphicsPipelineResource> Resources => _resources;

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
