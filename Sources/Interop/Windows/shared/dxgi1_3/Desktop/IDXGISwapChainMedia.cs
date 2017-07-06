// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.Desktop
{
    [Guid("DD95B90B-F05F-4F6A-BD65-25BFB264BD84")]
    unsafe public /* blittable */ struct IDXGISwapChainMedia
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFrameStatisticsMedia(
            [In] IDXGISwapChainMedia* This,
            [Out] DXGI_FRAME_STATISTICS_MEDIA* pStats
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetPresentDuration(
            [In] IDXGISwapChainMedia* This,
            [In] UINT Duration
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CheckPresentDurationSupport(
            [In] IDXGISwapChainMedia* This,
            [In] UINT DesiredPresentDuration,
            [Out] UINT* pClosestSmallerPresentDuration,
            [Out] UINT* pClosestLargerPresentDuration
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetFrameStatisticsMedia GetFrameStatisticsMedia;

            public SetPresentDuration SetPresentDuration;

            public CheckPresentDurationSupport CheckPresentDurationSupport;
            #endregion
        }
        #endregion
    }
}
