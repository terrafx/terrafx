// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public struct D3D12_RESOURCE_BARRIER
    {
        #region Fields
        public D3D12_RESOURCE_BARRIER_TYPE Type;

        public D3D12_RESOURCE_BARRIER_FLAGS Flags;

        public _Anonymous_e__Union Anonymous;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public struct _Anonymous_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public D3D12_RESOURCE_TRANSITION_BARRIER Transition;

            [FieldOffset(0)]
            public D3D12_RESOURCE_ALIASING_BARRIER Aliasing;

            [FieldOffset(0)]
            public D3D12_RESOURCE_UAV_BARRIER UAV;
            #endregion
        }
        #endregion
    }
}
