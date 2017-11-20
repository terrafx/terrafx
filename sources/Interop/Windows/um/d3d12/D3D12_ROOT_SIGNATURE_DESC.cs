// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct D3D12_ROOT_SIGNATURE_DESC
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint NumParameters;

        [ComAliasName("D3D12_ROOT_PARAMETER[]")]
        public D3D12_ROOT_PARAMETER* pParameters;

        [ComAliasName("UINT")]
        public uint NumStaticSamplers;

        [ComAliasName("D3D12_STATIC_SAMPLER_DESC[]")]
        public D3D12_STATIC_SAMPLER_DESC* pStaticSamplers;

        public D3D12_ROOT_SIGNATURE_FLAGS Flags;
        #endregion
    }
}
