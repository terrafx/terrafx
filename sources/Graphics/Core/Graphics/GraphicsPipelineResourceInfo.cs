// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>Contains information about a graphics pipeline resource which is available to one or more stages of a graphics pipeline.</summary>
public readonly struct GraphicsPipelineResourceInfo
{
    private readonly GraphicsPipelineResourceKind _kind;
    private readonly GraphicsShaderVisibility _shaderVisibility;

    /// <summary>Initializes a new instance of the <see cref="GraphicsPipelineInput" /> struct.</summary>
    /// <param name="kind">The kind of the pipeline input.</param>
    /// <param name="shaderVisibility">The graphics shader kind(s) for which the pipeline resource is visible.</param>
    public GraphicsPipelineResourceInfo(GraphicsPipelineResourceKind kind, GraphicsShaderVisibility shaderVisibility)
    {
        _kind = kind;
        _shaderVisibility = shaderVisibility;
    }

    /// <summary>Gets the kind of the pipeline resource.</summary>
    public GraphicsPipelineResourceKind Kind => _kind;

    /// <summary>Gets the graphics shader  kind(s) for which the pipeline resource is visible.</summary>
    public GraphicsShaderVisibility ShaderVisibility => _shaderVisibility;
}
