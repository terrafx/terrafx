// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("0000002F-0000-0000-C000-000000000046")]
    public /* blittable */ unsafe struct IRecordInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IRecordInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IRecordInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IRecordInfo* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RecordInit(
            [In] IRecordInfo* This,
            [Out, ComAliasName("PVOID")] void* pvNew
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RecordClear(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvExisting
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RecordCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvExisting,
            [Out, ComAliasName("PVOID")] void* pvNew
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetGuid(
            [In] IRecordInfo* This,
            [Out, ComAliasName("GUID")] Guid* pGuid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetName(
            [In] IRecordInfo* This,
            [Out, ComAliasName("BSTR")] char** pbstrName = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSize(
            [In] IRecordInfo* This,
            [Out, ComAliasName("ULONG")] uint* pcbSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeInfo(
            [In] IRecordInfo* This,
            [Out] ITypeInfo** ppTypeInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetField(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [Out] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFieldNoCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [Out] VARIANT* pvarField,
            [Out, ComAliasName("PVOID")] void** ppvDataCArray
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PutField(
            [In] IRecordInfo* This,
            [In, ComAliasName("ULONG")] uint wFlags,
            [In, Out, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [In] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PutFieldNoCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("ULONG")] uint wFlags,
            [In, Out, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [In] VARIANT* pvarField
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFieldNames(
            [In] IRecordInfo* This,
            [In, Out, ComAliasName("ULONG")] uint* pcNames,
            [Out, ComAliasName("BSTR[]")] char** rgBstrNames
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsMatchingType(
            [In] IRecordInfo* This,
            [In] IRecordInfo* pRecordInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void* _RecordCreate(
            [In] IRecordInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RecordCreateCopy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvSource,
            [Out, ComAliasName("PVOID")] void** ppvDest
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RecordDestroy(
            [In] IRecordInfo* This,
            [In, ComAliasName("PVOID")] void* pvRecord
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int RecordInit(
            [Out, ComAliasName("PVOID")] void* pvNew
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_RecordInit>(lpVtbl->RecordInit)(
                    This,
                    pvNew
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RecordClear(
            [In, ComAliasName("PVOID")] void* pvExisting
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_RecordClear>(lpVtbl->RecordClear)(
                    This,
                    pvExisting
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RecordCopy(
            [In, ComAliasName("PVOID")] void* pvExisting,
            [Out, ComAliasName("PVOID")] void* pvNew
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_RecordCopy>(lpVtbl->RecordCopy)(
                    This,
                    pvExisting,
                    pvNew
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetGuid(
            [Out, ComAliasName("GUID")] Guid* pGuid
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_GetGuid>(lpVtbl->GetGuid)(
                    This,
                    pGuid
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetName(
            [Out, ComAliasName("BSTR")] char** pbstrName = null
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_GetName>(lpVtbl->GetName)(
                    This,
                    pbstrName
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSize(
            [Out, ComAliasName("ULONG")] uint* pcbSize
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_GetSize>(lpVtbl->GetSize)(
                    This,
                    pcbSize
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTypeInfo(
            [Out] ITypeInfo** ppTypeInfo
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_GetTypeInfo>(lpVtbl->GetTypeInfo)(
                    This,
                    ppTypeInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetField(
            [In, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [Out] VARIANT* pvarField
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_GetField>(lpVtbl->GetField)(
                    This,
                    pvData,
                    szFieldName,
                    pvarField
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFieldNoCopy(
            [In, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [Out] VARIANT* pvarField,
            [Out, ComAliasName("PVOID")] void** ppvDataCArray
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_GetFieldNoCopy>(lpVtbl->GetFieldNoCopy)(
                    This,
                    pvData,
                    szFieldName,
                    pvarField,
                    ppvDataCArray
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PutField(
            [In, ComAliasName("ULONG")] uint wFlags,
            [In, Out, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [In] VARIANT* pvarField
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_PutField>(lpVtbl->PutField)(
                    This,
                    wFlags,
                    pvData,
                    szFieldName,
                    pvarField
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PutFieldNoCopy(
            [In, ComAliasName("ULONG")] uint wFlags,
            [In, Out, ComAliasName("PVOID")] void* pvData,
            [In, ComAliasName("LPCOLESTR")] char* szFieldName,
            [In] VARIANT* pvarField
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_PutFieldNoCopy>(lpVtbl->PutFieldNoCopy)(
                    This,
                    wFlags,
                    pvData,
                    szFieldName,
                    pvarField
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFieldNames(
            [In, Out, ComAliasName("ULONG")] uint* pcNames,
            [Out, ComAliasName("BSTR[]")] char** rgBstrNames
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_GetFieldNames>(lpVtbl->GetFieldNames)(
                    This,
                    pcNames,
                    rgBstrNames
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int IsMatchingType(
            [In] IRecordInfo* pRecordInfo
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_IsMatchingType>(lpVtbl->IsMatchingType)(
                    This,
                    pRecordInfo
                );
            }
        }

        public void* RecordCreate()
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_RecordCreate>(lpVtbl->RecordCreate)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RecordCreateCopy(
            [In, ComAliasName("PVOID")] void* pvSource,
            [Out, ComAliasName("PVOID")] void** ppvDest
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_RecordCreateCopy>(lpVtbl->RecordCreateCopy)(
                    This,
                    pvSource,
                    ppvDest
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RecordDestroy(
            [In, ComAliasName("PVOID")] void* pvRecord
        )
        {
            fixed (IRecordInfo* This = &this)
            {
                return MarshalFunction<_RecordDestroy>(lpVtbl->RecordDestroy)(
                    This,
                    pvRecord
                );
            }
        }
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
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

