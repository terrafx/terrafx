// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_BUFFER_UAV
    {
        #region Fields
        [ComAliasName("UINT64")]
        public ulong FirstElement;

        [ComAliasName("UINT")]
        public uint NumElements;

        [ComAliasName("UINT")]
        public uint StructureByteStride;

        [ComAliasName("UINT64")]
        public ulong CounterOffsetInBytes;

        public D3D12_BUFFER_UAV_FLAGS Flags;
        #endregion
    }
}
