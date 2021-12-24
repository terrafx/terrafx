// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics compute commands.</summary>
public abstract class GraphicsComputeCommandQueue : GraphicsCommandQueue<GraphicsComputeContext>
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsComputeCommandQueue" /> class.</summary>
    /// <param name="device">The device for which the compute command queue was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsComputeCommandQueue(GraphicsDevice device) : base(device)
    {
        CommandQueueInfo.Kind = GraphicsContextKind.Compute;
    }
}
