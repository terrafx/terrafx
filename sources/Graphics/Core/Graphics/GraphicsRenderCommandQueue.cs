// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics render commands.</summary>
public abstract class GraphicsRenderCommandQueue : GraphicsCommandQueue<GraphicsRenderContext>
{
    /// <summary>Initializes a new instance of the <see cref="GraphicsRenderCommandQueue" /> class.</summary>
    /// <param name="device">The device for which the render command queue was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsRenderCommandQueue(GraphicsDevice device) : base(device)
    {
        CommandQueueInfo.Kind = GraphicsContextKind.Render;
    }
}
