// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_VIEWPORT
    {
        #region Fields
        public FLOAT TopLeftX;

        public FLOAT TopLeftY;

        public FLOAT Width;

        public FLOAT Height;

        public FLOAT MinDepth;

        public FLOAT MaxDepth;
        #endregion
    }
}
