// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("ABA496DD-B617-4CB8-A866-BC44D7EB1FA2")]
    unsafe public struct IDXGISurface2
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetResource(
            [In] IDXGISurface2* This,
            [In] Guid* riid,
            [Out] void** ppParentResource,
            [Out] uint* pSubresourceIndex
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

            public IDXGISurface.GetDesc GetDesc;

            public IDXGISurface.Map Map;

            public IDXGISurface.Unmap Unmap;

            public IDXGISurface1.GetDC GetDC;

            public IDXGISurface1.ReleaseDC ReleaseDC;

            public GetResource GetResource;
            #endregion
        }
        #endregion
    }
}
