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
    [Guid("00020400-0000-0000-C000-000000000046")]
    [Unmanaged]
    public unsafe struct IDispatch
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDispatch* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDispatch* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDispatch* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeInfoCount(
            [In] IDispatch* This,
            [Out, ComAliasName("UINT")] uint* pctinfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTypeInfo(
            [In] IDispatch* This,
            [In, ComAliasName("UINT")] uint iTInfo,
            [In, ComAliasName("LCID")] uint lcid,
            [Out] ITypeInfo** ppTInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetIDsOfNames(
            [In] IDispatch* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [In, ComAliasName("LPOLESTR[]")] char** rgszNames,
            [In, ComAliasName("UINT")] uint cNames,
            [In, ComAliasName("LCID")] uint lcid,
            [Out, ComAliasName("DISPID[]")] int* rgDispId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Invoke(
            [In] IDispatch* This,
            [In, ComAliasName("DISPID")] int dispIdMember,
            [In, ComAliasName("REFIID")] Guid* riid,
            [In, ComAliasName("LCID")] uint lcid,
            [In, ComAliasName("WORD")] ushort wFlags,
            [In, ComAliasName("DISPPARAMS[]")] DISPPARAMS* pDispParams,
            [Out] VARIANT* pVarResult = null,
            [Out] EXCEPINFO* pExcepInfo = null,
            [Out, ComAliasName("UINT")] uint* puArgErr = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDispatch* This = &this)
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
            fixed (IDispatch* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDispatch* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetTypeInfoCount(
            [Out, ComAliasName("UINT")] uint* pctinfo
        )
        {
            fixed (IDispatch* This = &this)
            {
                return MarshalFunction<_GetTypeInfoCount>(lpVtbl->GetTypeInfoCount)(
                    This,
                    pctinfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTypeInfo(
            [In, ComAliasName("UINT")] uint iTInfo,
            [In, ComAliasName("LCID")] uint lcid,
            [Out] ITypeInfo** ppTInfo = null
        )
        {
            fixed (IDispatch* This = &this)
            {
                return MarshalFunction<_GetTypeInfo>(lpVtbl->GetTypeInfo)(
                    This,
                    iTInfo,
                    lcid,
                    ppTInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetIDsOfNames(
            [In, ComAliasName("REFIID")] Guid* riid,
            [In, ComAliasName("LPOLESTR[]")] char** rgszNames,
            [In, ComAliasName("UINT")] uint cNames,
            [In, ComAliasName("LCID")] uint lcid,
            [Out, ComAliasName("DISPID[]")] int* rgDispId
        )
        {
            fixed (IDispatch* This = &this)
            {
                return MarshalFunction<_GetIDsOfNames>(lpVtbl->GetIDsOfNames)(
                    This,
                    riid,
                    rgszNames,
                    cNames,
                    lcid,
                    rgDispId
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Invoke(
            [In, ComAliasName("DISPID")] int dispIdMember,
            [In, ComAliasName("REFIID")] Guid* riid,
            [In, ComAliasName("LCID")] uint lcid,
            [In, ComAliasName("WORD")] ushort wFlags,
            [In, ComAliasName("DISPPARAMS[]")] DISPPARAMS* pDispParams,
            [Out] VARIANT* pVarResult = null,
            [Out] EXCEPINFO* pExcepInfo = null,
            [Out, ComAliasName("UINT")] uint* puArgErr = null
        )
        {
            fixed (IDispatch* This = &this)
            {
                return MarshalFunction<_Invoke>(lpVtbl->Invoke)(
                    This,
                    dispIdMember,
                    riid,
                    lcid,
                    wFlags,
                    pDispParams,
                    pVarResult,
                    pExcepInfo,
                    puArgErr
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

            public IntPtr GetIDsOfNames;

            public IntPtr Invoke;
            #endregion
        }
        #endregion
    }
}
