// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct D3D12_SHADER_VARIABLE_DESC
    {
        #region Fields
        public LPCSTR Name;

        public UINT StartOffset;

        public UINT Size;

        public UINT uFlags;

        public LPVOID DefaultValue;

        public UINT StartTexture;

        public UINT TextureSize;

        public UINT StartSampler;

        public UINT SamplerSize;
        #endregion
    }
}
