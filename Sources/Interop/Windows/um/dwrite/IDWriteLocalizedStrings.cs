// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    // <summary>Represents a collection of strings indexed by locale name.</summary>
    [Guid("08256209-099A-4B34-B86D-C22B110E7771")]
    public /* blittable */ unsafe struct IDWriteLocalizedStrings
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets the number of language/string pairs.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetCount(
            [In] IDWriteLocalizedStrings* This
        );

        /// <summary>Gets the index of the item with the specified locale name.</summary>
        /// <param name="localeName">Locale name to look for.</param>
        /// <param name="index">Receives the zero-based index of the locale name/string pair.</param>
        /// <param name="exists">Receives TRUE if the locale name exists or FALSE if not.</param>
        /// <returns>Standard HRESULT error code. If the specified locale name does not exist, the return value is S_OK, but *index is UINT_MAX and *exists is FALSE.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FindLocaleName(
            [In] IDWriteLocalizedStrings* This,
            [In, ComAliasName("WCHAR[]")] char* localeName,
            [Out, ComAliasName("UINT32")] uint* index,
            [Out, ComAliasName("BOOL")] int* exists
        );

        /// <summary>Gets the length in characters (not including the null terminator) of the locale name with the specified index.</summary>
        /// <param name="index">Zero-based index of the locale name.</param>
        /// <param name="length">Receives the length in characters, not including the null terminator.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocaleNameLength(
            [In] IDWriteLocalizedStrings* This,
            [In, ComAliasName("UINT32")] uint index,
            [Out, ComAliasName("UINT32")] uint* length
        );

        /// <summary>Copies the locale name with the specified index to the specified array.</summary>
        /// <param name="index">Zero-based index of the locale name.</param>
        /// <param name="localeName">Character array that receives the locale name.</param>
        /// <param name="size">Size of the array in characters. The size must include space for the terminating null character.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLocaleName(
            [In] IDWriteLocalizedStrings* This,
            [In, ComAliasName("UINT32")] uint index,
            [Out, ComAliasName("WCHAR[]")] char* localeName,
            [In, ComAliasName("UINT32")] uint size
        );

        /// <summary>Gets the length in characters (not including the null terminator) of the string with the specified index.</summary>
        /// <param name="index">Zero-based index of the string.</param>
        /// <param name="length">Receives the length in characters, not including the null terminator.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetStringLength(
            [In] IDWriteLocalizedStrings* This,
            [In, ComAliasName("UINT32")] uint index,
            [Out, ComAliasName("UINT32")] uint* length
        );

        /// <summary>Copies the string with the specified index to the specified array.</summary>
        /// <param name="index">Zero-based index of the string.</param>
        /// <param name="stringBuffer">Character array that receives the string.</param>
        /// <param name="size">Size of the array in characters. The size must include space for the terminating null character.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetString(
            [In] IDWriteLocalizedStrings* This,
            [In, ComAliasName("UINT32")] uint index,
            [Out, ComAliasName("WCHAR[]")] char* stringBuffer,
            [In, ComAliasName("UINT32")] uint size
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr GetCount;

            public IntPtr FindLocaleName;

            public IntPtr GetLocaleNameLength;

            public IntPtr GetLocaleName;

            public IntPtr GetStringLength;

            public IntPtr GetString;
            #endregion
        }
        #endregion
    }
}
