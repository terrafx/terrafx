// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("035F3AB4-482E-4E50-B41F-8A7F8BD8960B")]
    unsafe public struct IDXGIResource
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetSharedHandle(
            [In] IDXGIResource* This,
            [Out] HANDLE* pSharedHandle
        );

        public /* static */ delegate HRESULT GetUsage(
            [In] IDXGIResource* This,
            [Out] DXGI_USAGE* pUsage
        );

        public /* static */ delegate HRESULT SetEvictionPriority(
            [In] IDXGIResource* This,
            [In] DXGI_RESOURCE_PRIORITY EvictionPriority
        );

        public /* static */ delegate HRESULT GetEvictionPriority(
            [In] IDXGIResource* This,
            [Out] DXGI_RESOURCE_PRIORITY* pEvictionPriority
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIDeviceSubObject.Vtbl BaseVtbl;

            public GetSharedHandle GetSharedHandle;

            public GetUsage GetUsage;

            public SetEvictionPriority SetEvictionPriority;

            public GetEvictionPriority GetEvictionPriority;
            #endregion
        }
        #endregion
    }
}
