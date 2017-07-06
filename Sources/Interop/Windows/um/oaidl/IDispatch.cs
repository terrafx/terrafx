// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00020400-0000-0000-C000-000000000046")]
    unsafe public /* blittable */ struct IDispatch
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeInfoCount(
            [In] IDispatch* This,
            [Out] UINT* pctinfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeInfo(
            [In] IDispatch* This,
            [In] UINT iTInfo,
            [In] LCID lcid,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetIDsOfNames(
            [In] IDispatch* This,
            [In] REFIID riid,
            [In] LPOLESTR* rgszNames,
            [In] UINT cNames,
            [In] LCID lcid,
            [Out] DISPID* rgDispId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Invoke(
            [In] IDispatch* This,
            [In] DISPID dispIdMember,
            [In] REFIID riid,
            [In] LCID lcid,
            [In] WORD wFlags,
            [In] DISPPARAMS* pDispParams,
            [Out, Optional] VARIANT* pVarResult,
            [Out, Optional] EXCEPINFO* pExcepInfo,
            [Out, Optional] UINT* puArgErr
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
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
