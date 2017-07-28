// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00000121-A8F2-4877-BA0A-FD2B6645FB94")]
    unsafe public /* blittable */ struct IWICBitmap
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Lock(
            [In] IWICBitmap* This,
            [In, Optional] /* readonly */ WICRect* prcLock,
            [In, ComAliasName("DWORD")] uint flags,
            [Out, Optional] IWICBitmapLock** ppILock
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPalette(
            [In] IWICBitmap* This,
            [In, Optional] IWICPalette* pIPalette
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetResolution(
            [In] IWICBitmap* This,
            [In] double dpiX,
            [In] double dpiY
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICBitmapSource.Vtbl BaseVtbl;

            public Lock Lock;

            public SetPalette SetPalette;

            public SetResolution SetResolution;
            #endregion
        }
        #endregion
    }
}
