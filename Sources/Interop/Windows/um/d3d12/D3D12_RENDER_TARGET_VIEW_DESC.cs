// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D3D12_RENDER_TARGET_VIEW_DESC
    {
        #region Fields
        [FieldOffset(0)]
        public DXGI_FORMAT Format;

        [FieldOffset(4)]
        public D3D12_RTV_DIMENSION ViewDimension;

        #region union
        [FieldOffset(8)]
        public D3D12_BUFFER_RTV Buffer;

        [FieldOffset(8)]
        public D3D12_TEX1D_RTV Texture1D;

        [FieldOffset(8)]
        public D3D12_TEX1D_ARRAY_RTV Texture1DArray;

        [FieldOffset(8)]
        public D3D12_TEX2D_RTV Texture2D;

        [FieldOffset(8)]
        public D3D12_TEX2D_ARRAY_RTV Texture2DArray;

        [FieldOffset(8)]
        public D3D12_TEX2DMS_RTV Texture2DMS;

        [FieldOffset(8)]
        public D3D12_TEX2DMS_ARRAY_RTV Texture2DMSArray;

        [FieldOffset(8)]
        public D3D12_TEX3D_RTV Texture3D;
        #endregion
        #endregion
    }
}
