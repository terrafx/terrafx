// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics render pass.</summary>
public sealed unsafe class GraphicsRenderPass : GraphicsDeviceObject
{
    private readonly GraphicsFormat _renderTargetFormat;

    private readonly IGraphicsSurface _surface;

    private GraphicsSwapchain _swapchain;

    internal GraphicsRenderPass(GraphicsDevice device, in GraphicsRenderPassCreateOptions createOptions) : base(device)
    {
        device.AddRenderPass(this);

        _renderTargetFormat = createOptions.RenderTargetFormat;
        _surface = createOptions.Surface;

        var swapchainCreateOptions = new GraphicsSwapchainCreateOptions {
            MinimumRenderTargetCount = createOptions.MinimumRenderTargetCount,
            RenderTargetFormat = createOptions.RenderTargetFormat,
            Surface = createOptions.Surface,
        };
        _swapchain = new GraphicsSwapchain(this, in swapchainCreateOptions);
    }

    /// <summary>Gets the format of render targets used by the render pass.</summary>
    public GraphicsFormat RenderTargetFormat => _renderTargetFormat;

    /// <summary>Gets the surface used by the render pass.</summary>
    public IGraphicsSurface Surface => _surface;

    /// <summary>Gets the swapchain for the render pass.</summary>
    public GraphicsSwapchain Swapchain => _swapchain;

    /// <summary>Creates a new graphics pipeline for the device.</summary>
    /// <param name="createOptions">The options to use when creating the pipeline.</param>
    /// <returns>A new graphics pipeline created for the device.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsPipelineCreateOptions.Signature" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.Signature" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.VertexShader" /> is not <see cref="GraphicsShaderKind.Vertex"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.VertexShader" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.PixelShader" /> is not <see cref="GraphicsShaderKind.Pixel"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.PixelShader" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsPipeline CreatePipeline(in GraphicsPipelineCreateOptions createOptions)
    {
        ThrowIfNull(createOptions.Signature);

        if (createOptions.Signature.Device != Device)
        {
            ThrowForInvalidParent(createOptions.Signature.Device);
        }

        if (createOptions.VertexShader is GraphicsShader vertexShader)
        {
            if (vertexShader.Device != Device)
            {
                ThrowForInvalidParent(vertexShader.Device);
            }

            if (vertexShader.Kind != GraphicsShaderKind.Vertex)
            {
                ThrowForInvalidKind(vertexShader.Kind);
            }
        }

        if (createOptions.PixelShader is GraphicsShader pixelShader)
        {
            if (pixelShader.Device != Device)
            {
                ThrowForInvalidParent(pixelShader.Device);
            }

            if (pixelShader.Kind != GraphicsShaderKind.Pixel)
            {
                ThrowForInvalidKind(pixelShader.Kind);
            }
        }

        return CreatePipelineUnsafe(in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _swapchain.Dispose();
            _swapchain = null!;
        }

        _ = Device.RemoveRenderPass(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    private GraphicsPipeline CreatePipelineUnsafe(in GraphicsPipelineCreateOptions createOptions) => new GraphicsPipeline(this, in createOptions);
}
