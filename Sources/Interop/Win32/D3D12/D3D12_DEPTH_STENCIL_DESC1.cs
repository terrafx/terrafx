// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct D3D12_DEPTH_STENCIL_DESC1
    {
        #region Fields
        public BOOL DepthEnable;

        public D3D12_DEPTH_WRITE_MASK DepthWriteMask;

        public D3D12_COMPARISON_FUNC DepthFunc;

        public BOOL StencilEnable;

        public byte StencilReadMask;

        public byte StencilWriteMask;

        public D3D12_DEPTH_STENCILOP_DESC FrontFace;

        public D3D12_DEPTH_STENCILOP_DESC BackFace;

        public BOOL DepthBoundsTestEnable;
        #endregion
    }
}
