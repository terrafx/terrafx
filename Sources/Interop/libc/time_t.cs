// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from sys\types.h in the Open Group Base Specifications: Issue 7
// Original source is Copyright © The IEEE and The Open Group.

using System;

namespace TerraFX.Interop
{
    unsafe public struct time_t
    {
        #region Fields
        public IntPtr Value;
        #endregion
    }
}
