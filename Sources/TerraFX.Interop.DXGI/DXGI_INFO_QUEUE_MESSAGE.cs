// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct DXGI_INFO_QUEUE_MESSAGE
    {
        public Guid Producer;
        public DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category;
        public DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity;
        public int ID;
        public IntPtr pDescription;
        public ulong DescriptionByteLength;
    }
}
