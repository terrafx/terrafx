// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public struct VkPhysicalDeviceMemoryProperties
    {
        #region Fields
        public uint memoryTypeCount;

        public _memoryTypes_e__FixedBuffer memoryTypes;

        public uint memoryHeapCount;

        public _memoryHeaps_e__FixedBuffer memoryHeaps;
        #endregion

        #region Structs
        public struct _memoryTypes_e__FixedBuffer
        {
            #region Fields
            public VkMemoryType _0;

            public VkMemoryType _1;

            public VkMemoryType _2;

            public VkMemoryType _3;

            public VkMemoryType _4;

            public VkMemoryType _5;

            public VkMemoryType _6;

            public VkMemoryType _7;

            public VkMemoryType _8;

            public VkMemoryType _9;

            public VkMemoryType _10;

            public VkMemoryType _11;

            public VkMemoryType _12;

            public VkMemoryType _13;

            public VkMemoryType _14;

            public VkMemoryType _15;

            public VkMemoryType _16;

            public VkMemoryType _17;

            public VkMemoryType _18;

            public VkMemoryType _19;

            public VkMemoryType _20;

            public VkMemoryType _21;

            public VkMemoryType _22;

            public VkMemoryType _23;

            public VkMemoryType _24;

            public VkMemoryType _25;

            public VkMemoryType _26;

            public VkMemoryType _27;

            public VkMemoryType _28;

            public VkMemoryType _29;

            public VkMemoryType _30;

            public VkMemoryType _31;
            #endregion
        }

        public struct _memoryHeaps_e__FixedBuffer
        {
            #region Fields
            public VkMemoryHeap _0;

            public VkMemoryHeap _1;

            public VkMemoryHeap _2;

            public VkMemoryHeap _3;

            public VkMemoryHeap _4;

            public VkMemoryHeap _5;

            public VkMemoryHeap _6;

            public VkMemoryHeap _7;

            public VkMemoryHeap _8;

            public VkMemoryHeap _9;

            public VkMemoryHeap _10;

            public VkMemoryHeap _11;

            public VkMemoryHeap _12;

            public VkMemoryHeap _13;

            public VkMemoryHeap _14;

            public VkMemoryHeap _15;
            #endregion
        }
        #endregion
    }
}
