// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("00020402-0000-0000-C000-000000000046")]
    unsafe public struct ITypeLib
    {
        #region Constants
        public static readonly Guid IID = typeof(ITypeLib).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate uint GetTypeInfoCount(
        );

        public /* static */ delegate HRESULT GetTypeInfo(
            [In] uint index,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        public /* static */ delegate HRESULT GetTypeInfoType(
            [In] uint index,
            [Out] TYPEKIND* pTKind
        );

        public /* static */ delegate HRESULT GetTypeInfoOfGuid(
            [In] ref /* readonly */ Guid guid,
            [Out, Optional] ITypeInfo** ppTinfo
        );

        public /* static */ delegate HRESULT GetLibAttr(
            [Out] TLIBATTR** ppTLibAttr
        );

        public /* static */ delegate HRESULT GetTypeComp(
            [Out, Optional] ITypeComp** ppTComp
        );

        public /* static */ delegate HRESULT GetDocumentation(
            [In] int index,
            [Out, Optional] BSTR *pBstrName,
            [Out, Optional] BSTR *pBstrDocString,
            [Out] uint *pdwHelpContext,
            [Out, Optional] BSTR *pBstrHelpFile
        );

        public /* static */ delegate HRESULT IsName(
            [In, Out] LPOLESTR szNameBuf,
            [In] uint lHashVal,
            [Out] BOOL* pfName
        );

        public /* static */ delegate HRESULT FindName(
            [In, Out] LPOLESTR szNameBuf,
            [In] uint lHashVal,
            [Out] ITypeInfo** ppTInfo,
            [Out] MEMBERID* rgMemId,
            [In, Out] ushort* pcFound
        );

        public /* static */ delegate void ReleaseTLibAttr(
            [In] TLIBATTR* pTLibAttr
        );
        #endregion

        #region Structs
        public struct Vtbl
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
