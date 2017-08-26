// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    public /* blittable */ struct VkPhysicalDeviceMemoryProperties
    {
        #region Fields
        public uint memoryTypeCount;

        public _memoryTypes_e__FixedBuffer memoryTypes;

        public uint memoryHeapCount;

        public _memoryHeaps_e__FixedBuffer memoryHeaps;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _memoryTypes_e__FixedBuffer
        {
            #region Fields
            public VkMemoryType e0;

            public VkMemoryType e1;

            public VkMemoryType e2;

            public VkMemoryType e3;

            public VkMemoryType e4;

            public VkMemoryType e5;

            public VkMemoryType e6;

            public VkMemoryType e7;

            public VkMemoryType e8;

            public VkMemoryType e9;

            public VkMemoryType e10;

            public VkMemoryType e11;

            public VkMemoryType e12;

            public VkMemoryType e13;

            public VkMemoryType e14;

            public VkMemoryType e15;

            public VkMemoryType e16;

            public VkMemoryType e17;

            public VkMemoryType e18;

            public VkMemoryType e19;

            public VkMemoryType e20;

            public VkMemoryType e21;

            public VkMemoryType e22;

            public VkMemoryType e23;

            public VkMemoryType e24;

            public VkMemoryType e25;

            public VkMemoryType e26;

            public VkMemoryType e27;

            public VkMemoryType e28;

            public VkMemoryType e29;

            public VkMemoryType e30;

            public VkMemoryType e31;
            #endregion

            #region Properties
            public VkMemoryType this[int index]
            {
                get
                {
                    if ((uint)(index) > 31) // (index < 0) || (index > 31)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (VkMemoryType* e = &e0)
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

                    fixed (VkMemoryType* e = &e0)
                    {
                        e[index] = value;
                    }
                }
            }
            #endregion
        }

        unsafe public /* blittable */ struct _memoryHeaps_e__FixedBuffer
        {
            #region Fields
            public VkMemoryHeap e0;

            public VkMemoryHeap e1;

            public VkMemoryHeap e2;

            public VkMemoryHeap e3;

            public VkMemoryHeap e4;

            public VkMemoryHeap e5;

            public VkMemoryHeap e6;

            public VkMemoryHeap e7;

            public VkMemoryHeap e8;

            public VkMemoryHeap e9;

            public VkMemoryHeap e10;

            public VkMemoryHeap e11;

            public VkMemoryHeap e12;

            public VkMemoryHeap e13;

            public VkMemoryHeap e14;

            public VkMemoryHeap e15;
            #endregion

            #region Properties
            public VkMemoryHeap this[int index]
            {
                get
                {
                    if ((uint)(index) > 15) // (index < 0) || (index > 15)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (VkMemoryHeap* e = &e0)
                    {
                        return e[index];
                    }
                }

                set
                {
                    if ((uint)(index) > 15) // (index < 0) || (index > 15)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (VkMemoryHeap* e = &e0)
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
