// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winnt.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* unmanaged */ struct ULARGE_INTEGER
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        [ComAliasName("DWORD")]
        public uint LowPart;

        [FieldOffset(4)]
        [ComAliasName("DWORD")]
        public uint HighPart;
        #endregion

        [FieldOffset(0)]
        public _u__e__Struct u;

        [FieldOffset(0)]
        [ComAliasName("ULONGLONG")]
        public ulong QuadPart;
        #endregion

        #region Structs
        public /* unmanaged */ struct _u__e__Struct
        {
            #region Fields
            [ComAliasName("DWORD")]
            public uint LowPart;

            [ComAliasName("DWORD")]
            public uint HighPart;
            #endregion
        }
        #endregion
    }
}
