// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("50C83A1C-E072-4C48-87B0-3630FA36A6D0")]
    unsafe public struct IDXGIFactory2
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate BOOL IsWindowedStereoEnabled(
            [In] IDXGIFactory2* This
        );

        public /* static */ delegate HRESULT CreateSwapChainForHwnd(
            [In] IDXGIFactory2* This,
            [In] IUnknown* pDevice,
            [In] HWND hWnd,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pFullscreenDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        );

        public /* static */ delegate HRESULT CreateSwapChainForCoreWindow(
            [In] IDXGIFactory2* This,
            [In] IUnknown* pDevice,
            [In] IUnknown* pWindow,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        );

        public /* static */ delegate HRESULT GetSharedResourceAdapterLuid(
            [In] IDXGIFactory2* This,
            [In] HANDLE hResource,
            [Out] LUID* plong
        );

        public /* static */ delegate HRESULT RegisterStereoStatusWindow(
            [In] IDXGIFactory2* This,
            [In] HWND WindowHandle,
            [In] uint wMsg,
            [Out] uint* pdwCookie
        );

        public /* static */ delegate HRESULT RegisterStereoStatusEvent(
            [In] IDXGIFactory2* This,
            [In] HANDLE hEvent,
            [Out] uint* pdwCookie
        );

        public /* static */ delegate void UnregisterStereoStatus(
            [In] IDXGIFactory2* This,
            [In] uint dwCookie
        );

        public /* static */ delegate HRESULT RegisterOcclusionStatusWindow(
            [In] IDXGIFactory2* This,
            [In] HWND WindowHandle,
            [In] uint wMsg,
            [Out] uint* pdwCookie
        );

        public /* static */ delegate HRESULT RegisterOcclusionStatusEvent(
            [In] IDXGIFactory2* This,
            [In] HANDLE hEvent,
            [Out] uint* pdwCookie
        );

        public /* static */ delegate void UnregisterOcclusionStatus(
            [In] IDXGIFactory2* This,
            [In] uint dwCookie
        );

        public /* static */ delegate HRESULT CreateSwapChainForComposition(
            [In] IDXGIFactory2* This,
            [In] IUnknown* pDevice,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
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

            public IsWindowedStereoEnabled IsWindowedStereoEnabled;

            public CreateSwapChainForHwnd CreateSwapChainForHwnd;

            public CreateSwapChainForCoreWindow CreateSwapChainForCoreWindow;

            public GetSharedResourceAdapterLuid GetSharedResourceAdapterLuid;

            public RegisterStereoStatusWindow RegisterStereoStatusWindow;

            public RegisterStereoStatusEvent RegisterStereoStatusEvent;

            public UnregisterStereoStatus UnregisterStereoStatus;

            public RegisterOcclusionStatusWindow RegisterOcclusionStatusWindow;

            public RegisterOcclusionStatusEvent RegisterOcclusionStatusEvent;

            public UnregisterOcclusionStatus UnregisterOcclusionStatus;

            public CreateSwapChainForComposition CreateSwapChainForComposition;
            #endregion
        }
        #endregion
    }
}
