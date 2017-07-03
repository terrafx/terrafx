// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum CALLCONV
    {
        FASTCALL = 0,

        CDECL = 1,

        MSCPASCAL = (CDECL + 1),

        PASCAL = MSCPASCAL,

        MACPASCAL = (PASCAL + 1),

        STDCALL = (MACPASCAL + 1),

        FPFASTCALL = (STDCALL + 1),

        SYSCALL = (FPFASTCALL + 1),

        MPWCDECL = (SYSCALL + 1),

        MPWPASCAL = (MPWCDECL + 1),

        MAX = (MPWPASCAL + 1)
    }
}
