// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct DXGI_HDR_METADATA_HDR10
    {
        #region Fields
        [ComAliasName("UINT16[2]")]
        public fixed ushort RedPrimary[2];

        [ComAliasName("UINT16[2]")]
        public fixed ushort GreenPrimary[2];

        [ComAliasName("UINT16[2]")]
        public fixed ushort BluePrimary[2];

        [ComAliasName("UINT16[2]")]
        public fixed ushort WhitePoint[2];

        [ComAliasName("UINT")]
        public uint MaxMasteringLuminance;

        [ComAliasName("UINT")]
        public uint MinMasteringLuminance;

        [ComAliasName("UINT16")]
        public ushort MaxContentLightLevel;

        [ComAliasName("UINT16")]
        public ushort MaxFrameAverageLightLevel;
        #endregion
    }
}
