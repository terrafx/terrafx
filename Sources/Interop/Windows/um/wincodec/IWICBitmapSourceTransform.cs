// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3B16811B-6A43-4EC9-B713-3D5A0C13B940")]
    unsafe public /* blittable */ struct IWICBitmapSourceTransform
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyPixels(
            [In] IWICBitmapSourceTransform* This,
            [In, Optional] /* readonly */ WICRect* prc,
            [In] UINT uiWidth,
            [In] UINT uiHeight,
            [In, Optional] WICPixelFormatGUID* pguidDstFormat,
            [In] WICBitmapTransformOptions dstTransform,
            [In] UINT nStride,
            [In] UINT cbBufferSize,
            [Out] BYTE* pbBuffer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetClosestSize(
            [In] IWICBitmapSourceTransform* This,
            [In, Out] UINT* puiWidth,
            [In, Out] UINT* puiHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetClosestPixelFormat(
            [In] IWICBitmapSourceTransform* This,
            [In, Out] WICPixelFormatGUID* pguidDstFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportTransform(
            [In] IWICBitmapSourceTransform* This,
            [In] WICBitmapTransformOptions dstTransform,
            [Out] BOOL* pfIsSupported
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public CopyPixels CopyPixels;

            public GetClosestSize GetClosestSize;

            public GetClosestPixelFormat GetClosestPixelFormat;

            public DoesSupportTransform DoesSupportTransform;
            #endregion
        }
        #endregion
    }
}
