// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct DWRITE_FONT_METRICS1
    {
        #region Fields
        public DWRITE_FONT_METRICS BaseValue;

        /// <summary>Left edge of accumulated bounding blackbox of all glyphs in the font.</summary>>
        public INT16 glyphBoxLeft;

        /// <summary>Top edge of accumulated bounding blackbox of all glyphs in the font.</summary>>
        public INT16 glyphBoxTop;

        /// <summary>Right edge of accumulated bounding blackbox of all glyphs in the font.</summary>>
        public INT16 glyphBoxRight;

        /// <summary>Bottom edge of accumulated bounding blackbox of all glyphs in the font.</summary>>
        public INT16 glyphBoxBottom;

        /// <summary>Horizontal position of the subscript relative to the baseline origin. This is typically negative (to the left) in italic/oblique fonts, and zero in regular fonts.</summary>>
        public INT16 subscriptPositionX;

        /// <summary>Vertical position of the subscript relative to the baseline. This is typically negative.</summary>>
        public INT16 subscriptPositionY;

        /// <summary>Horizontal size of the subscript em box in design units, used to scale the simulated subscript relative to the full em box size. This the numerator of the scaling ratio where denominator is the design units per em. If this member is zero, the font does not specify a scale factor, and the client should use its own policy.</summary>>
        public INT16 subscriptSizeX;

        /// <summary>Vertical size of the subscript em box in design units, used to scale the simulated subscript relative to the full em box size. This the numerator of the scaling ratio where denominator is the design units per em. If this member is zero, the font does not specify a scale factor, and the client should use its own policy.</summary>>
        public INT16 subscriptSizeY;

        /// <summary>Horizontal position of the superscript relative to the baseline origin. This is typically positive (to the right) in italic/oblique fonts, and zero in regular fonts.</summary>>
        public INT16 superscriptPositionX;

        /// <summary>Vertical position of the superscript relative to the baseline. This is typically positive.</summary>>
        public INT16 superscriptPositionY;

        /// <summary>Horizontal size of the superscript em box in design units, used to scale the simulated superscript relative to the full em box size. This the numerator of the scaling ratio where denominator is the design units per em. If this member is zero, the font does not specify a scale factor, and the client should use its own policy.</summary>>
        public INT16 superscriptSizeX;

        /// <summary>Vertical size of the superscript em box in design units, used to scale the simulated superscript relative to the full em box size. This the numerator of the scaling ratio where denominator is the design units per em. If this member is zero, the font does not specify a scale factor, and the client should use its own policy.</summary>>
        public INT16 superscriptSizeY;

        /// <summary>Indicates that the ascent, descent, and lineGap are based on newer 'typographic' values in the font, rather than legacy values.</summary>>
        public BOOL hasTypographicMetrics;
    #endregion
    }
}
