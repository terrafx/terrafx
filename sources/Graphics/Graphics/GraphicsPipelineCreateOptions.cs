// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics;

/// <summary>The options used when creating a graphics pipeline.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsPipelineCreateOptions
{
    /// <summary>The pixel shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? PixelShader;

    /// <summary>The pipeline signature which details the inputs given and resources available to the pipeline.</summary>
    public GraphicsPipelineSignature Signature;

    /// <summary>The vertex shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? VertexShader;
}
