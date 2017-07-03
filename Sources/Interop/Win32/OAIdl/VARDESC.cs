// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
   unsafe  public struct VARDESC
    {
        #region Fields
        public MEMBERID memid;

        public LPOLESTR lpstrSchema;

        public _Anonymous_e__Union DUMMYUNIONNAME;

        public ELEMDESC elemdescVar;

        public ushort wVarFlags;

        public VARKIND varkind;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public struct _Anonymous_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public uint oInst;

            [FieldOffset(0)]
            public VARIANT* lpvarValue;
            #endregion
        }
        #endregion
    }
}
