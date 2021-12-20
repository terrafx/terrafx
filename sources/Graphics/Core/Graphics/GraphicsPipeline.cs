// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics pipeline which defines how a graphics primitive should be rendered.</summary>
public abstract class GraphicsPipeline : GraphicsRenderPassObject
{
    private readonly GraphicsPipelineSignature _signature;
    private readonly GraphicsShader? _vertexShader;
    private readonly GraphicsShader? _pixelShader;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipeline" /> class.</summary>
    /// <param name="renderPass">The render pass for which the pipeline is being created.</param>
    /// <param name="signature">The signature which details the inputs given and resources available to the pipeline.</param>
    /// <param name="vertexShader">The vertex shader for the pipeline or <c>null</c> if none exists.</param>
    /// <param name="pixelShader">The pixel shader for the pipeline or <c>null</c> if none exists.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="signature" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="signature" /> was not created for the same device as <paramref name="renderPass" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The kind of <paramref name="vertexShader" /> is not <see cref="GraphicsShaderKind.Vertex" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="vertexShader" /> was not created for the same device as <paramref name="renderPass" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The kind of <paramref name="pixelShader" /> is not <see cref="GraphicsShaderKind.Pixel" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pixelShader" /> was not created for the same device as <paramref name="renderPass" />.</exception>
    protected GraphicsPipeline(GraphicsRenderPass renderPass, GraphicsPipelineSignature signature, GraphicsShader? vertexShader, GraphicsShader? pixelShader)
        : base(renderPass)
    {
        ThrowIfNull(signature);

        if (signature.Device != renderPass.Device)
        {
            ThrowForInvalidParent(signature.Device);
        }

        if (vertexShader is not null)
        {
            if (vertexShader.Kind != GraphicsShaderKind.Vertex)
            {
                ThrowForInvalidKind(vertexShader.Kind, GraphicsShaderKind.Vertex);
            }

            if (vertexShader.Device != renderPass.Device)
            {
                ThrowForInvalidParent(vertexShader.Device);
            }
        }

        if (pixelShader is not null)
        {
            if (pixelShader.Kind != GraphicsShaderKind.Pixel)
            {
                ThrowForInvalidKind(pixelShader.Kind, GraphicsShaderKind.Pixel);
            }

            if (pixelShader.Device != renderPass.Device)
            {
                ThrowForInvalidParent(pixelShader.Device);
            }
        }

        _signature = signature;
        _vertexShader = vertexShader;
        _pixelShader = pixelShader;
    }

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

    /// <summary>Creates a new resource view set for the pipeline.</summary>
    /// <param name="resourceViews">The resource views in the resource view set.</param>
    /// <returns>The created resource view set.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="resourceViews" /> is <c>empty</c>.</exception>
    /// <exception cref="ObjectDisposedException">The pipeline has been disposed.</exception>
    public abstract GraphicsPipelineResourceViewSet CreateResourceViews(ReadOnlySpan<GraphicsResourceView> resourceViews);
}
