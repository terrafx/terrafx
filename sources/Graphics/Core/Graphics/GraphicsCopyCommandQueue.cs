// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics copy commands.</summary>
public abstract class GraphicsCopyCommandQueue : GraphicsCommandQueue<GraphicsCopyContext>
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsCopyCommandQueue" /> class.</summary>
    /// <param name="device">The device for which the copy command queue was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsCopyCommandQueue(GraphicsDevice device) : base(device)
    {
        CommandQueueInfo.Kind = GraphicsContextKind.Copy;
    }
}
