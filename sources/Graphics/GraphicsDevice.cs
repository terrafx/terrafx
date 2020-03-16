// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics device which provides state management and isolation for a graphics adapter.</summary>
    public abstract class GraphicsDevice : IDisposable
    {
        private readonly GraphicsAdapter _graphicsAdapter;
        private readonly IGraphicsSurface _graphicsSurface;

        /// <summary>Initializes a new instance of the <see cref="GraphicsDevice" /> class.</summary>
        /// <param name="graphicsAdapter">The underlying graphics adapter for the device.</param>
        /// <param name="graphicsSurface">The graphics surface on which the device can render.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsAdapter" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsSurface" /> is <c>null</c>.</exception>
        protected GraphicsDevice(GraphicsAdapter graphicsAdapter, IGraphicsSurface graphicsSurface)
        {
            ThrowIfNull(graphicsAdapter, nameof(graphicsAdapter));
            ThrowIfNull(graphicsSurface, nameof(graphicsSurface));

            _graphicsAdapter = graphicsAdapter;
            _graphicsSurface = graphicsSurface;
        }

        /// <summary>Gets the current graphics context.</summary>
        public GraphicsContext CurrentGraphicsContext => GraphicsContexts[GraphicsContextIndex];

        /// <summary>Gets the <see cref="Graphics.GraphicsAdapter" /> for the instance.</summary>
        public GraphicsAdapter GraphicsAdapter => _graphicsAdapter;

        /// <summary>Gets the graphics contexts for the device.</summary>
        public abstract ReadOnlySpan<GraphicsContext> GraphicsContexts { get; }

        /// <summary>Gets an index which can be used to lookup the current graphics context.</summary>
        public abstract int GraphicsContextIndex { get; }

        /// <summary>Gets the graphics surface on which the device can render.</summary>
        public IGraphicsSurface GraphicsSurface => _graphicsSurface;

        /// <summary>Creates a new graphics heap for the device.</summary>
        /// <param name="size">The size, in bytes, of the graphics heap.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the graphics heap.</param>
        /// <returns>A new graphics heap created for the device.</returns>
        public abstract GraphicsHeap CreateGraphicsHeap(ulong size, GraphicsHeapCpuAccess cpuAccess = GraphicsHeapCpuAccess.None);

        /// <summary>Creates a new graphics pipeline for the device.</summary>
        /// <param name="signature">The signature which details the inputs given and resources available to the graphics pipeline.</param>
        /// <param name="vertexShader">The vertex shader for the graphics pipeline or <c>null</c> if none exists.</param>
        /// <param name="pixelShader">The pixel shader for the graphics pipeline or <c>null</c> if none exists.</param>
        /// <returns>A new graphics pipeline created for the device.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="signature" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexShader" /> is not <see cref="GraphicsShaderKind.Vertex"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexShader" /> was not created for this device.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelShader" /> is not <see cref="GraphicsShaderKind.Pixel"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelShader" /> was not created for this device.</exception>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract GraphicsPipeline CreateGraphicsPipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null);

        /// <summary>Creates a new graphics pipeline signature for the device.</summary>
        /// <param name="inputs">The inputs given to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
        /// <param name="resources">The resources available to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
        /// <returns>A new graphics pipeline signature created for the device.</returns>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract GraphicsPipelineSignature CreateGraphicsPipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default);

        /// <summary>Creates a new graphics primitive for the device.</summary>
        /// <param name="graphicsPipeline">The graphics pipeline used for rendering the graphics primitive.</param>
        /// <param name="vertexBuffer">The graphics buffer which holds the vertices for the graphics primitive.</param>
        /// <param name="indexBuffer">The graphics buffer which holds the indices for the graphics primitive or <c>null</c> if none exists.</param>
        /// <param name="inputResources">The graphics resources which hold the input data for the graphics primitive or an empty span if none exist.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsPipeline" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vertexBuffer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="graphicsPipeline" /> was not created for this device.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBuffer" /> was not created for this device.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBuffer" /> was not created for this device.</exception>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract GraphicsPrimitive CreateGraphicsPrimitive(GraphicsPipeline graphicsPipeline, GraphicsBuffer vertexBuffer, GraphicsBuffer? indexBuffer = null, ReadOnlySpan<GraphicsResource> inputResources = default);

        /// <summary>Creates a new graphics shader for the device.</summary>
        /// <param name="kind">The kind of graphics shader to create.</param>
        /// <param name="bytecode">The underlying bytecode for the graphics shader.</param>
        /// <param name="entryPointName">The name of the entry point for the graphics shader.</param>
        /// <returns>A new graphics shader created for the device.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract GraphicsShader CreateGraphicsShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Presents the last frame rendered.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract void PresentFrame();

        /// <summary>Signals a graphics fence.</summary>
        /// <param name="graphicsFence">The graphics fence to be signaled</param>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract void Signal(GraphicsFence graphicsFence);

        /// <summary>Waits for the device to become idle.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public abstract void WaitForIdle();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
