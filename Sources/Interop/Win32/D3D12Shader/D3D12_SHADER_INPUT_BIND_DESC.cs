// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct D3D12_SHADER_INPUT_BIND_DESC
    {
        #region Fields
        public LPSTR Name;

        public D3D_SHADER_INPUT_TYPE Type;

        public uint BindPoint;

        public uint BindCount;

        public uint uFlags;

        public D3D_RESOURCE_RETURN_TYPE ReturnType;

        public D3D_SRV_DIMENSION Dimension;

        public uint NumSamples;

        public uint Space;

        public uint uID;
        #endregion
    }
}
