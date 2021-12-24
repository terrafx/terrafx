// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Advanced;

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics memory allocator.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsMemoryAllocatorCreateOptions
{
    /// <summary>The length, in bytes, of the memory being managed.</summary>
    public nuint ByteLength;

    /// <summary><c>true</c> if the allocator is dedicated to a single allocation; otherwise, <c>false</c>.</summary>
    public bool IsDedicated;

    /// <summary>A pointer to the function that should be invoked when <see cref="GraphicsMemoryAllocator.Free(in GraphicsMemoryRegion)" /> completes.</summary>
    public GraphicsMemoryAllocatorOnFreeCallback OnFree;
}
