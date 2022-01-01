// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics devices.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsDeviceInfo
{
    /// <summary>The compute command queue for the device.</summary>
    public GraphicsComputeCommandQueue ComputeQueue;

    /// <summary>The copy command queue for the device.</summary>
    public GraphicsCopyCommandQueue CopyQueue;

    /// <summary>The render command queue for the device.</summary>
    public GraphicsRenderCommandQueue RenderQueue;
}
