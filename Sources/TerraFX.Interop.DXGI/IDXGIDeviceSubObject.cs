// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("3D3E0379-F9DE-4D58-BB6C-18D62992F1A6")]
    unsafe public struct IDXGIDeviceSubObject
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDevice(
            [In] IDXGIDeviceSubObject* This,
            [In] /* readonly */ Guid* riid,
            [Out] void** ppDevice
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIObject.Vtbl BaseVtbl;

            public GetDevice GetDevice;
            #endregion
        }
        #endregion
    }
}
