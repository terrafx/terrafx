// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System;
using System.Runtime.CompilerServices;
using TerraFX.Graphics;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkFormat;
using static TerraFX.Interop.Vulkan.VkResult;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Utilities;

internal static unsafe partial class VulkanUtilities
{
    private static readonly VkFormat[] s_vkFormatMap = new VkFormat[] {
        VK_FORMAT_UNDEFINED,                    // Unknown

        VK_FORMAT_R32G32B32A32_SFLOAT,          // R32G32B32A32_SFLOAT
        VK_FORMAT_R32G32B32A32_UINT,            // R32G32B32A32_UINT
        VK_FORMAT_R32G32B32A32_SINT,            // R32G32B32A32_SINT

        VK_FORMAT_R32G32B32_SFLOAT,             // R32G32B32_SFLOAT
        VK_FORMAT_R32G32B32_UINT,               // R32G32B32_UINT
        VK_FORMAT_R32G32B32_SINT,               // R32G32B32_SINT

        VK_FORMAT_R16G16B16A16_SFLOAT,          // R16G16B16A16_SFLOAT
        VK_FORMAT_R16G16B16A16_UNORM,           // R16G16B16A16_UNORM
        VK_FORMAT_R16G16B16A16_UINT,            // R16G16B16A16_UINT 
        VK_FORMAT_R16G16B16A16_SNORM,           // R16G16B16A16_SNORM
        VK_FORMAT_R16G16B16A16_SINT,            // R16G16B16A16_SINT

        VK_FORMAT_R32G32_SFLOAT,                // R32G32_SFLOAT
        VK_FORMAT_R32G32_UINT,                  // R32G32_UINT
        VK_FORMAT_R32G32_SINT,                  // R32G32_SINT

        VK_FORMAT_D32_SFLOAT_S8_UINT,           // D32_SFLOAT_S8X24_UINT

        VK_FORMAT_A2R10G10B10_UNORM_PACK32,     // R10G10B10A2_UNORM
        VK_FORMAT_A2R10G10B10_UINT_PACK32,      // R10G10B10A2_UINT

        VK_FORMAT_B10G11R11_UFLOAT_PACK32,      // R11G11B10_UFLOAT

        VK_FORMAT_R8G8B8A8_UNORM,               // R8G8B8A8_UNORM
        VK_FORMAT_R8G8B8A8_SRGB,                // R8G8B8A8_SRGB
        VK_FORMAT_R8G8B8A8_UINT,                // R8G8B8A8_UINT
        VK_FORMAT_R8G8B8A8_SNORM,               // R8G8B8A8_SNORM
        VK_FORMAT_R8G8B8A8_SINT,                // R8G8B8A8_SINT

        VK_FORMAT_R16G16_SFLOAT,                // R16G16_SFLOAT
        VK_FORMAT_R16G16_UNORM,                 // R16G16_UNORM
        VK_FORMAT_R16G16_UINT,                  // R16G16_UINT
        VK_FORMAT_R16G16_SNORM,                 // R16G16_SNORM
        VK_FORMAT_R16G16_SINT,                  // R16G16_SINT

        VK_FORMAT_D32_SFLOAT,                   // D32_SFLOAT

        VK_FORMAT_R32_SFLOAT,                   // R32_SFLOAT
        VK_FORMAT_R32_UINT,                     // R32_UINT
        VK_FORMAT_R32_SINT,                     // R32_SINT

        VK_FORMAT_D24_UNORM_S8_UINT,            // D24_UNORM_S8_UINT

        VK_FORMAT_R8G8_UNORM,                   // R8G8_UNORM
        VK_FORMAT_R8G8_UINT,                    // R8G8_UINT
        VK_FORMAT_R8G8_SNORM,                   // R8G8_SNORM
        VK_FORMAT_R8G8_SINT,                    // R8G8_SINT

        VK_FORMAT_D16_UNORM,                    // D16_UNORM

        VK_FORMAT_R16_SFLOAT,                   // R16_SFLOAT
        VK_FORMAT_R16_UNORM,                    // R16_UNORM
        VK_FORMAT_R16_UINT,                     // R16_UINT
        VK_FORMAT_R16_SNORM,                    // R16_SNORM
        VK_FORMAT_R16_SINT,                     // R16_SINT

        VK_FORMAT_R8_UNORM,                     // R8_UNORM
        VK_FORMAT_R8_UINT,                      // R8_UINT
        VK_FORMAT_R8_SNORM,                     // R8_SNORM
        VK_FORMAT_R8_SINT,                      // R8_SINT

        VK_FORMAT_E5B9G9R9_UFLOAT_PACK32,       // R9G9B9E5_UFLOAT

        VK_FORMAT_B8G8R8G8_422_UNORM,           // R8G8B8G8_UNORM
        VK_FORMAT_G8B8G8R8_422_UNORM,           // G8R8G8B8_UNORM

        VK_FORMAT_BC1_RGBA_UNORM_BLOCK,         // BC1_UNORM 
        VK_FORMAT_BC1_RGBA_SRGB_BLOCK,          // BC1_UNORM_SRGB 

        VK_FORMAT_BC2_UNORM_BLOCK,              // BC2_UNORM
        VK_FORMAT_BC2_SRGB_BLOCK,               // BC2_UNORM_SRGB

        VK_FORMAT_BC3_UNORM_BLOCK,              // BC3_UNORM
        VK_FORMAT_BC3_SRGB_BLOCK,               // BC3_UNORM_SRGB

        VK_FORMAT_BC4_UNORM_BLOCK,              // BC4_UNORM
        VK_FORMAT_BC4_SNORM_BLOCK,              // BC4_SNORM

        VK_FORMAT_BC5_UNORM_BLOCK,              // BC5_UNORM 
        VK_FORMAT_BC5_SNORM_BLOCK,              // BC5_SNORM

        VK_FORMAT_R5G6B5_UNORM_PACK16,          // B5G6R5_UNORM

        VK_FORMAT_A1R5G5B5_UNORM_PACK16,        // B5G5R5A1_UNORM

        VK_FORMAT_B8G8R8A8_UNORM,               // B8G8R8A8_UNORM
        VK_FORMAT_B8G8R8A8_SRGB,                // B8G8R8A8_SRGB

        VK_FORMAT_BC6H_UFLOAT_BLOCK,            // BC6H_UFLOAT
        VK_FORMAT_BC6H_SFLOAT_BLOCK,            // BC6H_SFLOAT

        VK_FORMAT_BC7_UNORM_BLOCK,              // BC7_UNORM
        VK_FORMAT_BC7_SRGB_BLOCK,               // BC7_SRGB

        VK_FORMAT_G8_B8R8_2PLANE_420_UNORM,     // NV12
        VK_FORMAT_G8B8G8R8_422_UNORM,           // YUY2

        VK_FORMAT_A4R4G4B4_UNORM_PACK16_EXT,    // B4G4R4A4_UNORM
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VkFormat AsVkFormat(this GraphicsFormat format)
    {
        Assert(AssertionsEnabled && (s_vkFormatMap.Length == Enum.GetValues<GraphicsFormat>().Length));
        return s_vkFormatMap[(uint)format];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowExternalExceptionIfNotSuccess(VkResult value, [CallerArgumentExpression("value")] string? valueExpression = null)
    {
        if ((value != VK_SUCCESS) && (value != VK_SUBOPTIMAL_KHR))
        {
            AssertNotNull(valueExpression);
            ThrowExternalException(valueExpression, (int)value);
        }
    }
}
