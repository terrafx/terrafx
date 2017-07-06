// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00020402-0000-0000-C000-000000000046")]
    unsafe public  /* blittable */ struct ITypeLib
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate uint GetTypeInfoCount(
            [In] ITypeLib* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeInfo(
            [In] ITypeLib* This,
            [In] UINT index,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeInfoType(
            [In] ITypeLib* This,
            [In] UINT index,
            [Out] TYPEKIND* pTKind
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeInfoOfGuid(
            [In] ITypeLib* This,
            [In] REFGUID guid,
            [Out, Optional] ITypeInfo** ppTinfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetLibAttr(
            [In] ITypeLib* This,
            [Out] TLIBATTR** ppTLibAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeComp(
            [In] ITypeLib* This,
            [Out, Optional] ITypeComp** ppTComp
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDocumentation(
            [In] ITypeLib* This,
            [In] INT index,
            [Out, Optional] BSTR *pBstrName,
            [Out, Optional] BSTR *pBstrDocString,
            [Out] DWORD *pdwHelpContext,
            [Out, Optional] BSTR *pBstrHelpFile
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT IsName(
            [In] ITypeLib* This,
            [In, Out] LPOLESTR szNameBuf,
            [In] ULONG lHashVal,
            [Out] BOOL* pfName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT FindName(
            [In] ITypeLib* This,
            [In, Out] LPOLESTR szNameBuf,
            [In] ULONG lHashVal,
            [Out] ITypeInfo** ppTInfo,
            [Out] MEMBERID* rgMemId,
            [In, Out] USHORT* pcFound
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseTLibAttr(
            [In] ITypeLib* This,
            [In] TLIBATTR* pTLibAttr
        );
        #endregion

        #region Structs
        public  /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetTypeInfoCount GetTypeInfoCount;

            public GetTypeInfo GetTypeInfo;

            public GetTypeInfoType GetTypeInfoType;

            public GetTypeInfoOfGuid GetTypeInfoOfGuid;

            public GetLibAttr GetLibAttr;

            public GetTypeComp GetTypeComp;

            public GetDocumentation GetDocumentation;

            public IsName IsName;

            public FindName FindName;

            public ReleaseTLibAttr ReleaseTLibAttr;
            #endregion
        }
        #endregion
    }
}
