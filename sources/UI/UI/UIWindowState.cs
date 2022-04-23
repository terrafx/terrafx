// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.UI;

/// <summary>Defines whether a window is restored, minimized, or maximized.</summary>
public enum UIWindowState
{
    /// <summary>The window is hidden.</summary>
    Hidden = 0,

    /// <summary>The window is minimized.</summary>
    Minimized = 1,

    /// <summary>The window is not hidden and is neither minimized nor maximized.</summary>
    Normal = 2,

    /// <summary>The window is maximized.</summary>
    Maximized = 3
}
