// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.Desktop
{
    [Guid("41E7D1F2-A591-4F7B-A2E5-FA9C843E1C12")]
    unsafe public /* blittable */ struct IDXGIFactoryMedia
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateSwapChainForCompositionSurfaceHandle(
            [In] IDXGIFactoryMedia* This,
            [In] IUnknown* pDevice,
            [In, Optional, ComAliasName("HANDLE")] void* hSurface,
            [In] /* readonly */ DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecodeSwapChainForCompositionSurfaceHandle(
            [In] IDXGIFactoryMedia* This,
            [In] IUnknown* pDevice,
            [In, Optional, ComAliasName("HANDLE")] void* hSurface,
            [In] DXGI_DECODE_SWAP_CHAIN_DESC* pDesc,
            [In] IDXGIResource* pYuvDecodeBuffers,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGIDecodeSwapChain** ppSwapChain
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
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
