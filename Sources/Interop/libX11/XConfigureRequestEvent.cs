// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;

namespace TerraFX.Interop
{
    unsafe public struct XConfigureRequestEvent
    {
        #region Fields
        public int type;

        public UIntPtr serial;

        public Bool send_event;

        public Display* display;

        public Window parent;

        public Window window;

        public int x, y;

        public int width, height;

        public int border_width;

        public Window above;

        public int detail;

        public UIntPtr value_mask;
        #endregion
    }
}
