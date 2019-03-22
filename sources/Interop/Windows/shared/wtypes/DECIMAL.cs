// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    [Unmanaged]
    public struct DECIMAL
    {
        #region Fields
        [FieldOffset(0)]
        public ushort wReserved;

        #region union
        #region struct
        [FieldOffset(2)]
        [NativeTypeName("BYTE")]
        public byte scale;

        [FieldOffset(3)]
        [NativeTypeName("BYTE")]
        public byte sign;
        #endregion

        [FieldOffset(2)]
        [NativeTypeName("USHORT")]
        public ushort signscale;
        #endregion

        [FieldOffset(4)]
        [NativeTypeName("ULONG")]
        public uint Hi32;

        #region union
        #region struct
        [FieldOffset(8)]
        [NativeTypeName("ULONG")]
        public uint Lo32;

        [FieldOffset(12)]
        [NativeTypeName("ULONG")]
        public uint Mid32;
        #endregion

        [FieldOffset(8)]
        [NativeTypeName("ULONGLONG")]
        public ulong Lo64;
        #endregion
        #endregion
    }
}
