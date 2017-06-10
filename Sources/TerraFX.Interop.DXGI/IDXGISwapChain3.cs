// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("94D99BDB-F1F8-4AB0-B236-7DA0170EDAB1")]
    unsafe public struct IDXGISwapChain3
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate uint GetCurrentBackBufferIndex(
            [In] IDXGISwapChain3* This
        );

        public /* static */ delegate HRESULT CheckColorSpaceSupport(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [Out] uint* pColorSpaceSupport
        );

        public /* static */ delegate HRESULT SetColorSpace1(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace
        );

        public /* static */ delegate HRESULT ResizeBuffers1(
            [In] IDXGISwapChain3* This,
            [In] uint BufferCount,
            [In] uint Width,
            [In] uint Height,
            [In] DXGI_FORMAT Format,
            [In] DXGI_SWAP_CHAIN_FLAG SwapChainFlags,
            [In] uint* pCreationNodeMask,
            [In] IUnknown** ppPresentQueue
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

            public IDXGISwapChain.Present Present;

            public IDXGISwapChain.GetBuffer GetBuffer;

            public IDXGISwapChain.SetFullscreenState SetFullscreenState;

            public IDXGISwapChain.GetFullscreenState GetFullscreenState;

            public IDXGISwapChain.GetDesc GetDesc;

            public IDXGISwapChain.ResizeBuffers ResizeBuffers;

            public IDXGISwapChain.ResizeTarget ResizeTarget;

            public IDXGISwapChain.GetContainingOutput GetContainingOutput;

            public IDXGISwapChain.GetFrameStatistics GetFrameStatistics;

            public IDXGISwapChain.GetLastPresentCount GetLastPresentCount;

            public IDXGISwapChain1.GetDesc1 GetDesc1;

            public IDXGISwapChain1.GetFullscreenDesc GetFullscreenDesc;

            public IDXGISwapChain1.GetHwnd GetHwnd;

            public IDXGISwapChain1.GetCoreWindow GetCoreWindow;

            public IDXGISwapChain1.Present1 Present1;

            public IDXGISwapChain1.IsTemporaryMonoSupported IsTemporaryMonoSupported;

            public IDXGISwapChain1.GetRestrictToOutput GetRestrictToOutput;

            public IDXGISwapChain1.SetBackgroundColor SetBackgroundColor;

            public IDXGISwapChain1.GetBackgroundColor GetBackgroundColor;

            public IDXGISwapChain1.SetRotation SetRotation;

            public IDXGISwapChain1.GetRotation GetRotation;

            public IDXGISwapChain2.SetSourceSize SetSourceSize;

            public IDXGISwapChain2.GetSourceSize GetSourceSize;

            public IDXGISwapChain2.SetMaximumFrameLatency SetMaximumFrameLatency;

            public IDXGISwapChain2.GetMaximumFrameLatency GetMaximumFrameLatency;

            public IDXGISwapChain2.GetFrameLatencyWaitableObject GetFrameLatencyWaitableObject;

            public IDXGISwapChain2.SetMatrixTransform SetMatrixTransform;

            public IDXGISwapChain2.GetMatrixTransform GetMatrixTransform;

            public GetCurrentBackBufferIndex GetCurrentBackBufferIndex;

            public CheckColorSpaceSupport CheckColorSpaceSupport;

            public SetColorSpace1 SetColorSpace1;

            public ResizeBuffers1 ResizeBuffers1;
            #endregion
        }
        #endregion
    }
}
