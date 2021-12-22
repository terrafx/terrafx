// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics pipeline signature which details the inputs given and resources available to a graphics pipeline.</summary>
public abstract class GraphicsPipelineSignature : GraphicsDeviceObject
{
    private readonly UnmanagedArray<GraphicsPipelineInput> _inputs;
    private readonly UnmanagedArray<GraphicsPipelineResourceInfo> _resources;

    /// <summary>Creates a new instance of the <see cref="GraphicsPipelineSignature" /> class.</summary>
    /// <param name="device">The device for which the pipeline signature is being created.</param>
    /// <param name="inputs">The inputs given to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <param name="resources">The info about resources available to the graphics pipeline or <see cref="ReadOnlySpan{T}.Empty" /> if none exist.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsPipelineSignature(GraphicsDevice device, ReadOnlySpan<GraphicsPipelineInput> inputs, ReadOnlySpan<GraphicsPipelineResourceInfo> resources)
        : base(device)
    {
        _inputs = inputs.ToUnmanagedArray();
        _resources = resources.ToUnmanagedArray();
    }

    /// <summary>Gets the inputs given to the graphics pipeline or <see cref="UnmanagedReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public UnmanagedReadOnlySpan<GraphicsPipelineInput> Inputs => _inputs;

    /// <summary>Gets the info about resources available to the graphics pipeline or <see cref="UnmanagedReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public UnmanagedReadOnlySpan<GraphicsPipelineResourceInfo> Resources => _resources;
}
