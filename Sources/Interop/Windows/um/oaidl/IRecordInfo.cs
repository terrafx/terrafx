// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0000002F-0000-0000-C000-000000000046")]
    unsafe public /* blittable */ struct IRecordInfo
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RecordInit(
            [In] IRecordInfo* This,
            [Out] PVOID pvNew
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RecordClear(
            [In] IRecordInfo* This,
            [In] PVOID pvExisting
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RecordCopy(
            [In] IRecordInfo* This,
            [In] PVOID pvExisting,
            [Out] PVOID pvNew
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetGuid(
            [In] IRecordInfo* This,
            [Out] GUID* pGuid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetName(
            [In] IRecordInfo* This,
            [Out, Optional] BSTR* pbstrName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSize(
            [In] IRecordInfo* This,
            [Out] ULONG* pcbSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetTypeInfo(
            [In] IRecordInfo* This,
            [Out] ITypeInfo** ppTypeInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetField(
            [In] IRecordInfo* This,
            [In] PVOID pvData,
            [In] LPCOLESTR szFieldName,
            [Out] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFieldNoCopy(
            [In] IRecordInfo* This,
            [In] PVOID pvData,
            [In] LPCOLESTR szFieldName,
            [Out] VARIANT* pvarField,
            [Out] PVOID* ppvDataCArray
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PutField(
            [In] IRecordInfo* This,
            [In] ULONG wFlags,
            [In, Out] PVOID pvData,
            [In] LPCOLESTR szFieldName,
            [In] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PutFieldNoCopy(
            [In] IRecordInfo* This,
            [In] ULONG wFlags,
            [In, Out] PVOID pvData,
            [In] LPCOLESTR szFieldName,
            [In] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFieldNames(
            [In] IRecordInfo* This,
            [In, Out] ULONG* pcNames,
            [Out] BSTR* rgBstrNames
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL IsMatchingType(
            [In] IRecordInfo* This,
            [In] IRecordInfo* pRecordInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void* RecordCreate(
            [In] IRecordInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RecordCreateCopy(
            [In] IRecordInfo* This,
            [In] PVOID pvSource,
            [Out] PVOID* ppvDest
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RecordDestroy(
            [In] IRecordInfo* This,
            [In] PVOID pvRecord
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public RecordInit RecordInit;

            public RecordClear RecordClear;

            public RecordCopy RecordCopy;

            public GetGuid GetGuid;

            public GetName GetName;

            public GetSize GetSize;

            public GetTypeInfo GetTypeInfo;

            public GetField GetField;

            public GetFieldNoCopy GetFieldNoCopy;

            public PutField PutField;

            public PutFieldNoCopy PutFieldNoCopy;

            public GetFieldNames GetFieldNames;

            public IsMatchingType IsMatchingType;

            public RecordCreate RecordCreate;

            public RecordCreateCopy RecordCreateCopy;

            public RecordDestroy RecordDestroy;
            #endregion
        }
        #endregion
    }
}
