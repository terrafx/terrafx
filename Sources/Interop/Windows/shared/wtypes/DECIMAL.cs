// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct DECIMAL
    {
        #region Fields
        [FieldOffset(0)]
        public ushort wReserved;

        #region union
        #region struct
        [FieldOffset(2)]
        public BYTE scale;

        [FieldOffset(3)]
        public BYTE sign;
        #endregion

        [FieldOffset(2)]
        public USHORT signscale;
        #endregion

        [FieldOffset(4)]
        public ULONG Hi32;

        #region union
        #region struct
        [FieldOffset(8)]
        public ULONG Lo32;

        [FieldOffset(12)]
        public ULONG Mid32;
        #endregion

        [FieldOffset(8)]
        public ULONGLONG Lo64;
        #endregion
        #endregion
    }
}
