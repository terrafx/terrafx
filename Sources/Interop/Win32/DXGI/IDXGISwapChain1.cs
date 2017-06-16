// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("790A45F7-0D42-4876-983A-0A55CFE6F4AA")]
    unsafe public struct IDXGISwapChain1
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGISwapChain1).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc1(
            [In] IDXGISwapChain1* This,
            [Out] DXGI_SWAP_CHAIN_DESC1* pDesc
        );

        public /* static */ delegate HRESULT GetFullscreenDesc(
            [In] IDXGISwapChain1* This,
            [Out] DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pDesc
        );

        public /* static */ delegate HRESULT GetHwnd(
            [In] IDXGISwapChain1* This,
            [Out] HWND* pHwnd
        );

        public /* static */ delegate HRESULT GetCoreWindow(
            [In] IDXGISwapChain1* This,
            [In] ref /* readonly */ Guid refiid,
            [Out] void** ppUnk
        );

        public /* static */ delegate HRESULT Present1(
            [In] IDXGISwapChain1* This,
            [In] uint SyncInterval,
            [In] DXGI_PRESENT_FLAG PresentFlags,
            [In] /* readonly */ DXGI_PRESENT_PARAMETERS* pPresentParameters
        );

        public /* static */ delegate BOOL IsTemporaryMonoSupported(
            [In] IDXGISwapChain1* This
        );

        public /* static */ delegate HRESULT GetRestrictToOutput(
            [In] IDXGISwapChain1* This,
            [Out] IDXGIOutput** ppRestrictToOutput
        );

        public /* static */ delegate HRESULT SetBackgroundColor(
            [In] IDXGISwapChain1* This,
            [In] /* readonly */ DXGI_RGBA* pColor
        );

        public /* static */ delegate HRESULT GetBackgroundColor(
            [In] IDXGISwapChain1* This,
            [Out] DXGI_RGBA* pColor
        );

        public /* static */ delegate HRESULT SetRotation(
            [In] IDXGISwapChain1* This,
            [In] DXGI_MODE_ROTATION Rotation
        );

        public /* static */ delegate HRESULT GetRotation(
            [In] IDXGISwapChain1* This,
            [Out] DXGI_MODE_ROTATION* pRotation
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGISwapChain.Vtbl BaseVtbl;

            public GetDesc1 GetDesc1;

            public GetFullscreenDesc GetFullscreenDesc;

            public GetHwnd GetHwnd;

            public GetCoreWindow GetCoreWindow;

            public Present1 Present1;

            public IsTemporaryMonoSupported IsTemporaryMonoSupported;

            public GetRestrictToOutput GetRestrictToOutput;

            public SetBackgroundColor SetBackgroundColor;

            public GetBackgroundColor GetBackgroundColor;

            public SetRotation SetRotation;

            public GetRotation GetRotation;
            #endregion
        }
        #endregion
    }
}
