// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The text analyzer interface represents a set of application-defined callbacks that perform rendering of text, inline objects, and decorations such as underlines.</summary>
    [Guid("553A9FF3-5693-4DF7-B52B-74806F7F2EB9")]
    unsafe public /* blittable */ struct IDWriteTextAnalyzer2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Returns 2x3 transform matrix for the respective angle to draw the glyph run or other object.</summary>
        /// <param name="glyphOrientationAngle">The angle reported to one of the application callbacks, including IDWriteTextAnalysisSink1::SetGlyphOrientation and IDWriteTextRenderer1::Draw*.</param>
        /// <param name="isSideways">Whether the run's glyphs are sideways or not.</param>
        /// <param name="originX">X origin of the element, be it a glyph run or underline or other.</param>
        /// <param name="originY">Y origin of the element, be it a glyph run or underline or other.</param>
        /// <param name="transform">Returned transform.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> This rotates around the given origin x and y, returning a translation component such that the glyph run, text decoration, or inline object is drawn with the right orientation at the expected coordinate.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGlyphOrientationTransform(
            [In] IDWriteTextAnalyzer2* This,
            [In] DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, ComAliasName("FLOAT")] float originX,
            [In, ComAliasName("FLOAT")] float originY,
            [Out] DWRITE_MATRIX* transform
        );

        /// <summary>Returns a list of typographic feature tags for the given script and language.</summary>
        /// <param name="fontFace">The font face to get features from.</param>
        /// <param name="scriptAnalysis">Script analysis result from AnalyzeScript.</param>
        /// <param name="localeName">The locale to use when selecting the feature, such en-us or ja-jp.</param>
        /// <param name="maxTagCount">Maximum tag count.</param>
        /// <param name="actualTagCount">Actual tag count. If greater than maxTagCount, E_NOT_SUFFICIENT_BUFFER is returned, and the call should be retried with a larger buffer.</param>
        /// <param name="tags">Feature tag list.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypographicFeatures(
            [In] IDWriteTextAnalyzer2* This,
            [In] IDWriteFontFace* fontFace,
            [In] DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            [In, Optional, ComAliasName("WCHAR")] /* readonly */ char* localeName,
            [In, ComAliasName("UINT32")] uint maxTagCount,
            [Out, ComAliasName("UINT32")] uint* actualTagCount,
            [Out] DWRITE_FONT_FEATURE_TAG* tags
        );

        /// <summary>Returns an array of which glyphs are affected by a given feature.</summary>
        /// <param name="fontFace">The font face to read glyph information from.</param>
        /// <param name="scriptAnalysis">Script analysis result from AnalyzeScript.</param>
        /// <param name="localeName">The locale to use when selecting the feature, such en-us or ja-jp.</param>
        /// <param name="featureTag">OpenType feature name to use, which may be one of the DWRITE_FONT_FEATURE_TAG values or a custom feature using DWRITE_MAKE_OPENTYPE_TAG.</param>
        /// <param name="glyphCount">Number of glyph indices to check.</param>
        /// <param name="glyphIndices">Glyph indices to check for feature application.</param>
        /// <param name="featureApplies">Output of which glyphs are affected by the feature, where for each glyph affected, the respective array index will be 1. The result is returned per-glyph without regard to neighboring context of adjacent glyphs.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CheckTypographicFeature(
            [In] IDWriteTextAnalyzer2* This,
            [In] IDWriteFontFace* fontFace,
            [In] DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            [In, Optional, ComAliasName("WCHAR")] /* readonly */ char* localeName,
            [In] DWRITE_FONT_FEATURE_TAG featureTag,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* glyphIndices,
            [Out, ComAliasName("UINT8")] byte* featureApplies
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextAnalyzer1.Vtbl BaseVtbl;

            public IntPtr GetGlyphOrientationTransform;

            public IntPtr GetTypographicFeatures;

            public IntPtr CheckTypographicFeature;
            #endregion
        }
        #endregion
    }
}
