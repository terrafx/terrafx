// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_HDR_METADATA_HDR10
    {
        #region Fields
        public _RedPrimary_e__FixedBuffer RedPrimary;

        public _GreenPrimary_e__FixedBuffer GreenPrimary;

        public _BluePrimary_e__FixedBuffer BluePrimary;

        public _WhitePoint_e__FixedBuffer WhitePoint;

        public UINT MaxMasteringLuminance;

        public UINT MinMasteringLuminance;

        public UINT16 MaxContentLightLevel;

        public UINT16 MaxFrameAverageLightLevel;
        #endregion

        #region Structs
        public struct _RedPrimary_e__FixedBuffer
        {
            #region Fields
            public UINT16 _0;

            public UINT16 _1;
            #endregion
        }

        public struct _GreenPrimary_e__FixedBuffer
        {
            #region Fields
            public UINT16 _0;

            public UINT16 _1;
            #endregion
        }

        public struct _BluePrimary_e__FixedBuffer
        {
            #region Fields
            public UINT16 _0;

            public UINT16 _1;
            #endregion
        }

        public struct _WhitePoint_e__FixedBuffer
        {
            #region Fields
            public UINT16 _0;

            public UINT16 _1;
            #endregion
        }
        #endregion
    }
}
