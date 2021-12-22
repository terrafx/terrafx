// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Advanced;
using TerraFX.Interop.Vulkan;

namespace TerraFX.Graphics;

public partial class VulkanGraphicsBuffer
{
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct CreateInfo
    {
        public GraphicsResourceCpuAccess CpuAccess;

        public GraphicsMemoryAllocatorCreateFunc CreateMemoryAllocator;

        public GraphicsBufferKind Kind;

        public GraphicsMemoryRegion MemoryRegion;

        public VkBuffer VkBuffer;
    }
}
