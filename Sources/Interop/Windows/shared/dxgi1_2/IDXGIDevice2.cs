// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("05008617-FBFD-4051-A790-144884B4F6A9")]
    unsafe public /* blittable */ struct IDXGIDevice2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OfferResources(
            [In] IDXGIDevice2* This,
            [In, ComAliasName("UINT")] uint NumResources,
            [In] /* readonly */ IDXGIResource** ppResources,
            [In] DXGI_OFFER_RESOURCE_PRIORITY Priority
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReclaimResources(
            [In] IDXGIDevice2* This,
            [In, ComAliasName("UINT")] uint NumResources,
            [In] /* readonly */ IDXGIResource** ppResources,
            [Out, Optional, ComAliasName("BOOL")] int* pDiscarded
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int EnqueueSetEvent(
            [In] IDXGIDevice2* This,
            [In, ComAliasName("HANDLE")] void* hEvent
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIDevice1.Vtbl BaseVtbl;

            public OfferResources OfferResources;

            public ReclaimResources ReclaimResources;

            public EnqueueSetEvent EnqueueSetEvent;
            #endregion
        }
        #endregion
    }
}
