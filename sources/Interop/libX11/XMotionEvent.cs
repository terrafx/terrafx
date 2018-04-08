// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct XMotionEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        [ComAliasName("Bool")]
        public int send_event;

        [ComAliasName("Display")]
        public IntPtr display;

        [ComAliasName("Window")]
        public nuint window;

        [ComAliasName("Window")]
        public nuint root;

        [ComAliasName("Window")]
        public nuint subwindow;

        [ComAliasName("Time")]
        public nuint time;

        public int x, y;

        public int x_root, y_root;

        public uint state;

        public sbyte is_hint;

        [ComAliasName("Bool")]
        public int same_screen;
        #endregion
    }
}
