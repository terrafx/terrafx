// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.
namespace TerraFX.Interop
{
    public enum D3D12_HEAP_TYPE
    {
        UNDEFINED = 0,

        DEFAULT = 1,

        UPLOAD = 2,

        READBACK = 3,

        CUSTOM = 4
    }
}
