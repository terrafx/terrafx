// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("4B0B5BD3-0797-4549-8AC5-FE915CC53856")]
    public /* blittable */ unsafe struct IDWriteFactory4
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Translates a glyph run to a sequence of color glyph runs, which can be rendered to produce a color representation of the original "base" run.</summary>
        /// <param name="baselineOrigin">Horizontal and vertical origin of the base glyph run in pre-transform coordinates.</param>
        /// <param name="glyphRun">Pointer to the original "base" glyph run.</param>
        /// <param name="glyphRunDescription">Optional glyph run description.</param>
        /// <param name="desiredGlyphImageFormats">Which data formats TranslateColorGlyphRun should split the runs into.</param>
        /// <param name="measuringMode">Measuring mode, needed to compute the origins of each glyph.</param>
        /// <param name="worldAndDpiTransform">Matrix converting from the client's coordinate space to device coordinates (pixels), i.e., the world transform multiplied by any DPI scaling.</param>
        /// <param name="colorPaletteIndex">Zero-based index of the color palette to use. Valid indices are less than the number of palettes in the font, as returned by IDWriteFontFace2::GetColorPaletteCount.</param>
        /// <param name="colorLayers">If the function succeeds, receives a pointer to an enumerator object that can be used to obtain the color glyph runs. If the base run has no color glyphs, then the output pointer is NULL and the method returns DWRITE_E_NOCOLOR.</param>
        /// <returns> Returns DWRITE_E_NOCOLOR if the font has no color information, the glyph run does not contain any color glyphs, or the specified color palette index is out of range. In this case, the client should render the original glyph run. Otherwise, returns a standard HRESULT error code.</returns>
        /// <remarks> The old IDWriteFactory2::TranslateColorGlyphRun is equivalent to passing DWRITE_GLYPH_IMAGE_FORMATS_TRUETYPE|CFF|COLR.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int TranslateColorGlyphRun(
            [In] IDWriteFactory4* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In, Optional] /* readonly */ DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,
            [In] DWRITE_GLYPH_IMAGE_FORMATS desiredGlyphImageFormats,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, Optional] /* readonly */ DWRITE_MATRIX* worldAndDpiTransform,
            [In, ComAliasName("UINT32")] uint colorPaletteIndex,
            [Out] IDWriteColorGlyphRunEnumerator1** colorLayers
        );

        /// <summary>Converts glyph run placements to glyph origins.</summary>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> The transform and DPI have no affect on the origin scaling. They are solely used to compute glyph advances when not supplied and align glyphs in pixel aligned measuring modes.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputeGlyphOrigins(
            [In] IDWriteFactory4* This,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [In, Optional] /* readonly */ DWRITE_MATRIX* worldAndDpiTransform,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* glyphOrigins
        );

        /// <summary>Converts glyph run placements to glyph origins. This overload is for natural metrics, which includes SVG, TrueType natural modes, and bitmap placement.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputeGlyphOrigins1(
            [In] IDWriteFactory4* This,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F baselineOrigin,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* glyphOrigins
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFactory3.Vtbl BaseVtbl;

            public IntPtr TranslateColorGlyphRun;

            public IntPtr ComputeGlyphOrigins;

            public IntPtr ComputeGlyphOrigins1;
            #endregion
        }
        #endregion
    }
}
