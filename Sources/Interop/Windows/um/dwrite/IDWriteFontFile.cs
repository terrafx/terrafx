// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface that represents a reference to a font file.</summary>
    [Guid("739D886A-CEF5-47DC-8769-1A8B41BEBBB0")]
    unsafe public /* blittable */ struct IDWriteFontFile
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>This method obtains the pointer to the reference key of a font file. The pointer is only valid until the object that refers to it is released.</summary>
        /// <param name="fontFileReferenceKey">Pointer to the font file reference key. IMPORTANT: The pointer value is valid until the font file reference object it is obtained from is released.</param>
        /// <param name="fontFileReferenceKeySize">Size of font file reference key in bytes.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetReferenceKey(
            [In] IDWriteFontFile* This,
            [Out] /* readonly */ void** fontFileReferenceKey,
            [Out, ComAliasName("UINT32")] uint* fontFileReferenceKeySize
        );

        /// <summary>Obtains the file loader associated with a font file object.</summary>
        /// <param name="fontFileLoader">The font file loader associated with the font file object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLoader(
            [In] IDWriteFontFile* This,
            [Out] IDWriteFontFileLoader** fontFileLoader
        );

        /// <summary>Analyzes a file and returns whether it represents a font, and whether the font type is supported by the font system.</summary>
        /// <param name="isSupportedFontType">TRUE if the font type is supported by the font system, FALSE otherwise.</param>
        /// <param name="fontFileType">The type of the font file. Note that even if isSupportedFontType is FALSE, the fontFileType value may be different from DWRITE_FONT_FILE_TYPE_UNKNOWN.</param>
        /// <param name="fontFaceType">The type of the font face that can be conpublic /* blittable */ structed from the font file. Note that even if isSupportedFontType is FALSE, the fontFaceType value may be different from DWRITE_FONT_FACE_TYPE_UNKNOWN.</param>
        /// <param name="numberOfFaces">Number of font faces contained in the font file.</param>
        /// <returns>Standard HRESULT error code if there was a processing error during analysis.</returns>
        /// <remarks>IMPORTANT: certain font file types are recognized, but not supported by the font system. For example, the font system will recognize a file as a Type 1 font file, but will not be able to construct a font face object from it. In such situations, Analyze will set isSupportedFontType output parameter to FALSE.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Analyze(
            [In] IDWriteFontFile* This,
            [Out, ComAliasName("BOOL")] int* isSupportedFontType,
            [Out] DWRITE_FONT_FILE_TYPE* fontFileType,
            [Out, Optional] DWRITE_FONT_FACE_TYPE* fontFaceType,
            [Out, ComAliasName("UINT32")] uint* numberOfFaces
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr GetReferenceKey;

            public IntPtr GetLoader;

            public IntPtr Analyze;
            #endregion
        }
        #endregion
    }
}
