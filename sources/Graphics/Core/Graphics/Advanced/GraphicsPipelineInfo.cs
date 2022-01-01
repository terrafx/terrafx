// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics pipeline signatures.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsPipelineInfo
{
    /// <summary>The pixel shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? PixelShader;

    /// <summary>The pipeline signature which details the inputs given and resources available to the pipeline.</summary>
    public GraphicsPipelineSignature Signature;

    /// <summary>The vertex shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? VertexShader;
}
