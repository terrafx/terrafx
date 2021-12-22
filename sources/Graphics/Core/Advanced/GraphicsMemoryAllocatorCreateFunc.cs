// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Advanced;

/// <summary>Defines a function which creates a graphics memory allocator.</summary>
public readonly unsafe struct GraphicsMemoryAllocatorCreateFunc
{
    private readonly delegate*<GraphicsDeviceObject, GraphicsMemoryAllocatorOnFreeCallback, nuint, bool, GraphicsMemoryAllocator> _value;

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryAllocatorCreateFunc" /> struct.</summary>
    /// <param name="value">A pointer to the function that will be called in <see cref="Invoke" />.</param>
    public GraphicsMemoryAllocatorCreateFunc(delegate*<GraphicsDeviceObject, GraphicsMemoryAllocatorOnFreeCallback, nuint, bool, GraphicsMemoryAllocator> value)
    {
        _value = value;
    }

    /// <summary>Gets <c>true</c> if the backing function pointer is null; otherwise, <c>false</c>.</summary>
    public bool IsNull => _value is null;

    /// <summary>Gets <c>true</c> if the backing function pointer is not null; otherwise, <c>false</c>.</summary>
    public bool IsValid => _value is not null;

    /// <summary>Creates a new graphics memory allocator.</summary>
    /// <param name="deviceObject">The device object for which the allocator is managing memory.</param>
    /// <param name="onFree">A pointer to the function that should be invoked when <see cref="GraphicsMemoryAllocator.Free(in GraphicsMemoryRegion)" /> completes.</param>
    /// <param name="size">The size, in bytes, of the memory that is to be managed.</param>
    /// <param name="isDedicated"><c>true</c> if the allocator is dedicated to a single allocation; otherwise, <c>false</c>.</param>
    /// <returns>A new graphics memory allocator.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="deviceObject" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    public GraphicsMemoryAllocator Invoke(GraphicsDeviceObject deviceObject, GraphicsMemoryAllocatorOnFreeCallback onFree, nuint size, bool isDedicated)
        => _value(deviceObject, onFree, size, isDedicated);
}
