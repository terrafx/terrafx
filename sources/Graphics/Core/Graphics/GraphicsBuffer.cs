// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer which can hold data for a graphics device.</summary>
public abstract unsafe class GraphicsBuffer : GraphicsResource
{
    private readonly GraphicsBufferKind _kind;

    /// <summary>Initializes a new instance of the <see cref="GraphicsBuffer" /> class.</summary>
    /// <param name="device">The device for which the buffer was created.</param>
    /// <param name="cpuAccess">The CPU access capabilities for the buffer.</param>
    /// <param name="size">The size, in bytes, of the buffer.</param>
    /// <param name="alignment">The alignment, in bytes, of the buffer.</param>
    /// <param name="memoryRegion">The memory region in which the buffer exists.</param>
    /// <param name="kind">The buffer kind.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsBuffer(GraphicsDevice device, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment, in GraphicsMemoryRegion memoryRegion, GraphicsBufferKind kind)
        : base(device, cpuAccess, size, alignment, in memoryRegion)
    {
        _kind = kind;
    }

    /// <summary>Gets the buffer kind.</summary>
    public GraphicsBufferKind Kind => _kind;
}
