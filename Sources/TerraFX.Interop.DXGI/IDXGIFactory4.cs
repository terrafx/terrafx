// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("1BC6EA02-EF36-464F-BF0C-21CA39E5168A")]
    unsafe public struct IDXGIFactory4
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT EnumAdapterByLuid(
            [In] IDXGIFactory4* This,
            [In] LUID AdapterLuid,
            [In] /* readonly */ Guid* riid,
            [Out] void** ppvAdapter
        );

        public /* static */ delegate HRESULT EnumWarpAdapter(
            [In] IDXGIFactory4* This,
            [In] /* readonly */ Guid* riid,
            [Out] void** ppvAdapter
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIFactory3.Vtbl BaseVtbl;

            public EnumAdapterByLuid EnumAdapterByLuid;

            public EnumWarpAdapter EnumWarpAdapter;
            #endregion
        }
        #endregion
    }
}
