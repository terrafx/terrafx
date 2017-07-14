// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wingdi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct LOGFONT
    {
        #region Fields
        public LONG lfHeight;

        public LONG lfWidth;

        public LONG lfEscapement;

        public LONG lfOrientation;

        public LONG lfWeight;

        public BYTE lfItalic;

        public BYTE lfUnderline;

        public BYTE lfStrikeOut;

        public BYTE lfCharSet;

        public BYTE lfOutPrecision;

        public BYTE lfClipPrecision;

        public BYTE lfQuality;

        public BYTE lfPitchAndFamily;

        public _lfFaceName_e__FixedBuffer lfFaceName;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _lfFaceName_e__FixedBuffer
        {
            #region Fields
            public WCHAR e0;

            public WCHAR e1;

            public WCHAR e2;

            public WCHAR e3;

            public WCHAR e4;

            public WCHAR e5;

            public WCHAR e6;

            public WCHAR e7;

            public WCHAR e8;

            public WCHAR e9;

            public WCHAR e10;

            public WCHAR e11;

            public WCHAR e12;

            public WCHAR e13;

            public WCHAR e14;

            public WCHAR e15;

            public WCHAR e16;

            public WCHAR e17;

            public WCHAR e18;

            public WCHAR e19;

            public WCHAR e20;

            public WCHAR e21;

            public WCHAR e22;

            public WCHAR e23;

            public WCHAR e24;

            public WCHAR e25;

            public WCHAR e26;

            public WCHAR e27;

            public WCHAR e28;

            public WCHAR e29;

            public WCHAR e30;

            public WCHAR e31;
            #endregion

            #region Properties
            public WCHAR this[int index]
            {
                get
                {
                    if ((uint)(index) > 31) // (index < 0) || (index > 31)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (WCHAR* e = &e0)
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
