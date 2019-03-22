// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from time.h in the Open Group Base Specifications: Issue 7
// Original source is Copyright © The IEEE and The Open Group.

using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Exposes methods exported by <c>libc</c>.</summary>
    public static unsafe partial class libc
    {
        private const string DllName = nameof(libc);

        #region CLOCK_* Constants
        /// <summary>The identifier of the system-wide clock measuring real time.</summary>
        public const int CLOCK_REALTIME = 0;

        /// <summary>The identifier for the system-wide monotonic clock, which is defined as a clock measuring real time, whose value cannot be set via <see cref="libc.clock_settime(int, in timespec)" /> and which cannot have negative clock jumps.</summary>
        /// <remarks>The maximum possible clock jump shall be implementation-defined.</remarks>
        public const int CLOCK_MONOTONIC = 1;

        /// <summary>The identifier of the CPU-time clock associated with the process making a <c>clock</c> or <c>timer</c> function call.</summary>
        public const int CLOCK_PROCESS_CPUTIME_ID = 2;

        /// <summary>The identifier of the CPU-time clock associated with the thread making a <c>clock</c> or <c>timer</c> function call.</summary>
        public const int CLOCK_THREAD_CPUTIME_ID = 3;
        #endregion

        #region External Methods
        /// <summary>Gets the resolution of a clock.</summary>
        /// <param name="clock_id">The clock for which to get the resolution.</param>
        /// <param name="res">On return, contains the resolution of <paramref name="clock_id" />.</param>
        /// <returns><c>0</c> to indicate the call succeeded; otherwise, <c>-1</c> to indicate that an error occurred. The error can be retrieved via <see cref="Marshal.GetLastWin32Error()" />.</returns>
        /// <remarks>Clock resolutions are implementation-defined and cannot be set by a process.</remarks>
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "clock_getres", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int clock_getres(
            [In, NativeTypeName("clockid_t")] int clock_id,
            [Out] out timespec res
        );

        /// <summary>Gets the current value of a clock.</summary>
        /// <param name="clock_id">The clock for which to get the current value.</param>
        /// <param name="tp">On return, contains the current value of <paramref name="clock_id" />.</param>
        /// <returns><c>0</c> to indicate the call succeeded; otherwise, <c>-1</c> to indicate that an error occurred. The error can be retrieved via <see cref="Marshal.GetLastWin32Error()" />.</returns>
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "clock_gettime", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int clock_gettime(
            [In, NativeTypeName("clockid_t")] int clock_id,
            [Out] out timespec tp
        );

        /// <summary>Sets the value of a clock.</summary>
        /// <param name="clock_id">The clock for which to set the value.</param>
        /// <param name="tp">The value which will be assigned to <paramref name="clock_id" />.</param>
        /// <returns><c>0</c> to indicate the call succeeded; otherwise, <c>-1</c> to indicate that an error occurred. The error can be retrieved via <see cref="Marshal.GetLastWin32Error()" />.</returns>
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "clock_settime", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int clock_settime(
            [In, NativeTypeName("clockid_t")] int clock_id,
            [In] in timespec tp
        );
        #endregion
    }
}
