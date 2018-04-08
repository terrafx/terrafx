// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("A06EB39A-50DA-425B-8C31-4EECD6C270F3")]
    public /* unmanaged */ unsafe struct ID3DDestructionNotifier
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID3DDestructionNotifier* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID3DDestructionNotifier* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID3DDestructionNotifier* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RegisterDestructionCallback(
            [In] ID3DDestructionNotifier* This,
            [In, ComAliasName("PFN_DESTRUCTION_CALLBACK")] IntPtr callbackFn,
            [In] void* pData,
            [Out, ComAliasName("UINT")] uint* pCallbackID
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _UnregisterDestructionCallback(
            [In] ID3DDestructionNotifier* This,
            [In, ComAliasName("UINT")] uint callbackID
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID3DDestructionNotifier* This = &this)
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
            fixed (ID3DDestructionNotifier* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID3DDestructionNotifier* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int RegisterDestructionCallback(
            [In, ComAliasName("PFN_DESTRUCTION_CALLBACK")] IntPtr callbackFn,
            [In] void* pData,
            [Out, ComAliasName("UINT")] uint* pCallbackID
        )
        {
            fixed (ID3DDestructionNotifier* This = &this)
            {
                return MarshalFunction<_RegisterDestructionCallback>(lpVtbl->RegisterDestructionCallback)(
                    This,
                    callbackFn,
                    pData,
                    pCallbackID
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int UnregisterDestructionCallback(
            [In, ComAliasName("UINT")] uint callbackID
        )
        {
            fixed (ID3DDestructionNotifier* This = &this)
            {
                return MarshalFunction<_UnregisterDestructionCallback>(lpVtbl->UnregisterDestructionCallback)(
                    This,
                    callbackID
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
            public IntPtr RegisterDestructionCallback;

            public IntPtr UnregisterDestructionCallback;
            #endregion
        }
        #endregion
    }
}

