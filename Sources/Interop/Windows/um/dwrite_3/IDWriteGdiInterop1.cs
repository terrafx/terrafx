// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The GDI interop interface provides interoperability with GDI.</summary>
    [Guid("4556BE70-3ABD-4F70-90BE-421780A6F515")]
    unsafe public /* blittable */ struct IDWriteGdiInterop1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a font object that matches the properties specified by the LOGFONT public structure.</summary>
        /// <param name="logFont">Structure containing a GDI-compatible font description.</param>
        /// <param name="fontCollection">The font collection to search. If NULL, the local system font collection is used.</param>
        /// <param name="font">Receives a newly created font object if successful, or NULL in case of error.</param>
        /// <returns> Standard HRESULT error code.</returns>
        /// <remarks> The only fields that matter include: lfFaceName, lfCharSet, lfWeight, lfItalic. Font size and rendering mode are a rendering time property, not a font property, and text decorations like underline are drawn separately from the text. If no font matches the given weight, slope, and character set, the best match within the given GDI family name will be returned. DWRITE_E_NOFONT is returned if there is no matching font name using either the GDI family name (e.g. Arial) or the full font name (e.g. Arial Bold Italic).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFromLOGFONT(
            [In] IDWriteGdiInterop1* This,
            [In] /* readonly */ LOGFONT* logFont,
            [In, Optional] IDWriteFontCollection* fontCollection,
            [Out] IDWriteFont** font
        );

        /// <summary>Reads the font signature from the given font.</summary>
        /// <param name="font">Font to read font signature from.</param>
        /// <param name="fontSignature">Font signature from the OS/2 table, ulUnicodeRange and ulCodePageRange.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontSignature(
            [In] IDWriteGdiInterop1* This,
            [In] IDWriteFont* font,
            [Out] FONTSIGNATURE* fontSignature
        );

        /// <summary>Reads the font signature from the given font.</summary>
        /// <param name="fontFace">Font to read font signature from.</param>
        /// <param name="fontSignature">Font signature from the OS/2 table, ulUnicodeRange and ulCodePageRange.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontSignature1(
            [In] IDWriteGdiInterop1* This,
            [In] IDWriteFontFace* fontFace,
            [Out] FONTSIGNATURE* fontSignature
        );

        /// <summary>Get a list of matching fonts based on the LOGFONT values. Only fonts of that family name will be returned.</summary>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMatchingFontsByLOGFONT(
            [In] IDWriteGdiInterop1* This,
            [In] /* readonly */ LOGFONT* logFont,
            [In] IDWriteFontSet* fontSet,
            [Out] IDWriteFontSet** filteredSet
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteGdiInterop.Vtbl BaseVtbl;

            public IntPtr CreateFontFromLOGFONT;

            public IntPtr GetFontSignature;

            public IntPtr GetFontSignature1;

            public IntPtr GetMatchingFontsByLOGFONT;
            #endregion
        }
        #endregion
    }
}
