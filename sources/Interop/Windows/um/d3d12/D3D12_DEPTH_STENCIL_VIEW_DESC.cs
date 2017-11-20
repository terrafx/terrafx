// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D3D12_DEPTH_STENCIL_VIEW_DESC
    {
        #region Fields
        [FieldOffset(0)]
        public DXGI_FORMAT Format;

        [FieldOffset(4)]
        public D3D12_DSV_DIMENSION ViewDimension;

        [FieldOffset(8)]
        public D3D12_DSV_FLAGS Flags;

        #region union
        [FieldOffset(12)]
        public D3D12_TEX1D_DSV Texture1D;

        [FieldOffset(12)]
        public D3D12_TEX1D_ARRAY_DSV Texture1DArray;

        [FieldOffset(12)]
        public D3D12_TEX2D_DSV Texture2D;

        [FieldOffset(12)]
        public D3D12_TEX2D_ARRAY_DSV Texture2DArray;

        [FieldOffset(12)]
        public D3D12_TEX2DMS_DSV Texture2DMS;

        [FieldOffset(12)]
        public D3D12_TEX2DMS_ARRAY_DSV Texture2DMSArray;
        #endregion
        #endregion
    }
}
