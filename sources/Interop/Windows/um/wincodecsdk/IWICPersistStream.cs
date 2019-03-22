// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("00675040-6908-45F8-86A3-49C7DFD6D9AD")]
    [Unmanaged]
    public unsafe struct IWICPersistStream
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICPersistStream* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICPersistStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICPersistStream* This
        );
        #endregion

        #region IPersist Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetClassID(
            [In] IWICPersistStream* This,
            [Out, ComAliasName("CLSID")] Guid* pClassID
        );
        #endregion

        #region IPersistStream Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _IsDirty(
            [In] IWICPersistStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Load(
            [In] IWICPersistStream* This,
            [In] IStream* pStm = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Save(
            [In] IWICPersistStream* This,
            [In, Optional] IStream* pStm,
            [In, ComAliasName("BOOL")] int fClearDirty
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSizeMax(
            [In] IWICPersistStream* This,
            [Out] ULARGE_INTEGER* pcbSize
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _LoadEx(
            [In] IWICPersistStream* This,
            [In, Optional] IStream* pIStream,
            [In, Optional, ComAliasName("GUID")] Guid* pguidPreferredVendor,
            [In, ComAliasName("DWORD")] uint dwPersistOptions
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SaveEx(
            [In] IWICPersistStream* This,
            [In, Optional] IStream* pIStream,
            [In, ComAliasName("DWORD")] uint dwPersistOptions,
            [In, ComAliasName("BOOL")] int fClearDirty
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICPersistStream* This = &this)
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
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IPersist Methods
        [return: ComAliasName("HRESULT")]
        public int GetClassID(
            [Out, ComAliasName("CLSID")] Guid* pClassID
        )
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_GetClassID>(lpVtbl->GetClassID)(
                    This,
                    pClassID
                );
            }
        }
        #endregion

        #region IPersistStream Methods
        [return: ComAliasName("HRESULT")]
        public int IsDirty()
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_IsDirty>(lpVtbl->IsDirty)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Load(
            [In] IStream* pStm = null
        )
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_Load>(lpVtbl->Load)(
                    This,
                    pStm
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Save(
            [In, Optional] IStream* pStm,
            [In, ComAliasName("BOOL")] int fClearDirty
        )
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_Save>(lpVtbl->Save)(
                    This,
                    pStm,
                    fClearDirty
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSizeMax(
            [Out] ULARGE_INTEGER* pcbSize
        )
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_GetSizeMax>(lpVtbl->GetSizeMax)(
                    This,
                    pcbSize
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int LoadEx(
            [In, Optional] IStream* pIStream,
            [In, Optional, ComAliasName("GUID")] Guid* pguidPreferredVendor,
            [In, ComAliasName("DWORD")] uint dwPersistOptions
        )
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_LoadEx>(lpVtbl->LoadEx)(
                    This,
                    pIStream,
                    pguidPreferredVendor,
                    dwPersistOptions
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SaveEx(
            [In, Optional] IStream* pIStream,
            [In, ComAliasName("DWORD")] uint dwPersistOptions,
            [In, ComAliasName("BOOL")] int fClearDirty
        )
        {
            fixed (IWICPersistStream* This = &this)
            {
                return MarshalFunction<_SaveEx>(lpVtbl->SaveEx)(
                    This,
                    pIStream,
                    dwPersistOptions,
                    fClearDirty
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

            #region IPersist Fields
            public IntPtr GetClassID;
            #endregion

            #region IPersistStream Fields
            public IntPtr IsDirty;

            public IntPtr Load;

            public IntPtr Save;

            public IntPtr GetSizeMax;
            #endregion

            #region Fields
            public IntPtr LoadEx;

            public IntPtr SaveEx;
            #endregion
        }
        #endregion
    }
}

