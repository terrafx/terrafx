// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct XWindowChanges
    {
        #region Fields
        public int x, y;

        public int width, height;

        public int border_width;

        [NativeTypeName("Window")]
        public UIntPtr sibling;

        public int stack_mode;
        #endregion
    }
}
