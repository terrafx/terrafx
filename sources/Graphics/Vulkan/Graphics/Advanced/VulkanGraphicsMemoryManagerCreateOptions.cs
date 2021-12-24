// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics.Advanced;

internal struct VulkanGraphicsMemoryManagerCreateOptions
{
    public GraphicsMemoryAllocatorCreateFunc CreateMemoryAllocator;

    public uint VkMemoryTypeIndex;
}
