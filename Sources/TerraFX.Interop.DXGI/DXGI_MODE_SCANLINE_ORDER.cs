// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_MODE_SCANLINE_ORDER
    {
        DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED,
        DXGI_MODE_SCANLINE_ORDER_PROGRESSIVE,
        DXGI_MODE_SCANLINE_ORDER_UPPER_FIELD_FIRST,
        DXGI_MODE_SCANLINE_ORDER_LOWER_FIELD_FIRST,
    }
}
