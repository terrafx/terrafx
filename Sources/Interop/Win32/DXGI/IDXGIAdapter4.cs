// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("3C8D99D1-4FBF-4181-A82C-AF66BF7BD24E")]
    unsafe public struct IDXGIAdapter4
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGIAdapter4).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc3(
            [Out] DXGI_ADAPTER_DESC3* pDesc
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIAdapter3.Vtbl BaseVtbl;

            public GetDesc3 GetDesc3;
            #endregion
        }
        #endregion
    }
}
