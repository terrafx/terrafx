// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("41E7D1F2-A591-4F7B-A2E5-FA9C843E1C12")]
    unsafe public struct IDXGIFactoryMedia
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CreateSwapChainForCompositionSurfaceHandle(
            [In] IDXGIFactoryMedia* This,
            [In] IUnknown* pDevice,
            [In, Optional] HANDLE hSurface,
            [In] /* readonly */ DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        );

        public /* static */ delegate HRESULT CreateDecodeSwapChainForCompositionSurfaceHandle(
            [In] IDXGIFactoryMedia* This,
            [In] IUnknown* pDevice,
            [In, Optional] HANDLE hSurface,
            [In] DXGI_DECODE_SWAP_CHAIN_DESC* pDesc,
            [In] IDXGIResource* pYuvDecodeBuffers,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGIDecodeSwapChain** ppSwapChain
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public CreateSwapChainForCompositionSurfaceHandle CreateSwapChainForCompositionSurfaceHandle;

            public CreateDecodeSwapChainForCompositionSurfaceHandle CreateDecodeSwapChainForCompositionSurfaceHandle;
            #endregion
        }
        #endregion
    }
}
