// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct D3D12_TEXTURE_COPY_LOCATION
    {
        #region Fields
        public ID3D12Resource* pResource;

        public D3D12_TEXTURE_COPY_TYPE Type;

        public _Anonymous_e__Union Anonymous;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        [Unmanaged]
        public struct _Anonymous_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public D3D12_PLACED_SUBRESOURCE_FOOTPRINT PlacedFootprint;

            [FieldOffset(0)]
            [ComAliasName("UINT")]
            public uint SubresourceIndex;
            #endregion
        }
        #endregion
    }
}
