// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the D3D12MA_CurrentBudgetData class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Graphics;

public partial class GraphicsDevice
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

        private fixed ulong _totalFreeMemoryRegionByteLengthAtLastUpdate[MaxMemoryManagerKinds];

        private fixed ulong _totalByteLengthAtLastUpdate[MaxMemoryManagerKinds];

        public MemoryBudgetInfo()
        {
            DxgiQueryLocalVideoMemoryInfo = new DXGI_QUERY_VIDEO_MEMORY_INFO();
            DxgiQueryNonLocalVideoMemoryInfo = new DXGI_QUERY_VIDEO_MEMORY_INFO();
            RWLock = new ValueReaderWriterLock();
            TotalOperationCountAtLastUpdate = 0;

            for (var index = 0; index < MaxMemoryManagerKinds; index++)
            {
                _totalFreeMemoryRegionByteLengthAtLastUpdate[index] = 0;
                _totalByteLengthAtLastUpdate[index] = 0;
            }
        }

        public readonly void Dispose() => RWLock.Dispose();

        public ulong GetTotalAllocatedMemoryRegionByteLengthAtLastUpdate(int memoryManagerKindIndex)
        {
            Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);

            var totalSizeAtLastUpdate = _totalByteLengthAtLastUpdate[memoryManagerKindIndex];
            var totalFreeMemoryRegionSizeAtLastUpdate = _totalFreeMemoryRegionByteLengthAtLastUpdate[memoryManagerKindIndex];

            return totalSizeAtLastUpdate - totalFreeMemoryRegionSizeAtLastUpdate;
        }

        public ulong GetTotalFreeMemoryRegionByteLengthAtLastUpdate(int memoryManagerKindIndex)
        {
            Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);
            return _totalFreeMemoryRegionByteLengthAtLastUpdate[memoryManagerKindIndex];
        }

        public ulong GetTotalByteLengthAtLastUpdate(int memoryManagerKindIndex)
        {
            Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);
            return _totalByteLengthAtLastUpdate[memoryManagerKindIndex];
        }

        public void SetTotalFreeMemoryRegionByteLengthAtLastUpdate(int memoryManagerKindIndex, ulong value)
        {
            Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);
            _totalFreeMemoryRegionByteLengthAtLastUpdate[memoryManagerKindIndex] = value;
        }

        public void SetTotalSizeAtLastUpdate(int memoryManagerKindIndex, ulong value)
        {
            Assert((uint)memoryManagerKindIndex < MaxMemoryManagerKinds);
            _totalByteLengthAtLastUpdate[memoryManagerKindIndex] = value;
        }
    }
}
