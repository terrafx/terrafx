// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1815 // Override equals and operator equals on value types

namespace TerraFX.Graphics;

/// <summary>Describes a graphics pipeline input.</summary>
public readonly struct GraphicsPipelineInput
{
    /// <summary>Gets the binding index of the pipeline input.</summary>
    public uint BindingIndex { get; init; }

    /// <summary>Gets the alignment, in bytes, of the pipeline input.</summary>
    public uint ByteAlignment { get; init; }

    /// <summary>Gets the length, in bytes, of the pipeline input.</summary>
    public uint ByteLength { get; init; }

    /// <summary>Gets the format of the pipeline input.</summary>
    public GraphicsFormat Format { get; init; }

    /// <summary>Gets the kind of the pipeline input.</summary>
    public GraphicsPipelineInputKind Kind { get; init; }

    /// <summary>Gets the shader kind(s) for which the pipeline input is visible.</summary>
    public GraphicsShaderVisibility ShaderVisibility { get; init; }
}
