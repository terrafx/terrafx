// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("29038F61-3839-4626-91FD-086879011A05")]
    unsafe public struct IDXGIAdapter1
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc1(
            [In] IDXGIAdapter1* This,
            [Out] DXGI_ADAPTER_DESC1* pDesc
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIAdapter.Vtbl BaseVtbl;

            public GetDesc1 GetDesc1;
            #endregion
        }
        #endregion
    }
}
