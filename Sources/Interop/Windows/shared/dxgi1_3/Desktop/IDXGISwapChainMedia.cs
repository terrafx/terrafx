// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop.Desktop
{
    [Guid("DD95B90B-F05F-4F6A-BD65-25BFB264BD84")]
    public /* blittable */ unsafe struct IDXGISwapChainMedia
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGISwapChainMedia* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGISwapChainMedia* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGISwapChainMedia* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrameStatisticsMedia(
            [In] IDXGISwapChainMedia* This,
            [Out] DXGI_FRAME_STATISTICS_MEDIA* pStats
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPresentDuration(
            [In] IDXGISwapChainMedia* This,
            [In, ComAliasName("UINT")] uint Duration
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CheckPresentDurationSupport(
            [In] IDXGISwapChainMedia* This,
            [In, ComAliasName("UINT")] uint DesiredPresentDuration,
            [Out, ComAliasName("UINT")] uint* pClosestSmallerPresentDuration,
            [Out, ComAliasName("UINT")] uint* pClosestLargerPresentDuration
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGISwapChainMedia* This = &this)
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
            fixed (IDXGISwapChainMedia* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGISwapChainMedia* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetFrameStatisticsMedia(
            [Out] DXGI_FRAME_STATISTICS_MEDIA* pStats
        )
        {
            fixed (IDXGISwapChainMedia* This = &this)
            {
                return MarshalFunction<_GetFrameStatisticsMedia>(lpVtbl->GetFrameStatisticsMedia)(
                    This,
                    pStats
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPresentDuration(
            [In, ComAliasName("UINT")] uint Duration
        )
        {
            fixed (IDXGISwapChainMedia* This = &this)
            {
                return MarshalFunction<_SetPresentDuration>(lpVtbl->SetPresentDuration)(
                    This,
                    Duration
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CheckPresentDurationSupport(
            [In, ComAliasName("UINT")] uint DesiredPresentDuration,
            [Out, ComAliasName("UINT")] uint* pClosestSmallerPresentDuration,
            [Out, ComAliasName("UINT")] uint* pClosestLargerPresentDuration
        )
        {
            fixed (IDXGISwapChainMedia* This = &this)
            {
                return MarshalFunction<_CheckPresentDurationSupport>(lpVtbl->CheckPresentDurationSupport)(
                    This,
                    DesiredPresentDuration,
                    pClosestSmallerPresentDuration,
                    pClosestLargerPresentDuration
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
            public IntPtr GetFrameStatisticsMedia;

            public IntPtr SetPresentDuration;

            public IntPtr CheckPresentDurationSupport;
            #endregion
        }
        #endregion
    }
}

