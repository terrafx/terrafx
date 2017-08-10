// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00020401-0000-0000-C000-000000000046")]
    unsafe public /* blittable */ struct ITypeInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeAttr(
            [In] ITypeInfo* This,
            [Out] TYPEATTR** ppTypeAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeComp(
            [In] ITypeInfo* This,
            [Out, Optional] ITypeComp** ppTComp
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFuncDesc(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] FUNCDESC** ppFuncDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetVarDesc(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] VARDESC** ppVarDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetNames(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, ComAliasName("BSTR")] char** rgBstrNames,
            [In, ComAliasName("UINT")] uint cMaxNames,
            [Out, ComAliasName("UINT")] uint* pcNames
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRefTypeOfImplType(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out, ComAliasName("HREFTYPE")] uint* pRefType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetImplTypeFlags(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out, ComAliasName("INT")] int* pImplTypeFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetIDsOfNames(
            [In] ITypeInfo* This,
            [In, ComAliasName("LPOLESTR")] char** rgszNames,
            [In, ComAliasName("UINT")] uint cNames,
            [Out, ComAliasName("MEMBERID")] int* pMemId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Invoke(
            [In] ITypeInfo* This,
            [In, ComAliasName("PVOID")] void* pvInstance,
            [In, ComAliasName("MEMBERID")] int memid,
            [In, ComAliasName("WORD")] ushort wFlags,
            [In, Out] DISPPARAMS* pDispParams,
            [Out] VARIANT* pVarResult,
            [Out] EXCEPINFO* pExcepInfo,
            [Out, ComAliasName("UINT")] uint* puArgErr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDocumentation(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDocString,
            [Out, ComAliasName("DWORD")] uint* pdwHelpContext,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrHelpFile
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDllEntry(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [In] INVOKEKIND invKind,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDllName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, ComAliasName("WORD")] ushort* pwOrdinal
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRefTypeInfo(
            [In] ITypeInfo* This,
            [In, ComAliasName("HREFTYPE")] uint hRefType,
            [Out, Optional] ITypeInfo** ppTInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddressOfMember(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [In] INVOKEKIND invKind,
            [Out, ComAliasName("PVOID")] void** ppv
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInstance(
            [In] ITypeInfo* This,
            [In] IUnknown* pUnkOuter,
            [In, ComAliasName("REFIID")] /* readonly */ Guid* riid,
            [Out, ComAliasName("PVOID")] void** ppvObj
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMops(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrMops
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetContainingTypeLib(
            [In] ITypeInfo* This,
            [Out] ITypeLib** ppTLib,
            [Out, ComAliasName("UINT")] uint* pIndex
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
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr GetTypeAttr;

            public IntPtr GetTypeComp;

            public IntPtr GetFuncDesc;

            public IntPtr GetVarDesc;

            public IntPtr GetNames;

            public IntPtr GetRefTypeOfImplType;

            public IntPtr GetImplTypeFlags;

            public IntPtr GetIDsOfNames;

            public IntPtr Invoke;

            public IntPtr GetDocumentation;

            public IntPtr GetDllEntry;

            public IntPtr GetRefTypeInfo;

            public IntPtr AddressOfMember;

            public IntPtr CreateInstance;

            public IntPtr GetMops;

            public IntPtr GetContainingTypeLib;

            public IntPtr ReleaseTypeAttr;

            public IntPtr ReleaseFuncDesc;

            public IntPtr ReleaseVarDesc;
            #endregion
        }
        #endregion
    }
}
