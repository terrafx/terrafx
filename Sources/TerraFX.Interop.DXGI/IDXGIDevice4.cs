// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("95B4F95F-D8DA-4CA4-9EE6-3B76D5968A10")]
    unsafe public struct IDXGIDevice4
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT OfferResources1(
            [In] IDXGIDevice4* This,
            [In] uint NumResources,
            [In] IDXGIResource** ppResources,
            [In] DXGI_OFFER_RESOURCE_PRIORITY Priority,
            [In] DXGI_OFFER_RESOURCE_FLAGS Flags
        );

        public /* static */ delegate HRESULT ReclaimResources1(
            [In] IDXGIDevice4* This,
            [In] uint NumResources,
            [In] IDXGIResource** ppResources,
            [Out] DXGI_RECLAIM_RESOURCE_RESULTS* pResults
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

            public IDXGIDevice.GetAdapter GetAdapter;

            public IDXGIDevice.CreateSurface CreateSurface;

            public IDXGIDevice.QueryResourceResidency QueryResourceResidency;

            public IDXGIDevice.SetGPUThreadPriority SetGPUThreadPriority;

            public IDXGIDevice.GetGPUThreadPriority GetGPUThreadPriority;

            public IDXGIDevice1.SetMaximumFrameLatency SetMaximumFrameLatency;

            public IDXGIDevice1.GetMaximumFrameLatency GetMaximumFrameLatency;

            public IDXGIDevice2.OfferResources OfferResources;

            public IDXGIDevice2.ReclaimResources ReclaimResources;

            public IDXGIDevice2.EnqueueSetEvent EnqueueSetEvent;

            public IDXGIDevice3.Trim Trim;

            public OfferResources1 OfferResources1;

            public ReclaimResources1 ReclaimResources1;
            #endregion
        }
        #endregion
    }
}
