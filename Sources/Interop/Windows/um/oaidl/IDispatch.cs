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
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeInfoCount(
            [In] IDispatch* This,
            [Out, ComAliasName("UINT")] uint* pctinfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeInfo(
            [In] IDispatch* This,
            [In, ComAliasName("UINT")] uint iTInfo,
            [In, ComAliasName("LCID")] uint lcid,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetIDsOfNames(
            [In] IDispatch* This,
            [In, ComAliasName("REFIID")] /* readonly */ Guid* riid,
            [In, ComAliasName("LPOLESTR")] char** rgszNames,
            [In, ComAliasName("UINT")] uint cNames,
            [In, ComAliasName("LCID")] uint lcid,
            [Out, ComAliasName("DISPID")] int* rgDispId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Invoke(
            [In] IDispatch* This,
            [In, ComAliasName("DISPID")] int dispIdMember,
            [In, ComAliasName("REFIID")] /* readonly */ Guid* riid,
            [In, ComAliasName("LCID")] uint lcid,
            [In, ComAliasName("WORD")] ushort wFlags,
            [In] DISPPARAMS* pDispParams,
            [Out, Optional] VARIANT* pVarResult,
            [Out, Optional] EXCEPINFO* pExcepInfo,
            [Out, Optional, ComAliasName("UINT")] uint* puArgErr
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr GetTypeInfoCount;

            public IntPtr GetTypeInfo;

            public IntPtr GetIDsOfNames;

            public IntPtr Invoke;
            #endregion
        }
        #endregion
    }
}
