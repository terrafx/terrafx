// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("3D585D5A-BD4A-489E-B1F4-3DBCB6452FFB")]
    unsafe public struct IDXGISwapChain4
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetHDRMetaData(
            [In] IDXGISwapChain4* This,
            [In] DXGI_HDR_METADATA_TYPE Type,
            [In] uint Size,
            [In] void* pMetaData
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

            public IDXGISwapChain3.GetCurrentBackBufferIndex GetCurrentBackBufferIndex;

            public IDXGISwapChain3.CheckColorSpaceSupport CheckColorSpaceSupport;

            public IDXGISwapChain3.SetColorSpace1 SetColorSpace1;

            public IDXGISwapChain3.ResizeBuffers1 ResizeBuffers1;

            public SetHDRMetaData SetHDRMetaData;
            #endregion
        }
        #endregion
    }
}
