// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct D3D12_SAMPLER_DESC
    {
        #region Fields
        public D3D12_FILTER Filter;

        public D3D12_TEXTURE_ADDRESS_MODE AddressU;

        public D3D12_TEXTURE_ADDRESS_MODE AddressV;

        public D3D12_TEXTURE_ADDRESS_MODE AddressW;

        [ComAliasName("FLOAT")]
        public float MipLODBias;

        [ComAliasName("UINT")]
        public uint MaxAnisotropy;

        public D3D12_COMPARISON_FUNC ComparisonFunc;

        [ComAliasName("FLOAT[4]")]
        public fixed float BorderColor[4];

        [ComAliasName("FLOAT")]
        public float MinLOD;

        [ComAliasName("FLOAT")]
        public float MaxLOD;
        #endregion
    }
}
