// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct XSelectionEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        [ComAliasName("Bool")]
        public int send_event;

        [ComAliasName("Display")]
        public IntPtr display;

        [ComAliasName("Window")]
        public nuint requestor;

        [ComAliasName("Atom")]
        public nuint selection;

        [ComAliasName("Atom")]
        public nuint target;

        [ComAliasName("Atom")]
        public nuint property;

        [ComAliasName("Time")]
        public nuint time;
        #endregion
    }
}
