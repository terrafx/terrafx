// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct D3D12_RESOURCE_DESC
    {
        #region Fields
        public D3D12_RESOURCE_DIMENSION Dimension;

        [ComAliasName("UINT64")]
        public ulong Alignment;

        [ComAliasName("UINT64")]
        public ulong Width;

        [ComAliasName("UINT")]
        public uint Height;

        [ComAliasName("UINT16")]
        public ushort DepthOrArraySize;

        [ComAliasName("UINT16")]
        public ushort MipLevels;

        public DXGI_FORMAT Format;

        public DXGI_SAMPLE_DESC SampleDesc;

        public D3D12_TEXTURE_LAYOUT Layout;

        public D3D12_RESOURCE_FLAGS Flags;
        #endregion
    }
}
