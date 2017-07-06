// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("310D36A0-D2E7-4C0A-AA04-6A9D23B8886A")]
    unsafe public /* blittable */ struct IDXGISwapChain
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Present(
            [In] IDXGISwapChain* This,
            [In] UINT SyncInterval,
            [In] UINT Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetBuffer(
            [In] IDXGISwapChain* This,
            [In] UINT Buffer,
            [In] REFIID riid,
            [In, Out] void** ppSurface
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetFullscreenState(
            [In] IDXGISwapChain* This,
            [In] BOOL Fullscreen,
            [In, Optional] IDXGIOutput* pTarget
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFullscreenState(
            [In] IDXGISwapChain* This,
            [Out, Optional] BOOL* pFullscreen,
            [Out, Optional] IDXGIOutput** ppTarget
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDesc(
            [In] IDXGISwapChain* This,
            [Out] DXGI_SWAP_CHAIN_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ResizeBuffers(
            [In] IDXGISwapChain* This,
            [In] UINT BufferCount,
            [In] UINT Width,
            [In] UINT Height,
            [In] DXGI_FORMAT NewFormat,
            [In] UINT SwapChainFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ResizeTarget(
            [In] IDXGISwapChain* This,
            [In] /* readonly */ DXGI_MODE_DESC* pNewTargetParameters
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetContainingOutput(
            [In] IDXGISwapChain* This,
            [Out] IDXGIOutput** ppOutput
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFrameStatistics(
            [In] IDXGISwapChain* This,
            [Out] DXGI_FRAME_STATISTICS* pStats
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetLastPresentCount(
            [In] IDXGISwapChain* This,
            [Out] UINT* pLastPresentCount
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIDeviceSubObject.Vtbl BaseVtbl;

            public Present Present;

            public GetBuffer GetBuffer;

            public SetFullscreenState SetFullscreenState;

            public GetFullscreenState GetFullscreenState;

            public GetDesc GetDesc;

            public ResizeBuffers ResizeBuffers;

            public ResizeTarget ResizeTarget;

            public GetContainingOutput GetContainingOutput;

            public GetFrameStatistics GetFrameStatistics;

            public GetLastPresentCount GetLastPresentCount;
            #endregion
        }
        #endregion
    }
}
