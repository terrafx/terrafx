// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00000301-A8F2-4877-BA0A-FD2B6645FB94")]
    unsafe public /* blittable */ struct IWICFormatConverter
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Initialize(
            [In] IWICFormatConverter* This,
            [In, Optional] IWICBitmapSource* pISource,
            [In] REFWICPixelFormatGUID dstFormat,
            [In] WICBitmapDitherType dither,
            [In, Optional] IWICPalette* pIPalette,
            [In] double alphaThresholdPercent,
            [In] WICBitmapPaletteType paletteTranslate
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CanConvert(
            [In] IWICFormatConverter* This,
            [In] REFWICPixelFormatGUID srcPixelFormat,
            [In] REFWICPixelFormatGUID dstPixelFormat,
            [Out] BOOL* pfCanConvert
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICBitmapSource.Vtbl BaseVtbl;

            public Initialize Initialize;

            public CanConvert CanConvert;
            #endregion
        }
        #endregion
    }
}
