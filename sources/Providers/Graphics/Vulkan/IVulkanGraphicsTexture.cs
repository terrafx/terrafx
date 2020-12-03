// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using static TerraFX.Interop.Vulkan;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public interface IVulkanGraphicsTexture : IVulkanGraphicsResource, IGraphicsTexture
    {
        /// <summary>Gets the underlying <see cref="VkImage" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkBindImageMemory(IntPtr, ulong, ulong, ulong)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkImage VulkanImage { get; }

        /// <summary>Gets the underlying <see cref="VkImageView" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateImageView(IntPtr, VkImageViewCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkImageView VulkanImageView { get; }

        /// <summary>Gets the underlying <see cref="VkSampler" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="vkCreateSampler(IntPtr, VkSamplerCreateInfo*, VkAllocationCallbacks*, ulong*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The texture has been disposed.</exception>
        public VkSampler VulkanSampler { get; }
    }
}
