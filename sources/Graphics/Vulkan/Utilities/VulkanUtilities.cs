// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using TerraFX.Graphics;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkBufferUsageFlags;
using static TerraFX.Interop.Vulkan.VkImageUsageFlags;
using static TerraFX.Interop.Vulkan.VkResult;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Utilities
{
    internal static partial class VulkanUtilities
    {
        public static VkBufferUsageFlags GetVulkanBufferUsageKind(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess)
        {
            var vulkanBufferUsageKind = kind switch {
                GraphicsBufferKind.Vertex => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT,
                GraphicsBufferKind.Index => VK_BUFFER_USAGE_INDEX_BUFFER_BIT,
                GraphicsBufferKind.Constant => VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT,
                _ => default,
            };

            // TODO: This is modeled poorly and doesn't accurately track the dst/src
            // requirements for CPU to/from GPU copies. It might be simplest to just
            // mirror what DX12 does for resources, but forcing "none" to be "dst"
            // resolves the validation layer warnings for the time being.

            vulkanBufferUsageKind |= cpuAccess switch {
                GraphicsResourceCpuAccess.None => VK_BUFFER_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Read => VK_BUFFER_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Write => VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                _ => default,
            };

            return vulkanBufferUsageKind;
        }

        public static VkImageUsageFlags GetVulkanImageUsageKind(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess)
        {
            // TODO: This is modeled poorly and doesn't accurately track the dst/src
            // requirements for CPU to/from GPU copies. It might be simplest to just
            // mirror what DX12 does for resources, but forcing "none" to be "dst"
            // resolves the validation layer warnings for the time being.

            var vulkanImageUsageKind = cpuAccess switch {
                GraphicsResourceCpuAccess.None => VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Read => VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Write => VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
                _ => default,
            };

            vulkanImageUsageKind |= VK_IMAGE_USAGE_SAMPLED_BIT;
            return vulkanImageUsageKind;
        }

        public static VkFormat Map(TexelFormat texelFormat) => texelFormat switch {
            TexelFormat.R8G8B8A8_UNORM => VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
            TexelFormat.R16_SINT => VkFormat.VK_FORMAT_R16_SINT,
            TexelFormat.R16G16UINT => VkFormat.VK_FORMAT_R16G16_SINT,
            _ => VkFormat.VK_FORMAT_UNDEFINED,
        };

        public static void ThrowExternalExceptionIfNotSuccess(VkResult result, string methodName)
        {
            if (result != VK_SUCCESS)
            {
                ThrowExternalException(methodName, (int)result);
            }
        }
    }
}
