// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>A built-in implementation of IDWriteFontFileLoader interface that operates on local font files and exposes local font file information from the font file reference key. Font file references created using CreateFontFileReference use this font file loader.</summary>
    [Guid("B2D9F3EC-C9FE-4A11-A2EC-D86208F7C0A2")]
    unsafe public /* blittable */ struct IDWriteLocalFontFileLoader
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Obtains the length of the absolute file path from the font file reference key.</summary>
        /// <param name="fontFileReferenceKey">Font file reference key that uniquely identifies the local font file within the scope of the font loader being used.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <param name="filePathLength">Length of the file path string not including the terminated NULL character.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFilePathLengthFromKey(
            [In] IDWriteLocalFontFileLoader* This,
            [In] /* readonly */ void* fontFileReferenceKey,
            [In] UINT32 fontFileReferenceKeySize,
            [Out] UINT32* filePathLength
        );

        /// <summary>Obtains the absolute font file path from the font file reference key.</summary>
        /// <param name="fontFileReferenceKey">Font file reference key that uniquely identifies the local font file within the scope of the font loader being used.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <param name="filePath">Character array that receives the local file path.</param>
        /// <param name="filePathSize">Size of the filePath array in character count including the terminated NULL character.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFilePathFromKey(
            [In] IDWriteLocalFontFileLoader* This,
            [In] /* readonly */ void* fontFileReferenceKey,
            [In] UINT32 fontFileReferenceKeySize,
            [Out] WCHAR* filePath,
            [In] UINT32 filePathSize
        );

        /// <summary>Obtains the last write time of the file from the font file reference key.</summary>
        /// <param name="fontFileReferenceKey">Font file reference key that uniquely identifies the local font file within the scope of the font loader being used.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <param name="lastWriteTime">Last modified time of the font file.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetLastWriteTimeFromKey(
            [In] IDWriteLocalFontFileLoader* This,
            [In] /* readonly */ void* fontFileReferenceKey,
            [In] UINT32 fontFileReferenceKeySize,
            [Out] FILETIME* lastWriteTime
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontFileLoader.Vtbl BaseVtbl;

            public GetFilePathLengthFromKey GetFilePathLengthFromKey;

            public GetFilePathFromKey GetFilePathFromKey;

            public GetLastWriteTimeFromKey GetLastWriteTimeFromKey;
            #endregion
        }
        #endregion
    }
}
