// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from time.h in the Open Group Base Specifications: Issue 7
// Original source is Copyright © The IEEE and The Open Group.

namespace TerraFX.Interop
{
    public enum clockid_t
    {
        MONOTONIC,

        PROCESS_CPUTIME_ID,

        REALTIME,

        THREAD_CPUTIME_ID
    }
}
