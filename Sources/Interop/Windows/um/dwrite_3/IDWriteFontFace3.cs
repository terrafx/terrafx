// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    /// <summary>The interface that represents an absolute reference to a font face. It contains font face type, appropriate file references and face identification data. Various font data such as metrics, names and glyph outlines is obtained from IDWriteFontFace.</summary>
    [Guid("D37D7598-09BE-4222-A236-2081341CC1F2")]
    public /* blittable */ unsafe struct IDWriteFontFace3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteFontFace3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteFontFace3* This
        );
        #endregion

        #region IDWriteFontFace Delegates
        /// <summary>Obtains the file format type of a font face.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_FACE_TYPE _GetType(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Obtains the font files representing a font face.</summary>
        /// <param name="numberOfFiles">The number of files representing the font face.</param>
        /// <param name="fontFiles">User provided array that stores pointers to font files representing the font face. This parameter can be NULL if the user is only interested in the number of files representing the font face. This API increments reference count of the font file pointers returned according to COM conventions, and the client should release them when finished.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFiles(
            [In] IDWriteFontFace3* This,
            [In, Out, ComAliasName("UINT32")] uint* numberOfFiles,
            [Out] IDWriteFontFile** fontFiles = null
        );

        /// <summary>Obtains the zero-based index of the font face in its font file or files. If the font files contain a single face, the return value is zero.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetIndex(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Obtains the algorithmic style simulation flags of a font face.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_SIMULATIONS GetSimulations(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Determines whether the font is a symbol font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsSymbolFont(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Obtains design units and common metrics for the font face. These metrics are applicable to all the glyphs within a fontface and are used by applications for layout calculations.</summary>
        /// <param name="fontFaceMetrics">Points to a DWRITE_FONT_METRICS public structure to fill in. The metrics returned by this function are in font design units.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetMetrics(
            [In] IDWriteFontFace3* This,
            [Out] DWRITE_FONT_METRICS* fontFaceMetrics
        );

        /// <summary>Obtains the number of glyphs in the font face.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT16")]
        public /* static */ delegate ushort GetGlyphCount(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Obtains ideal glyph metrics in font design units. Design glyphs metrics are used for glyph positioning.</summary>
        /// <param name="glyphIndices">An array of glyph indices to compute the metrics for.</param>
        /// <param name="glyphCount">The number of elements in the glyphIndices array.</param>
        /// <param name="glyphMetrics">Array of DWRITE_GLYPH_METRICS public structures filled by this function. The metrics returned by this function are in font design units.</param>
        /// <param name="isSideways">Indicates whether the font is being used in a sideways run. This can affect the glyph metrics if the font has oblique simulation because sideways oblique simulation differs from non-sideways oblique simulation.</param>
        /// <returns>Standard HRESULT error code. If any of the input glyph indices are outside of the valid glyph index range for the current font face, E_INVALIDARG will be returned.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDesignGlyphMetrics(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT16[]")] ushort* glyphIndices,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [Out] DWRITE_GLYPH_METRICS* glyphMetrics,
            [In, ComAliasName("BOOL")] int isSideways = FALSE
        );

        /// <summary>Returns the nominal mapping of UTF-32 Unicode code points to glyph indices as defined by the font 'cmap' table. Note that this mapping is primarily provided for line layout engines built on top of the physical font API. Because of OpenType glyph substitution and line layout character substitution, the nominal conversion does not always correspond to how a Unicode string will map to glyph indices when rendering using a particular font face. Also, note that Unicode Variation Selectors provide for alternate mappings for character to glyph. This call will always return the default variant.</summary>
        /// <param name="codePoints">An array of UTF-32 code points to obtain nominal glyph indices from.</param>
        /// <param name="codePointCount">The number of elements in the codePoints array.</param>
        /// <param name="glyphIndices">Array of nominal glyph indices filled by this function.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGlyphIndices(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32[]")] uint* codePoints,
            [In, ComAliasName("UINT32")] uint codePointCount,
            [Out, ComAliasName("UINT16")] ushort* glyphIndices
        );

        /// <summary>Finds the specified OpenType font table if it exists and returns a pointer to it. The function accesses the underlying font data via the IDWriteFontFileStream interface implemented by the font file loader.</summary>
        /// <param name="openTypeTableTag">Four character tag of table to find. Use the DWRITE_MAKE_OPENTYPE_TAG() macro to create it. Unlike GDI, it does not support the special TTCF and null tags to access the whole font.</param>
        /// <param name="tableData">Pointer to base of table in memory. The pointer is only valid so long as the FontFace used to get the font table still exists (not any other FontFace, even if it actually refers to the same physical font).</param>
        /// <param name="tableSize">Byte size of table.</param>
        /// <param name="tableContext">Opaque context which must be freed by calling ReleaseFontTable. The context actually comes from the lower level IDWriteFontFileStream, which may be implemented by the application or DWrite itself. It is possible for a NULL tableContext to be returned, especially if the implementation directly memory maps the whole file. Nevertheless, always release it later, and do not use it as a test for function success. The same table can be queried multiple times, but each returned context can be different, so release each separately.</param>
        /// <param name="exists">True if table exists.</param>
        /// <returns>Standard HRESULT error code.If a table can not be found, the function will not return an error, but the size will be 0, table NULL, and exists = FALSE. The context does not need to be freed if the table was not found.</returns>
        /// <remarks>The context for the same tag may be different for each call,so each one must be held and released separately.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int TryGetFontTable(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint openTypeTableTag,
            [Out] void** tableData,
            [Out, ComAliasName("UINT32")] uint* tableSize,
            [Out] void** tableContext,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Releases the table obtained earlier from TryGetFontTable.</summary>
        /// <param name="tableContext">Opaque context from TryGetFontTable.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseFontTable(
            [In] IDWriteFontFace3* This,
            [In] void* tableContext
        );

        /// <summary>Computes the outline of a run of glyphs by calling back to the outline sink interface.</summary>
        /// <param name="emSize">Logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch.</param>
        /// <param name="glyphIndices">Array of glyph indices.</param>
        /// <param name="glyphAdvances">Optional array of glyph advances in DIPs.</param>
        /// <param name="glyphOffsets">Optional array of glyph offsets.</param>
        /// <param name="glyphCount">Number of glyphs.</param>
        /// <param name="isSideways">If true, specifies that glyphs are rotated 90 degrees to the left and vertical metrics are used. A client can render a vertical run by specifying isSideways = true and rotating the resulting geometry 90 degrees to the right using a transform.</param>
        /// <param name="isRightToLeft">If true, specifies that the advance direction is right to left. By default, the advance direction is left to right.</param>
        /// <param name="geometrySink">Interface the function calls back to draw each element of the geometry.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGlyphRunOutline(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("UINT16[]")] ushort* glyphIndices,
            [In, Optional, ComAliasName("FLOAT[]")] float* glyphAdvances,
            [In, Optional, ComAliasName("DWRITE_GLYPH_OFFSET[]")] DWRITE_GLYPH_OFFSET* glyphOffsets,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, ComAliasName("BOOL")] int isRightToLeft,
            [In, ComAliasName("IDWriteGeometrySink")] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Determines the recommended rendering mode for the font given the specified size and rendering parameters.</summary>
        /// <param name="emSize">Logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch.</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP. For example, if the DPI of the rendering surface is 96 this value is 1.0f. If the DPI is 120, this value is 120.0f/96.</param>
        /// <param name="measuringMode">Specifies measuring mode that will be used for glyphs in the font. Renderer implementations may choose different rendering modes for given measuring modes, but best results are seen when the corresponding modes match: DWRITE_RENDERING_MODE_CLEARTYPE_NATURAL for DWRITE_MEASURING_MODE_NATURAL DWRITE_RENDERING_MODE_CLEARTYPE_GDI_CLASSIC for DWRITE_MEASURING_MODE_GDI_CLASSIC DWRITE_RENDERING_MODE_CLEARTYPE_GDI_NATURAL for DWRITE_MEASURING_MODE_GDI_NATURAL</param>
        /// <param name="renderingParams">Rendering parameters object. This parameter is necessary in case the rendering parameters object overrides the rendering mode.</param>
        /// <param name="renderingMode">Receives the recommended rendering mode to use.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRecommendedRenderingMode(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] IDWriteRenderingParams* renderingParams,
            [Out] DWRITE_RENDERING_MODE* renderingMode
        );

        /// <summary>Obtains design units and common metrics for the font face. These metrics are applicable to all the glyphs within a fontface and are used by applications for layout calculations.</summary>
        /// <param name="emSize">Logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch.</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP. For example, if the DPI of the rendering surface is 96 this value is 1.0f. If the DPI is 120, this value is 120.0f/96.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the font size and pixelsPerDip.</param>
        /// <param name="fontFaceMetrics">Points to a DWRITE_FONT_METRICS public structure to fill in. The metrics returned by this function are in font design units.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGdiCompatibleMetrics(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] DWRITE_MATRIX* transform,
            [Out] DWRITE_FONT_METRICS* fontFaceMetrics
        );

        /// <summary>Obtains glyph metrics in font design units with the return values compatible with what GDI would produce. Glyphs metrics are used for positioning of individual glyphs.</summary>
        /// <param name="emSize">Logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch.</param>
        /// <param name="pixelsPerDip">Number of physical pixels per DIP. For example, if the DPI of the rendering surface is 96 this value is 1.0f. If the DPI is 120, this value is 120.0f/96.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the font size and pixelsPerDip.</param>
        /// <param name="useGdiNatural"> When set to FALSE, the metrics are the same as the metrics of GDI aliased text. When set to TRUE, the metrics are the same as the metrics of text measured by GDI using a font created with CLEARTYPE_NATURAL_QUALITY.</param>
        /// <param name="glyphIndices">An array of glyph indices to compute the metrics for.</param>
        /// <param name="glyphCount">The number of elements in the glyphIndices array.</param>
        /// <param name="glyphMetrics">Array of DWRITE_GLYPH_METRICS public structures filled by this function. The metrics returned by this function are in font design units.</param>
        /// <param name="isSideways">Indicates whether the font is being used in a sideways run. This can affect the glyph metrics if the font has oblique simulation because sideways oblique simulation differs from non-sideways oblique simulation.</param>
        /// <returns>Standard HRESULT error code. If any of the input glyph indices are outside of the valid glyph index range for the current font face, E_INVALIDARG will be returned.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGdiCompatibleGlyphMetrics(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int useGdiNatural,
            [In, ComAliasName("UINT16[]")] ushort* glyphIndices,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [Out, ComAliasName("DWRITE_GLYPH_METRICS[]")] DWRITE_GLYPH_METRICS* glyphMetrics,
            [In, ComAliasName("BOOL")] int isSideways = FALSE
        );
        #endregion

        #region IDWriteFontFace1 Delegates
        /// <summary>Gets common metrics for the font in design units. These metrics are applicable to all the glyphs within a font, and are used by applications for layout calculations.</summary>
        /// <param name="fontMetrics">Metrics public structure to fill in.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetMetrics1(
            [In] IDWriteFontFace3* This,
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
        public /* static */ delegate int GetGdiCompatibleMetrics1(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] DWRITE_MATRIX* transform,
            [Out] DWRITE_FONT_METRICS1* fontMetrics
        );

        /// <summary>Gets caret metrics for the font in design units. These are used by text editors for drawing the correct caret placement/slant.</summary>
        /// <param name="caretMetrics">Metrics public structure to fill in.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetCaretMetrics(
            [In] IDWriteFontFace3* This,
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
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint maxRangeCount,
            [Out, Optional, ComAliasName("DWRITE_UNICODE_RANGE[]")] DWRITE_UNICODE_RANGE* unicodeRanges,
            [Out, ComAliasName("UINT32")] uint* actualRangeCount
        );

        /// <summary>Returns true if the font is monospaced, meaning its characters are the same fixed-pitch width (non-proportional).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsMonospacedFont(
            [In] IDWriteFontFace3* This
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
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16[]")] ushort* glyphIndices,
            [Out, ComAliasName("INT32[]")] int* glyphAdvances,
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
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float emSize,
            [In, ComAliasName("FLOAT")] float pixelsPerDip,
            [In, Optional] DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int useGdiNatural,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16[]")] ushort* glyphIndices,
            [Out, ComAliasName("INT32[]")] int* glyphAdvances
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
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16[]")] ushort* glyphIndices,
            [Out, ComAliasName("INT32[]")] int* glyphAdvanceAdjustments
        );

        /// <summary>Returns whether or not the font supports pair-kerning.</summary>
        /// <remarks> If the font does not support pair table kerning, there is no need to call GetKerningPairAdjustments (it would be all zeroes).</remarks>
        /// <returns> Whether the font supports kerning pairs.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int HasKerningPairs(
            [In] IDWriteFontFace3* This
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
        public /* static */ delegate int GetRecommendedRenderingMode1(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("FLOAT")] float dpiX,
            [In, ComAliasName("FLOAT")] float dpiY,
            [In, Optional] DWRITE_MATRIX* transform,
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
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("UINT16[]")] ushort* nominalGlyphIndices,
            [Out, ComAliasName("UINT16[]")] ushort* verticalGlyphIndices
        );

        /// <summary>Returns whether or not the font has any vertical glyph variants.</summary>
        /// <remarks> For OpenType fonts, this will return true if the font contains a 'vert' feature.</remarks>
        /// <returns> True if the font contains vertical glyph variants.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int HasVerticalGlyphVariants(
            [In] IDWriteFontFace3* This
        );
        #endregion

        #region IDWriteFontFace2 Delegates
        /// <summary>Returns TRUE if the font contains tables that can provide color information (including COLR, CPAL, SVG, CBDT, sbix  tables), or FALSE if not. Note that TRUE is returned even in the case when the font tables contain only grayscale images.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsColorFont(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Returns the number of color palettes defined by the font. The return value is zero if the font has no color information. Color fonts must have at least one palette, with palette index zero being the default.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetColorPaletteCount(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Returns the number of entries in each color palette. All color palettes in a font have the same number of palette entries. The return value is zero if the font has no color information.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetPaletteEntryCount(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Reads color values from the font's color palette.</summary>
        /// <param name="colorPaletteIndex">Zero-based index of the color palette. If the font does not have a palette with the specified index, the method returns DWRITE_E_NOCOLOR.</param>
        /// <param name="firstEntryIndex">Zero-based index of the first palette entry to read.</param>
        /// <param name="entryCount">Number of palette entries to read.</param>
        /// <param name="paletteEntries">Array that receives the color values.</param>
        /// <returns> Standard HRESULT error code. The return value is E_INVALIDARG if firstEntryIndex + entryCount is greater than the actual number of palette entries as returned by GetPaletteEntryCount. The return value is DWRITE_E_NOCOLOR if the font does not have a palette with the specified palette index.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPaletteEntries(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint colorPaletteIndex,
            [In, ComAliasName("UINT32")] uint firstEntryIndex,
            [In, ComAliasName("UINT32")] uint entryCount,
            [Out, ComAliasName("DWRITE_COLOR_F[]")] DXGI_RGBA* paletteEntries
        );

        /// <summary>Determines the recommended text rendering and grid-fit mode to be used based on the font, size, world transform, and measuring mode.</summary>
        /// <param name="fontEmSize">Logical font size in DIPs.</param>
        /// <param name="dpiX">Number of pixels per logical inch in the horizontal direction.</param>
        /// <param name="dpiY">Number of pixels per logical inch in the vertical direction.</param>
        /// <param name="transform">Specifies the world transform.</param>
        /// <param name="outlineThreshold">Specifies the quality of the graphics system's outline rendering, affects the size threshold above which outline rendering is used.</param>
        /// <param name="measuringMode">Specifies the method used to measure during text layout. For proper glyph spacing, the function returns a rendering mode that is compatible with the specified measuring mode.</param>
        /// <param name="renderingParams">Rendering parameters object. This parameter is necessary in case the rendering parameters object overrides the rendering mode.</param>
        /// <param name="renderingMode">Receives the recommended rendering mode.</param>
        /// <param name="gridFitMode">Receives the recommended grid-fit mode.</param>
        /// <remarks> This method should be used to determine the actual rendering mode in cases where the rendering mode of the rendering params object is DWRITE_RENDERING_MODE_DEFAULT, and the actual grid-fit mode when the rendering params object is DWRITE_GRID_FIT_MODE_DEFAULT.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRecommendedRenderingMode2(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("FLOAT")] float dpiX,
            [In, ComAliasName("FLOAT")] float dpiY,
            [In, Optional] DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int isSideways,
            [In] DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, Optional] IDWriteRenderingParams* renderingParams,
            [Out] DWRITE_RENDERING_MODE* renderingMode,
            [Out] DWRITE_GRID_FIT_MODE* gridFitMode
        );
        #endregion

        #region Delegates
        /// <summary>Return a font face reference identifying this font.</summary>
        /// <param name="fontFaceReference">A uniquely identifying reference to a font face.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFaceReference(
            [In] IDWriteFontFace3* This,
            [Out] IDWriteFontFaceReference** fontFaceReference
        );

        /// <summary>Gets the PANOSE values from the font, used for font selection and matching.</summary>
        /// <param name="panose">PANOSE public structure to fill in.</param>
        /// <remarks> The function does not simulate these, such as substituting a weight or proportion inferred on other values. If the font does not specify them, they are all set to 'any' (0).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetPanose(
            [In] IDWriteFontFace3* This,
            [Out] DWRITE_PANOSE* panose
        );

        /// <summary>Gets the weight of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_WEIGHT GetWeight(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Gets the stretch (aka. width) of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STRETCH GetStretch(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Gets the style (aka. slope) of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STYLE GetStyle(
            [In] IDWriteFontFace3* This
        );

        /// <summary>Creates an localized strings object that contains the family names for the font family, indexed by locale name.</summary>
        /// <param name="names">Receives a pointer to the newly created localized strings object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFamilyNames(
            [In] IDWriteFontFace3* This,
            [Out] IDWriteLocalizedStrings** names
        );

        /// <summary>Gets a localized strings collection containing the face names for the font (e.g., Regular or Bold), indexed by locale name.</summary>
        /// <param name="names">Receives a pointer to the newly created localized strings object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFaceNames(
            [In] IDWriteFontFace3* This,
            [Out] IDWriteLocalizedStrings** names
        );

        /// <summary>Gets a localized strings collection containing the specified informational strings, indexed by locale name.</summary>
        /// <param name="informationalStringID">Identifies the string to get.</param>
        /// <param name="informationalStrings">Receives a pointer to the newly created localized strings object.</param>
        /// <param name="exists">Receives the value TRUE if the font contains the specified string ID or FALSE if not.</param>
        /// <returns> Standard HRESULT error code. If the font does not contain the specified string, the return value is S_OK but informationalStrings receives a NULL pointer and exists receives the value FALSE.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetInformationalStrings(
            [In] IDWriteFontFace3* This,
            [In] DWRITE_INFORMATIONAL_STRING_ID informationalStringID,
            [Out] IDWriteLocalizedStrings** informationalStrings,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Determines whether the font supports the specified character.</summary>
        /// <param name="unicodeValue">Unicode (UCS-4) character value.</param>
        /// <returns> Returns TRUE if the font has the specified character, FALSE if not.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int HasCharacter(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint unicodeValue
        );

        /// <summary>Determines the recommended text rendering and grid-fit mode to be used based on the font, size, world transform, and measuring mode.</summary>
        /// <param name="fontEmSize">Logical font size in DIPs.</param>
        /// <param name="dpiX">Number of pixels per logical inch in the horizontal direction.</param>
        /// <param name="dpiY">Number of pixels per logical inch in the vertical direction.</param>
        /// <param name="transform">Specifies the world transform.</param>
        /// <param name="outlineThreshold">Specifies the quality of the graphics system's outline rendering, affects the size threshold above which outline rendering is used.</param>
        /// <param name="measuringMode">Specifies the method used to measure during text layout. For proper glyph spacing, the function returns a rendering mode that is compatible with the specified measuring mode.</param>
        /// <param name="renderingParams">Rendering parameters object. This parameter is necessary in case the rendering parameters object overrides the rendering mode.</param>
        /// <param name="renderingMode">Receives the recommended rendering mode.</param>
        /// <param name="gridFitMode">Receives the recommended grid-fit mode.</param>
        /// <remarks> This method should be used to determine the actual rendering mode in cases where the rendering mode of the rendering params object is DWRITE_RENDERING_MODE_DEFAULT, and the actual grid-fit mode when the rendering params object is DWRITE_GRID_FIT_MODE_DEFAULT.</remarks>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRecommendedRenderingMode3(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("FLOAT")] float dpiX,
            [In, ComAliasName("FLOAT")] float dpiY,
            [In, Optional] DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int isSideways,
            [In] DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, Optional] IDWriteRenderingParams* renderingParams,
            [Out] DWRITE_RENDERING_MODE1* renderingMode,
            [Out] DWRITE_GRID_FIT_MODE* gridFitMode
        );

        /// <summary>Determines whether the character is locally downloaded from the font.</summary>
        /// <param name="unicodeValue">Unicode (UCS-4) character value.</param>
        /// <returns> Returns TRUE if the font has the specified character locally available, FALSE if not or if the font does not support that character.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsCharacterLocal(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT32")] uint unicodeValue
        );

        /// <summary>Determines whether the glyph is locally downloaded from the font.</summary>
        /// <param name="glyphId">Glyph identifier.</param>
        /// <returns> Returns TRUE if the font has the specified glyph locally available.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsGlyphLocal(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT16")] ushort glyphId
        );

        /// <summary>Determines whether the specified characters are local.</summary>
        /// <param name="characters">Array of characters.</param>
        /// <param name="characterCount">The number of elements in the character array.</param>
        /// <param name="enqueueIfNotLocal">Specifies whether to enqueue a download request if any of the specified characters are not local.</param>
        /// <param name="isLocal">Receives TRUE if all of the specified characters are local, FALSE if any of the specified characters are remote.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AreCharactersLocal(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("WCHAR[]")] char* characters,
            [In, ComAliasName("UINT32")] uint characterCount,
            [In, ComAliasName("BOOL")] int enqueueIfNotLocal,
            [Out, ComAliasName("BOOL")] int* isLocal
        );

        /// <summary>Determines whether the specified glyphs are local.</summary>
        /// <param name="glyphIndices">Array of glyph indices.</param>
        /// <param name="glyphCount">The number of elements in the glyph index array.</param>
        /// <param name="enqueueIfNotLocal">Specifies whether to enqueue a download request if any of the specified glyphs are not local.</param>
        /// <param name="isLocal">Receives TRUE if all of the specified glyphs are local, FALSE if any of the specified glyphs are remote.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AreGlyphsLocal(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("UINT16[]")] ushort* glyphIndices,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("BOOL")] int enqueueIfNotLocal,
            [Out, ComAliasName("BOOL")] int* isLocal
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region IDWriteFontFace Fields
            public IntPtr _GetType;

            public IntPtr GetFiles;

            public IntPtr GetIndex;

            public IntPtr GetSimulations;

            public IntPtr IsSymbolFont;

            public IntPtr GetMetrics;

            public IntPtr GetGlyphCount;

            public IntPtr GetDesignGlyphMetrics;

            public IntPtr GetGlyphIndices;

            public IntPtr TryGetFontTable;

            public IntPtr ReleaseFontTable;

            public IntPtr GetGlyphRunOutline;

            public IntPtr GetRecommendedRenderingMode;

            public IntPtr GetGdiCompatibleMetrics;

            public IntPtr GetGdiCompatibleGlyphMetrics;
            #endregion

            #region IDWriteFontFace1 Fields
            public IntPtr GetMetrics1;

            public IntPtr GetGdiCompatibleMetrics1;

            public IntPtr GetCaretMetrics;

            public IntPtr GetUnicodeRanges;

            public IntPtr IsMonospacedFont;

            public IntPtr GetDesignGlyphAdvances;

            public IntPtr GetGdiCompatibleGlyphAdvances;

            public IntPtr GetKerningPairAdjustments;

            public IntPtr HasKerningPairs;

            public IntPtr GetRecommendedRenderingMode1;

            public IntPtr GetVerticalGlyphVariants;

            public IntPtr HasVerticalGlyphVariants;
            #endregion

            #region IDWriteFontFace2 Fields
            public IntPtr IsColorFont;

            public IntPtr GetColorPaletteCount;

            public IntPtr GetPaletteEntryCount;

            public IntPtr GetPaletteEntries;

            public IntPtr GetRecommendedRenderingMode2;
            #endregion

            #region Fields
            public IntPtr GetFontFaceReference;

            public IntPtr GetPanose;

            public IntPtr GetWeight;

            public IntPtr GetStretch;

            public IntPtr GetStyle;

            public IntPtr GetFamilyNames;

            public IntPtr GetFaceNames;

            public IntPtr GetInformationalStrings;

            public IntPtr HasCharacter;

            public IntPtr GetRecommendedRenderingMode3;

            public IntPtr IsCharacterLocal;

            public IntPtr IsGlyphLocal;

            public IntPtr AreCharactersLocal;

            public IntPtr AreGlyphsLocal;
            #endregion
        }
        #endregion
    }
}
