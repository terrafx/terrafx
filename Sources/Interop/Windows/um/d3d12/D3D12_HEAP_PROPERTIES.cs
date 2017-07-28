// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_HEAP_PROPERTIES
    {
        #region Fields
        public D3D12_HEAP_TYPE Type;

        public D3D12_CPU_PAGE_PROPERTY CPUPageProperty;

        public D3D12_MEMORY_POOL MemoryPoolPreference;

        [ComAliasName("UINT")]
        public uint CreationNodeMask;

        [ComAliasName("UINT")]
        public uint VisibleNodeMask;
        #endregion
    }
}
