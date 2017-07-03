// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("0000002F-0000-0000-C000-000000000046")]
    unsafe public struct IRecordInfo
    {
        #region Constants
        public static readonly Guid IID = typeof(IRecordInfo).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT RecordInit(
            [Out] void* pvNew
        );

        public /* static */ delegate HRESULT RecordClear(
            [In] void* pvExisting
        );

        public /* static */ delegate HRESULT RecordCopy(
            [In] void* pvExisting,
            [Out] void* pvNew
        );

        public /* static */ delegate HRESULT GetGuid(
            [Out] Guid* pGuid
        );

        public /* static */ delegate HRESULT GetName(
            [Out, Optional]  BSTR* pbstrName
        );

        public /* static */ delegate HRESULT GetSize(
            [Out] uint* pcbSize
        );

        public /* static */ delegate HRESULT GetTypeInfo(
            [Out] ITypeInfo** ppTypeInfo
        );

        public /* static */ delegate HRESULT GetField(
            [In] void* pvData,
            [In] LPOLESTR szFieldName,
            [Out] VARIANT* pvarField
        );

        public /* static */ delegate HRESULT GetFieldNoCopy(
            [In] void* pvData,
            [In] LPOLESTR szFieldName,
            [Out] VARIANT* pvarField,
            [Out] void** ppvDataCArray
        );

        public /* static */ delegate HRESULT PutField(
            [In] uint wFlags,
            [In, Out] void* pvData,
            [In] LPOLESTR szFieldName,
            [In] VARIANT* pvarField
        );

        public /* static */ delegate HRESULT PutFieldNoCopy(
            [In] uint wFlags,
            [In, Out] void* pvData,
            [In] LPOLESTR szFieldName,
            [In] VARIANT* pvarField
        );

        public /* static */ delegate HRESULT GetFieldNames(
            [In, Out] uint* pcNames,
            [Out]  BSTR* rgBstrNames
        );

        public /* static */ delegate BOOL IsMatchingType(
            [In] IRecordInfo* pRecordInfo
        );

        public /* static */ delegate void* RecordCreate(
        );

        public /* static */ delegate HRESULT RecordCreateCopy(
            [In] void* pvSource,
            [Out] void** ppvDest
        );

        public /* static */ delegate HRESULT RecordDestroy(
            [In] void* pvRecord
        );
        #endregion

        #region Structs
        public struct Vtbl
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
