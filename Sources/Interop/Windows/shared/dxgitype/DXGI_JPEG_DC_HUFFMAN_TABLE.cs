// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_JPEG_DC_HUFFMAN_TABLE
    {
        #region Fields
        public _CodeCounts_e__FixedBuffer CodeCounts;

        public _CodeValues_e__FixedBuffer CodeValues;
        #endregion

        #region Structs
        public /* blittable */ struct _CodeCounts_e__FixedBuffer
        {
            #region Fields
            public BYTE _0;

            public BYTE _1;

            public BYTE _2;

            public BYTE _3;

            public BYTE _4;

            public BYTE _5;

            public BYTE _6;

            public BYTE _7;

            public BYTE _8;

            public BYTE _9;

            public BYTE _10;

            public BYTE _11;
            #endregion
        }

        public /* blittable */ struct _CodeValues_e__FixedBuffer
        {
            #region Fields
            public BYTE _0;

            public BYTE _1;

            public BYTE _2;

            public BYTE _3;

            public BYTE _4;

            public BYTE _5;

            public BYTE _6;

            public BYTE _7;

            public BYTE _8;

            public BYTE _9;

            public BYTE _10;

            public BYTE _11;
            #endregion
        }
        #endregion
    }
}
