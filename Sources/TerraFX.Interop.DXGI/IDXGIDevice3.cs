// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("6007896C-3244-4AFD-BF18-A6D3BEDA5023")]
    unsafe public struct IDXGIDevice3
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void Trim(
            [In] IDXGIDevice3* This
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

            public Trim Trim;
            #endregion
        }
        #endregion
    }
}
