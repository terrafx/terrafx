// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Enumerator for an ordered collection of color glyph runs.</summary>
    [Guid("7C5F86DA-C7A1-4F05-B8E1-55A179FE5A35")]
    public /* blittable */ unsafe struct IDWriteColorGlyphRunEnumerator1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Gets the current color glyph run.</summary>
        /// <param name="colorGlyphRun">Receives a pointer to the color glyph run. The pointer remains valid until the next call to MoveNext or until the interface is released.</param>
        /// <returns> Standard HRESULT error code. An error is returned if there is no current glyph run, i.e., if MoveNext has not yet been called or if the end of the sequence has been reached.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCurrentRun(
            [In] IDWriteColorGlyphRunEnumerator1* This,
            [Out] /* readonly */ DWRITE_COLOR_GLYPH_RUN1** colorGlyphRun
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteColorGlyphRunEnumerator.Vtbl BaseVtbl;

            public IntPtr GetCurrentRun;
            #endregion
        }
        #endregion
    }
}
