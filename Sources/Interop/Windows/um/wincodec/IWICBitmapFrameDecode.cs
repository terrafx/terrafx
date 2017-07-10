// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3B16811B-6A43-4EC9-A813-3D930C13B940")]
    unsafe public /* blittable */ struct IWICBitmapFrameDecode
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMetadataQueryReader(
            [In] IWICBitmapFrameDecode* This,
            [Out, Optional] IWICMetadataQueryReader** ppIMetadataQueryReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetColorContexts(
            [In] IWICBitmapFrameDecode* This,
            [In] UINT cCount,
            [In, Out, Optional] IWICColorContext** ppIColorContexts,
            [Out] UINT* pcActualCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetThumbnail(
            [In] IWICBitmapFrameDecode* This,
            [Out, Optional] IWICBitmapSource** ppIThumbnail
         );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICBitmapSource.Vtbl BaseVtbl;

            public GetMetadataQueryReader GetMetadataQueryReader;

            public GetColorContexts GetColorContexts;

            public GetThumbnail GetThumbnail;
            #endregion
        }
        #endregion
    }
}
