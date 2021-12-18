// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the D3D12MA_CurrentBudgetData class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Graphics;

public partial class D3D12GraphicsDevice : GraphicsDevice
{
    private const int MaxMemoryManagerCount = MaxMemoryManagerKinds * 3;

    private const int MaxMemoryManagerKinds = 3;

    [StructLayout(LayoutKind.Auto)]
    private unsafe struct MemoryBudgetInfo : IDisposable
    {
        public DXGI_QUERY_VIDEO_MEMORY_INFO DxgiQueryLocalVideoMemoryInfo;

        public DXGI_QUERY_VIDEO_MEMORY_INFO DxgiQueryNonLocalVideoMemoryInfo;

        public ValueReaderWriterLock RWLock;

        public ulong TotalOperationCountAtLastUpdate;

        private fixed ulong _totalFreeMemoryRegionSizeAtLastUpdate[MaxMemoryManagerKinds];

        private fixed ulong _totalSizeAtLastUpdate[MaxMemoryManagerKinds];

        public MemoryBudgetInfo()
        {
            DxgiQueryLocalVideoMemoryInfo = new DXGI_QUERY_VIDEO_MEMORY_INFO();
            DxgiQueryNonLocalVideoMemoryInfo = new DXGI_QUERY_VIDEO_MEMORY_INFO();
            RWLock = new ValueReaderWriterLock();
            TotalOperationCountAtLastUpdate = 0;

            for (var index = 0; index < MaxMemoryManagerKinds; index++)
            {
                _totalFreeMemoryRegionSizeAtLastUpdate[index] = 0;
                _totalSizeAtLastUpdate[index] = 0;
            }
        }

        public void Dispose() => RWLock.Dispose();

        public ulong GetTotalAllocatedMemoryRegionSizeAtLastUpdate(int memoryManagerKindIndex)
        {
            Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));

            var totalSizeAtLastUpdate = _totalSizeAtLastUpdate[memoryManagerKindIndex];
            var totalFreeMemoryRegionSizeAtLastUpdate = _totalFreeMemoryRegionSizeAtLastUpdate[memoryManagerKindIndex];

            return totalSizeAtLastUpdate - totalFreeMemoryRegionSizeAtLastUpdate;
        }

        public ulong GetTotalFreeMemoryRegionSizeAtLastUpdate(int memoryManagerKindIndex)
        {
            Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));
            return _totalFreeMemoryRegionSizeAtLastUpdate[memoryManagerKindIndex];
        }

        public ulong GetTotalSizeAtLastUpdate(int memoryManagerKindIndex)
        {
            Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));
            return _totalSizeAtLastUpdate[memoryManagerKindIndex];
        }

        public void SetTotalFreeMemoryRegionSizeAtLastUpdate(int memoryManagerKindIndex, ulong value)
        {
            Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));
            _totalFreeMemoryRegionSizeAtLastUpdate[memoryManagerKindIndex] = value;
        }

        public void SetTotalSizeAtLastUpdate(int memoryManagerKindIndex, ulong value)
        {
            Assert(AssertionsEnabled && ((uint)memoryManagerKindIndex < MaxMemoryManagerKinds));
            _totalSizeAtLastUpdate[memoryManagerKindIndex] = value;
        }
    }
}
