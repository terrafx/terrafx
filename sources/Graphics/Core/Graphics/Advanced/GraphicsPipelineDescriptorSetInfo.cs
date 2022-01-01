// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics pipeline descriptor sets.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsPipelineDescriptorSetInfo
{
    /// <summary>The resource views for the pipeline descriptor set.</summary>
    public GraphicsResourceView[] ResourceViews;
}
