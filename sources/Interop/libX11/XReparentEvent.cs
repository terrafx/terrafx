// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct XReparentEvent
    {
        #region Fields
        public int type;

        public UIntPtr serial;

        [NativeTypeName("Bool")]
        public int send_event;

        [NativeTypeName("Display")]
        public IntPtr display;

        [NativeTypeName("Window")]
        public UIntPtr @event;

        [NativeTypeName("Window")]
        public UIntPtr window;

        [NativeTypeName("Window")]
        public UIntPtr parent;

        public int x, y;

        [NativeTypeName("Bool")]
        public int override_redirect;
        #endregion
    }
}
