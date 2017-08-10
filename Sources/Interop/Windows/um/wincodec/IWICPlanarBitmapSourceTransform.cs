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
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportTransform(
            [In] IWICPlanarBitmapSourceTransform* This,
            [In, Out, ComAliasName("UINT")] uint* puiWidth,
            [In, Out, ComAliasName("UINT")] uint* puiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In, ComAliasName("WICPixelFormatGUID")] /* readonly */ Guid* pguidDstFormats,
            [Out] WICBitmapPlaneDescription* pPlaneDescriptions,
            [In, ComAliasName("UINT")] uint cPlanes,
            [Out, ComAliasName("BOOL")] int* pfIsSupported
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CopyPixels(
            [In] IWICPlanarBitmapSourceTransform* This,
            [In, Optional] /* readonly */ WICRect* prcSource,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In] /* readonly */ WICBitmapPlane* pDstPlanes,
            [In, ComAliasName("UINT")] uint cPlanes
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr DoesSupportTransform;

            public IntPtr CopyPixels;
            #endregion
        }
        #endregion
    }
}
