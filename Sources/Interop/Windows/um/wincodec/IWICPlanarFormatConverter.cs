// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("BEBEE9CB-83B0-4DCC-8132-B0AAA55EAC96")]
    public /* blittable */ unsafe struct IWICPlanarFormatConverter
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Initialize(
            [In] IWICPlanarFormatConverter* This,
            [In] IWICBitmapSource** ppPlanes,
            [In, ComAliasName("UINT")] uint cPlanes,
            [In, ComAliasName("REFWICPixelFormatGUID")] /* readonly */ Guid* dstFormat,
            [In] WICBitmapDitherType dither,
            [In, Optional] IWICPalette* pIPalette,
            [In] double alphaThresholdPercent,
            [In] WICBitmapPaletteType paletteTranslate
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CanConvert(
            [In] IWICPlanarFormatConverter* This,
            [In, ComAliasName("WICPixelFormatGUID")] /* readonly */ Guid* pSrcPixelFormats,
            [In, ComAliasName("UINT")] uint cSrcPlanes,
            [In, ComAliasName("REFWICPixelFormatGUID")] /* readonly */ Guid* dstPixelFormat,
            [Out, ComAliasName("BOOL")] int* pfCanConvert
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICBitmapSource.Vtbl BaseVtbl;

            public IntPtr Initialize;

            public IntPtr CanConvert;
            #endregion
        }
        #endregion
    }
}
