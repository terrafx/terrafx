// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.D2D1_COLOR_BITMAP_GLYPH_SNAP_OPTION;
using static TerraFX.Interop.D2D1_DRAW_TEXT_OPTIONS;
using static TerraFX.Interop.DWRITE_MEASURING_MODE;

namespace TerraFX.Interop
{
    [Guid("8C427831-3D90-4476-B647-C4FAE349E4DB")]
    unsafe public /* blittable */ struct ID2D1DeviceContext4
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates an SVG glyph style object.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateSvgGlyphStyle(
            [In] ID2D1DeviceContext4* This,
            [Out] ID2D1SvgGlyphStyle** svgGlyphStyle
        );

        /// <summary>Draws the text within the given layout rectangle. By default, this method performs baseline snapping and renders color versions of glyphs in color fonts.</summary>
        /// <param name="svgGlyphStyle">Object used to style SVG glyphs.</param>
        /// <param name="colorPaletteIndex">The index used to select a color palette within a color font.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawText(
            [In] ID2D1DeviceContext4* This,
            [In, ComAliasName("WCHAR")] /* readonly */ char* @string,
            [In, ComAliasName("UINT32")] uint stringLength,
            [In] IDWriteTextFormat* textFormat,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* layoutRect,
            [In, Optional] ID2D1Brush* defaultFillBrush,
            [In, Optional] ID2D1SvgGlyphStyle* svgGlyphStyle,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint colorPaletteIndex,
            [In] D2D1_DRAW_TEXT_OPTIONS options = D2D1_DRAW_TEXT_OPTIONS_ENABLE_COLOR_FONT,
            [In] DWRITE_MEASURING_MODE measuringMode = DWRITE_MEASURING_MODE_NATURAL
        );

        /// <summary>Draw a text layout object. If the layout is not subsequently changed, this can be more efficient than DrawText when drawing the same layout repeatedly.</summary>
        /// <param name="svgGlyphStyle">Object used to style SVG glyphs.</param>
        /// <param name="colorPaletteIndex">The index used to select a color palette within a color font.</param>
        /// <param name="options">The specified text options. If D2D1_DRAW_TEXT_OPTIONS_CLIP is used, the text is clipped to the layout bounds. These bounds are derived from the origin and the layout bounds of the corresponding IDWriteTextLayout object.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawTextLayout(
            [In] ID2D1DeviceContext4* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F origin,
            [In] IDWriteTextLayout* textLayout,
            [In, Optional] ID2D1Brush* defaultFillBrush,
            [In, Optional] ID2D1SvgGlyphStyle* svgGlyphStyle,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint colorPaletteIndex,
            [In] D2D1_DRAW_TEXT_OPTIONS options = D2D1_DRAW_TEXT_OPTIONS_ENABLE_COLOR_FONT
        );

        /// <summary>Draws a color glyph run using one (and only one) of the bitmap formats- DWRITE_GLYPH_IMAGE_FORMATS_PNG, DWRITE_GLYPH_IMAGE_FORMATS_JPEG, DWRITE_GLYPH_IMAGE_FORMATS_TIFF, or DWRITE_GLYPH_IMAGE_FORMATS_PREMULTIPLIED_B8G8R8A8.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawColorBitmapGlyphRun(
            [In] ID2D1DeviceContext4* This,
            [In] DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In] DWRITE_MEASURING_MODE measuringMode = DWRITE_MEASURING_MODE_NATURAL,
            [In] D2D1_COLOR_BITMAP_GLYPH_SNAP_OPTION bitmapSnapOption = D2D1_COLOR_BITMAP_GLYPH_SNAP_OPTION_DEFAULT
        );

        /// <summary>Draws a color glyph run that has the format of DWRITE_GLYPH_IMAGE_FORMATS_SVG.</summary>
        /// <param name="svgGlyphStyle">Object used to style SVG glyphs.</param>
        /// <param name="colorPaletteIndex">The index used to select a color palette within a color font. Note that this not the same as the paletteIndex in the DWRITE_COLOR_GLYPH_RUN struct, which is not relevant for SVG glyphs.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawSvgGlyphRun(
            [In] ID2D1DeviceContext4* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] ID2D1Brush* defaultFillBrush,
            [In, Optional] ID2D1SvgGlyphStyle* svgGlyphStyle,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint colorPaletteIndex,
            [In] DWRITE_MEASURING_MODE measuringMode = DWRITE_MEASURING_MODE_NATURAL
        );

        /// <summary>Retrieves an image of the color bitmap glyph from the color glyph cache. If the cache does not already contain the requested resource, it will be created. This method may be used to extend the lifetime of a glyph image even after it is evicted from the color glyph cache.</summary>
        /// <param name="fontEmSize">The specified font size affects the choice of which bitmap to use from the font. It also affects the output glyphTransform, causing it to properly scale the glyph.</param>
        /// <param name="glyphTransform">Output transform, which transforms from the glyph's space to the same output space as the worldTransform. This includes the input glyphOrigin, the glyph's offset from the glyphOrigin, and any other required transformations.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetColorBitmapGlyphImage(
            [In] ID2D1DeviceContext4* This,
            [In] DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F glyphOrigin,
            [In] IDWriteFontFace* fontFace,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("UINT16")] ushort glyphIndex,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] /* readonly */ D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float dpiX,
            [In, ComAliasName("FLOAT")] float dpiY,
            [Out, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* glyphTransform,
            [Out] ID2D1Image** glyphImage
        );

        /// <summary>Retrieves an image of the SVG glyph from the color glyph cache. If the cache does not already contain the requested resource, it will be created. This method may be used to extend the lifetime of a glyph image even after it is evicted from the color glyph cache.</summary>
        /// <param name="fontEmSize">The specified font size affects the output glyphTransform, causing it to properly scale the glyph.</param>
        /// <param name="svgGlyphStyle">Object used to style SVG glyphs.</param>
        /// <param name="colorPaletteIndex">The index used to select a color palette within a color font. Note that this not the same as the paletteIndex in the DWRITE_COLOR_GLYPH_RUN struct, which is not relevant for SVG glyphs.</param>
        /// <param name="glyphTransform">Output transform, which transforms from the glyph's space to the same output space as the worldTransform. This includes the input glyphOrigin, the glyph's offset from the glyphOrigin, and any other required transformations.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSvgGlyphImage(
            [In] ID2D1DeviceContext4* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F glyphOrigin,
            [In] IDWriteFontFace* fontFace,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("UINT16")] ushort glyphIndex,
            [In, ComAliasName("BOOL")] int isSideways,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] /* readonly */ D2D_MATRIX_3X2_F* worldTransform,
            [In, Optional] ID2D1Brush* defaultFillBrush,
            [In, Optional] ID2D1SvgGlyphStyle* svgGlyphStyle,
            [In, ComAliasName("UINT32")] uint colorPaletteIndex,
            [Out, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* glyphTransform,
            [Out] ID2D1CommandList** glyphImage
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1DeviceContext3.Vtbl BaseVtbl;

            public IntPtr CreateSvgGlyphStyle;

            public IntPtr DrawText;

            public IntPtr DrawTextLayout;

            public IntPtr DrawColorBitmapGlyphRun;

            public IntPtr DrawSvgGlyphRun;

            public IntPtr GetColorBitmapGlyphImage;

            public IntPtr GetSvgGlyphImage;
            #endregion
        }
        #endregion
    }
}
