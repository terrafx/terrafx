// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winnt.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct LARGE_INTEGER
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        public DWORD LowPart;

        [FieldOffset(4)]
        public LONG HighPart;
        #endregion

        [FieldOffset(0)]
        public _u__e__Struct u;

        [FieldOffset(0)]
        public LONGLONG QuadPart;
        #endregion

        #region Structs
        public  /* blittable */ struct _u__e__Struct
        {
            #region Fields
            public DWORD LowPart;

            public LONG HighPart;
            #endregion
        }
        #endregion
    }
}
