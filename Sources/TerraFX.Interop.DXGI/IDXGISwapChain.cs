// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("310D36A0-D2E7-4C0A-AA04-6A9D23B8886A")]
    unsafe public struct IDXGISwapChain
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Present(
            [In] IDXGISwapChain* This,
            [In] uint SyncInterval,
            [In] DXGI_PRESENT_FLAG Flags
        );

        public /* static */ delegate HRESULT GetBuffer(
            [In] IDXGISwapChain* This,
            [In] uint Buffer,
            [In] Guid* riid,
            [In, Out] void** ppSurface
        );

        public /* static */ delegate HRESULT SetFullscreenState(
            [In] IDXGISwapChain* This,
            [In] BOOL Fullscreen,
            [In, Optional] IDXGIOutput* pTarget
        );

        public /* static */ delegate HRESULT GetFullscreenState(
            [In] IDXGISwapChain* This,
            [Out, Optional] BOOL* pFullscreen,
            [Out, Optional] IDXGIOutput** ppTarget
        );

        public /* static */ delegate HRESULT GetDesc(
            [In] IDXGISwapChain* This,
            [Out] DXGI_SWAP_CHAIN_DESC* pDesc
        );

        public /* static */ delegate HRESULT ResizeBuffers(
            [In] IDXGISwapChain* This,
            [In] uint BufferCount,
            [In] uint Width,
            [In] uint Height,
            [In] DXGI_FORMAT NewFormat,
            [In] DXGI_SWAP_CHAIN_FLAG SwapChainFlags
        );

        public /* static */ delegate HRESULT ResizeTarget(
            [In] IDXGISwapChain* This,
            [In] DXGI_MODE_DESC* pNewTargetParameters
        );

        public /* static */ delegate HRESULT GetContainingOutput(
            [In] IDXGISwapChain* This,
            [Out] IDXGIOutput** ppOutput
        );

        public /* static */ delegate HRESULT GetFrameStatistics(
            [In] IDXGISwapChain* This,
            [Out] DXGI_FRAME_STATISTICS* pStats
        );

        public /* static */ delegate HRESULT GetLastPresentCount(
            [In] IDXGISwapChain* This,
            [Out] uint* pLastPresentCount
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.QueryInterface QueryInterface;

            public IUnknown.AddRef AddRef;

            public IUnknown.Release Release;

            public IDXGIObject.SetPrivateData SetPrivateData;

            public IDXGIObject.SetPrivateDataInterface SetPrivateDataInterface;

            public IDXGIObject.GetPrivateData GetPrivateData;

            public IDXGIObject.GetParent GetParent;

            public IDXGIDeviceSubObject.GetDevice GetDevice;

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
