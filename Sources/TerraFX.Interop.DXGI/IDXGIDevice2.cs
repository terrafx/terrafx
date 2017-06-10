// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("05008617-FBFD-4051-A790-144884B4F6A9")]
    unsafe public struct IDXGIDevice2
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT OfferResources(
            [In] IDXGIDevice2* This,
            [In] uint NumResources,
            [In] IDXGIResource** ppResources,
            [In] DXGI_OFFER_RESOURCE_PRIORITY Priority
        );

        public /* static */ delegate HRESULT ReclaimResources(
            [In] IDXGIDevice2* This,
            [In] uint NumResources,
            [In] IDXGIResource** ppResources,
            [Out, Optional] BOOL* pDiscarded
        );

        public /* static */ delegate HRESULT EnqueueSetEvent(
            [In] IDXGIDevice2* This,
            [In] HANDLE hEvent
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

            public OfferResources OfferResources;

            public ReclaimResources ReclaimResources;

            public EnqueueSetEvent EnqueueSetEvent;
            #endregion
        }
        #endregion
    }
}
