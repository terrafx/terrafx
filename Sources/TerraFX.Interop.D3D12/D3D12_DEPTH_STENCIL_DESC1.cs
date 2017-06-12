// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    public struct D3D12_DEPTH_STENCIL_DESC1
    {
        #region Fields
        public D3D12_DEPTH_STENCIL_DESC BaseValue;

        public BOOL DepthBoundsTestEnable;
        #endregion
    }
}
