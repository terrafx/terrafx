// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_DISPLAY_COLOR_SPACE
    {
        #region Fields
        public _PrimaryCoordinates_e__FixedBuffer PrimaryCoordinates;

        public _WhitePoints_e__FixedBuffer WhitePoints;
        #endregion

        #region Struct
        unsafe public /* blittable */ struct _PrimaryCoordinates_e__FixedBuffer
        {
            #region Fields
            #region e0_*
            public FLOAT e0_0;

            public FLOAT e0_1;
            #endregion

            #region e1_*
            public FLOAT e1_0;

            public FLOAT e1_1;
            #endregion

            #region e2_*
            public FLOAT e2_0;

            public FLOAT e2_1;
            #endregion

            #region e3_*
            public FLOAT e3_0;

            public FLOAT e3_1;
            #endregion

            #region e4_*
            public FLOAT e4_0;

            public FLOAT e4_1;
            #endregion

            #region e5_*
            public FLOAT e5_0;

            public FLOAT e5_1;
            #endregion

            #region e6_*
            public FLOAT e6_0;

            public FLOAT e6_1;
            #endregion

            #region e7_*
            public FLOAT e7_0;

            public FLOAT e7_1;
            #endregion
            #endregion

            #region Properties
            public FLOAT this[int index1, int index2]
            {
                get
                {
                    if ((uint)(index1) > 7) // (index1 < 0) || (index1 > 7)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index1), index1);
                    }

                    if ((uint)(index2) > 1) // (index2 < 0) || (index2 > 1)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index2), index2);
                    }

                    fixed (FLOAT* e = &e0_0)
                    {
                        return e[(index1 * 2) + index2];
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _WhitePoints_e__FixedBuffer
        {
            #region Fields
            #region e0_*
            public FLOAT e0_0;

            public FLOAT e0_1;
            #endregion

            #region e1_*
            public FLOAT e1_0;

            public FLOAT e1_1;
            #endregion

            #region e2_*
            public FLOAT e2_0;

            public FLOAT e2_1;
            #endregion

            #region e3_*
            public FLOAT e3_0;

            public FLOAT e3_1;
            #endregion

            #region e4_*
            public FLOAT e4_0;

            public FLOAT e4_1;
            #endregion

            #region e5_*
            public FLOAT e5_0;

            public FLOAT e5_1;
            #endregion

            #region e6_*
            public FLOAT e6_0;

            public FLOAT e6_1;
            #endregion

            #region e7_*
            public FLOAT e7_0;

            public FLOAT e7_1;
            #endregion

            #region e8_*
            public FLOAT e8_0;

            public FLOAT e8_1;
            #endregion

            #region e9_*
            public FLOAT e9_0;

            public FLOAT e9_1;
            #endregion

            #region e10_*
            public FLOAT e10_0;

            public FLOAT e10_1;
            #endregion

            #region e11_*
            public FLOAT e11_0;

            public FLOAT e11_1;
            #endregion

            #region e12_*
            public FLOAT e12_0;

            public FLOAT e12_1;
            #endregion

            #region e13_*
            public FLOAT e13_0;

            public FLOAT e13_1;
            #endregion

            #region e14_*
            public FLOAT e14_0;
            public FLOAT e14_1;
            #endregion

            #region e15_*
            public FLOAT e15_0;

            public FLOAT e15_1;
            #endregion
            #endregion

            #region Properties
            public FLOAT this[int index1, int index2]
            {
                get
                {
                    if ((uint)(index1) > 15) // (index1 < 0) || (index1 > 15)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index1), index1);
                    }

                    if ((uint)(index2) > 1) // (index2 < 0) || (index2 > 1)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index2), index2);
                    }

                    fixed (FLOAT* e = &e0_0)
                    {
                        return e[(index1 * 2) + index2];
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
