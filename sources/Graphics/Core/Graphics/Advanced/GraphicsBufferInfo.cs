// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics buffers.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsBufferInfo
{
    /// <summary>The buffer kind.</summary>
    public GraphicsBufferKind Kind;
}
