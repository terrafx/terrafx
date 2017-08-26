// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkPipelineColorBlendStateCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkPipelineColorBlendStateCreateFlags")]
        public uint flags;

        [ComAliasName("VkBool32")]
        public uint logicOpEnable;

        public VkLogicOp logicOp;

        public uint attachmentCount;

        public VkPipelineColorBlendAttachmentState* pAttachments;

        public _blendConstants_e__FixedBuffer blendConstants;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _blendConstants_e__FixedBuffer
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
        #endregion
    }
}
