// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct XCrossingEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        public Bool send_event;

        public Display* display;

        public Window window;

        public Window root;

        public Window subwindow;

        public Time time;

        public int x, y;

        public int x_root, y_root;

        public int mode;

        public int detail;

        public Bool same_screen;

        public Bool focus;

        public uint state;
        #endregion
    }
}
