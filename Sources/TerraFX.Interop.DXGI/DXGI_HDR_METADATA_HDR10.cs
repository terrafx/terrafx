// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_HDR_METADATA_HDR10
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] RedPrimary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] GreenPrimary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] BluePrimary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] WhitePoint;
        public uint MaxMasteringLuminance;
        public uint MinMasteringLuminance;
        public ushort MaxContentLightLevel;
        public ushort MaxFrameAverageLightLevel;
    }
}
