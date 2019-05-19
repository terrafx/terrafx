// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_BARRIER_TYPE;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    [Unmanaged]
    public unsafe struct D3D12_RESOURCE_BARRIER
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

        #region Methods
        public static D3D12_RESOURCE_BARRIER InitTransition(ID3D12Resource* pResource, D3D12_RESOURCE_STATES stateBefore, D3D12_RESOURCE_STATES stateAfter, uint subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES, D3D12_RESOURCE_BARRIER_FLAGS flags = D3D12_RESOURCE_BARRIER_FLAG_NONE)
        {
            D3D12_RESOURCE_BARRIER result = default;
            result.Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION;
            result.Flags = flags;
            result.Transition.pResource = pResource;
            result.Transition.StateBefore = stateBefore;
            result.Transition.StateAfter = stateAfter;
            result.Transition.Subresource = subresource;
            return result;
        }
        public static D3D12_RESOURCE_BARRIER InitAliasing(ID3D12Resource* pResourceBefore, ID3D12Resource* pResourceAfter)
        {
            D3D12_RESOURCE_BARRIER result = default;
            result.Type = D3D12_RESOURCE_BARRIER_TYPE_ALIASING;
            result.Aliasing.pResourceBefore = pResourceBefore;
            result.Aliasing.pResourceAfter = pResourceAfter;
            return result;
        }
        public static D3D12_RESOURCE_BARRIER InitUAV(ID3D12Resource* pResource)
        {
            D3D12_RESOURCE_BARRIER result = default;
            result.Type = D3D12_RESOURCE_BARRIER_TYPE_UAV;
            result.UAV.pResource = pResource;
            return result;
        }
        #endregion
    }
}
