// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D12_ROOT_PARAMETER_TYPE
    {
        DESCRIPTOR_TABLE = 0,

        _32BIT_CONSTANTS = 1,

        CBV = 2,

        SRV = 3,

        UAV = 4
    }
}
