// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing compute commands.</summary>
public abstract class GraphicsComputeContext : GraphicsContext
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsComputeContext" /> class.</summary>
    /// <param name="device">The device for which the compute context is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsComputeContext(GraphicsDevice device)
        : base(device, GraphicsContextKind.Compute)
    {
    }
}
