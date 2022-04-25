// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>Defines a graphics shader kind.</summary>
public enum GraphicsShaderKind
{
    /// <summary>Defines an unknown graphics shader kind.</summary>
    Unknown,

    /// <summary>Defines a vertex shader which can transform vertices for a graphics device.</summary>
    Vertex,

    /// <summary>Defines a pixel shader which can transform pixels for a graphics device.</summary>
    Pixel,
}
