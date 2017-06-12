// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.D3D12
{
    public enum D3D12_TEXTURE_LAYOUT
    {
        UNKNOWN = 0,

        ROW_MAJOR = 1,

        _64KB_UNDEFINED_SWIZZLE = 2,

        _64KB_STANDARD_SWIZZLE = 3
    }
}
