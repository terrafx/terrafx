// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct Screen
    {
        #region Fields
        public XExtData* ext_data;

        [NativeTypeName("Display")]
        public IntPtr display;

        [NativeTypeName("Window")]
        public UIntPtr root;

        public int width, height;

        public int mwidth, mheight;

        public int ndepths;

        [NativeTypeName("Depth[]")]
        public Depth* depths;

        public int root_depth;

        public Visual* root_visual;

        [NativeTypeName("GC")]
        public IntPtr default_gc;

        [NativeTypeName("Colormap")]
        public UIntPtr cmap;

        public UIntPtr white_pixel;

        public UIntPtr black_pixel;

        public int max_maps, min_maps;

        public int backing_store;

        [NativeTypeName("Bool")]
        public int save_unders;

        public IntPtr root_input_mask;
        #endregion
    }
}
