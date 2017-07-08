// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_HDR_METADATA_HDR10
    {
        #region Fields
        public _RedPrimary_e__FixedBuffer RedPrimary;

        public _GreenPrimary_e__FixedBuffer GreenPrimary;

        public _BluePrimary_e__FixedBuffer BluePrimary;

        public _WhitePoint_e__FixedBuffer WhitePoint;

        public UINT MaxMasteringLuminance;

        public UINT MinMasteringLuminance;

        public UINT16 MaxContentLightLevel;

        public UINT16 MaxFrameAverageLightLevel;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _RedPrimary_e__FixedBuffer
        {
            #region Fields
            public UINT16 e0;

            public UINT16 e1;
            #endregion

            #region Properties
            public UINT16 this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (UINT16* e = &e0)
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
            public UINT16 e0;

            public UINT16 e1;
            #endregion

            #region Properties
            public UINT16 this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (UINT16* e = &e0)
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
            public UINT16 e0;

            public UINT16 e1;
            #endregion

            #region Properties
            public UINT16 this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (UINT16* e = &e0)
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
            public UINT16 e0;

            public UINT16 e1;
            #endregion

            #region Properties
            public UINT16 this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (UINT16* e = &e0)
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
