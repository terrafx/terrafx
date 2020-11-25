// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public class VulkanGraphicsMemoryTexelMapper
    {
        /// <summary>Maps from a texel format to VkFormat.</summary>
        /// <param name="texelFormat">The texel format to map.</param>
        /// <returns></returns>
        public static VkFormat Map(TexelFormat texelFormat) => texelFormat switch {
            TexelFormat.R8G8B8A8_UNORM => VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
            TexelFormat.R16_SINT => VkFormat.VK_FORMAT_R16_SINT,
            TexelFormat.R16G16UINT => VkFormat.VK_FORMAT_R16G16_SINT,
            _ => VkFormat.VK_FORMAT_UNDEFINED,
        };
    }
}
