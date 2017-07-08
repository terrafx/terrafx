// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\minwindef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.CompilerServices;

namespace TerraFX.Interop
{
    unsafe public static partial class Windows
    {
        #region Constants
        public const uint NULL = 0;

        public const int FALSE = 0;

        public const int TRUE = 1;
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WORD MAKEWORD(DWORD_PTR a, DWORD_PTR b)
        {
            return (ushort)((a & 0xFF) | ((b & 0xFF) << 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LONG MAKELONG(DWORD_PTR a, DWORD_PTR b)
        {
            return (int)((uint)((a & 0xFFFF) | ((b & 0xFFFF) << 16)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WORD LOWORD(DWORD_PTR l)
        {
            return (ushort)(l & 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WORD HIWORD(DWORD_PTR l)
        {
            return (ushort)((l >> 16) >> 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BYTE LOBYTE(DWORD_PTR w)
        {
            return (byte)(w & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BYTE HIBYTE(DWORD_PTR w)
        {
            return (byte)((w >> 8) & 0xFF);
        }
        #endregion
    }
}
