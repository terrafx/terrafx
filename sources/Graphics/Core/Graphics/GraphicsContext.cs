// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing commands.</summary>
public abstract class GraphicsContext : GraphicsDeviceObject
{
    private readonly GraphicsContextKind _kind;

    /// <summary>Initializes a new instance of the <see cref="GraphicsContext" /> class.</summary>
    /// <param name="device">The device for which the context is being created.</param>
    /// <param name="kind">The context kind.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsContext(GraphicsDevice device, GraphicsContextKind kind)
        : base(device)
    {
        _kind = kind;
    }

    /// <summary>Gets the fence used by the context for synchronization.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public abstract GraphicsFence Fence { get; }

    /// <summary>Gets the context kind.</summary>
    public GraphicsContextKind Kind => _kind;

    /// <summary>Flushes the graphics context.</summary>
    public abstract void Flush();

    /// <summary>Resets the graphics context.</summary>
    public abstract void Reset();
}
