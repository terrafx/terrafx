// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.DXGI
{
    [Flags]
    public enum DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS
    {
        NONE = 0,

        NOMINAL_RANGE = 1,

        BT709 = 2,

        xvYCC = 4
    }
}
