// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct D3D12_DEPTH_STENCIL_DESC1
    {
        #region Fields
        [ComAliasName("BOOL")]
        public int DepthEnable;

        public D3D12_DEPTH_WRITE_MASK DepthWriteMask;

        public D3D12_COMPARISON_FUNC DepthFunc;

        [ComAliasName("BOOL")]
        public int StencilEnable;

        [ComAliasName("UINT8")]
        public byte StencilReadMask;

        [ComAliasName("UINT8")]
        public byte StencilWriteMask;

        public D3D12_DEPTH_STENCILOP_DESC FrontFace;

        public D3D12_DEPTH_STENCILOP_DESC BackFace;

        [ComAliasName("BOOL")]
        public int DepthBoundsTestEnable;
        #endregion
    }
}
