// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Interop.DirectX;

namespace TerraFX.Graphics;

public partial class D3D12GraphicsTexture
{
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct CreateInfo
    {
        public GraphicsResourceCpuAccess CpuAccess;

        public UnmanagedArray<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints;

        public ID3D12Resource* D3D12Resource;

        public D3D12_RESOURCE_STATES D3D12ResourceState;

        public GraphicsMemoryRegion MemoryRegion;

        public GraphicsTextureInfo TextureInfo;
    }
}
