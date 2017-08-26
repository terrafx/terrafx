// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct XKeymapEvent
    {
        #region Fields
        public int type;

        public nuint serial;

        public Bool send_event;

        public Display* display;

        public Window window;

        public _key_vector_e__FixedBuffer key_vector;
        #endregion

        #region Structs
        public /* blittable */ struct _key_vector_e__FixedBuffer
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

            public sbyte e20;

            public sbyte e21;

            public sbyte e22;

            public sbyte e23;

            public sbyte e24;

            public sbyte e25;

            public sbyte e26;

            public sbyte e27;

            public sbyte e28;

            public sbyte e29;

            public sbyte e30;

            public sbyte e31;
            #endregion

            #region Properties
            public sbyte this[int index]
            {
                get
                {
                    if ((uint)(index) > 31) // (index < 0) || (index > 31)
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
                    if ((uint)(index) > 31) // (index < 0) || (index > 31)
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
        #endregion
    }
}
