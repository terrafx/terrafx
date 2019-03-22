// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct XWindowAttributes
    {
        #region Fields
        public int x, y;

        public int width, height;

        public int border_width;

        public int depth;

        public Visual* visual;

        [NativeTypeName("Window")]
        public nuint root;

        public int @class;

        public int bit_gravity;

        public int win_gravity;

        public int backing_store;

        public nuint backing_planes;

        public nuint backing_pixel;

        [NativeTypeName("Bool")]
        public int save_under;

        [NativeTypeName("Colormap")]
        public nuint colormap;

        [NativeTypeName("Bool")]
        public int map_installed;

        public int map_state;

        public nint all_event_masks;

        public nint your_event_mask;

        public nint do_not_propagate_mask;

        [NativeTypeName("Bool")]
        public int override_redirect;

        public Screen* screen;
        #endregion
    }
}
