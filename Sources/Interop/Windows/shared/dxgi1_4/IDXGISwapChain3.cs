// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("94D99BDB-F1F8-4AB0-B236-7DA0170EDAB1")]
    unsafe public /* blittable */ struct IDXGISwapChain3
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetCurrentBackBufferIndex(
            [In] IDXGISwapChain3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CheckColorSpaceSupport(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [Out] UINT* pColorSpaceSupport
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetColorSpace1(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ResizeBuffers1(
            [In] IDXGISwapChain3* This,
            [In] UINT BufferCount,
            [In] UINT Width,
            [In] UINT Height,
            [In] DXGI_FORMAT Format,
            [In] UINT SwapChainFlags,
            [In] /* readonly */ UINT* pCreationNodeMask,
            [In] /* readonly */ IUnknown** ppPresentQueue
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
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
