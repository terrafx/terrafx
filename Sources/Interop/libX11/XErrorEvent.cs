// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;

namespace TerraFX.Interop
{
    unsafe public struct XErrorEvent
    {
        #region Fields
        public int type;

        public Display* display;

        public XID resourceid;

        public UIntPtr serial;

        public byte error_code;

        public byte request_code;

        public byte minor_code;
        #endregion
    }
}
