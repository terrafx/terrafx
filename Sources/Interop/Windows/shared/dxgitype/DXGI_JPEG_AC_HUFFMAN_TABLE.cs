// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_JPEG_AC_HUFFMAN_TABLE
    {
        #region Fields
        public _CodeCounts_e__FixedBuffer CodeCounts;

        public _CodeValues_e__FixedBuffer CodeValues;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _CodeCounts_e__FixedBuffer
        {
            #region Fields
            public BYTE e0;

            public BYTE e1;

            public BYTE e2;

            public BYTE e3;

            public BYTE e4;

            public BYTE e5;

            public BYTE e6;

            public BYTE e7;

            public BYTE e8;

            public BYTE e9;

            public BYTE e10;

            public BYTE e11;

            public BYTE e12;

            public BYTE e13;

            public BYTE e14;

            public BYTE e15;
            #endregion

            #region Properties
            public BYTE this[int index]
            {
                get
                {
                    if ((uint)(index) > 15) // (index < 0) || (index > 15)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (BYTE* e = &e0)
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
            public BYTE e0;

            public BYTE e1;

            public BYTE e2;

            public BYTE e3;

            public BYTE e4;

            public BYTE e5;

            public BYTE e6;

            public BYTE e7;

            public BYTE e8;

            public BYTE e9;

            public BYTE e10;

            public BYTE e11;

            public BYTE e12;

            public BYTE e13;

            public BYTE e14;

            public BYTE e15;

            public BYTE e16;

            public BYTE e17;

            public BYTE e18;

            public BYTE e19;

            public BYTE e20;

            public BYTE e21;

            public BYTE e22;

            public BYTE e23;

            public BYTE e24;

            public BYTE e25;

            public BYTE e26;

            public BYTE e27;

            public BYTE e28;

            public BYTE e29;

            public BYTE e30;

            public BYTE e31;

            public BYTE e32;

            public BYTE e33;

            public BYTE e34;

            public BYTE e35;

            public BYTE e36;

            public BYTE e37;

            public BYTE e38;

            public BYTE e39;

            public BYTE e40;

            public BYTE e41;

            public BYTE e42;

            public BYTE e43;

            public BYTE e44;

            public BYTE e45;

            public BYTE e46;

            public BYTE e47;

            public BYTE e48;

            public BYTE e49;

            public BYTE e50;

            public BYTE e51;

            public BYTE e52;

            public BYTE e53;

            public BYTE e54;

            public BYTE e55;

            public BYTE e56;

            public BYTE e57;

            public BYTE e58;

            public BYTE e59;

            public BYTE e60;

            public BYTE e61;

            public BYTE e62;

            public BYTE e63;

            public BYTE e64;

            public BYTE e65;

            public BYTE e66;

            public BYTE e67;

            public BYTE e68;

            public BYTE e69;

            public BYTE e70;

            public BYTE e71;

            public BYTE e72;

            public BYTE e73;

            public BYTE e74;

            public BYTE e75;

            public BYTE e76;

            public BYTE e77;

            public BYTE e78;

            public BYTE e79;

            public BYTE e80;

            public BYTE e81;

            public BYTE e82;

            public BYTE e83;

            public BYTE e84;

            public BYTE e85;

            public BYTE e86;

            public BYTE e87;

            public BYTE e88;

            public BYTE e89;

            public BYTE e90;

            public BYTE e91;

            public BYTE e92;

            public BYTE e93;

            public BYTE e94;

            public BYTE e95;

            public BYTE e96;

            public BYTE e97;

            public BYTE e98;

            public BYTE e99;

            public BYTE e100;

            public BYTE e101;

            public BYTE e102;

            public BYTE e103;

            public BYTE e104;

            public BYTE e105;

            public BYTE e106;

            public BYTE e107;

            public BYTE e108;

            public BYTE e109;

            public BYTE e110;

            public BYTE e111;

            public BYTE e112;

            public BYTE e113;

            public BYTE e114;

            public BYTE e115;

            public BYTE e116;

            public BYTE e117;

            public BYTE e118;

            public BYTE e119;

            public BYTE e120;

            public BYTE e121;

            public BYTE e122;

            public BYTE e123;

            public BYTE e124;

            public BYTE e125;

            public BYTE e126;

            public BYTE e127;

            public BYTE e128;

            public BYTE e129;

            public BYTE e130;

            public BYTE e131;

            public BYTE e132;

            public BYTE e133;

            public BYTE e134;

            public BYTE e135;

            public BYTE e136;

            public BYTE e137;

            public BYTE e138;

            public BYTE e139;

            public BYTE e140;

            public BYTE e141;

            public BYTE e142;

            public BYTE e143;

            public BYTE e144;

            public BYTE e145;

            public BYTE e146;

            public BYTE e147;

            public BYTE e148;

            public BYTE e149;

            public BYTE e150;

            public BYTE e151;

            public BYTE e152;

            public BYTE e153;

            public BYTE e154;

            public BYTE e155;

            public BYTE e156;

            public BYTE e157;

            public BYTE e158;

            public BYTE e159;

            public BYTE e160;

            public BYTE e161;
            #endregion

            #region Properties
            public BYTE this[int index]
            {
                get
                {
                    if ((uint)(index) > 161) // (index < 0) || (index > 161)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (BYTE* e = &e0)
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
