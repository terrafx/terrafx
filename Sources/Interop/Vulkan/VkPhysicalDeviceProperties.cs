// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct VkPhysicalDeviceProperties
    {
        #region Fields
        public uint apiVersion;

        public uint driverVersion;

        public uint vendorID;

        public uint deviceID;

        public VkPhysicalDeviceType deviceType;

        public _deviceName_e__FixedBuffer deviceName;

        public _pipelineCacheUUID_e__FixedBuffer pipelineCacheUUID;

        public VkPhysicalDeviceLimits limits;

        public VkPhysicalDeviceSparseProperties sparseProperties;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _deviceName_e__FixedBuffer
        {
            #region Fields
            public sbyte e0;

            public sbyte e1;

            public sbyte e2;

            public sbyte e3;

            public sbyte e4;

            public sbyte e5;

            public sbyte e6;

            public sbyte e7;

            public sbyte e8;

            public sbyte e9;

            public sbyte e10;

            public sbyte e11;

            public sbyte e12;

            public sbyte e13;

            public sbyte e14;

            public sbyte e15;

            public sbyte e16;

            public sbyte e17;

            public sbyte e18;

            public sbyte e19;

            public sbyte e20;

            public sbyte e21;

            public sbyte e22;

            public sbyte e23;

            public sbyte e24;

            public sbyte e25;

            public sbyte e26;

            public sbyte e27;

            public sbyte e28;

            public sbyte e29;

            public sbyte e30;

            public sbyte e31;

            public sbyte e32;

            public sbyte e33;

            public sbyte e34;

            public sbyte e35;

            public sbyte e36;

            public sbyte e37;

            public sbyte e38;

            public sbyte e39;

            public sbyte e40;

            public sbyte e41;

            public sbyte e42;

            public sbyte e43;

            public sbyte e44;

            public sbyte e45;

            public sbyte e46;

            public sbyte e47;

            public sbyte e48;

            public sbyte e49;

            public sbyte e50;

            public sbyte e51;

            public sbyte e52;

            public sbyte e53;

            public sbyte e54;

            public sbyte e55;

            public sbyte e56;

            public sbyte e57;

            public sbyte e58;

            public sbyte e59;

            public sbyte e60;

            public sbyte e61;

            public sbyte e62;

            public sbyte e63;

            public sbyte e64;

            public sbyte e65;

            public sbyte e66;

            public sbyte e67;

            public sbyte e68;

            public sbyte e69;

            public sbyte e70;

            public sbyte e71;

            public sbyte e72;

            public sbyte e73;

            public sbyte e74;

            public sbyte e75;

            public sbyte e76;

            public sbyte e77;

            public sbyte e78;

            public sbyte e79;

            public sbyte e80;

            public sbyte e81;

            public sbyte e82;

            public sbyte e83;

            public sbyte e84;

            public sbyte e85;

            public sbyte e86;

            public sbyte e87;

            public sbyte e88;

            public sbyte e89;

            public sbyte e90;

            public sbyte e91;

            public sbyte e92;

            public sbyte e93;

            public sbyte e94;

            public sbyte e95;

            public sbyte e96;

            public sbyte e97;

            public sbyte e98;

            public sbyte e99;

            public sbyte e100;

            public sbyte e101;

            public sbyte e102;

            public sbyte e103;

            public sbyte e104;

            public sbyte e105;

            public sbyte e106;

            public sbyte e107;

            public sbyte e108;

            public sbyte e109;

            public sbyte e110;

            public sbyte e111;

            public sbyte e112;

            public sbyte e113;

            public sbyte e114;

            public sbyte e115;

            public sbyte e116;

            public sbyte e117;

            public sbyte e118;

            public sbyte e119;

            public sbyte e120;

            public sbyte e121;

            public sbyte e122;

            public sbyte e123;

            public sbyte e124;

            public sbyte e125;

            public sbyte e126;

            public sbyte e127;

            public sbyte e128;

            public sbyte e129;

            public sbyte e130;

            public sbyte e131;

            public sbyte e132;

            public sbyte e133;

            public sbyte e134;

            public sbyte e135;

            public sbyte e136;

            public sbyte e137;

            public sbyte e138;

            public sbyte e139;

            public sbyte e140;

            public sbyte e141;

            public sbyte e142;

            public sbyte e143;

            public sbyte e144;

            public sbyte e145;

            public sbyte e146;

            public sbyte e147;

            public sbyte e148;

            public sbyte e149;

            public sbyte e150;

            public sbyte e151;

            public sbyte e152;

            public sbyte e153;

            public sbyte e154;

            public sbyte e155;

            public sbyte e156;

            public sbyte e157;

            public sbyte e158;

            public sbyte e159;

            public sbyte e160;

            public sbyte e161;

            public sbyte e162;

            public sbyte e163;

            public sbyte e164;

            public sbyte e165;

            public sbyte e166;

            public sbyte e167;

            public sbyte e168;

            public sbyte e169;

            public sbyte e170;

            public sbyte e171;

            public sbyte e172;

            public sbyte e173;

            public sbyte e174;

            public sbyte e175;

            public sbyte e176;

            public sbyte e177;

            public sbyte e178;

            public sbyte e179;

            public sbyte e180;

            public sbyte e181;

            public sbyte e182;

            public sbyte e183;

            public sbyte e184;

            public sbyte e185;

            public sbyte e186;

            public sbyte e187;

            public sbyte e188;

            public sbyte e189;

            public sbyte e190;

            public sbyte e191;

            public sbyte e192;

            public sbyte e193;

            public sbyte e194;

            public sbyte e195;

            public sbyte e196;

            public sbyte e197;

            public sbyte e198;

            public sbyte e199;

            public sbyte e200;

            public sbyte e201;

            public sbyte e202;

            public sbyte e203;

            public sbyte e204;

            public sbyte e205;

            public sbyte e206;

            public sbyte e207;

            public sbyte e208;

            public sbyte e209;

            public sbyte e210;

            public sbyte e211;

            public sbyte e212;

            public sbyte e213;

            public sbyte e214;

            public sbyte e215;

            public sbyte e216;

            public sbyte e217;

            public sbyte e218;

            public sbyte e219;

            public sbyte e220;

            public sbyte e221;

            public sbyte e222;

            public sbyte e223;

            public sbyte e224;

            public sbyte e225;

            public sbyte e226;

            public sbyte e227;

            public sbyte e228;

            public sbyte e229;

            public sbyte e230;

            public sbyte e231;

            public sbyte e232;

            public sbyte e233;

            public sbyte e234;

            public sbyte e235;

            public sbyte e236;

            public sbyte e237;

            public sbyte e238;

            public sbyte e239;

            public sbyte e240;

            public sbyte e241;

            public sbyte e242;

            public sbyte e243;

            public sbyte e244;

            public sbyte e245;

            public sbyte e246;

            public sbyte e247;

            public sbyte e248;

            public sbyte e249;

            public sbyte e250;

            public sbyte e251;

            public sbyte e252;

            public sbyte e253;

            public sbyte e254;

            public sbyte e255;
            #endregion

            #region Properties
            public sbyte this[int index]
            {
                get
                {
                    if ((uint)(index) > 255) // (index < 0) || (index > 255)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (sbyte* e = &e0)
                    {
                        return e[index];
                    }
                }

                set
                {
                    if ((uint)(index) > 255) // (index < 0) || (index > 255)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (sbyte* e = &e0)
                    {
                        e[index] = value;
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _pipelineCacheUUID_e__FixedBuffer
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

                set
                {
                    if ((uint)(index) > 15) // (index < 0) || (index > 15)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (byte* e = &e0)
                    {
                        e[index] = value;
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
