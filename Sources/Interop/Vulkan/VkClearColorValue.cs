// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    unsafe public /* blittable */ struct VkClearColorValue
    {
        #region Fields
        [FieldOffset(0)]
        public _float32_e__FixedBuffer float32;

        [FieldOffset(0)]
        public _int32_e__FixedBuffer int32;

        [FieldOffset(0)]
        public _uint32_e__FixedBuffer uint32;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _float32_e__FixedBuffer
        {
            #region Fields
            public float e0;

            public float e1;

            public float e2;

            public float e3;
            #endregion

            #region Properties
            public float this[int index]
            {
                get
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (float* e = &e0)
                    {
                        return e[index];
                    }
                }

                set
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (float* e = &e0)
                    {
                        e[index] = value;
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _int32_e__FixedBuffer
        {
            #region Fields
            public int e0;

            public int e1;

            public int e2;

            public int e3;
            #endregion

            #region Properties
            public int this[int index]
            {
                get
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (int* e = &e0)
                    {
                        return e[index];
                    }
                }

                set
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (int* e = &e0)
                    {
                        e[index] = value;
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _uint32_e__FixedBuffer
        {
            #region Fields
            public uint e0;

            public uint e1;

            public uint e2;

            public uint e3;
            #endregion

            #region Properties
            public uint this[int index]
            {
                get
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (uint* e = &e0)
                    {
                        return e[index];
                    }
                }

                set
                {
                    if ((uint)(index) > 3) // (index < 0) || (index > 3)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (uint* e = &e0)
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
