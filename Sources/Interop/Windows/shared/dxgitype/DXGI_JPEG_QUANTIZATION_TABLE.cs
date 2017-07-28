// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_JPEG_QUANTIZATION_TABLE
    {
        #region Fields
        [ComAliasName("BYTE[64]")]
        public _Elements_e__FixedBuffer Elements;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _Elements_e__FixedBuffer
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

            public byte e12;

            public byte e13;

            public byte e14;

            public byte e15;

            public byte e16;

            public byte e17;

            public byte e18;

            public byte e19;

            public byte e20;

            public byte e21;

            public byte e22;

            public byte e23;

            public byte e24;

            public byte e25;

            public byte e26;

            public byte e27;

            public byte e28;

            public byte e29;

            public byte e30;

            public byte e31;

            public byte e32;

            public byte e33;

            public byte e34;

            public byte e35;

            public byte e36;

            public byte e37;

            public byte e38;

            public byte e39;

            public byte e40;

            public byte e41;

            public byte e42;

            public byte e43;

            public byte e44;

            public byte e45;

            public byte e46;

            public byte e47;

            public byte e48;

            public byte e49;

            public byte e50;

            public byte e51;

            public byte e52;

            public byte e53;

            public byte e54;

            public byte e55;

            public byte e56;

            public byte e57;

            public byte e58;

            public byte e59;

            public byte e60;

            public byte e61;

            public byte e62;

            public byte e63;
            #endregion

            #region Properties
            public byte this[int index]
            {
                get
                {
                    if ((uint)(index) > 63) // (index < 0) || (index > 63)
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
