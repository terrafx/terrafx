// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum D3D12_ROOT_DESCRIPTOR_FLAGS
    {
        NONE = 0,

        DATA_VOLATILE = 2,

        DATA_STATIC_WHILE_SET_AT_EXECUTE = 4,

        DATA_STATIC = 8
    }
}
