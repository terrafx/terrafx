// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics fences.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsFenceInfo
{
    /// <summary><c>true</c> if the fence is in the signalled state; otherwise, <c>false</c>.</summary>
    public bool IsSignalled;
}
