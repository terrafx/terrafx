// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("54EC77FA-1377-44E6-8C32-88FD5F44C84C")]
    unsafe public struct IDXGIDevice
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetAdapter(
            [In] IDXGIDevice* This,
            [Out] IDXGIAdapter** pAdapter
        );

        public /* static */ delegate HRESULT CreateSurface(
            [In] IDXGIDevice* This,
            [In] DXGI_SURFACE_DESC* pDesc,
            [In] uint NumSurfaces,
            [In] DXGI_USAGE Usage,
            [In, Optional] DXGI_SHARED_RESOURCE* pSharedResource,
            [Out] IDXGISurface** ppSurface
        );

        public /* static */ delegate HRESULT QueryResourceResidency(
            [In] IDXGIDevice* This,
            [In] IUnknown** ppResources,
            [Out] DXGI_RESIDENCY* pResidencyStatus,
            [In] uint NumResources
        );

        public /* static */ delegate HRESULT SetGPUThreadPriority(
            [In] IDXGIDevice* This,
            [In] int Priority
        );

        public /* static */ delegate HRESULT GetGPUThreadPriority(
            [In] IDXGIDevice* This,
            [Out] int* pPriority
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

            public GetAdapter GetAdapter;

            public CreateSurface CreateSurface;

            public QueryResourceResidency QueryResourceResidency;

            public SetGPUThreadPriority SetGPUThreadPriority;

            public GetGPUThreadPriority GetGPUThreadPriority;
            #endregion
        }
        #endregion
    }
}
