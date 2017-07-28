// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_HDR_METADATA_HDR10
    {
        #region Fields
        [ComAliasName("UINT16[2]")]
        public _RedPrimary_e__FixedBuffer RedPrimary;

        [ComAliasName("UINT16[2]")]
        public _GreenPrimary_e__FixedBuffer GreenPrimary;

        [ComAliasName("UINT16[2]")]
        public _BluePrimary_e__FixedBuffer BluePrimary;

        [ComAliasName("UINT16[2]")]
        public _WhitePoint_e__FixedBuffer WhitePoint;

        [ComAliasName("UINT")]
        public uint MaxMasteringLuminance;

        [ComAliasName("UINT")]
        public uint MinMasteringLuminance;

        [ComAliasName("UINT16")]
        public ushort MaxContentLightLevel;

        [ComAliasName("UINT16")]
        public ushort MaxFrameAverageLightLevel;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _RedPrimary_e__FixedBuffer
        {
            #region Fields
            public ushort e0;

            public ushort e1;
            #endregion

            #region Properties
            public ushort this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (ushort* e = &e0)
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
            public ushort e0;

            public ushort e1;
            #endregion

            #region Properties
            public ushort this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (ushort* e = &e0)
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
            public ushort e0;

            public ushort e1;
            #endregion

            #region Properties
            public ushort this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (ushort* e = &e0)
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
            public ushort e0;

            public ushort e1;
            #endregion

            #region Properties
            public ushort this[int index]
            {
                get
                {
                    if ((uint)(index) > 1) // (index < 0) || (index > 1)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (ushort* e = &e0)
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
