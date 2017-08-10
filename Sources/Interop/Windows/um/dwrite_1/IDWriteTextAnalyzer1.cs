// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Analyzes various text properties for complex script processing.</summary>
    [Guid("80DAD800-E21F-4E83-96CE-BFCCE500DB7C")]
    unsafe public /* blittable */ struct IDWriteTextAnalyzer1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Applies spacing between characters, properly adjusting glyph clusters and diacritics.</summary>
        /// <param name="leadingSpacing">The spacing before each character, in reading order.</param>
        /// <param name="trailingSpacing">The spacing after each character, in reading order.</param>
        /// <param name="minimumAdvanceWidth">The minimum advance of each character, to prevent characters from becoming too thin or zero-width. This must be zero or greater.</param>
        /// <param name="textLength">The length of the clustermap and original text.</param>
        /// <param name="glyphCount">The number of glyphs.</param>
        /// <param name="clusterMap">Mapping from character ranges to glyph ranges.</param>
        /// <param name="glyphAdvances">The advance width of each glyph.</param>
        /// <param name="glyphOffsets">The offset of the origin of each glyph.</param>
        /// <param name="glyphProperties">Properties of each glyph, from GetGlyphs.</param>
        /// <param name="modifiedGlyphAdvances">The new advance width of each glyph.</param>
        /// <param name="modifiedGlyphOffsets">The new offset of the origin of each glyph.</param>
        /// <remarks> The input and output advances/offsets are allowed to alias the same array.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ApplyCharacterSpacing(
            [In] IDWriteTextAnalyzer1* This,
            [In, ComAliasName("FLOAT")] float leadingSpacing,
            [In, ComAliasName("FLOAT")] float trailingSpacing,
            [In, ComAliasName("FLOAT")] float minimumAdvanceWidth,
            [In, ComAliasName("UINT32")] uint textLength,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* clusterMap,
            [In, ComAliasName("FLOAT")] /* readonly */ float* glyphAdvances,
            [In] /* readonly */ DWRITE_GLYPH_OFFSET* glyphOffsets,
            [In] /* readonly */ DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProperties,
            [Out, ComAliasName("FLOAT")] float* modifiedGlyphAdvances,
            [Out] DWRITE_GLYPH_OFFSET* modifiedGlyphOffsets
        );

        /// <summary>Retrieves the given baseline from the font.</summary>
        /// <param name="fontFace">The font face to read.</param>
        /// <param name="baseline">The baseline of interest.</param>
        /// <param name="isVertical">Whether the baseline is vertical or horizontal.</param>
        /// <param name="isSimulationAllowed">Simulate the baseline if it is missing in the font.</param>
        /// <param name="scriptAnalysis">Script analysis result from AnalyzeScript.</param>
        /// <param name="localeName">The language of the run.</param>
        /// <param name="baselineCoordinate">The baseline coordinate value in design units.</param>
        /// <param name="exists">Whether the returned baseline exists in the font.</param>
        /// <remarks> If the baseline does not exist in the font, it is not considered an error, but the function will return exists = false. You may then use heuristics to calculate the missing base, or, if the flag simulationAllowed is true, the function will compute a reasonable approximation for you.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetBaseline(
            [In] IDWriteTextAnalyzer1* This,
            [In] IDWriteFontFace* fontFace,
            [In] DWRITE_BASELINE baseline,
            [In, ComAliasName("BOOL")] int isVertical,
            [In, ComAliasName("BOOL")] int isSimulationAllowed,
            [In] DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            [In, Optional, ComAliasName("WCHAR")] /* readonly */ char* localeName,
            [Out, ComAliasName("INT32")] int* baselineCoordinate,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Analyzes a text range for script orientation, reading text and attributes from the source and reporting results to the sink.</summary>
        /// <param name="analysisSource">Source object to analyze.</param>
        /// <param name="textPosition">Starting position within the source object.</param>
        /// <param name="textLength">Length to analyze.</param>
        /// <param name="analysisSink">Callback object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> All bidi analysis should be resolved before calling this.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AnalyzeVerticalGlyphOrientation(
            [In] IDWriteTextAnalyzer1* This,
            [In] IDWriteTextAnalysisSource1* analysisSource,
            [In, ComAliasName("UINT32")] uint textPosition,
            [In, ComAliasName("UINT32")] uint textLength,
            [In] IDWriteTextAnalysisSink1* analysisSink
        );

        /// <summary>Returns 2x3 transform matrix for the respective angle to draw the glyph run.</summary>
        /// <param name="glyphOrientationAngle">The angle reported into SetGlyphOrientation.</param>
        /// <param name="isSideways">Whether the run's glyphs are sideways or not.</param>
        /// <param name="transform">Returned transform.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> The returned displacement is zero.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGlyphOrientationTransform(
            [In] IDWriteTextAnalyzer1* This,
            [In] DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle,
            [In, ComAliasName("BOOL")] int isSideways,
            [Out] DWRITE_MATRIX* transform
        );

        /// <summary>Returns the properties for a given script.</summary>
        /// <param name="scriptAnalysis">The script for a run of text returned from IDWriteTextAnalyzer::AnalyzeScript.</param>
        /// <param name="scriptProperties">Information for the script.</param>
        /// <returns> Returns properties for the given script. If the script is invalid, it returns generic properties for the unknown script and E_INVALIDARG.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetScriptProperties(
            [In] IDWriteTextAnalyzer1* This,
            [In] DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            [Out] DWRITE_SCRIPT_PROPERTIES* scriptProperties
        );

        /// <summary>Determines the complexity of text, and whether or not full script shaping needs to be called (GetGlyphs).</summary>
        /// <param name="fontFace">The font face to read.</param>
        /// <param name="textLength">Length of the text to check.</param>
        /// <param name="textString">The text to check for complexity. This string may be UTF-16, but any supplementary characters will be considered complex.</param>
        /// <param name="isTextSimple">If true, the text is simple, and the glyphIndices array will already have the nominal glyphs for you. Otherwise you need to call GetGlyphs to properly shape complex scripts and OpenType features.</param>
        /// <param name="textLengthRead">The length read of the text run with the same complexity, simple or complex. You may call again from that point onward.</param>
        /// <param name="glyphIndices">Optional glyph indices for the text. If the function returned that the text was simple, you already have the glyphs you need. Otherwise the glyph indices are not meaningful, and you should call shaping instead.</param>
        /// <remarks> Text is not simple if the characters are part of a script that has complex shaping requirements, require bidi analysis, combine with other characters, reside in the supplementary planes, or have glyphs which participate in standard OpenType features. The length returned will not split combining marks from their base characters.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTextComplexity(
            [In] IDWriteTextAnalyzer1* This,
            [In, ComAliasName("WCHAR")] /* readonly */ char* textString,
            [In, ComAliasName("UINT32")] uint textLength,
            [In] IDWriteFontFace* fontFace,
            [Out, ComAliasName("BOOL")] int* isTextSimple,
            [Out, ComAliasName("UINT32")] uint* textLengthRead,
            [Out, Optional, ComAliasName("UINT16")] ushort* glyphIndices
        );

        /// <summary>Retrieves justification opportunity information for each of the glyphs given the text and shaping glyph properties.</summary>
        /// <param name="fontFace">Font face that was used for shaping. This is mainly important for returning correct results of the kashida width.</param>
        /// <param name="fontEmSize">Font em size used for the glyph run.</param>
        /// <param name="scriptAnalysis">Script of the text from the itemizer.</param>
        /// <param name="textLength">Length of the text.</param>
        /// <param name="glyphCount">Number of glyphs.</param>
        /// <param name="textString">Characters used to produce the glyphs.</param>
        /// <param name="clusterMap">Clustermap produced from shaping.</param>
        /// <param name="glyphProperties">Glyph properties produced from shaping.</param>
        /// <param name="justificationOpportunities">Receives information for the allowed justification expansion/compression for each glyph.</param>
        /// <remarks> This function is called per-run, after shaping is done via GetGlyphs(). Note this function only supports natural metrics (DWRITE_MEASURING_MODE_NATURAL).</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetJustificationOpportunities(
            [In] IDWriteTextAnalyzer1* This,
            [In, Optional] IDWriteFontFace* fontFace,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In] DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            [In, ComAliasName("UINT32")] uint textLength,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("WCHAR")] /* readonly */ char* textString,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* clusterMap,
            [In] /* readonly */ DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProperties,
            [Out] DWRITE_JUSTIFICATION_OPPORTUNITY* justificationOpportunities
        );

        /// <summary>Justifies an array of glyph advances to fit the line width.</summary>
        /// <param name="lineWidth">Width of the line.</param>
        /// <param name="glyphCount">Number of glyphs.</param>
        /// <param name="justificationOpportunities">Opportunities per glyph. Call GetJustificationOpportunities() to get suitable opportunities according to script.</param>
        /// <param name="glyphAdvances">Original glyph advances from shaping.</param>
        /// <param name="glyphOffsets">Original glyph offsets from shaping.</param>
        /// <param name="justifiedGlyphAdvances">Justified glyph advances.</param>
        /// <param name="justifiedGlyphOffsets">Justified glyph offsets.</param>
        /// <remarks> This is called after all the opportunities have been collected, and it spans across the entire line. The input and output arrays are allowed to alias each other, permitting in-place update.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int JustifyGlyphAdvances(
            [In] IDWriteTextAnalyzer1* This,
            [In, ComAliasName("FLOAT")] float lineWidth,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In] /* readonly */ DWRITE_JUSTIFICATION_OPPORTUNITY* justificationOpportunities,
            [In, ComAliasName("FLOAT")] /* readonly */ float* glyphAdvances,
            [In] /* readonly */ DWRITE_GLYPH_OFFSET* glyphOffsets,
            [Out, ComAliasName("FLOAT")] float* justifiedGlyphAdvances,
            [Out, Optional] DWRITE_GLYPH_OFFSET* justifiedGlyphOffsets
        );

        /// <summary>Fills in new glyphs for complex scripts where justification increased the advances of glyphs, such as Arabic with kashida.</summary>
        /// <param name="fontFace">Font face used for shaping.</param>
        /// <param name="fontEmSize">Font em size used for the glyph run.</param>
        /// <param name="scriptAnalysis">Script of the text from the itemizer.</param>
        /// <param name="textLength">Length of the text.</param>
        /// <param name="glyphCount">Number of glyphs.</param>
        /// <param name="maxGlyphCount">Maximum number of output glyphs allocated by caller.</param>
        /// <param name="clusterMap">Clustermap produced from shaping.</param>
        /// <param name="glyphIndices">Original glyphs produced from shaping.</param>
        /// <param name="glyphAdvances">Original glyph advances produced from shaping.</param>
        /// <param name="justifiedGlyphAdvances">Justified glyph advances from JustifyGlyphAdvances().</param>
        /// <param name="justifiedGlyphOffsets">Justified glyph offsets from JustifyGlyphAdvances().</param>
        /// <param name="glyphProperties">Properties of each glyph, from GetGlyphs.</param>
        /// <param name="actualGlyphCount">The new glyph count written to the modified arrays, or the needed glyph count if the size is not large enough.</param>
        /// <param name="modifiedClusterMap">Updated clustermap.</param>
        /// <param name="modifiedGlyphIndices">Updated glyphs with new glyphs inserted where needed.</param>
        /// <param name="modifiedGlyphAdvances">Updated glyph advances.</param>
        /// <param name="modifiedGlyphOffsets">Updated glyph offsets.</param>
        /// <remarks> This is called after the line has been justified, and it is per-run. It only needs to be called if the script has a specific justification character via GetScriptProperties, and it is mainly for cursive scripts like Arabic. If maxGlyphCount is not large enough, the error E_NOT_SUFFICIENT_BUFFER will be returned, with actualGlyphCount holding the final/needed glyph count.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetJustifiedGlyphs(
            [In] IDWriteTextAnalyzer1* This,
            [In, Optional] IDWriteFontFace* fontFace,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In] DWRITE_SCRIPT_ANALYSIS scriptAnalysis,
            [In, ComAliasName("UINT32")] uint textLength,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT32")] uint maxGlyphCount,
            [In, Optional, ComAliasName("UINT16")] /* readonly */ ushort* clusterMap,
            [In, ComAliasName("UINT16")] /* readonly */ ushort* glyphIndices,
            [In, ComAliasName("FLOAT")] /* readonly */ float* glyphAdvances,
            [In, ComAliasName("FLOAT")] /* readonly */ float* justifiedGlyphAdvances,
            [In] /* readonly */ DWRITE_GLYPH_OFFSET* justifiedGlyphOffsets,
            [In] /* readonly */ DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProperties,
            [Out, ComAliasName("UINT32")] uint* actualGlyphCount,
            [Out, Optional, ComAliasName("UINT16")] ushort* modifiedClusterMap,
            [Out, ComAliasName("UINT16")] ushort* modifiedGlyphIndices,
            [Out, ComAliasName("FLOAT")] float* modifiedGlyphAdvances,
            [Out] DWRITE_GLYPH_OFFSET* modifiedGlyphOffsets
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextAnalyzer.Vtbl BaseVtbl;

            public IntPtr ApplyCharacterSpacing;

            public IntPtr GetBaseline;

            public IntPtr AnalyzeVerticalGlyphOrientation;

            public IntPtr GetGlyphOrientationTransform;

            public IntPtr GetScriptProperties;

            public IntPtr GetTextComplexity;

            public IntPtr GetJustificationOpportunities;

            public IntPtr JustifyGlyphAdvances;

            public IntPtr GetJustifiedGlyphs;
            #endregion
        }
        #endregion
    }
}
