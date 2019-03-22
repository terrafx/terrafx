// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct D3D12_TILE_REGION_SIZE
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint NumTiles;

        [ComAliasName("BOOL")]
        public int UseBox;

        [ComAliasName("UINT")]
        public uint Width;

        [ComAliasName("UINT16")]
        public ushort Height;

        [ComAliasName("UINT16")]
        public ushort Depth;
        #endregion
    }
}
