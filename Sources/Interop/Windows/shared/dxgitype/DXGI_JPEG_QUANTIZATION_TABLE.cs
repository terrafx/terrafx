// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_JPEG_QUANTIZATION_TABLE
    {
        #region Fields
        public _Elements_e__FixedBuffer Elements;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _Elements_e__FixedBuffer
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

            public BYTE e12;

            public BYTE e13;

            public BYTE e14;

            public BYTE e15;

            public BYTE e16;

            public BYTE e17;

            public BYTE e18;

            public BYTE e19;

            public BYTE e20;

            public BYTE e21;

            public BYTE e22;

            public BYTE e23;

            public BYTE e24;

            public BYTE e25;

            public BYTE e26;

            public BYTE e27;

            public BYTE e28;

            public BYTE e29;

            public BYTE e30;

            public BYTE e31;

            public BYTE e32;

            public BYTE e33;

            public BYTE e34;

            public BYTE e35;

            public BYTE e36;

            public BYTE e37;

            public BYTE e38;

            public BYTE e39;

            public BYTE e40;

            public BYTE e41;

            public BYTE e42;

            public BYTE e43;

            public BYTE e44;

            public BYTE e45;

            public BYTE e46;

            public BYTE e47;

            public BYTE e48;

            public BYTE e49;

            public BYTE e50;

            public BYTE e51;

            public BYTE e52;

            public BYTE e53;

            public BYTE e54;

            public BYTE e55;

            public BYTE e56;

            public BYTE e57;

            public BYTE e58;

            public BYTE e59;

            public BYTE e60;

            public BYTE e61;

            public BYTE e62;

            public BYTE e63;
            #endregion

            #region Properties
            public BYTE this[int index]
            {
                get
                {
                    if ((uint)(index) > 63) // (index < 0) || (index > 63)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
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
