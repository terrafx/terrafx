// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct D3D12_SAMPLER_DESC
    {
        #region Fields
        public D3D12_FILTER Filter;

        public D3D12_TEXTURE_ADDRESS_MODE AddressU;

        public D3D12_TEXTURE_ADDRESS_MODE AddressV;

        public D3D12_TEXTURE_ADDRESS_MODE AddressW;

        public FLOAT MipLODBias;

        public UINT MaxAnisotropy;

        public D3D12_COMPARISON_FUNC ComparisonFunc;

        public _BorderColor_e__FixedBuffer BorderColor;

        public FLOAT MinLOD;

        public FLOAT MaxLOD;
        #endregion

        #region Structs
        public /* blittable */ struct _BorderColor_e__FixedBuffer
        {
            #region Fields
            public FLOAT _0;

            public FLOAT _1;

            public FLOAT _2;

            public FLOAT _3;
            #endregion
        }
        #endregion
    }
}
