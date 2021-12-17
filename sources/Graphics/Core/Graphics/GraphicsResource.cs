// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace TerraFX.Graphics;

/// <summary>A graphics resource bound to a graphics device.</summary>
public abstract unsafe class GraphicsResource : GraphicsDeviceObject, IReadOnlyCollection<GraphicsResourceView>
{
    private readonly GraphicsResourceCpuAccess _cpuAccess;
    private readonly GraphicsMemoryRegion _memoryRegion;

    /// <summary>Initializes a new instance of the <see cref="GraphicsResource" /> class.</summary>
    /// <param name="device">The device for which the resource was created.</param>
    /// <param name="memoryRegion">The memory region in which the resource resides.</param>
    /// <param name="cpuAccess">The CPU access capabilitites of the resource.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
    protected GraphicsResource(GraphicsDevice device, in GraphicsMemoryRegion memoryRegion, GraphicsResourceCpuAccess cpuAccess)
        : base(device)
    {
        _cpuAccess = cpuAccess;
        _memoryRegion = memoryRegion;
    }

    /// <summary>Gets the number of resource views in the resource.</summary>
    public abstract int Count { get; }

    /// <summary>Gets the CPU access capabilitites of the resource.</summary>
    public GraphicsResourceCpuAccess CpuAccess => _cpuAccess;

    /// <summary>Gets <c>true</c> if the resource is mapped; otherwise, <c>false</c>.</summary>
    public abstract bool IsMapped { get; }

    /// <summary>Gets the mapped address of the resouce or <c>null</c> if the resource is not currently mapped.</summary>
    public abstract void* MappedAddress { get; }

    /// <summary>Gets the memory region in which the resource exists.</summary>
    public ref readonly GraphicsMemoryRegion MemoryRegion => ref _memoryRegion;

    /// <summary>Gets the size, in bytes, of the resource.</summary>
    public nuint Size => _memoryRegion.Size;

    /// <summary>Disposes all resource views in the resource.</summary>
    public abstract void DisposeAllViews();

    /// <summary>Gets an enumerator that can be used to iterate through the resource views of the buffer.</summary>
    /// <returns>An enumerator that can be used to iterate through the resource views of the buffer.</returns>
    public abstract IEnumerator<GraphicsResourceView> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
