// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;

namespace TerraFX.Interop
{
    unsafe public struct Display
    {
        #region Fields
        public XExtData* ext_data;

        public void* private1;

        public int fd;

        public int private2;

        public int proto_major_version;

        public int proto_minor_version;

        public byte* vendor;

        public XID private3;

        public XID private4;

        public XID private5;

        public int private6;

        public UIntPtr resource_alloc;

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

        public UIntPtr last_request_read;

        public UIntPtr request;

        public XPointer private11;

        public XPointer private12;

        public XPointer private13;

        public XPointer private14;

        public uint max_request_size;

        public void* db;

        public UIntPtr private15;

        public byte* displayName;

        public int default_screen;

        public int nscreens;

        public Screen* screens;

        public UIntPtr motion_buffer;

        public UIntPtr private16;

        public int min_keycode;

        public int max_keycode;

        public XPointer private17;

        public XPointer private18;

        public int private19;

        public byte* xdefaults;

        // There is more to this structure but it is private to the Xlib implementation
        #endregion
    }
}
