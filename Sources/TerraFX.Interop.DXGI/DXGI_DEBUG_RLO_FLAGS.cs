// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_DEBUG_RLO_FLAGS
    {
        DXGI_DEBUG_RLO_SUMMARY = 1,
        DXGI_DEBUG_RLO_DETAIL = 2,
        DXGI_DEBUG_RLO_IGNORE_INTERNAL = 4,
        DXGI_DEBUG_RLO_ALL = 7,
    }
}
