// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct XClientMessageEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        public Bool send_event;

        public Display* display;

        public Window window;

        public Atom message_type;

        public int format;

        public _data_e__Union data;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public /* blittable */ struct _data_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public _b_e__FixedBuffer b;

            [FieldOffset(0)]
            public _s_e__FixedBuffer s;

            [FieldOffset(0)]
            public _l_e__FixedBuffer l;
            #endregion

            #region Structs
            public /* blittable */ struct _b_e__FixedBuffer
            {
                #region Fields
                public sbyte e0;

                public sbyte e1;

                public sbyte e2;

                public sbyte e3;

                public sbyte e4;

                public sbyte e5;

                public sbyte e6;

                public sbyte e7;

                public sbyte e8;

                public sbyte e9;

                public sbyte e10;

                public sbyte e11;

                public sbyte e12;

                public sbyte e13;

                public sbyte e14;

                public sbyte e15;

                public sbyte e16;

                public sbyte e17;

                public sbyte e18;

                public sbyte e19;
                #endregion

                #region Properties
                public sbyte this[int index]
                {
                    get
                    {
                        if ((uint)(index) > 19) // (index < 0) || (index > 19)
                        {
                            ThrowArgumentOutOfRangeException(nameof(index), index);
                        }

                        fixed (sbyte* e = &e0)
                        {
                            return e[index];
                        }
                    }

                    set
                    {
                        if ((uint)(index) > 19) // (index < 0) || (index > 19)
                        {
                            ThrowArgumentOutOfRangeException(nameof(index), index);
                        }

                        fixed (sbyte* e = &e0)
                        {
                            e[index] = value;
                        }
                    }
                }
                #endregion
            }

            public /* blittable */ struct _s_e__FixedBuffer
            {
                #region Fields
                public short e0;

                public short e1;

                public short e2;

                public short e3;

                public short e4;

                public short e5;

                public short e6;

                public short e7;

                public short e8;

                public short e9;
                #endregion

                #region Properties
                public short this[int index]
                {
                    get
                    {
                        if ((uint)(index) > 9) // (index < 0) || (index > 9)
                        {
                            ThrowArgumentOutOfRangeException(nameof(index), index);
                        }

                        fixed (short* e = &e0)
                        {
                            return e[index];
                        }
                    }

                    set
                    {
                        if ((uint)(index) > 9) // (index < 0) || (index > 9)
                        {
                            ThrowArgumentOutOfRangeException(nameof(index), index);
                        }

                        fixed (short* e = &e0)
                        {
                            e[index] = value;
                        }
                    }
                }
                #endregion
            }

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
                        if ((uint)(index) > 4) // (index < 0) || (index > 4)
                        {
                            ThrowArgumentOutOfRangeException(nameof(index), index);
                        }

                        fixed (nint* e = &e0)
                        {
                            return e[index];
                        }
                    }

                    set
                    {
                        if ((uint)(index) > 4) // (index < 0) || (index > 4)
                        {
                            ThrowArgumentOutOfRangeException(nameof(index), index);
                        }

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
