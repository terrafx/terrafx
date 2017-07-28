// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct D3D12_INPUT_ELEMENT_DESC
    {
        #region Fields
        [ComAliasName("LPCSTR")]
        public /* readonly */ sbyte* SemanticName;

        [ComAliasName("UINT")]
        public uint SemanticIndex;

        public DXGI_FORMAT Format;

        [ComAliasName("UINT")]
        public uint InputSlot;

        [ComAliasName("UINT")]
        public uint AlignedByteOffset;

        public D3D12_INPUT_CLASSIFICATION InputSlotClass;

        [ComAliasName("UINT")]
        public uint InstanceDataStepRate;
        #endregion
    }
}
