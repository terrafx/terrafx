// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.Windows;

namespace TerraFX.Interop
{
    /// <summary>The root factory interface for all DWrite objects.</summary>
    [Guid("9A1B41C3-D3BB-466A-87FC-FE67556A3B65")]
    unsafe public /* blittable */ struct IDWriteFactory3
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a glyph run analysis object, which encapsulates information used to render a glyph run.</summary>
        /// <param name="glyphRun">Structure specifying the properties of the glyph run.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the emSize.</param>
        /// <param name="renderingMode">Specifies the rendering mode, which must be one of the raster rendering modes (i.e., not default and not outline).</param>
        /// <param name="measuringMode">Specifies the method to measure glyphs.</param>
        /// <param name="gridFitMode">How to grid-fit glyph outlines. This must be non-default.</param>
        /// <param name="baselineOriginX">Horizontal position of the baseline origin, in DIPs.</param>
        /// <param name="baselineOriginY">Vertical position of the baseline origin, in DIPs.</param>
        /// <param name="glyphRunAnalysis">Receives a pointer to the newly created object.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGlyphRunAnalysis(
            [In] IDWriteFactory3* This,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform,
            [In] DWRITE_RENDERING_MODE1 renderingMode,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [In] DWRITE_TEXT_ANTIALIAS_MODE antialiasMode,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [Out] IDWriteGlyphRunAnalysis** glyphRunAnalysis
        );

        /// <summary>Creates a rendering parameters object with the specified properties.</summary>
        /// <param name="gamma">The gamma value used for gamma correction, which must be greater than zero and cannot exceed 256.</param>
        /// <param name="enhancedContrast">The amount of contrast enhancement, zero or greater.</param>
        /// <param name="grayscaleEnhancedContrast">The amount of contrast enhancement to use for grayscale antialiasing, zero or greater.</param>
        /// <param name="clearTypeLevel">The degree of ClearType level, from 0.0f (no ClearType) to 1.0f (full ClearType).</param>
        /// <param name="pixelGeometry">The geometry of a device pixel.</param>
        /// <param name="renderingMode">Method of rendering glyphs. In most cases, this should be DWRITE_RENDERING_MODE_DEFAULT to automatically use an appropriate mode.</param>
        /// <param name="gridFitMode">How to grid fit glyph outlines. In most cases, this should be DWRITE_GRID_FIT_DEFAULT to automatically choose an appropriate mode.</param>
        /// <param name="renderingParams">Holds the newly created rendering parameters object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCustomRenderingParams(
            [In] IDWriteFactory3* This,
            [In, ComAliasName("FLOAT")] float gamma,
            [In, ComAliasName("FLOAT")] float enhancedContrast,
            [In, ComAliasName("FLOAT")] float grayscaleEnhancedContrast,
            [In, ComAliasName("FLOAT")] float clearTypeLevel,
            [In] DWRITE_PIXEL_GEOMETRY pixelGeometry,
            [In] DWRITE_RENDERING_MODE1 renderingMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [Out] IDWriteRenderingParams3** renderingParams
        );

        /// <summary>Creates a reference to a font given a full path.</summary>
        /// <param name="filePath">Absolute file path. Subsequent operations on the conpublic /* blittable */ structed object may fail if the user provided filePath doesn't correspond to a valid file on the disk.</param>
        /// <param name="lastWriteTime">Last modified time of the input file path. If the parameter is omitted, the function will access the font file to obtain its last write time, so the clients are encouraged to specify this value to avoid extra disk access. Subsequent operations on the conpublic /* blittable */ structed object may fail if the user provided lastWriteTime doesn't match the file on the disk.</param>
        /// <param name="faceIndex">The zero based index of a font face in cases when the font files contain a collection of font faces. If the font files contain a single face, this value should be zero.</param>
        /// <param name="fontSimulations">Font face simulation flags for algorithmic emboldening and italicization.</param>
        /// <param name="fontFaceReference">Contains newly created font face reference object, or nullptr in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFaceReference(
            [In] IDWriteFactory3* This,
            [In, ComAliasName("WCHAR")] /* readonly */ char* filePath,
            [In, Optional] /* readonly */ FILETIME* lastWriteTime,
            [In, ComAliasName("UINT32")] uint faceIndex,
            [In] DWRITE_FONT_SIMULATIONS fontSimulations,
            [Out] IDWriteFontFaceReference** fontFaceReference
        );

        /// <summary>Creates a reference to a font given a file.</summary>
        /// <param name="fontFile">User provided font file representing the font face.</param>
        /// <param name="faceIndex">The zero based index of a font face in cases when the font files contain a collection of font faces. If the font files contain a single face, this value should be zero.</param>
        /// <param name="fontSimulations">Font face simulation flags for algorithmic emboldening and italicization.</param>
        /// <param name="fontFaceReference">Contains newly created font face reference object, or nullptr in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFaceReference1(
            [In] IDWriteFactory3* This,
            [In] IDWriteFontFile* fontFile,
            [In, ComAliasName("UINT32")] uint faceIndex,
            [In] DWRITE_FONT_SIMULATIONS fontSimulations,
            [Out] IDWriteFontFaceReference** fontFaceReference
        );

        /// <summary>Retrieves the list of system fonts.</summary>
        /// <param name="fontSet">Holds the newly created font set object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSystemFontSet(
            [In] IDWriteFactory3* This,
            [Out] IDWriteFontSet** fontSet
        );

        /// <summary>Creates an empty font set builder to add font face references and create a custom font set.</summary>
        /// <param name="fontSetBuilder">Holds the newly created font set builder object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontSetBuilder(
            [In] IDWriteFactory3* This,
            [Out] IDWriteFontSetBuilder** fontSetBuilder
        );

        /// <summary>Create a weight/width/slope tree from a set of fonts.</summary>
        /// <param name="fontSet">A set of fonts to use to build the collection.</param>
        /// <param name="fontCollection">Holds the newly created font collection object, or NULL in case of failure.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontCollectionFromFontSet(
            [In] IDWriteFactory3* This,
            [In] IDWriteFontSet* fontSet,
            [Out] IDWriteFontCollection1** fontCollection
        );

        /// <summary>Retrieves a weight/width/slope tree of system fonts.</summary>
        /// <param name="includeDownloadableFonts">Include cloud fonts or only locally installed ones.</param>
        /// <param name="fontCollection">Holds the newly created font collection object, or NULL in case of failure.</param>
        /// <param name="checkForUpdates">If this parameter is nonzero, the function performs an immediate check for changes to the set of system fonts. If this parameter is FALSE, the function will still detect changes if the font cache service is running, but there may be some latency. For example, an application might specify TRUE if it has itself just installed a font and wants to be sure the font collection contains that font.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSystemFontCollection(
            [In] IDWriteFactory3* This,
            [In, ComAliasName("BOOL")] int includeDownloadableFonts,
            [Out] IDWriteFontCollection1** fontCollection,
            [In, DefaultParameterValue(FALSE), ComAliasName("BOOL")] int checkForUpdates
        );

        /// <summary>Gets the font download queue associated with this factory object.</summary>
        /// <param name="fontDownloadQueue">Receives a pointer to the font download queue interface.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontDownloadQueue(
            [In] IDWriteFactory3* This,
            [Out] IDWriteFontDownloadQueue** fontDownloadQueue
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFactory2.Vtbl BaseVtbl;

            public IntPtr CreateGlyphRunAnalysis;

            public IntPtr CreateCustomRenderingParams;

            public IntPtr CreateFontFaceReference;

            public IntPtr CreateFontFaceReference1;

            public IntPtr GetSystemFontSet;

            public IntPtr CreateFontSetBuilder;

            public IntPtr CreateFontCollectionFromFontSet;

            public IntPtr GetSystemFontCollection;

            public IntPtr GetFontDownloadQueue;
            #endregion
        }
        #endregion
    }
}
