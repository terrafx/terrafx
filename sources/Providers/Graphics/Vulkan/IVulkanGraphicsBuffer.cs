// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public interface IVulkanGraphicsBuffer : IVulkanGraphicsResource, IGraphicsBuffer
    {
        /// <summary>Gets the underlying <see cref="VkBuffer" /> for the buffer.</summary>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public VkBuffer VulkanBuffer { get; }
    }
}
