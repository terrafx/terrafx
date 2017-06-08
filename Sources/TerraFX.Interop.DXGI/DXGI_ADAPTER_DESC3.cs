// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct DXGI_ADAPTER_DESC3
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public ushort[] Description;
        public uint VendorId;
        public uint DeviceId;
        public uint SubSysId;
        public uint Revision;
        public ulong DedicatedVideoMemory;
        public ulong DedicatedSystemMemory;
        public ulong SharedSystemMemory;
        public long AdapterLuid;
        public DXGI_ADAPTER_FLAG3 Flags;
        public DXGI_GRAPHICS_PREEMPTION_GRANULARITY GraphicsPreemptionGranularity;
        public DXGI_COMPUTE_PREEMPTION_GRANULARITY ComputePreemptionGranularity;
    }
}
