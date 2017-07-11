// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct Display
    {
        #region Fields
        public XExtData* ext_data;

        public void* private1;

        public int fd;

        public int private2;

        public int proto_major_version;

        public int proto_minor_version;

        public sbyte* vendor;

        public XID private3;

        public XID private4;

        public XID private5;

        public int private6;

        public IntPtr /* resource_alloc */ resource_alloc;

        public int byte_order;

        public int bitmap_unit;

        public int bitmap_pad;

        public int bitmap_bit_order;

        public int nformats;

        public ScreenFormat* pixmap_format;

        public int private8;

        public int release;

        public void* private9, private10;

        public int qlen;

        public nuint last_request_read;

        public nuint request;

        public XPointer private11;

        public XPointer private12;

        public XPointer private13;

        public XPointer private14;

        public uint max_request_size;

        public void* db;

        public nuint private15;

        public sbyte* displayName;

        public int default_screen;

        public int nscreens;

        public Screen* screens;

        public nuint motion_buffer;

        public nuint private16;

        public int min_keycode;

        public int max_keycode;

        public XPointer private17;

        public XPointer private18;

        public int private19;

        public sbyte* xdefaults;

        // There is more to this structure but it is private to the Xlib implementation
        #endregion
    }
}
