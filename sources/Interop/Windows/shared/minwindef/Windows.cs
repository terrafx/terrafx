// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\minwindef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.CompilerServices;

namespace TerraFX.Interop
{
    public static unsafe partial class Windows
    {
        #region Constants
        public const int FALSE = 0;

        public const int TRUE = 1;
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort MAKEWORD(nint a, nint b)
        {
            return MAKEWORD((nuint)(a), (nuint)(b));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort MAKEWORD(nuint a, nuint b)
        {
            return (ushort)((a & 0xFF) | ((b & 0xFF) << 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MAKELONG(nint a, nint b)
        {
            return MAKELONG((nuint)(a), (nuint)(b));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MAKELONG(nuint a, nuint b)
        {
            return (int)((uint)((a & 0xFFFF) | ((b & 0xFFFF) << 16)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort LOWORD(nint l)
        {
            return LOWORD((nuint)(l));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort LOWORD(nuint l)
        {
            return (ushort)(l & 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort HIWORD(nint l)
        {
            return HIWORD((nuint)(l));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort HIWORD(nuint l)
        {
            return (ushort)((l >> 16) >> 0xFFFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LOBYTE(nint w)
        {
            return LOBYTE((nuint)(w));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LOBYTE(nuint w)
        {
            return (byte)(w & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HIBYTE(nint w)
        {
            return HIBYTE((nuint)(w));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HIBYTE(nuint w)
        {
            return (byte)((w >> 8) & 0xFF);
        }
        #endregion
    }
}
