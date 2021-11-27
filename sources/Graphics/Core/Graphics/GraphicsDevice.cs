// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics device which provides state management and isolation for a graphics adapter.</summary>
public abstract class GraphicsDevice : IDisposable
{
    private readonly GraphicsAdapter _adapter;

    /// <summary>Initializes a new instance of the <see cref="GraphicsDevice" /> class.</summary>
    /// <param name="adapter">The underlying adapter for the device.</param>
    /// <exception cref="ArgumentNullException"><paramref name="adapter" /> is <c>null</c>.</exception>
    protected GraphicsDevice(GraphicsAdapter adapter)
    {
        ThrowIfNull(adapter);
        _adapter = adapter;
    }

    /// <summary>Gets the underlying adapter for the device.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the contexts for the device.</summary>
    public abstract ReadOnlySpan<GraphicsContext> Contexts { get; }

    /// <summary>Gets the memory allocator for the device.</summary>
    public abstract GraphicsMemoryAllocator MemoryAllocator { get; }

    /// <summary>Creates a new graphics fence for the device.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsFence CreateFence();

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
    public abstract GraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null);

    /// <summary>Creates a new graphics pipeline signature for the device.</summary>
    /// <param name="inputs">The inputs given to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <param name="resources">The resources available to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <returns>A new graphics pipeline signature created for the device.</returns>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default);

    /// <summary>Creates a new graphics primitive for the device.</summary>
    /// <param name="pipeline">The pipeline used for rendering the graphics primitive.</param>
    /// <param name="vertexBufferRegion">The buffer region which holds the vertices for the graphics primitive.</param>
    /// <param name="vertexBufferStride">The stride of <paramref name="vertexBufferRegion" />.</param>
    /// <param name="indexBufferRegion">The buffer region which holds the indices for the graphics primitive or <c>default</c> if none exists.</param>
    /// <param name="indexBufferStride">The stride of <paramref name="indexBufferRegion" />.</param>
    /// <param name="inputResourceRegions">The resources which hold the input data for the graphics primitive or an empty span if none exist.</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipeline" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pipeline" /> was not created for this device.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexBufferRegion" /> was not created for this device.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="indexBufferRegion" /> was not created for this device.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsPrimitive CreatePrimitive(GraphicsPipeline pipeline, in GraphicsMemoryRegion<GraphicsResource> vertexBufferRegion, uint vertexBufferStride, in GraphicsMemoryRegion<GraphicsResource> indexBufferRegion = default, uint indexBufferStride = 0, ReadOnlySpan<GraphicsMemoryRegion<GraphicsResource>> inputResourceRegions = default);

    /// <summary>Creates a new graphics shader for the device.</summary>
    /// <param name="kind">The kind of graphics shader to create.</param>
    /// <param name="bytecode">The underlying bytecode for the graphics shader.</param>
    /// <param name="entryPointName">The name of the entry point for the graphics shader.</param>
    /// <returns>A new graphics shader created for the device.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName);

    /// <summary>Creates a new graphics swapchain for the device.</summary>
    /// <param name="surface">The surface on which the swapchain can render.</param>
    /// <returns>A new graphics swapchain created for the device.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="surface" /> is <c>null</c>.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsSwapchain CreateSwapchain(IGraphicsSurface surface);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Signals a graphics fence.</summary>
    /// <param name="fence">The fence to be signaled</param>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void Signal(GraphicsFence fence);

    /// <summary>Waits for the device to become idle.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract void WaitForIdle();

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);
}
