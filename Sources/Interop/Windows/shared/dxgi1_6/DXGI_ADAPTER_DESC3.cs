// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct DXGI_ADAPTER_DESC3
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

        public DXGI_ADAPTER_FLAG3 Flags;

        public DXGI_GRAPHICS_PREEMPTION_GRANULARITY GraphicsPreemptionGranularity;

        public DXGI_COMPUTE_PREEMPTION_GRANULARITY ComputePreemptionGranularity;
        #endregion

        #region Structs
        public /* blittable */ struct _Description_e__FixedBuffer
        {
            #region Fields
            public WCHAR _0;

            public WCHAR _1;

            public WCHAR _2;

            public WCHAR _3;

            public WCHAR _4;

            public WCHAR _5;

            public WCHAR _6;

            public WCHAR _7;

            public WCHAR _8;

            public WCHAR _9;

            public WCHAR _10;

            public WCHAR _11;

            public WCHAR _12;

            public WCHAR _13;

            public WCHAR _14;

            public WCHAR _15;

            public WCHAR _16;

            public WCHAR _17;

            public WCHAR _18;

            public WCHAR _19;

            public WCHAR _20;

            public WCHAR _21;

            public WCHAR _22;

            public WCHAR _23;

            public WCHAR _24;

            public WCHAR _25;

            public WCHAR _26;

            public WCHAR _27;

            public WCHAR _28;

            public WCHAR _29;

            public WCHAR _30;

            public WCHAR _31;

            public WCHAR _32;

            public WCHAR _33;

            public WCHAR _34;

            public WCHAR _35;

            public WCHAR _36;

            public WCHAR _37;

            public WCHAR _38;

            public WCHAR _39;

            public WCHAR _40;

            public WCHAR _41;

            public WCHAR _42;

            public WCHAR _43;

            public WCHAR _44;

            public WCHAR _45;

            public WCHAR _46;

            public WCHAR _47;

            public WCHAR _48;

            public WCHAR _49;

            public WCHAR _50;

            public WCHAR _51;

            public WCHAR _52;

            public WCHAR _53;

            public WCHAR _54;

            public WCHAR _55;

            public WCHAR _56;

            public WCHAR _57;

            public WCHAR _58;

            public WCHAR _59;

            public WCHAR _60;

            public WCHAR _61;

            public WCHAR _62;

            public WCHAR _63;

            public WCHAR _64;

            public WCHAR _65;

            public WCHAR _66;

            public WCHAR _67;

            public WCHAR _68;

            public WCHAR _69;

            public WCHAR _70;

            public WCHAR _71;

            public WCHAR _72;

            public WCHAR _73;

            public WCHAR _74;

            public WCHAR _75;

            public WCHAR _76;

            public WCHAR _77;

            public WCHAR _78;

            public WCHAR _79;

            public WCHAR _80;

            public WCHAR _81;

            public WCHAR _82;

            public WCHAR _83;

            public WCHAR _84;

            public WCHAR _85;

            public WCHAR _86;

            public WCHAR _87;

            public WCHAR _88;

            public WCHAR _89;

            public WCHAR _90;

            public WCHAR _91;

            public WCHAR _92;

            public WCHAR _93;

            public WCHAR _94;

            public WCHAR _95;

            public WCHAR _96;

            public WCHAR _97;

            public WCHAR _98;

            public WCHAR _99;

            public WCHAR _100;

            public WCHAR _101;

            public WCHAR _102;

            public WCHAR _103;

            public WCHAR _104;

            public WCHAR _105;

            public WCHAR _106;

            public WCHAR _107;

            public WCHAR _108;

            public WCHAR _109;

            public WCHAR _110;

            public WCHAR _111;

            public WCHAR _112;

            public WCHAR _113;

            public WCHAR _114;

            public WCHAR _115;

            public WCHAR _116;

            public WCHAR _117;

            public WCHAR _118;

            public WCHAR _119;

            public WCHAR _120;

            public WCHAR _121;

            public WCHAR _122;

            public WCHAR _123;

            public WCHAR _124;

            public WCHAR _125;

            public WCHAR _126;

            public WCHAR _127;
            #endregion
        }
        #endregion
    }
}
