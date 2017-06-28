// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from time.h in the Open Group Base Specifications: Issue 7
// Original source is Copyright © The IEEE and The Open Group.

namespace TerraFX.Interop
{
    /// <summary>Used for clock ID type in the clock and timer functions.</summary>
    public enum clockid_t
    {
        /// <summary>The identifier of the system-wide clock measuring real time.</summary>
        REALTIME = 0,

        /// <summary>The identifier for the system-wide monotonic clock, which is defined as a clock measuring real time, whose value cannot be set via <see cref="libc.clock_settime(clockid_t, ref timespec)" /> and which cannot have negative clock jumps.</summary>
        /// <remarks>The maximum possible clock jump shall be implementation-defined.</remarks>
        MONOTONIC = 1,

        /// <summary>The identifier of the CPU-time clock associated with the process making a <c>clock</c> or <c>timer</c> function call.</summary>
        PROCESS_CPUTIME_ID = 2,

        /// <summary>The identifier of the CPU-time clock associated with the thread making a <c>clock</c> or <c>timer</c> function call.</summary>
        THREAD_CPUTIME_ID = 3
    }
}
