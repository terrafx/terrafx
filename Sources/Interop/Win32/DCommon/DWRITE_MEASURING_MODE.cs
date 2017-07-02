// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>The measuring method used for text layout.</summary>
    public enum DWRITE_MEASURING_MODE
    {
        /// <summary>Text is measured using glyph ideal metrics whose values are independent to the current display resolution.</summary>
        NATURAL = 0,

        /// <summary>Text is measured using glyph display compatible metrics whose values tuned for the current display resolution.</summary>
        GDI_CLASSIC = 1,

        /// <summary>Text is measured using the same glyph display metrics as text measured by GDI using a font created with <c>CLEARTYPE_NATURAL_QUALITY</c>.</summary>
        GDI_NATURAL = 2
    }
}
