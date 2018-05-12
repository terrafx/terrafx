// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct TYPEDESC
    {
        #region Fields
        private _u_e__Union u;

        [ComAliasName("VARTYPE")]
        public ushort vt;
        #endregion

        #region Properties
        public TYPEDESC* lptdesc
        {
            get
            {
                return u.lptdesc;
            }

            set
            {
                u.lptdesc = value;
            }
        }

        public ARRAYDESC* lpadesc
        {
            get
            {
                return u.lpadesc;
            }

            set
            {
                u.lpadesc = value;
            }
        }

        public uint hreftype
        {
            get
            {
                return u.hreftype;
            }

            set
            {
                u.hreftype = value;
            }
        }
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public /* unmanaged */ struct _u_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public TYPEDESC* lptdesc;

            [FieldOffset(0)]
            public ARRAYDESC* lpadesc;

            [FieldOffset(0)]
            [ComAliasName("HREFTYPE")]
            public uint hreftype;
            #endregion
        }
        #endregion
    }
}
