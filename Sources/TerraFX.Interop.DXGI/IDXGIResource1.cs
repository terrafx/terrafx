// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("30961379-4609-4A41-998E-54FE567EE0C1")]
    unsafe public struct IDXGIResource1
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CreateSubresourceSurface(
            [In] IDXGIResource1* This,
            [In] uint index,
            [Out] IDXGISurface2** ppSurface
        );

        public /* static */ delegate HRESULT CreateSharedHandle(
            [In] IDXGIResource1* This,
            [In, Optional] SECURITY_ATTRIBUTES* pAttributes,
            [In] uint dwAccess,
            [In, Optional] ushort* lpName,
            [Out] HANDLE pHandle
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

            public IDXGIResource.GetSharedHandle GetSharedHandle;

            public IDXGIResource.GetUsage GetUsage;

            public IDXGIResource.SetEvictionPriority SetEvictionPriority;

            public IDXGIResource.GetEvictionPriority GetEvictionPriority;

            public CreateSubresourceSurface CreateSubresourceSurface;

            public CreateSharedHandle CreateSharedHandle;
            #endregion
        }
        #endregion
    }
}
