// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct XNoExposeEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        [NativeTypeName("Bool")]
        public int send_event;

        [NativeTypeName("Display")]
        public IntPtr display;

        [NativeTypeName("Drawable")]
        public nuint drawable;

        public int major_code;

        public int minor_code;
        #endregion
    }
}
