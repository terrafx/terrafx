// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_DISPLAY_COLOR_SPACE
    {
        #region Fields
        public _PrimaryCoordinates_e__FixedBuffer PrimaryCoordinates;

        public _WhitePoints_e__FixedBuffer WhitePoints;
        #endregion

        #region Struct
        public /* blittable */ struct _PrimaryCoordinates_e__FixedBuffer
        {
            #region Fields
            #region 0
            public FLOAT _0_0;
            public FLOAT _0_1;
            #endregion

            #region 1
            public FLOAT _1_0;
            public FLOAT _1_1;
            #endregion

            #region 2
            public FLOAT _2_0;
            public FLOAT _2_1;
            #endregion

            #region 3
            public FLOAT _3_0;
            public FLOAT _3_1;
            #endregion

            #region 4
            public FLOAT _4_0;
            public FLOAT _4_1;
            #endregion

            #region 5
            public FLOAT _5_0;
            public FLOAT _5_1;
            #endregion

            #region 6
            public FLOAT _6_0;
            public FLOAT _6_1;
            #endregion

            #region 7
            public FLOAT _7_0;
            public FLOAT _7_1;
            #endregion
            #endregion
        }

        public /* blittable */ struct _WhitePoints_e__FixedBuffer
        {
            #region Fields
            #region 0
            public FLOAT _0_0;
            public FLOAT _0_1;
            #endregion

            #region 1
            public FLOAT _1_0;
            public FLOAT _1_1;
            #endregion

            #region 2
            public FLOAT _2_0;
            public FLOAT _2_1;
            #endregion

            #region 3
            public FLOAT _3_0;
            public FLOAT _3_1;
            #endregion

            #region 4
            public FLOAT _4_0;
            public FLOAT _4_1;
            #endregion

            #region 5
            public FLOAT _5_0;
            public FLOAT _5_1;
            #endregion

            #region 6
            public FLOAT _6_0;
            public FLOAT _6_1;
            #endregion

            #region 7
            public FLOAT _7_0;
            public FLOAT _7_1;
            #endregion

            #region 8
            public FLOAT _8_0;
            public FLOAT _8_1;
            #endregion

            #region 9
            public FLOAT _9_0;
            public FLOAT _9_1;
            #endregion

            #region 10
            public FLOAT _10_0;
            public FLOAT _10_1;
            #endregion

            #region 11
            public FLOAT _11_0;
            public FLOAT _11_1;
            #endregion

            #region 12
            public FLOAT _12_0;
            public FLOAT _12_1;
            #endregion

            #region 13
            public FLOAT _13_0;
            public FLOAT _13_1;
            #endregion

            #region 14
            public FLOAT _14_0;
            public FLOAT _14_1;
            #endregion

            #region 15
            public FLOAT _15_0;
            public FLOAT _15_1;
            #endregion
            #endregion
        }
        #endregion
    }
}
