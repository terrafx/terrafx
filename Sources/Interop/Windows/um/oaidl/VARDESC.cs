// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
   unsafe  public  /* blittable */ struct VARDESC
    {
        #region Fields
        public MEMBERID memid;

        public LPOLESTR lpstrSchema;

        internal _u_e__Union u;

        public ELEMDESC elemdescVar;

        public WORD wVarFlags;

        public VARKIND varkind;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public  /* blittable */ struct _u_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public ULONG oInst;

            [FieldOffset(0)]
            public VARIANT* lpvarValue;
            #endregion
        }
        #endregion
    }
}
