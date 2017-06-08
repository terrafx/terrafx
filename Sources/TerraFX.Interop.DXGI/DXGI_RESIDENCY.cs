// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_RESIDENCY
    {
        UNKNOWN = 0,

        FULLY_RESIDENT = 1,

        RESIDENT_IN_SHARED_MEMORY = 2,

        EVICTED_TO_DISK = 3
    }
}
