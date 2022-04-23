// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics device.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsDeviceCreateOptions
{
    /// <summary>The function which creates the backing memory allocators used by the device or <c>default</c> to use the system provided default.</summary>
    public GraphicsMemoryAllocatorCreateFunc CreateMemoryAllocator;
}
