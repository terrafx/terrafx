// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("F928B7B8-2221-40C1-B72E-7E82F1974D1A")]
    unsafe public /* blittable */ struct IWICPlanarBitmapFrameEncode
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WritePixels(
            [In] IWICPlanarBitmapFrameEncode* This,
            [In, ComAliasName("UINT")] uint lineCount,
            [In] WICBitmapPlane* pPlanes,
            [In, ComAliasName("UINT")] uint cPlanes
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WriteSource(
            [In] IWICPlanarBitmapFrameEncode* This,
            [In] IWICBitmapSource** ppPlanes,
            [In, ComAliasName("UINT")] uint cPlanes,
            [In, Optional] WICRect* prcSource
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public WritePixels WritePixels;

            public WriteSource WriteSource;
            #endregion
        }
        #endregion
    }
}
