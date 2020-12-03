// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public interface IVulkanGraphicsResource : IGraphicsResource
    {
        /// <inheritdoc cref="IGraphicsResource.Allocator" />
        new VulkanGraphicsMemoryAllocator Allocator { get; }
    }
}
