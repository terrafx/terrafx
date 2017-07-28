// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_JPEG_DC_HUFFMAN_TABLE
    {
        #region Fields
        [ComAliasName("BYTE[12]")]
        public _CodeCounts_e__FixedBuffer CodeCounts;

        [ComAliasName("BYTE[12]")]
        public _CodeValues_e__FixedBuffer CodeValues;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _CodeCounts_e__FixedBuffer
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
            #endregion

            #region Properties
            public byte this[int index]
            {
                get
                {
                    if ((uint)(index) > 11) // (index < 0) || (index > 11)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (byte* e = &e0)
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
            #endregion

            #region Properties
            public byte this[int index]
            {
                get
                {
                    if ((uint)(index) > 11) // (index < 0) || (index > 11)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (byte* e = &e0)
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
