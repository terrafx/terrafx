// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public struct XClientMessageEvent
    {
        #region Fields
        public int type;

        public UIntPtr serial;

        public Bool send_event;

        public Display* display;

        public Window window;

        public Atom message_type;

        public int format;

        public _data_e__Union data;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public struct _data_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public fixed byte b[20];

            [FieldOffset(0)]
            public fixed short s[10];

            [FieldOffset(0)]
            public _l_e__FixedBuffer l;
            #endregion

            #region Structs
            public struct _l_e__FixedBuffer
            {
                #region Fields
                public IntPtr _0;
                public IntPtr _1;
                public IntPtr _2;
                public IntPtr _3;
                public IntPtr _4;
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
