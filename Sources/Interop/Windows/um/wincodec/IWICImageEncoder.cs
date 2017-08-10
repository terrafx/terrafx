// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("04C75BF8-3CE1-473B-ACC5-3CC4F5E94999")]
    unsafe public /* blittable */ struct IWICImageEncoder
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WriteFrame(
            [In] IWICImageEncoder* This,
            [In] ID2D1Image* pImage,
            [In] IWICBitmapFrameEncode* pFrameEncode,
            [In] /* readonly */ WICImageParameters* pImageParameters
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WriteFrameThumbnail(
            [In] IWICImageEncoder* This,
            [In] ID2D1Image* pImage,
            [In] IWICBitmapFrameEncode* pFrameEncode,
            [In] /* readonly */ WICImageParameters* pImageParameters
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WriteThumbnail(
            [In] IWICImageEncoder* This,
            [In] ID2D1Image* pImage,
            [In] IWICBitmapEncoder* pEncoder,
            [In] /* readonly */ WICImageParameters* pImageParameters
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr WriteFrame;

            public IntPtr WriteFrameThumbnail;

            public IntPtr WriteThumbnail;
            #endregion
        }
        #endregion
    }
}
