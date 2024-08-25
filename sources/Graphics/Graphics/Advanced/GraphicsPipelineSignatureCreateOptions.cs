// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace TerraFX.Graphics.Advanced;

/// <summary>The options used when creating a graphics pipeline signature.</summary>
[StructLayout(LayoutKind.Auto)]
public unsafe struct GraphicsPipelineSignatureCreateOptions
{
    /// <summary>The pipeline inputs for the pipeline signature.</summary>
    public UnmanagedArray<GraphicsPipelineInput> Inputs;

    /// <summary>The pipeline resources for the pipeline signature.</summary>
    public UnmanagedArray<GraphicsPipelineResource> Resources;

    /// <summary><c>true</c> if the graphics pipeline signature should take ownership of <see cref="Inputs" />; otherwise, <c>false</c>.</summary>
    public bool TakeInputsOwnership;

    /// <summary><c>true</c> if the graphics pipeline signature should take ownership of <see cref="Resources" />; otherwise, <c>false</c>.</summary>
    public bool TakeResourcesOwnership;
}
