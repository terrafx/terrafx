// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from time.h in the Open Group Base Specifications: Issue 7
// Original source is Copyright © The IEEE and The Open Group.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>A time specification.</summary>
    public /* blittable */ struct timespec
    {
        #region Fields
        /// <summary>Seconds.</summary>
        public time_t tv_sec;

        /// <summary>Nanoseconds.</summary>
        public nint tv_nsec;
        #endregion
    }
}
