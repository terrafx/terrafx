// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct XWindowAttributes
    {
        #region Fields
        public int x, y;

        public int width, height;

        public int border_width;

        public int depth;

        public Visual* visual;

        [ComAliasName("Window")]
        public nuint root;

        public int @class;

        public int bit_gravity;

        public int win_gravity;

        public int backing_store;

        public nuint backing_planes;

        public nuint backing_pixel;

        [ComAliasName("Bool")]
        public int save_under;

        [ComAliasName("Colormap")]
        public nuint colormap;

        [ComAliasName("Bool")]
        public int map_installed;

        public int map_state;

        public nint all_event_masks;

        public nint your_event_mask;

        public nint do_not_propagate_mask;

        [ComAliasName("Bool")]
        public int override_redirect;

        public Screen* screen;
        #endregion
    }
}
