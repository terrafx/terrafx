// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Advanced;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics memory allocators.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsMemoryAllocatorInfo
{
    /// <summary>The length, in bytes, of the memory being managed.</summary>
    public nuint ByteLength;

    /// <summary><c>true</c> if the allocator is dedicated to a single allocation; otherwise, <c>false</c>.</summary>
    public bool IsDedicated;

    /// <summary><c>true</c> if there are no allocated memory regions; otherwise, <c>false</c>.</summary>
    public bool IsEmpty;

    /// <summary>The length, in bytes, of the largest free memory region.</summary>
    public nuint LargestFreeMemoryRegionByteLength;

    /// <summary>A pointer to the function that should be invoked when <see cref="GraphicsMemoryAllocator.Free(in GraphicsMemoryRegion)" /> completes.</summary>
    public GraphicsMemoryAllocatorOnFreeCallback OnFree;

    /// <summary>Gets the total length, in bytes, of free memory regions.</summary>
    public nuint TotalFreeMemoryRegionByteLength;
}
