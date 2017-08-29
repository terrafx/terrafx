// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("00020401-0000-0000-C000-000000000046")]
    public /* blittable */ unsafe struct ITypeInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ITypeInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ITypeInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ITypeInfo* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeAttr(
            [In] ITypeInfo* This,
            [Out] TYPEATTR** ppTypeAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeComp(
            [In] ITypeInfo* This,
            [Out] ITypeComp** ppTComp = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFuncDesc(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] FUNCDESC** ppFuncDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetVarDesc(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] VARDESC** ppVarDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetNames(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, ComAliasName("BSTR[]")] char** rgBstrNames,
            [In, ComAliasName("UINT")] uint cMaxNames,
            [Out, ComAliasName("UINT")] uint* pcNames
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetRefTypeOfImplType(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out, ComAliasName("HREFTYPE")] uint* pRefType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetImplTypeFlags(
            [In] ITypeInfo* This,
            [In, ComAliasName("UINT")] uint index,
            [Out, ComAliasName("INT")] int* pImplTypeFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetIDsOfNames(
            [In] ITypeInfo* This,
            [In, ComAliasName("LPOLESTR[]")] char** rgszNames,
            [In, ComAliasName("UINT")] uint cNames,
            [Out, ComAliasName("MEMBERID")] int* pMemId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Invoke(
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
        public /* static */ delegate int _GetDocumentation(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDocString,
            [Out, ComAliasName("DWORD")] uint* pdwHelpContext,
            [Out, ComAliasName("BSTR")] char** pBstrHelpFile = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDllEntry(
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
        public /* static */ delegate int _GetRefTypeInfo(
            [In] ITypeInfo* This,
            [In, ComAliasName("HREFTYPE")] uint hRefType,
            [Out] ITypeInfo** ppTInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _AddressOfMember(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [In] INVOKEKIND invKind,
            [Out, ComAliasName("PVOID")] void** ppv
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateInstance(
            [In] ITypeInfo* This,
            [In] IUnknown* pUnkOuter,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out, ComAliasName("PVOID")] void** ppvObj
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMops(
            [In] ITypeInfo* This,
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, ComAliasName("BSTR")] char** pBstrMops = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetContainingTypeLib(
            [In] ITypeInfo* This,
            [Out] ITypeLib** ppTLib,
            [Out, ComAliasName("UINT")] uint* pIndex
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ReleaseTypeAttr(
            [In] ITypeInfo* This,
            [In] TYPEATTR* pTypeAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ReleaseFuncDesc(
            [In] ITypeInfo* This,
            [In] FUNCDESC* pFuncDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ReleaseVarDesc(
            [In] ITypeInfo* This,
            [In] VARDESC* pVarDesc
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ITypeInfo* This = &this)
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
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetTypeAttr(
            [Out] TYPEATTR** ppTypeAttr
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetTypeAttr>(lpVtbl->GetTypeAttr)(
                    This,
                    ppTypeAttr
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTypeComp(
            [Out] ITypeComp** ppTComp = null
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetTypeComp>(lpVtbl->GetTypeComp)(
                    This,
                    ppTComp
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFuncDesc(
            [In, ComAliasName("UINT")] uint index,
            [Out] FUNCDESC** ppFuncDesc
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetFuncDesc>(lpVtbl->GetFuncDesc)(
                    This,
                    index,
                    ppFuncDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetVarDesc(
            [In, ComAliasName("UINT")] uint index,
            [Out] VARDESC** ppVarDesc
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetVarDesc>(lpVtbl->GetVarDesc)(
                    This,
                    index,
                    ppVarDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetNames(
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, ComAliasName("BSTR[]")] char** rgBstrNames,
            [In, ComAliasName("UINT")] uint cMaxNames,
            [Out, ComAliasName("UINT")] uint* pcNames
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetNames>(lpVtbl->GetNames)(
                    This,
                    memid,
                    rgBstrNames,
                    cMaxNames,
                    pcNames
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetRefTypeOfImplType(
            [In, ComAliasName("UINT")] uint index,
            [Out, ComAliasName("HREFTYPE")] uint* pRefType
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetRefTypeOfImplType>(lpVtbl->GetRefTypeOfImplType)(
                    This,
                    index,
                    pRefType
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetImplTypeFlags(
            [In, ComAliasName("UINT")] uint index,
            [Out, ComAliasName("INT")] int* pImplTypeFlags
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetImplTypeFlags>(lpVtbl->GetImplTypeFlags)(
                    This,
                    index,
                    pImplTypeFlags
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetIDsOfNames(
            [In, ComAliasName("LPOLESTR[]")] char** rgszNames,
            [In, ComAliasName("UINT")] uint cNames,
            [Out, ComAliasName("MEMBERID")] int* pMemId
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetIDsOfNames>(lpVtbl->GetIDsOfNames)(
                    This,
                    rgszNames,
                    cNames,
                    pMemId
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Invoke(
            [In, ComAliasName("PVOID")] void* pvInstance,
            [In, ComAliasName("MEMBERID")] int memid,
            [In, ComAliasName("WORD")] ushort wFlags,
            [In, Out] DISPPARAMS* pDispParams,
            [Out] VARIANT* pVarResult,
            [Out] EXCEPINFO* pExcepInfo,
            [Out, ComAliasName("UINT")] uint* puArgErr
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_Invoke>(lpVtbl->Invoke)(
                    This,
                    pvInstance,
                    memid,
                    wFlags,
                    pDispParams,
                    pVarResult,
                    pExcepInfo,
                    puArgErr
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDocumentation(
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDocString,
            [Out, ComAliasName("DWORD")] uint* pdwHelpContext,
            [Out, ComAliasName("BSTR")] char** pBstrHelpFile = null
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetDocumentation>(lpVtbl->GetDocumentation)(
                    This,
                    memid,
                    pBstrName,
                    pBstrDocString,
                    pdwHelpContext,
                    pBstrHelpFile
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDllEntry(
            [In, ComAliasName("MEMBERID")] int memid,
            [In] INVOKEKIND invKind,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDllName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, ComAliasName("WORD")] ushort* pwOrdinal
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetDllEntry>(lpVtbl->GetDllEntry)(
                    This,
                    memid,
                    invKind,
                    pBstrDllName,
                    pBstrName,
                    pwOrdinal
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetRefTypeInfo(
            [In, ComAliasName("HREFTYPE")] uint hRefType,
            [Out] ITypeInfo** ppTInfo = null
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetRefTypeInfo>(lpVtbl->GetRefTypeInfo)(
                    This,
                    hRefType,
                    ppTInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int AddressOfMember(
            [In, ComAliasName("MEMBERID")] int memid,
            [In] INVOKEKIND invKind,
            [Out, ComAliasName("PVOID")] void** ppv
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_AddressOfMember>(lpVtbl->AddressOfMember)(
                    This,
                    memid,
                    invKind,
                    ppv
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateInstance(
            [In] IUnknown* pUnkOuter,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out, ComAliasName("PVOID")] void** ppvObj
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_CreateInstance>(lpVtbl->CreateInstance)(
                    This,
                    pUnkOuter,
                    riid,
                    ppvObj
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetMops(
            [In, ComAliasName("MEMBERID")] int memid,
            [Out, ComAliasName("BSTR")] char** pBstrMops = null
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetMops>(lpVtbl->GetMops)(
                    This,
                    memid,
                    pBstrMops
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetContainingTypeLib(
            [Out] ITypeLib** ppTLib,
            [Out, ComAliasName("UINT")] uint* pIndex
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                return MarshalFunction<_GetContainingTypeLib>(lpVtbl->GetContainingTypeLib)(
                    This,
                    ppTLib,
                    pIndex
                );
            }
        }

        public void ReleaseTypeAttr(
            [In] TYPEATTR* pTypeAttr
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                MarshalFunction<_ReleaseTypeAttr>(lpVtbl->ReleaseTypeAttr)(
                    This,
                    pTypeAttr
                );
            }
        }

        public void ReleaseFuncDesc(
            [In] FUNCDESC* pFuncDesc
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                MarshalFunction<_ReleaseFuncDesc>(lpVtbl->ReleaseFuncDesc)(
                    This,
                    pFuncDesc
                );
            }
        }

        public void ReleaseVarDesc(
            [In] VARDESC* pVarDesc
        )
        {
            fixed (ITypeInfo* This = &this)
            {
                MarshalFunction<_ReleaseVarDesc>(lpVtbl->ReleaseVarDesc)(
                    This,
                    pVarDesc
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

