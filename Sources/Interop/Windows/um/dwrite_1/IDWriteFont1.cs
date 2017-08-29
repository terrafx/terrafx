// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteFont interface represents a physical font in a font collection.</summary>
    [Guid("ACD16696-8C14-4F5D-877E-FE3FC1D32738")]
    public /* blittable */ unsafe struct IDWriteFont1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteFont1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteFont1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteFont1* This
        );
        #endregion

        #region IDWriteFont Delegates
        /// <summary>Gets the font family to which the specified font belongs.</summary>
        /// <param name="fontFamily">Receives a pointer to the font family object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFamily(
            [In] IDWriteFont1* This,
            [Out] IDWriteFontFamily** fontFamily
        );

        /// <summary>Gets the weight of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_WEIGHT GetWeight(
            [In] IDWriteFont1* This
        );

        /// <summary>Gets the stretch (aka. width) of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STRETCH GetStretch(
            [In] IDWriteFont1* This
        );

        /// <summary>Gets the style (aka. slope) of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STYLE GetStyle(
            [In] IDWriteFont1* This
        );

        /// <summary>Returns TRUE if the font is a symbol font or FALSE if not.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsSymbolFont(
            [In] IDWriteFont1* This
        );

        /// <summary>Gets a localized strings collection containing the face names for the font (e.g., Regular or Bold), indexed by locale name.</summary>
        /// <param name="names">Receives a pointer to the newly created localized strings object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFaceNames(
            [In] IDWriteFont1* This,
            [Out] IDWriteLocalizedStrings** names
        );

        /// <summary>Gets a localized strings collection containing the specified informational strings, indexed by locale name.</summary>
        /// <param name="informationalStringID">Identifies the string to get.</param>
        /// <param name="informationalStrings">Receives a pointer to the newly created localized strings object.</param>
        /// <param name="exists">Receives the value TRUE if the font contains the specified string ID or FALSE if not.</param>
        /// <returns>Standard HRESULT error code. If the font does not contain the specified string, the return value is S_OK but informationalStrings receives a NULL pointer and exists receives the value FALSE.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetInformationalStrings(
            [In] IDWriteFont1* This,
            [In] DWRITE_INFORMATIONAL_STRING_ID informationalStringID,
            [Out] IDWriteLocalizedStrings** informationalStrings,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Gets a value that indicates what simulation are applied to the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_SIMULATIONS GetSimulations(
            [In] IDWriteFont1* This
        );

        /// <summary>Gets the metrics for the font.</summary>
        /// <param name="fontMetrics">Receives the font metrics.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetMetrics(
            [In] IDWriteFont1* This,
            [Out] DWRITE_FONT_METRICS* fontMetrics
        );

        /// <summary>Determines whether the font supports the specified character.</summary>
        /// <param name="unicodeValue">Unicode (UCS-4) character value.</param>
        /// <param name="exists">Receives the value TRUE if the font supports the specified character or FALSE if not.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int HasCharacter(
            [In] IDWriteFont1* This,
            [In, ComAliasName("UINT32")] uint unicodeValue,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Creates a font face object for the font.</summary>
        /// <param name="fontFace">Receives a pointer to the newly created font face object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFace(
            [In] IDWriteFont1* This,
            [Out] IDWriteFontFace** fontFace
        );
        #endregion

        #region Delegates
        /// <summary>Gets common metrics for the font in design units. These metrics are applicable to all the glyphs within a font, and are used by applications for layout calculations.</summary>
        /// <param name="fontMetrics">Metrics public structure to fill in.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetMetrics1(
            [In] IDWriteFont1* This,
            [Out] DWRITE_FONT_METRICS1* fontMetrics
        );

        /// <summary>Gets the PANOSE values from the font, used for font selection and matching.</summary>
        /// <param name="panose">PANOSE public structure to fill in.</param>
        /// <remarks> The function does not simulate these, such as substituting a weight or proportion inferred on other values. If the font does not specify them, they are all set to 'any' (0).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetPanose(
            [In] IDWriteFont1* This,
            [Out] DWRITE_PANOSE* panose
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
            [In] IDWriteFont1* This,
            [In, ComAliasName("UINT32")] uint maxRangeCount,
            [Out, Optional, ComAliasName("DWRITE_UNICODE_RANGE[]")] DWRITE_UNICODE_RANGE* unicodeRanges,
            [Out, ComAliasName("UINT32")] uint* actualRangeCount
        );

        /// <summary>Returns true if the font is monospaced, meaning its characters are the same fixed-pitch width (non-proportional).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsMonospacedFont(
            [In] IDWriteFont1* This
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

            #region IDWriteFont Fields
            public IntPtr GetFontFamily;

            public IntPtr GetWeight;

            public IntPtr GetStretch;

            public IntPtr GetStyle;

            public IntPtr IsSymbolFont;

            public IntPtr GetFaceNames;

            public IntPtr GetInformationalStrings;

            public IntPtr GetSimulations;

            public IntPtr GetMetrics;

            public IntPtr HasCharacter;

            public IntPtr CreateFontFace;
            #endregion

            #region Fields
            public IntPtr GetMetrics1;

            public IntPtr GetPanose;

            public IntPtr GetUnicodeRanges;

            public IntPtr IsMonospacedFont;
            #endregion
        }
        #endregion
    }
}
