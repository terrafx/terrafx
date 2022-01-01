// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>Defines a graphics pipeline resource kind.</summary>
public enum GraphicsPipelineResourceKind
{
    /// <summary>An unknown resource kind.</summary>
    Unknown,

    /// <summary>A constant buffer pipeline resource.</summary>
    ConstantBuffer,

    /// <summary>A texture pipeline resource.</summary>
    Texture,
}
