// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics pipeline which defines how a graphics primitive should be rendered.</summary>
public abstract class GraphicsPipeline : GraphicsRenderPassObject
{
    /// <summary>The information for the graphics pipeline.</summary>
    protected GraphicsPipelineInfo PipelineInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipeline" /> class.</summary>
    /// <param name="renderPass">The render pass for which the pipeline is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="renderPass" /> is <c>null</c>.</exception>
    protected GraphicsPipeline(GraphicsRenderPass renderPass) : base(renderPass)
    {
    }

    /// <summary>Gets <c>true</c> if the pipeline has a pixel shader; otherwise, <c>false</c>.</summary>
    public bool HasPixelShader => PipelineInfo.PixelShader is not null;

    /// <summary>Gets <c>true</c> if the pipeline has a vertex shader; otherwise, <c>false</c>.</summary>
    public bool HasVertexShader => PipelineInfo.VertexShader is not null;

    /// <summary>Gets the pixel shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? PixelShader => PipelineInfo.PixelShader;

    /// <summary>Gets the signature of the pipeline.</summary>
    public GraphicsPipelineSignature Signature => PipelineInfo.Signature;

    /// <summary>Gets the vertex shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? VertexShader => PipelineInfo.VertexShader;

    /// <summary>Creates a new descriptor set for the pipeline.</summary>
    /// <param name="resourceViews">The resource views for the pipeline descriptor set.</param>
    /// <returns>The created descriptor set.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>empty</c>.</exception>
    /// <exception cref="ObjectDisposedException">The pipeline has been disposed.</exception>
    public GraphicsPipelineDescriptorSet CreateDescriptorSet(ReadOnlySpan<GraphicsResourceView> resourceViews)
    {
        ThrowIfDisposed();
        ThrowIfZero(resourceViews.Length);

        var createOptions = new GraphicsPipelineDescriptorSetCreateOptions {
            ResourceViews = resourceViews.ToArray(),
            TakeResourceViewsOwnership = true,
        };
        return CreateDescriptorSetUnsafe(in createOptions);
    }

    /// <summary>Creates a new descriptor set for the pipeline.</summary>
    /// <param name="createOptions">The options to use when creating the descriptor set.</param>
    /// <returns>The created descriptor set.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>empty</c>.</exception>
    /// <exception cref="ObjectDisposedException">The pipeline has been disposed.</exception>
    public GraphicsPipelineDescriptorSet CreateDescriptorSet(in GraphicsPipelineDescriptorSetCreateOptions createOptions)
    {
        ThrowIfDisposed();
        ThrowIfNull(createOptions.ResourceViews);
        ThrowIfZero(createOptions.ResourceViews.Length);

        return CreateDescriptorSetUnsafe(in createOptions);
    }

    /// <summary>Creates a new descriptor set for the pipeline.</summary>
    /// <param name="createOptions">The options to use when creating the descriptor set.</param>
    /// <returns>The created descriptor set.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract GraphicsPipelineDescriptorSet CreateDescriptorSetUnsafe(in GraphicsPipelineDescriptorSetCreateOptions createOptions);
}
