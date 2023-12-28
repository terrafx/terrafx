// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when allocating memory.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsMemoryAllocationOptions
{
    /// <summary>The alignment, in bytes, of the memory allocation.</summary>
    public nuint ByteAlignment;

    /// <summary>The length, in bytes, of the memory allocation.</summary>
    public nuint ByteLength;

    /// <summary>The flags that modify how the memory is allocated..</summary>
    public GraphicsMemoryAllocationFlags AllocationFlags;
}
