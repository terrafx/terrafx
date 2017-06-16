// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("95B4F95F-D8DA-4CA4-9EE6-3B76D5968A10")]
    unsafe public struct IDXGIDevice4
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGIDevice4).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT OfferResources1(
            [In] IDXGIDevice4* This,
            [In] uint NumResources,
            [In] /* readonly */ IDXGIResource** ppResources,
            [In] DXGI_OFFER_RESOURCE_PRIORITY Priority,
            [In] DXGI_OFFER_RESOURCE_FLAGS Flags
        );

        public /* static */ delegate HRESULT ReclaimResources1(
            [In] IDXGIDevice4* This,
            [In] uint NumResources,
            [In] /* readonly */ IDXGIResource** ppResources,
            [Out] DXGI_RECLAIM_RESOURCE_RESULTS* pResults
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIDevice3.Vtbl BaseVtbl;

            public OfferResources1 OfferResources1;

            public ReclaimResources1 ReclaimResources1;
            #endregion
        }
        #endregion
    }
}
