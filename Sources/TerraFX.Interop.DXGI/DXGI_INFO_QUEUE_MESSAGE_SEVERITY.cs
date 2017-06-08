// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.DXGI
{
    public enum DXGI_INFO_QUEUE_MESSAGE_SEVERITY
    {
        DXGI_INFO_QUEUE_MESSAGE_SEVERITY_CORRUPTION,
        DXGI_INFO_QUEUE_MESSAGE_SEVERITY_ERROR,
        DXGI_INFO_QUEUE_MESSAGE_SEVERITY_WARNING,
        DXGI_INFO_QUEUE_MESSAGE_SEVERITY_INFO,
        DXGI_INFO_QUEUE_MESSAGE_SEVERITY_MESSAGE,
    }
}
