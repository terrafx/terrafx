// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct XClientMessageEvent
    {
        #region Fields
        public int type;

        public UIntPtr serial;

        [NativeTypeName("Bool")]
        public int send_event;

        [NativeTypeName("Display")]
        public IntPtr display;

        [NativeTypeName("Window")]
        public UIntPtr window;

        [NativeTypeName("Atom")]
        public UIntPtr message_type;

        public int format;

        public _data_e__Union data;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        [Unmanaged]
        public struct _data_e__Union
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
            [Unmanaged]
            public struct _l_e__FixedBuffer
            {
                #region Fields
                public IntPtr e0;

                public IntPtr e1;

                public IntPtr e2;

                public IntPtr e3;

                public IntPtr e4;
                #endregion

                #region Properties
                public IntPtr this[int index]
                {
                    get
                    {
                        fixed (IntPtr* e = &e0)
                        {
                            return e[index];
                        }
                    }

                    set
                    {
                        fixed (IntPtr* e = &e0)
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
