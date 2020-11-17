// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

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
using ChannelType = TerraFX.Graphics.TexelFormat.Channel.Type;
using ChannelKind = TerraFX.Graphics.TexelFormat.Channel.Kind;
using System.Security.Cryptography.X509Certificates;

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
            if (texelFormat.Channels == null || texelFormat.Channels.Length == 0)
                return VkFormat.VK_FORMAT_UNDEFINED;
            var c = texelFormat.Channels;

            // first quickly check for some common formats
            if (c.Length == 4
                && c[0].ChannelType == ChannelType.UInt8 && c[0].ChannelKind == ChannelKind.R
                && c[1].ChannelType == ChannelType.UInt8 && c[1].ChannelKind == ChannelKind.G
                && c[2].ChannelType == ChannelType.UInt8 && c[2].ChannelKind == ChannelKind.B
                && c[3].ChannelType == ChannelType.UInt8 && c[3].ChannelKind == ChannelKind.A
                )
                return VkFormat.VK_FORMAT_R8G8B8A8_UNORM;

            if (c.Length == 3
                && c[0].ChannelType == ChannelType.UInt8 && c[0].ChannelKind == TexelFormat.Channel.Kind.R
                && c[1].ChannelType == ChannelType.UInt8 && c[1].ChannelKind == TexelFormat.Channel.Kind.G
                && c[2].ChannelType == ChannelType.UInt8 && c[2].ChannelKind == TexelFormat.Channel.Kind.B
                )
                return VkFormat.VK_FORMAT_R8G8B8_UNORM;

            if (c.Length == 1 && c[0].ChannelKind == ChannelKind.R)
            {
                if (c[0].ChannelType == ChannelType.UInt8)
                    return VkFormat.VK_FORMAT_R8_UINT;

                if (c[0].ChannelType == ChannelType.UInt16)
                    return VkFormat.VK_FORMAT_R16_UINT;

                if (c[0].ChannelType == ChannelType.SInt8)
                    return VkFormat.VK_FORMAT_R16_SINT;

                if (c[0].ChannelType == ChannelType.SInt16)
                    return VkFormat.VK_FORMAT_R16_SINT;

                if (c[0].ChannelType == ChannelType.Float32)
                    return VkFormat.VK_FORMAT_R32_SFLOAT;
            }

            // for the remainder do a name match
            string formatName = "VK_FORMAT_";
            var channelType = c[0].ChannelType;
            var channelKind = c[0].ChannelKind;
            string bitsStr = "";
            for (int i = 0; i < c.Length; i++)
            {
                if (channelType != c[i].ChannelType)
                    break;
                channelKind = c[i].ChannelKind;
                channelType = c[i].ChannelType;
                formatName += channelKind.ToString();
                bitsStr = TexelFormat.Channel.BitsUsed(channelType).ToString();
                formatName += bitsStr;
            }
            formatName += "_" + channelType.ToString().Replace(bitsStr, "").ToUpper();
            var formats = (VkFormat[])Enum.GetValues(typeof(VkFormat));
            foreach (var format in formats)
            {
                if (formatName == format.ToString())
                    return format;
            }

            // to do: NORM, SCALED, PACK, SRGB, BLOCK, KHR

            // nothing matched, return undefined
            return VkFormat.VK_FORMAT_UNDEFINED;
        }
    }
}