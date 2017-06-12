// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Interop.DXGI;

namespace TerraFX.Interop.D3D12
{
    unsafe public struct D3D12_CLEAR_VALUE
    {
        #region Fields
        public DXGI_FORMAT Format;

        public _Anonymous_e__Union Anonymous;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public struct _Anonymous_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public fixed float Color[4];

            [FieldOffset(0)]
            public D3D12_DEPTH_STENCIL_VALUE DepthStencil;
            #endregion
        }
        #endregion
    }
}
