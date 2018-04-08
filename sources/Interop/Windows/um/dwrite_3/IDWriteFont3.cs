// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteFont interface represents a font in a font collection.</summary>
    [Guid("29748ED6-8C9C-4A6A-BE0B-D912E8538944")]
    public /* unmanaged */ unsafe struct IDWriteFont3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDWriteFont3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDWriteFont3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDWriteFont3* This
        );
        #endregion

        #region IDWriteFont Delegates
        /// <summary>Gets the font family to which the specified font belongs.</summary>
        /// <param name="fontFamily">Receives a pointer to the font family object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFontFamily(
            [In] IDWriteFont3* This,
            [Out] IDWriteFontFamily** fontFamily
        );

        /// <summary>Gets the weight of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_WEIGHT _GetWeight(
            [In] IDWriteFont3* This
        );

        /// <summary>Gets the stretch (aka. width) of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STRETCH _GetStretch(
            [In] IDWriteFont3* This
        );

        /// <summary>Gets the style (aka. slope) of the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_STYLE _GetStyle(
            [In] IDWriteFont3* This
        );

        /// <summary>Returns TRUE if the font is a symbol font or FALSE if not.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsSymbolFont(
            [In] IDWriteFont3* This
        );

        /// <summary>Gets a localized strings collection containing the face names for the font (e.g., Regular or Bold), indexed by locale name.</summary>
        /// <param name="names">Receives a pointer to the newly created localized strings object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFaceNames(
            [In] IDWriteFont3* This,
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
        public /* static */ delegate int _GetInformationalStrings(
            [In] IDWriteFont3* This,
            [In] DWRITE_INFORMATIONAL_STRING_ID informationalStringID,
            [Out] IDWriteLocalizedStrings** informationalStrings,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Gets a value that indicates what simulation are applied to the specified font.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_FONT_SIMULATIONS _GetSimulations(
            [In] IDWriteFont3* This
        );

        /// <summary>Gets the metrics for the font.</summary>
        /// <param name="fontMetrics">Receives the font metrics.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetMetrics(
            [In] IDWriteFont3* This,
            [Out] DWRITE_FONT_METRICS* fontMetrics
        );

        /// <summary>Determines whether the font supports the specified character.</summary>
        /// <param name="unicodeValue">Unicode (UCS-4) character value.</param>
        /// <param name="exists">Receives the value TRUE if the font supports the specified character or FALSE if not.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _HasCharacter(
            [In] IDWriteFont3* This,
            [In, ComAliasName("UINT32")] uint unicodeValue,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Creates a font face object for the font.</summary>
        /// <param name="fontFace">Receives a pointer to the newly created font face object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFontFace(
            [In] IDWriteFont3* This,
            [Out] IDWriteFontFace** fontFace
        );
        #endregion

        #region IDWriteFont1 Delegates
        /// <summary>Gets common metrics for the font in design units. These metrics are applicable to all the glyphs within a font, and are used by applications for layout calculations.</summary>
        /// <param name="fontMetrics">Metrics public structure to fill in.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetMetrics1(
            [In] IDWriteFont3* This,
            [Out] DWRITE_FONT_METRICS1* fontMetrics
        );

        /// <summary>Gets the PANOSE values from the font, used for font selection and matching.</summary>
        /// <param name="panose">PANOSE public structure to fill in.</param>
        /// <remarks> The function does not simulate these, such as substituting a weight or proportion inferred on other values. If the font does not specify them, they are all set to 'any' (0).</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetPanose(
            [In] IDWriteFont3* This,
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
        public /* static */ delegate int _GetUnicodeRanges(
            [In] IDWriteFont3* This,
            [In, ComAliasName("UINT32")] uint maxRangeCount,
            [Out, Optional, ComAliasName("DWRITE_UNICODE_RANGE[]")] DWRITE_UNICODE_RANGE* unicodeRanges,
            [Out, ComAliasName("UINT32")] uint* actualRangeCount
        );

        /// <summary>Returns true if the font is monospaced, meaning its characters are the same fixed-pitch width (non-proportional).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsMonospacedFont(
            [In] IDWriteFont3* This
        );
        #endregion

        #region IDWriteFont2 Delegates
        /// <summary>Returns TRUE if the font contains tables that can provide color information (including COLR, CPAL, SVG, CBDT, sbix  tables), or FALSE if not. Note that TRUE is returned even in the case when the font tables contain only grayscale images.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsColorFont(
            [In] IDWriteFont3* This
        );
        #endregion

        #region Delegates
        /// <summary>Creates a font face object for the font.</summary>
        /// <param name="fontFace">Receives a pointer to the newly created font face object.</param>
        /// <returns> Standard HRESULT error code. The function returns DWRITE_E_REMOTEFONT if it could not conpublic /* unmanaged */ struct a remote font.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFontFace1(
            [In] IDWriteFont3* This,
            [Out] IDWriteFontFace3** fontFace
        );

        /// <summary>Compares two instances of a font references for equality.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int __Equals(
            [In] IDWriteFont3* This,
            [In] IDWriteFont* font
        );

        /// <summary>Return a font face reference identifying this font.</summary>
        /// <param name="fontFaceReference">A uniquely identifying reference to a font face.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFontFaceReference(
            [In] IDWriteFont3* This,
            [Out] IDWriteFontFaceReference** fontFaceReference
        );

        /// <summary>Determines whether the font supports the specified character.</summary>
        /// <param name="unicodeValue">Unicode (UCS-4) character value.</param>
        /// <returns> Returns TRUE if the font has the specified character, FALSE if not.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _HasCharacter1(
            [In] IDWriteFont3* This,
            [In, ComAliasName("UINT32")] uint unicodeValue
        );

        /// <summary>Gets the current locality of the font.</summary>
        /// <remarks> The locality enumeration. For fully local files, the result will always be DWRITE_LOCALITY_LOCAL. A downloadable file may be any of the states, and this function may change between calls.</remarks>
        /// <returns> The locality enumeration.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_LOCALITY _GetLocality(
            [In] IDWriteFont3* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IDWriteFont Methods
        [return: ComAliasName("HRESULT")]
        public int GetFontFamily(
            [Out] IDWriteFontFamily** fontFamily
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetFontFamily>(lpVtbl->GetFontFamily)(
                    This,
                    fontFamily
                );
            }
        }

        public DWRITE_FONT_WEIGHT GetWeight()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetWeight>(lpVtbl->GetWeight)(
                    This
                );
            }
        }

        public DWRITE_FONT_STRETCH GetStretch()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetStretch>(lpVtbl->GetStretch)(
                    This
                );
            }
        }

        public DWRITE_FONT_STYLE GetStyle()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetStyle>(lpVtbl->GetStyle)(
                    This
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int IsSymbolFont()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_IsSymbolFont>(lpVtbl->IsSymbolFont)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFaceNames(
            [Out] IDWriteLocalizedStrings** names
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetFaceNames>(lpVtbl->GetFaceNames)(
                    This,
                    names
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetInformationalStrings(
            [In] DWRITE_INFORMATIONAL_STRING_ID informationalStringID,
            [Out] IDWriteLocalizedStrings** informationalStrings,
            [Out, ComAliasName("BOOL")] int* exists
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetInformationalStrings>(lpVtbl->GetInformationalStrings)(
                    This,
                    informationalStringID,
                    informationalStrings,
                    exists
                );
            }
        }

        public DWRITE_FONT_SIMULATIONS GetSimulations()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetSimulations>(lpVtbl->GetSimulations)(
                    This
                );
            }
        }

        public void GetMetrics(
            [Out] DWRITE_FONT_METRICS* fontMetrics
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                MarshalFunction<_GetMetrics>(lpVtbl->GetMetrics)(
                    This,
                    fontMetrics
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int HasCharacter(
            [In, ComAliasName("UINT32")] uint unicodeValue,
            [Out, ComAliasName("BOOL")] int* exists
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_HasCharacter>(lpVtbl->HasCharacter)(
                    This,
                    unicodeValue,
                    exists
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateFontFace(
            [Out] IDWriteFontFace** fontFace
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_CreateFontFace>(lpVtbl->CreateFontFace)(
                    This,
                    fontFace
                );
            }
        }
        #endregion

        #region IDWriteFont1 Methods
        public void GetMetrics1(
            [Out] DWRITE_FONT_METRICS1* fontMetrics
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                MarshalFunction<_GetMetrics1>(lpVtbl->GetMetrics1)(
                    This,
                    fontMetrics
                );
            }
        }

        public void GetPanose(
            [Out] DWRITE_PANOSE* panose
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                MarshalFunction<_GetPanose>(lpVtbl->GetPanose)(
                    This,
                    panose
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetUnicodeRanges(
            [In, ComAliasName("UINT32")] uint maxRangeCount,
            [Out, Optional, ComAliasName("DWRITE_UNICODE_RANGE[]")] DWRITE_UNICODE_RANGE* unicodeRanges,
            [Out, ComAliasName("UINT32")] uint* actualRangeCount
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetUnicodeRanges>(lpVtbl->GetUnicodeRanges)(
                    This,
                    maxRangeCount,
                    unicodeRanges,
                    actualRangeCount
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int IsMonospacedFont()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_IsMonospacedFont>(lpVtbl->IsMonospacedFont)(
                    This
                );
            }
        }
        #endregion

        #region IDWriteFont2 Methods
        [return: ComAliasName("BOOL")]
        public int IsColorFont()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_IsColorFont>(lpVtbl->IsColorFont)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CreateFontFace1(
            [Out] IDWriteFontFace3** fontFace
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_CreateFontFace1>(lpVtbl->CreateFontFace1)(
                    This,
                    fontFace
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int _Equals(
            [In] IDWriteFont* font
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<__Equals>(lpVtbl->_Equals)(
                    This,
                    font
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFontFaceReference(
            [Out] IDWriteFontFaceReference** fontFaceReference
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetFontFaceReference>(lpVtbl->GetFontFaceReference)(
                    This,
                    fontFaceReference
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int HasCharacter1(
            [In, ComAliasName("UINT32")] uint unicodeValue
        )
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_HasCharacter1>(lpVtbl->HasCharacter1)(
                    This,
                    unicodeValue
                );
            }
        }

        public DWRITE_LOCALITY GetLocality()
        {
            fixed (IDWriteFont3* This = &this)
            {
                return MarshalFunction<_GetLocality>(lpVtbl->GetLocality)(
                    This
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
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

            #region IDWriteFont1 Fields
            public IntPtr GetMetrics1;

            public IntPtr GetPanose;

            public IntPtr GetUnicodeRanges;

            public IntPtr IsMonospacedFont;
            #endregion

            #region IDWriteFont2 Fields
            public IntPtr IsColorFont;
            #endregion

            #region Fields
            public IntPtr CreateFontFace1;

            public IntPtr _Equals;

            public IntPtr GetFontFaceReference;

            public IntPtr HasCharacter1;

            public IntPtr GetLocality;
            #endregion
        }
        #endregion
    }
}

