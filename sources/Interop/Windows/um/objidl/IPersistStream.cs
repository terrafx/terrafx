// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("00000109-0000-0000-C000-000000000046")]
    [Unmanaged]
    public unsafe struct IPersistStream
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IPersistStream* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IPersistStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IPersistStream* This
        );
        #endregion

        #region IPersist Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetClassID(
            [In] IPersistStream* This,
            [Out, ComAliasName("CLSID")] Guid* pClassID
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _IsDirty(
            [In] IPersistStream* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Load(
            [In] IPersistStream* This,
            [In] IStream* pStm = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Save(
            [In] IPersistStream* This,
            [In, Optional] IStream* pStm,
            [In, ComAliasName("BOOL")] int fClearDirty
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSizeMax(
            [In] IPersistStream* This,
            [Out] ULARGE_INTEGER* pcbSize
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IPersistStream* This = &this)
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
            fixed (IPersistStream* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IPersistStream* This = &this)
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
            fixed (IPersistStream* This = &this)
            {
                return MarshalFunction<_GetClassID>(lpVtbl->GetClassID)(
                    This,
                    pClassID
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int IsDirty()
        {
            fixed (IPersistStream* This = &this)
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
            fixed (IPersistStream* This = &this)
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
            fixed (IPersistStream* This = &this)
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
            fixed (IPersistStream* This = &this)
            {
                return MarshalFunction<_GetSizeMax>(lpVtbl->GetSizeMax)(
                    This,
                    pcbSize
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

            #region Fields
            public IntPtr IsDirty;

            public IntPtr Load;

            public IntPtr Save;

            public IntPtr GetSizeMax;
            #endregion
        }
        #endregion
    }
}

