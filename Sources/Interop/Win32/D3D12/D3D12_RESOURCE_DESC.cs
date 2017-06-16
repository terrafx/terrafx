// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct D3D12_RESOURCE_DESC
    {
        #region Fields
        public D3D12_RESOURCE_DIMENSION Dimension;

        public ulong Alignment;

        public ulong Width;

        public uint Height;

        public ushort DepthOrArraySize;

        public ushort MipLevels;

        public DXGI_FORMAT Format;

        public DXGI_SAMPLE_DESC SampleDesc;

        public D3D12_TEXTURE_LAYOUT Layout;

        public D3D12_RESOURCE_FLAGS Flags;
        #endregion
    }
}
