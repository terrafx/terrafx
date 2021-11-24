// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer which can hold data for a graphics device.</summary>
public abstract unsafe class GraphicsBuffer : GraphicsResource
{
    private readonly GraphicsBufferKind _kind;

    /// <summary>Initializes a new instance of the <see cref="GraphicsBuffer" /> class.</summary>
    /// <param name="device">The device for which the buffer was created.</param>
    /// <param name="kind">The buffer kind.</param>
    /// <param name="blockRegion">The memory block region in which the resource exists.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    /// <exception cref="ArgumentNullException"><paramref name="blockRegion" />.<see cref="GraphicsMemoryRegion{TCollection}.Collection"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="blockRegion" /> was not created for <paramref name="device" />.</exception>
    protected GraphicsBuffer(GraphicsDevice device, GraphicsBufferKind kind, in GraphicsMemoryRegion<GraphicsMemoryBlock> blockRegion, GraphicsResourceCpuAccess cpuAccess)
        : base(device, in blockRegion, cpuAccess)
    {
        _kind = kind;
    }

    /// <summary>Gets the buffer kind.</summary>
    public GraphicsBufferKind Kind => _kind;
}
