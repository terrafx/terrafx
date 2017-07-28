// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_JPEG_AC_HUFFMAN_TABLE
    {
        #region Fields
        [ComAliasName("BYTE[16]")]
        public _CodeCounts_e__FixedBuffer CodeCounts;

        [ComAliasName("BYTE[162]")]
        public _CodeValues_e__FixedBuffer CodeValues;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _CodeCounts_e__FixedBuffer
        {
            #region Fields
            public byte e0;

            public byte e1;

            public byte e2;

            public byte e3;

            public byte e4;

            public byte e5;

            public byte e6;

            public byte e7;

            public byte e8;

            public byte e9;

            public byte e10;

            public byte e11;

            public byte e12;

            public byte e13;

            public byte e14;

            public byte e15;
            #endregion

            #region Properties
            public byte this[int index]
            {
                get
                {
                    if ((uint)(index) > 15) // (index < 0) || (index > 15)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (byte* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _CodeValues_e__FixedBuffer
        {
            #region Fields
            public byte e0;

            public byte e1;

            public byte e2;

            public byte e3;

            public byte e4;

            public byte e5;

            public byte e6;

            public byte e7;

            public byte e8;

            public byte e9;

            public byte e10;

            public byte e11;

            public byte e12;

            public byte e13;

            public byte e14;

            public byte e15;

            public byte e16;

            public byte e17;

            public byte e18;

            public byte e19;

            public byte e20;

            public byte e21;

            public byte e22;

            public byte e23;

            public byte e24;

            public byte e25;

            public byte e26;

            public byte e27;

            public byte e28;

            public byte e29;

            public byte e30;

            public byte e31;

            public byte e32;

            public byte e33;

            public byte e34;

            public byte e35;

            public byte e36;

            public byte e37;

            public byte e38;

            public byte e39;

            public byte e40;

            public byte e41;

            public byte e42;

            public byte e43;

            public byte e44;

            public byte e45;

            public byte e46;

            public byte e47;

            public byte e48;

            public byte e49;

            public byte e50;

            public byte e51;

            public byte e52;

            public byte e53;

            public byte e54;

            public byte e55;

            public byte e56;

            public byte e57;

            public byte e58;

            public byte e59;

            public byte e60;

            public byte e61;

            public byte e62;

            public byte e63;

            public byte e64;

            public byte e65;

            public byte e66;

            public byte e67;

            public byte e68;

            public byte e69;

            public byte e70;

            public byte e71;

            public byte e72;

            public byte e73;

            public byte e74;

            public byte e75;

            public byte e76;

            public byte e77;

            public byte e78;

            public byte e79;

            public byte e80;

            public byte e81;

            public byte e82;

            public byte e83;

            public byte e84;

            public byte e85;

            public byte e86;

            public byte e87;

            public byte e88;

            public byte e89;

            public byte e90;

            public byte e91;

            public byte e92;

            public byte e93;

            public byte e94;

            public byte e95;

            public byte e96;

            public byte e97;

            public byte e98;

            public byte e99;

            public byte e100;

            public byte e101;

            public byte e102;

            public byte e103;

            public byte e104;

            public byte e105;

            public byte e106;

            public byte e107;

            public byte e108;

            public byte e109;

            public byte e110;

            public byte e111;

            public byte e112;

            public byte e113;

            public byte e114;

            public byte e115;

            public byte e116;

            public byte e117;

            public byte e118;

            public byte e119;

            public byte e120;

            public byte e121;

            public byte e122;

            public byte e123;

            public byte e124;

            public byte e125;

            public byte e126;

            public byte e127;

            public byte e128;

            public byte e129;

            public byte e130;

            public byte e131;

            public byte e132;

            public byte e133;

            public byte e134;

            public byte e135;

            public byte e136;

            public byte e137;

            public byte e138;

            public byte e139;

            public byte e140;

            public byte e141;

            public byte e142;

            public byte e143;

            public byte e144;

            public byte e145;

            public byte e146;

            public byte e147;

            public byte e148;

            public byte e149;

            public byte e150;

            public byte e151;

            public byte e152;

            public byte e153;

            public byte e154;

            public byte e155;

            public byte e156;

            public byte e157;

            public byte e158;

            public byte e159;

            public byte e160;

            public byte e161;
            #endregion

            #region Properties
            public byte this[int index]
            {
                get
                {
                    if ((uint)(index) > 161) // (index < 0) || (index > 161)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (byte* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
