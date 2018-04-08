// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using static System.Runtime.CompilerServices.Unsafe;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct VkImageBlit
    {
        #region Fields
        public VkImageSubresourceLayers srcSubresource;

        public _srcOffsets_e__FixedBuffer srcOffsets;

        public VkImageSubresourceLayers dstSubresource;

        public _dstOffsets_e__FixedBuffer dstOffsets;
        #endregion

        #region Structs
        public /* unmanaged */ unsafe struct _srcOffsets_e__FixedBuffer
        {
            #region Fields
            public VkOffset3D e0;

            public VkOffset3D e1;
            #endregion

            #region Properties
            public ref VkOffset3D this[int index]
            {
                get
                {
                    fixed (VkOffset3D* e = &e0)
                    {
                        return ref AsRef<VkOffset3D>(e + index);
                    }
                }
            }
            #endregion
        }

        public /* unmanaged */ unsafe struct _dstOffsets_e__FixedBuffer
        {
            #region Fields
            public VkOffset3D e0;

            public VkOffset3D e1;
            #endregion

            #region Properties
            public ref VkOffset3D this[int index]
            {
                get
                {
                    fixed (VkOffset3D* e = &e0)
                    {
                        return ref AsRef<VkOffset3D>(e + index);
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
