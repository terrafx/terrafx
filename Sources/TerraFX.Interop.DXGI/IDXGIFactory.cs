// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("7B7166EC-21C7-44AE-B21A-C9AE321AE369")]
    unsafe public struct IDXGIFactory
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT EnumAdapters(
            [In] IDXGIFactory* This,
            [In] uint Adapter,
            [Out] IDXGIAdapter** ppAdapter
        );

        public /* static */ delegate HRESULT MakeWindowAssociation(
            [In] IDXGIFactory* This,
            [In] HWND WindowHandle,
            [In] DXGI_MWA_FLAG Flags
        );

        public /* static */ delegate HRESULT GetWindowAssociation(
            [In] IDXGIFactory* This,
            [Out] HWND* pWindowHandle
        );

        public /* static */ delegate HRESULT CreateSwapChain(
            [In] IDXGIFactory* This,
            [In] IUnknown* pDevice,
            [In] DXGI_SWAP_CHAIN_DESC* pDesc,
            [Out] IDXGISwapChain** ppSwapChain
        );

        public /* static */ delegate HRESULT CreateSoftwareAdapter(
            [In] IDXGIFactory* This,
            [In] HMODULE Module,
            [Out] IDXGIAdapter** ppAdapter
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIObject.Vtbl BaseVtbl;

            public EnumAdapters EnumAdapters;

            public MakeWindowAssociation MakeWindowAssociation;

            public GetWindowAssociation GetWindowAssociation;

            public CreateSwapChain CreateSwapChain;

            public CreateSoftwareAdapter CreateSoftwareAdapter;
            #endregion
        }
        #endregion
    }
}
