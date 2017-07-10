// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3AFF9CCE-BE95-4303-B927-E7D16FF4A613")]
    unsafe public /* blittable */ struct IWICPlanarBitmapSourceTransform
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportTransform(
            [In] IWICPlanarBitmapSourceTransform* This,
            [In, Out] UINT* puiWidth,
            [In, Out] UINT* puiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In] /* readonly */ WICPixelFormatGUID* pguidDstFormats,
            [Out] WICBitmapPlaneDescription* pPlaneDescriptions,
            [In] UINT cPlanes,
            [Out] BOOL* pfIsSupported
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyPixels(
            [In] IWICPlanarBitmapSourceTransform* This,
            [In, Optional] /* readonly */ WICRect* prcSource,
            [In] UINT uiWidth,
            [In] UINT uiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In] /* readonly */ WICBitmapPlane* pDstPlanes,
            [In] UINT cPlanes
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public DoesSupportTransform DoesSupportTransform;

            public CopyPixels CopyPixels;
            #endregion
        }
        #endregion
    }
}
