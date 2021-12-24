// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics swapchains.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsSwapchainInfo
{
    /// <summary>The index of the current render target.</summary>
    public int CurrentRenderTargetIndex;

    /// <summary>The fence used to synchronize the swapchain.</summary>
    public GraphicsFence Fence;

    /// <summary>The render targets for the swapchain.</summary>
    public GraphicsRenderTarget[] RenderTargets;

    /// <summary>The format for the render targets of the swapchain.</summary>
    public GraphicsFormat RenderTargetFormat;

    /// <summary>The surface on which the swapchain can render.</summary>
    public IGraphicsSurface Surface;
}
