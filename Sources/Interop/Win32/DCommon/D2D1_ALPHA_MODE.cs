// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Qualifies how alpha is to be treated in a bitmap or render target containing alpha.</summary>
    public enum D2D1_ALPHA_MODE
    {
        /// <summary>Alpha mode should be determined implicitly. Some target surfaces do not supply or imply this information in which case alpha must be specified.</summary>
        UNKNOWN = 0,

        /// <summary>Treat the alpha as premultipled.</summary>
        PREMULTIPLIED = 1,

        /// <summary>Opacity is in the 'A' component only.</summary>
        STRAIGHT = 2,

        /// <summary>Ignore any alpha channel information.</summary>
        IGNORE = 3
    }
}
