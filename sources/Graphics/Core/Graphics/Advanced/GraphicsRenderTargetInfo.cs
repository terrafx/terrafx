// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics render targets.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsRenderTargetInfo
{
    /// <summary>The index of the render target.</summary>
    public int Index;
}
