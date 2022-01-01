// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics render pass.</summary>
public abstract class GraphicsRenderPass : GraphicsDeviceObject
{
    /// <summary>The information for the graphics render pass.</summary>
    protected GraphicsRenderPassInfo RenderPassInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderPass" /> class.</summary>
    /// <param name="device">The device for which the render pass is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsRenderPass(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets the format of render targets used by the render pass.</summary>
    public GraphicsFormat RenderTargetFormat => RenderPassInfo.RenderTargetFormat;

    /// <summary>Gets the surface used by the render pass.</summary>
    public IGraphicsSurface Surface => RenderPassInfo.Surface;

    /// <summary>Gets the swapchain for the render pass.</summary>
    public GraphicsSwapchain Swapchain => RenderPassInfo.Swapchain;

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

    /// <summary>Creates a new graphics pipeline for the device.</summary>
    /// <param name="createOptions">The options to use when creating the pipeline.</param>
    /// <returns>A new graphics pipeline created for the device.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsPipeline CreatePipelineUnsafe(in GraphicsPipelineCreateOptions createOptions);
}
