// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public unsafe interface ID3D12GraphicsMemoryBlock : IGraphicsMemoryBlock
    {
        /// <summary>Gets the <see cref="ID3D12Heap" /> for the memory block.</summary>
        ID3D12Heap* D3D12Heap { get; }

        /// <inheritdoc cref="IGraphicsMemoryBlock.Collection" />
        new D3D12GraphicsMemoryBlockCollection Collection { get; }
    }
}
