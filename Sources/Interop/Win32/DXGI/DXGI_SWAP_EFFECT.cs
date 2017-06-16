// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum DXGI_SWAP_EFFECT
    {
        DISCARD = 0,

        SEQUENTIAL = 1,

        FLIP_SEQUENTIAL = 3,

        FLIP_DISCARD = 4
    }
}
