// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00000040-A8F2-4877-BA0A-FD2B6645FB94")]
    unsafe public /* blittable */ struct IWICPalette
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializePredefined(
            [In] IWICPalette* This,
            [In] WICBitmapPaletteType ePaletteType,
            [In] BOOL fAddTransparentColor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializeCustom(
            [In] IWICPalette* This,
            [In] WICColor* pColors,
            [In] UINT cCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializeFromBitmap(
            [In] IWICPalette* This,
            [In, Optional] IWICBitmapSource* pISurface,
            [In] UINT cCount,
            [In] BOOL fAddTransparentColor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializeFromPalette(
            [In] IWICPalette* This,
            [In, Optional] IWICPalette* pIPalette
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT _GetType(
            [In] IWICPalette* This,
            [Out] WICBitmapPaletteType* pePaletteType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetColorCount(
            [In] IWICPalette* This,
            [Out] UINT* pcCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetColors(
            [In] IWICPalette* This,
            [In] UINT cCount,
            [Out] WICColor* pColors,
            [Out] UINT* pcActualColors
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT IsBlackWhite(
            [In] IWICPalette* This,
            [Out] BOOL* pfIsBlackWhite
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT IsGrayscale(
            [In] IWICPalette* This,
            [Out] BOOL* pfIsGrayscale
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT HasAlpha(
            [In] IWICPalette* This,
            [Out] BOOL* pfHasAlpha
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public InitializePredefined InitializePredefined;

            public InitializeCustom InitializeCustom;

            public InitializeFromBitmap InitializeFromBitmap;

            public InitializeFromPalette InitializeFromPalette;

            public _GetType _GetType;

            public GetColorCount GetColorCount;

            public GetColors GetColors;

            public IsBlackWhite IsBlackWhite;

            public IsGrayscale IsGrayscale;

            public HasAlpha HasAlpha;
            #endregion
        }
        #endregion
    }
}
