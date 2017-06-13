// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("94D99BDB-F1F8-4AB0-B236-7DA0170EDAB1")]
    unsafe public struct IDXGISwapChain3
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate uint GetCurrentBackBufferIndex(
            [In] IDXGISwapChain3* This
        );

        public /* static */ delegate HRESULT CheckColorSpaceSupport(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [Out] uint* pColorSpaceSupport
        );

        public /* static */ delegate HRESULT SetColorSpace1(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace
        );

        public /* static */ delegate HRESULT ResizeBuffers1(
            [In] IDXGISwapChain3* This,
            [In] uint BufferCount,
            [In] uint Width,
            [In] uint Height,
            [In] DXGI_FORMAT Format,
            [In] DXGI_SWAP_CHAIN_FLAG SwapChainFlags,
            [In] /* readonly */ uint* pCreationNodeMask,
            [In] /* readonly */ IUnknown** ppPresentQueue
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGISwapChain2.Vtbl BaseVtbl;

            public GetCurrentBackBufferIndex GetCurrentBackBufferIndex;

            public CheckColorSpaceSupport CheckColorSpaceSupport;

            public SetColorSpace1 SetColorSpace1;

            public ResizeBuffers1 ResizeBuffers1;
            #endregion
        }
        #endregion
    }
}
