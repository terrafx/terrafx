// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("A8BE2AC4-199F-4946-B331-79599FB98DE7")]
    unsafe public struct IDXGISwapChain2
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetSourceSize(
            [In] IDXGISwapChain2* This,
            [In] uint Width,
            [In] uint Height
        );

        public /* static */ delegate HRESULT GetSourceSize(
            [In] IDXGISwapChain2* This,
            [Out] uint* pWidth,
            [Out] uint* pHeight
        );

        public /* static */ delegate HRESULT SetMaximumFrameLatency(
            [In] IDXGISwapChain2* This,
            [In] uint MaxLatency
        );

        public /* static */ delegate HRESULT GetMaximumFrameLatency(
            [In] IDXGISwapChain2* This,
            [Out] uint* pMaxLatency
        );

        public /* static */ delegate HANDLE GetFrameLatencyWaitableObject(
            [In] IDXGISwapChain2* This
        );

        public /* static */ delegate HRESULT SetMatrixTransform(
            [In] IDXGISwapChain2* This,
            [In] DXGI_MATRIX_3X2_F* pMatrix
        );

        public /* static */ delegate HRESULT GetMatrixTransform(
            [In] IDXGISwapChain2* This,
            [Out] DXGI_MATRIX_3X2_F* pMatrix
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

            public SetSourceSize SetSourceSize;

            public GetSourceSize GetSourceSize;

            public SetMaximumFrameLatency SetMaximumFrameLatency;

            public GetMaximumFrameLatency GetMaximumFrameLatency;

            public GetFrameLatencyWaitableObject GetFrameLatencyWaitableObject;

            public SetMatrixTransform SetMatrixTransform;

            public GetMatrixTransform GetMatrixTransform;
            #endregion
        }
        #endregion
    }
}
