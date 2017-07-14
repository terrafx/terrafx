// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_ADAPTER_DESC
    {
        #region Fields
        public _Description_e__FixedBuffer Description;

        public UINT VendorId;

        public UINT DeviceId;

        public UINT SubSysId;

        public UINT Revision;

        public SIZE_T DedicatedVideoMemory;

        public SIZE_T DedicatedSystemMemory;

        public SIZE_T SharedSystemMemory;

        public LUID AdapterLuid;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _Description_e__FixedBuffer
        {
            #region Fields
            public WCHAR e0;

            public WCHAR e1;

            public WCHAR e2;

            public WCHAR e3;

            public WCHAR e4;

            public WCHAR e5;

            public WCHAR e6;

            public WCHAR e7;

            public WCHAR e8;

            public WCHAR e9;

            public WCHAR e10;

            public WCHAR e11;

            public WCHAR e12;

            public WCHAR e13;

            public WCHAR e14;

            public WCHAR e15;

            public WCHAR e16;

            public WCHAR e17;

            public WCHAR e18;

            public WCHAR e19;

            public WCHAR e20;

            public WCHAR e21;

            public WCHAR e22;

            public WCHAR e23;

            public WCHAR e24;

            public WCHAR e25;

            public WCHAR e26;

            public WCHAR e27;

            public WCHAR e28;

            public WCHAR e29;

            public WCHAR e30;

            public WCHAR e31;

            public WCHAR e32;

            public WCHAR e33;

            public WCHAR e34;

            public WCHAR e35;

            public WCHAR e36;

            public WCHAR e37;

            public WCHAR e38;

            public WCHAR e39;

            public WCHAR e40;

            public WCHAR e41;

            public WCHAR e42;

            public WCHAR e43;

            public WCHAR e44;

            public WCHAR e45;

            public WCHAR e46;

            public WCHAR e47;

            public WCHAR e48;

            public WCHAR e49;

            public WCHAR e50;

            public WCHAR e51;

            public WCHAR e52;

            public WCHAR e53;

            public WCHAR e54;

            public WCHAR e55;

            public WCHAR e56;

            public WCHAR e57;

            public WCHAR e58;

            public WCHAR e59;

            public WCHAR e60;

            public WCHAR e61;

            public WCHAR e62;

            public WCHAR e63;

            public WCHAR e64;

            public WCHAR e65;

            public WCHAR e66;

            public WCHAR e67;

            public WCHAR e68;

            public WCHAR e69;

            public WCHAR e70;

            public WCHAR e71;

            public WCHAR e72;

            public WCHAR e73;

            public WCHAR e74;

            public WCHAR e75;

            public WCHAR e76;

            public WCHAR e77;

            public WCHAR e78;

            public WCHAR e79;

            public WCHAR e80;

            public WCHAR e81;

            public WCHAR e82;

            public WCHAR e83;

            public WCHAR e84;

            public WCHAR e85;

            public WCHAR e86;

            public WCHAR e87;

            public WCHAR e88;

            public WCHAR e89;

            public WCHAR e90;

            public WCHAR e91;

            public WCHAR e92;

            public WCHAR e93;

            public WCHAR e94;

            public WCHAR e95;

            public WCHAR e96;

            public WCHAR e97;

            public WCHAR e98;

            public WCHAR e99;

            public WCHAR e100;

            public WCHAR e101;

            public WCHAR e102;

            public WCHAR e103;

            public WCHAR e104;

            public WCHAR e105;

            public WCHAR e106;

            public WCHAR e107;

            public WCHAR e108;

            public WCHAR e109;

            public WCHAR e110;

            public WCHAR e111;

            public WCHAR e112;

            public WCHAR e113;

            public WCHAR e114;

            public WCHAR e115;

            public WCHAR e116;

            public WCHAR e117;

            public WCHAR e118;

            public WCHAR e119;

            public WCHAR e120;

            public WCHAR e121;

            public WCHAR e122;

            public WCHAR e123;

            public WCHAR e124;

            public WCHAR e125;

            public WCHAR e126;

            public WCHAR e127;
            #endregion

            #region Properties
            public WCHAR this[int index]
            {
                get
                {
                    if ((uint)(index) > 127) // (index < 0) || (index > 127)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (WCHAR* e = &e0)
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
