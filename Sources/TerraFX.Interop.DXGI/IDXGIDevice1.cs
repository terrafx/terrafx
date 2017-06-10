// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("77DB970F-6276-48BA-BA28-070143B4392C")]
    unsafe public struct IDXGIDevice1
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetMaximumFrameLatency(
            [In] IDXGIDevice1* This,
            [In] uint MaxLatency
        );

        public /* static */ delegate HRESULT GetMaximumFrameLatency(
            [In] IDXGIDevice1* This,
            [Out] uint* pMaxLatency
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

            public SetMaximumFrameLatency SetMaximumFrameLatency;

            public GetMaximumFrameLatency GetMaximumFrameLatency;
            #endregion
        }
        #endregion
    }
}
