// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D3D12_RESOURCE_BARRIER
    {
        #region Fields
        [FieldOffset(0)]
        public D3D12_RESOURCE_BARRIER_TYPE Type;

        [FieldOffset(4)]
        public D3D12_RESOURCE_BARRIER_FLAGS Flags;

        #region union
        [FieldOffset(8)]
        public D3D12_RESOURCE_TRANSITION_BARRIER Transition;

        [FieldOffset(8)]
        public D3D12_RESOURCE_ALIASING_BARRIER Aliasing;

        [FieldOffset(8)]
        public D3D12_RESOURCE_UAV_BARRIER UAV;
        #endregion
        #endregion
    }
}
