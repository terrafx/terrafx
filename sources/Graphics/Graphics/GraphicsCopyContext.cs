// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing copy commands.</summary>
public sealed unsafe class GraphicsCopyContext : GraphicsContext
{
    internal GraphicsCopyContext(GraphicsCopyCommandQueue copyCommandQueue) : base(copyCommandQueue, GraphicsContextKind.Copy)
    {
    }

    /// <inheritdoc cref="GraphicsCommandQueueObject.CommandQueue" />
    public new GraphicsCopyCommandQueue CommandQueue => base.CommandQueue.As<GraphicsCopyCommandQueue>();

    /// <summary>Copies the contents from a buffer view to a buffer view.</summary>
    /// <param name="destination">The destination buffer view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The size of <paramref name="destination" /> is less than the size of <paramref name="source" />.</exception>
    /// <exception cref="ObjectDisposedException">The copy context has been disposed.</exception>
    public void Copy(GraphicsBufferView destination, GraphicsBufferView source)
    {
        ThrowIfDisposed();

        ThrowIfNull(destination);
        ThrowIfNull(source);

        ThrowIfNotInInsertBounds(source.ByteLength, destination.ByteLength);

        CopyUnsafe(destination, source);
    }

    /// <summary>Copies the contents from a buffer view to a texture view.</summary>
    /// <param name="destination">The destination texture view.</param>
    /// <param name="source">The source buffer view.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The size <paramref name="destination" /> is less than the size of <paramref name="source" />.</exception>
    /// <exception cref="ObjectDisposedException">The copy context has been disposed.</exception>
    public void Copy(GraphicsTextureView destination, GraphicsBufferView source)
    {
        ThrowIfDisposed();

        ThrowIfNull(destination);
        ThrowIfNull(source);

        ThrowIfNotInInsertBounds(source.ByteLength, destination.ByteLength);

        CopyUnsafe(destination, source);
    }

    private void CopyUnsafe(GraphicsBufferView destination, GraphicsBufferView source)
    {
        D3D12GraphicsCommandList->CopyBufferRegion(destination.Resource.D3D12Resource, destination.ByteOffset, source.Resource.D3D12Resource, source.ByteOffset, source.ByteLength);
    }

    private void CopyUnsafe(GraphicsTextureView destination, GraphicsBufferView source)
    {
        var d3d12DestinationTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(destination.Resource.D3D12Resource, Sub: destination.D3D12SubresourceIndex);

        var d3d12SourceTextureCopyLocation = new D3D12_TEXTURE_COPY_LOCATION(source.Resource.D3D12Resource, in destination.D3D12PlacedSubresourceFootprints[0]);
        d3d12SourceTextureCopyLocation.PlacedFootprint.Offset = source.ByteOffset;

        D3D12GraphicsCommandList->CopyTextureRegion(&d3d12DestinationTextureCopyLocation, DstX: 0, DstY: 0, DstZ: 0, &d3d12SourceTextureCopyLocation, pSrcBox: null);
    }
}
