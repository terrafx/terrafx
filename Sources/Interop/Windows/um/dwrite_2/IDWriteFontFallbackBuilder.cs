// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Builder used to create a font fallback definition by appending a series of fallback mappings, followed by a creation call.</summary>
    /// <remarks> This object may not be thread-safe.</remarks>
    [Guid("FD882D06-8ABA-4FB8-B849-8BE8B73E14DE")]
    public /* blittable */ unsafe struct IDWriteFontFallbackBuilder
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteFontFallbackBuilder* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteFontFallbackBuilder* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteFontFallbackBuilder* This
        );
        #endregion

        #region Delegates
        /// <summary>Appends a single mapping to the list. Call this once for each additional mapping.</summary>
        /// <param name="ranges">Unicode ranges that apply to this mapping.</param>
        /// <param name="rangesCount">Number of Unicode ranges.</param>
        /// <param name="localeName">Locale of the context (e.g. document locale).</param>
        /// <param name="baseFamilyName">Base family name to match against, if applicable.</param>
        /// <param name="fontCollection">Explicit font collection for this mapping (optional).</param>
        /// <param name="targetFamilyNames">List of target family name strings.</param>
        /// <param name="targetFamilyNamesCount">Number of target family names.</param>
        /// <param name="scale">Scale factor to multiply the result target font by.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddMapping(
            [In] IDWriteFontFallbackBuilder* This,
            [In, ComAliasName("DWRITE_UNICODE_RANGE[]")] DWRITE_UNICODE_RANGE* ranges,
            [In, ComAliasName("UINT32")] uint rangesCount,
            [In, ComAliasName("WCHAR[]")] char** targetFamilyNames,
            [In, ComAliasName("UINT32")] uint targetFamilyNamesCount,
            [In] IDWriteFontCollection* fontCollection = null,
            [In, ComAliasName("WCHAR[]")] char* localeName = null,
            [In, ComAliasName("WCHAR[]")] char* baseFamilyName = null,
            [In, ComAliasName("FLOAT")] float scale = 1.0f
        );

        /// <summary>Appends all the mappings from an existing font fallback object.</summary>
        /// <param name="fontFallback">Font fallback to read mappings from.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddMappings(
            [In] IDWriteFontFallbackBuilder* This,
            [In] IDWriteFontFallback* fontFallback
        );

        /// <summary>Creates the finalized fallback object from the mappings added.</summary>
        /// <param name="fontFallback">Created fallback list.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFallback(
            [In] IDWriteFontFallbackBuilder* This,
            [Out] IDWriteFontFallback** fontFallback
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

            #region Fields
            public IntPtr AddMapping;

            public IntPtr AddMappings;

            public IntPtr CreateFontFallback;
            #endregion
        }
        #endregion
    }
}
