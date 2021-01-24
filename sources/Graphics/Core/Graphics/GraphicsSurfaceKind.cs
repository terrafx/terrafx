// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>Defines a graphics surface kind.</summary>
    public enum GraphicsSurfaceKind
    {
        /// <summary>Defines an unknown graphics surface kind that may require specialized handling.</summary>
        Unknown,

        /// <summary>Defines an Android based graphics surface.</summary>
        Android,

        /// <summary>Defines a Wayland based graphics surface.</summary>
        Wayland,

        /// <summary>Defines a Win32 based graphics surface.</summary>
        Win32,

        /// <summary>Defines an XCB based graphics surface.</summary>
        Xcb,

        /// <summary>Defines an Xlib based graphics surface.</summary>
        Xlib,
    }
}
