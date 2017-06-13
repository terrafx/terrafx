// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("ABA496DD-B617-4CB8-A866-BC44D7EB1FA2")]
    unsafe public struct IDXGISurface2
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetResource(
            [In] IDXGISurface2* This,
            [In] /* readonly */ Guid* riid,
            [Out] void** ppParentResource,
            [Out] uint* pSubresourceIndex
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGISurface1.Vtbl BaseVtbl;

            public GetResource GetResource;
            #endregion
        }
        #endregion
    }
}
