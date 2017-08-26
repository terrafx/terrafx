// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The root factory interface for all DWrite objects.</summary>
    [Guid("0439FC60-CA44-4994-8DEE-3A9AF7B732EC")]
    unsafe public /* blittable */ struct IDWriteFactory2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Get the system-appropriate font fallback mapping list.</summary>
        /// <param name="fontFallback">The system fallback list.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSystemFontFallback(
            [In] IDWriteFactory2* This,
            [Out] IDWriteFontFallback** fontFallback
        );

        /// <summary>Create a custom font fallback builder.</summary>
        /// <param name="fontFallbackBuilder">Empty font fallback builder.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFallbackBuilder(
            [In] IDWriteFactory2* This,
            [Out] IDWriteFontFallbackBuilder** fontFallbackBuilder
        );

        /// <summary>Translates a glyph run to a sequence of color glyph runs, which can be rendered to produce a color representation of the original "base" run.</summary>
        /// <param name="baselineOriginX">Horizontal origin of the base glyph run in pre-transform coordinates.</param>
        /// <param name="baselineOriginY">Vertical origin of the base glyph run in pre-transform coordinates.</param>
        /// <param name="glyphRun">Pointer to the original "base" glyph run.</param>
        /// <param name="glyphRunDescription">Optional glyph run description.</param>
        /// <param name="measuringMode">Measuring mode, needed to compute the origins of each glyph.</param>
        /// <param name="worldToDeviceTransform">Matrix converting from the client's coordinate space to device coordinates (pixels), i.e., the world transform multiplied by any DPI scaling.</param>
        /// <param name="colorPaletteIndex">Zero-based index of the color palette to use. Valid indices are less than the number of palettes in the font, as returned by IDWriteFontFace2::GetColorPaletteCount.</param>
        /// <param name="colorLayers">If the function succeeds, receives a pointer to an enumerator object that can be used to obtain the color glyph runs. If the base run has no color glyphs, then the output pointer is NULL and the method returns DWRITE_E_NOCOLOR.</param>
        /// <returns> Returns DWRITE_E_NOCOLOR if the font has no color information, the base glyph run does not contain any color glyphs, or the specified color palette index is out of range. In this case, the client should render the base glyph run. Otherwise, returns a standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int TranslateColorGlyphRun(
            [In] IDWriteFactory2* This,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] /* readonly */ DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, Optional] /* readonly */ DWRITE_MATRIX* worldToDeviceTransform,
            [In, ComAliasName("UINT32")] uint colorPaletteIndex,
            [Out] IDWriteColorGlyphRunEnumerator** colorLayers
        );

        /// <summary>Creates a rendering parameters object with the specified properties.</summary>
        /// <param name="gamma">The gamma value used for gamma correction, which must be greater than zero and cannot exceed 256.</param>
        /// <param name="enhancedContrast">The amount of contrast enhancement, zero or greater.</param>
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
            [In] IDWriteFactory2* This,
            [In, ComAliasName("FLOAT")] float gamma,
            [In, ComAliasName("FLOAT")] float enhancedContrast,
            [In, ComAliasName("FLOAT")] float grayscaleEnhancedContrast,
            [In, ComAliasName("FLOAT")] float clearTypeLevel,
            [In] DWRITE_PIXEL_GEOMETRY pixelGeometry,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [Out] IDWriteRenderingParams2** renderingParams
        );

        /// <summary>Creates a glyph run analysis object, which encapsulates information used to render a glyph run.</summary>
        /// <param name="glyphRun">Structure specifying the properties of the glyph run.</param>
        /// <param name="transform">Optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the emSize and pixelsPerDip.</param>
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
            [In] IDWriteFactory2* This,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform,
            [In] DWRITE_RENDERING_MODE renderingMode,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] DWRITE_GRID_FIT_MODE gridFitMode,
            [In] DWRITE_TEXT_ANTIALIAS_MODE antialiasMode,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [Out] IDWriteGlyphRunAnalysis** glyphRunAnalysis
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFactory1.Vtbl BaseVtbl;

            public IntPtr GetSystemFontFallback;

            public IntPtr CreateFontFallbackBuilder;

            public IntPtr TranslateColorGlyphRun;

            public IntPtr CreateCustomRenderingParams;

            public IntPtr CreateGlyphRunAnalysis;
            #endregion
        }
        #endregion
    }
}
