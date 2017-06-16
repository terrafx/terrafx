// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    unsafe public struct VkLayerProperties
    {
        #region Fields
        public fixed byte layerName[VK.MAX_EXTENSION_NAME_SIZE];

        public uint specVersion;

        public uint implementationVersion;

        public fixed byte description[VK.MAX_DESCRIPTION_SIZE];
        #endregion
    }
}
