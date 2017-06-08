// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.DXGI
{
    [Flags]
    public enum DXGI_USAGE_FLAG : uint
    {
        NONE = 0x00000000,

        SHADER_INPUT = 0x00000010,

        RENDER_TARGET_OUTPUT = 0x00000020,

        BACK_BUFFER = 0x00000040,

        SHARED = 0x00000080,

        READ_ONLY = 0x00000100,

        DISCARD_ON_PRESENT = 0x00000200,

        UNORDERED_ACCESS = 0x00000400,

        // Part of dxgiinternal.h
        REMOTE_SWAPCHAIN_BUFFER = 0x00080000,

        // Part of dxgiinternal.h
        GDI_COMPATIBLE = 0x00100000,

        FIELD = 0x0007FFF0
    }
}
