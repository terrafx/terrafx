// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_JPEG_DC_HUFFMAN_TABLE
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
            #endregion

            #region Properties
            public BYTE this[int index]
            {
                get
                {
                    if ((uint)(index) > 11) // (index < 0) || (index > 11)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
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
            #endregion

            #region Properties
            public BYTE this[int index]
            {
                get
                {
                    if ((uint)(index) > 11) // (index < 0) || (index > 11)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
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
