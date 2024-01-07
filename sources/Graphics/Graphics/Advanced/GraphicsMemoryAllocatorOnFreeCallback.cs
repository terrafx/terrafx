// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines a function which is raised when a graphics memory region is freed.</summary>
/// <remarks>Initializes a new instance of the <see cref="GraphicsMemoryAllocatorOnFreeCallback" /> struct.</remarks>
/// <param name="value">A pointer to the function that will be called in <see cref="Invoke" />.</param>
[StructLayout(LayoutKind.Auto)]
public readonly unsafe struct GraphicsMemoryAllocatorOnFreeCallback(delegate*<in GraphicsMemoryRegion, void> value)
{
    private readonly delegate*<in GraphicsMemoryRegion, void> _value = value;

    /// <summary>Gets <c>true</c> if the backing function pointer is not null; otherwise, <c>false</c>.</summary>
    public bool IsNotNull => _value is not null;

    /// <summary>Gets <c>true</c> if the backing function pointer is null; otherwise, <c>false</c>.</summary>
    public bool IsNull => _value is null;

    /// <summary>Handles a graphics memory region being freed.</summary>
    /// <param name="memoryRegion">The memory region that was freed.</param>
    public void Invoke(in GraphicsMemoryRegion memoryRegion)
        => _value(in memoryRegion);
}
