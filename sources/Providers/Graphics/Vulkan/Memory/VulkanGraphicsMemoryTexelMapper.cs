// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Numerics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkImageType;
using static TerraFX.Interop.VkMemoryPropertyFlagBits;
using static TerraFX.Interop.VkPhysicalDeviceType;
using static TerraFX.Interop.VkSampleCountFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public class VulkanGraphicsMemoryTexelMapper
    {
        /// <summary>
        /// Maps from the TerraFX TexelFormat to the VkFormat
        /// </summary>
        /// <param name="texelFormat">The TerraFX TexelFormat to map.</param>
        /// <returns></returns>
        public static TerraFX.Interop.VkFormat Map(TexelFormat texelFormat)
        {
            switch (texelFormat)
            {
                case TexelFormat.R8G8B8A8_UNORM:
                    return VkFormat.VK_FORMAT_R8G8B8A8_UNORM;
                case TexelFormat.R16_SINT:
                    return VkFormat.VK_FORMAT_R16_SINT;
                case TexelFormat.R16G16UINT:
                    return VkFormat.VK_FORMAT_R16G16_SINT;
                default:
                    return VkFormat.VK_FORMAT_UNDEFINED;
            }
        }
    }
}
