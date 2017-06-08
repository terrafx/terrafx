// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_MODE_ROTATION
    {
        UNSPECIFIED = 0,

        IDENTITY = 1,

        ROTATE90 = 2,

        ROTATE180 = 3,

        ROTATE270 = 4
    }
}
