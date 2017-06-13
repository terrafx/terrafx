// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D12_UAV_DIMENSION
    {
        UNKNOWN = 0,

        BUFFER = 1,

        TEXTURE1D = 2,

        TEXTURE1DARRAY = 3,

        TEXTURE2D = 4,

        TEXTURE2DARRAY = 5,

        TEXTURE3D = 8
    }
}
