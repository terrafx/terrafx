// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing compute commands.</summary>
public abstract class GraphicsComputeContext : GraphicsContext<GraphicsComputeContext>
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsComputeContext" /> class.</summary>
    /// <param name="computeCommandQueue">The compute command queue for which the compute context is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="computeCommandQueue" /> is <c>null</c>.</exception>
    protected GraphicsComputeContext(GraphicsComputeCommandQueue computeCommandQueue) : base(computeCommandQueue)
    {
        ContextInfo.Kind = GraphicsContextKind.Compute;
    }

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new GraphicsComputeCommandQueue CommandQueue => base.CommandQueue.As<GraphicsComputeCommandQueue>();
}
