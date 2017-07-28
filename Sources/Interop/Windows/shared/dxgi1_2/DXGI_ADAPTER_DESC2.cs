// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_ADAPTER_DESC2
    {
        #region Fields
        [ComAliasName("WCHAR[128]")]
        public _Description_e__FixedBuffer Description;

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

        #region Structs
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        unsafe public /* blittable */ struct _Description_e__FixedBuffer
        {
            #region Fields
            public char e0;

            public char e1;

            public char e2;

            public char e3;

            public char e4;

            public char e5;

            public char e6;

            public char e7;

            public char e8;

            public char e9;

            public char e10;

            public char e11;

            public char e12;

            public char e13;

            public char e14;

            public char e15;

            public char e16;

            public char e17;

            public char e18;

            public char e19;

            public char e20;

            public char e21;

            public char e22;

            public char e23;

            public char e24;

            public char e25;

            public char e26;

            public char e27;

            public char e28;

            public char e29;

            public char e30;

            public char e31;

            public char e32;

            public char e33;

            public char e34;

            public char e35;

            public char e36;

            public char e37;

            public char e38;

            public char e39;

            public char e40;

            public char e41;

            public char e42;

            public char e43;

            public char e44;

            public char e45;

            public char e46;

            public char e47;

            public char e48;

            public char e49;

            public char e50;

            public char e51;

            public char e52;

            public char e53;

            public char e54;

            public char e55;

            public char e56;

            public char e57;

            public char e58;

            public char e59;

            public char e60;

            public char e61;

            public char e62;

            public char e63;

            public char e64;

            public char e65;

            public char e66;

            public char e67;

            public char e68;

            public char e69;

            public char e70;

            public char e71;

            public char e72;

            public char e73;

            public char e74;

            public char e75;

            public char e76;

            public char e77;

            public char e78;

            public char e79;

            public char e80;

            public char e81;

            public char e82;

            public char e83;

            public char e84;

            public char e85;

            public char e86;

            public char e87;

            public char e88;

            public char e89;

            public char e90;

            public char e91;

            public char e92;

            public char e93;

            public char e94;

            public char e95;

            public char e96;

            public char e97;

            public char e98;

            public char e99;

            public char e100;

            public char e101;

            public char e102;

            public char e103;

            public char e104;

            public char e105;

            public char e106;

            public char e107;

            public char e108;

            public char e109;

            public char e110;

            public char e111;

            public char e112;

            public char e113;

            public char e114;

            public char e115;

            public char e116;

            public char e117;

            public char e118;

            public char e119;

            public char e120;

            public char e121;

            public char e122;

            public char e123;

            public char e124;

            public char e125;

            public char e126;

            public char e127;
            #endregion

            #region Properties
            public char this[int index]
            {
                get
                {
                    if ((uint)(index) > 127) // (index < 0) || (index > 127)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (char* e = &e0)
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
