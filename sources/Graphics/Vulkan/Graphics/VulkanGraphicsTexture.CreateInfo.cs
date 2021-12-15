// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Interop.Vulkan;

namespace TerraFX.Graphics;

public partial class VulkanGraphicsTexture
{
    [StructLayout(LayoutKind.Auto)]
    internal struct CreateInfo
    {
        public GraphicsMemoryRegion MemoryRegion;

        public GraphicsResourceInfo ResourceInfo;

        public GraphicsTextureInfo TextureInfo;

        public VkImage VkImage;
    }
}
