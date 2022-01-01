// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics render passes.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsRenderPassInfo
{
    /// <summary>The fence used to synchronize the swapchain.</summary>
    public GraphicsFormat RenderTargetFormat;

    /// <summary>The surface on which the swapchain can render.</summary>
    public IGraphicsSurface Surface;

    /// <summary>The swapchain for the render pass.</summary>
    public GraphicsSwapchain Swapchain;
}
