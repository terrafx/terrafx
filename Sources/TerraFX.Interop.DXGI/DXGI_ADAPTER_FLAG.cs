// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_ADAPTER_FLAG
    {
        DXGI_ADAPTER_FLAG_FORCE_DWORD = -1,
        DXGI_ADAPTER_FLAG_NONE = 0,
        DXGI_ADAPTER_FLAG_REMOTE = 1,
        DXGI_ADAPTER_FLAG_SOFTWARE = 2,
    }
}
