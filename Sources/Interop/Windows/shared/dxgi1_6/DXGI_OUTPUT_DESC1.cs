// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_OUTPUT_DESC1
    {
        #region Fields
        public _DeviceName_e__FixedBuffer DeviceName;

        public RECT DesktopCoordinates;

        public BOOL AttachedToDesktop;

        public DXGI_MODE_ROTATION Rotation;

        public HMONITOR Monitor;

        public UINT BitsPerColor;

        public DXGI_COLOR_SPACE_TYPE ColorSpace;

        public _RedPrimary_e__FixedBuffer RedPrimary;

        public _GreenPrimary_e__FixedBuffer GreenPrimary;

        public _BluePrimary_e__FixedBuffer BluePrimary;

        public _WhitePoint_e__FixedBuffer WhitePoint;

        public FLOAT MinLuminance;

        public FLOAT MaxLuminance;

        public FLOAT MaxFullFrameLuminance;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _DeviceName_e__FixedBuffer
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

        unsafe public /* blittable */ struct _RedPrimary_e__FixedBuffer
        {
            #region Fields
            public FLOAT e0;

            public FLOAT e1;
            #endregion

            #region Properties
            public FLOAT this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (FLOAT* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _GreenPrimary_e__FixedBuffer
        {
            #region Fields
            public FLOAT e0;

            public FLOAT e1;
            #endregion

            #region Properties
            public FLOAT this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (FLOAT* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _BluePrimary_e__FixedBuffer
        {
            #region Fields
            public FLOAT e0;

            public FLOAT e1;
            #endregion

            #region Properties
            public FLOAT this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (FLOAT* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _WhitePoint_e__FixedBuffer
        {
            #region Fields
            public FLOAT e0;

            public FLOAT e1;
            #endregion

            #region Properties
            public FLOAT this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (FLOAT* e = &e0)
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
