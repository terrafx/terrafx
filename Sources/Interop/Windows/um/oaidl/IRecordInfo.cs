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
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RecordInit(
            [In] IRecordInfo* This,
            [Out, ComAliasName("PVOID")] void* pvNew
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RecordClear(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvExisting
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RecordCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvExisting,
            [Out, ComAliasName("PVOID")] void* pvNew
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGuid(
            [In] IRecordInfo* This,
            [Out, ComAliasName("GUID")] Guid* pGuid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetName(
            [In] IRecordInfo* This,
            [Out, ComAliasName("BSTR")] char** pbstrName = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSize(
            [In] IRecordInfo* This,
            [Out, ComAliasName("ULONG")] uint* pcbSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTypeInfo(
            [In] IRecordInfo* This,
            [Out] ITypeInfo** ppTypeInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetField(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] /* readonly */ char* szFieldName,
            [Out] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFieldNoCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] /* readonly */ char* szFieldName,
            [Out] VARIANT* pvarField,
            [Out, ComAliasName("PVOID")] void** ppvDataCArray
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PutField(
            [In] IRecordInfo* This,
            [In, ComAliasName("ULONG")] uint wFlags,
            [In, Out, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] /* readonly */ char* szFieldName,
            [In] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PutFieldNoCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("ULONG")] uint wFlags,
            [In, Out, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] /* readonly */ char* szFieldName,
            [In] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFieldNames(
            [In] IRecordInfo* This,
            [In, Out, ComAliasName("ULONG")] uint* pcNames,
            [Out, ComAliasName("BSTR")] char** rgBstrNames
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int IsMatchingType(
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
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RecordCreateCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvSource,
            [Out, ComAliasName("PVOID")] void** ppvDest
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RecordDestroy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvRecord
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr RecordInit;

            public IntPtr RecordClear;

            public IntPtr RecordCopy;

            public IntPtr GetGuid;

            public IntPtr GetName;

            public IntPtr GetSize;

            public IntPtr GetTypeInfo;

            public IntPtr GetField;

            public IntPtr GetFieldNoCopy;

            public IntPtr PutField;

            public IntPtr PutFieldNoCopy;

            public IntPtr GetFieldNames;

            public IntPtr IsMatchingType;

            public IntPtr RecordCreate;

            public IntPtr RecordCreateCopy;

            public IntPtr RecordDestroy;
            #endregion
        }
        #endregion
    }
}
