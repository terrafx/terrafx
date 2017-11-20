// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct D3D12_SIGNATURE_PARAMETER_DESC
    {
        #region Fields
        [ComAliasName("LPCSTR")]
        public sbyte* SemanticName;

        [ComAliasName("UINT")]
        public uint SemanticIndex;

        [ComAliasName("UINT")]
        public uint Register;

        public D3D_NAME SystemValueType;

        public D3D_REGISTER_COMPONENT_TYPE ComponentType;

        [ComAliasName("BYTE")]
        public byte Mask;

        [ComAliasName("BYTE")]
        public byte ReadWriteMask;

        [ComAliasName("UINT")]
        public uint Stream;

        public D3D_MIN_PRECISION MinPrecision;
        #endregion
    }
}
