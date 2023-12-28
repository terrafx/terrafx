// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics;

/// <summary>Describes a graphics pipeline resource.</summary>
public readonly struct GraphicsPipelineResource
{
    /// <summary>Gets the binding index of the pipeline resource.</summary>
    public uint BindingIndex { get; init; }

    /// <summary>Gets the kind of the pipeline resource.</summary>
    public GraphicsPipelineResourceKind Kind { get; init; }

    /// <summary>Gets the shader kind(s) for which the pipeline resource is visible.</summary>
    public GraphicsShaderVisibility ShaderVisibility { get; init; }
}
