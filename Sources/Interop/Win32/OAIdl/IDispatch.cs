// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("00020400-0000-0000-C000-000000000046")]
    unsafe public struct IDispatch
    {
        #region Constants
        public static readonly Guid IID = typeof(IDispatch).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetTypeInfoCount(
            [Out] uint* pctinfo
        );

        public /* static */ delegate HRESULT GetTypeInfo(
            [In] uint iTInfo,
            [In] LCID lcid,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        public /* static */ delegate HRESULT GetIDsOfNames(
            [In] ref /* readonly */ Guid riid,
            [In] LPOLESTR* rgszNames,
            [In] uint cNames,
            [In] LCID lcid,
            [Out] DISPID* rgDispId
        );

        public /* static */ delegate HRESULT Invoke(
            [In] DISPID dispIdMember,
            [In] ref /* readonly */ Guid riid,
            [In] LCID lcid,
            [In] ushort wFlags,
            [In] DISPPARAMS* pDispParams,
            [Out, Optional] VARIANT* pVarResult,
            [Out, Optional] EXCEPINFO* pExcepInfo,
            [Out, Optional] uint* puArgErr
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetTypeInfoCount GetTypeInfoCount;

            public GetTypeInfo GetTypeInfo;

            public GetIDsOfNames GetIDsOfNames;

            public Invoke Invoke;
            #endregion
        }
        #endregion
    }
}
