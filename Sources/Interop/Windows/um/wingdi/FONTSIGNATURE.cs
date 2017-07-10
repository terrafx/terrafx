// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wingdi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct FONTSIGNATURE
    {
        #region Fields
        public _fsUsb_e__FixedBuffer fsUsb;

        public _fsCsb_e__FixedBuffer fsCsb;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _fsUsb_e__FixedBuffer
        {
            #region Fields
            public DWORD e0;

            public DWORD e1;

            public DWORD e2;

            public DWORD e3;
            #endregion

            #region Properties
            public DWORD this[int index]
            {
                get
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (DWORD* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _fsCsb_e__FixedBuffer
        {
            #region Fields
            public DWORD e0;

            public DWORD e1;
            #endregion

            #region Properties
            public DWORD this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (DWORD* e = &e0)
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
