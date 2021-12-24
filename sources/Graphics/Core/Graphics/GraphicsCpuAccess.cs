// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>Defines CPU access capabilities.</summary>
public enum GraphicsCpuAccess
{
    /// <summary>No CPU access.</summary>
    None = 0,

    /// <summary>CPU read access.</summary>
    Read = 1,

    /// <summary>CPU write access.</summary>
    Write = 2,
}
