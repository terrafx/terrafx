// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("9204FE99-D8FC-4FD5-A001-9536B067A899")]
    unsafe public /* blittable */ struct IWICMetadataReader
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMetadataFormat(
            [In] IWICMetadataReader* This,
            [Out] GUID* pguidMetadataFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMetadataHandlerInfo(
            [In] IWICMetadataReader* This,
            [Out, Optional] IWICMetadataHandlerInfo** ppIHandler
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetCount(
            [In] IWICMetadataReader* This,
            [Out] UINT* pcCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetValueByIndex(
            [In] IWICMetadataReader* This,
            [In] UINT nIndex,
            [In, Out, Optional] PROPVARIANT* pvarSchema,
            [In, Out, Optional] PROPVARIANT* pvarId,
            [In, Out, Optional] PROPVARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetValue(
            [In] IWICMetadataReader* This,
            [In, Optional] /* readonly */ PROPVARIANT* pvarSchema,
            [In] /* readonly */ PROPVARIANT* pvarId,
            [In, Out, Optional] PROPVARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetEnumerator(
            [In] IWICMetadataReader* This,
            [Out, Optional] IWICEnumMetadataItem** ppIEnumMetadata
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetMetadataFormat GetMetadataFormat;

            public GetMetadataHandlerInfo GetMetadataHandlerInfo;

            public GetCount GetCount;

            public GetValueByIndex GetValueByIndex;

            public GetValue GetValue;

            public GetEnumerator GetEnumerator;
            #endregion
        }
        #endregion
    }
}
