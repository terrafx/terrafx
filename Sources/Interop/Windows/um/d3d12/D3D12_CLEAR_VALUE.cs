// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    unsafe public /* blittable */ struct D3D12_CLEAR_VALUE
    {
        #region Fields
        [FieldOffset(0)]
        public DXGI_FORMAT Format;

        #region union
        [FieldOffset(4)]
        [ComAliasName("FLOAT[4]")]
        public fixed float Color[4];

        [FieldOffset(4)]
        public D3D12_DEPTH_STENCIL_VALUE DepthStencil;
        #endregion
        #endregion
    }
}
