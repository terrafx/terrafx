// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct DXGI_OUTPUT_DESC
    {
        #region Fields
        [ComAliasName("WCHAR[32]")]
        public _DeviceName_e__FixedBuffer DeviceName;

        public RECT DesktopCoordinates;

        [ComAliasName("BOOL")]
        public int AttachedToDesktop;

        public DXGI_MODE_ROTATION Rotation;

        [ComAliasName("HMONITOR")]
        public void* Monitor;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        unsafe public /* blittable */ struct _DeviceName_e__FixedBuffer
        {
            #region Fields
            public char e0;

            public char e1;

            public char e2;

            public char e3;

            public char e4;

            public char e5;

            public char e6;

            public char e7;

            public char e8;

            public char e9;

            public char e10;

            public char e11;

            public char e12;

            public char e13;

            public char e14;

            public char e15;

            public char e16;

            public char e17;

            public char e18;

            public char e19;

            public char e20;

            public char e21;

            public char e22;

            public char e23;

            public char e24;

            public char e25;

            public char e26;

            public char e27;

            public char e28;

            public char e29;

            public char e30;

            public char e31;
            #endregion

            #region Properties
            public char this[int index]
            {
                get
                {
                    if ((uint)(index) > 31) // (index < 0) || (index > 31)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (char* e = &e0)
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
