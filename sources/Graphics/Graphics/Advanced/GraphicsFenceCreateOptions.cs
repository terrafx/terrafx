// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics fence.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsFenceCreateOptions
{
    /// <summary><c>true</c> if the fence is signaled by default; otherwise, <c>false</c>.</summary>
    public bool IsSignaled;
}
