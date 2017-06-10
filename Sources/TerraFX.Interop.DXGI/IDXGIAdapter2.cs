// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("0AA1AE0A-FA0E-4B84-8644-E05FF8E5ACB5")]
    unsafe public struct IDXGIAdapter2
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc2(
            [In] IDXGIAdapter2* This,
            [Out] DXGI_ADAPTER_DESC2* pDesc
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIAdapter1.Vtbl BaseVtbl;

            public GetDesc2 GetDesc2;
            #endregion
        }
        #endregion
    }
}
