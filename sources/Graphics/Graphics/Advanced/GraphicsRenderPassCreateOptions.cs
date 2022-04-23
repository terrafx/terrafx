// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics render pass.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsRenderPassCreateOptions
{
    /// <summary>The minimum number of render targets for the render pass.</summary>
    public uint MinimumRenderTargetCount;

    /// <summary>The format of render targets for the render pass.</summary>
    public GraphicsFormat RenderTargetFormat;

    /// <summary>The surface for the render pass.</summary>
    public IGraphicsSurface Surface;
}
