// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("00020401-0000-0000-C000-000000000046")]
    unsafe public struct ITypeInfo
    {
        #region Constants
        public static readonly Guid IID = typeof(ITypeInfo).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetTypeAttr(
            [Out] TYPEATTR** ppTypeAttr
        );

        public /* static */ delegate HRESULT GetTypeComp(
            [Out, Optional] ITypeComp **ppTComp
        );

        public /* static */ delegate HRESULT GetFuncDesc(
            [In] uint index,
            [Out] FUNCDESC** ppFuncDesc
        );

        public /* static */ delegate HRESULT GetVarDesc(
            [In] uint index,
            [Out] VARDESC** ppVarDesc
        );

        public /* static */ delegate HRESULT GetNames(
            [In] MEMBERID memid,
            [Out]  BSTR* rgBstrNames,
            [In] uint cMaxNames,
            [Out] uint* pcNames
        );

        public /* static */ delegate HRESULT GetRefTypeOfImplType(
            [In] uint index,
            [Out] HREFTYPE* pRefType
        );

        public /* static */ delegate HRESULT GetImplTypeFlags(
            [In] uint index,
            [Out] int* pImplTypeFlags
        );

        public /* static */ delegate HRESULT GetIDsOfNames(
            [In] LPOLESTR* rgszNames,
            [In] uint cNames,
            [Out] MEMBERID* pMemId
        );

        public /* static */ delegate HRESULT Invoke(
            [In] void* pvInstance,
            [In] MEMBERID memid,
            [In] ushort wFlags,
            [In, Out] DISPPARAMS* pDispParams,
            [Out] VARIANT* pVarResult,
            [Out] EXCEPINFO* pExcepInfo,
            [Out] uint* puArgErr
        );

        public /* static */ delegate HRESULT GetDocumentation(
            [In] MEMBERID memid,
            [Out, Optional]  BSTR* pBstrName,
            [Out, Optional]  BSTR* pBstrDocString,
            [Out] uint* pdwHelpContext,
            [Out, Optional]  BSTR* pBstrHelpFile
        );

        public /* static */ delegate HRESULT GetDllEntry(
            [In] MEMBERID memid,
            [In] INVOKEKIND invKind,
            [Out, Optional]  BSTR* pBstrDllName,
            [Out, Optional]  BSTR* pBstrName,
            [Out] ushort* pwOrdinal
        );

        public /* static */ delegate HRESULT GetRefTypeInfo(
            [In] HREFTYPE hRefType,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        public /* static */ delegate HRESULT AddressOfMember(
            [In] MEMBERID memid,
            [In] INVOKEKIND invKind,
            [Out] void** ppv
        );

        public /* static */ delegate HRESULT CreateInstance(
            [In] IUnknown* pUnkOuter,
            [In] ref /* readonly */ Guid riid,
            void** ppvObj
        );

        public /* static */ delegate HRESULT GetMops(
            [In] MEMBERID memid,
            [Out, Optional] BSTR* pBstrMops
        );

        public /* static */ delegate HRESULT GetContainingTypeLib(
            [Out] ITypeLib** ppTLib,
            [Out] uint* pIndex
        );

        public /* static */ delegate void ReleaseTypeAttr(
            [In] TYPEATTR* pTypeAttr
        );

        public /* static */ delegate void ReleaseFuncDesc(
            [In] FUNCDESC* pFuncDesc
        );

        public /* static */ delegate void ReleaseVarDesc(
            [In] VARDESC* pVarDesc
        );
        #endregion

        #region Structs
        public struct Vtbl
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
