// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface that represents an absolute reference to a font face. It contains font face type, appropriate file references and face identification data. Various font data such as metrics, names and glyph outlines is obtained from IDWriteFontFace.</summary>
    [Guid("D37D7598-09BE-4222-A236-2081341CC1F2")]
    unsafe public /* blittable */ struct IDWriteFontFace3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
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
        public /* static */ delegate int GetRecommendedRenderingMode(
            [In] IDWriteFontFace3* This,
            [In, ComAliasName("FLOAT")] float fontEmSize,
            [In, ComAliasName("FLOAT")] float dpiX,
            [In, ComAliasName("FLOAT")] float dpiY,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform,
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
            [In, ComAliasName("WCHAR")] /* readonly */ char* characters,
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
            [In, ComAliasName("UINT16")] /* readonly */ ushort* glyphIndices,
            [In, ComAliasName("UINT32")] uint glyphCount,
            [In, ComAliasName("BOOL")] int enqueueIfNotLocal,
            [Out, ComAliasName("BOOL")] int* isLocal
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontFace2.Vtbl BaseVtbl;

            public IntPtr GetFontFaceReference;

            public IntPtr GetPanose;

            public IntPtr GetWeight;

            public IntPtr GetStretch;

            public IntPtr GetStyle;

            public IntPtr GetFamilyNames;

            public IntPtr GetFaceNames;

            public IntPtr GetInformationalStrings;

            public IntPtr HasCharacter;

            public IntPtr GetRecommendedRenderingMode;

            public IntPtr IsCharacterLocal;

            public IntPtr IsGlyphLocal;

            public IntPtr AreCharactersLocal;

            public IntPtr AreGlyphsLocal;
            #endregion
        }
        #endregion
    }
}
