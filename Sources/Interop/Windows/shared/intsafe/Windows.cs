// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\winerror.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public static partial class Windows
    {
        #region Constants
        #region INTSAFE_E_*
        public static readonly HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW = (HRESULT)(0x80070216);
        #endregion

        public const ulong ULONGLONG_MAX = 0xFFFFFFFFFFFFFFFF;
        #endregion
    }
}
