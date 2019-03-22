// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("09E0BF36-54AC-484F-8847-4BAEEAB6053F")]
    [Unmanaged]
    public unsafe struct ID3D12DebugCommandList
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID3D12DebugCommandList* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID3D12DebugCommandList* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID3D12DebugCommandList* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate int _AssertResourceState(
            [In] ID3D12DebugCommandList* This,
            [In] ID3D12Resource* pResource,
            [In, ComAliasName("UINT")] uint Subresource,
            [In, ComAliasName("UINT")] uint State
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetFeatureMask(
            [In] ID3D12DebugCommandList* This,
            [In] D3D12_DEBUG_FEATURE Mask
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D3D12_DEBUG_FEATURE _GetFeatureMask(
            [In] ID3D12DebugCommandList* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID3D12DebugCommandList* This = &this)
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
            fixed (ID3D12DebugCommandList* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID3D12DebugCommandList* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        public int AssertResourceState(
            [In] ID3D12Resource* pResource,
            [In, ComAliasName("UINT")] uint Subresource,
            [In, ComAliasName("UINT")] uint State
        )
        {
            fixed (ID3D12DebugCommandList* This = &this)
            {
                return MarshalFunction<_AssertResourceState>(lpVtbl->AssertResourceState)(
                    This,
                    pResource,
                    Subresource,
                    State
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetFeatureMask(
            [In] D3D12_DEBUG_FEATURE Mask
        )
        {
            fixed (ID3D12DebugCommandList* This = &this)
            {
                return MarshalFunction<_SetFeatureMask>(lpVtbl->SetFeatureMask)(
                    This,
                    Mask
                );
            }
        }

        public D3D12_DEBUG_FEATURE GetFeatureMask()
        {
            fixed (ID3D12DebugCommandList* This = &this)
            {
                return MarshalFunction<_GetFeatureMask>(lpVtbl->GetFeatureMask)(
                    This
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
            public IntPtr AssertResourceState;

            public IntPtr SetFeatureMask;

            public IntPtr GetFeatureMask;
            #endregion
        }
        #endregion
    }
}
