// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)] // Size = 300 or 308
    unsafe public struct DXGI_ADAPTER_DESC1
    {
        #region Fields
        public fixed ushort Description[128];

        public uint VendorId;

        public uint DeviceId;

        public uint SubSysId;

        public uint Revision;

        public UIntPtr DedicatedVideoMemory;

        public UIntPtr DedicatedSystemMemory;

        public UIntPtr SharedSystemMemory;

        public long /* LUID */ AdapterLuid;

        public DXGI_ADAPTER_FLAG Flags;
        #endregion
    }
}
