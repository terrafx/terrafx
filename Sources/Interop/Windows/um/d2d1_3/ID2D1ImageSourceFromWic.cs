// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Produces 2D pixel data that has been sourced from WIC.</summary>
    [Guid("77395441-1C8F-4555-8683-F50DAB0FE792")]
    public /* blittable */ unsafe struct ID2D1ImageSourceFromWic
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EnsureCached(
            [In] ID2D1ImageSourceFromWic* This,
            [In, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* rectangleToFill = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int TrimCache(
            [In] ID2D1ImageSourceFromWic* This,
            [In, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* rectangleToPreserve = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetSource(
            [In] ID2D1ImageSourceFromWic* This,
            [Out] IWICBitmapSource** wicBitmapSource
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1ImageSource.Vtbl BaseVtbl;

            public IntPtr EnsureCached;

            public IntPtr TrimCache;

            public IntPtr GetSource;
            #endregion
        }
        #endregion
    }
}
