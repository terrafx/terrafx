// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\WTypesbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
#pragma warning disable CS1591
    unsafe public struct DECIMAL
    {
        #region Fields
        public ushort wReserved;

        public _DUMMYUNIONNAME_e__Union DUMMYUNIONNAME;

        public uint Hi32;

        public _DUMMYUNIONNAME2_e__Union DUMMYUNIONNAME2;
        #endregion

        #region Structs
        public struct _DUMMYUNIONNAME_e__Union
        {
            #region Fields
            public _DUMMYSTRUCTNAME_e__Struct DUMMYSTRUCTNAME;

            public ushort signscale;
            #endregion

            #region Structs
            public struct _DUMMYSTRUCTNAME_e__Struct
            {
                #region Fields
                public byte scale;

                public byte sign;
                #endregion
            }
            #endregion
        }

        public struct _DUMMYUNIONNAME2_e__Union
        {
            #region Fields
            public _DUMMYSTRUCTNAME2_e__Struct DUMMYSTRUCTNAME;

            public ulong Lo64;
            #endregion

            #region Structs
            public struct _DUMMYSTRUCTNAME2_e__Struct
            {
                #region Fields
                public uint Lo32;

                public uint Mid32;
                #endregion
            }
            #endregion
        }
        #endregion
    }
#pragma warning restore CS1591
}
