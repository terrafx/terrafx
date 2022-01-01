// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.DirectX;

namespace TerraFX.Graphics.Advanced;

internal struct D3D12GraphicsMemoryHeapCreateOptions
{
    public nuint ByteLength;

    public D3D12_HEAP_FLAGS D3D12HeapFlags;

    public D3D12_HEAP_TYPE D3D12HeapType;
}
