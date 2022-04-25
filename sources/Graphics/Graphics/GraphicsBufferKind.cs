// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>Defines a graphics buffer kind.</summary>
public enum GraphicsBufferKind
{
    /// <summary>Defines the default buffer kind.</summary>
    Default,

    /// <summary>Defines a vertex buffer.</summary>
    Vertex,

    /// <summary>Defines an index buffer.</summary>
    Index,

    /// <summary>Defines a constant buffer.</summary>
    Constant,
}
