// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3FF7715F-3CDC-4DC6-9B72-EC5621DCCAFD")]
    unsafe public /* blittable */ struct IDWriteFontSetBuilder1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Adds references to all the fonts in the specified font file. The method parses the font file to determine the fonts and their properties.</summary>
        /// <param name="fontFile">Font file reference object to add to the set.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddFontFile(
            [In] IDWriteFontSetBuilder1* This,
            [In] IDWriteFontFile* fontFile
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteFontSetBuilder.Vtbl BaseVtbl;

            public IntPtr AddFontFile;
            #endregion
        }
        #endregion
    }
}
