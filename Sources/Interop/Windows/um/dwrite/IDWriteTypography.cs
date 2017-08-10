// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Font typography setting.</summary>
    [Guid("55F1112B-1DC2-4B3C-9541-F46894ED85B6")]
    unsafe public /* blittable */ struct IDWriteTypography
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Add font feature.</summary>
        /// <param name="fontFeature">The font feature to add.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddFontFeature(
            [In] IDWriteTypography* This,
            [In] DWRITE_FONT_FEATURE fontFeature
        );

        /// <summary>Get the number of font features.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetFontFeatureCount(
            [In] IDWriteTypography* This
        );

        /// <summary>Get the font feature at the specified index.</summary>
        /// <param name="fontFeatureIndex">The zero-based index of the font feature to get.</param>
        /// <param name="fontFeature">The font feature.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFontFeature(
            [In] IDWriteTypography* This,
            [In, ComAliasName("UINT32")] uint fontFeatureIndex,
            [Out] DWRITE_FONT_FEATURE* fontFeature
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr AddFontFeature;

            public IntPtr GetFontFeatureCount;

            public IntPtr GetFontFeature;
            #endregion
        }
        #endregion
    }
}
