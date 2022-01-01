// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>Defines the graphics shader(s) for which a resource is visible.</summary>
[Flags]
public enum GraphicsShaderVisibility
{
    /// <summary>Defines visibility to no shaders.</summary>
    None = 0,

    /// <summary>Defines vertex shader visibility.</summary>
    Vertex = 1,

    /// <summary>Defines pixel shader visibility.</summary>
    Pixel = 2,

    /// <summary>Defines visibility to all shaders.</summary>
    All = Vertex | Pixel,
}
