// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing compute commands.</summary>
public sealed unsafe class GraphicsComputeContext : GraphicsContext
{
    internal GraphicsComputeContext(GraphicsComputeCommandQueue computeCommandQueue) : base(computeCommandQueue, GraphicsContextKind.Compute)
    {
    }

    /// <inheritdoc cref="GraphicsCommandQueueObject.CommandQueue" />
    public new GraphicsComputeCommandQueue CommandQueue => base.CommandQueue.As<GraphicsComputeCommandQueue>();
}
