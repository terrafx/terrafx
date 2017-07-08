// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_RASTERIZER_DESC
    {
        #region Fields
        public D3D12_FILL_MODE FillMode;

        public D3D12_CULL_MODE CullMode;

        public BOOL FrontCounterClockwise;

        public INT DepthBias;

        public FLOAT DepthBiasClamp;

        public FLOAT SlopeScaledDepthBias;

        public BOOL DepthClipEnable;

        public BOOL MultisampleEnable;

        public BOOL AntialiasedLineEnable;

        public UINT ForcedSampleCount;

        public D3D12_CONSERVATIVE_RASTERIZATION_MODE ConservativeRaster;
        #endregion
    }
}
