// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the BlockVector class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Reflection;
using TerraFX.Interop;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed class D3D12GraphicsMemoryBlockCollection : GraphicsMemoryBlockCollection
    {
        private readonly D3D12_HEAP_FLAGS _d3d12HeapFlags;
        private readonly D3D12_HEAP_TYPE _d3d12HeapType;

        internal D3D12GraphicsMemoryBlockCollection(D3D12GraphicsMemoryAllocator allocator, D3D12_HEAP_FLAGS d3d12HeapFlags, D3D12_HEAP_TYPE d3d12HeapType)
            : base(allocator)
        {
            _d3d12HeapFlags = d3d12HeapFlags;
            _d3d12HeapType = d3d12HeapType;
        }

        /// <inheritdoc cref="GraphicsMemoryBlockCollection.Allocator" />
        public new D3D12GraphicsMemoryAllocator Allocator => (D3D12GraphicsMemoryAllocator)base.Allocator;

        /// <summary>Gets the heap flags used when creating the <see cref="ID3D12Heap" /> instance for a memory block.</summary>
        public D3D12_HEAP_FLAGS D3D12HeapFlags => _d3d12HeapFlags;

        /// <summary>Gets the heap type used when creating the <see cref="ID3D12Heap" /> instance for a memory block.</summary>
        public D3D12_HEAP_TYPE D3D12HeapType => _d3d12HeapType;

        /// <inheritdoc />
        protected override D3D12GraphicsMemoryBlock CreateBlock(ulong size) => (D3D12GraphicsMemoryBlock)Activator.CreateInstance(
            typeof(D3D12GraphicsMemoryBlock<>).MakeGenericType(Allocator.Settings.RegionCollectionMetadataType!),
            bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance,
            binder: null,
            args: new object[] { this, size },
            culture: null,
            activationAttributes: null
        )!;
    }
}
