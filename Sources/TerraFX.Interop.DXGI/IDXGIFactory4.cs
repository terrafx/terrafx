// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("1BC6EA02-EF36-464F-BF0C-21CA39E5168A")]
    unsafe public struct IDXGIFactory4
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT EnumAdapterByLuid(
            [In] IDXGIFactory4* This,
            [In] LUID AdapterLuid,
            [In] Guid* riid,
            [Out] void** ppvAdapter
        );

        public /* static */ delegate HRESULT EnumWarpAdapter(
            [In] IDXGIFactory4* This,
            [In] Guid* riid,
            [Out] void** ppvAdapter
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

            public IDXGIFactory.EnumAdapters EnumAdapters;

            public IDXGIFactory.MakeWindowAssociation MakeWindowAssociation;

            public IDXGIFactory.GetWindowAssociation GetWindowAssociation;

            public IDXGIFactory.CreateSwapChain CreateSwapChain;

            public IDXGIFactory.CreateSoftwareAdapter CreateSoftwareAdapter;

            public IDXGIFactory1.EnumAdapters1 EnumAdapters1;

            public IDXGIFactory1.IsCurrent IsCurrent;

            public IDXGIFactory2.IsWindowedStereoEnabled IsWindowedStereoEnabled;

            public IDXGIFactory2.CreateSwapChainForHwnd CreateSwapChainForHwnd;

            public IDXGIFactory2.CreateSwapChainForCoreWindow CreateSwapChainForCoreWindow;

            public IDXGIFactory2.GetSharedResourceAdapterLuid GetSharedResourceAdapterLuid;

            public IDXGIFactory2.RegisterStereoStatusWindow RegisterStereoStatusWindow;

            public IDXGIFactory2.RegisterStereoStatusEvent RegisterStereoStatusEvent;

            public IDXGIFactory2.UnregisterStereoStatus UnregisterStereoStatus;

            public IDXGIFactory2.RegisterOcclusionStatusWindow RegisterOcclusionStatusWindow;

            public IDXGIFactory2.RegisterOcclusionStatusEvent RegisterOcclusionStatusEvent;

            public IDXGIFactory2.UnregisterOcclusionStatus UnregisterOcclusionStatus;

            public IDXGIFactory2.CreateSwapChainForComposition CreateSwapChainForComposition;

            public IDXGIFactory3.GetCreationFlags GetCreationFlags;

            public EnumAdapterByLuid EnumAdapterByLuid;

            public EnumWarpAdapter EnumWarpAdapter;
            #endregion
        }
        #endregion
    }
}
