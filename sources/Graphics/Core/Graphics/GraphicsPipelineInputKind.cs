// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>Defines the kind of a graphics pipeline input.</summary>
public enum GraphicsPipelineInputKind
{
    /// <summary>Defines an unknown graphics pipeline input kind.</summary>
    Unknown,

    /// <summary>Defines a graphics pipeline input for a position.</summary>
    Position,

    /// <summary>Defines a graphics pipeline input for a color.</summary>
    Color,

    /// <summary>Defines a graphics pipeline input for a normal.</summary>
    Normal,

    /// <summary>Defines a graphics pipeline input for a texture coordinate.</summary>
    TextureCoordinate,
}
