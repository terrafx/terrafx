// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("905DB94B-A00C-4140-9DF5-2B64CA9EA357")]
    unsafe public struct ID3D12DeviceChild
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12DeviceChild).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDevice(
            [In] ID3D12DeviceChild* This,
            [In] Guid* riid,
            [Out, Optional] void** ppvDevice
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Object.Vtbl BaseVtbl;

            public GetDevice GetDevice;
            #endregion
        }
        #endregion
    }
}
