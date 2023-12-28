// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace TerraFX.Graphics;

/// <summary>Defines a graphics context kind.</summary>
public enum GraphicsContextKind
{
    /// <summary>Defines an unknown context kind.</summary>
    Unknown,

    /// <summary>Defines a render context.</summary>
    Render,

    /// <summary>Defines a copy context.</summary>
    Copy,

    /// <summary>Defines a compute context.</summary>
    Compute,

    /// <summary>INTERNAL USE ONLY: The number of enumeration values currently defined.</summary>
    COUNT__
}
