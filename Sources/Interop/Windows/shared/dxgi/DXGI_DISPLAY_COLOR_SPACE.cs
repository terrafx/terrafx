// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_DISPLAY_COLOR_SPACE
    {
        #region Fields
        public _PrimaryCoordinates_e__FixedBuffer PrimaryCoordinates;

        public _WhitePoints_e__FixedBuffer WhitePoints;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _PrimaryCoordinates_e__FixedBuffer
        {
            #region Fields
            public FLOAT e0_0, e0_1;

            public FLOAT e1_0, e1_1;

            public FLOAT e2_0, e2_1;

            public FLOAT e3_0, e3_1;

            public FLOAT e4_0, e4_1;

            public FLOAT e5_0, e5_1;

            public FLOAT e6_0, e6_1;

            public FLOAT e7_0, e7_1;
            #endregion

            #region Properties
            public FLOAT this[int index1, int index2]
            {
                get
                {
                    if ((uint)(index1) > 7) // (index1 < 0) || (index1 > 7)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index1), index1);
                    }

                    if ((uint)(index2) > 1) // (index2 < 0) || (index2 > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index2), index2);
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
            public FLOAT e0_0, e0_1;

            public FLOAT e1_0, e1_1;

            public FLOAT e2_0, e2_1;

            public FLOAT e3_0, e3_1;

            public FLOAT e4_0, e4_1;

            public FLOAT e5_0, e5_1;

            public FLOAT e6_0, e6_1;

            public FLOAT e7_0, e7_1;

            public FLOAT e8_0, e8_1;

            public FLOAT e9_0, e9_1;

            public FLOAT e10_0, e10_1;

            public FLOAT e11_0, e11_1;

            public FLOAT e12_0, e12_1;

            public FLOAT e13_0, e13_1;

            public FLOAT e14_0, e14_1;

            public FLOAT e15_0, e15_1;
            #endregion

            #region Properties
            public FLOAT this[int index1, int index2]
            {
                get
                {
                    if ((uint)(index1) > 15) // (index1 < 0) || (index1 > 15)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index1), index1);
                    }

                    if ((uint)(index2) > 1) // (index2 < 0) || (index2 > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index2), index2);
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
