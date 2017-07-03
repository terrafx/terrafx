// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public struct TYPEDESC
    {
        #region Fields
        public _Anonymous_e__Union DUMMYUNIONNAME;

        public VARTYPE vt;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public struct _Anonymous_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public TYPEDESC* lptdesc;

            [FieldOffset(0)]
            public ARRAYDESC* lpadesc;

            [FieldOffset(0)]
            public HREFTYPE hrefType;
            #endregion
        }
        #endregion
    }
}
