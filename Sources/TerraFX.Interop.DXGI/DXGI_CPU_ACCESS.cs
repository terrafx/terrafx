// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum DXGI_CPU_ACCESS : uint
    {
        NONE = 0,

        DYNAMIC = 1,

        READ_WRITE = 2,

        SCRATCH = 3,

        FIELD = 15
    }
}
