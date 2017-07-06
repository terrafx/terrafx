// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\wtypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct CY
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        public ULONG Lo;

        [FieldOffset(4)]
        public LONG Hi;
        #endregion

        [FieldOffset(0)]
        public LONGLONG int64;
        #endregion
    }
}
