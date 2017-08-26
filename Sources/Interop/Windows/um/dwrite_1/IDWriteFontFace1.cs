// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    /// <summary>The interface that represents an absolute reference to a font face. It contains font face type, appropriate file references and face identification data. Various font data such as metrics, names and glyph outlines is obtained from IDWriteFontFace.</summary>
    [Guid("A71EFDB4-9FDB-4838-AD90-CFC3BE8C3DAF")]
    public /* blittable */ unsafe struct IDWriteFontFace1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets common metrics for the font in design units. These metrics are applicable to all the glyphs within a font, and are used by applications for layout calculations.</summary>
        /// <param name="fontMetrics">Metrics public structure to fill in.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetMetrics(
            [In] IDWriteFontFace1* This,
            [Out] DWRITE_FONT_METRICS1* fontMetrics
        );

        /// <summary>Gets common metrics for the font in design units. These metrics are applicable to all the glyphs within a font, and are used by applications for layout calculations.</summary>
        /// <param name="emSize">Logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch.</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP. For example, if the DPI of the rendering surface is 96 this value is 1.0f. If the DPI is 120, this value is 120.0f/96.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the font size and pixelsPerDip.</param>
        /// <param name="fontMetrics">Font metrics public structure to fill in.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGdiCompatibleMetrics(
            [In] IDWriteFontFace1* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform,
            [Out] DWRITE_FONT_METRICS1* fontMetrics
        );

        /// <summary>Gets caret metrics for the font in design units. These are used by text editors for drawing the correct caret placement/slant.</summary>
        /// <param name="caretMetrics">Metrics public structure to fill in.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetCaretMetrics(
            [In] IDWriteFontFace1* This,
            [Out] DWRITE_CARET_METRICS* caretMetrics
        );

        /// <summary>Returns the list of character ranges supported by the font, which is useful for scenarios like character picking, glyph display, and efficient font selection lookup. This is similar to GDI's GetFontUnicodeRanges, except that it returns the full Unicode range, not just 16-bit UCS-2.</summary>
        /// <param name="maxRangeCount">Maximum number of character ranges passed in from the client.</param>
        /// <param name="unicodeRanges">Array of character ranges.</param>
        /// <param name="actualRangeCount">Actual number of character ranges, regardless of the maximum count.</param>
        /// <remarks> These ranges are from the cmap, not the OS/2::ulCodePageRange1.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetUnicodeRanges(
            [In] IDWriteFontFace1* This,
            [In, ComAliasName("UINT32")] uint maxRangeCount,
            [Out, Optional] DWRITE_UNICODE_RANGE* unicodeRanges,
            [Out, ComAliasName("UINT32")] uint* actualRangeCount
        );

        /// <summary>Returns true if the font is monospaced, meaning its characters are the same fixed-pitch width (non-proportional).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsMonospacedFont(
            [In] IDWriteFontFace1* This
        );

        /// <summary>Returns the advances in design units for a sequences of glyphs.</summary>
        /// <param name="glyphCount">Number of glyphs to retrieve advances for.</param>
        /// <param name="glyphIndices">Array of glyph id's to retrieve advances for.</param>
        /// <param name="glyphAdvances">Returned advances in font design units for each glyph.</param>
        /// <param name="isSideways">Retrieve the glyph's vertical advance height rather than horizontal advance widths.</param>
        /// <remarks> This is equivalent to calling GetGlyphMetrics and using only the advance width/height.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDesignGlyphAdvances(
            [In] IDWriteFontFace1* This,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* glyphIndices,
            [Out, ComAliasName("INT32")] int* glyphAdvances,
            [In, ComAliasName("BOOL")] int isSideways = FALSE
        );

        /// <summary>Returns the pixel-aligned advances for a sequences of glyphs, the same as GetGdiCompatibleGlyphMetrics would return.</summary>
        /// <param name="emSize">Logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch.</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP. For example, if the DPI of the rendering surface is 96 this value is 1.0f. If the DPI is 120, this value is 120.0f/96.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the font size and pixelsPerDip.</param>
        /// <param name="useGdiNatural">When FALSE, the metrics are the same as GDI aliased text (DWRITE_MEASURING_MODE_GDI_CLASSIC). When TRUE, the metrics are the same as those measured by GDI using a font using CLEARTYPE_NATURAL_QUALITY (DWRITE_MEASURING_MODE_GDI_NATURAL).</param>
        /// <param name="isSideways">Retrieve the glyph's vertical advances rather than horizontal advances.</param>
        /// <param name="glyphCount">Total glyphs to retrieve adjustments for.</param>
        /// <param name="glyphIndices">Array of glyph id's to retrieve advances.</param>
        /// <param name="glyphAdvances">Returned advances in font design units for each glyph.</param>
        /// <remarks> This is equivalent to calling GetGdiCompatibleGlyphMetrics and using only the advance width/height. Like GetGdiCompatibleGlyphMetrics, these are in design units, meaning they must be scaled down by DWRITE_FONT_METRICS::designUnitsPerEm.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGdiCompatibleGlyphAdvances(
            [In] IDWriteFontFace1* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int useGdiNatural,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* glyphIndices,
            [Out, ComAliasName("INT32")] int* glyphAdvances
        );

        /// <summary>Retrieves the kerning pair adjustments from the font's kern table.</summary>
        /// <param name="glyphCount">Number of glyphs to retrieve adjustments for.</param>
        /// <param name="glyphIndices">Array of glyph id's to retrieve adjustments for.</param>
        /// <param name="glyphAdvanceAdjustments">Returned advances in font design units for each glyph. The last glyph adjustment is zero.</param>
        /// <remarks> This is not a direct replacement for GDI's character based GetKerningPairs, but it serves the same role, without the client needing to cache them locally. It also uses glyph id's directly rather than UCS-2 characters (how the kern table actually stores them) which avoids glyph collapse and ambiguity, such as the dash and hyphen, or space and non-breaking space.</remarks>
        /// <remarks> Newer fonts may have only GPOS kerning instead of the legacy pair table kerning. Such fonts, like Gabriola, will only return 0's for adjustments. This function does not virtualize and flatten these GPOS entries into kerning pairs.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetKerningPairAdjustments(
            [In] IDWriteFontFace1* This,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* glyphIndices,
            [Out, ComAliasName("INT32")] int* glyphAdvanceAdjustments
        );

        /// <summary>Returns whether or not the font supports pair-kerning.</summary>
        /// <remarks> If the font does not support pair table kerning, there is no need to call GetKerningPairAdjustments (it would be all zeroes).</remarks>
        /// <returns> Whether the font supports kerning pairs.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int HasKerningPairs(
            [In] IDWriteFontFace1* This
        );

        /// <summary>Determines the recommended text rendering mode to be used based on the font, size, world transform, and measuring mode.</summary>
        /// <param name="fontEmSize">Logical font size in DIPs.</param>
        /// <param name="dpiX">Number of pixels per logical inch in the horizontal direction.</param>
        /// <param name="dpiY">Number of pixels per logical inch in the vertical direction.</param>
        /// <param name="transform">Specifies the world transform.</param>
        /// <param name="outlineThreshold">Specifies the quality of the graphics system's outline rendering, affects the size threshold above which outline rendering is used.</param>
        /// <param name="measuringMode">Specifies the method used to measure during text layout. For proper glyph spacing, the function returns a rendering mode that is compatible with the specified measuring mode.</param>
        /// <param name="renderingMode">Receives the recommended rendering mode.</param>
        /// <remarks> This method should be used to determine the actual rendering mode in cases where the rendering mode of the rendering params object is DWRITE_RENDERING_MODE_DEFAULT.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRecommendedRenderingMode(
            [In] IDWriteFontFace1* This,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("FLOAT")] float dpiX,
            [In, ComAliasName("FLOAT")] float dpiY,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int isSideways,
            [In] DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [Out] DWRITE_RENDERING_MODE* renderingMode
        );

        /// <summary>Retrieves the vertical forms of the nominal glyphs retrieved from GetGlyphIndices, using the font's 'vert' table. This is used in CJK vertical layout so the correct characters are shown.</summary>
        /// <param name="glyphCount">Number of glyphs to retrieve.</param>
        /// <param name="nominalGlyphIndices">Original glyph indices from cmap.</param>
        /// <param name="verticalGlyphIndices">The vertical form of glyph indices.</param>
        /// <remarks> Call GetGlyphIndices to get the nominal glyph indices, followed by calling this to remap the to the substituted forms, when the run is sideways, and the font has vertical glyph variants.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetVerticalGlyphVariants(
            [In] IDWriteFontFace1* This,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* nominalGlyphIndices,
            [Out, ComAliasName("UINT16")] ushort* verticalGlyphIndices
        );

        /// <summary>Returns whether or not the font has any vertical glyph variants.</summary>
        /// <remarks> For OpenType fonts, this will return true if the font contains a 'vert' feature.</remarks>
        /// <returns> True if the font contains vertical glyph variants.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int HasVerticalGlyphVariants(
            [In] IDWriteFontFace1* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontFace.Vtbl BaseVtbl;

            public IntPtr GetMetrics;

            public IntPtr GetGdiCompatibleMetrics;

            public IntPtr GetCaretMetrics;

            public IntPtr GetUnicodeRanges;

            public IntPtr IsMonospacedFont;

            public IntPtr GetDesignGlyphAdvances;

            public IntPtr GetGdiCompatibleGlyphAdvances;

            public IntPtr GetKerningPairAdjustments;

            public IntPtr HasKerningPairs;

            public IntPtr GetRecommendedRenderingMode;

            public IntPtr GetVerticalGlyphVariants;

            public IntPtr HasVerticalGlyphVariants;
            #endregion
        }
        #endregion
    }
}
