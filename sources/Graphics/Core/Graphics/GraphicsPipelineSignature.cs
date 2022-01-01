// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>A graphics pipeline signature which details the inputs given and resources available to a graphics pipeline.</summary>
public abstract class GraphicsPipelineSignature : GraphicsDeviceObject
{
    /// <summary>The information for the graphics pipeline signature.</summary>
    protected GraphicsPipelineSignatureInfo PipelineSignatureInfo;

    /// <summary>Creates a new instance of the <see cref="GraphicsPipelineSignature" /> class.</summary>
    /// <param name="device">The device for which the pipeline signature is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsPipelineSignature(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets the inputs given to the graphics pipeline or <see cref="UnmanagedReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public UnmanagedReadOnlySpan<GraphicsPipelineInput> Inputs => PipelineSignatureInfo.Inputs;

    /// <summary>Gets the resources given to the graphics pipeline or <see cref="UnmanagedReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public UnmanagedReadOnlySpan<GraphicsPipelineResource> Resources => PipelineSignatureInfo.Resources;
}
