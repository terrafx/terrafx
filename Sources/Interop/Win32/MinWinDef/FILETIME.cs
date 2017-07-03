// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\minwindef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
#pragma warning disable CS1591
    unsafe public struct FILETIME
    {
        #region Fields
        public uint dwLowDateTime;

        public uint dwHighDateTime;
        #endregion
    }
#pragma warning restore CS1591
}
