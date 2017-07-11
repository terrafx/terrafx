// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct XSetWindowAttributes
    {
        #region Fields
        public Pixmap background_pixmap;

        public nuint background_pixel;

        public Pixmap border_pixmap;

        public nuint border_pixel;

        public int bit_gravity;

        public int win_gravity;

        public int backing_store;

        public nuint backing_planes;

        public nuint backing_pixel;

        public Bool save_under;

        public nint event_mask;

        public nint do_not_propagate_mask;

        public Bool override_redirect;

        public Colormap colormap;

        public Cursor cursor;
        #endregion
    }
}
