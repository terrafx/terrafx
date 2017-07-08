// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D3D12_SHADER_RESOURCE_VIEW_DESC
    {
        #region Fields
        [FieldOffset(0)]
        public DXGI_FORMAT Format;

        [FieldOffset(4)]
        public D3D12_SRV_DIMENSION ViewDimension;

        [FieldOffset(8)]
        public uint Shader4ComponentMapping;

        #region union
        [FieldOffset(16)]
        public D3D12_BUFFER_SRV Buffer;

        [FieldOffset(16)]
        public D3D12_TEX1D_SRV Texture1D;

        [FieldOffset(16)]
        public D3D12_TEX1D_ARRAY_SRV Texture1DArray;

        [FieldOffset(16)]
        public D3D12_TEX2D_SRV Texture2D;

        [FieldOffset(16)]
        public D3D12_TEX2D_ARRAY_SRV Texture2DArray;

        [FieldOffset(16)]
        public D3D12_TEX2DMS_SRV Texture2DMS;

        [FieldOffset(16)]
        public D3D12_TEX2DMS_ARRAY_SRV Texture2DMSArray;

        [FieldOffset(16)]
        public D3D12_TEX3D_SRV Texture3D;

        [FieldOffset(16)]
        public D3D12_TEXCUBE_SRV TextureCube;

        [FieldOffset(16)]
        public D3D12_TEXCUBE_ARRAY_SRV TextureCubeArray;
        #endregion
        #endregion
    }
}
