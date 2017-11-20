// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_SUBRESOURCE_FOOTPRINT
    {
        #region Fields
        public DXGI_FORMAT Format;

        [ComAliasName("UINT")]
        public uint Width;

        [ComAliasName("UINT")]
        public uint Height;

        [ComAliasName("UINT")]
        public uint Depth;

        [ComAliasName("UINT")]
        public uint RowPitch;
        #endregion
    }
}
