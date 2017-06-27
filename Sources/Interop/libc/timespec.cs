// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from time.h in the Open Group Base Specifications: Issue 7
// Original source is Copyright © The IEEE and The Open Group.

using System;

namespace TerraFX.Interop
{
    unsafe public struct timespec
    {
        #region Fields
        public time_t tv_sec;

        public IntPtr tv_nsec;
        #endregion
    }
}
