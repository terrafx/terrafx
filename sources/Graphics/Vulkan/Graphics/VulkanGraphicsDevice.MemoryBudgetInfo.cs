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

        private fixed ulong _totalFreeMemoryRegionByteLengthAtLastUpdate[(int)VK_MAX_MEMORY_HEAPS];

        private fixed ulong _totalByteLengthAtLastUpdate[(int)VK_MAX_MEMORY_HEAPS];

        public MemoryBudgetInfo()
        {
            VkPhysicalDeviceMemoryBudgetProperties = new VkPhysicalDeviceMemoryBudgetPropertiesEXT();
            RWLock = new ValueReaderWriterLock();
            TotalOperationCountAtLastUpdate = 0;

            for (var index = 0; index < MaxMemoryManagerTypes; index++)
            {
                _totalFreeMemoryRegionByteLengthAtLastUpdate[index] = 0;
                _totalByteLengthAtLastUpdate[index] = 0;
            }
        }

        public void Dispose() => RWLock.Dispose();

        public ulong GetTotalAllocatedMemoryRegionByteLengthAtLastUpdate(uint vkMemoryTypeIndex)
        {
            Assert((vkMemoryTypeIndex < MaxMemoryManagerTypes));

            var totalSizeAtLastUpdate = _totalByteLengthAtLastUpdate[vkMemoryTypeIndex];
            var totalFreeMemoryRegionSizeAtLastUpdate = _totalFreeMemoryRegionByteLengthAtLastUpdate[vkMemoryTypeIndex];

            return totalSizeAtLastUpdate - totalFreeMemoryRegionSizeAtLastUpdate;
        }

        public ulong GetTotalFreeMemoryRegionByteLengthAtLastUpdate(uint vkMemoryTypeIndex)
        {
            Assert((vkMemoryTypeIndex < MaxMemoryManagerTypes));
            return _totalFreeMemoryRegionByteLengthAtLastUpdate[vkMemoryTypeIndex];
        }

        public ulong GetTotalByteLengthAtLastUpdate(uint vkMemoryTypeIndex)
        {
            Assert((vkMemoryTypeIndex < MaxMemoryManagerTypes));
            return _totalByteLengthAtLastUpdate[vkMemoryTypeIndex];
        }

        public void SetTotalFreeMemoryRegionByteLengthAtLastUpdate(uint vkMemoryTypeIndex, ulong value)
        {
            Assert((vkMemoryTypeIndex < MaxMemoryManagerTypes));
            _totalFreeMemoryRegionByteLengthAtLastUpdate[vkMemoryTypeIndex] = value;
        }

        public void SetTotalByteLengthAtLastUpdate(uint vkMemoryTypeIndex, ulong value)
        {
            Assert((vkMemoryTypeIndex < MaxMemoryManagerTypes));
            _totalByteLengthAtLastUpdate[vkMemoryTypeIndex] = value;
        }
    }
}
