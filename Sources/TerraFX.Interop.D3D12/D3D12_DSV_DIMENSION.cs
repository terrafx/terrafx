// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.D3D12
{
    public enum D3D12_DSV_DIMENSION
    {
        UNKNOWN = 0,

        TEXTURE1D = 1,

        TEXTURE1DARRAY = 2,

        TEXTURE2D = 3,

        TEXTURE2DARRAY = 4,

        TEXTURE2DMS = 5,

        TEXTURE2DMSARRAY = 6
    }
}
