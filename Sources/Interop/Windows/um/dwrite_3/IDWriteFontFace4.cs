// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface that represents an absolute reference to a font face. It contains font face type, appropriate file references and face identification data. Various font data such as metrics, names and glyph outlines is obtained from IDWriteFontFace.</summary>
    [Guid("27F2A904-4EB8-441D-9678-0563F53E3E2F")]
    unsafe public /* blittable */ struct IDWriteFontFace4
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets all the glyph image formats supported by the entire font (SVG, PNG, JPEG, ...).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_GLYPH_IMAGE_FORMATS GetGlyphImageFormats(
            [In] IDWriteFontFace4* This
        );

        /// <summary>Gets the available image formats of a specific glyph and ppem. Glyphs often have at least TrueType or CFF outlines, but they may also have SVG outlines, or they may have only bitmaps with no TrueType/CFF outlines. Some image formats, notably the PNG/JPEG ones, are size specific and will return no match when there isn't an entry in that size range.</summary>
        /// <remarks> Glyph ids beyond the glyph count return DWRITE_GLYPH_IMAGE_FORMATS_NONE.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGlyphImageFormats1(
            [In] IDWriteFontFace4* This,
            [In, ComAliasName("UINT16")] ushort glyphId,
            [In, ComAliasName("UINT32")] uint pixelsPerEmFirst,
            [In, ComAliasName("UINT32")] uint pixelsPerEmLast,
            [Out] DWRITE_GLYPH_IMAGE_FORMATS* glyphImageFormats
        );

        /// <summary>Gets a pointer to the glyph data based on the desired image format.</summary>
        /// <remarks> The glyphDataContext must be released via ReleaseGlyphImageData when done if the data is not empty, similar to IDWriteFontFileStream::ReadFileFragment and IDWriteFontFileStream::ReleaseFileFragment. The data pointer is valid so long as the IDWriteFontFace exists and ReleaseGlyphImageData has not been called.</remarks>
        /// <remarks> The DWRITE_GLYPH_IMAGE_DATA::uniqueDataId is valuable for caching purposes so that if the same resource is returned more than once, an existing resource can be quickly retrieved rather than needing to reparse or decompress the data.</remarks>
        /// <remarks> The function only returns SVG or raster data - requesting TrueType/CFF/COLR data returns DWRITE_E_INVALIDARG. Those must be drawn via DrawGlyphRun or queried using GetGlyphOutline instead. Exactly one format may be requested or else the function returns DWRITE_E_INVALIDARG. If the glyph does not have that format, the call is not an error, but the function returns empty data.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGlyphImageData(
            [In] IDWriteFontFace4* This,
            [In, ComAliasName("UINT16")] ushort glyphId,
            [In, ComAliasName("UINT32")] uint pixelsPerEm,
            [In] DWRITE_GLYPH_IMAGE_FORMATS glyphImageFormat,
            [Out] DWRITE_GLYPH_IMAGE_DATA* glyphData,
            [Out] void** glyphDataContext
        );

        /// <summary>Releases the table data obtained earlier from ReadGlyphData.</summary>
        /// <param name="glyphDataContext">Opaque context from ReadGlyphData.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseGlyphImageData(
            [In] IDWriteFontFace4* This,
            [In] void* glyphDataContext
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontFace3.Vtbl BaseVtbl;

            public IntPtr GetGlyphImageFormats;

            public IntPtr GetGlyphImageFormats1;

            public IntPtr GetGlyphImageData;

            public IntPtr ReleaseGlyphImageData;
            #endregion
        }
        #endregion
    }
}
