// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct XGraphicsExposeEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        public Bool send_event;

        public Display* display;

        public Window window;

        public int x, y;

        public int width, height;

        public int count;

        public int major_code;

        public int minor_code;
        #endregion
    }
}
