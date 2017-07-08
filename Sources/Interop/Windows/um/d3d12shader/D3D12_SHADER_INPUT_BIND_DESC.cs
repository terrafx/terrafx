// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_SHADER_INPUT_BIND_DESC
    {
        #region Fields
        public LPCSTR Name;

        public D3D_SHADER_INPUT_TYPE Type;

        public UINT BindPoint;

        public UINT BindCount;

        public UINT uFlags;

        public D3D_RESOURCE_RETURN_TYPE ReturnType;

        public D3D_SRV_DIMENSION Dimension;

        public UINT NumSamples;

        public UINT Space;

        public UINT uID;
        #endregion
    }
}
