// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("94D99BDB-F1F8-4AB0-B236-7DA0170EDAB1")]
    public /* blittable */ unsafe struct IDXGISwapChain3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint GetCurrentBackBufferIndex(
            [In] IDXGISwapChain3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CheckColorSpaceSupport(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [Out, ComAliasName("UINT")] uint* pColorSpaceSupport
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetColorSpace1(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ResizeBuffers1(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint BufferCount,
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height,
            [In] DXGI_FORMAT Format,
            [In, ComAliasName("UINT")] uint SwapChainFlags,
            [In, ComAliasName("UINT[]")] uint* pCreationNodeMask,
            [In, ComAliasName("IUnknown*[]")] IUnknown** ppPresentQueue
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGISwapChain2.Vtbl BaseVtbl;

            public IntPtr GetCurrentBackBufferIndex;

            public IntPtr CheckColorSpaceSupport;

            public IntPtr SetColorSpace1;

            public IntPtr ResizeBuffers1;
            #endregion
        }
        #endregion
    }
}
