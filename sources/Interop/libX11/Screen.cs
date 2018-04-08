// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct Screen
    {
        #region Fields
        public XExtData* ext_data;

        [ComAliasName("Display")]
        public IntPtr display;

        [ComAliasName("Window")]
        public nuint root;

        public int width, height;

        public int mwidth, mheight;

        public int ndepths;

        [ComAliasName("Depth[]")]
        public Depth* depths;

        public int root_depth;

        public Visual* root_visual;

        [ComAliasName("GC")]
        public IntPtr default_gc;

        [ComAliasName("Colormap")]
        public nuint cmap;

        public nuint white_pixel;

        public nuint black_pixel;

        public int max_maps, min_maps;

        public int backing_store;

        [ComAliasName("Bool")]
        public int save_unders;

        public nint root_input_mask;
        #endregion
    }
}
