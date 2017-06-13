// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum DXGI_MODE_SCANLINE_ORDER
    {
        UNSPECIFIED = 0,

        PROGRESSIVE = 1,

        UPPER_FIELD_FIRST = 2,

        LOWER_FIELD_FIRST = 3
    }
}
