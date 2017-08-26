// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteFont interface represents a font in a font collection.</summary>
    [Guid("29748ED6-8C9C-4A6A-BE0B-D912E8538944")]
    public /* blittable */ unsafe struct IDWriteFont3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Creates a font face object for the font.</summary>
        /// <param name="fontFace">Receives a pointer to the newly created font face object.</param>
        /// <returns> Standard HRESULT error code. The function returns DWRITE_E_REMOTEFONT if it could not conpublic /* blittable */ struct a remote font.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFontFace(
            [In] IDWriteFont3* This,
            [Out] IDWriteFontFace3** fontFace
        );

        /// <summary>Compares two instances of a font references for equality.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _Equals(
            [In] IDWriteFont3* This,
            [In] IDWriteFont* font
        );

        /// <summary>Return a font face reference identifying this font.</summary>
        /// <param name="fontFaceReference">A uniquely identifying reference to a font face.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFaceReference(
            [In] IDWriteFont3* This,
            [Out] IDWriteFontFaceReference** fontFaceReference
        );

        /// <summary>Determines whether the font supports the specified character.</summary>
        /// <param name="unicodeValue">Unicode (UCS-4) character value.</param>
        /// <returns> Returns TRUE if the font has the specified character, FALSE if not.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int HasCharacter(
            [In] IDWriteFont3* This,
            [In, ComAliasName("UINT32")] uint unicodeValue
        );

        /// <summary>Gets the current locality of the font.</summary>
        /// <remarks> The locality enumeration. For fully local files, the result will always be DWRITE_LOCALITY_LOCAL. A downloadable file may be any of the states, and this function may change between calls.</remarks>
        /// <returns> The locality enumeration.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_LOCALITY GetLocality(
            [In] IDWriteFont3* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFont2.Vtbl BaseVtbl;

            public IntPtr CreateFontFace;

            public IntPtr _Equals;

            public IntPtr GetFontFaceReference;

            public IntPtr HasCharacter;

            public IntPtr GetLocality;
            #endregion
        }
        #endregion
    }
}
