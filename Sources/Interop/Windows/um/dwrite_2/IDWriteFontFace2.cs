// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface that represents an absolute reference to a font face. It contains font face type, appropriate file references and face identification data. Various font data such as metrics, names and glyph outlines is obtained from IDWriteFontFace.</summary>
    [Guid("D8B768FF-64BC-4E66-982B-EC8E87F693F7")]
    unsafe public /* blittable */ struct IDWriteFontFace2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Returns TRUE if the font contains tables that can provide color information (including COLR, CPAL, SVG, CBDT, sbix  tables), or FALSE if not. Note that TRUE is returned even in the case when the font tables contain only grayscale images.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsColorFont(
            [In] IDWriteFontFace2* This
        );

        /// <summary>Returns the number of color palettes defined by the font. The return value is zero if the font has no color information. Color fonts must have at least one palette, with palette index zero being the default.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetColorPaletteCount(
            [In] IDWriteFontFace2* This
        );

        /// <summary>Returns the number of entries in each color palette. All color palettes in a font have the same number of palette entries. The return value is zero if the font has no color information.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetPaletteEntryCount(
            [In] IDWriteFontFace2* This
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
            [In] IDWriteFontFace2* This,
            [In, ComAliasName("UINT32")] uint colorPaletteIndex,
            [In, ComAliasName("UINT32")] uint firstEntryIndex,
            [In, ComAliasName("UINT32")] uint entryCount,
            [Out, ComAliasName("DWRITE_COLOR_F")] DXGI_RGBA* paletteEntries
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
        public /* static */ delegate int GetRecommendedRenderingMode(
            [In] IDWriteFontFace2* This,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("FLOAT")] float dpiX,
            [In, ComAliasName("FLOAT")] float dpiY,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform,
            [In, ComAliasName("BOOL")] int isSideways,
            [In] DWRITE_OUTLINE_THRESHOLD outlineThreshold,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In, Optional] IDWriteRenderingParams* renderingParams,
            [Out] DWRITE_RENDERING_MODE* renderingMode,
            [Out] DWRITE_GRID_FIT_MODE* gridFitMode
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontFace1.Vtbl BaseVtbl;

            public IntPtr IsColorFont;

            public IntPtr GetColorPaletteCount;

            public IntPtr GetPaletteEntryCount;

            public IntPtr GetPaletteEntries;

            public IntPtr GetRecommendedRenderingMode;
            #endregion
        }
        #endregion
    }
}
