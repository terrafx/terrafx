// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00020401-0000-0000-C000-000000000046")]
    unsafe public  /* blittable */ struct ITypeInfo
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeAttr(
            [In] ITypeInfo* This,
            [Out] TYPEATTR** ppTypeAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeComp(
            [In] ITypeInfo* This,
            [Out, Optional] ITypeComp** ppTComp
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFuncDesc(
            [In] ITypeInfo* This,
            [In] UINT index,
            [Out] FUNCDESC** ppFuncDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetVarDesc(
            [In] ITypeInfo* This,
            [In] UINT index,
            [Out] VARDESC** ppVarDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetNames(
            [In] ITypeInfo* This,
            [In] MEMBERID memid,
            [Out] BSTR* rgBstrNames,
            [In] UINT cMaxNames,
            [Out] UINT* pcNames
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetRefTypeOfImplType(
            [In] ITypeInfo* This,
            [In] UINT index,
            [Out] HREFTYPE* pRefType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetImplTypeFlags(
            [In] ITypeInfo* This,
            [In] UINT index,
            [Out] INT* pImplTypeFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetIDsOfNames(
            [In] ITypeInfo* This,
            [In] LPOLESTR* rgszNames,
            [In] UINT cNames,
            [Out] MEMBERID* pMemId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Invoke(
            [In] ITypeInfo* This,
            [In] PVOID pvInstance,
            [In] MEMBERID memid,
            [In] WORD wFlags,
            [In, Out] DISPPARAMS* pDispParams,
            [Out] VARIANT* pVarResult,
            [Out] EXCEPINFO* pExcepInfo,
            [Out] UINT* puArgErr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDocumentation(
            [In] ITypeInfo* This,
            [In] MEMBERID memid,
            [Out, Optional] BSTR* pBstrName,
            [Out, Optional] BSTR* pBstrDocString,
            [Out] DWORD* pdwHelpContext,
            [Out, Optional] BSTR* pBstrHelpFile
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDllEntry(
            [In] ITypeInfo* This,
            [In] MEMBERID memid,
            [In] INVOKEKIND invKind,
            [Out, Optional] BSTR* pBstrDllName,
            [Out, Optional] BSTR* pBstrName,
            [Out] WORD* pwOrdinal
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetRefTypeInfo(
            [In] ITypeInfo* This,
            [In] HREFTYPE hRefType,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddressOfMember(
            [In] ITypeInfo* This,
            [In] MEMBERID memid,
            [In] INVOKEKIND invKind,
            [Out] PVOID* ppv
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateInstance(
            [In] ITypeInfo* This,
            [In] IUnknown* pUnkOuter,
            [In] REFIID riid,
            [Out] PVOID* ppvObj
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMops(
            [In] ITypeInfo* This,
            [In] MEMBERID memid,
            [Out, Optional] BSTR* pBstrMops
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetContainingTypeLib(
            [In] ITypeInfo* This,
            [Out] ITypeLib** ppTLib,
            [Out] UINT* pIndex
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseTypeAttr(
            [In] ITypeInfo* This,
            [In] TYPEATTR* pTypeAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseFuncDesc(
            [In] ITypeInfo* This,
            [In] FUNCDESC* pFuncDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseVarDesc(
            [In] ITypeInfo* This,
            [In] VARDESC* pVarDesc
        );
        #endregion

        #region Structs
        public  /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetTypeAttr GetTypeAttr;

            public GetTypeComp GetTypeComp;

            public GetFuncDesc GetFuncDesc;

            public GetVarDesc GetVarDesc;

            public GetNames GetNames;

            public GetRefTypeOfImplType GetRefTypeOfImplType;

            public GetImplTypeFlags GetImplTypeFlags;

            public GetIDsOfNames GetIDsOfNames;

            public Invoke Invoke;

            public GetDocumentation GetDocumentation;

            public GetDllEntry GetDllEntry;

            public GetRefTypeInfo GetRefTypeInfo;

            public AddressOfMember AddressOfMember;

            public CreateInstance CreateInstance;

            public GetMops GetMops;

            public GetContainingTypeLib GetContainingTypeLib;

            public ReleaseTypeAttr ReleaseTypeAttr;

            public ReleaseFuncDesc ReleaseFuncDesc;

            public ReleaseVarDesc ReleaseVarDesc;
            #endregion
        }
        #endregion
    }
}
