// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.


namespace TerraFX.Interop.D3D12
{
    unsafe public struct D3D12_ROOT_SIGNATURE_DESC
    {
        #region Fields
        public uint NumParameters;

        public D3D12_ROOT_PARAMETER* pParameters;

        public uint NumStaticSamplers;

        public D3D12_STATIC_SAMPLER_DESC* pStaticSamplers;

        public D3D12_ROOT_SIGNATURE_FLAGS Flags;
        #endregion
    }
}
