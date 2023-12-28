// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics texture view.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsTextureViewCreateOptions
{
    /// <summary>The number of mip levels in the texture view.</summary>
    public ushort MipLevelCount;

    /// <summary>The index of the first mip level of the texture view.</summary>
    public ushort MipLevelStart;
}
