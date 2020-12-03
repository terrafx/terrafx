// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public unsafe interface ID3D12GraphicsResource : IGraphicsResource
    {
        /// <inheritdoc cref="IGraphicsResource.Allocator" />
        new D3D12GraphicsMemoryAllocator Allocator { get; }

        /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the resource.</summary>
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Device.CreateCommittedResource(D3D12_HEAP_PROPERTIES*, D3D12_HEAP_FLAGS, D3D12_RESOURCE_DESC*, D3D12_RESOURCE_STATES, D3D12_CLEAR_VALUE*, Guid*, void**)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        ID3D12Resource* D3D12Resource { get; }

        /// <summary>Gets the default state of the underlying <see cref="ID3D12Resource" /> for the resource.</summary>
        D3D12_RESOURCE_STATES D3D12ResourceState { get; }
    }
}
