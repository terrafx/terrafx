// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The font file enumerator interface encapsulates a collection of font files. The font system uses this interface to enumerate font files when building a font collection.</summary>
    [Guid("72755049-5FF7-435D-8348-4BE97CFA6C7C")]
    unsafe public /* blittable */ struct IDWriteFontFileEnumerator
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Advances to the next font file in the collection. When it is first created, the enumerator is positioned before the first element of the collection and the first call to MoveNext advances to the first file.</summary>
        /// <param name="hasCurrentFile">Receives the value TRUE if the enumerator advances to a file, or FALSE if the enumerator advanced past the last file in the collection.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MoveNext(
            [In] IDWriteFontFileEnumerator* This,
            [Out, ComAliasName("BOOL")] int* hasCurrentFile
        );

        /// <summary>Gets a reference to the current font file.</summary>
        /// <param name="fontFile">Pointer to the newly created font file object.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCurrentFontFile(
            [In] IDWriteFontFileEnumerator* This,
            [Out] IDWriteFontFile** fontFile
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public MoveNext MoveNext;

            public GetCurrentFontFile GetCurrentFontFile;
            #endregion
        }
        #endregion
    }
}
