// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum DXGI_RESOURCE_PRIORITY : uint
    {
        UNDEFINED = 0x00000000,

        MINIMUM = 0x28000000,

        LOW = 0x50000000,

        NORMAL = 0x78000000,

        HIGH = 0xA0000000,

        MAXIMUM = 0xC8000000
    }
}
