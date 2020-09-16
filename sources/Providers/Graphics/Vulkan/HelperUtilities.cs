// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using TerraFX.Interop;
using static TerraFX.Interop.VkBufferUsageFlagBits;
using static TerraFX.Interop.VkImageUsageFlagBits;
using static TerraFX.Interop.VkResult;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics.Providers.Vulkan
{
    internal static partial class HelperUtilities
    {
        public static uint GetVulkanBufferUsageKind(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess)
        {
            var vulkanBufferUsageKind = kind switch
            {
                GraphicsBufferKind.Vertex => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT,
                GraphicsBufferKind.Index => VK_BUFFER_USAGE_INDEX_BUFFER_BIT,
                GraphicsBufferKind.Constant => VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT,
                _ => default,
            };
            
            vulkanBufferUsageKind |= cpuAccess switch
            {
                GraphicsResourceCpuAccess.Read => VK_BUFFER_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Write => VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                _ => default,
            };

            return (uint)vulkanBufferUsageKind;
        }

        public static uint GetVulkanImageUsageKind(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess)
        {
            var vulkanImageUsageKind = cpuAccess switch
            {
                GraphicsResourceCpuAccess.Read => VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Write => VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
                _ => default,
            };

            vulkanImageUsageKind |= VK_IMAGE_USAGE_SAMPLED_BIT;
            return (uint)vulkanImageUsageKind;
        }

        public static void ThrowExternalExceptionIfNotSuccess(string methodName, VkResult result)
        {
            if (result != VK_SUCCESS)
            {
                ThrowExternalException(methodName, (int)result);
            }
        }
    }
}
