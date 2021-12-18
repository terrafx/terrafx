// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the D3D12MA_CurrentBudgetData class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Graphics;

public partial class VulkanGraphicsDevice : GraphicsDevice
{
    private const int MaxMemoryManagerCount = MaxMemoryManagerTypes;

    private const int MaxMemoryManagerTypes = (int)VK_MAX_MEMORY_HEAPS;

    [StructLayout(LayoutKind.Auto)]
    private unsafe struct MemoryBudgetInfo : IDisposable
    {
        public VkPhysicalDeviceMemoryBudgetPropertiesEXT VkPhysicalDeviceMemoryBudgetProperties;

        public ValueReaderWriterLock RWLock;

        public ulong TotalOperationCountAtLastUpdate;

        private fixed ulong _totalFreeMemoryRegionSizeAtLastUpdate[(int)VK_MAX_MEMORY_HEAPS];

        private fixed ulong _totalSizeAtLastUpdate[(int)VK_MAX_MEMORY_HEAPS];

        public MemoryBudgetInfo()
        {
            VkPhysicalDeviceMemoryBudgetProperties = new VkPhysicalDeviceMemoryBudgetPropertiesEXT();
            RWLock = new ValueReaderWriterLock();
            TotalOperationCountAtLastUpdate = 0;

            for (var index = 0; index < MaxMemoryManagerTypes; index++)
            {
                _totalFreeMemoryRegionSizeAtLastUpdate[index] = 0;
                _totalSizeAtLastUpdate[index] = 0;
            }
        }

        public void Dispose() => RWLock.Dispose();

        public ulong GetTotalAllocatedMemoryRegionSizeAtLastUpdate(uint vkMemoryTypeIndex)
        {
            Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));

            var totalSizeAtLastUpdate = _totalSizeAtLastUpdate[vkMemoryTypeIndex];
            var totalFreeMemoryRegionSizeAtLastUpdate = _totalFreeMemoryRegionSizeAtLastUpdate[vkMemoryTypeIndex];

            return totalSizeAtLastUpdate - totalFreeMemoryRegionSizeAtLastUpdate;
        }

        public ulong GetTotalFreeMemoryRegionSizeAtLastUpdate(uint vkMemoryTypeIndex)
        {
            Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
            return _totalFreeMemoryRegionSizeAtLastUpdate[vkMemoryTypeIndex];
        }

        public ulong GetTotalSizeAtLastUpdate(uint vkMemoryTypeIndex)
        {
            Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
            return _totalSizeAtLastUpdate[vkMemoryTypeIndex];
        }

        public void SetTotalFreeMemoryRegionSizeAtLastUpdate(uint vkMemoryTypeIndex, ulong value)
        {
            Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
            _totalFreeMemoryRegionSizeAtLastUpdate[vkMemoryTypeIndex] = value;
        }

        public void SetTotalSizeAtLastUpdate(uint vkMemoryTypeIndex, ulong value)
        {
            Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
            _totalSizeAtLastUpdate[vkMemoryTypeIndex] = value;
        }
    }
}
