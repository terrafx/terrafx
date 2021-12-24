// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics contexts.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsContextInfo
{
    /// <summary>The context kind.</summary>
    public GraphicsContextKind Kind;

    /// <summary>The fence used by the context for synchronization.</summary>
    public GraphicsFence Fence;
}
