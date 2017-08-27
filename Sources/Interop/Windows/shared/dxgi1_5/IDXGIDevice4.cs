// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("95B4F95F-D8DA-4CA4-9EE6-3B76D5968A10")]
    public /* blittable */ unsafe struct IDXGIDevice4
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OfferResources1(
            [In] IDXGIDevice4* This,
            [In, ComAliasName("UINT")] uint NumResources,
            [In, ComAliasName("IDXGIResource*[]")] IDXGIResource** ppResources,
            [In] DXGI_OFFER_RESOURCE_PRIORITY Priority,
            [In, ComAliasName("UINT")] uint Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReclaimResources1(
            [In] IDXGIDevice4* This,
            [In, ComAliasName("UINT")] uint NumResources,
            [In, ComAliasName("IDXGIResource*[]")] IDXGIResource** ppResources,
            [Out] DXGI_RECLAIM_RESOURCE_RESULTS* pResults
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIDevice3.Vtbl BaseVtbl;

            public IntPtr OfferResources1;

            public IntPtr ReclaimResources1;
            #endregion
        }
        #endregion
    }
}
