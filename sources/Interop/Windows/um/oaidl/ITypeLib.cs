// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("00020402-0000-0000-C000-000000000046")]
    [Unmanaged]
    public unsafe struct ITypeLib
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ITypeLib* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ITypeLib* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ITypeLib* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate uint _GetTypeInfoCount(
            [In] ITypeLib* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeInfo(
            [In] ITypeLib* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] ITypeInfo** ppTInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeInfoType(
            [In] ITypeLib* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] TYPEKIND* pTKind
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeInfoOfGuid(
            [In] ITypeLib* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [Out] ITypeInfo** ppTinfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetLibAttr(
            [In] ITypeLib* This,
            [Out] TLIBATTR** ppTLibAttr
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeComp(
            [In] ITypeLib* This,
            [Out] ITypeComp** ppTComp = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDocumentation(
            [In] ITypeLib* This,
            [In, ComAliasName("INT")] int index,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDocString,
            [Out, ComAliasName("DWORD")] uint* pdwHelpContext,
            [Out, ComAliasName("BSTR")] char** pBstrHelpFile = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _IsName(
            [In] ITypeLib* This,
            [In, Out, ComAliasName("LPOLESTR")] char* szNameBuf,
            [In, ComAliasName("ULONG")] uint lHashVal,
            [Out, ComAliasName("BOOL")] int* pfName
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _FindName(
            [In] ITypeLib* This,
            [In, Out, ComAliasName("LPOLESTR")] char* szNameBuf,
            [In, ComAliasName("ULONG")] uint lHashVal,
            [Out] ITypeInfo** ppTInfo,
            [Out, ComAliasName("MEMBERID")] int* rgMemId,
            [In, Out, ComAliasName("USHORT")] ushort* pcFound
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ReleaseTLibAttr(
            [In] ITypeLib* This,
            [In] TLIBATTR* pTLibAttr
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ITypeLib* This = &this)
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
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        public uint GetTypeInfoCount()
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_GetTypeInfoCount>(lpVtbl->GetTypeInfoCount)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTypeInfo(
            [In, ComAliasName("UINT")] uint index,
            [Out] ITypeInfo** ppTInfo = null
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_GetTypeInfo>(lpVtbl->GetTypeInfo)(
                    This,
                    index,
                    ppTInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTypeInfoType(
            [In, ComAliasName("UINT")] uint index,
            [Out] TYPEKIND* pTKind
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_GetTypeInfoType>(lpVtbl->GetTypeInfoType)(
                    This,
                    index,
                    pTKind
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTypeInfoOfGuid(
            [In, ComAliasName("REFGUID")] Guid* guid,
            [Out] ITypeInfo** ppTinfo = null
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_GetTypeInfoOfGuid>(lpVtbl->GetTypeInfoOfGuid)(
                    This,
                    guid,
                    ppTinfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetLibAttr(
            [Out] TLIBATTR** ppTLibAttr
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_GetLibAttr>(lpVtbl->GetLibAttr)(
                    This,
                    ppTLibAttr
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTypeComp(
            [Out] ITypeComp** ppTComp = null
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_GetTypeComp>(lpVtbl->GetTypeComp)(
                    This,
                    ppTComp
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDocumentation(
            [In, ComAliasName("INT")] int index,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrName,
            [Out, Optional, ComAliasName("BSTR")] char** pBstrDocString,
            [Out, ComAliasName("DWORD")] uint* pdwHelpContext,
            [Out, ComAliasName("BSTR")] char** pBstrHelpFile = null
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_GetDocumentation>(lpVtbl->GetDocumentation)(
                    This,
                    index,
                    pBstrName,
                    pBstrDocString,
                    pdwHelpContext,
                    pBstrHelpFile
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int IsName(
            [In, Out, ComAliasName("LPOLESTR")] char* szNameBuf,
            [In, ComAliasName("ULONG")] uint lHashVal,
            [Out, ComAliasName("BOOL")] int* pfName
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_IsName>(lpVtbl->IsName)(
                    This,
                    szNameBuf,
                    lHashVal,
                    pfName
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int FindName(
            [In, Out, ComAliasName("LPOLESTR")] char* szNameBuf,
            [In, ComAliasName("ULONG")] uint lHashVal,
            [Out] ITypeInfo** ppTInfo,
            [Out, ComAliasName("MEMBERID")] int* rgMemId,
            [In, Out, ComAliasName("USHORT")] ushort* pcFound
        )
        {
            fixed (ITypeLib* This = &this)
            {
                return MarshalFunction<_FindName>(lpVtbl->FindName)(
                    This,
                    szNameBuf,
                    lHashVal,
                    ppTInfo,
                    rgMemId,
                    pcFound
                );
            }
        }

        public void ReleaseTLibAttr(
            [In] TLIBATTR* pTLibAttr
        )
        {
            fixed (ITypeLib* This = &this)
            {
                MarshalFunction<_ReleaseTLibAttr>(lpVtbl->ReleaseTLibAttr)(
                    This,
                    pTLibAttr
                );
            }
        }
        #endregion

        #region Structs
        [Unmanaged]
        public struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
            public IntPtr GetTypeInfoCount;

            public IntPtr GetTypeInfo;

            public IntPtr GetTypeInfoType;

            public IntPtr GetTypeInfoOfGuid;

            public IntPtr GetLibAttr;

            public IntPtr GetTypeComp;

            public IntPtr GetDocumentation;

            public IntPtr IsName;

            public IntPtr FindName;

            public IntPtr ReleaseTLibAttr;
            #endregion
        }
        #endregion
    }
}
