// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics render pass.</summary>
public abstract class GraphicsRenderPass : GraphicsDeviceObject
{
    private readonly IGraphicsSurface _surface;
    private readonly GraphicsFormat _renderTargetFormat;

    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderPass" /> class.</summary>
    /// <param name="device">The device for which the render pass is being created.</param>
    /// <param name="surface">The surface used by the render pass.</param>
    /// <param name="renderTargetFormat">The format of render targets used by the render pass.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsRenderPass(GraphicsDevice device, IGraphicsSurface surface, GraphicsFormat renderTargetFormat)
        : base(device)
    {
        ThrowIfNull(surface);

        _surface = surface;
        _renderTargetFormat = renderTargetFormat;
    }

    /// <summary>Gets the format of render targets used by the render pass.</summary>
    public GraphicsFormat RenderTargetFormat => _renderTargetFormat;

    /// <summary>Gets the surface used by the render pass.</summary>
    public IGraphicsSurface Surface => _surface;

    /// <summary>Gets the swapchain for the render pass.</summary>
    public abstract GraphicsSwapchain Swapchain { get; }

    /// <summary>Creates a new graphics pipeline for the device.</summary>
    /// <param name="signature">The signature which details the inputs given and resources available to the graphics pipeline.</param>
    /// <param name="vertexShader">The vertex shader for the graphics pipeline or <c>null</c> if none exists.</param>
    /// <param name="pixelShader">The pixel shader for the graphics pipeline or <c>null</c> if none exists.</param>
    /// <returns>A new graphics pipeline created for the device.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="signature" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="signature" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexShader" /> is not <see cref="GraphicsShaderKind.Vertex"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexShader" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelShader" /> is not <see cref="GraphicsShaderKind.Pixel"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelShader" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public abstract GraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null);
}
