// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>Defines information common to all graphics pipeline signatures.</summary>
[StructLayout(LayoutKind.Auto)]
public struct GraphicsPipelineSignatureInfo
{
    /// <summary>The inputs given to the graphics pipeline or <see cref="UnmanagedReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public UnmanagedArray<GraphicsPipelineInput> Inputs;

    /// <summary>The resources given to the graphics pipeline or <see cref="UnmanagedReadOnlySpan{T}.Empty" /> if none exist.</summary>
    public UnmanagedArray<GraphicsPipelineResource> Resources;
}
