// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [Unmanaged]
    public unsafe struct DXGI_ADAPTER_DESC2
    {
        #region Fields
        [ComAliasName("WCHAR[128]")]
        public fixed char Description[128];

        [ComAliasName("UINT")]
        public uint VendorId;

        [ComAliasName("UINT")]
        public uint DeviceId;

        [ComAliasName("UINT")]
        public uint SubSysId;

        [ComAliasName("UINT")]
        public uint Revision;

        [ComAliasName("SIZE_T")]
        public nuint DedicatedVideoMemory;

        [ComAliasName("SIZE_T")]
        public nuint DedicatedSystemMemory;

        [ComAliasName("SIZE_T")]
        public nuint SharedSystemMemory;

        public LUID AdapterLuid;

        [ComAliasName("UINT")]
        public uint Flags;

        public DXGI_GRAPHICS_PREEMPTION_GRANULARITY GraphicsPreemptionGranularity;

        public DXGI_COMPUTE_PREEMPTION_GRANULARITY ComputePreemptionGranularity;
        #endregion
    }
}
