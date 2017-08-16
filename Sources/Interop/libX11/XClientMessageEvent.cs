// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct XClientMessageEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        [ComAliasName("Bool")]
        public int send_event;

        [ComAliasName("Display")]
        public IntPtr display;

        [ComAliasName("Window")]
        public nuint window;

        [ComAliasName("Atom")]
        public nuint message_type;

        public int format;

        public _data_e__Union data;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public /* blittable */ struct _data_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public fixed sbyte b[20];

            [FieldOffset(0)]
            public fixed short s[10];

            [FieldOffset(0)]
            public _l_e__FixedBuffer l;
            #endregion

            #region Structs
            public /* blittable */ struct _l_e__FixedBuffer
            {
                #region Fields
                public nint e0;

                public nint e1;

                public nint e2;

                public nint e3;

                public nint e4;
                #endregion

                #region Properties
                public nint this[int index]
                {
                    get
                    {
                        fixed (nint* e = &e0)
                        {
                            return e[index];
                        }
                    }

                    set
                    {
                        fixed (nint* e = &e0)
                        {
                            e[index] = value;
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
