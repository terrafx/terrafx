// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public unsafe interface IVulkanGraphicsMemoryBlock : IGraphicsMemoryBlock
    {
        /// <summary>Gets the <see cref="VkDeviceMemory" /> for the memory block.</summary>
        VkDeviceMemory VulkanDeviceMemory { get; }

        /// <inheritdoc cref="IGraphicsMemoryBlock.Collection" />
        new VulkanGraphicsMemoryBlockCollection Collection { get; }
    }
}
