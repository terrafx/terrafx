// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("F67E0EDD-9E3D-4ECC-8C32-4183253DFE70")]
    unsafe public /* blittable */ struct IDWriteTextFormat2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Set line spacing.</summary>
        /// <param name="lineSpacingOptions">How to manage space between lines.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetLineSpacing(
            [In] IDWriteTextFormat2* This,
            [In] /* readonly */ DWRITE_LINE_SPACING* lineSpacingOptions
        );

        /// <summary>Get line spacing.</summary>
        /// <param name="lineSpacingOptions">How to manage space between lines.</param>
        /// <returns> Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetLineSpacing(
            [In] IDWriteTextFormat2* This,
            [Out] DWRITE_LINE_SPACING* lineSpacingOptions
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDWriteTextFormat1.Vtbl BaseVtbl;

            public IntPtr SetLineSpacing;

            public IntPtr GetLineSpacing;
            #endregion
        }
        #endregion
    }
}
